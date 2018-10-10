using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BE;
using DS;

namespace DAL
{
    internal class Dal_XML : IDAL
    {
        const int FIRST_SERIAL_CONTRACT = 10000000;

        public void AddNanny(Nanny nanny)
        {
            // Searching for the nanny ID in the nanny's XML file (returns 0 if not found)
            int temp = (from n in DataSourceXml.Nannies.Elements()
                        where int.Parse(n.Element("Id").Value) == nanny.Id
                        select int.Parse(n.Element("Id").Value)).FirstOrDefault();

            if (temp != 0)
            {
                throw new Exception("ERROR: The nanny with id: " + nanny.Id + " already exist in the database !!!");
            }
            else
            {
                DataSourceXml.Nannies.Add(nanny.toXML());
                DataSourceXml.SaveInNannies();
            }
        }

        public void RemoveNanny(int nannyId)
        {
            // Searching for the nanny instance in the nanny's XML file (returns null if not found)
            XElement nannyToRemove = (from n in DataSourceXml.Nannies.Elements()
                                      where int.Parse(n.Element("Id").Value) == nannyId
                                      select n).FirstOrDefault();

            if (nannyToRemove == null)
            {
                throw new Exception("ERROR: The nanny with id: " + nannyId + " not exist in the database !!!");
            }
            else
            {
                nannyToRemove.Remove();
                DataSourceXml.SaveInNannies();
            }
        }

        public void AddMother(Mother mother)
        {
            // Searching for the mother ID in the mother's XML file (returns 0 if not found)
            int temp = (from n in DataSourceXml.Mothers.Elements()
                        where int.Parse(n.Element("Id").Value) == mother.Id
                        select int.Parse(n.Element("Id").Value)).FirstOrDefault();

            if (temp != 0)
            {
                throw new Exception("ERROR: The mother with id: " + mother.Id + " already exist in the database !!!");
            }
            else
            {
                DataSourceXml.Mothers.Add(mother.toXML());
                DataSourceXml.SaveInMothers();
            }
        }

        public void AddChild(Child child)
        {
            // Searching for the child ID in the children XML file (returns 0 if not found)
            int temp = (from n in DataSourceXml.Children.Elements()
                        where int.Parse(n.Element("Id").Value) == child.Id
                        select int.Parse(n.Element("Id").Value)).FirstOrDefault();

            if (temp != 0)
            {
                throw new Exception("ERROR: The child with id: " + child.Id + " already exist in the database !!!");
            }
            else
            {
                DataSourceXml.Children.Add(child.toXML());
                DataSourceXml.SaveInChildren();
            }
        }

        public void AddContract(Contract contract, bool update)
        {
            // Searching for the contract number in the contract's XML file (returns 0 if not found)
            int temp = (from n in DataSourceXml.Contracts.Elements()
                        where int.Parse(n.Element("ContractNumber").Value) == contract.ContractNumber
                        select int.Parse(n.Element("ContractNumber").Value)).FirstOrDefault();

            if (temp != 0)
            {
                throw new Exception("ERROR: It's not a new contract !!!");
            }
            else
            {
                if (!update) // Makes sure that we don't updates an exist contract
                {
                    int theLastContractNumber = FIRST_SERIAL_CONTRACT;
                    if (DataSourceXml.Contracts.Elements().Any())
                    {
                        // Finds the number of the last contract that added
                        theLastContractNumber = (from n in DataSourceXml.Contracts.Elements()
                                                 select int.Parse(n.Element("ContractNumber").Value)).Max();
                    }
                    contract.ContractNumber = theLastContractNumber + 1; // Update the current contract number 
                }
                DataSourceXml.Contracts.Add(contract.toXML());
                DataSourceXml.SaveInContracts();
            }
        }

        public void RemoveMother(int motherId)
        {
            // Searching for the mother instance in the mother's XML file (returns null if not found)
            XElement motherToRemove = (from n in DataSourceXml.Mothers.Elements()
                                       where int.Parse(n.Element("Id").Value) == motherId
                                       select n).FirstOrDefault();

            if (motherToRemove == null)
            {
                throw new Exception("ERROR: The mother with id: " + motherId + " not exist in the database !!!");
            }
            else
            {
                motherToRemove.Remove();
                DataSourceXml.SaveInMothers();
            }
        }

        public void RemoveChild(int childId)
        {
            // Searching for the child instance in the children XML file (returns null if not found)
            XElement childToRemove = (from n in DataSourceXml.Children.Elements()
                                      where int.Parse(n.Element("Id").Value) == childId
                                      select n).FirstOrDefault();

            if (childToRemove == null)
            {
                throw new Exception("ERROR: The child with id: " + childId + " not exist in the database !!!");
            }
            else
            {
                childToRemove.Remove();
                DataSourceXml.SaveInChildren();
            }
        }

