using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2020.Day8
{
    class Day8Challenge
    {
        public static string GetInput()
        {
            string filePath = @"C:\Users\chris\source\repos\AdventOfCode2020\AdventOfCode2020\Day8\Day8Data.csv";
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

        public static void Day8Solution()
        {
            var gameRules = ParseChallengeData();
            List<GameRule> gameRuleObjects = new List<GameRule>();

            foreach (var gameRule in gameRules)
            {
                var rules = gameRule.Split(' ');
                string ruleType = rules[0];
                int ruleValue = Int32.Parse(rules[1]);
                GameRule rule = new GameRule(ruleType, ruleValue);
                gameRuleObjects.Add(rule);
            }

            int accumulator = RunGameLoop(gameRuleObjects);


            Console.WriteLine($"Accumulator: {accumulator}");
        }

        public static int RunGameLoop(List<GameRule> gameRuleObjects)
        {
            int accumulator = 0;
            List<GameRule> rulesRan = new List<GameRule>();

            for (var i = 0; i <= gameRuleObjects.Count; i++)
            {
                var currentObj = gameRuleObjects[i];
                if (!rulesRan.Contains(currentObj))
                {
                    rulesRan.Add(currentObj);
                    var rules = RunRules(currentObj);
                    accumulator += rules[0];
                    i += rules[1];
                }
                else 
                {
                    break;
                }
            }

            
            for (var i = 0; i < rulesRan.Count; i++)
            {
                var currentRule = rulesRan[i];
                var ruleType = currentRule.RuleType;
                var id = currentRule.RuleId;

                if (ruleType == "jmp" || ruleType == "nop")
                {
                    var RuleToChange = gameRuleObjects[id-1];
                    RuleToChange.RuleType = ChangeType(RuleToChange);

                    accumulator = 0;
                    List<int> duplicateTest = new List<int>();
                    for (var x = 0; x < gameRuleObjects.Count; x++)
                    {
                        var currentObj = gameRuleObjects[x];
                        if (!duplicateTest.Contains(currentObj.RuleId))
                        {
                            if(currentObj.RuleId == 611)
                            {
                                return accumulator;
                            }
                            else
                            {
                                duplicateTest.Add(currentObj.RuleId);
                                var rules = RunRules(currentObj);
                                accumulator += rules[0];
                                x += rules[1];
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    RuleToChange.RuleType = ChangeType(RuleToChange);
                }
            }
            return accumulator;
        }

        public static int[] RunRules(GameRule rule)
        {
            switch (rule.RuleType)
            {
                case "acc":
                    int[] accValues = { rule.RuleValue, 0 };
                    return accValues;
                    break;
                case "jmp":
                    int[] jmpValues = { 0, rule.RuleValue -1 };
                    return jmpValues;
                    break;
                case "nop":
                    int[] nopValues = { 0, 0 };
                    return nopValues;                  
                    break;
                default:
                    int[] values = { 0, 0 };
                    return values;
                    break;
            }
        }

        public static string ChangeType(GameRule rule)
        {
            if (rule.RuleType == "jmp")
            {
                return "nop";
            }
            else return "jmp";
        }
            
                





            

            

        public class GameRule
        {   
            private static int m_Counter = 0;
            private string _ruleType;
            private int _ruleValue;

            public int RuleId { get; set; }
            public string RuleType 
            {
                get { return _ruleType; }
                set { _ruleType = value; } 
            }
            public int RuleValue
            {
                get { return _ruleValue; }
                set { _ruleValue = value; }
            }

            public GameRule(string ruleType, int ruleValue)
            {
                this.RuleId = System.Threading.Interlocked.Increment(ref m_Counter);
                _ruleType = ruleType;
                _ruleValue = ruleValue;
            }

        }
    }
}
