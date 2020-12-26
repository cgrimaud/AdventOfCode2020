using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    if (nextVal - currentVal == 1) oneJoltCount++;
                    if (nextVal - currentVal == 2) twoJoltCount++;
                    if (nextVal - currentVal == 3) threeJoltCount++;

            }
            Console.WriteLine($"Part 1 Answer: {oneJoltCount} X {threeJoltCount} equals: {oneJoltCount*threeJoltCount}");

        }

        public static void Day10SolutionPt2()
        {
            // solution found on https://www.youtube.com/watch?v=unEILSNifFA&ab_channel=NKCSS

            string[] values = ParseChallengeData();
            var numbers = values.Select(x => Int32.Parse(x)).Union(new int[] { 0 }).OrderBy(x => x).ToList();
           
            int skippable = 0;
            long result = 1;
            int num;
            var pathCount = new Dictionary<int, long>();
            pathCount[0] = result;
            for(int i = 1; i < numbers.Count; i++)
            {
                num = numbers[i];
                if (i > 1 && num - numbers[i - 2] <= 3)
                {
                    result += pathCount[i - 2];
                    skippable++;
                } 
                if (i > 2 && num - numbers[i - 3]<= 3)
                {
                    result += pathCount[i - 3];
                    skippable++;
                }
                pathCount[i] = result;
                //Console.WriteLine(result);
            }
            Console.WriteLine($"Skippable: {skippable}; result: {result}");
        }
    }
}
