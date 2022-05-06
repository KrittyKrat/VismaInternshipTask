using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meetings
{
    public class Meet
    {
        public string Name { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> People { get; set; }
        public Meet(string name, string responsiblePerson, string description, string category, string type, DateTime startDate, DateTime endDate)
        {
            Name = name;
            ResponsiblePerson = responsiblePerson;
            Description = description;
            Category = category;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            People = new List<string>();
            People.Add(responsiblePerson);
        }

        public void DeletePerson(string name)
        {
            People.Remove(name);
        }

        public bool CheckName(string name)
        {
            return name == Name;
        }

        public bool CheckPerson(string name)
        {
            return People.Contains(name);
        }

        public bool CheckDate(DateTime date)
        {
            
            if (StartDate <= date && EndDate >= date)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public bool CheckBetweenDates(DateTime date1, DateTime date2)
        {
            if (date1 <= StartDate && date2 >= EndDate)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public bool CheckFromDate(DateTime date)
        {
            if (date <= StartDate)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public bool CheckToDate(DateTime date)
        {
            if (date >= StartDate)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public void AddPerson(string name)
        {
            if (!CheckPerson(name))
            {
                People.Add(name);
            }

            else
            {
                Console.WriteLine("{0} is already in this meeting", name);
                return;
            }
        }

        public void Print()
        {
            Console.Write("{0}, {1}, {2}, {3}, {4}, {5}, {6}, People: ", Name, ResponsiblePerson, Description, Category, Type, StartDate.ToString("yyyy-MM-dd (HH:mm)"), EndDate.ToString("yyyy-MM-dd (HH:mm)"));

            foreach (var person in People)
            {
                Console.Write("{0}, ", person);
            }

            Console.WriteLine();
        }
    }
}
