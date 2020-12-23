using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Day7
{
    // followed along with https://www.youtube.com/watch?v=NYFMumQf9h8&ab_channel=NKCSS to get the answer
    class Day7Challenge
    {
        public static string GetInput()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day7\Day7Data.csv";
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


            var values = GetInput().Split(new char[] { '\r','\n' }, StringSplitOptions.RemoveEmptyEntries);


            Console.WriteLine($"Found {values.Length:N0} groups");

            return values;
        }

        public static IEnumerable<string> SplitBy(string contents, string splitBy)
        {
            int splitLength = splitBy.Length;
            int previousIndex = 0;
            int ix = contents.IndexOf(splitBy);
            while(ix >= 0)
            {
                yield return contents.Substring(previousIndex, ix - previousIndex);
                previousIndex = ix + splitLength;
                ix = contents.IndexOf(splitBy, previousIndex);
            }
            string remain = contents.Substring(previousIndex);
            if(!string.IsNullOrEmpty(remain)) yield return remain;
        }

        public static int Day7ChallengeSolutionPt1() 
        {
            const string ourBag = "shiny gold";
            int result = 0;

            var rules = ParseChallengeData();
            const string splitBy = " bags contain ";
            Dictionary<string, List<(string bag, int occurence)>> parentRules = new Dictionary<string, List<(string bag, int occurence)>>();
            foreach(string rule in rules)
            {
                string[] parts = SplitBy(rule.Trim('"'), splitBy).ToArray();
                if (parts[1].StartsWith("no"))
                {
                    // fine; no children
                    parentRules.Add(parts[0].Replace("\"", ""), new List<(string bag, int occurence)>());
                } 
                else
                {
                    var children = SplitBy(parts[1].Replace("bags", "bag").Replace("bag.", ""), " bag, ").ToList();
                    var childInfo = new List<(string bag, int occurence)>();
                    foreach (var child in children)
                    {
                        string[] childParts = child.Replace("\"", "").TrimEnd().Split(' ');
                        if(childParts.Length == 3)
                        {
                            if (int.TryParse(childParts[0], out int occurence))
                            {
                                childInfo.Add(($"{childParts[1]} {childParts[2]}", occurence));
                            }
                            else throw new ArgumentException($"Could not parse '{childParts[0]}' as a valid number");
                        }
                        else throw new ArgumentException($"Expected 3 parts, got '{childParts.Length}'");

                    }
                    parentRules.Add(parts[0], childInfo);
                }
            }
            Dictionary<string, List<(string bag, int occurence)>> childRules = new Dictionary<string, List<(string bag, int occurence)>>();
            foreach(string key in parentRules.Keys)
            {
                var children = parentRules[key];
                foreach(var child in children)
                {
                    if(!childRules.TryGetValue(child.bag, out var parents))
                    {
                        parents = new List<(string bag, int occurence)>();
                        childRules.Add(child.bag, parents);
                    }
                    parents.Add((key, child.occurence));
                }
            }
            HashSet<string> all = new HashSet<string>();
            var resultInfo = CountParents((ourBag, 1), new HashSet<string>(), all, 1, childRules);
            //Console.WriteLine($"Highest: {resultInfo.allSeen.Count -1}, {string.Join(", ", resultInfo.allSeen)}");
            Console.WriteLine($"Highest: {resultInfo.allSeen.Count - 1}");
            var answer = GetBagCount(ourBag, parentRules) - 1;
            Console.WriteLine($"Total number of bags is {answer}");
            // rebuilt is test to reverse the parsing to make sure everything is correct - can compare to original data
            //string rebuilt = string.Join("\n", parentRules.Select(x => $"{x.Key} bag contain {(x.Value.Count == 0 ? "no other bag" : $"{string.Join(" bag, ", x.Value)}")}"));
            return result;
        }

        public static int GetBagCount(string bag, Dictionary<string, List<(string bag, int occurence)>> parentRules)
        {
            int result = 1;
            if(parentRules.TryGetValue(bag, out var children))
            {
                foreach(var child in children)
                {
                    result += child.occurence * GetBagCount(child.bag, parentRules);
                }
            }
            return result;
        }

        //recursive function
        public static (int highest, HashSet<string> allSeen) CountParents((string bag, int occurence) bagInfo, HashSet<string> seen, HashSet<string> allSeen, int count, Dictionary<string, List<(string bag, int occurence)>> childRules)
        {
            allSeen.Add(bagInfo.bag);
            int highest = count;
         
            if(!childRules.TryGetValue(bagInfo.bag, out var children) || children.Count == 0)
            {
                //Console.WriteLine($"{count} {string.Join(", ", seen)}, {bagInfo.bag}");
            } 
            else
            {
                foreach (var child in children)
                {
                    if (!seen.Contains(child.bag))
                    {
                        var localSeen = new HashSet<string>(seen.Union(new string[] { bagInfo.bag }));
                        var info = CountParents(child, localSeen, allSeen, count + 1, childRules);
                        allSeen.UnionWith(info.allSeen);
                        if(info.highest > highest)
                        {
                            highest = info.highest;
                        }
                        
                    }
                    else Console.WriteLine($"Already processed {child.bag}");
                }
            }

            return (highest, allSeen);
        }
    }
}
