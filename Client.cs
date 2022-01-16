using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpRule
{
    class Client<T>
    {
        // Events Handler
        public event EventHandler<object> DataReceivedEvent;
        public event EventHandler<object> DataSendEvent;
        public event EventHandler ServerDownEvent;
        public event EventHandler<Exception> ExceptionEvent;
        
        private Socket socket;
        private EndPoint endPointFrom;
        private Packet<T> packet;
        
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
            this.packet         = new Packet<T>();

            this.socket.Connect(this.endPointFrom);

            if(this.socket.Connected)
            {
                this.Receive();
            }
            else ServerDownEvent?.Invoke(this, null);
        }

        public void SendData(T data)
        {
            if(this.socket.Connected)
            {
                try
                {
                    this.packet.AddOwner(data);

                    this.packet.PacketSerializeClient();

                    this.socket.BeginSend(this.packet.Buffer, 0, this.packet.Buffer.Length, SocketFlags.None, BeginSendCallback, this.packet);
                }
                catch(Exception exception)
                {
                    ExceptionEvent?.Invoke(this, exception);
                }
            }
            else ServerDownEvent?.Invoke(this, null);
        }

        private void BeginSendCallback(IAsyncResult iAsyncResult)
        {
            if(this.socket.Connected)
            {
                try
                {
                    Packet<T> packet = (Packet<T>) iAsyncResult.AsyncState;

                    int bytes = this.socket.EndSend(iAsyncResult);
            
                    DataSendEvent?.Invoke(this, packet);
                }
                catch(Exception exception)
                {
                    ExceptionEvent?.Invoke(this, exception);
                }
            }
            else ServerDownEvent?.Invoke(this, null);
        }

        private void Receive()
        {
            if(this.socket.Connected)
            {
                this.socket.BeginReceiveFrom(
                    this.packet.Buffer, 0, Packet<T>.BufferSize, 
                    SocketFlags.None, ref endPointFrom, BeginReceiveFromCallback, 
                    new Packet<T>()
                );
            }
            else ServerDownEvent?.Invoke(this, null);
        }

        private void BeginReceiveFromCallback(IAsyncResult iAsyncResult)
        {
            if(this.socket.Connected)
            {
                try
                {
                    Packet<T> packet = (Packet<T>) iAsyncResult.AsyncState;

                    int bytes = this.socket.EndReceiveFrom(iAsyncResult, ref endPointFrom);
                    
                    this.socket.BeginReceiveFrom(packet.Buffer, 0, Packet<T>.BufferSize, SocketFlags.None, ref endPointFrom, BeginReceiveFromCallback, packet);
                    
                    byte[] buffer = packet.Buffer;

                    Array.Resize(ref buffer, bytes);

                    Packet<T> packetReceived = new Packet<T>(buffer);

                    DataReceivedEvent?.Invoke(this, packetReceived);
                }
                catch(Exception exception)
                {
                    ExceptionEvent?.Invoke(this, exception);
                }
            }
            else ServerDownEvent?.Invoke(this, null);
        }
    }
}
