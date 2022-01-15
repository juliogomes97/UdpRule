using System;

namespace UdpRule.Test.Objects
{
    class GameObject
    {
        public Player Player { get; private set; }
        public Vector3 Position { get; private set; }
        public GameObject(Player player, Vector3 position)
        {
            this.Player     = player;
            this.Position   = position;
        }
    }
}
