using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DS
{
    public class DataSourceList
    {
        private static List<Nanny> nanniesList = new List<Nanny>();
        private static List<Mother> mothersList = new List<Mother>();
        private static List<Child> childrenList = new List<Child>();
        private static List<Contract> contractsList = new List<Contract>();

        public static List<Nanny> NanniesList { get => nanniesList; }
        public static List<Mother> MothersList { get => mothersList;}
        public static List<Child> ChildrenList { get => childrenList; }
        public static List<Contract> ContractsList { get => contractsList;}
    }
}
