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
            server.DataReceivedEvent    += OnDataReceivedEvent;
            server.DataSendEvent        += OnDataSendEvent;
            server.ClientConnectEvent   += OnClientConnectEvent;
            server.ClientDisconectEvent += OnClientDisconectEvent;
            server.ExceptionEvent       += OnExceptionEvent;

            ConsoleDebug.WriteLine("- Server sarted!", ConsoleDebug.DebugColor.Green);

            server.Start();
        }
       
        private void OnDataReceivedEvent(object sender, object data)
        {      
            Datagram<GameObject> datagram = (Datagram<GameObject>) data;

            string dataEncodind = Encoding.ASCII.GetString(datagram.Packet.Buffer);

            ConsoleDebug.WriteLine($"- Received from Address({datagram.IpEndPoint.Address}) Port({datagram.IpEndPoint.Port})");
            ConsoleDebug.WriteLine($"- Data: {dataEncodind}", ConsoleDebug.DebugColor.Yellow);
        } 
        private void OnDataSendEvent(object sender, object data)
        {      
            Packet<GameObject> packet = (Packet<GameObject>) data;

            string dataEncodind = Encoding.ASCII.GetString(packet.Buffer);

            ConsoleDebug.WriteLine($"- Send to Client");
            ConsoleDebug.WriteLine($"- Data: {dataEncodind}", ConsoleDebug.DebugColor.Cyan);
        }   
        private void OnClientConnectEvent(object sender, object data)
        {
            GameObject gameObject = (GameObject) data;

            ConsoleDebug.WriteLine($"{gameObject.Player.Name} is Added!", ConsoleDebug.DebugColor.Green);
        }
        private void OnClientDisconectEvent(object sender, object data)
        {
            Packet<GameObject> gameObject = (Packet<GameObject>) data;

            ConsoleDebug.WriteLine($"{gameObject.PacketDeserialize().Player.Name} as disconected!", ConsoleDebug.DebugColor.Red);
        }
            
        private void OnExceptionEvent(object sender, Exception exception)
        {
            ConsoleDebug.WriteLine($"- Exception: {exception.Message}", ConsoleDebug.DebugColor.Red);
        }
    }
}
