using System;
using UdpRule.Test;

namespace UdpRule
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Start with Server[Y] or Client[N]: ");
            
            string param = Console.ReadLine();

            switch(param.ToLower())
            {
                case "y":
                    new ServerTest();
                    break;
                default:
                    new ClientTest();
                    break;
            }
        }
    }
}
