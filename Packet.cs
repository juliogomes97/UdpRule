using System.Text;
using System.Text.Json;

namespace UdpRule
{
    class Packet<T>
    {
        public byte[] Buffer { get; private set; }
        public const int BufferSize = 8192;
        private long lastTimeComunication;
        public Packet(byte[] buffer = null)
        {            
            if(buffer == null)
            {
                this.Buffer = new byte[BufferSize];
            }
            else this.Buffer = buffer;
            
            this.lastTimeComunication = 0;
        }

        public void PacketSerialize(T objectClass)
        {
            string text = JsonSerializer.Serialize<T>(objectClass);
            
            this.Buffer = Encoding.ASCII.GetBytes(text);
        }
        public T PacketDeserialize()
        {
            return JsonSerializer.Deserialize<T>(this.Buffer);
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
