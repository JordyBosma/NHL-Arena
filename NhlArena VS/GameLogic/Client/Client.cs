using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Text;
using Commands;
using WorldObjects;

namespace Clients
{
    public class Client : View, IObserver<Command>
    {
        public Guid gameId;
        private Player player;
        public string username { get; }
        private ClientSendManager sendManager { get; }
        private ClientReceiveManager receiveManager { get; }

        public Client(WebSocket socket, string username) : base(socket)
        {
            this.username = username;
            sendManager = new ClientSendManager();
            receiveManager = new ClientReceiveManager();
        }

        public Client(WebSocket socket, string username, Guid gameId) : base(socket)
        {
            this.gameId = gameId;
            this.username = username;
            sendManager = new ClientSendManager();
            receiveManager = new ClientReceiveManager();
        }

        public void SetGameId(Guid gameId)
        {
            this.gameId = gameId;
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public override async Task StartReceiving()
        {
            var buffer = new byte[1024 * 4];

            Console.WriteLine("ClientView connection started");

            WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                //string yeetmessage = Encoding.UTF8.GetString(buffer);
                //System.Diagnostics.Debug.WriteLine("Received the following information from client: " + yeetmessage );
                WebSocketState state = socket.State;
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                SendCommandsToObservers(receiveManager.ReceiveString(Encoding.UTF8.GetString(buffer)));
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = 0;
                }
            }

            if (socket.State != WebSocketState.Open)
            {
                List<Command> cmdList = new List<Command>();
                cmdList.Add(new DeleteObjectCommand(player));

                SendCommandsToObservers(cmdList);
            }

            Console.WriteLine("ClientView has disconnected");

            await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            socket.Dispose();
        }

        public override async void SendCommands()
        {
            string message = sendManager.GetCommandsForSending();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            try
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer, 0, message.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while sending information to client, probably a Socket disconnect");
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// receive commands from commandmanager
        /// </summary>
        /// <param name="value"></param>
        public override async void OnNext(Command value)
        {
            if (value is SendCommand)
            {
                SendCommands();
            }
            else if (value is DisconnectCommand)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket was disconnected by commandmanager", CancellationToken.None);
                socket.Dispose();
            }
            else
            {
                sendManager.AddCommand(value);
            }
        }

    }
}