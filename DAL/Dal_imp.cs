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
    internal class Dal_imp : IDAL
    {
        const int SERIAL_CONTRACT = 10000000;
        static int contractSerial = SERIAL_CONTRACT;

        public Nanny FindNanny(int nannyId)
        {
            // Returns the nanny or null (if the nanny was not founds)
            return DataSourceList.NanniesList.FirstOrDefault(n => n.Id == nannyId);
        }

        public void AddNanny(Nanny nanny)
        {
            if (FindNanny(nanny.Id) != null) // Makes sure that the nanny not exist already
            {
                throw new Exception("ERROR: The nanny with id: " + nanny.Id + " already exist in the database !!!");
            }
            else // Adds the nanny to the list
            {
                Nanny newNanny = nanny.Clone(); // Creates a new instance
                DataSourceList.NanniesList.Add(newNanny);
            }
        }

        public void RemoveNanny(int nannyId)
        {
            // Finds in the list the nanny to remove 
            Nanny nannyToRemove = FindNanny(nannyId);
            if (nannyToRemove == null) // Makes sure that the nanny exist in the list
            {
                throw new Exception("ERROR: The nanny with id: " + nannyId + " not exist in the database !!!");
            }
            else
            {
                DataSourceList.NanniesList.Remove(nannyToRemove); // Removes the nanny from the list
                // Removes the contracts of this nanny
                //??? DataSource.ContractsList.RemoveAll(c => c.NannyId == nanny.Id); 
            }
        }

        public void UpdateNanny(Nanny nanny)
        {
            // Finds in the list the index of the nanny to update
            int index = DataSourceList.NanniesList.FindIndex(n => n.Id == nanny.Id);
            if (index == -1) // The nanny was not exist
            {
                throw new Exception("ERROR: The nanny with id: " + nanny.Id + " not exist in the database !!!");
            }
            else // Apdating the nanny
            {
                Nanny newNanny = nanny.Clone(); // Creates a new instance
                DataSourceList.NanniesList[index] = newNanny;
            }
        }

        public IEnumerable<Nanny> GetNannies(Func<Nanny, bool> predicate = null)
        {
            if (DataSourceList.NanniesList.Count() == 0)
            {
                throw new Exception("ERROR: The list of nannies are empty !!!");
            }
            if (predicate == null) // Returns a new list with the all nannies 
            {
                return DataSourceList.NanniesList.Clone();
            }
            else // Returns only the wanted nannies
            {
                IEnumerable<Nanny> temp = DataSourceList.NanniesList.Where(predicate);
                if (temp.Count() == 0)
                {
                    throw new Exception("ERROR: There is no nanny who fits this condition !!!");
                }
                else
                {
                    return temp;
                }
            }
        }

        public Mother FindMother(int motherId)
        {
            // Returns the mother or null (if the mother was not founds)
            return DataSourceList.MothersList.FirstOrDefault(m => m.Id == motherId);
        }

        public void AddMother(Mother mother)
        {
            if (FindMother(mother.Id) != null) // Makes sure that the mother not exist already
            {
                throw new Exception("ERROR: The mother with id: " + mother.Id + " already exist in the database !!!");
            }
            else // Adds the mother to the list
            {
                Mother newMother = mother.Clone(); // Creates a new instance
                DataSourceList.MothersList.Add(newMother);
            }
        }

        public void RemoveMother(int motherId)
        {
            // Finds in the list the mother to remove 
            Mother motherToRemove = FindMother(motherId);
            if (motherToRemove == null) // Makes sure that the mother exist in the list
            {
                throw new Exception("ERROR: The mother with id: " + motherId + " not exist in the database !!!");
            }
            else
            {
                DataSourceList.MothersList.Remove(motherToRemove); // Removes the mother from the list
                                                                   // Removes the contracts of this mother
                                                                   //?? DataSource.ContractsList.RemoveAll(c => c.MotherId == mother.Id);
            }
        }

        public void UpdateMother(Mother mother)
        {
            // Finds in the list the index of the mother to update
            int index = DataSourceList.MothersList.FindIndex(m => m.Id == mother.Id);
            if (index == -1) // The mother was not exist
            {
                throw new Exception("ERROR: The mother with id: " + mother.Id + " not exist in the database !!!");
            }
            else // Apdating the mother
            {
                Mother newMother = mother.Clone(); // Creates a new instance
                DataSourceList.MothersList[index] = newMother;
            }
        }

        public IEnumerable<Mother> GetMothers(Func<Mother, bool> predicate = null)
        {
            if (DataSourceList.MothersList.Count() == 0)
            {
                throw new Exception("ERROR: The list of mothers are empty !!!");
            }
            if (predicate == null) // Returns a new list with the all mothers
            {
                return DataSourceList.MothersList.Clone();
            }
            else // Returns only the wanted mothers
            {
                IEnumerable<Mother> temp = DataSourceList.MothersList.Where(predicate);
                if (temp.Count() == 0)
                {
                    throw new Exception("ERROR: There is no mother who fits this condition !!!");
                }
                else
                {
                    return temp;
                }
            }
        }

        public Child FindChild(int childId)
        {
            // Returns the child or null (if the child was not founds)
            return DataSourceList.ChildrenList.FirstOrDefault(c => c.Id == childId);
        }

        public void AddChild(Child child)
        {
            if (FindChild(child.Id) != null) // Makes sure that the child not exist already
            {
                throw new Exception("ERROR: The child with id: " + child.Id + " already exist in the database !!!");
            }
            else // Adds the child to the list
            {
                Child newChild = child.Clone(); // Creates a new instance
                DataSourceList.ChildrenList.Add(newChild);
            }
        }

        public void RemoveChild(int childId)
        {
            // Finds in the list the child to remove 
            Child childToRemove = FindChild(childId);
            if (childToRemove == null) // Makes sure that the child exist in the list
            {
                throw new Exception("ERROR: The child with id: " + childId + " not exist in the database !!!");
            }
            else // Removes the child from the list
            {
                DataSourceList.ChildrenList.Remove(childToRemove);
                // Removes the contract of this child
                //?? DataSource.ContractsList.RemoveAll(c => c.ChildId == child.Id);
            }
        }

        public void UpdateChild(Child child)
        {
            // Finds in the list the index of the child to update
            int index = DataSourceList.ChildrenList.FindIndex(c => c.Id == child.Id);
            if (index == -1) // The child not exist
            {
                throw new Exception("ERROR: The child with id: " + child.Id + " not exist in the database !!!");
            }
            else // Apdating the child
            {
                Child newChild = child.Clone(); // Creates a new instance
                DataSourceList.ChildrenList[index] = newChild;
            }
        }

        public IEnumerable<Child> GetChildren(Func<Child, bool> predicate = null)
        {
            if (DataSourceList.ChildrenList.Count() == 0)
            {
                throw new Exception("ERROR: The list of children are empty !!!");
            }
            if (predicate == null) // Returns a new list with the all children
            {
                return DataSourceList.ChildrenList.Clone();
            }
            else // Returns only the wanted children
            {
                IEnumerable<Child> temp = DataSourceList.ChildrenList.Where(predicate);
                if (temp.Count() == 0)
                {
                    throw new Exception("ERROR: There is no child who fits this condition !!!");
                }
                else
                {
                    return temp;
                }
            }
        }

        public Contract FindContract(int contractNumber)
        {
            // Returns the contract or null (if the contract was not founds)
            return DataSourceList.ContractsList.FirstOrDefault(c => c.ContractNumber == contractNumber);
        }

        public void AddContract(Contract contract, bool update)
        {
            Contract newContract = FindContract(contract.ContractNumber);
            if (newContract != null) // It's not a new contract
            {
                throw new Exception("ERROR: It's not a new contract !!!");
            }
            newContract = contract.Clone(); // Creates a new instance
            if (!update) // Makes sure that we don't updates an exist contract
            {
                newContract.ContractNumber = contractSerial++; // Adds the serial number
            }
            DataSourceList.ContractsList.Add(newContract); // Adds the contract to the list
        }

        public void RemoveContract(int contractNumber)
        {
            // Finds in the list the contract to remove 
            Contract contractToRemove = FindContract(contractNumber);
            if (contractToRemove == null) // Makes sure that the contract exist in the list
            {
                throw new Exception("ERROR: The contract with serial number: " + contractNumber + " not exist in the database !!!");
            }
            else // Removes the contract from the list
            {
                DataSourceList.ContractsList.Remove(contractToRemove);
            }
        }

        public void UpdateContract(Contract contract)
        {
            // Finds in the list the index of the contract to update
            int index = DataSourceList.ContractsList.FindIndex(c => c.ContractNumber == contract.ContractNumber);
            if (index == -1) // The contract not exist
            {
                throw new Exception("ERROR: The contract with serial number: " + contract.ContractNumber + " not exist in the database !!!");
            }
            else // Apdating the contract
            {
                Contract newContract = contract.Clone(); // Creates a new instance
                DataSourceList.ContractsList[index] = newContract;
            }
        }

        public IEnumerable<Contract> GetContracts(Func<Contract, bool> predicate = null)
        {
            //if (DataSourceList.ContractsList.Count() == 0)
            //{
            //    throw new Exception("ERROR: The list of contracts are empty !!!");
            //}
            if (predicate == null) // returns a new list with the all contracts
            {
                return DataSourceList.ContractsList.Clone();
            }
            else // returns only the wanted contracts
            {
                return DataSourceList.ContractsList.Where(predicate);
            }
        }

        public IEnumerable<IGrouping<int, Child>> GetChildrenByMothers()
        {
            if (DataSourceList.ChildrenList.Count() == 0)
            {
                throw new Exception("ERROR: The list of children are empty !!!");
            }
            IEnumerable<Child> temp = DataSourceList.ChildrenList.Clone();
            var children = from child in temp
                           group child by (child.MotherId);
            return children;
        }
    }

}
