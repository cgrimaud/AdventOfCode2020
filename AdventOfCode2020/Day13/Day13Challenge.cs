using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day13
{
    class Day13Challenge
    {
        public static string GetInput()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day13\Day13Data.csv";
            if (File.Exists(filePath)) return File.ReadAllText(filePath);
            else return "an error has occured";
        }
        public static string[] ParseChallengeData()
        {
            var values = GetInput().Replace("\"", "").Split(new char[] { '\r', '\n', ',' }, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine($"Found {values.Length:N0} groups");
            return values;
        }

        public static void Day13Solution()
        {
            var schedule = ParseChallengeData();
            // https://stackoverflow.com/questions/19091393/find-all-integers-in-list-of-strings
            var filteredSchedule = schedule.Where(s => s.All(char.IsDigit)).ToList();

            var departureTime = Decimal.Parse(filteredSchedule[0]);
            var busLines = new List<Decimal>();
            for (int i = 1; i < filteredSchedule.Count; i++)
            {
                busLines.Add(Decimal.Parse(filteredSchedule[i]));
            }
            Console.WriteLine($"Departure Time {departureTime}");
            var nextAvailableBus = new Dictionary<decimal, decimal>();
            foreach (var bus in busLines)
            {
                var busId = bus;
                var availableTime = Math.Ceiling(departureTime/bus)*busId;
                nextAvailableBus[busId] = availableTime;
                Console.WriteLine($"BusID: {busId} -----> Available Time: {availableTime}");
            }
            var nextBusIdTime = nextAvailableBus.MinBy(kvp => kvp.Value).ToDictionary();


            decimal nextBusId = nextBusIdTime.ElementAt(0).Key;
            decimal nextBusTime = nextBusIdTime.ElementAt(0).Value;

            var solutionAnswer = nextBusId * (nextBusTime - departureTime);

            Console.WriteLine($"Next Bus ID: {nextBusId} Next Bus Time:{nextBusTime}");
            Console.WriteLine($"Solution Answer: {solutionAnswer}");
            Console.WriteLine();
        }


    }
}
