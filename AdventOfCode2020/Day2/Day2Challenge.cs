using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Day2
{
    class Day2Challenge
    {
        public static int FindValidPasswordsPt1(List<PasswordObject> passwords)
        {
           
            int correctPasswords = 0;
            // loop through password list
            foreach (var pObject in passwords)
            {
                // for each password, count how many times the ~character~ appears in the ~password~ 
                var passwordString = pObject.Password;
                var character = Convert.ToChar(pObject.Character);

                int count = passwordString.Count(x => x == character);
                
                // then check to see if that number is between the ~startRange~ and ~endRange~ 
                // if number falls in range, add to a counter

                if (count >= pObject.StartRange && count <= pObject.EndRange)
                    {
                        correctPasswords++;
                    }

            }
            Console.WriteLine(correctPasswords);
            return correctPasswords;
        }


        public static int FindValidPasswordsPt2(List<PasswordObject> passwords)
        {
            int correctPasswords = 0;

            foreach (var pObject in passwords)
            {
                // for each password if ~character~ is in startRange-1 or endRange-1 add to correct password
                var passwordString = pObject.Password;
                var character = Convert.ToChar(pObject.Character);
                int startIndex = pObject.StartRange - 1;
                int endIndex = pObject.EndRange - 1;

                // Logical exclusive OR operator ^
                if (passwordString[startIndex] == character ^ passwordString[endIndex] == character)
                {
                    correctPasswords++;
                }

            }
            Console.WriteLine(correctPasswords);
            return correctPasswords;
        }

        public static void RunDay2Solution()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day2\Day2Data.csv";
            if (File.Exists(filePath))
            {
                var reader = new StreamReader(File.OpenRead(filePath));
                List<string> listA = new List<string>();
                List<PasswordObject> passwords = new List<PasswordObject>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    foreach (var item in values)
                    {
                        listA.Add(item);
                        //Console.WriteLine(item);
                    }
                }


                foreach (string item in listA)
                {
                    //Console.WriteLine(item);
                    var objValues = item.Split(new[] { '-', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

                    var startRange = objValues[0];
                    var endRange = objValues[1];
                    var character = objValues[2];
                    var password = objValues[3];

                    var pObject = new PasswordObject(Int32.Parse(startRange), Int32.Parse(endRange), character, password);

                    passwords.Add(pObject);
                    //Console.WriteLine($"Start--> {pObject.StartRange} End--> {pObject.EndRange} Character--> {pObject.Character} Password--> {pObject.Password}");
                }
                FindValidPasswordsPt1(passwords);
                FindValidPasswordsPt2(passwords);

            }
            else
            {
                Console.WriteLine("File doesn't exist");
            }
            Console.ReadLine();
        }

        
    }

    public class PasswordObject
    {
        public PasswordObject(int startRange, int endRange, string character, string password)
        {
            StartRange = startRange;
            EndRange = endRange;
            Character = character;
            Password = password;
        }

        public int StartRange { get; set; }
        public int EndRange { get; set; }
        public string Character { get; set; }
        public string Password { get; set; }
    }
}
