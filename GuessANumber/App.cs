using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GuessANumber
{
    public class App
    {
        string filePath = "C:\\Users\\Matti\\OneDrive\\Skrivbord\\Highscore.txt";
        public void Run()
        {
            Console.WriteLine("Input 1 to play");
            Console.WriteLine("Input 2 to see highscore list");
            Console.WriteLine("Input 3 to change file directory");
            Console.WriteLine("Input 4 to quit");
            string answer = Console.ReadLine();

            switch (answer)
            {
                case "1":
                    GuessANumber();
                    break;
                case "2":
                    List<Score> list = FileToList(filePath);
                    Console.WriteLine("Hgihscore list:");
                    list.ForEach(x => Console.WriteLine($"Result: {x.Result}, Name: {x.Name}, Time: {x.Time}"));
                    Console.WriteLine();
                    break;
                case "3":
                    ChangeFileDirectory();
                    break;
                case "4":
                    Console.WriteLine("Thanks for playing!");
                    Environment.Exit(0);
                    break;
            }
            Run();
        }

        void ChangeFileDirectory()
        {
            Console.WriteLine("Enter an existing directory!");
            Console.WriteLine("Input 'Back' to go to the main menu.");
            string directory = Console.ReadLine();
            string fileName;

            if (directory.ToLower() == "back") { Run(); }

            if (Directory.Exists(directory))
            {
                Console.WriteLine("Enter a file name:");
                fileName = Console.ReadLine();
                try
                {
                    File.WriteAllText(directory + "\\" + fileName + ".txt", "");
                }
                catch
                {
                    Console.WriteLine("Access denied to that directory, choose another one:");
                    Console.WriteLine(new UnauthorizedAccessException());
                    ChangeFileDirectory();
                }

                filePath = directory + "\\" + fileName + ".txt";
                Console.WriteLine("Directory changed to:");
                Console.WriteLine(filePath);
            }
            else
            {
                Console.WriteLine("You must enter a VALID directory that exists!");
                ChangeFileDirectory();
            }
        }

        void GuessANumber()
        {
            int score = 0;

            int number = new Random().Next(1, 101);
            Console.WriteLine("Guess a number between 1 and 100!");

            while (true)
            {
                string answer = Console.ReadLine();
                int intAnswer;
                score++;

                if (!int.TryParse(answer, out intAnswer))
                {
                    Console.WriteLine("You must input a number nothing else!");
                    score--;
                }

                else if (intAnswer > 100) { Console.WriteLine("The highest possible number is 100!"); }

                else if (intAnswer < number) { Console.WriteLine("Too low!"); }

                else if (intAnswer > number) { Console.WriteLine("Too high!"); }

                else if (intAnswer == number)
                {
                    Console.WriteLine("Correct!");
                    WriteToFile(filePath, score);
                    Run();
                }
            }
        }

        List<Score> FileToList(string filePath)
        {
            List<string> list;
            var scoreList = new List<Score>();

            string content = File.ReadAllText(filePath);
            content = content.Replace("},{", "");
            content = content.Replace("}", "");
            content = content.Replace("\n", "");
            content = content.Replace("{", "");
            content = content.Replace("   ", "");
            content = content.Trim();
            string[] elements = content.Split(",");
            int objCount = elements.Length / 3;

            for (int i = 0; i < objCount; i++)
            {
                int result = Convert.ToInt32(elements[0 + i*3]);
                string name = elements[1 + i*3];
                string time = elements[2 + i*3];
                scoreList.Add(new Score(result, name, time));
            }

            return scoreList;
        }

        string ListToFile(List<Score> list)
        {
            var stringBuilder = "";
            for(var i = 0; i < list.Count; i++)
            {
                stringBuilder += "{\n   ";
                stringBuilder += list[i].Result + ",\n   ";
                stringBuilder += list[i].Name + ",\n   ";
                stringBuilder += list[i].Time + "\n}";

                if (list.Count > 1 && i + 1 < list.Count) { stringBuilder += ",\n"; }
            }
            return stringBuilder;
        }

        bool IsNameValid(string name)
        {
            bool isNameValid = true;
            string[] forbiddenCharacters = { "{", ",", "}" };
            List<string> forbiddenCharList = forbiddenCharacters.ToList();

            forbiddenCharList.ForEach(x =>
            {
                if (name.Contains(x))
                {
                    Console.WriteLine("Name can not contain the following characters:");
                    forbiddenCharList.ForEach(x => Console.WriteLine(x));
                    isNameValid = false;
                }
            });

            return isNameValid;
        }

        void WriteToFile(string filePath, int score)
        {
            var scoresList = new List<Score>();

            if (File.Exists(filePath) && File.ReadAllText(filePath) != "")
            {
                scoresList = FileToList(filePath);
            }
            if (scoresList.Count == 5 && score > scoresList[4].Result) { return; }

            Console.WriteLine("Your score are amongst the top 5!");

            if (scoresList.Count == 5) { scoresList.Remove(scoresList[4]); }

            string name = "";
            while(true)
            {
                Console.WriteLine("Enter your name below!");
                name = Console.ReadLine();
                if (IsNameValid(name)) { break; }
            }
            scoresList.Add(new Score(score, name, DateTime.Now.ToString()));
            File.WriteAllText(filePath, ListToFile(SortArray(scoresList)));
        }

        List<Score> SortArray(List<Score> list)
        {
            for (int s = 0; s < list.Count; s++)
            {
                if (s + 1 < list.Count && list[s].Result > list[s + 1].Result)
                {
                    var temporary = list[s];
                    list[s] = list[s + 1];
                    list[s + 1] = temporary;
                    SortArray(list);
                }
            }
            return list;
        }
    }
}
