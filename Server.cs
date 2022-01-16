using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace UdpRule
{
    class Server<T>
    {
        // Events Handler
        public event EventHandler<object> DatagramReceivedEvent;
        public event EventHandler<object> DatagramSendEvent;
        public event EventHandler<object> OnClientConnectEvent;
        public event EventHandler<object> OnClientDisconectEvent;
        public event EventHandler<SocketException> ExceptionEvent;

        private UdpClient client;
        private IPEndPoint ipEendPoint;
        private Dictionary<IPEndPoint, Packet<T>> listClientsInformation;
        private int clientOffsetTimeOutDisconnect = 100000000; // 10 Seconds
        public Server(int port)
        {
            this.client      = new UdpClient(port);
            this.ipEendPoint = new IPEndPoint(IPAddress.Any, port);

            this.listClientsInformation = new Dictionary<IPEndPoint, Packet<T>>();
        }

        ~Server()
        {
            this.client.Close();
            this.client.Dispose();
        }

        public void Start()
        {
            try
            {
                while(true)
                {
                    try
                    {
                        byte[] dataReceived = client.Receive(ref ipEendPoint);

                        this.HandlerData(dataReceived);
                    }
                    catch(SocketException socketException)
                    {
                        ExceptionEvent?.Invoke(this, socketException);
                    }
                }
            }
            catch(SocketException socketException)
            {
                ExceptionEvent?.Invoke(this, socketException);
            }
            finally
            {
                this.client.Close();
                this.client.Dispose();
            }
        }

        private void HandlerData(byte[] data)
        {
            Packet<T> packet = new Packet<T>(data);

            packet.UpdateTimeComunication(this.GetUnixTimeNow);

            DatagramReceivedEvent?.Invoke(this, new Datagram<T>(this.ipEendPoint, packet));

            if(!this.listClientsInformation.ContainsKey(this.ipEendPoint))
            {
                this.listClientsInformation.Add(this.ipEendPoint, packet);
                
                OnClientConnectEvent?.Invoke(this, packet.PacketDeserialize());
            }
            else
            {
                this.listClientsInformation[this.ipEendPoint] = packet;
            }

            this.SendData(this.ipEendPoint);
        }

        private void SendData(IPEndPoint ipEendPoint)
        {                        
            try
            {
                List<T> listCLients = new List<T>();

                foreach(KeyValuePair<IPEndPoint, Packet<T>> clientInfo in this.listClientsInformation)
                {
                    if(clientInfo.Value.LastTimeComunication() + this.clientOffsetTimeOutDisconnect < this.GetUnixTimeNow)
                    {
                        this.listClientsInformation.Remove(clientInfo.Key);

                        OnClientDisconectEvent?.Invoke(this, clientInfo.Value);
                    }
                    else
                    {
                        listCLients.Add(clientInfo.Value.PacketDeserialize());
                    }
                }
                
                Packet<T> packet = new Packet<T>();
                
                packet.AddList(listCLients);

                packet.PacketSerialize();

                this.client.Send(packet.Buffer, packet.Buffer.Length, ipEendPoint);

                DatagramSendEvent?.Invoke(this, packet);
            }
            catch(SocketException socketException)
            {
                ExceptionEvent?.Invoke(this, socketException);
            }
        }

        private long GetUnixTimeNow
        {
            get 
            {
                return DateTime.Now.Ticks;
            }
        }
    }
}
