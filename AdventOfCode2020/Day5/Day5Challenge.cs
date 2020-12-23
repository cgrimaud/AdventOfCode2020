using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day5
{
    class Day5Challenge
    {


        public static string GetInput()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day5\Day5Data.csv";
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return "an error has occured";
            }
        }

        public static string[] ParseChallengeData()
        {
            
            var values = GetInput().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine($"Found {values.Length:N0} options");
            return values;
        }

        public static void boardingPasses()
        {
            var boardingPasses = ParseChallengeData();

            List<int> seatId = new List<int>();

            foreach (var pass in boardingPasses)
            {
                var row = pass.Substring(0, 7);
                var seat = pass.Substring(7, 3);

                var rowNumb = rowSearch(row);
                var seatNumb = seatSearch(seat);
                var calcSeatId = ((rowNumb) * 8) + seatNumb;

                seatId.Add(calcSeatId);
            }


            seatId.Sort();
            int firstNum = seatId[0];
            int lastNum = seatId[seatId.Count - 1];

            Console.WriteLine($"The highest seatID is {lastNum}");
            var range = Enumerable.Range(firstNum, lastNum - firstNum);
            var missingNumbs = range.Except(seatId);

            
            foreach (var numb in missingNumbs)
            {
                Console.WriteLine($"missing number {numb} is your seat!");
            }

        }

        public static int rowSearch(string str)
        {
            int min = 0;
            int max = 127;

            foreach (var c in str)
            {
                int mid = (min + max) / 2;
                if (c == 'F')
                {
                    // F = lower half
                    max = mid;
                }
                else if (c == 'B')
                {
                    // B = upper half
                    min = mid + 1;
                }
            }
            return max;
        }

        public static int seatSearch(string str)
        {
            int min = 0;
            int max = 7;
            foreach (var c in str)
            {
                int mid = (min + max) / 2;
                if (c == 'L')
                {
                    // L = lower half
                    max = mid;
                }
                else if (c == 'R')
                {
                    // R = upper half
                    min = mid + 1;
                }
            }
            return max;

        } 

    }
}
