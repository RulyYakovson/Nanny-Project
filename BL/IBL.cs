using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public interface IBL
    {
        void AddNanny(Nanny nanny);
        void RemoveNanny(int nannyId);
        void UpdateNanny(Nanny nanny);
        void AddMother(Mother mother);
        void RemoveMother(int motherId);
        void UpdateMother(Mother mother);
        void AddChild(Child child);
        void RemoveChild(int childId);
        void UpdateChild(Child child);
        void AddContract(Contract contract, bool update = false);
        void RemoveContract(int contractNumber, bool update = false);
        void UpdateContract(Contract contract);
        IEnumerable<Nanny> GetNannies(Func<Nanny, bool> predicate = null);
        IEnumerable<Child> GetChildren(Func<Child, bool> predicate = null);
        IEnumerable<Mother> GetMothers(Func<Mother, bool> predicate = null);
        IEnumerable<Contract> GetContracts(Func<Contract, bool> predicate = null);
        IEnumerable<IGrouping<int, Child>> GetChildrenByMothers();
        IEnumerable<Child> ChildrenWithoutNanny();
        IEnumerable<Nanny> NanniesVacationDaysByTamat();
        IEnumerable<Nanny> GetNanniesByDistance(Mother mother, int wantedDistance);
        IEnumerable<IGrouping<int, Nanny>> GetNanniesByChildrenAge(bool minAge, bool toSort = false);
        IEnumerable<IGrouping<int, Contract>> GetContractsByDistance(bool toSort = false);
        IEnumerable<IGrouping<string, Contract>> GetContractsByNannies(bool signedOnly = false);
        IEnumerable<Nanny> NanniesSuitableMother(int motherId);
        IEnumerable<Contract> GetUnsignedContracts();
        IEnumerable<Contract> GetSignedContracts();
        IEnumerable<Nanny> NanniesVacationDaysByMinistryOfEducation();
        IEnumerable<Nanny> NanniesByMotherNeeds(Mother theMother, bool? daysAndHours, bool? elevator, bool ifFloor, int floor, bool ifExperience, int experience);
        int GetNumOfContracts(Func<Contract, bool> predicate = null);
        int CalculateDistance(string source, string dest);
        void Init();
    }
}
