using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace DS
{
    public class DataSourceXml
    {
        private static XElement nannyRoot = null;
        private static string nannyPath = @"NanniesXml.xml";
        private static XElement motherRoot = null;
        private static string motherPath = @"MothersXml.xml";
        private static XElement childrenRoot = null;
        private static string childrenPath = @"ChildrenXml.xml";
        private static XElement contractRoot = null;
        private static string contractPath = @"ContractsXml.xml";

        // Constructor
        static DataSourceXml()
        {
            if (!File.Exists(nannyPath))
            {
                CreateFile("Nannies" ,nannyPath);
            }

            if (!File.Exists(motherPath))
            {
                CreateFile("Mothers", motherPath);
            }
            if (!File.Exists(childrenPath))
            {
                CreateFile("Children", childrenPath);
            }
            if (!File.Exists(contractPath))
            {
                CreateFile("Contracts", contractPath);
            }
        }

        public static XElement Nannies
        {
            get
            {
                nannyRoot = LoadData(nannyPath);
                return nannyRoot;
            }
        }

        public static XElement Mothers
        {
            get
            {
                motherRoot = LoadData(motherPath);
                return motherRoot;
            }
        }

        public static XElement Children
        {
            get
            {
                childrenRoot = LoadData(childrenPath);
                return childrenRoot;
            }
        }

        public static XElement Contracts
        {
            get
            {
                contractRoot = LoadData(contractPath);
                return contractRoot;
            }
        }

        public static void SaveInNannies()
        {
            nannyRoot.Save(nannyPath);
        }

        public static void SaveInMothers()
        {
            motherRoot.Save(motherPath);
        }

        public static void SaveInChildren()
        {
            childrenRoot.Save(childrenPath);
        }

        public static void SaveInContracts()
        {
            contractRoot.Save(contractPath);
        }


        private static void CreateFile(string rootName ,string path)
        {
            XElement newRoot = new XElement(rootName);
            newRoot.Save(path);
        }

        private static XElement LoadData(string path)
        {
            XElement root;
            try
            {
                root = XElement.Load(path);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
            return root;
        }
    }
}
