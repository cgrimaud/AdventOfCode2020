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
            //var testInput = @"F10
            //N3
            //F7
            //R90
            //F11";

            //var values = testInput.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

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
        public static void Day12SolutionPt1()
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
                        string newDirectionLeft = Rotation(action.value, currentDirection, direction);
                        currentBoatDirection = currentBoatDirection.Replace(currentBoatDirection, newDirectionLeft);
                        break;
                    case "R":
                        string newDirectionRight = Rotation(action.value, currentDirection, direction);
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

        public static void Day12SolutionPt2()
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

            var boatLocation = new Dictionary<string, int>()
            {
                {"N", 0},
                {"E", 0},
                {"S", 0},
                {"W", 0}
            };

            var wayPoint = new Dictionary<string, int>()
            {
                {"E", 10},
                {"W", 0},
                {"N", 1},
                {"S", 0}

            };

            foreach (var action in actions)
            {

                int wayEastWest = wayPoint["E"] - wayPoint["W"];
                int wayNorthSouth = wayPoint["N"] - wayPoint["S"];

                var direction = action.direction;
                switch (direction)
                {
                    // *****moves waypoint North by given value*****
                    case "N":
                        wayPoint["N"] += action.value;
                        break;
                    // *****moves waypoint South by given value*****
                    case "S":
                        wayPoint["S"] += action.value;
                        break;
                    // *****moves waypoint East by given value****
                    case "E":
                        wayPoint["E"] += action.value;
                        break;
                    // ****moves waypoint West by given value****
                    case "W":
                        wayPoint["W"] += action.value;
                        break;
                    // ****rotates waypoint around the ship Left(counter-clockwise) the given number of degrees****
                    case "L":
                        // returns array of current cardinal locations as ints
                        int currentEastWestValue = wayEastWest;
                        int currentNorthSouthValue = wayNorthSouth;
                        var currentWayDirectionsL = getCurrentWaypoint(wayPoint);
                        var newEastWestL = Rotation(action.value, currentWayDirectionsL[0], direction);
                        var newNorthSouthL = Rotation(action.value, currentWayDirectionsL[1], direction);
                        clearWaypointDict(wayPoint);
                        wayPoint[newEastWestL] = Math.Abs(currentEastWestValue);
                        wayPoint[newNorthSouthL] = Math.Abs(currentNorthSouthValue);

                        break;
                    // ****rotates waypoint around the ship Left(counter-clockwise) the given number of degrees****
                    case "R":
                        int currentEastWestVal = wayEastWest;
                        int currentNorthSouthVal = wayNorthSouth;
                        var currentWayDirectionsR = getCurrentWaypoint(wayPoint);
                        var newEastWestR = Rotation(action.value, currentWayDirectionsR[0], direction);
                        var newNorthSouthR = Rotation(action.value, currentWayDirectionsR[1], direction);
                        clearWaypointDict(wayPoint);
                        wayPoint[newEastWestR] = Math.Abs(currentEastWestVal);
                        wayPoint[newNorthSouthR] = Math.Abs(currentNorthSouthVal);
                        break;
                    // ****Move forward to the waypoint a number of times equal to the given value*****
                    case "F":
                        var currentWay = getCurrentWaypoint(wayPoint);
                        var eastWest = GetKeyFromValue(currentWay[0]);
                        var northSouth = GetKeyFromValue(currentWay[1]);
                        boatLocation[eastWest] += Math.Abs(wayEastWest) * action.value;
                        boatLocation[northSouth] += Math.Abs(wayNorthSouth) * action.value;
                        //boatEastWest = boatEastWest + Math.Abs((wayEastWest * action.value));
                        //boatNorthSouth = boatNorthSouth + Math.Abs((wayNorthSouth * action.value));
                        break;
                    default:
                        Console.WriteLine("Something went wrong...");
                        break;
                }
            }


            int absEastWest = Math.Abs(boatLocation["E"] - boatLocation["W"]);
            int absNorthSouth = Math.Abs(boatLocation["N"] - boatLocation["S"]);
            int manhaattanDistance = absEastWest + absNorthSouth;
            Console.WriteLine($"EW: {absEastWest}");
            Console.WriteLine($"NS: {absNorthSouth}");
            Console.WriteLine($"Manhattan Distance: {manhaattanDistance}");
            Console.WriteLine();
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

        public static string Rotation(int rotationAmount, int currentDirection, string rotationDirection)
        {
            if (rotationDirection == "R")
            {
                var newLocationRight = currentDirection + rotationAmount;
                if (newLocationRight >= 360) return GetKeyFromValue(newLocationRight - 360);  
                else return GetKeyFromValue(newLocationRight);
            }
            else
            {
                var newLocationLeft = currentDirection - rotationAmount;
                if (newLocationLeft < 0) return GetKeyFromValue(newLocationLeft + 360);
                else return GetKeyFromValue(newLocationLeft);
            }
        }
        
        public static int[] getCurrentWaypoint(Dictionary<string, int> waypoint)
        {
            // East/West is x axis 
            int wayEastWest = waypoint["E"] - waypoint["W"];
            string eastWest = wayEastWest > 0 ? "E" : "W";
            int numEastWest = directionsDict[eastWest];
            // North/south is y axis 
            int wayNorthSouth = waypoint["N"] - waypoint["S"];
            string northSouth = wayNorthSouth > 0 ? "N" : "S";
            int numNorthSouth = directionsDict[northSouth];

            int[] location = { numEastWest, numNorthSouth };
            return location;
        }

        public static void clearWaypointDict(Dictionary<string, int> waypoint)
        {
            waypoint["E"] = 0;
            waypoint["W"] = 0;
            waypoint["N"] = 0;
            waypoint["S"] = 0;
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
