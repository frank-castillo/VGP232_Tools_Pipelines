using System;
using System.Collections.Generic;
using System.Text;

namespace VGP232_Notes.Week_01
{
    class LoopExercise
    {
        private void Main(string[] args)
        {
            const string cDone = "DONE";
            string result = string.Empty;
            int totalCount = 0;

            while(result.ToUpper() != cDone)
            {
                Console.Clear();
                Console.WriteLine(@"Enter a number or write 'DONE' to exit...");
                result = Console.ReadLine();
                int intValue = 0;
                if(int.TryParse(result, out intValue))
                {
                    totalCount += intValue;
                }
            }

            Console.Clear();
            Console.WriteLine($"Total sum = {totalCount}");
        }
    }
}
