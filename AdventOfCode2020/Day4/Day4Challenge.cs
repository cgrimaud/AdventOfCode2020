using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Day4
{
    class Day4Challenge
    {

        // used https://www.youtube.com/watch?v=v0iZrsZ1Ubw&ab_channel=NKCSS for help solving
        public static string GetInput()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day4\Day4Data.csv";
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

            //var values = GetInput().Split(',');
            var values = GetInput().Replace("\r", "").Replace("\n\n", "@").Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"Found {values.Length:N0} passports");
            return values;
        }



        // VALIDATION // 
        const string inches = "in", cm = nameof(cm);

        static Func<string, bool> fourDigits = (val) => !string.IsNullOrWhiteSpace(val) && val.Length == 4 && int.TryParse(val, out int _);
        static Func<string, int, int, bool> fourDigitsValue = (val, min, max) => !string.IsNullOrWhiteSpace(val) && val.Length == 4 && int.TryParse(val, out int year) && year >= min && year <= max;

        //byr(Birth Year) - four digits; at least 1920 and at most 2002.
        static Func<string, bool> fourDigits1920 = (val) => fourDigitsValue(val, 1920, 2002);
        //eyr (Expiration Year) -four digits; at least 2020 and at most 2030.
        static Func<string, bool> fourDigits2020 = (val) => fourDigitsValue(val, 2020, 2030);
        //iyr (Issue Year) -four digits; at least 2010 and at most 2020.
        static Func<string, bool> fourDigits2010 = (val) => fourDigitsValue(val, 2010, 2020);
        static Func<string, string, bool> regexPattern = (val, pattern) => new Regex(pattern).Match(val).Success;

        //hcl (Hair Color) -a # followed by exactly six characters 0-9 or a-f.
        static Func<string, bool> hairColor = (val) => regexPattern(val, "^#[0-9a-z]{6}$");
        //ecl(Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
        static Func<string, bool> eyeColor = (val) => regexPattern(val, "^(amb|blu|brn|gry|grn|hzl|oth)$");
        //pid (Passport ID) -a nine - digit number, including leading zeroes. 
        static Func<string, bool> passport = (val) => regexPattern(val, "^[0-9]{9}$");

        //hgt (Height) -a number followed by either cm or in:
        //If cm, the number must be at least 150 and at most 193.
        //If in, the number must be at least 59 and at most 76.
        static Func<string, bool> funcLength = (val) => {
            var match = new Regex(@"^(?<val>\d+)(?<kind>in|cm)$").Match(val);
            if (match.Success)
            {
                if (!int.TryParse(match.Groups["val"].Value, out int length)) return false;

                string kind = match.Groups["kind"].Value;

                switch (kind)
                {
                    case inches:
                        {
                            return length >= 59 && length <= 76;
                        }
                    case cm:
                        {
                            return length >= 150 && length <= 193;
                        }
                    default:
                        {
                            Console.WriteLine($"Invalid height kind {kind}");
                            return false;
                        }
                }
            }
            return false;
        };



        static Dictionary<string, (bool required, Func<string, bool> validator)> fieldIds = new Dictionary<string, (bool required, Func<string, bool> validator)>()
        {
            {"iyr", (true, fourDigits2010)},
            {"eyr", (true, fourDigits2020)},
            {"hgt", (true, funcLength)},
            {"hcl", (true, hairColor)},
            {"ecl", (true, eyeColor)},
            {"pid", (true, passport)},
            {"byr", (true, fourDigits1920)},
            {"cid", (false, null)}
        };

        public static void validPassports()
        {
            var passports = ParseChallengeData();
            var validPassports = 0;
            var expectedRequiredFieldCount = fieldIds.Count(x => x.Value.required);
            var invalidFields = new List<string>();
            var invalidPassports = new List<string>();
            
            //List<string> listA = new List<string>();
            foreach (var passport in passports)
            {
                string[] parts = passport.Split(new char[] { '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var fields = parts.Select(p => p.Split(':')).Select(p => new { key = p[0], value = p[1]}).ToList();
                int requiredFieldCount = 0;
                
                foreach(var field in fields)
                {
                    if(fieldIds.TryGetValue(field.key, out (bool required, Func<string, bool> validator) info))
                    {
                        if (info.required && info.validator(field.value))
                        {
                            requiredFieldCount++;
                        }
                        else 
                        {
                            invalidFields.Add($"{field.key}: {field.value}");
                        }
                       
                    }
                    else
                    {
                        Console.WriteLine($"field '{field.key}' is not valid");
                    }
                }
                if (requiredFieldCount == expectedRequiredFieldCount)
                {
                    validPassports++;
                } else
                {
                    invalidPassports.Add(passport);
                }
            }
            Console.WriteLine($"There are {validPassports} valid passports");
        }

    }
}








