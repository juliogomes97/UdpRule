using System;
using System.Net.Sockets;
using System.Text;

using UdpRule.Test.Objects;

namespace UdpRule.Test
{
    public class ServerTest
    {
        public ServerTest()
        {            
            StartServer();
        }

        private void StartServer()
        {
            Server<GameObject> server = new Server<GameObject>(27000);

            // Handlers Events
            server.DatagramReceivedEvent += OnServerDatagramReceivedEvent;
            server.OnClientConnectEvent += OnServerOnClientConnectEvent;
            server.OnClientDisconectEvent += OnServerOnClientDisconectEvent;
            server.ExceptionEvent += OnServerExceptionEvent;

            Console.WriteLine("Server sarted!");

            server.Start();
        }
       
        private void OnServerDatagramReceivedEvent(object sender, object data)
        {      
            Datagram<GameObject> datagram = (Datagram<GameObject>) data;

            string dataEncodind = Encoding.ASCII.GetString(datagram.Packet.Buffer);

            Console.WriteLine($"- Received from Address({datagram.IpEndPoint.Address}) Port({datagram.IpEndPoint.Port})");
            Console.WriteLine($"Data: {dataEncodind}");
        }        
        private void OnServerExceptionEvent(object sender, SocketException socketException)
        {
            Console.WriteLine("Server Socket Exception:");
            Console.WriteLine(socketException.Message);
        }
        private void OnServerOnClientConnectEvent(object sender, object data)
        {
            GameObject gameObject = (GameObject) data;

            Console.WriteLine($"{gameObject.Player.Name} is Added!");
        }
        private void OnServerOnClientDisconectEvent(object sender, object data)
        {
            Packet<GameObject> gameObject = (Packet<GameObject>) data;

            Console.WriteLine($"{gameObject.PacketDeserialize().Player.Name} as disconected!");
        }
    }
}
