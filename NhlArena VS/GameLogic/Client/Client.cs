using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Text;
using Commands;

namespace Clients
{
    public class Client : View
    {
        public Guid gameId { get; }
        public string username { get; }
        public ClientSendManager sendManager { get; }

        public Client(WebSocket socket, string username, Guid gameId) : base(socket)
        {
            this.gameId = gameId;
            this.username = username;
            sendManager = new ClientSendManager();
        }

        public Client(WebSocket socket, string username) : base(socket)
        {
            this.username = username;
        }

        public override async Task StartReceiving() {
            var buffer = new byte[1024 * 4];

            Console.WriteLine("ClientView connection started");

            WebSocketReceiveResult result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                //string yeetmessage = Encoding.UTF8.GetString(buffer);
                //System.Diagnostics.Debug.WriteLine("Received the following information from client: " + yeetmessage );

                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);


            }

            Console.WriteLine("ClientView has disconnected");

            await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
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
    }
}