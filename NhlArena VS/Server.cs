using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.WebSockets;
using System.Security.Cryptography;

namespace NhlArena_VS
{
    public class Server
    {
        public static class ConnectorSocket
        {
            static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            public static void StartSocket()
            {
                //Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, 11000));
                serverSocket.Listen(128);
                serverSocket.BeginAccept(null, 0, OnAccept, null);
            }

            private static void OnAccept(IAsyncResult result)
            {
                try
                {
                    Socket client = null;
                    if (serverSocket != null && serverSocket.IsBound)
                    {
                        client = serverSocket.EndAccept(result);
                    }
                    if (client != null)
                    {
                        HandShakeClient(client);
                        byte[] messagebuffer = new byte[1024 * 4];
                        client.Receive(messagebuffer);
                        string message = Encoding.UTF8.GetString(messagebuffer);
                        Console.Write(message);
                    }
                }
                catch (SocketException exception)
                {
                }
                finally
                {
                    if (serverSocket != null && serverSocket.IsBound)
                    {
                        serverSocket.BeginAccept(null, 0, OnAccept, null);
                    }
                }
            }

            private static void OnReceive(IAsyncResult ar)
            {
                throw new NotImplementedException();
            }

            private static void HandShakeClient(Socket client)
            {
                byte[] buffer = new byte[1024 * 4];
                client.Receive(buffer);
                string data = Encoding.ASCII.GetString(buffer);

                if (new System.Text.RegularExpressions.Regex("^GET").IsMatch(data))
                {
                    const string eol = "\r\n"; // HTTP/1.1 defines the sequence CR LF as the end-of-line marker

                    Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + eol
                        + "Connection: Upgrade" + eol
                        + "Upgrade: websocket" + eol
                        + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                            System.Security.Cryptography.SHA1.Create().ComputeHash(
                                Encoding.UTF8.GetBytes(
                                    new System.Text.RegularExpressions.Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                )
                            )
                        ) + eol
                        + eol);

                    client.Send(response);
                }

                //Console.Write(message);
            }

            //static private string guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            //private static string AcceptKey(ref string key)
            //{
            //    string longKey = key + guid;
            //    SHA1 sha1 = SHA1CryptoServiceProvider.Create();
            //    byte[] hashBytes = sha1.ComputeHash(System.Text.Encoding.ASCII.GetBytes(longKey));
            //    return Convert.ToBase64String(hashBytes);
            //}
        }
    }
}
