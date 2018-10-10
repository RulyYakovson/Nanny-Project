using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DalFactorySingleton
    {
        private static IDAL instance = null;
        private DalFactorySingleton()
        {
            //do not add nothing here
        }
        
        //factory method with singleton
        public static IDAL Instance
        {
            get
            {
                //singleton pattern
                if (instance == null)
                {
                    instance = new Dal_XML();
                }
                return instance;
            }
        }
    }
}
