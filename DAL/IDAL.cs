using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface IDAL
    {
        Nanny FindNanny(int nannyId);
        void AddNanny(Nanny nanny);
        void RemoveNanny(int nannyId);
        void UpdateNanny(Nanny nanny);
        Mother FindMother(int motherId);
        void AddMother(Mother mother);
        void RemoveMother(int motherId);
        void UpdateMother(Mother mother);
        Child FindChild(int childId);
        void AddChild(Child child);
        void RemoveChild(int childId);
        void UpdateChild(Child child);
        Contract FindContract(int contractNumber);
        void AddContract(Contract contract, bool update);
        void RemoveContract(int contractNumber);
        void UpdateContract(Contract contract);

        IEnumerable<Nanny> GetNannies(Func<Nanny, bool> predicate = null);
        IEnumerable<Child> GetChildren(Func<Child, bool> predicate = null);
        IEnumerable<Mother> GetMothers(Func<Mother, bool> predicate = null);
        IEnumerable<Contract> GetContracts(Func<Contract, bool> predicate = null);
        IEnumerable<IGrouping<int, Child>> GetChildrenByMothers();
    }
}
