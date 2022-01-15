using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace UdpRule
{
    class Packet<T>
    {
        public byte[] Buffer { get; private set; }
        public const int BufferSize = 8192;
        private long lastTimeComunication;
        
        public T client { get; private set; }
        public List<T> clients { get; private set; }
        public Packet(byte[] buffer = null)
        {            
            if(buffer == null)
            {
                this.Buffer = new byte[BufferSize];
            }
            else this.Buffer = buffer;
            
            this.lastTimeComunication = 0;

            this.clients = new List<T>();
        }
        public void AddOwner(T client)
        {
            this.client = client;
        }
        public void AddList(List<T> clients)
        {
            this.clients = clients;
        }
        public void PacketSerializeClient()
        {
            string text = JsonSerializer.Serialize<T>(this.client);
            
            this.Buffer = Encoding.ASCII.GetBytes(text);
        }
        public void PacketSerialize()
        {
            string text = JsonSerializer.Serialize<List<T>>(this.clients);
            
            this.Buffer = Encoding.ASCII.GetBytes(text);
        }
        public T PacketDeserialize()
        {
            return JsonSerializer.Deserialize<T>(this.Buffer);
        }
        public List<T> PacketDeserializeList()
        {
            return JsonSerializer.Deserialize<List<T>>(this.Buffer);
        }
        public void UpdateTimeComunication(long serverTimeUnix)
        {
            this.lastTimeComunication = serverTimeUnix;
        }
        public long LastTimeComunication()
        {
            return this.lastTimeComunication;
        }
    }
}
