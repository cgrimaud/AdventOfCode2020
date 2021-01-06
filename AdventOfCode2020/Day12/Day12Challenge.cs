using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day12
{
    class Day12Challenge
    {
        public static string GetInput()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day12\Day12Data.csv";
            if (File.Exists(filePath)) return File.ReadAllText(filePath);
            else return "an error has occured";
        }
        public static string[] ParseChallengeData()
        {
//            var testInput = @"F10
//N3
//F7
//R90
//F11";
//            var values = testInput.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var values = GetInput().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine($"Found {values.Length:N0} groups");
            return values;
        }

        public static Dictionary<string, int> directionsDict = new Dictionary<string, int>()
            {
                {"N", 0},
                {"E", 90},
                {"S", 180},
                {"W", 270}
            };
        public static void Day12Solution()
        {
            var actionsList = ParseChallengeData();
            var actions = new List<Action>();
            foreach (var item in actionsList)
            {
                string direction = item.Substring(0, 1);
                int value = int.Parse(item.Substring(1));
                var action = new Action(direction, value);
                actions.Add(action);
            }

            //var directionsDict = new Dictionary<char, int>()
            //{
            //    {'N', 0},
            //    {'E', 90},
            //    {'S', 180},
            //    {'W', 270}
            //};

            var movementsCount = new Dictionary<string, int>()
            {
                {"N", 0},
                {"E", 0},
                {"S", 0},
                {"W", 0}
            };

            string currentBoatDirection = "E";
            int count = 0;

            foreach (var action in actions)
            {
                count ++;
                int currentDirection = directionsDict[currentBoatDirection];
                var direction = action.direction;
                switch (direction)
                {
                    case "N":
                        movementsCount["N"] += action.value;
                        break;
                    case "S":
                        movementsCount["S"] += action.value;
                        break;
                    case "E":
                        movementsCount["E"] += action.value;
                        break;
                    case "W":
                        movementsCount["W"] += action.value;
                        break;
                    case "L":
                        string newDirectionLeft = BoatRotation(action.value, currentDirection, direction);
                        currentBoatDirection = currentBoatDirection.Replace(currentBoatDirection, newDirectionLeft);
                        break;
                    case "R":
                        string newDirectionRight = BoatRotation(action.value, currentDirection, direction);
                        currentBoatDirection = currentBoatDirection.Replace(currentBoatDirection, newDirectionRight);
                        break;
                    case "F":
                        // apply action.Value to whatever direction boatDirection is facing
                        movementsCount[currentBoatDirection] += action.value;
                        break;
                    default:
                        Console.WriteLine("Something went wrong...");
                        break;
                }
            }

            int absEastWest = Math.Abs(movementsCount["E"] - movementsCount["W"]);
            int absNorthSouth = Math.Abs(movementsCount["N"] - movementsCount["S"]);
            int manhaattanDistance = absEastWest + absNorthSouth;
            Console.WriteLine($"EW: {absEastWest}");
            Console.WriteLine($"NS: {absNorthSouth}");
            Console.WriteLine($"Manhattan Distance: {manhaattanDistance}");

        }

        public static string GetKeyFromValue(int val)
        {
            string key;
            if (val == 1)
            {
                key = directionsDict.FirstOrDefault(x => x.Value == val-1).Key;
            } else
            {
                key = directionsDict.FirstOrDefault(x => x.Value == val).Key;
            }
            return key;
        }

        public static string BoatRotation(int rotationAmount, int currentBoatDirection, string rotationDirection)
        {
            
            // example/test: (180, 90, R)
            if (rotationDirection == "R")
            {
                var newLocationRight = currentBoatDirection + rotationAmount;
                if (newLocationRight >= 360) return GetKeyFromValue(newLocationRight - 360);  
                else return GetKeyFromValue(newLocationRight);
            }
            else
            {
                var newLocationLeft = currentBoatDirection - rotationAmount;
                if (newLocationLeft < 0) return GetKeyFromValue(newLocationLeft + 360);
                else return GetKeyFromValue(newLocationLeft);
            }

        }


        public class Action
        {
            public string direction { get; set; }
            public int value { get; set; }
            public Action(string dir, int val)
            {
                direction = dir;
                value = val;
            }
        }
    }
}
