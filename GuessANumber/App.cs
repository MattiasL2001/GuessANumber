using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        void WriteToFile(string filePath, int score)
        {
            string fileContain = "";
            var scoresList = new List<string>();
            var intScoresList = new List<int>();

            if (File.Exists(filePath))
            {
                fileContain = File.ReadAllText(filePath);
            }
            else
            {
                File.WriteAllText(filePath, "");
            }

            if (fileContain == "")
            {
                scoresList.Add(score.ToString());
            }
            else
            {
                fileContain += "," + score;
                Console.WriteLine("file contain:" + fileContain);
                scoresList = fileContain.Split(",").ToList();
                scoresList.ForEach(x => Console.WriteLine("score list: " + x));
            }

            foreach (var element in scoresList) { intScoresList.Add(Convert.ToInt32(element)); }
            string str = SortArray(intScoresList);
            File.WriteAllText(filePath, str);
        }

        string SortArray(List<int> list)
        {
            for (int s = 0; s < list.Count; s++)
            {
                if (s + 1 < list.Count && list[s] > list[s + 1])
                {
                    var temporary = list[s];
                    list[s] = list[s + 1];
                    list[s + 1] = temporary;
                    SortArray(list);
                }
            }

            string stringbuilder = "";

            for (var i = 0; i < list.Count; i++)
            {
                stringbuilder += list[i].ToString();
                Console.WriteLine("item: " + i);
                Console.WriteLine("list count: " + list.Count);
                if (i + 1 < list.Count) { stringbuilder += ",\n"; };
            }
            return stringbuilder;
        }
    }
}
