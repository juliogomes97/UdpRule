using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpRule
{
    class Client<T>
    {
        // Events Handler
        public event EventHandler<object> DatagramSendEvent;
        public event EventHandler<object> DatagramReceivedEvent;
        public event EventHandler ServerDisconnectedEvent;
        public event EventHandler<SocketException> ExceptionEvent;
        
        private Socket socket;
        private EndPoint endPointFrom;
        private Packet<T> packetSend;
        private Packet<ClientPacket<T>> packetReceive;
        public Client()
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        ~Client()
        {
            this.socket.Close();
            this.socket.Dispose();
        }

        public void Connect(string ip, int port)
        {
            IPEndPoint iPEndPoint   = new IPEndPoint(IPAddress.Parse(ip), port);
            
            this.endPointFrom   = iPEndPoint;
            this.packetSend     = new Packet<T>();
            this.packetReceive  = new Packet<ClientPacket<T>>();

            this.socket.Connect(this.endPointFrom);

            if(this.socket.Connected)
            {
                this.Receive();
            }
            else ServerDisconnectedEvent?.Invoke(this, null);
        }

        public void SendData(T data)
        {
            if(this.socket.Connected)
            {
                this.packetSend.PacketSerialize(data);

                this.socket.BeginSend(this.packetSend.Buffer, 0, this.packetSend.Buffer.Length, SocketFlags.None, BeginSendCallback, this.packetSend);
            }
            else ServerDisconnectedEvent?.Invoke(this, null);
        }

        private void BeginSendCallback(IAsyncResult iAsyncResult)
        {
            if(this.socket.Connected)
            {
                try
                {
                    Packet<T> packet = (Packet<T>) iAsyncResult.AsyncState;

                    int bytes = this.socket.EndSend(iAsyncResult);
            
                    DatagramSendEvent?.Invoke(this, packet);
                }
                catch(SocketException socketException)
                {
                    ExceptionEvent?.Invoke(this, socketException);
                }
            }
            else ServerDisconnectedEvent?.Invoke(this, null);
        }

        private void Receive()
        {
            if(this.socket.Connected)
            {
                this.socket.BeginReceiveFrom(
                    this.packetSend.Buffer, 0, Packet<ClientPacket<T>>.BufferSize, 
                    SocketFlags.None, ref endPointFrom, BeginReceiveFromCallback, 
                    this.packetReceive
                );
            }
            else ServerDisconnectedEvent?.Invoke(this, null);
        }

        private void BeginReceiveFromCallback(IAsyncResult iAsyncResult)
        {
            if(this.socket.Connected)
            {
                try
                {
                    Packet<ClientPacket<T>> packet = (Packet<ClientPacket<T>>) iAsyncResult.AsyncState;

                    int bytes = this.socket.EndReceiveFrom(iAsyncResult, ref endPointFrom);
                    
                    this.socket.BeginReceiveFrom(packet.Buffer, 0, Packet<ClientPacket<T>>.BufferSize, SocketFlags.None, ref endPointFrom, BeginReceiveFromCallback, packet);
                    
                    string newData = Encoding.ASCII.GetString(packet.Buffer, 0, bytes);

                    byte[] buffer = Encoding.ASCII.GetBytes(newData);

                    Packet<ClientPacket<T>> packetReceived = new Packet<ClientPacket<T>>(buffer);

                    DatagramReceivedEvent?.Invoke(this, packetReceived);
                }
                catch(SocketException socketException)
                {
                    ExceptionEvent?.Invoke(this, socketException);
                }
            }
            else ServerDisconnectedEvent?.Invoke(this, null);
        }
    }
}
