using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Time
    {
        private int hour = 0;
        private int minutes = 0;

        public int Hour
        {
            get => hour;
            set
            {
                if (value >= 0 && value < 24)
                {
                    hour = value;
                }
            }
        }

        public int Minutes
        {
            get => minutes;
            set
            {
                if (value >= 0 && value < 60)
                {
                    minutes = value;
                }
            }
        }

        public override string ToString()
        {
            string result;
            if (Hour < 10)
            {
                result = "0" + Hour.ToString();
            }
            else
            {
                result = Hour.ToString();
            }
            result += ":";
            if (Minutes < 10)
            {
                result += "0" + Minutes.ToString();
            }
            else
            {
                result += Minutes.ToString();
            }
            return result;
        }

        public int CompareTo(Time other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var hourComparison = hour.CompareTo(other.hour);
            if (hourComparison != 0) return hourComparison;
            return minutes.CompareTo(other.minutes);
            }
    }
}
