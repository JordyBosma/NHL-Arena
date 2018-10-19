using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Commands;

namespace Clients
{
    abstract public class View : IObservable<List<Command>>, IObserver<Command>
    {
        protected WebSocket socket;
        private List<IObserver<List<Command>>> observers = new List<IObserver<List<Command>>>();

        //base constructer
        public View(WebSocket socket)
        {
            this.socket = socket;
        }

        //connection code
        public abstract Task StartReceiving();
        public abstract void SendCommands();
        public virtual void SendCommand()
        {
            //SendMessage(c.ToJson());
        }

        public virtual void OnError()
        {
            socket.Abort();
        }

        public IDisposable Subscribe(IObserver<List<Command>> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber<Command>(observers, observer);
        }

        /// <summary>
        /// send commands to commandmanager
        /// </summary>
        /// <param name="c"></param>
        public void SendCommandsToObservers(List<Command> c)
        {
            for (int i = 0; i < this.observers.Count; i++)
            {
                this.observers[i].OnNext(c);
            }
        }

        internal class Unsubscriber<Command> : IDisposable
        {
            private List<IObserver<List<Command>>> _observers;
            private IObserver<List<Command>> _observer;

            internal Unsubscriber(List<IObserver<List<Command>>> observers, IObserver<List<Command>> observer)
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

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception e)
        {
            socket.Abort();
        }

        /// <summary>
        /// receive commands from commandmanager
        /// </summary>
        /// <param name="value"></param>
        public virtual void OnNext(Command value)
        {
            throw new NotImplementedException();
        }
    }
}
