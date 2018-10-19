using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLogic;

namespace Commands
{
    public class CommandManager : IObserver<List<Command>> , IObservable<Command>
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
            throw new NotImplementedException();
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
        public void SendCommandsToObservers(Command c)
        {
            for (int i = 0; i < this.observers.Count; i++)
            {
                this.observers[i].OnNext(c);
            }
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
