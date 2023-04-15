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
        public void Run()
        {
            string filePath = "C:\\Users\\Matti\\OneDrive\\Skrivbord\\Highscore.txt";
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
                    WriteToFile(filePath, score);

                    Console.WriteLine("Correct!");
                    Console.WriteLine("Do you want to play again? (Y/N)");

                    answer = Console.ReadLine().ToLower();

                    if (answer == "n")
                    {
                        Console.WriteLine("Thanks for the play!");
                        Environment.Exit(0);
                    }

                    if (answer == "y") { Run(); }
                }
            }
        }

        List<Score> FileToList(string filePath)
        {
            List<string> list;
            var scoreList = new List<Score>();

            if (File.ReadAllLines(filePath).Contains("},{"))
            {
                list = File.ReadAllText(filePath).Split("},{").ToList();
            }
            else
            {
                list = File.ReadAllText(filePath).Split("}").ToList();
            }

            foreach (var _score in list)
            {
                if (_score != "")
                {
                    Console.WriteLine(_score.Split(",")[0].Replace("{", ""));
                    int result = Convert.ToInt32(_score.Split(",")[0].Replace("{", "").Trim());
                    string name = _score.Split(",")[1];
                    string time = _score.Split(",")[2];
                    var scoreObj = new Score(result, name, time);
                    scoreList.Add(scoreObj);
                }
            }

            return scoreList;
        }

        string ListToFile(List<Score> list)
        {
            var stringBuilder = "";
            for (int i = 0; i < list.Count; i++)
            {
                stringBuilder += "{\n   ";
                stringBuilder += list[i].Result + ",\n   ";
                stringBuilder += list[i].Name + ",\n   ";
                stringBuilder += list[i].Time + "\n}";

                if (list.Count > 1 && i + 1 < list.Count) { stringBuilder += ",\n"; }
            }
            return stringBuilder;
        }

        void WriteToFile(string filePath, int score)
        {
            var scoresList = new List<Score>();

            if (File.Exists(filePath) && File.ReadAllText(filePath) != "")
            {
                scoresList = FileToList(filePath);
            }
            scoresList.Add(new Score(score, "", DateTime.Now.ToString()));
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
