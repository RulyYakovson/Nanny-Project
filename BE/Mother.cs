using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Mother
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                if (value >= 100000000 && value <= 999999999)
                    id = value;
                else
                    throw new Exception("Illegal id number");
            }
        }
        public string IdAndName
        {
            get
            {
                return string.Format("{0}  {1} {2}", Id, FirstName, LastName);
            }
        }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int PhonNumber { get; set; }
        private string address;
        private string locationForNanny;
        public string Address
        {
            get => address;
            set
            {
                address = value;
                if (address == null && locationForNanny == null)
                {
                    throw new Exception("From the fields address and location for nanny, must mark at least one");
                }
            }
        }
        public string LocationForNanny
        {
            get => locationForNanny;
            set
            {
                locationForNanny = value;
                if (address == null && locationForNanny == null)
                {
                    throw new Exception("From the fields address and location for nanny, must mark at least one");
                }
            }
        }
        public bool[] NeededDays { get; set; }
        public Day[] NeededHours { get; set; }
        public string Comments { get; set; }
        public int WantedRange { get; set; }

        public Mother()
        {
            Address = "";
            locationForNanny = "";
            NeededDays = new bool[6];
            NeededHours = new Day[]
            {
                new Day{TheDay = "Sunday",    BeginningTime = new TimeSpan(7,30,0),EndTime = new TimeSpan(13, 0, 0) },
                new Day{TheDay = "Monday",    BeginningTime = new TimeSpan(7,30,0),EndTime = new TimeSpan(13, 0, 0) },
                new Day{TheDay = "Tuesday",   BeginningTime = new TimeSpan(7,30,0),EndTime = new TimeSpan(13, 0, 0) },
                new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(7,30,0),EndTime = new TimeSpan(13, 0, 0) },
                new Day{TheDay = "Thursday",  BeginningTime = new TimeSpan(7,30,0),EndTime = new TimeSpan(13, 0, 0) },
                new Day{TheDay = "Friday",    BeginningTime = new TimeSpan(7,30,0),EndTime = new TimeSpan(13, 0, 0) },
            };
        }


        public override string ToString()
        {
            int index = 0;
            string result = "";
            result += "Id: " + Id;
            result += "\nLast name: " + LastName;
            result += "\nFirst name: " + FirstName;
            result += "\nPhonNumber: " + PhonNumber;
            result += "\nAddress: " + Address;
            result += "\nLocation for nanny: " + LocationForNanny;
            result += "\nWanted days: ";
            result += "\nWanted days and hours:\n";
            foreach (var item in NeededHours)
            {
                if (NeededDays[index++])
                {
                    result += item + ",\n";
                }
            }
            result += "Comments: " + Comments;
            return result;
        }
    }
}
