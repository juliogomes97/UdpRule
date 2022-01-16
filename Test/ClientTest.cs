using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using UdpRule.Test.Objects;

namespace UdpRule.Test
{
    class ClientTest
    {
        static bool clientConnectedToServer = true;
        static bool start = false;
        public ClientTest()
        {            
            StartClient();
        }

        private void StartClient()
        {
            Console.Write("Player name: ");
            
            string name = Console.ReadLine();

            Player player = new Player(name);

            GameObject gameObject = new GameObject(player, new Vector3(1, 2, 3));

            Client<GameObject> client = new Client<GameObject>();
            
            // Handlers Events
            client.DatagramReceivedEvent += OnClientDatagramReceivedEvent;
            client.DatagramSendEvent += OnClientDatagramSendEvent;
            client.ServerDisconnectedEvent += OnClientServerDisconnectEvent;
            client.ExceptionEvent += OnClientExceptionEvent;

            client.Connect("127.0.0.1", 27000);

            ConsoleDebug.WriteLine("Press (Enter) to start send");

            Random r = new Random();

            do 
            {
                Console.ReadKey();

                Vector3 randomPosition = new Vector3(r.Next(1, 9999) , r.Next(1, 9999), r.Next(1, 9999));

                gameObject.SetPositon(randomPosition);

                client.SendData(gameObject);
            } 
            while(clientConnectedToServer);
        }

        private void OnClientDatagramReceivedEvent(object sender, object data)
        {
            Packet<GameObject> packet = (Packet<GameObject>) data;

            string dataEncodind = Encoding.Default.GetString(packet.Buffer, 0 , packet.Buffer.Length);

            ConsoleDebug.WriteLine($"- Received from server:");
            ConsoleDebug.WriteLine($"- Data: {dataEncodind}", ConsoleDebug.DebugColor.Yellow);

            List<GameObject> listClients = packet.PacketDeserializeList();

            foreach(GameObject gameObject in listClients)
            {
                ConsoleDebug.WriteLine($"{gameObject.Player.Name} have position: X({gameObject.Position.X}) Y({gameObject.Position.Y}) Z({gameObject.Position.Z})", ConsoleDebug.DebugColor.Cyan);
            }
        }
        private void OnClientDatagramSendEvent(object sender, object data)
        {
            Packet<GameObject> packet = (Packet<GameObject>) data;

            string dataEncodind = Encoding.ASCII.GetString(packet.Buffer);

            ConsoleDebug.WriteLine($"Sent data to server.", ConsoleDebug.DebugColor.Cyan);
        }
        private void OnClientServerDisconnectEvent(object sender, EventArgs e)
        {
            clientConnectedToServer = false;

            ConsoleDebug.WriteLine("Server is down!", ConsoleDebug.DebugColor.Red);
        }
        private void OnClientExceptionEvent(object sender, SocketException socketException)
        {
            ConsoleDebug.WriteLine($"Server Socket Exception({socketException.ErrorCode}):");
            ConsoleDebug.WriteLine(socketException.Message, ConsoleDebug.DebugColor.Red);
        }
    }
}
