using System;
using System.Net.Sockets;
using System.Text;

using UdpRule.Test.Objects;

namespace UdpRule.Test
{
    class ClientTest
    {
        static bool clientConnectedToServer = true;
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

            Console.WriteLine("Press (Enter) to start send");

            do 
            {
                Console.ReadKey();

                client.SendData(gameObject);
            } 
            while(clientConnectedToServer);
        }

        private void OnClientDatagramReceivedEvent(object sender, object data)
        {
            Packet<ClientPacket<GameObject>> packet = (Packet<ClientPacket<GameObject>>) data;

            string dataEncodind = Encoding.ASCII.GetString(packet.Buffer);

            Console.WriteLine($"- Received from server:");
            Console.WriteLine($"- Data: {dataEncodind}");
        }
        private void OnClientDatagramSendEvent(object sender, object data)
        {
            Packet<GameObject> packet = (Packet<GameObject>) data;

            string dataEncodind = Encoding.ASCII.GetString(packet.Buffer);

            Console.WriteLine($"Sent data to server.");
        }
        private void OnClientServerDisconnectEvent(object sender, EventArgs e)
        {
            clientConnectedToServer = false;

            Console.WriteLine("Server is down!");
        }
        private void OnClientExceptionEvent(object sender, SocketException socketException)
        {
            Console.WriteLine($"Server Socket Exception({socketException.ErrorCode}):");
            Console.WriteLine(socketException.Message);
        }
    }
}
