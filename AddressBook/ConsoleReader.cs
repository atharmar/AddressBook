using System;

namespace AddressBook
{
    public class ConsoleReader : IGetInputFromUsers
    {
        public string GetNonEmptyString()
        {
            string input = Console.ReadLine();
            while (input.Length == 0)
            {
                Console.WriteLine("That is not valid.");
                input = Console.ReadLine();
            }
            return input;
        }

        public int GetNumber()
        {
            //Get the number//
            int value;
            string input = Console.ReadLine();
            while (!int.TryParse(input, out value))
            {
                Console.WriteLine("You should type a number.");
                input = Console.ReadLine();
            }

            return value;
        }

        public void WaitForEnterKey()
        {
            Console.WriteLine("Press ENTER to continue.......");
            Console.ReadLine();
        }
    }
}