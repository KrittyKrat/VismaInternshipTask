using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Meetings;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        List<Meet> meetList = new List<Meet>();
        string user = "Loyd Jr.";

        public List<Meet> GenerateMeet()
        {
            string[] input1 = { "create", "Very cool meeting", "Peter Griffin", "We will discuss some cool things here", "Hub", "Live", "2022-5-1 10:30", "2022-5-1 11:00" };
            string[] input2 = { "create", "Meetingest", "John D", "What is this meeting. I do not know...", "CodeMonkey", "InPerson", "2022-5-2 15:00", "2022-5-2 16:00" };
            string[] input3 = { "create", "123", "Loyd Surname", "Thing", "Hub", "Super Live", "2022-4-1 10:30", "2022-4-1 10:35" };
            string[] input4 = { "create", "TEAM", "Loyd Jr.", "Build that team!", "TeamBuilding", "Live", "2022-4-16 10:30", "2022-4-16 15:30" };

            string[] input5 = { "add", "Adam Reeve", "Meetingest", "2022-5-2 15:05" };
            string[] input6 = { "add", "Saul Goodman", "Meetingest", "2022-5-2 15:20" };

            meetList = Task.CreateMeet(input1, meetList);
            meetList = Task.CreateMeet(input2, meetList);
            meetList = Task.CreateMeet(input3, meetList);
            meetList = Task.CreateMeet(input4, meetList);

            meetList = Task.AddPerson(input5, meetList);
            meetList = Task.AddPerson(input6, meetList);

            Console.WriteLine();

            return meetList;
        }

        [TestMethod]
        public void CreateMeet()
        {
            meetList = GenerateMeet();
            Console.WriteLine("Amount of meetings created: ", meetList.Count());
            Assert.AreEqual(meetList.Count(), 3);
        }

        [TestMethod]
        public void DeleteMeeting()
        {
            string[] input1 = { "delete", "TEAM" };
            string[] input2 = { "delete", "NOT A TEAM" };
            string[] input3 = { "delete", "Meetingest" };

            meetList = GenerateMeet();

            meetList = Task.DeleteMeet(input1, meetList, user);
            meetList = Task.DeleteMeet(input2, meetList, user);
            meetList = Task.DeleteMeet(input3, meetList, user);

            Assert.AreEqual(meetList.Count(), 2);
        }

        [TestMethod]
        public void AddPerson()
        {
            string[] input1 = { "add", "Markus J", "Meetingest", "2022-5-2 15:00" };
            string[] input2 = { "add", "Markus J", "Meetingest", "2022-5-2 15:10" };
            string[] input3 = { "add", "Paul", "TEAM", "2022-4-16 10:30" };
            string[] input4 = { "add", "Paul", "Very cool meeting", "2022-12-30 10:30" };

            meetList = GenerateMeet();

            meetList = Task.AddPerson(input1, meetList);
            meetList = Task.AddPerson(input2, meetList);
            meetList = Task.AddPerson(input3, meetList);
            meetList = Task.AddPerson(input4, meetList);

            Assert.AreEqual(meetList[0].People.Count(), 1);
            Assert.AreEqual(meetList[1].People.Count(), 4);
            Assert.AreEqual(meetList[2].People.Count(), 2);
        }

        [TestMethod]
        public void RemovePerson()
        {
            string[] input1 = { "remove", "Adam Reeve", "Meetingest" };
            string[] input2 = { "remove", "Saul Goodman", "Meetingest" };
            string[] input3 = { "remove", "John D", "Meetingest" };

            meetList = GenerateMeet();

            meetList = Task.RemovePerson(input1, meetList);
            meetList = Task.RemovePerson(input2, meetList);
            meetList = Task.RemovePerson(input3, meetList);

            Assert.AreEqual(meetList[1].People.Count(), 1);
        }

        [TestMethod]
        public void Print()
        {
            string[] input1 = { "print", "responsible", "Peter" };
            string[] input2 = { "print", "from", "3" };

            meetList = GenerateMeet();

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            Task.Print(input1, meetList);
            Assert.AreEqual("Very cool meeting, Peter Griffin, We will discuss some cool things here, Hub, Live, 2022-05-01 (10:30), 2022-05-01 (11:00), People: Peter Griffin, \r\n", stringWriter.ToString());
            stringWriter.Close();

            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            Task.Print(input2, meetList);
            Assert.AreEqual("Meetingest, John D, What is this meeting. I do not know..., CodeMonkey, InPerson, 2022-05-02 (15:00), 2022-05-02 (16:00), People: John D, Adam Reeve, Saul Goodman, \r\n", stringWriter.ToString());
        } 
    }
}