        public void RemoveContract(int contractNumber)
        {
            // Searching for the contract instance in the contract's XML file (returns null if not found)
            XElement contractToRemove = (from n in DataSourceXml.Contracts.Elements()
                                         where int.Parse(n.Element("ContractNumber").Value) == contractNumber
                                         select n).FirstOrDefault();

            if (contractToRemove == null)
            {
                throw new Exception("ERROR: The contract with serial number: " + contractNumber + " not exist in the database !!!");
            }
            else
            {
                contractToRemove.Remove();
                DataSourceXml.SaveInContracts();
            }
        }

        public Child FindChild(int childId)
        {
            Child child = (from n in DataSourceXml.Children.Elements()
                           where int.Parse(n.Element("Id").Value) == childId
                           select n).FirstOrDefault().toChildInstance();

            return child;
        }

        public Contract FindContract(int contractNumber)
        {
            Contract contract = (from n in DataSourceXml.Contracts.Elements()
                                 where int.Parse(n.Element("ContractNumber").Value) == contractNumber
                                 select n).FirstOrDefault().toContractInstance();
            return contract;
        }

        public Mother FindMother(int motherId)
        {
            Mother mother = (from n in DataSourceXml.Mothers.Elements()
                             where int.Parse(n.Element("Id").Value) == motherId
                             select n).FirstOrDefault().toMotherInstance();
            return mother;
        }

        public Nanny FindNanny(int nannyId)
        {
            Nanny theNanny = (from n in DataSourceXml.Nannies.Elements()
                              where int.Parse(n.Element("Id").Value) == nannyId
                              select n).FirstOrDefault().toNannyInstance();

            return theNanny;
        }

        public IEnumerable<Child> GetChildren(Func<Child, bool> predicate = null)
        {
            //if (DataSourceXml.Children.Elements().Any() == false) // The file of children are empty
            //{
            //    return null;
            //}
            // Builds from the file a collection of child's instance 
            IEnumerable<Child> children = from c in DataSourceXml.Children.Elements()
                                          select c.toChildInstance();

            if (predicate != null)
            {
                // Filters the collection
                children = children.Where(predicate);
            }
            return children;
        }

        public IEnumerable<IGrouping<int, Child>> GetChildrenByMothers()
        {
            IEnumerable<Child> temp = GetChildren();
            var children = from child in temp
                           group child by (child.MotherId) into g
                           select g;

            return children;
        }

        public IEnumerable<Contract> GetContracts(Func<Contract, bool> predicate = null)
        {
            // Builds from the file a collection of contract's instance 
            IEnumerable<Contract> contracts = from c in DataSourceXml.Contracts.Elements()
                                              orderby int.Parse(c.Element("ContractNumber").Value)
                                              select c.toContractInstance();

            if (predicate != null)
            {
                // Filters the collection
                contracts = contracts.Where(predicate);
            }
            return contracts;
        }

        public IEnumerable<Mother> GetMothers(Func<Mother, bool> predicate = null)
        {
            // Builds from the file a collection of mother's instance 
            IEnumerable<Mother> mothers = from c in DataSourceXml.Mothers.Elements()
                                          select c.toMotherInstance();

            if (predicate != null)
            {
                // Filters the collection
                mothers = mothers.Where(predicate);
            }
            return mothers;
        }

        public IEnumerable<Nanny> GetNannies(Func<Nanny, bool> predicate = null)
        {
            // Builds from the file a collection of nanny's instance 
            IEnumerable<Nanny> nannies = from c in DataSourceXml.Nannies.Elements()
                                         select c.toNannyInstance();

            if (predicate != null)
            {
                // Filters the collection
                nannies = nannies.Where(predicate);
            }
            return nannies;
        }

        public void UpdateChild(Child child)
        {
            // Removes the old element
            RemoveChild(child.Id);
            // Updating the element
            AddChild(child);
        }

        public void UpdateContract(Contract contract)
        {
            // Removes the old element
            RemoveContract(contract.ContractNumber);
            // Updating the element
            AddContract(contract, true);
        }

        public void UpdateMother(Mother mother)
        {
            // Removes the old element
            RemoveMother(mother.Id);
            // Updating the element
            AddMother(mother);
        }

        public void UpdateNanny(Nanny nanny)
        {
            // Removes the old element
            RemoveNanny(nanny.Id);
            // Updating the element
            AddNanny(nanny);
        }
    }
}
