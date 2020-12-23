using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day6
{
    class Day6Challenge
    {

        public static string GetInput()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day6\Day6Data.csv";
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

            var values = GetInput().Replace("\r", "").Replace("\n\n", "@").Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine($"Found {values.Length:N0} groups");

            return values;
        }

        public static void countYesAnswersPt1()
        {
            var groupAnswers = ParseChallengeData();

            List<int> yesCountPerGroup = new List<int>();

            foreach (var group in groupAnswers)
            {
                // count unique letters
                var noSpaces = group.Replace("\n", "");
                var count = noSpaces.Distinct().Count();
                yesCountPerGroup.Add(count);


            }

            int total = yesCountPerGroup.Sum();
            Console.WriteLine($"Sum of total yes counts per group: {total}");
        }

        public static void countYesAnswersPt2()
        {
            var groupAnswers = ParseChallengeData();

            int peopleCount = 1;
            int groupYesCount = 0;

            foreach (var group in groupAnswers)
            {
                 // count new lines in each group: newline +1 = number of people in the group
                foreach(var ch in group)
                {
                    if (ch == '\n')
                    {
                        peopleCount++;
                    }
                }

                // last group ends with new line, while all other groups didn't
                if(group[group.Length-1] == '\n')
                {
                    peopleCount--;
                }

                // create dictionary of character keys and their counts
                Dictionary<char, int> charDictionary = new Dictionary<char, int>();
                var chars = group.ToCharArray();
                foreach(var ch in chars)
                {
                    if (charDictionary.ContainsKey(ch))
                    {
                        charDictionary[ch] = charDictionary[ch] + 1;
                    }
                    else
                    {
                        charDictionary.Add(ch, 1);
                    }
                }

                // if group number of people matched count of letter, incrase total group yes count by 1;
                foreach (KeyValuePair<char, int> item in charDictionary)
                {                    
                    if(item.Key != '\n' && item.Value == peopleCount)
                    {                      
                        groupYesCount++;
                    }
                }

                peopleCount = 1;
            }
            
            Console.WriteLine($"Sum of total unanimous group Yes: {groupYesCount}");
            
        }
    }
}
