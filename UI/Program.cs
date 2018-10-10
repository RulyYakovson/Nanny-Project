using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BE;
using BL;

namespace UI_Console
{
    class Program
    {
        static void Main(string[] args)
        {

           
            // Day d = new Day { TheDay = "sun", BeginningTime = new TimeSpan(8, 5, 0), EndTime = new TimeSpan(16, 10, 0) };
            // Console.WriteLine(d.EndTime.Hours);
            // var a = BlFactorySingleton.Instance.GetChildrenByMothers();
            // foreach (var item in a)
            // {
            //     Console.WriteLine(item.Key);
            //     foreach (var item2 in item)
            //     {
            //         Console.WriteLine(item2);
            //     }
            //     Console.WriteLine("********************\n");
            // }
            int action, id;
            try
            {
                BlFactorySingleton.Instance.Init();
                do
                {

                    MainMenu();

                    action = int.Parse(Console.ReadLine());
                    try
                    {
                        switch (action)
                        {
                            case 1:
                                Add();
                                break;
                            case 2:
                                Delete();
                                break;
                            case 3:
                                Edit();
                                break;
                            case 4:
                                Show();
                                break;
                            case 5:
                                List<Child> toPrintChildren = BlFactorySingleton.Instance.ChildrenWithoutNanny().ToList();
                                foreach (Child c in toPrintChildren)
                                {
                                    Console.WriteLine(c.ToString());
                                    Console.WriteLine("*************");
                                }
                                break;
                            case 6:
                                Console.WriteLine("enter the id number of the mother");
                                id = int.Parse(Console.ReadLine());
                                List<Nanny> toPrintNannies = BlFactorySingleton.Instance.NanniesSuitableMother(id).ToList();
                                foreach (Nanny n in toPrintNannies)
                                {
                                    Console.WriteLine(n.ToString());
                                    Console.WriteLine("*************");
                                }
                                break;
                            case 7:
                                Console.WriteLine("enter the nanny id");
                                id = int.Parse(Console.ReadLine());
                                List<Contract> toPrintContracts = BlFactorySingleton.Instance.GetContracts(c => c.NannyId == id).ToList();
                                foreach (Contract con in toPrintContracts)
                                {
                                    Console.WriteLine(con.ToString());
                                    Console.WriteLine("*************");
                                }
                                break;
                            case 8:
                                IEnumerable<IGrouping<int, Nanny>> nanniesByAges = BlFactorySingleton.Instance.GetNanniesByChildrenAge(minAge: true);
                                foreach (var item in nanniesByAges)
                                {
                                    Console.WriteLine(item.Key);
                                    foreach (var item2 in item)
                                    {
                                        Console.WriteLine(item2);
                                    }
                                    Console.WriteLine("********************\n");
                                }
                                break;
                            case 9:
                                IEnumerable<IGrouping<int, Child>> childrenByMothers = BlFactorySingleton.Instance.GetChildrenByMothers().ToList();
                                foreach (var item in childrenByMothers)
                                {
                                    Console.WriteLine(item.Key);
                                    foreach (var item2 in item)
                                    {
                                        Console.WriteLine(item2);
                                    }
                                    Console.WriteLine("********************\n");
                                }
                                break;
                            case 10:
                                List<Nanny> nannies = BlFactorySingleton.Instance.NanniesVacationDaysByTamat().ToList();
                                foreach (Nanny item in nannies)
                                {
                                    Console.WriteLine(item);
                                    Console.WriteLine("\n***********\n");
                                }
                                break;
                            case 11:
                                Console.WriteLine("enter the mother id");
                                id = int.Parse(Console.ReadLine());
                                Mother mother = BlFactorySingleton.Instance.GetMothers(m => m.Id == id).FirstOrDefault();
                                Console.WriteLine("Enter the wanted range in meters");
                                mother.WantedRange = int.Parse(Console.ReadLine());
                                BlFactorySingleton.Instance.UpdateMother(mother);
                                Thread nanniesInRange = new Thread(printNanniesInRange);
                                nanniesInRange.Start(mother);
                                break;
                            case 12:
                                Thread t1 = new Thread(printContractByDistance);
                                t1.Start();
                                break;
                            case 0:
                                Console.WriteLine("bye");
                                break;
                            default:
                                Console.WriteLine("ERROR");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }

                } while (action != 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); ;
            }

        }

        private static void printNanniesInRange(object mother)
        {
            try
            {
                Mother theMother = mother as Mother;
                List<Nanny> nannies = BlFactorySingleton.Instance.GetNanniesByDistance(theMother, theMother.WantedRange).ToList();
                foreach (Nanny item in nannies)
                {
                    Console.WriteLine(item);
                    Console.WriteLine("\n***************\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void printContractByDistance()
        {
            try
            {
                var contracts = BlFactorySingleton.Instance.GetContractsByDistance(toSort: true);
                foreach (var item in contracts)
                {
                    Console.WriteLine(item.Key);
                    foreach (var item2 in item)
                    {
                        Console.WriteLine(item2);
                    }
                    Console.WriteLine("********************\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); ;
            }
        }
        private static void MainMenu()
        {
            Console.WriteLine("What action do you want to do?");
            Console.WriteLine("1: add");
            Console.WriteLine("2: delete");
            Console.WriteLine("3: edit");
            Console.WriteLine("4: show all");
            Console.WriteLine("5: print children with no nanny");
            Console.WriteLine("6: print the nannies suitable to the mother");
            Console.WriteLine("7: print the contracts of specific nanny");
            Console.WriteLine("8: print nannys group by the age of children");
            Console.WriteLine("9: print children groups by mothers");
            Console.WriteLine("10: print the nannies that their vacation days is by tamat");
            Console.WriteLine("11: print the nannies that in the wanted range");
            Console.WriteLine("12: print the contracts groups by distance");
            Console.WriteLine("0: exit");
        }

        private static void SubMenu()
        {
            Console.WriteLine("Which object you want to operate?");
            Console.WriteLine("1: nanny");
            Console.WriteLine("2: mother");
            Console.WriteLine("3: child");
            Console.WriteLine("4: contract");
            Console.WriteLine("0: back to main menu");
        }

        private static void Add()
        {
            SubMenu();

            int action;
            do
            {
                action = int.Parse(Console.ReadLine());
                switch (action)
                {
                    case 1:
                        AddNan();
                        action = 0;
                        break;
                    case 2:
                        AddMom();
                        action = 0;
                        break;
                    case 3:
                        AddSon();
                        action = 0;
                        break;
                    case 4:
                        AddCon();
                        action = 0;
                        break;
                    case 0:
                        Console.WriteLine("**************");
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (action != 0);
        }

        private static void Edit()
        {
            SubMenu();

            int action;
            do
            {
                action = int.Parse(Console.ReadLine());
                switch (action)
                {
                    case 1:
                        EditNan();
                        action = 0;
                        break;
                    case 2:
                        EditMom();
                        action = 0;
                        break;
                    case 3:
                        EditSon();
                        action = 0;
                        break;
                    case 4:
                        EditCon();
                        action = 0;
                        break;
                    case 0:
                        Console.WriteLine("**************");
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (action != 0);
        }
        private static void Delete()
        {
            SubMenu();

            int action;
            do
            {
                action = int.Parse(Console.ReadLine());
                switch (action)
                {
                    case 1:
                        DelNan();
                        action = 0;
                        break;
                    case 2:
                        DelMom();
                        action = 0;
                        break;
                    case 3:
                        DelSon();
                        action = 0;
                        break;
                    case 4:
                        DelCon();
                        action = 0;
                        break;
                    case 0:
                        Console.WriteLine("**************");
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (action != 0);
        }
        private static void Show()
        {
            SubMenu();

            int action;
            do
            {
                action = int.Parse(Console.ReadLine());
                switch (action)
                {
                    case 1:
                        ShowNan();
                        action = 0;
                        break;
                    case 2:
                        ShowMom();
                        action = 0;
                        break;
                    case 3:
                        ShowSon();
                        action = 0;
                        break;
                    case 4:
                        ShowCon();
                        action = 0;
                        break;
                    case 0:
                        Console.WriteLine("**************");
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (action != 0);
        }

        private static void AddNan()
        {
            int id;
            string lastName;
            string firstName;
            DateTime dateOfBirth;
            int phonNumber;
            string address;
            bool elevator;
            int floor;
            int yearsOfExperience;
            int maxChildren;
            int minimumAgeOfChild;
            int maximumAgeOfChild;
            Payment typeOfPayment;
            double hourlyRate;
            double monthlyRate;
            bool[] workDays = new bool[6];
            Day[] workHours = new Day[6];
            VacationDaysBy vacationDaysBy;
            string recommendations;
            string theDay;
            int beginHour, beginMinute, endHour, endMinute;

            Console.WriteLine("enter id: ");
            id = int.Parse(Console.ReadLine());

            Console.WriteLine("enter last Name: ");
            lastName = Console.ReadLine();

            Console.WriteLine("enter first Name: ");
            firstName = Console.ReadLine();

            Console.WriteLine("enter date Of Birth: (dd mm yyyy)");
            dateOfBirth = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("enter phone: ");
            phonNumber = int.Parse(Console.ReadLine());

            Console.WriteLine("enter address: ");
            address = Console.ReadLine();

            Console.WriteLine("enter elevator: (y/n)");
            char elevat;
            do
            {
                elevat = char.Parse(Console.ReadLine());
                switch (elevat)
                {
                    case 'n':
                    case 'N':
                        elevator = false;
                        break;
                    case 'y':
                    case 'Y':
                        elevator = true;
                        break;
                    default:
                        elevator = false;
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (elevat != 'y' && elevat != 'Y' && elevat != 'N' && elevat != 'n');

            Console.WriteLine("enter floor: ");
            floor = int.Parse(Console.ReadLine());

            Console.WriteLine("enter years of experience");
            yearsOfExperience = int.Parse(Console.ReadLine());

            Console.WriteLine("enter maximum age of kids: ");
            maximumAgeOfChild = int.Parse(Console.ReadLine());

            Console.WriteLine("enter minimum age of kids: ");
            minimumAgeOfChild = int.Parse(Console.ReadLine());

            Console.WriteLine("enter maximum number of kids: ");
            maxChildren = int.Parse(Console.ReadLine());

            Console.WriteLine("enter payment method: (h for hourly pay, m for monthly pay)");
            char pay;
            do
            {
                pay = char.Parse(Console.ReadLine());
                switch (pay)
                {
                    case 'm':
                    case 'M':
                        typeOfPayment = Payment.MONTHLY;
                        Console.WriteLine("enter monthly Rate: ");
                        monthlyRate = double.Parse(Console.ReadLine());
                        hourlyRate = 0;
                        break;
                    case 'h':
                    case 'H':
                        typeOfPayment = Payment.HOURLY;
                        Console.WriteLine("enter hourly Rate: ");
                        hourlyRate = double.Parse(Console.ReadLine());
                        monthlyRate = 0;
                        break;
                    default:
                        hourlyRate = 0;
                        monthlyRate = 0;
                        typeOfPayment = Payment.HOURLY;
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (pay != 'h' && pay != 'H' && pay != 'm' && pay != 'M');

            int nannySum = 0;
            Console.WriteLine("enter works day: ");
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine("day " + (i + 1) + ": (y/n)");
                char day;
                do
                {
                    day = char.Parse(Console.ReadLine());
                    switch (day)
                    {
                        case 'n':
                        case 'N':
                            workDays[i] = false;
                            break;
                        case 'y':
                        case 'Y':
                            nannySum++;
                            workDays[i] = true;
                            break;
                        default:
                            workDays[i] = false;
                            Console.WriteLine("ERROR");
                            break;
                    }
                } while (day != 'y' && day != 'Y' && day != 'N' && day != 'n');
            }
            for (int i = 0; i < nannySum; i++)
            {
                Console.WriteLine("Enter the name of the day:");
                theDay = Console.ReadLine();
                Console.WriteLine("Enter the beginning hour:");
                beginHour = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the minute of the beginning hour:");
                beginMinute = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the end hour:");
                endHour = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the minute of the end hour:");
                endMinute = int.Parse(Console.ReadLine());
                workHours[i] = new Day { TheDay = theDay, BeginningTime = new TimeSpan(beginHour, beginMinute ,0), EndTime = new TimeSpan(endHour, endMinute, 0) };
            }
            Console.WriteLine("enter vacation day by: (m/t)");
            char vacation;
            do
            {
                vacation = char.Parse(Console.ReadLine());
                switch (vacation)
                {
                    case 'm':
                    case 'M':
                        vacationDaysBy = VacationDaysBy.MINISTRY_OF_EDUCATION;
                        break;
                    case 't':
                    case 'T':
                        vacationDaysBy = VacationDaysBy.TAMAT;
                        break;
                    default:
                        vacationDaysBy = VacationDaysBy.TAMAT;
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (vacation != 'm' && vacation != 'M' && vacation != 't' && vacation != 'T');

            Console.WriteLine("enter recommendations: ");
            recommendations = Console.ReadLine();

            Nanny newNan = new Nanny
            {
                Address = address,
                DateOfBirth = dateOfBirth,
                MaximumAgeOfChild = maximumAgeOfChild,
                MinimumAgeOfChild = minimumAgeOfChild,
                Elevator = elevator,
                FirstName = firstName,
                Id = id,
                Floor = floor,
                HourlyRate = hourlyRate,
                TypeOfPayment = typeOfPayment,
                LastName = lastName,
                MaxChildren = maxChildren,
                MonthlyRate = monthlyRate,
                PhonNumber = phonNumber,
                Recommendations = recommendations,
                VacationDays = vacationDaysBy,
                YearsOfExperience = yearsOfExperience,
                WorkDays = workDays,
                WorkHours = workHours
            };
            try
            {
                BlFactorySingleton.Instance.AddNanny(newNan);
                Console.WriteLine("Add successful!");
            }
            catch (Exception exp) { Console.WriteLine(exp.Message); }
        }
        private static void AddMom()
        {
            int id;
            string lastName;
            string firstName;
            int phone;
            string address;
            string locationForNanny;
            bool[] wantedDays = new bool[6];
            Day[] wantedHours = new Day[6];
            string comments;
            string theDay;
            int beginHour, beginMinute, endHour, endMinute;

            Console.WriteLine("enter id: ");
            id = int.Parse(Console.ReadLine());

            Console.WriteLine("enter last Name: ");
            lastName = Console.ReadLine();

            Console.WriteLine("enter first Name: ");
            firstName = Console.ReadLine();

            Console.WriteLine("enter phone: ");
            phone = int.Parse(Console.ReadLine());

            Console.WriteLine("enter address: ");
            address = Console.ReadLine();

            Console.WriteLine("enter job area: ");
            locationForNanny = Console.ReadLine();

            int motherSum = 0;
            Console.WriteLine("enter nanny Needed days: ");
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine("day " + (i + 1) + ": (y/n)");
                char day;
                do
                {
                    day = char.Parse(Console.ReadLine());
                    switch (day)
                    {
                        case 'n':
                        case 'N':
                            wantedDays[i] = false;
                            break;
                        case 'y':
                        case 'Y':
                            motherSum++;
                            wantedDays[i] = true;
                            break;
                        default:
                            wantedDays[i] = false;
                            Console.WriteLine("ERROR");
                            break;
                    }
                } while (day != 'y' && day != 'Y' && day != 'N' && day != 'n');
            }
            for (int i = 0; i < motherSum; i++)
            {
                Console.WriteLine("Enter the name of the day:");
                theDay = Console.ReadLine();
                Console.WriteLine("Enter the beginning hour:");
                beginHour = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the minute of the beginning hour:");
                beginMinute = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the end hour:");
                endHour = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the minute of the end hour:");
                endMinute = int.Parse(Console.ReadLine());
                wantedHours[i] = new Day { TheDay = theDay, BeginningTime = new TimeSpan(beginHour, beginMinute, 0), EndTime = new TimeSpan(endHour, endMinute, 0) };
            }

            Console.WriteLine("enter notes: ");
            comments = Console.ReadLine();

            Mother newMom = new Mother
            {
                Address = address,
                Comments = comments,
                FirstName = firstName,
                LastName = lastName,
                Id = id,
                LocationForNanny = locationForNanny,
                PhonNumber = phone,
                NeededDays = wantedDays,
                NeededHours = wantedHours
            };
            try
            {
                BlFactorySingleton.Instance.AddMother(newMom);
                Console.WriteLine("Add successful!");
            }
            catch (Exception exp) { Console.WriteLine(exp.Message); }
        }
        private static void AddSon()
        {
            int id;
            int momID;
            string firstName;
            DateTime dateOfBirth;
            bool specialNeeds = false;
            string specialNeedsDetails = "";


            Console.WriteLine("enter id: ");
            id = int.Parse(Console.ReadLine());

            Console.WriteLine("enter mother ID: ");
            momID = int.Parse(Console.ReadLine());

            Console.WriteLine("enter first Name: ");
            firstName = Console.ReadLine();

            Console.WriteLine("enter date Of Birth: (dd mm yyyy)");
            dateOfBirth = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("enter special Needs: (y/n)");
            char spcl;
            do
            {
                spcl = char.Parse(Console.ReadLine());
                switch (spcl)
                {
                    case 'n':
                    case 'N':
                        specialNeeds = false;
                        break;
                    case 'y':
                    case 'Y':
                        specialNeeds = true;
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (spcl != 'y' && spcl != 'Y' && spcl != 'N' && spcl != 'n');

            if (specialNeeds)
            {
                Console.WriteLine("enter details of special needs: ");
                specialNeedsDetails = Console.ReadLine();
            }

            Child newSon = new Child
            {
                Id = id,
                ContractSigned = false,
                DateOfBirth = dateOfBirth,
                FirstName = firstName,
                MotherId = momID,
                SpecialNeeds = specialNeeds,
                TheSpecialNeeds = specialNeedsDetails
            };
            try
            {
                BlFactorySingleton.Instance.AddChild(newSon);
                Console.WriteLine("Add successful!");
            }
            catch (Exception exp) { Console.WriteLine(exp.Message); }

        }
        private static void AddCon()
        {
            int nannyID;
            int childID;
            bool introductionMeeting = false;
            DateTime imploymentDateEnd;

            Console.WriteLine("enter nanny ID: ");
            nannyID = int.Parse(Console.ReadLine());

            Console.WriteLine("enter child ID: ");
            childID = int.Parse(Console.ReadLine());

            Console.WriteLine("does introduction meeting has been made? (y/n)");
            char intr;
            do
            {
                intr = char.Parse(Console.ReadLine());
                switch (intr)
                {
                    case 'n':
                    case 'N':
                        introductionMeeting = false;
                        break;
                    case 'y':
                    case 'Y':
                        introductionMeeting = true;
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            } while (intr != 'y' && intr != 'Y' && intr != 'N' && intr != 'n');

            Console.WriteLine("enter imployment Date End: ");
            imploymentDateEnd = DateTime.Parse(Console.ReadLine());

            Contract newCon = new Contract
            {
                NannyId = nannyID,
                ChildId = childID,
                HadInterview = introductionMeeting,
                DateOfEnd = imploymentDateEnd,
            };
            try
            {
                BlFactorySingleton.Instance.AddContract(newCon);
                Console.WriteLine("Add successful!");
            }
            catch (Exception exp) { Console.WriteLine(exp.Message); }
        }

        private static void EditNan()
        {
            int id;
            Console.WriteLine("enter id to edit: ");
            id = int.Parse(Console.ReadLine());
            Nanny nan = BlFactorySingleton.Instance.GetNannies().ToList().Find(x => x.Id == id);
            if (nan != null)
            {
                Console.WriteLine(nan.ToString());
                Console.WriteLine("Enter new details. If you do not want to change, insert the existing one according to the list above");
                string lastName;
                string firstName;
                DateTime dateOfBirth;
                int phonNumber;
                string address;
                bool elevator;
                int floor;
                int yearsOfExperience;
                int maxChildren;
                int minimumAgeOfChild;
                int maximumAgeOfChild;
                Payment typeOfPayment;
                double hourlyRate;
                double monthlyRate;
                bool[] workDays = new bool[6];
                Day[] workHours = new Day[6];
                VacationDaysBy vacationDaysBy;
                string recommendations;
                string theDay;
                int beginHour, beginMinute, endHour, endMinute;

                Console.WriteLine("enter last Name: ");
                lastName = Console.ReadLine();

                Console.WriteLine("enter first Name: ");
                firstName = Console.ReadLine();

                Console.WriteLine("enter date Of Birth: (dd mm yyyy)");
                dateOfBirth = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("enter phone: ");
                phonNumber = int.Parse(Console.ReadLine());

                Console.WriteLine("enter address: ");
                address = Console.ReadLine();

                Console.WriteLine("enter elevator: (y/n)");
                char elevat;
                do
                {
                    elevat = char.Parse(Console.ReadLine());
                    switch (elevat)
                    {
                        case 'n':
                        case 'N':
                            elevator = false;
                            break;
                        case 'y':
                        case 'Y':
                            elevator = true;
                            break;
                        default:
                            elevator = false;
                            Console.WriteLine("ERROR");
                            break;
                    }
                } while (elevat != 'y' && elevat != 'Y' && elevat != 'N' && elevat != 'n');

                Console.WriteLine("enter floor: ");
                floor = int.Parse(Console.ReadLine());

                Console.WriteLine("enter years of experience");
                yearsOfExperience = int.Parse(Console.ReadLine());

                Console.WriteLine("enter maximum age of kids: ");
                maximumAgeOfChild = int.Parse(Console.ReadLine());

                Console.WriteLine("enter minimum age of kids: ");
                minimumAgeOfChild = int.Parse(Console.ReadLine());

                Console.WriteLine("enter maximum number of kids: ");
                maxChildren = int.Parse(Console.ReadLine());

                Console.WriteLine("enter payment method: (h for hourly pay, m for monthly pay)");
                char pay;
                do
                {
                    pay = char.Parse(Console.ReadLine());
                    switch (pay)
                    {
                        case 'm':
                        case 'M':
                            typeOfPayment = Payment.MONTHLY;
                            Console.WriteLine("enter monthly Rate: ");
                            monthlyRate = double.Parse(Console.ReadLine());
                            hourlyRate = 0;
                            break;
                        case 'h':
                        case 'H':
                            typeOfPayment = Payment.HOURLY;
                            Console.WriteLine("enter hourly Rate: ");
                            hourlyRate = double.Parse(Console.ReadLine());
                            monthlyRate = 0;
                            break;
                        default:
                            hourlyRate = 0;
                            monthlyRate = 0;
                            typeOfPayment = Payment.HOURLY;
                            Console.WriteLine("ERROR");
                            break;
                    }
                } while (pay != 'h' && pay != 'H' && pay != 'm' && pay != 'M');

                int nannySum = 0;
                Console.WriteLine("enter works day: ");
                for (int i = 0; i < 6; i++)
                {
                    Console.WriteLine("day " + (i + 1) + ": (y/n)");
                    char day;
                    do
                    {
                        day = char.Parse(Console.ReadLine());
                        switch (day)
                        {
                            case 'n':
                            case 'N':
                                workDays[i] = false;
                                break;
                            case 'y':
                            case 'Y':
                                nannySum++;
                                workDays[i] = true;
                                break;
                            default:
                                workDays[i] = false;
                                Console.WriteLine("ERROR");
                                break;
                        }
                    } while (day != 'y' && day != 'Y' && day != 'N' && day != 'n');
                }
                for (int i = 0; i < nannySum; i++)
                {
                    Console.WriteLine("Enter the name of the day:");
                    theDay = Console.ReadLine();
                    Console.WriteLine("Enter the beginning hour:");
                    beginHour = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the minute of the beginning hour:");
                    beginMinute = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the end hour:");
                    endHour = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the minute of the end hour:");
                    endMinute = int.Parse(Console.ReadLine());
                    workHours[i] = new Day { TheDay = theDay, BeginningTime = new TimeSpan(beginHour, beginMinute, 0), EndTime = new TimeSpan(endHour, endMinute, 0) };
                }
                Console.WriteLine("enter vacation day by: (m/t)");
                char vacation;
                do
                {
                    vacation = char.Parse(Console.ReadLine());
                    switch (vacation)
                    {
                        case 'm':
                        case 'M':
                            vacationDaysBy = VacationDaysBy.MINISTRY_OF_EDUCATION;
                            break;
                        case 't':
                        case 'T':
                            vacationDaysBy = VacationDaysBy.TAMAT;
                            break;
                        default:
                            vacationDaysBy = VacationDaysBy.TAMAT;
                            Console.WriteLine("ERROR");
                            break;
                    }
                } while (vacation != 'm' && vacation != 'M' && vacation != 't' && vacation != 'T');

                Console.WriteLine("enter recommendations: ");
                recommendations = Console.ReadLine();

                Nanny newNan = new Nanny
                {
                    Id = nan.Id,
                    Address = address,
                    DateOfBirth = dateOfBirth,
                    MaximumAgeOfChild = maximumAgeOfChild,
                    MinimumAgeOfChild = minimumAgeOfChild,
                    Elevator = elevator,
                    FirstName = firstName,
                    Floor = floor,
                    HourlyRate = hourlyRate,
                    TypeOfPayment = typeOfPayment,
                    LastName = lastName,
                    MaxChildren = maxChildren,
                    MonthlyRate = monthlyRate,
                    PhonNumber = phonNumber,
                    Recommendations = recommendations,
                    VacationDays = vacationDaysBy,
                    YearsOfExperience = yearsOfExperience,
                    WorkDays = workDays,
                    WorkHours = workHours
                };
                try { BlFactorySingleton.Instance.UpdateNanny(newNan); }
                catch (Exception exp) { Console.WriteLine(exp.Message); }
            }
            else Console.WriteLine("error: the nanny does not found!");
        }
        private static void EditMom()
        {
            int id;
            Console.WriteLine("enter id to edit: ");
            id = int.Parse(Console.ReadLine());
            Mother mom = BlFactorySingleton.Instance.GetMothers().ToList().Find(x => x.Id == id);
            if (mom != null)
            {
                Console.WriteLine(mom.ToString());
                Console.WriteLine("Enter new details. If you do not want to change, insert the existing one according to the list above");

                string lastName;
                string firstName;
                int phone;
                string address;
                string locationForNanny;
                bool[] wantedDays = new bool[6];
                Day[] wantedHours = new Day[6];
                string comments;
                string theDay;
                int beginHour, beginMinute, endHour, endMinute;

                Console.WriteLine("enter last Name: ");
                lastName = Console.ReadLine();

                Console.WriteLine("enter first Name: ");
                firstName = Console.ReadLine();

                Console.WriteLine("enter phone: ");
                phone = int.Parse(Console.ReadLine());

                Console.WriteLine("enter address: ");
                address = Console.ReadLine();

                Console.WriteLine("enter job area: ");
                locationForNanny = Console.ReadLine();

                int motherSum = 0;
                Console.WriteLine("enter nanny Needed days: ");
                for (int i = 0; i < 6; i++)
                {
                    Console.WriteLine("day " + (i + 1) + ": (y/n)");
                    char day;
                    do
                    {
                        day = char.Parse(Console.ReadLine());
                        switch (day)
                        {
                            case 'n':
                            case 'N':
                                wantedDays[i] = false;
                                break;
                            case 'y':
                            case 'Y':
                                motherSum++;
                                wantedDays[i] = true;
                                break;
                            default:
                                wantedDays[i] = false;
                                Console.WriteLine("ERROR");
                                break;
                        }
                    } while (day != 'y' && day != 'Y' && day != 'N' && day != 'n');
                }
                for (int i = 0; i < motherSum; i++)
                {
                    Console.WriteLine("Enter the name of the day:");
                    theDay = Console.ReadLine();
                    Console.WriteLine("Enter the beginning hour:");
                    beginHour = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the minute of the beginning hour:");
                    beginMinute = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the end hour:");
                    endHour = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the minute of the end hour:");
                    endMinute = int.Parse(Console.ReadLine());
                    wantedHours[i] = new Day { TheDay = theDay, BeginningTime = new TimeSpan(beginHour, beginMinute, 0), EndTime = new TimeSpan(endHour, endMinute, 0) };
                }

                Console.WriteLine("enter notes: ");
                comments = Console.ReadLine();

                Mother newMom = new Mother
                {
                    Id = mom.Id,
                    Address = address,
                    Comments = comments,
                    FirstName = firstName,
                    LastName = lastName,
                    LocationForNanny = locationForNanny,
                    PhonNumber = phone,
                    NeededDays = wantedDays,
                    WantedRange = mom.WantedRange,
                    NeededHours = wantedHours
                };
                try
                {
                    BlFactorySingleton.Instance.UpdateMother(newMom);
                }
                catch (Exception exp) { Console.WriteLine(exp.Message); }
            }
            else Console.WriteLine("error: the mother does not found!");
        }
        private static void EditSon()
        {
            int id;
            Console.WriteLine("enter id to edit: ");
            id = int.Parse(Console.ReadLine());
            Child son = BlFactorySingleton.Instance.GetChildren().ToList().Find(x => x.Id == id);
            if (son != null)
            {
                Console.WriteLine(son.ToString());
                Console.WriteLine("Enter new details. If you do not want to change, insert the existing one according to the list above");
                string firstName;
                DateTime dateOfBirth;
                bool specialNeeds = false;
                string specialNeedsDetails = "";

                Console.WriteLine("enter first Name: ");
                firstName = Console.ReadLine();

                Console.WriteLine("enter date Of Birth: (dd mm yyyy)");
                dateOfBirth = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("enter special Needs: (y/n)");
                char spcl;
                do
                {
                    spcl = char.Parse(Console.ReadLine());
                    switch (spcl)
                    {
                        case 'n':
                        case 'N':
                            specialNeeds = false;
                            break;
                        case 'y':
                        case 'Y':
                            specialNeeds = true;
                            break;
                        default:
                            Console.WriteLine("ERROR");
                            break;
                    }
                } while (spcl != 'y' && spcl != 'Y' && spcl != 'N' && spcl != 'n');

                if (specialNeeds)
                {
                    Console.WriteLine("enter details of special needs: ");
                    specialNeedsDetails = Console.ReadLine();
                }

                Child newSon = new Child
                {
                    Id = id,
                    ContractSigned = false,
                    DateOfBirth = dateOfBirth,
                    FirstName = firstName,
                    SpecialNeeds = specialNeeds,
                    MotherId = son.MotherId,
                    TheSpecialNeeds = specialNeedsDetails

                };
                try { BlFactorySingleton.Instance.UpdateChild(newSon); }
                catch (Exception exp) { Console.WriteLine(exp.Message); }
           }
           else Console.WriteLine("ERROR: the child does not found!");
        }
        private static void EditCon()
        {
            int num;
            Console.WriteLine("enter num to edit: ");
            num = int.Parse(Console.ReadLine());
            Contract con = BlFactorySingleton.Instance.GetContracts().ToList().Find(x => x.ContractNumber == num);
            if (con != null)
            {
                Console.WriteLine(con.ToString());
                Console.WriteLine("Enter new details. If you do not want to change, insert the existing one according to the list above");

                DateTime imploymentDateEnd;
                bool introductionMeeting = false;

                Console.WriteLine("does introduction meeting has been made? (y/n)");
                char intr;
                do
                {
                    intr = char.Parse(Console.ReadLine());
                    switch (intr)
                    {
                        case 'n':
                        case 'N':
                            introductionMeeting = false;
                            break;
                        case 'y':
                        case 'Y':
                            introductionMeeting = true;
                            break;
                        default:
                            Console.WriteLine("ERROR");
                            break;
                    }
                } while (intr != 'y' && intr != 'Y' && intr != 'N' && intr != 'n');

                Console.WriteLine("enter imployment Date End: ");
                imploymentDateEnd = DateTime.Parse(Console.ReadLine());

                Contract newCon = new Contract
                {
                    ChildId = con.ChildId,
                    ContractNumber = con.ContractNumber,
                    ContractSigned = con.ContractSigned,
                    DateOfBeginning = con.DateOfBeginning,
                    FinalPayment = con.FinalPayment,
                    MotherId = con.MotherId,
                    NannyId = con.NannyId,
                    PayForHour = con.PayForHour,
                    PayForMonth = con.PayForMonth,
                    RangeOfDistance = con.RangeOfDistance,
                    TypeOfPayment = con.TypeOfPayment,
                    HadInterview = introductionMeeting,
                    DateOfEnd = imploymentDateEnd,
                };
                try { BlFactorySingleton.Instance.UpdateContract(newCon); }
                catch (Exception exp) { Console.WriteLine(exp.Message); }

            }
            else Console.WriteLine("error: the contract does not found!");
        }

        private static void DelNan()
        {
            int id;
            Console.WriteLine("enter id to remove: ");
            id = int.Parse(Console.ReadLine());
            try { BlFactorySingleton.Instance.RemoveNanny(id); }
            catch (Exception exp) { Console.WriteLine(exp.Message); }

        }
        private static void DelMom()
        {
            int id;
            Console.WriteLine("enter id to remove: ");
            id = int.Parse(Console.ReadLine());
            try { BlFactorySingleton.Instance.RemoveMother(id); }
            catch (Exception exp) { Console.WriteLine(exp.Message); }
        }
        private static void DelSon()
        {
            int id;
            Console.WriteLine("enter id to remove: ");
            id = int.Parse(Console.ReadLine());
            try { BlFactorySingleton.Instance.RemoveChild(id); }
            catch (Exception exp) { Console.WriteLine(exp.Message); }
        }
        private static void DelCon()
        {
            int num;
            Console.WriteLine("enter num contract to remove: ");
            num = int.Parse(Console.ReadLine());
            try { BlFactorySingleton.Instance.RemoveContract(num); }
            catch (Exception exp) { Console.WriteLine(exp.Message); }
        }

        private static void ShowNan()
        {
            List<Nanny> nannies = BlFactorySingleton.Instance.GetNannies().ToList();
            foreach (Nanny item in nannies)
            {
                Console.WriteLine(item);
                Console.WriteLine("\n***********\n");
            }
        }
        private static void ShowMom()
        {
            List<Mother> mothers = BlFactorySingleton.Instance.GetMothers().ToList();
            foreach (Mother item in mothers)
            {
                Console.WriteLine(item);
                Console.WriteLine("\n***********\n");
            }
        }
        private static void ShowSon()
        {
            List<Child> children = BlFactorySingleton.Instance.GetChildren().ToList();
            foreach (Child item in children)
            {
                Console.WriteLine(item);
                Console.WriteLine("\n***********\n");
            }
        }
        private static void ShowCon()
        {
            List<Contract> contracts = BlFactorySingleton.Instance.GetContracts().ToList();
            foreach (Contract item in contracts)
            {
                Console.WriteLine(item);
                Console.WriteLine("\n***********\n");
            }
        }
    }
}


