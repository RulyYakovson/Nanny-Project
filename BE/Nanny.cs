using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Nanny
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
        private DateTime dateOfBirth;
        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                if (value.CompareTo(DateTime.Now) == 1)
                {
                    throw new Exception("Cannot insert future date");
                }
                if (value.CompareTo(DateTime.Now.AddYears(-18)) == 1)
                {
                    throw new Exception("A nanny cannot be under the age of 18");
                }
                else
                {
                    dateOfBirth = value;
                }
            }
        }
        public int PhonNumber { get; set; }
        public string Address { get; set; }
        public bool Elevator { get; set; }
        public int Floor { get; set; }
        private int yearsOfExperience;
        public int YearsOfExperience
        {
            get => yearsOfExperience;
            set
            {
                if ((DateTime.Now.Year - dateOfBirth.Year - 18) < value)
                {
                    throw new Exception("Nanny too young for such an experience");
                }
                yearsOfExperience = value;
            }
        }
        public int MaxChildren { get; set; }
        private int minimumAgeOfChild;
        public int MinimumAgeOfChild
        {
            get => minimumAgeOfChild;
            set
            {
                if (value < 3)
                {
                    throw new Exception("The Minimum Age Of Child cannot be under than 3 month");
                }
                if (value > maximumAgeOfChild)
                {
                    throw new Exception("Minimum age must be smaller than maximum age");
                }
                minimumAgeOfChild = value;
            }
        }
        private int maximumAgeOfChild;
        public int MaximumAgeOfChild
        {
            get => maximumAgeOfChild;
            set
            {
                if (value < minimumAgeOfChild)
                {
                    throw new Exception("Maximum age must be bigger than minimum age");
                }
                maximumAgeOfChild = value;
            }
        }
        public Payment TypeOfPayment { get; set; }
        public double HourlyRate { get; set; }
        public double MonthlyRate { get; set; }
        public bool[] WorkDays { get; set; }
        public Day[] WorkHours { get; set; }
        public VacationDaysBy VacationDays { get; set; }
        public string Recommendations { get; set; }

        public Nanny()
        {
            minimumAgeOfChild = 3;
            maximumAgeOfChild = 24;
            WorkDays = new bool[6];
            WorkHours = new Day[]
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
            string result;
            result = "\nId: " + Id;
            result += "\nLast name: " + LastName;
            result += "\nFirst name: " + FirstName;
            result += "\nDate of birth: " + DateOfBirth.ToShortDateString();
            result += "\nPhon number: " + PhonNumber;
            result += "\nAddress: " + Address;
            result += "\nElevator: " + Elevator;
            result += "\nFloor: " + Floor;
            result += "\nYears of experience: " + YearsOfExperience;
            result += "\nMaximum children: " + MaxChildren;
            result += "\nMinimum age of child: " + MinimumAgeOfChild + " month";
            result += "\nMaximum age of child: " + MaximumAgeOfChild + " month";
            result += "\nType of payment: " + TypeOfPayment;
            result += "\nHourly rate: " + HourlyRate;
            result += "\nMonthly rate: " + MonthlyRate;
            result += "\nWork days: ";
            result += "\nWork days and hours:\n";
            foreach (var item in WorkHours)
            {
                if (WorkDays[index++])
                {
                    result += item + ",\n";
                }
            }
            result += "Vacation days: " + VacationDays;
            if (Recommendations != "")
            {
                result += "\nRecommendations: " + Recommendations;

            }
            return result;
        }
    }
}