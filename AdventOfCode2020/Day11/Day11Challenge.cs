using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Day11
{
    class Day11Challenge
    {

        public static string GetInput()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day11\Day11Data.csv";
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

        public static void Day11Solution()
        {
            var results = ParseChallengeData();

            List<string> rowLayout = new List<string>(results);
            int iterations = 0;
            FillUpSeats(rowLayout, iterations);

            Console.WriteLine();
        }

        private static void FillUpSeats(List<string> rowLayout, int iterations)
        {
            
            char empty = 'L';
            char occupied = '#';

            int rows = rowLayout.Count;
            int columns = rowLayout[0].Length;

            List<string> currentLayout = rowLayout;
            List<string> nextLayout = new List<string>();

            for (var y = 0; y < rows; y++)
            {
                StringBuilder nextRow = new StringBuilder();
                for (var x = 0; x < columns; x++)
                {
                    var currentLocation = rowLayout[y][x];
                    var seatIsEmpty = currentLocation == empty;
                    var seatIsOccupied = currentLocation == occupied;

                    if (seatIsEmpty || seatIsOccupied)
                    {
                        int[] location = { y, x };
                        //var surroundingLocations = GetAdjValues(rowLayout, location);
                        var surroundingLocations = GetNewAdjValues(rowLayout, location);
                        //int emptySeats = 0;
                        int surroundingOccupiedSeats = 0;
                        if (surroundingLocations.ContainsKey(occupied)) surroundingOccupiedSeats = surroundingLocations[occupied];
                        if (seatIsEmpty && surroundingOccupiedSeats == 0)
                        {
                            nextRow.Append(occupied);
                        }
                        else if (seatIsOccupied && surroundingOccupiedSeats > 4)
                        {
                            nextRow.Append(empty);
                        } else
                        {
                            nextRow.Append(currentLocation);
                        }
                    }
                    else
                    {
                        nextRow.Append(currentLocation);
                    }
                }
                nextLayout.Add(nextRow.ToString());
            }
            int seatCount = 0;
            foreach(var seat in nextLayout)
            {
                int count = seat.Count(x => x == occupied);
                seatCount += count;
            }
            if (Changes(rowLayout, nextLayout))
            {
                iterations++;               
                FillUpSeats(nextLayout, iterations);
            }
            else
            {
                Console.WriteLine($"# of iterations {iterations}, final number of seats: {seatCount}");
            }            
        }

        public static Dictionary<char, int> GetAdjValues(List<string> layout, int[] location)
        {

            var rowLength = layout.Count-1;
            var colLength = layout[0].Length-1;
            var row = location[0];
            var col = location[1];

            var characters = new List<char>();
            var charDictionary = new Dictionary<char, int>();

            bool rowAboveExists = row > 0;
            bool rowBelowExists = row < rowLength;
            bool colLeftExists = col > 0;
            bool colRightExists = col < colLength;
            //top left from location
            if (rowAboveExists && colLeftExists) characters.Add(layout[row - 1][col - 1]);
            // top middle from location
            if(rowAboveExists) characters.Add(layout[row-1][col]);
            // top right from location
            if(rowAboveExists && col < colLength) characters.Add(layout[row-1][col+1]);
            // left of location
            if(colLeftExists) characters.Add(layout[row][col - 1]);
            // right of location
            if(colRightExists) characters.Add(layout[row][col + 1]);
            // bottom left of location
            if(rowBelowExists && colLeftExists) characters.Add(layout[row + 1][col - 1]);            
            //  bottom middle of location
            if(rowBelowExists) characters.Add(layout[row + 1][col]);            
            // bottom right of location
            if(rowBelowExists && colRightExists) characters.Add(layout[row + 1][col + 1]);
            
            // TODO: can this be refactored into ternary?
            foreach(var ch in characters)
            {
                if (charDictionary.ContainsKey(ch))
                {
                    charDictionary[ch]++;
                }
                else
                {
                    charDictionary.Add(ch, 1);
                }
            }
           
            return charDictionary;
        }

        public static bool Changes (List<string> currentLayout, List<string> nextLayout)
        {
            if (currentLayout.SequenceEqual(nextLayout)) return false;
            else return true;


        }

        public static Dictionary<char, int> GetNewAdjValues(List<string> layout, int[] location)
        {
            var characters = new List<char>();
            var charDictionary = new Dictionary<char, int>();

            characters.Add(GetNextNorthWest(layout, location));
            characters.Add(GetNextNorth(layout, location));
            characters.Add(GetNextNorthEast(layout, location));
            characters.Add(GetNextEast(layout, location));
            characters.Add(GetNextSouthEast(layout, location));
            characters.Add(GetNextSouth(layout, location));
            characters.Add(GetNextSouthWest(layout, location));
            characters.Add(GetNextWest(layout, location));


            // TODO: can this be refactored into ternary?
            foreach (var ch in characters)
            {
                if (charDictionary.ContainsKey(ch))
                {
                    charDictionary[ch]++;
                }
                else
                {
                    charDictionary.Add(ch, 1);
                }
            }

            return charDictionary;
        }

        public static char GetNextNorthWest(List<string> layout, int[] currentLocation)
        {
            var cRow = currentLocation[0];
            var cCol = currentLocation[1];
            var nRow = cRow - 1;
            var nCol = cCol - 1;

            bool northWestValidation = cRow > 0 && cCol > 0;

            if (northWestValidation)
            {
                char nextValue = layout[nRow][nCol];
                if (nextValue == '.')
                {
                    int[] nextLocation = { nRow, nCol };
                    return GetNextNorthWest(layout, nextLocation);
                }
                else return nextValue;
            }
            else return ' ';
        }

        public static char GetNextNorth(List<string> layout, int[] currentLocation)
        {
            var cRow = currentLocation[0];
            var cCol = currentLocation[1];
            var nRow = cRow - 1;
            var nCol = cCol;

            bool northValidation = cRow > 0;

            if (northValidation)
            {
                char nextValue = layout[nRow][nCol];
                if (nextValue == '.')
                {
                    int[] nextLocation = { nRow, nCol };
                    return GetNextNorth(layout, nextLocation);
                }
                else return nextValue;
            }
            else return ' ';
        }

        public static char GetNextNorthEast(List<string> layout, int[] currentLocation)
        {
            var colLength = layout[0].Length - 1;
            var cRow = currentLocation[0];
            var cCol = currentLocation[1];
            var nRow = cRow - 1;
            var nCol = cCol + 1;

            bool northEastValidation = cRow > 0 && cCol < colLength;

            if (northEastValidation)
            {
                char nextValue = layout[nRow][nCol];
                if (nextValue == '.')
                {
                    int[] nextLocation = { nRow, nCol };
                    return GetNextNorthEast(layout, nextLocation);
                }
                else return nextValue;
            }
            else return ' ';
        }

        public static char GetNextEast(List<string> layout, int[] currentLocation)
        {
            var colLength = layout[0].Length - 1;
            var cRow = currentLocation[0];
            var cCol = currentLocation[1];
            var nRow = cRow;
            var nCol = cCol + 1;

            bool eastValidation = cCol < colLength;

            if (eastValidation)
            {
                char nextValue = layout[nRow][nCol];
                if (nextValue == '.')
                {
                    int[] nextLocation = { nRow, nCol };
                    return GetNextEast(layout, nextLocation);
                }
                else return nextValue;
            }
            else return ' ';
        }

        public static char GetNextSouthEast(List<string> layout, int[] currentLocation)
        {
            var rowLength = layout.Count - 1;
            var colLength = layout[0].Length - 1;
            var cRow = currentLocation[0];
            var cCol = currentLocation[1];
            var nRow = cRow + 1;
            var nCol = cCol + 1;


            bool southEastValidation = cRow < rowLength && cCol < colLength;

            if (southEastValidation)
            {
                char nextValue = layout[nRow][nCol];
                if (nextValue == '.')
                {
                    int[] nextLocation = { nRow, nCol };
                    return GetNextSouthEast(layout, nextLocation);
                }
                else return nextValue;
            }
            else return ' ';
        }

        public static char GetNextSouth(List<string> layout, int[] currentLocation)
        {
            var rowLength = layout.Count - 1;
            var cRow = currentLocation[0];
            var cCol = currentLocation[1];
            var nRow = cRow + 1;
            var nCol = cCol;

            bool southValidation = cRow < rowLength;

            if (southValidation)
            {
                char nextValue = layout[nRow][nCol];
                if (nextValue == '.')
                {
                    int[] nextLocation = { nRow, nCol };
                    return GetNextSouth(layout, nextLocation);
                }
                else return nextValue;
            }
            else return ' ';
        }

        public static char GetNextSouthWest(List<string> layout, int[] currentLocation)
        {
            var rowLength = layout.Count - 1;
            var cRow = currentLocation[0];
            var cCol = currentLocation[1];
            var nRow = cRow + 1;
            var nCol = cCol - 1;

            bool southWestValidation = cRow < rowLength && cCol > 0;


            if (southWestValidation)
            {
                char nextValue = layout[nRow][nCol];
                if (nextValue == '.')
                {
                    int[] nextLocation = { nRow, nCol };
                    return GetNextSouthWest(layout, nextLocation);
                }
                else return nextValue;
            }
            else return ' ';
        }

        public static char GetNextWest(List<string> layout, int[] currentLocation)
        {
            var cRow = currentLocation[0];
            var cCol = currentLocation[1];
            var nRow = cRow;
            var nCol = cCol - 1;

            bool westValidation = cCol > 0;

            if (westValidation)
            {
                char nextValue = layout[nRow][nCol];
                if (nextValue == '.')
                {
                    int[] nextLocation = { nRow, nCol };
                    return GetNextWest(layout, nextLocation);
                }
                else return nextValue;
            }
            else return ' ';
        }
    }

}

