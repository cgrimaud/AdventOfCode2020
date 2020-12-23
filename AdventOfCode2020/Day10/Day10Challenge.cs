using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2020.Day10
{
    class Day10Challenge
    {
        public static string GetInput()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day10\Day10Data.csv";
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
             var values = GetInput().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"Found {values.Length:N0} groups");
            return values;
        }

        public static void Day10Solution()
        {
            string[] values = ParseChallengeData();
            int[] joltageValues = Array.ConvertAll(values, int.Parse);
            Array.Sort(joltageValues);
            // start at 1 to account for 0 to 1;
            int oneJoltCount = 1;
            int twoJoltCount = 0;
            // start at 1 to account for final +3 built-in joltage adapter
            int threeJoltCount = 1;

            for (int i = 0; i < joltageValues.Length-1; i++)
            {
                int currentVal = joltageValues[i];
                
                    int nextVal = joltageValues[i + 1];
                    int count = i + 1;
                    if (nextVal - currentVal == 1)
                    {
                        Console.WriteLine($"1 Jolt iteration #{count} ------>  {currentVal}, {nextVal} = {nextVal-currentVal}");
                        oneJoltCount++;
                    }
                    else if (nextVal - currentVal == 2)
                    {
                        Console.WriteLine($"2 Jolt iteration #{count} ------>  {currentVal}, {nextVal} = {nextVal - currentVal}");
                        twoJoltCount++;
                    }
                    else if (nextVal - currentVal == 3)
                    {
                        Console.WriteLine($"3 Jolt iteration #{count} ------>  {currentVal}, {nextVal} = {nextVal - currentVal}");
                        threeJoltCount++;
                    }
            }
            Console.WriteLine($"{oneJoltCount} X {threeJoltCount} equals: {oneJoltCount*threeJoltCount}");

        }
    }
}
