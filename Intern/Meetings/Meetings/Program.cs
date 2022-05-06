using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Meetings
{
    public enum Category
    {
        CodeMonkey = 1,
        Hub = 2,
        Short = 3,
        TeamBuilding = 4
    }

    public enum Type
    {
        Live = 1,
        InPerson = 2
    }

    internal class Program
    {

        static void Main(string[] args)
        {
            bool loopContinue = true;

            List<Meet> meetList = new List<Meet>();
            meetList = Task.ReadJson();

            Task.PrintStart();
            Console.WriteLine("Please enter your name");
            string userName = Console.ReadLine();
            Console.WriteLine();

            while (loopContinue)
            {
                Console.WriteLine("Enter a command");
                string input = Console.ReadLine();
                string[] seperateInput = Regex.Matches(input, @"\'.*?\'|[\w-]+").Cast<Match>().Select(m => m.Value).ToArray();

                for (int i = 0; i < seperateInput.Count(); i++)
                {
                    seperateInput[i] = seperateInput[i].Replace("'", "");
                }

                if (seperateInput.Count() == 0)
                {
                    Console.WriteLine("Please enter something");
                    continue;
                }

                switch (seperateInput[0].ToString().ToLower())
                {
                    case "create":

                        if (seperateInput.Count() != 8)
                        {
                            Console.WriteLine("Arguments are wrong");
                            break;
                        }

                        else
                        {
                            Task.CreateMeet(seperateInput, meetList);
                            break;
                        }

                    case "add":

                        if (seperateInput.Count() != 4)
                        {
                            Console.WriteLine("Arguments are wrong");
                            break;
                        }

                        else
                        {
                            Task.AddPerson(seperateInput, meetList);
                            break;
                        }

                    case "delete":

                        if (seperateInput.Count() != 2)
                        {
                            Console.WriteLine("Arguments are wrong");
                            break;
                        }

                        else
                        {
                            Task.DeleteMeet(seperateInput, meetList, userName);
                            break;
                        }

                    case "remove":

                        if (seperateInput.Count() != 3)
                        {
                            Console.WriteLine("Arguments are wrong");
                            break;
                        }

                        else
                        {
                            Task.RemovePerson(seperateInput, meetList);
                            break;
                        }

                    case "print":

                        Task.Print(seperateInput, meetList);
                        break;

                    case "save":

                        Task.Save(meetList);
                        loopContinue = false;
                        break;

                    case "exit":

                        loopContinue = false;
                        break;

                    case "info":
                        Task.PrintStart();
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}
