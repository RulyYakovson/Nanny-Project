using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using BE;
using DAL;

namespace BL
{
    internal class Bl_imp : IBL
    {

        const int MIN_AGE_OF_NANNY = 18;
        const int MIN_MONTH_OF_CHILD = 3;
        const double DISCOUNT_FOR_EACH_BROTHER = 0.02;

        public void AddNanny(Nanny nanny)
        {
            if (NannyToYoung(nanny) == true) // Makes sure that the nanny above 18 years old
            {
                throw new Exception("ERROR: Cannot add a nanny under 18 years old");
            }
            else // Adds the nanny to the list of the nannies
            {
                DalFactorySingleton.Instance.AddNanny(nanny);
            }
        }

        public void RemoveNanny(int nannyId)
        {
            // Checks if the nanny has a signed contract (if not returns null)
            var check = (from contract in GetContracts().ToList()
                         where contract.NannyId == nannyId
                         select contract).FirstOrDefault();
            if (check == null) // It's possible to remove the nanny from the list
            {
                DalFactorySingleton.Instance.RemoveNanny(nannyId);
            }
            else
            {
                throw new Exception("ERROR: Cannot remove the nanny until she's contracts was canceled");
            }
        }

        public void UpdateNanny(Nanny nanny)
        {
            DalFactorySingleton.Instance.UpdateNanny(nanny);
        }

        public void AddMother(Mother mother)
        {
            DalFactorySingleton.Instance.AddMother(mother);
        }

        public void RemoveMother(int motherId)
        {
            // Checks if the mother has a signed contract (if not returns null)
            var check = (from contract in GetContracts().ToList()
                         where contract.MotherId == motherId
                         select contract).FirstOrDefault();
            if (check == null) // It's possible to remove the mother from the list
            {
                DalFactorySingleton.Instance.RemoveMother(motherId);
            }
            else
            {
                throw new Exception("ERROR: Cannot remove the mother until she's contracts was canceled");
            }
        }

        public void UpdateMother(Mother mother)
        {
            DalFactorySingleton.Instance.UpdateMother(mother);
        }

        public void AddChild(Child child)
        {
            DalFactorySingleton.Instance.AddChild(child);
        }

        public void RemoveChild(int childId)
        {
            // Checks if the child has a signed contract (if not returns null)
            var check = (from contract in GetContracts().ToList()
                         where contract.ChildId == childId
                         select contract).FirstOrDefault();
            if (check == null) // It's possible to remove the child from the list
            {
                DalFactorySingleton.Instance.RemoveChild(childId);
            }
            else
            {
                throw new Exception("ERROR: Cannot remove the child until he's contract was canceled");
            }
        }

        public void UpdateChild(Child child)
        {
            DalFactorySingleton.Instance.UpdateChild(child);
        }

        public void AddContract(Contract contract, bool update = false)
        {
            // Finds the child for whom the contract was signed
            Child theChild = DalFactorySingleton.Instance.FindChild(contract.ChildId);
            if (theChild == null) // Makes sure that the child exist in the list
            {
                throw new Exception("ERROR: The child not exist in the database");
            }
            int childAgeInMonth = calculateAgeInMonth(theChild.DateOfBirth);
            // Finds the mother of the child for whom the contract was signed
            Mother theMother = DalFactorySingleton.Instance.FindMother(theChild.MotherId);
            if (theMother == null) // Makes sure that the mother exist in the list
            {
                throw new Exception("ERROR: The mother not exist in the database");
            }
            // Finds the nanny for whom the contract was signed
            Nanny theNanny = DalFactorySingleton.Instance.FindNanny(contract.NannyId);
            if (theNanny == null) // Makes sure that the nanny exist in the list
            {
                throw new Exception("ERROR: The nanny not exist in the database");
            }
            if (childAgeInMonth < 3 && contract.ContractSigned == true) // Checks if the child above 3 month
            {
                throw new Exception("ERROR: It is not possible to sign a contract for a child under the age of 3 months !!!");
            }
            if (GetContracts(c => c.ChildId == theChild.Id && c.ContractSigned == true).Count() != 0 && contract.ContractSigned == true) // Checks if the child is already signed
            {
                throw new Exception("ERROR: This child already signed a contract with another nanny !!!");
            }
            // Checks if the child suitable to the ages range of this nanny
            if (childAgeInMonth < theNanny.MinimumAgeOfChild && contract.ContractSigned == true)
            {
                throw new Exception("ERROR: This child to young for this nanny !!!");
            }
            if (childAgeInMonth > theNanny.MaximumAgeOfChild)
            {
                throw new Exception("ERROR: This child to big for this nanny !!!");
            }
            if (NannyAlreadyFull(theNanny) == true && contract.ContractSigned == true) // Checks if the nanny already full
            {
                throw new Exception("ERROR: This nanny already full !!!");
            }
            if (contract.DateOfBeginning.CompareTo(contract.DateOfEnd) == 1)
            {
                throw new Exception("ERROR: The beginning date is later than the end date");
            }
            // Updating the details of the signed contract
            contract.MotherId = theChild.MotherId;
            contract.ChildIdAndName = theChild.IdAndName;
            contract.NannyIdAndName = theNanny.IdAndName;
            switch (theNanny.TypeOfPayment)
            {
                case Payment.HOURLY:
                    contract.TypeOfPayment = Payment.HOURLY;
                    contract.PayForHour = theNanny.HourlyRate;
                    contract.FinalPayment = ThePayment(contract);
                    break;
                case Payment.MONTHLY:
                    contract.TypeOfPayment = Payment.MONTHLY;
                    contract.PayForMonth = theNanny.MonthlyRate;
                    contract.FinalPayment = ThePayment(contract);
                    break;
            }
            DalFactorySingleton.Instance.AddContract(contract, update);
        }

