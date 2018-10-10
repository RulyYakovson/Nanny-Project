using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Child
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
                return string.Format("{0}  {1}", Id, FirstName);
            }
        }
        public int MotherId { get; set; }
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
                dateOfBirth = value;
            }
        }
        public string DateOfBirthString { get => dateOfBirth.ToShortDateString(); }
        public bool SpecialNeeds { get; set; }
        public string TheSpecialNeeds { get; set; }
        public bool ContractSigned { get; set; }

        public override string ToString()
        {
            string result = "ID: " + Id;
            result += "\nFirst name: " + FirstName;
            result += "\nMother ID: " + MotherId;
            result += "\nDate Of Birth :" + DateOfBirth.ToShortDateString();
            if (SpecialNeeds)
            {
                result += "\nSpecial needs: " + TheSpecialNeeds;
            }
            return result;
        }
    }
}
