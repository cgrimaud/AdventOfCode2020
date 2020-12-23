using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day9
{
    class Day9Challenge
    {
        public static string GetInput()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day9\Day9Data.txt";
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

        public static void Day9Solution()
        {
            string[] xmasValues = ParseChallengeData();
            long[] xmasNumbers = Array.ConvertAll(xmasValues, long.Parse);
            long sum = 0;
            
            for (int i = 0; i < xmasNumbers.Length; i++)
            {
                long[] preamble = xmasNumbers[i..(i + 25)];
                sum = xmasNumbers[i + 25];
                bool check = CheckForSumInArray(preamble, sum);
                if (!check)
                {
                    Console.WriteLine($"First number that does not sum: {sum}");
                    break;
                }
                else
                {
                    int count = i + 1;
                    //Console.WriteLine($"iteration #{count}: {preamble[0]} through {preamble[24]} successful sum: {sum}");
                }
            }
            
            long invalidNumber = sum;

            long[] subArrayLocation = subArraySum(xmasNumbers, xmasNumbers.Length, invalidNumber);
            int[] subArrayIndexes = new int[2];
            for (int i = 0; i < subArrayLocation.Length; i++)
            {
                var index = subArrayLocation[i].ToString();

                subArrayIndexes[i] = Int32.Parse(index);
            }

            long[] subArray = xmasNumbers[subArrayIndexes[0]..subArrayIndexes[1]];
            Console.WriteLine($"{subArray.Min()} plus {subArray.Max()} = {subArray.Min() + subArray.Max()}");
        }

        public static bool CheckForSumInArray(long[] arr, long sum)
        {
            // process found on https://www.geeksforgeeks.org/given-an-array-a-and-a-number-x-check-for-pair-in-a-with-sum-as-x/

            HashSet<long> numSet = new HashSet<long>();
            for(var i = 0; i < arr.Length; i++)
            {
                long temp = sum - arr[i];
                if (numSet.Contains(temp))
                {
                    return true;
                }
                numSet.Add(arr[i]);
            }
            return false;
        }

        public static long[] subArraySum(long[] arr, long n,long sum)
        {

            // method found on https://www.geeksforgeeks.org/find-subarray-with-given-sum/
            long curr_sum, i, j;

            // Pick a starting point 
            for (i = 0; i < n; i++)
            {
                curr_sum = arr[i];

                // try all subarrays 
                // starting with 'i' 
                for (j = i + 1; j <= n; j++)
                {
                    if (curr_sum == sum)
                    {
                        long p = j - 1;
                        Console.WriteLine($"Sum found between indexes {i} and {p}");
                        long[] indexes = { i, p };
                        return indexes;
                    }
                    if (curr_sum > sum || j == n)
                        break;

                    curr_sum = curr_sum + arr[j];
                }
            }

            Console.WriteLine("No subarray found");
            return null;
        }
    }
}