        public void RemoveContract(int contractNumber, bool update = false)
        {
            // Finds the contract to remove
            Contract contractToRemove = DalFactorySingleton.Instance.FindContract(contractNumber);
            if (contractToRemove.ContractSigned == true && !update)
            {
                throw new Exception("ERROR: Cannot remove a signed contract");
            }
            // Removes the contract from the list of contracts
            DalFactorySingleton.Instance.RemoveContract(contractToRemove.ContractNumber);
        }

        public void UpdateContract(Contract contract)
        {
            RemoveContract(contract.ContractNumber, true);
            AddContract(contract, true);
            // DalFactorySingleton.Instance.UpdateContract(contract);
        }

        private double ThePayment(Contract contract)
        {
            double thePayment; // The final payment
            // Gets the number of siblings registered with this nanny
            int numOfBrothers = GetNumOfContracts(c => c.MotherId == contract.MotherId && c.NannyId == contract.NannyId);
            double discount = numOfBrothers * DISCOUNT_FOR_EACH_BROTHER;
            // Finds the nanny for whom the contract was signed
            Nanny theNanny = DalFactorySingleton.Instance.FindNanny(contract.NannyId);
            // Finds the mother for whom the contract was signed
            Mother theMother = DalFactorySingleton.Instance.FindMother(contract.MotherId);
            switch (theNanny.TypeOfPayment)
            {
                case Payment.MONTHLY: // Calculates the payment per month according to the monthly rate
                    thePayment = theNanny.MonthlyRate * (1 - discount);
                    break;
                case Payment.HOURLY: // Calculates the payment per month according to the hourly rate
                    double numOfHours = SumOfHours(theMother);
                    thePayment = theNanny.HourlyRate * numOfHours * (1 - discount);
                    break;
                default:
                    thePayment = 0;
                    break;
            }
            return thePayment;
        }

        // Returns the sum of hours to pay for
        private double SumOfHours(Mother theMother)
        {
            double minutes, sum = 0;
            foreach (Day item in theMother.NeededHours)
            {
                // Counts the number of hours of each day and converts to minutes
                minutes = (item.EndTime.Hours - item.BeginningTime.Hours) * 60;
                minutes += item.EndTime.Minutes; // Adds the minutes of the last hour
                minutes -= item.BeginningTime.Minutes; // Subtracting the minutes of the first hour
                sum += minutes / 60; // Sum the minutes of all days in the week and converts back to hours
            }
            return sum * 4; // The hours of the entire month
        }

        // Returns all nannies that exactly correspond to the mother's days and hours
        public IEnumerable<Nanny> NanniesSuitableMother(int motherId)
        {
            Mother theMother = DalFactorySingleton.Instance.FindMother(motherId);
            // Get a copy of the nannies list
            List<Nanny> temp = DalFactorySingleton.Instance.GetNannies().ToList();
            var nannies = from nanny in temp
                          where (DaysMatch(nanny, theMother) == true) && (HoursMatch(nanny, theMother) == true)
                          select nanny;
            return nannies;
        }

