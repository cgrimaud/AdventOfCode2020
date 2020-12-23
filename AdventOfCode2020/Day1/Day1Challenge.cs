using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020.Day1
{

    // https://adventofcode.com/2020/day/1
    class Day1Challenge
    {
        public static void FindPair(List<int> numbList, int sum)
        {
            for (int i = 0; i < numbList.Count; i++)
            {
                int numbToFind = sum - numbList[i];
                if (numbList.Contains(numbToFind))
                {
                    Console.WriteLine(numbList[i] * numbToFind);
                    return;
                }

            }
        }

        // found on https://www.geeksforgeeks.org/find-a-triplet-that-sum-to-a-given-value/
        static bool Find3Numbers(List<int> numbList, int arr_size, int sum)
        {
            // Fix the first 
            // element as A[i] 
            for (int i = 0; i < arr_size - 2; i++)
            {

                // Fix the second 
                // element as A[j] 
                for (int j = i + 1; j < arr_size - 1; j++)
                {

                    // Now look for 
                    // the third number 
                    for (int k = j + 1; k < arr_size; k++)
                    {
                        if (numbList[i] + numbList[j] + numbList[k] == sum)
                        {
                            Console.WriteLine(numbList[i] * numbList[j] * numbList[k]);
                            return true;
                        }
                    }
                }
            }

            // If we reach here, 
            // then no triplet was found 
            return false;
        }

        public static void RunDay1Solution()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day1\Day1Data.csv";
            if (File.Exists(filePath))
            {
                var reader = new StreamReader(File.OpenRead(filePath));
                List<int> listA = new List<int>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    foreach (var item in values)
                    {
                        listA.Add(Int32.Parse(item));
                    }
                }
                FindPair(listA, 2020);
                Find3Numbers(listA, listA.Count, 2020);
            }
            else
            {
                Console.WriteLine("File doesn't exist");
            }
            Console.ReadLine();
        }
    }
}
