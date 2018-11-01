using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLogic;
using ItemLogic;
using WorldObjects;

namespace Commands
{
    public class CommandManager : IObserver<List<Command>>, IObservable<Command>
    {
        private Game game;
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        private List<SpawnLocation> spawnList;

        public CommandManager(Game game)
        {
            this.game = game;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// receive commands from clients
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(List<Command> value)
        {
            if (value != null)
            {
                foreach (Command c in value)
                {
                    string commandType = c.commandType;

                    switch (commandType)
                    {
                        case "HitCommand":
                            HitCommand hit = (HitCommand)c;
                            HitHandler(hit);
                            break;
                        case "UpdatePlayerCommand":
                            UpdatePlayerCommand uPlayerCmd = (UpdatePlayerCommand)c;
                            PlayerUpdateHandler(uPlayerCmd);
                            break;
                        case "DeleteObjectCommand":
                            DeleteObjectCommand deleteObjectCmd = (DeleteObjectCommand)c;
                            PlayerDisconnectHandler(deleteObjectCmd);
                            break;
                        case "FireCommand":
                            SendCommandsToObservers(c);
                            break;
                    }
                }
            }
        }

        public void PlayerDisconnectHandler(DeleteObjectCommand cmd)
        {
            game.getWorldObjects().Remove(cmd.obj);
            Player p = (Player)cmd.obj;
            Unsubscriber<Command> unsubscriber = new Unsubscriber<Command>(observers, p.GetClient());
            unsubscriber.Dispose();
            if(game.GetPlayerCount() == 0)
            {
                game.Dispose();
            }
            SendCommandsToObservers(cmd);
        }

        /// <summary>
        /// handlees hit messages from clients and determines if one scored a kill or died
        /// </summary>
        /// <param name="hit">a hitcommand containing detail about a hit</param>
        public void HitHandler(HitCommand hit)
        {
            List<Object3D> worldObjects = game.getWorldObjects();
            Player hitPlayer = null;
            Player shootingPlayer = null;

            foreach (Object3D obj in worldObjects)
            {
                if (obj is Player)
                {
                    if (obj.guid == hit.hitPlayerGuid || obj.guid == hit.hitPlayerGuid)
                    {
                        if (obj.guid == hit.hitPlayerGuid)
                        {
                            hitPlayer = (Player)obj;
                        }
                        else
                        {
                            shootingPlayer = (Player)obj;
                        }
                    }
                }
            }

            if (hitPlayer.DoDamage(hit.damage))
            {
                shootingPlayer.addKill();
                DeathCommand cmd = new DeathCommand(hitPlayer);
                UpdatePlayerStatsCommand cmd2 = new UpdatePlayerStatsCommand(shootingPlayer);
                SendCommandsToObservers(cmd);
                SendCommandsToObservers(cmd2);
            }
            else
            {
                UpdatePlayerStatsCommand cmd = new UpdatePlayerStatsCommand(shootingPlayer);
                SendCommandsToObservers(cmd);
            }
        }

        /// <summary>
        /// handle updateplayer commands from clients
        /// </summary>
        /// <param name="uPlayer">a command message contiaing details about hte player/client</param>
        public void PlayerUpdateHandler(UpdatePlayerCommand uPlayer)
        {
            List<Object3D> worldObjects = game.getWorldObjects();

            foreach (Object3D obj in worldObjects)
            {
                if (obj is Player)
                {
                    if (obj.guid == uPlayer.playerGuid)
                    {
                        obj.Move(uPlayer.x, uPlayer.y, uPlayer.z);
                        obj.Rotate(uPlayer.rotationX, uPlayer.rotationY, uPlayer.rotationZ);
                        UpdateObjectCommand cmd = new UpdateObjectCommand(obj);
                        SendCommandsToObservers(cmd);

                        foreach (SpawnLocation s in spawnList)
                        {
                            if (s.item != null)
                            {
                                if (uPlayer.x > (s.item.x - 0.4) && uPlayer.x < (s.item.x + 0.4) && uPlayer.y > (s.item.y + 1.6) && uPlayer.y < (s.item.y + 2.4) && uPlayer.z > (s.item.z - 0.4) && uPlayer.z < (s.item.z + 0.4))
                                {
                                    DeleteObjectCommand cmd2 = new DeleteObjectCommand(s.item);
                                    SendCommandsToObservers(cmd2);
                                    s.dellItem();
                                    // player ammo pickup command!!!!
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// initialize the first player
        /// </summary>
        /// <param name="newPlayer"></param>
        public void InitializePlayer(Player newPlayer)
        {
            InitializePlayerCommand cmd = new InitializePlayerCommand(newPlayer.guid, game.gameId);
            observers[observers.Count - 1].OnNext(cmd);

            NewObjectCommand cmd2 = new NewObjectCommand(newPlayer);
            SendCommandsToObservers(cmd2);

            List<Object3D> worldObjects = game.getWorldObjects();
            foreach (Object3D obj in worldObjects)
            {
                NewObjectCommand cmd3 = new NewObjectCommand(obj);
                observers[observers.Count() - 1].OnNext(cmd3);
            }
        }

        public void InitializeSpawnList(List<SpawnLocation> spawnList)
        {
            this.spawnList = spawnList;
        }

        public void InitializeItem(Item newItem)
        {
            NewObjectCommand newDBoost = new NewObjectCommand(newItem);
            SendCommandsToObservers(newDBoost);
        }

        /// <summary>
        /// disconnect all of the clients in current game
        /// </summary>
        public void DisconnectAllClients()
        {
            DisconnectCommand cmd = new DisconnectCommand();
            SendCommandsToObservers(cmd);
            SendCommandQueue();
        }

        public IDisposable Subscribe(IObserver<Command> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber<Command>(observers, observer);
        }

        public void SendGameTimeLeftCommand(GameTimeLeftCommand cmd)
        {
            SendCommandsToObservers(cmd);
        }
        
        public void SendGameEndingCommand(GameEndingCommand cmd)
        {
            SendCommandsToObservers(cmd);
        }

        /// <summary>
        /// send commands to clients
        /// </summary>
        /// <param name="c"></param>
        private void SendCommandsToObservers(Command c)
        {
            for (int i = 0; i < this.observers.Count; i++)
            {
                this.observers[i].OnNext(c);
            }
        }

        /// <summary>
        /// send triggermsg sending commands to all clients
        /// </summary>
        public void SendCommandQueue()
        {
            SendCommand send = new SendCommand();
            SendCommandsToObservers(send);
        }

        internal class Unsubscriber<Command> : IDisposable
        {
            private List<IObserver<Command>> _observers;
            private IObserver<Command> _observer;

            internal Unsubscriber(List<IObserver<Command>> observers, IObserver<Command> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
