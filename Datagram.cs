using System.Net;

namespace UdpRule
{
    public class Datagram<T>
    {
        public IPEndPoint IpEndPoint { get; private set; }
        
        public Packet<T> Packet { get; private set; }
        public Datagram(IPEndPoint ipEndPoint, Packet<T> packet)
        {
            this.IpEndPoint = ipEndPoint;
            this.Packet     = packet;
        }
    }
}
