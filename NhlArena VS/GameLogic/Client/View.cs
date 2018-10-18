using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Commands;

namespace Clients
{
    abstract public class View
    {

        protected WebSocket socket;

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

        //IObserver Implementation:
        public virtual void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public virtual void OnError()
        {
            socket.Abort();
        }

        public virtual void OnNext()
        {
            //SendCommand(value);
        }
    }
}
