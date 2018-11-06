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
        private PlayerSpawnLocationList playerSpawnList;

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

        /// <summary>
        /// deletes a player on all other clients when said player loses connection to the server
        /// </summary>
        /// <param name="cmd">the command containing the details about the player that needs to be deleted</param>
        public void PlayerDisconnectHandler(DeleteObjectCommand cmd)
        {
            game.getWorldObjects().Remove(cmd.obj);
            Player p = (Player)cmd.obj;
            Unsubscriber<Command> unsubscriber = new Unsubscriber<Command>(observers, p.GetClient());
            unsubscriber.Dispose();
            if (game.GetPlayerCount() == 0)
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

                    if (obj.guid == hit.hitPlayerGuid)
                    {
                        hitPlayer = (Player)obj;
                    }
                    else if (obj.guid == hit.shootingPlayerGuid)
                    {
                        shootingPlayer = (Player)obj;
                    }
                }
            }

            if (hitPlayer.DoDamage(hit.damage))
            {
                shootingPlayer.addKill();
                PlayerSpawnLocation respawnLocation = playerSpawnList.GetSpawnLocation();
                DeathCommand cmd = new DeathCommand(hitPlayer, respawnLocation);
                UpdatePlayerStatsCommand cmd2 = new UpdatePlayerStatsCommand(shootingPlayer);
                SendCommandsToObservers(cmd);
                SendCommandsToObservers(cmd2);
            }
            else
            {
                UpdatePlayerStatsCommand cmd = new UpdatePlayerStatsCommand(hitPlayer);
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
            List<Object3D> deleteCueue = new List<Object3D>();

            foreach (Object3D obj in worldObjects)
            {
                if (obj is Player)
                {
                    if (obj.guid == uPlayer.playerGuid)
                    {
                        if (uPlayer.y > -100)
                        {
                            obj.Move(uPlayer.x, uPlayer.y, uPlayer.z);
                            obj.Rotate(uPlayer.rotationX, uPlayer.rotationY, uPlayer.rotationZ);
                            deleteCueue = CheckForPickup(uPlayer, obj);
                            UpdateObjectCommand cmd = new UpdateObjectCommand(obj);
                            SendCommandsToObservers(cmd);
                        }
                        else
                        {
                            PlayerSpawnLocation respawnLocation = playerSpawnList.GetSpawnLocation();
                            DeathCommand cmd = new DeathCommand((Player)obj, respawnLocation);
                            ((Player)obj).addDeath();
                            UpdatePlayerStatsCommand cmd2 = new UpdatePlayerStatsCommand((Player)obj);
                            SendCommandsToObservers(cmd);
                            SendCommandsToObservers(cmd2);
                        }
                    }
                }
            }

            foreach (Object3D obj in deleteCueue)
            {
                game.getWorldObjects().Remove(obj);
            }
        }

        /// <summary>
        /// checks if a player is currently on a pickup and if so sends the player a pickupcommand with the items details of the item he picked up
        /// </summary>
        /// <param name="uPlayer">the current position update received from the client</param>
        /// <param name="obj">the previous position of the player</param>
        /// <returns></returns>
        public List<Object3D> CheckForPickup(UpdatePlayerCommand uPlayer, Object3D obj)
        {
            List<Object3D> deleteCueue = new List<Object3D>();

            foreach (SpawnLocation s in spawnList)
            {
                if (s.item != null)
                {
                    if (s.item.type != "DamageBoost")
                    {
                        if (uPlayer.x > (s.item.x - 0.6) && uPlayer.x < (s.item.x + 0.6) && uPlayer.y > (s.item.y + 1.4) && uPlayer.y < (s.item.y + 2.6) && uPlayer.z > (s.item.z - 0.6) && uPlayer.z < (s.item.z + 0.6))
                        {
                            if (s.item.type == "HealthItem")
                            {
                                ((Player)obj).addHealth(((HealthItem)s.item).itemValue);
                                UpdatePlayerStatsCommand cmd = new UpdatePlayerStatsCommand((Player)obj);
                                SendCommandsToObservers(cmd);
                            }
                            if (s.item.type == "ArmourItem")
                            {
                                ((Player)obj).addArmour(((ArmourItem)s.item).itemValue);
                                UpdatePlayerStatsCommand cmd = new UpdatePlayerStatsCommand((Player)obj);
                                SendCommandsToObservers(cmd);
                            }
                            if (s.item.type == "SpeedBoost")
                            {
                                PlayerPickupCommand cmd = new PlayerPickupCommand(s.item, ((Player)obj).guid);
                                SendCommandsToObservers(cmd);
                            }
                            if (s.item.type == "AmmoItem")
                            {
                                PlayerPickupCommand cmd = new PlayerPickupCommand(s.item, ((Player)obj).guid);
                                SendCommandsToObservers(cmd);
                            }

                            DeleteObjectCommand cmd1 = new DeleteObjectCommand(s.item);
                            SendCommandsToObservers(cmd1);
                            deleteCueue.Add(s.item);
                            s.dellItem();
                        }
                    }
                    else
                    {
                        if (uPlayer.x > (s.item.x - 1.45) && uPlayer.x < (s.item.x + 1.45) && uPlayer.y > (s.item.y + 0.75) && uPlayer.y < (s.item.y + 3.25) && uPlayer.z > (s.item.z - 1.45) && uPlayer.z < (s.item.z + 1.45))
                        {
                            DeleteObjectCommand cmd3 = new DeleteObjectCommand(s.item);
                            PlayerPickupCommand cmd4 = new PlayerPickupCommand(s.item, ((Player)obj).guid);
                            SendCommandsToObservers(cmd3);
                            SendCommandsToObservers(cmd4);
                            deleteCueue.Add(s.item);
                            s.dellItem();
                        }
                    }
                }
            }
            return deleteCueue;
        }

        /// <summary>
        /// initialize the first player
        /// </summary>
        /// <param name="newPlayer"></param>
        public void InitializePlayer(Player newPlayer)
        {
            //new player krijgt zn guid en game guid
            InitializePlayerCommand cmd = new InitializePlayerCommand(newPlayer.guid, game.gameId ,newPlayer.x, newPlayer.y,newPlayer.z);
            observers[observers.Count - 1].OnNext(cmd);

            //alle andere spelers krijgen die nieuwe speler
            NewObjectCommand cmd2 = new NewObjectCommand(newPlayer);
            SendCommandsToObservers(cmd2);

            //de nieuwe speler krijgt alle worldobjects
            List<Object3D> worldObjects = game.getWorldObjects();
            foreach (Object3D obj in worldObjects)
            {
                NewObjectCommand cmd3 = new NewObjectCommand(obj);
                observers[observers.Count() - 1].OnNext(cmd3);
            }
        }

        /// <summary>
        /// initializes the list of spawn locations for players and items
        /// </summary>
        /// <param name="spawnList">the list with location for itemspawns</param>
        /// <param name="playerSpawnList">the list with locations for playerspawns</param>
        public void InitializeSpawnList(List<SpawnLocation> spawnList, PlayerSpawnLocationList playerSpawnList)
        {
            this.spawnList = spawnList;
            this.playerSpawnList = playerSpawnList;
        }

        public void InitializeItem(Item newItem)
        {
            NewObjectCommand newDBoost = new NewObjectCommand(newItem);
            game.getWorldObjects().Add(newItem);
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

        /// <summary>
        /// sends commands used to update the gametimer in the top of the screen
        /// </summary>
        /// <param name="cmd"></param>
        public void SendGameTimeLeftCommand(GameTimeLeftCommand cmd)
        {
            SendCommandsToObservers(cmd);
        }


        /// <summary>
        /// sends a command that triggers the end game display on all clients
        /// </summary>
        /// <param name="cmd"></param>
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
