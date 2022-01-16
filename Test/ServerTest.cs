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
            server.DatagramReceivedEvent    += OnServerDatagramReceivedEvent;
            server.DatagramSendEvent        += OnServerDatagramSendEvent;
            server.OnClientConnectEvent     += OnServerOnClientConnectEvent;
            server.OnClientDisconectEvent   += OnServerOnClientDisconectEvent;
            server.ExceptionEvent           += OnServerExceptionEvent;

            ConsoleDebug.WriteLine("Server sarted!");

            server.Start();
        }
       
        private void OnServerDatagramReceivedEvent(object sender, object data)
        {      
            Datagram<GameObject> datagram = (Datagram<GameObject>) data;

            string dataEncodind = Encoding.ASCII.GetString(datagram.Packet.Buffer);

            ConsoleDebug.WriteLine($"- Received from Address({datagram.IpEndPoint.Address}) Port({datagram.IpEndPoint.Port})");
            ConsoleDebug.WriteLine($"- Data: {dataEncodind}", ConsoleDebug.DebugColor.Yellow);
        } 
        private void OnServerDatagramSendEvent(object sender, object data)
        {      
            Packet<GameObject> packet = (Packet<GameObject>) data;

            string dataEncodind = Encoding.ASCII.GetString(packet.Buffer);

            ConsoleDebug.WriteLine($"- Send to Client");
            ConsoleDebug.WriteLine($"- Data: {dataEncodind}", ConsoleDebug.DebugColor.Cyan);
        }       
        private void OnServerExceptionEvent(object sender, SocketException socketException)
        {
            ConsoleDebug.WriteLine("Server Socket Exception:");
            ConsoleDebug.WriteLine(socketException.Message, ConsoleDebug.DebugColor.Red);
        }
        private void OnServerOnClientConnectEvent(object sender, object data)
        {
            GameObject gameObject = (GameObject) data;

            ConsoleDebug.WriteLine($"{gameObject.Player.Name} is Added!", ConsoleDebug.DebugColor.Yellow);
        }
        private void OnServerOnClientDisconectEvent(object sender, object data)
        {
            Packet<GameObject> gameObject = (Packet<GameObject>) data;

            ConsoleDebug.WriteLine($"{gameObject.PacketDeserialize().Player.Name} as disconected!", ConsoleDebug.DebugColor.Red);
        }
    }
}
