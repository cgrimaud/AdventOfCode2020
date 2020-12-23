using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2020.Day3
{
    class Day3Challenge
    {


        public static List<string> GetChallengeData()
        {
            try
            {
                string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day3\Day3Data.csv";
                if (File.Exists(filePath))
                {
                    var reader = new StreamReader(File.OpenRead(filePath));
                    List<string> listA = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        foreach (var item in values)
                        {
                            listA.Add(item);
                            
                        }
                    }
                    return listA;
                }
                else
                {
                    throw new Exception($"Error: unable to find file: {filePath}");
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("Error",
                    $"An error occurred while trying to get the data.");
                throw;
            }
        }

        public static int FindNumberOfTreesPt1(List<string> map)
        {
            int treeCount = 0;
            int maxStrLength = map[0].Length;
            int indexRight = 3;
            //starting at map[LineIndex][CharIndex] go right 3 Char done 1 Line 
            // until you go past the bottom of the map. CharIndex should loop back to 0 when it hits the end

            for (var i = 1; i < map.Count; i++)
            {
                var currentChar = map[i][indexRight];
                var tree = '#';

                if (currentChar == tree)
                {
                    treeCount++;
                }
                indexRight += 3;

                if (indexRight >= maxStrLength)
                {
                    indexRight -= maxStrLength;
                }
                
            }
            Console.WriteLine(treeCount);
            return treeCount;
        }

        public static int FindNumberOfTreesPt2(int indexRight, int line)
        {
            List<string> map = GetChallengeData();

            int treeCount = 0;
            int maxStrLength = map[0].Length;
            int indRight = indexRight;
            
            //starting at map[LineIndex][CharIndex] go right 3 Char done 1 Line 
            // until you go past the bottom of the map. CharIndex should loop back to 0 when it hits the end

            for (var i = line; i < map.Count; i+=line)
            {
                var currentChar = map[i][indRight];
                var tree = '#';

                if (currentChar == tree)
                {
                    treeCount++;
                }
                indRight += indexRight;

                if (indRight >= maxStrLength)
                {
                    indRight -= maxStrLength;
                }

            }
            Console.WriteLine(treeCount);
            return treeCount;
        }

        public static int totalTrees()
        {
            Console.WriteLine("Right 1 Down 1");
            int path1 = FindNumberOfTreesPt2(1, 1);
            Console.WriteLine("Right 3 Down 1");
            int path2 = FindNumberOfTreesPt2(3, 1);
            Console.WriteLine("Right 5 Down 1");
            int path3 = FindNumberOfTreesPt2(5, 1);
            Console.WriteLine("Right 7 Down 1");
            int path4 = FindNumberOfTreesPt2(7, 1);
            Console.WriteLine("Right 1 Down 1");
            int path5 = FindNumberOfTreesPt2(1, 2);

            int treeTotal = path1 * path2 * path3 * path4 * path5;

            Console.WriteLine(treeTotal);
            return treeTotal;

        }


    }
}