        // Checks if the nanny's work days suitable to mother's work days
        private bool DaysMatch(Nanny nanny, Mother mother)
        {
            bool flag = true;
            // Checks if the days are suitable
            for (int i = 0; i < nanny.WorkDays.Count(); i++)
            {
                if (nanny.WorkDays[i] != mother.NeededDays[i])
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        // Checks if the nanny's work days suitable to mother's work days
        private bool HoursMatch(Nanny nanny, Mother mother)
        {
            bool flag = true;
            // Checks if the hours are suitable
            for (int i = 0; i < nanny.WorkHours.Count() && i < mother.NeededHours.Count(); i++)
            {
                // If the days not suitable
                if (nanny.WorkHours[i].TheDay != mother.NeededHours[i].TheDay)
                {
                    flag = false;
                    break;
                }
                // If the hours not suitable
                if (HoursOfDayMatch(mother.NeededHours[i], nanny.WorkHours[i]) == false)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        // Checks for one day if the work hours of the nanny suitable to the hours needed by the mother
        private bool HoursOfDayMatch(Day motherHours, Day nannyHours)
        {
            // If the nanny's working hours are just like that of the mother
            // or the nanny starts working earlier and finishes later - return true
            return (nannyHours.BeginningTime.CompareTo(motherHours.BeginningTime) <= 0 && nannyHours.EndTime.CompareTo(motherHours.EndTime) >= 0);
        }

        // Returns the all children without a nanny
        public IEnumerable<Child> ChildrenWithoutNanny()
        {
            // Gets a copy of the children list
            List<Child> tempCh = DalFactorySingleton.Instance.GetChildren().ToList();
            // Gets a copy of the contracts list
            List<Contract> tempCon = DalFactorySingleton.Instance.GetContracts().ToList();
            // Updating each child in the copied list of children if he has a nanny
            foreach (Child child in tempCh)
            {
                foreach (Contract contract in tempCon)
                {
                    if (contract.ChildId == child.Id && contract.ContractSigned == true)
                    {
                        child.ContractSigned = true;
                    }
                }
            }
            // Selects all the children without a nanny
            var children = from child in tempCh
                           where child.ContractSigned == false
                           select child;
            return children;
        }

        // Returns the signed contracts
        public IEnumerable<Contract> GetSignedContracts()
        {
            List<Contract> temp = DalFactorySingleton.Instance.GetContracts().ToList();
            var contracts = from contract in temp
                            where contract.ContractSigned == true
                            select contract;

            return contracts;
        }

        // Returns the unsigned contracts
        public IEnumerable<Contract> GetUnsignedContracts()
        {
            List<Contract> temp = DalFactorySingleton.Instance.GetContracts().ToList();
            var contracts = from contract in temp
                            where contract.ContractSigned == false
                            select contract;
            return contracts;
        }

        // Returns the nannies by the needs of the mother
        public IEnumerable<Nanny> NanniesByMotherNeeds(Mother theMother, bool? daysAndHours, bool? elevator, bool ifFloor, int floor, bool ifExperience, int experience)
        {
            // Gets a copy of the nannies list
            List<Nanny> nannies = DalFactorySingleton.Instance.GetNannies().ToList();
            if (daysAndHours == true)
            {
                nannies = (from n in nannies
                           where (DaysMatch(n, theMother) == true) && (HoursMatch(n, theMother) == true)
                           select n).ToList();
            }
            if (elevator == true)
            {
                nannies = (from n in nannies
                           where n.Elevator == true
                           select n).ToList();
            }
            if (ifFloor)
            {
                nannies = (from n in nannies
                           where n.Floor <= floor
                           select n).ToList();
            }
            if (ifExperience)
            {
                nannies = (from n in nannies
                           where n.YearsOfExperience >= experience
                           select n).ToList();
            }

            return nannies;
        }

        // Returns the all nannies that their vacation days by tamat
        public IEnumerable<Nanny> NanniesVacationDaysByTamat()
        {
            // Gets a copy of the nannies list
            List<Nanny> temp = DalFactorySingleton.Instance.GetNannies().ToList();
            var nannies = from nanny in temp
                          where nanny.VacationDays == VacationDaysBy.TAMAT
                          select nanny;

            return nannies;
        }

        // Returns the all nannies that their vacation days by ministry of education
        public IEnumerable<Nanny> NanniesVacationDaysByMinistryOfEducation()
        {
            // Get a copy of the nannies list
            List<Nanny> temp = DalFactorySingleton.Instance.GetNannies().ToList();
            var nannies = from nanny in temp
                          where nanny.VacationDays == VacationDaysBy.MINISTRY_OF_EDUCATION
                          select nanny;

            return nannies;
        }

        // Checks if the nanny above 18 years
        private bool NannyToYoung(Nanny nanny)
        {
            // The most later date for the nanny's birthday
            DateTime theLaterDate = DateTime.Now.AddYears(-MIN_AGE_OF_NANNY);
            if (nanny.DateOfBirth.CompareTo(theLaterDate) == -1) // The nanny born before this date
            {
                return false;
            }
            else // The nanny born after this date
            {
                return true;
            }
        }

        private int calculateAgeInMonth(DateTime date)
        {
            DateTime today = DateTime.Today;

            int months = today.Month - date.Month;
            int years = today.Year - date.Year;

            if (today.Day < date.Day)
            {
                months--;
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }
            return years * 12 + months;
        }

        // Returns the number of contracts of this nanny
        private bool NannyAlreadyFull(Nanny nanny)
        {
            // Counts the number of contract that has already been signed with this nanny
            List<Contract> temp = DalFactorySingleton.Instance.GetContracts(c => c.NannyId == nanny.Id && c.ContractSigned == true).ToList();
            int numOfContracts = temp.Count();
            // Return true if the nanny already full
            return numOfContracts >= nanny.MaxChildren;
        }

        // Returns all nannies who are close enough to the address given by the mother
        public IEnumerable<Nanny> GetNanniesByDistance(Mother mother, int wantedDistance)
        {
            string theWantedAddress;
            if (mother.LocationForNanny != "") // The mother mention a desired address for a nanny
            {
                theWantedAddress = mother.LocationForNanny;
            }
            else // The mother did not mention a desired address for a nanny
            {
                theWantedAddress = mother.Address;
            }
            // Finds all nannies in this distance
            IEnumerable<Nanny> temp = DalFactorySingleton.Instance.GetNannies();
            var nannies = from nanny in temp
                          where (CalculateDistance(theWantedAddress, nanny.Address) <= wantedDistance)
                          select nanny;
            return nannies;
        }

        // Groupes the nannies by the age of the children, with range of a month for each group
        public IEnumerable<IGrouping<int, Nanny>> GetNanniesByChildrenAge(bool minAge, bool toSort = false)
        {
            // gets a copy of the nannies list
            IEnumerable<Nanny> temp = DalFactorySingleton.Instance.GetNannies();
            IEnumerable<IGrouping<int, Nanny>> nannies = null;
            if (toSort) // Returns the groups sorted
            {
                nannies = from nanny in temp
                          group nanny by (minAge ? nanny.MinimumAgeOfChild : nanny.MaximumAgeOfChild) into g
                          orderby g.Key
                          select g;
            }
            else // Returns the groups not sorted
            {
                nannies = from nanny in temp
                          group nanny by (minAge ? nanny.MinimumAgeOfChild : nanny.MaximumAgeOfChild);
            }
            return nannies;
        }

        // Calculates the range of distance between the nanny 
        // and the address where the mother is looking for a nanny
        // (up to 2 km, between 2-4 km,  4-6 km,  6-8 km and so).
        // returns the number of the end of the range (2, 4, 6, 8....)
        private int RangeOfDistance(Contract contract)
        {
            // Gets the nanny from the contract
            Nanny theNanny = DalFactorySingleton.Instance.GetNannies(n => n.Id == contract.NannyId).ToList()[0];
            // Gets the mother from the contract
            Mother theMother = DalFactorySingleton.Instance.GetMothers(m => m.Id == contract.MotherId).ToList()[0];
            // The address of the nanny
            string dest = theNanny.Address;
            // Selectes the address where the mother is looking for a nanny (if mentioned) or the address of the mother house 
            string source = theMother.LocationForNanny != "" ? theMother.LocationForNanny : theMother.Address;
            double distance = Math.Floor((double)CalculateDistance(source, dest) / 2000);
            return (int)(distance + 1) * 2;
        }

        // Groupes the contracts by the nannies
        public IEnumerable<IGrouping<string, Contract>> GetContractsByNannies(bool signedOnly = false)
        {
            // Gets a copy of the contract list
            IEnumerable<Contract> temp = DalFactorySingleton.Instance.GetContracts();
            IEnumerable<IGrouping<string, Contract>> contracts = null;
            if (signedOnly) // Groupes only the signed contracts
            {
                contracts = from contract in temp
                            where contract.ContractSigned == true
                            group contract by contract.NannyIdAndName;
            }
            else // Groupes all contracts
            {
                contracts = from contract in temp
                            group contract by contract.NannyIdAndName;
            }

            return contracts;
        }

        // Groupes the contracts by the distance between the nanny
        // and the address where the mother is looking for a nanny
        public IEnumerable<IGrouping<int, Contract>> GetContractsByDistance(bool toSort = false)
        {
            // Gets a copy of the contract list
            IEnumerable<Contract> temp = DalFactorySingleton.Instance.GetContracts();
            IEnumerable<IGrouping<int, Contract>> contracts = null;
            if (toSort) // Returns the groups sorted
            {
                contracts = from contract in temp
                            group contract by (RangeOfDistance(contract)) into g
                            orderby g.Key
                            select g;

            }
            else // Returns the groups not sorted
            {
                contracts = from contract in temp
                            group contract by (RangeOfDistance(contract));
            }
            return contracts;
        }

        // Calculates the distance between 2 addresses by using google maps
        public int CalculateDistance(string source, string dest)
        {
            var drivingDirectionRequest = new DirectionsRequest
            {
                TravelMode = TravelMode.Walking,
                Origin = source,
                Destination = dest,
            };
            DirectionsResponse drivingDirections = GoogleMaps.Directions.Query(drivingDirectionRequest);
            Route route = drivingDirections.Routes.First();
            Leg leg = route.Legs.First();
            return leg.Distance.Value;
        }

        public IEnumerable<Child> GetChildren(Func<Child, bool> predicate = null)
        {
            return DalFactorySingleton.Instance.GetChildren(predicate);
        }

        public IEnumerable<Contract> GetContracts(Func<Contract, bool> predicate = null)
        {
            return DalFactorySingleton.Instance.GetContracts(predicate);
        }

        public int GetNumOfContracts(Func<Contract, bool> predicate = null)
        {
            return DalFactorySingleton.Instance.GetContracts(predicate).Count();
        }

        public IEnumerable<Mother> GetMothers(Func<Mother, bool> predicate = null)
        {
            return DalFactorySingleton.Instance.GetMothers(predicate);
        }

        public IEnumerable<Nanny> GetNannies(Func<Nanny, bool> predicate = null)
        {
            return DalFactorySingleton.Instance.GetNannies(predicate);
        }

        public IEnumerable<IGrouping<int, Child>> GetChildrenByMothers()
        {
            return DalFactorySingleton.Instance.GetChildrenByMothers();
        }

        //initializes the data base
        public void Init()
        {
            this.AddNanny(new Nanny
            {
                Id = 300452785,
                FirstName = "Ayala",
                LastName = "zehavi",
                DateOfBirth = DateTime.Parse("02, 1, 1995"),
                Address = "Beit Ha-Defus St 21, Jerusalem, Israel",
                Elevator = true,
                Floor = 2,
                YearsOfExperience = 3,
                PhonNumber = 0523433333,
                MaximumAgeOfChild = 14,
                MinimumAgeOfChild = 3,
                MaxChildren = 8,
                TypeOfPayment = Payment.HOURLY,
                HourlyRate = 10,
                MonthlyRate = 800,
                VacationDays = VacationDaysBy.TAMAT,
                Recommendations = "",
                WorkDays = new bool[6] { true, true, true, true, true, false },
                WorkHours = new Day[6]
                {
                new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                                new Day{TheDay = "Friday", BeginningTime =   new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },

                }
            });
            this.AddNanny(new Nanny
            {
                Id = 300400200,
                FirstName = "Shani",
                LastName = "Tzur",
                DateOfBirth = DateTime.Parse("19, 5, 1990"),
                Address = "Shakhal St 15, Jerusalem, Israel",
                Elevator = true,
                Floor = 2,
                YearsOfExperience = 3,
                PhonNumber = 0523432223,
                MaximumAgeOfChild = 12,
                MinimumAgeOfChild = 4,
                MaxChildren = 5,
                TypeOfPayment = Payment.MONTHLY,
                HourlyRate = 12,
                MonthlyRate = 1000,
                VacationDays = VacationDaysBy.MINISTRY_OF_EDUCATION,
                Recommendations = "",
                WorkDays = new bool[6] { true, true, true, true, true, false },
                WorkHours = new Day[6]
                {
                new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Friday", BeginningTime =   new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },

                }
            });
            this.AddNanny(new Nanny
            {
                Id = 325685214,
                FirstName = "Haya",
                LastName = "Cohen",
                DateOfBirth = DateTime.Parse("19, 5, 1995"),
                Address = "Bar Ilan St 15, Jerusalem, Israel",
                Elevator = false,
                Floor = 3,
                YearsOfExperience = 3,
                PhonNumber = 0528962223,
                MaximumAgeOfChild = 15,
                MinimumAgeOfChild = 6,
                MaxChildren = 7,
                TypeOfPayment = Payment.MONTHLY,
                HourlyRate = 11,
                MonthlyRate = 1200,
                VacationDays = VacationDaysBy.MINISTRY_OF_EDUCATION,
                Recommendations = "",
                WorkDays = new bool[6] { true, true, true, true, true, false },
                WorkHours = new Day[6]
                {
                new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Friday", BeginningTime =   new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                }
            });

            this.AddNanny(new Nanny
            {
                Id = 300190000,
                FirstName = "Mina",
                LastName = "Porush",
                DateOfBirth = DateTime.Parse("29, 8, 1999"),
                Address = "Amram Gaon St 15, Jerusalem, Israel",
                Elevator = true,
                Floor = 2,
                YearsOfExperience = 1,
                PhonNumber = 0523433333,
                MaximumAgeOfChild = 18,
                MinimumAgeOfChild = 8,
                MaxChildren = 5,
                TypeOfPayment = Payment.HOURLY,
                HourlyRate = 15,
                MonthlyRate = 950,
                VacationDays = VacationDaysBy.TAMAT,
                Recommendations = "",
                WorkDays = new bool[6] { true, true, true, true, true, true },
                WorkHours = new Day[6]
                {
                new Day{TheDay = "Sunday", BeginningTime = new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Monday", BeginningTime = new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0), },
                new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Thursday", BeginningTime = new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Friday", BeginningTime =   new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                },
            });
            this.AddNanny(new Nanny
            {
                Id = 215300054,
                FirstName = "Shifra",
                LastName = "Levi",
                DateOfBirth = DateTime.Parse("15, 10, 1989"),
                Address = "Ha-Rav Pinkhas Kehati St 5, Jerusalem, Israel",
                Elevator = true,
                Floor = 8,
                YearsOfExperience = 7,
                PhonNumber = 0548523333,
                MaximumAgeOfChild = 12,
                MinimumAgeOfChild = 4,
                MaxChildren = 5,
                TypeOfPayment = Payment.MONTHLY,
                HourlyRate = 11.5,
                MonthlyRate = 880,
                VacationDays = VacationDaysBy.MINISTRY_OF_EDUCATION,
                Recommendations = "",
                WorkDays = new bool[6] { true, true, true, true, true, true },
                WorkHours = new Day[6]
                {
                new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,30,0),EndTime = new TimeSpan(15, 45, 0), },
                new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Friday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                },
            });
            this.AddNanny(new Nanny
            {
                Id = 222555666,
                FirstName = "Miri",
                LastName = "Harary",
                DateOfBirth = DateTime.Parse("19, 5, 1995"),
                Address = "Eli'ezrov St 15, Jerusalem, Israel",
                Elevator = false,
                Floor = 1,
                YearsOfExperience = 5,
                PhonNumber = 0502225554,
                MaximumAgeOfChild = 11,
                MinimumAgeOfChild = 3,
                MaxChildren = 6,
                TypeOfPayment = Payment.MONTHLY,
                HourlyRate = 12,
                MonthlyRate = 1000,
                VacationDays = VacationDaysBy.MINISTRY_OF_EDUCATION,
                Recommendations = "",
                WorkDays = new bool[6] { true, true, true, true, true, true },
                WorkHours = new Day[6]
                {
                new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,30,0),EndTime = new TimeSpan(15, 45, 0), },
                new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Friday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                },
            });

            this.AddNanny(new Nanny
            {
                Id = 300444444,
                FirstName = "Adi",
                LastName = "Shurin",
                DateOfBirth = DateTime.Parse("19, 5, 1978"),
                Address = "Shakhal St 28, Jerusalem, Israel",
                Elevator = true,
                Floor = 2,
                YearsOfExperience = 3,
                PhonNumber = 0523432223,
                MaximumAgeOfChild = 12,
                MinimumAgeOfChild = 4,
                MaxChildren = 3,
                TypeOfPayment = Payment.MONTHLY,
                HourlyRate = 10.5,
                MonthlyRate = 990,
                VacationDays = VacationDaysBy.TAMAT,
                Recommendations = "",
                WorkDays = new bool[6] { true, true, false, true, true, false },
                WorkHours = new Day[6]
                {
                new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,30,0),EndTime = new TimeSpan(15, 15, 0), },
                new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(15, 15, 0) },
                new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Friday", BeginningTime =    new TimeSpan(9,15,0),EndTime = new TimeSpan(15, 45, 0) }, }
            });
            this.AddNanny(new Nanny
            {
                Id = 200300600,
                FirstName = "Sarit",
                LastName = "Golomb",
                DateOfBirth = DateTime.Parse("04, 01, 2000"),
                Address = "Shakhal St 24, Jerusalem, Israel",
                Elevator = false,
                Floor = 0,
                YearsOfExperience = 0,
                PhonNumber = 0525487523,
                MaximumAgeOfChild = 24,
                MinimumAgeOfChild = 10,
                MaxChildren = 10,
                TypeOfPayment = Payment.MONTHLY,
                HourlyRate = 8,
                MonthlyRate = 950,
                VacationDays = VacationDaysBy.MINISTRY_OF_EDUCATION,
                Recommendations = "",
                WorkDays = new bool[6] { true, true, false, true, true, false },
                WorkHours = new Day[6]
               {
                new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,30,0),EndTime = new TimeSpan(15, 15, 0), },
                new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(15, 15, 0) },
                new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0) },
                new Day{TheDay = "Friday", BeginningTime =    new TimeSpan(9,15,0),EndTime = new TimeSpan(15, 45, 0) }, }
            });

            this.AddMother(new Mother
            {
                Id = 300190055,
                FirstName = "Bracha",
                LastName = "Shimon",
                PhonNumber = 0588886652,
                Address = "HaRav Herzog St 12, Jerusalem, Israel",
                LocationForNanny = "Shakhal St 23, Jerusalem, Israel",
                Comments = "",
                NeededDays = new bool[6] { true, true, true, true, true, false },
                NeededHours = new Day[6]
                {
                 new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                 new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                 new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                 new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                 new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                 new Day { TheDay = "Friday", BeginningTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(15, 45, 0) }, }

            });
            this.AddMother(new Mother
            {
                Id = 200100666,
                FirstName = "Haya",
                LastName = "Polak",
                PhonNumber = 0589874563,
                Address = "Ha-Va'ad ha-Le'umi Street 21, Jerusalem, Israel",
                LocationForNanny = "Shakhal St 23, Jerusalem, Israel",
                Comments = "",
                NeededDays = new bool[6] { true, true, true, true, true, true },
                NeededHours = new Day[6]
                {
                  new Day{TheDay = "Sunday", BeginningTime = new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                  new Day{TheDay = "Monday", BeginningTime = new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                  new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0), },
                  new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(15, 45, 0) },
                  new Day{TheDay = "Thursday", BeginningTime = new TimeSpan(8,0,0),EndTime = new TimeSpan(16, 5, 0) },
                  new Day{TheDay = "Friday", BeginningTime =   new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },}
            });
            this.AddMother(new Mother
            {
                Id = 301036563,
                FirstName = "Osnat",
                LastName = "Levi",
                PhonNumber = 0525257852,
                Address = "Shakhal St 23, Jerusalem, Israel",
                LocationForNanny = "",
                Comments = "",
                NeededDays = new bool[6] { true, true, false, true, true, false },
                NeededHours = new Day[6]
                {
                      new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0) },
                      new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                      new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,30,0),EndTime = new TimeSpan(15, 15, 0), },
                      new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(15, 15, 0) },
                      new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0) },
                      new Day{TheDay = "Friday", BeginningTime =    new TimeSpan(9,15,0),EndTime = new TimeSpan(15, 45, 0) }, }
            });
            this.AddMother(new Mother
            {
                Id = 301043333,
                FirstName = "Hades",
                LastName = "Mashiach",
                PhonNumber = 0505258963,
                Address = "Jaffa St 8, Jerusalem, Israel",
                LocationForNanny = "Shakhal St 23, Jerusalem, Israel",
                Comments = "",
                NeededDays = new bool[6] { true, true, true, true, true, false },
                NeededHours = new Day[6]
                    {
                      new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0) },
                      new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                      new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,30,0),EndTime = new TimeSpan(15, 15, 0), },
                      new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(15, 15, 0) },
                      new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0) },
                      new Day{TheDay = "Friday", BeginningTime =    new TimeSpan(9,15,0),EndTime = new TimeSpan(15, 45, 0) }, }
            });
            this.AddMother(new Mother
            {
                Id = 301043366,
                FirstName = "Hades",
                LastName = "Montag",
                PhonNumber = 039632566,
                Address = "Ha-Amarkalim St 4, Jerusalem, Israel",
                LocationForNanny = "Shakhal St 23, Jerusalem, Israel",
                Comments = "",
                NeededDays = new bool[6] { true, true, true, true, true, false },
                NeededHours = new Day[6]
                {
                   new Day{TheDay = "Sunday", BeginningTime =    new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0) },
                   new Day{TheDay = "Monday", BeginningTime =    new TimeSpan(9,0,0),EndTime = new TimeSpan(15, 45, 0) },
                   new Day{TheDay = "Tuesday", BeginningTime =   new TimeSpan(8,30,0),EndTime = new TimeSpan(15, 15, 0), },
                   new Day{TheDay = "Wednesday", BeginningTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(15, 15, 0) },
                   new Day{TheDay = "Thursday", BeginningTime =  new TimeSpan(8,30,0),EndTime = new TimeSpan(16, 5, 0) },
                   new Day{TheDay = "Friday", BeginningTime =    new TimeSpan(12,15,0),EndTime = new TimeSpan(14, 45, 0) }, }
            });


            this.AddChild(new Child
            {
                Id = 303025632,
                FirstName = "Shani",
                MotherId = 300190055,
                DateOfBirth = DateTime.Parse("04, 01, 2017"),
                SpecialNeeds = true,
                TheSpecialNeeds = "kol minenei beayot"
            });
            this.AddChild(new Child
            {
                Id = 332369852,
                FirstName = "Itamar",
                MotherId = 300190055,
                DateOfBirth = DateTime.Parse("04, 05, 2017"),
            });
            this.AddChild(new Child
            {
                Id = 369852147,
                FirstName = "Uriya",
                MotherId = 300190055,
                DateOfBirth = DateTime.Parse("04, 10, 2017"),
            });
            this.AddChild(new Child
            {
                Id = 258965236,
                FirstName = "Yair",
                MotherId = 200100666,
                DateOfBirth = DateTime.Parse("04, 12, 2016"),
            });
            this.AddChild(new Child
            {
                Id = 362565236,
                FirstName = "Oriel",
                MotherId = 200100666,
                DateOfBirth = DateTime.Parse("04, 09, 2017"),
            });
            this.AddChild(new Child
            {
                Id = 300256300,
                FirstName = "Adi",
                MotherId = 301043366,
                DateOfBirth = DateTime.Parse("04, 08, 2017"),
            });
            this.AddChild(new Child
            {
                Id = 332584563,
                FirstName = "Noa",
                MotherId = 301036563,
                DateOfBirth = DateTime.Parse("04, 09, 2017"),
            });
            this.AddChild(new Child
            {
                Id = 333366552,
                FirstName = "Avi",
                MotherId = 301036563,
                DateOfBirth = DateTime.Parse("04, 07, 2017"),
            });
            this.AddChild(new Child
            {
                Id = 369852525,
                FirstName = "Neriya",
                MotherId = 301043333,
                DateOfBirth = DateTime.Parse("04, 06, 2016"),
            });
            this.AddChild(new Child
            {
                Id = 323232652,
                FirstName = "David",
                MotherId = 301043333,
                DateOfBirth = DateTime.Parse("04, 04, 2017"),
            });
            this.AddChild(new Child
            {
                Id = 398789547,
                FirstName = "Yael",
                MotherId = 301043333,
                DateOfBirth = DateTime.Parse("04, 03, 2017"),
            });
            this.AddChild(new Child
            {
                Id = 300106365,
                FirstName = "Yosi",
                MotherId = 301043333,
                DateOfBirth = DateTime.Parse("04, 02, 2017"),
            });
            this.AddChild(new Child
            {
                Id = 374747474,
                FirstName = "Bentzi",
                MotherId = 039632566,
                DateOfBirth = DateTime.Parse("04, 09, 2016"),
            });

            //this.AddContract(new Contract
            //{
            //    ChildId = 300106365,
            //    NannyId = 300190000,
            //    ContractNumber = 10000001,
            //    ContractSigned = true,
            //    DateOfBeginning = DateTime.Parse("14 1 2018"),
            //    DateOfEnd = DateTime.Parse("14 1 2019"),
            //    HadInterview = true,
            //});
            //this.AddContract(new Contract
            //{
            //    ChildId = 374747474,
            //    NannyId = 200300600,
            //    ContractNumber = 666,
            //    ContractSigned = true,
            //    DateOfBeginning = DateTime.Parse("14 1 2018"),
            //    DateOfEnd = DateTime.Parse("14 1 2019"),
            //    HadInterview = true,
            //});
        }







    }
}
