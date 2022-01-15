using System.Collections.Generic;

namespace UdpRule
{
    class ClientPacket<T>
    {
        public List<T> clients { get; private set; }
        public ClientPacket()
        {
            this.clients = new List<T>();
        }

        public void Add(T client)
        {
            this.clients.Add(client);
        }
    }
}
