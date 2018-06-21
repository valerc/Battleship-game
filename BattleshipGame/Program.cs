using System;

namespace BattleshipGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Battleship battleship = new Battleship();

            Console.WriteLine("Push any button to fill the field");
            Console.ReadKey();
            char input;

            do
            {
                battleship.DrawBattlefield();

                Console.WriteLine("\nPush any button to refill the field. Exit: Esc, 'E'");
                input = Console.ReadKey().KeyChar;

            } while (input != 27 && input != 101 && input != 69);
        }
    }
}
