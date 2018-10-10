using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Contract
    {
        public int ContractNumber { set; get; }
        public int NannyId { get; set; }
        public string NannyIdAndName { get; set; }
        public string ChildIdAndName { get; set; }
        public int ChildId { get; set; }
        public int MotherId { get; set; }
        public bool HadInterview { get; set; }
        public bool ContractSigned { get; set; }
        public double FinalPayment { get; set; }
        public double PayForHour { get; set; }
        public double PayForMonth { get; set; }
        public int RangeOfDistance { get; set; }
        public Payment TypeOfPayment { get; set; }
        private DateTime dateOfBeginning;
        public DateTime DateOfBeginning
        {
            get => dateOfBeginning;
            set
            {
                              if (value.CompareTo(DateOfEnd) >= 0)
                {
                    throw new Exception("Date of beginning must be earlier than date of end");
                }
                dateOfBeginning = value;
            }
        }
        private DateTime dateOfEnd;
        public DateTime DateOfEnd
        {
            get => dateOfEnd;
            set
            {
                if (value.CompareTo(dateOfBeginning) < 0)
                {
                    throw new Exception("Date of end must be later than date of beginning");
                }
                dateOfEnd = value;
            }
        }
        public string NumberAndSignatures
        {
            get
            {
                return string.Format("{0}  {1} {2}", ContractNumber, NannyIdAndName, ChildIdAndName);
            }
        }

        public Contract()
        {
            dateOfBeginning = DateTime.Now;
            DateOfEnd = DateTime.Now.AddYears(1);
        }

        public override string ToString()
        {
            string result;
            result = "Contract number: " + ContractNumber;
            result += "\nNanny: " + NannyIdAndName;
            result += "\nChild: " + ChildIdAndName;
            result += "\nMother id: " + MotherId;
            result += "\nInterview meeting: " + HadInterview;
            result += "\nContract was signed: " + ContractSigned;
            result += "\nFinal payment: " + FinalPayment;
            result += "\nType of payment: " + TypeOfPayment;
            switch (TypeOfPayment)
            {
                case Payment.HOURLY:
                    result += "\npay for hour: " + PayForHour;
                    break;
                case Payment.MONTHLY:
                    result += "\nPay for month: " + PayForMonth;
                    break;
                default:
                    break;
            }
            result += "\nDate of beginning: " + DateOfBeginning.ToShortDateString();
            result += "\nDate of end: " + DateOfEnd.ToShortDateString();
            return result;
        }
    }
}
