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
    public class Task
    {
        public static bool ValidateDate(string text)
        {
            DateTime tempDate;
            return DateTime.TryParse(text, out tempDate);
        }
        public static void PrintStart()
        {
            Console.WriteLine("IMPORTANT: Use SPACE between single word commands. For multiple words put them between ''");
            Console.WriteLine("Example: remove Peter 'Work overview'");
            Console.WriteLine();

            Console.WriteLine("--Commands--");
            Console.WriteLine();

            Console.WriteLine("Create a meeting:");
            Console.WriteLine("Create (MEET NAME) (RESPONSIBLE PERSON NAME) (DESCRIPTION) (CATEGORY) (TYPE) (START DATE) (END DATE)");

            Console.Write("Possible meeting categories: ");
            foreach (var item in Enum.GetValues(typeof(Category)))
            {
                Console.Write("{0}, ", item);
            }
            Console.WriteLine();

            Console.Write("Possible meeting types: ");
            foreach (var item in Enum.GetValues(typeof(Type)))
            {
                Console.Write("{0}, ", item);
            }
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Delete a meeting:");
            Console.WriteLine("Delete (MEET NAME)");
            Console.WriteLine();

            Console.WriteLine("Add a person to a meeting:");
            Console.WriteLine("Add (NAME) (MEET NAME) (TIME)");
            Console.WriteLine();

            Console.WriteLine("Remove a person from a meeting:");
            Console.WriteLine("Remove (NAME) (MEET NAME)");
            Console.WriteLine();

            Console.WriteLine("Print and filter results:");
            Console.WriteLine("Print | Print (description, responsible, category, type, date from, date to, date between, from , to, equals) (SEARCH TERM)");
            Console.WriteLine("Date format example: '2021-05-06 15:30'");
            Console.WriteLine();

            Console.WriteLine("Save changes and close program:");
            Console.WriteLine("Save");
            Console.WriteLine();

            Console.WriteLine("Close program without saving changes:");
            Console.WriteLine("Exit");
            Console.WriteLine();

            Console.WriteLine("Provide this info screen again:");
            Console.WriteLine("Info");
            Console.WriteLine("------------");
            Console.WriteLine();
        }

        public static List<Meet> ReadJson()
        {
            List<Meet> temp = new List<Meet>();
            string path = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Output.json";

            if (File.Exists(path))
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    temp = JsonSerializer.Deserialize<List<Meet>>(json);
                }
            }

            else
            {
                File.Create(path);
            }

            return temp;
        }

        public static List<Meet> CreateMeet(string[] seperateInput, List<Meet> meetList)
        {
            if (!Enum.IsDefined(typeof(Category), seperateInput[4]))
            {
                Console.Write("Please chose one of these valid meeting categories: ");

                foreach (var item in Enum.GetValues(typeof(Category)))
                {
                    Console.Write("{0}, ", item);
                }

                Console.WriteLine("");
            }

            else if (!Enum.IsDefined(typeof(Type), seperateInput[5]))
            {
                Console.Write("Please chose one of these valid meeting types: ");

                foreach (var item in Enum.GetValues(typeof(Type)))
                {
                    Console.Write("{0}, ", item);
                }

                Console.WriteLine("");
            }

            else if (!ValidateDate(seperateInput[6]) || !ValidateDate(seperateInput[7]))
            {
                Console.WriteLine("Please enter a valid date");
            }

            else
            {
                meetList.Add(new Meet(seperateInput[1], seperateInput[2], seperateInput[3], seperateInput[4], seperateInput[5], DateTime.Parse(seperateInput[6]), DateTime.Parse(seperateInput[7])));
                Console.WriteLine("--created--");
            }

            return meetList;
        }

        public static List<Meet> AddPerson(string[] seperateInput, List<Meet> meetList)
        {
            bool canJoin = false;

            if (!ValidateDate(seperateInput[3]))
            {
                Console.WriteLine("Please enter a valid date");
                return null;
            }

            for (int i = 0; i < meetList.Count(); i++)
            {
                if (meetList[i].CheckDate(DateTime.Parse(seperateInput[3])))
                {
                    if (meetList[i].CheckPerson(seperateInput[1]) && meetList[i].Name != seperateInput[2])
                    {
                        Console.WriteLine("{0} is already in a meeting at the specified time", seperateInput[1]);
                        canJoin = true;
                        break;
                    }

                    else if (meetList[i].CheckName(seperateInput[2]))
                    {
                        meetList[i].AddPerson(seperateInput[1]);
                        Console.WriteLine("{0} was added to the meeting", seperateInput[1]);
                        canJoin = true;
                        break;
                    }
                }
            }

            if (!canJoin)
            {
                Console.WriteLine("This meeting does not exist");
            }

            return meetList;
        }

        public static List<Meet> DeleteMeet(string[] seperateInput, List<Meet> meetList, string userName)
        {
            bool found = false;

            foreach (var meet in meetList)
            {
                if (meet.Name == seperateInput[1])
                {
                    if (meet.ResponsiblePerson != userName)
                    {
                        Console.WriteLine("Only the person who is responsible for the meeting can delete it");
                        found = true;
                    }

                    else
                    {
                        meetList.Remove(meet);
                        found = true;
                        Console.WriteLine("Meeting removed");
                        break;
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("Meeting does not exist");
            }

            return meetList;
        }

        public static List<Meet> RemovePerson(string[] seperateInput, List<Meet> meetList)
        {
            foreach (var meet in meetList)
            {
                if (meet.Name == seperateInput[2])
                {
                    if (!meet.CheckPerson(seperateInput[1]))
                    {
                        Console.WriteLine("{0} is not in this meeting", seperateInput[1]);
                    }

                    else if (meet.ResponsiblePerson == seperateInput[1])
                    {
                        Console.WriteLine("The person responsible can not be removed from the meeting");
                    }

                    else
                    {
                        meet.DeletePerson(seperateInput[1]);
                        Console.WriteLine("{0} was deleted", seperateInput[1]);
                    }
                }
            }

            return meetList;
        }

        public static void Print(string[] seperateInput, List<Meet> meetList)
        {
            if (seperateInput.Count() == 1)
            {
                foreach (var meet in meetList)
                {
                    meet.Print();
                }
            }

            else if (seperateInput.Count() == 3)
            {

                Regex rx = new Regex(seperateInput[2]);

                switch (seperateInput[1].ToString().ToLower())
                {
                    case "description":

                        foreach (var meet in meetList)
                        {
                            if (rx.IsMatch(meet.Description))
                            {
                                meet.Print();
                            }
                        }

                        break;

                    case "responsible":

                        foreach (var meet in meetList)
                        {
                            if (rx.IsMatch(meet.ResponsiblePerson))
                            {
                                meet.Print();
                            }
                        }

                        break;

                    case "category":

                        foreach (var meet in meetList)
                        {
                            if (rx.IsMatch(meet.Category))
                            {
                                meet.Print();
                            }
                        }

                        break;

                    case "type":

                        foreach (var meet in meetList)
                        {
                            if (rx.IsMatch(meet.Type))
                            {
                                meet.Print();
                            }
                        }

                        break;

                    case "from":

                        foreach (var meet in meetList)
                        {
                            if (Int32.Parse(seperateInput[2]) <= meet.People.Count())
                            {
                                meet.Print();
                            }
                        }

                        break;

                    case "to":

                        foreach (var meet in meetList)
                        {
                            if (Int32.Parse(seperateInput[2]) >= meet.People.Count())
                            {
                                meet.Print();
                            }
                        }

                        break;

                    case "equals":

                        foreach (var meet in meetList)
                        {
                            if (Int32.Parse(seperateInput[2]) == meet.People.Count())
                            {
                                meet.Print();
                            }
                        }

                        break;
                }
            }

           else if (seperateInput.Count() == 4 && seperateInput[1].ToString().ToLower() == "date")
            {
                if (!ValidateDate(seperateInput[3]))
                {
                    Console.WriteLine("Please enter a valid date");
                }

                switch (seperateInput[2].ToString().ToLower())
                {
                    case "from":

                        foreach (var meet in meetList)
                        {
                            if (meet.CheckFromDate(DateTime.Parse(seperateInput[3])))
                            {
                                meet.Print();
                            }
                        }

                        break;

                    case "to":

                        foreach (var meet in meetList)
                        {
                            if (meet.CheckToDate(DateTime.Parse(seperateInput[3])))
                            {
                                meet.Print();
                            }
                        }

                        break;
                }
            }

            else if (seperateInput.Count() == 5 && seperateInput[1].ToString().ToLower() == "date")
            {
                if (!ValidateDate(seperateInput[3]) || !ValidateDate(seperateInput[4]))
                {
                    Console.WriteLine("Please enter a valid date");
                }

                switch (seperateInput[2].ToString().ToLower())
                {
                    case "between":

                        foreach (var meet in meetList)
                        {
                            if (meet.CheckBetweenDates(DateTime.Parse(seperateInput[3]), DateTime.Parse(seperateInput[4])))
                            {
                                meet.Print();
                            }
                        }

                        break;
                }
            }

            else
            {
                Console.WriteLine("Arguments are wrong");
            }
        }

        public static void Save(List<Meet> meetList)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Output.json";
            string json = JsonSerializer.Serialize(meetList);
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                outputFile.WriteLine(json);
            }
        }
    }
}
