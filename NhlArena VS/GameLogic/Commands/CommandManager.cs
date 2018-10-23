using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLogic;
using WorldObjects;

namespace Commands
{
    public class CommandManager : IObserver<List<Command>>, IObservable<Command>
    {
        private Game game;
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();

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
                        UpdatePlayerCommand uPlayer = (UpdatePlayerCommand)c;
                        PlayerUpdateHandler(uPlayer);
                        break;
                }
            }
        }

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
                        obj.Rotate(0, uPlayer.rotationY, 0);
                        //checkPickUp
                        UpdateObjectCommand cmd = new UpdateObjectCommand(obj);
                        SendCommandsToObservers(cmd);
                    }
                }
            }
        }

        public void InitializePlayer(Player newPlayer)
        {
            NewObjectCommand cmd = new NewObjectCommand(newPlayer);
            SendCommandsToObservers(cmd);

            List<Object3D> worldObjects = game.getWorldObjects();
            foreach(Object3D obj in worldObjects)
            {
                NewObjectCommand cmd2 = new NewObjectCommand(obj);
                observers[observers.Count() - 1].OnNext(cmd2);
            }
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
