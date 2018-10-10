using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Day
    {
        public string TheDay { get; set; }
        public TimeSpan BeginningTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public override string ToString()
        {
            return TheDay + ": " + BeginningTime + " to " + EndTime;
        }
    }
}
