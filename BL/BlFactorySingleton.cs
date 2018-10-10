using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BlFactorySingleton
    {
        private static IBL instance = null;
        private BlFactorySingleton()
        {
            //do not add nothing here
        }

        //factory method with singleton
        public static IBL Instance
        {
            get
            {
                //singleton pattern
                if (instance == null)
                {
                    instance = new Bl_imp();
                }
                return instance;
            }
        }
    }
}

