using System;

namespace VGP232_Notes
{
    class InputExercise
    {
        private void Main(string[] args)
        {
            string input = string.Empty;
            string myName = "Frank";

            Console.WriteLine("Please enter your name: ");
            input = Console.ReadLine();

            if(input.ToLower() == myName.ToLower())
            {
                Console.WriteLine($"Welcome {myName}, have a nice day!");
            }
            else
            {
                Console.WriteLine($"Hello {input}");
            }
        }
    }
}
