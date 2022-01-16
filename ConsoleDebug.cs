using System;

namespace UdpRule
{
    public class ConsoleDebug
    {
        public enum DebugColor
        {
            Green = 1,
            Yellow,
            Cyan,
            Red

        }
        
        public static void WriteLine(string message, DebugColor debugColor = 0)
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write(DateTime.Now.ToString("yyy-MM-dd HH:mm:ss "));

            ConsoleColor color;

            switch(debugColor)
            {
                case DebugColor.Green:
                    color = ConsoleColor.Green;
                    break;
                case DebugColor.Yellow:
                    color = ConsoleColor.Yellow;
                    break;
                case DebugColor.Red:
                    color = ConsoleColor.Red;
                    break;
                case DebugColor.Cyan:
                    color = ConsoleColor.Cyan;
                    break;
                default:
                    color = ConsoleColor.White;
                    break;
            }

            Console.ForegroundColor = color;

            Console.WriteLine(message);
            
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
