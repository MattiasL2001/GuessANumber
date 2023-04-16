using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessANumber
{
    public class Score
    {
        public int Result;
        public string Name;
        public string Time;

        public Score(int result, string name, string time)
        {
            Result = result;
            Name = name;
            Time = time;
        }
    }
}
