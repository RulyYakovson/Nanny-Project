using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    internal static class ToolsExtensions
    {
        public static XElement toXML(this Nanny n)
        {
            return new XElement("nanny",
                new XElement("Id", n.Id),
                new XElement("FirstName", n.FirstName),
                new XElement("LastName", n.LastName),
                new XElement("Address", n.Address),
                new XElement("PhonNumber", n.PhonNumber),
                new XElement("DateOfBirth", n.DateOfBirth.ToShortDateString()),
                new XElement("Elevator", n.Elevator.ToString()),
                new XElement("Floor", n.Floor),
                new XElement("HourlyRate", n.HourlyRate),
                new XElement("MonthlyRate", n.MonthlyRate),
                new XElement("MaxChildren", n.MaxChildren),
                new XElement("MaximumAgeOfChild", n.MaximumAgeOfChild),
                new XElement("MinimumAgeOfChild", n.MinimumAgeOfChild),
                new XElement("TypeOfPayment", n.TypeOfPayment),
                new XElement("VacationDays", n.VacationDays),
                new XElement("YearsOfExperience", n.YearsOfExperience),
                new XElement("Recommendations", n.Recommendations),
                new XElement("WorkDays", from d in n.WorkDays
                                         select new XElement
                                         ("ifWork", d)),
                new XElement("WorkHours", from d in n.WorkHours
                                          select new XElement
                                          ("HoursOfDay", new XElement("TheDay", d.TheDay),
                                                         new XElement("BeginningTime",
                                                                       new XElement("Hours", d.BeginningTime.Hours),
                                                                       new XElement("Minutes", d.BeginningTime.Minutes),
                                                                       new XElement("Seconds", d.BeginningTime.Seconds)),
                                                         new XElement("EndTime",
                                                                       new XElement("Hours", d.EndTime.Hours),
                                                                       new XElement("Minutes", d.EndTime.Minutes),
                                                                       new XElement("Seconds", d.EndTime.Seconds)
                                                                      )
                                          )
                             )
             );
        }

        public static XElement toXML(this Mother m)
        {
            return new XElement("Mother",
                new XElement("Id", m.Id),
                new XElement("FirstName", m.FirstName),
                new XElement("LastName", m.LastName),
                new XElement("Address", m.Address),
                new XElement("PhonNumber", m.PhonNumber),
                new XElement("LocationForNanny", m.LocationForNanny),
                new XElement("Comments", m.Comments),
                new XElement("WantedRange", m.WantedRange),
                new XElement("WantedDays", from d in m.NeededDays
                                           select new XElement
                                           ("ifNeed", d)),
                new XElement("NeededHours", from d in m.NeededHours
                                            select new XElement
                                          ("HoursOfDay", new XElement("TheDay", d.TheDay),
                                                         new XElement("BeginningTime",
                                                                       new XElement("Hours", d.BeginningTime.Hours),
                                                                       new XElement("Minutes", d.BeginningTime.Minutes),
                                                                       new XElement("Seconds", d.BeginningTime.Seconds)),
                                                         new XElement("EndTime",
                                                                       new XElement("Hours", d.EndTime.Hours),
                                                                       new XElement("Minutes", d.EndTime.Minutes),
                                                                       new XElement("Seconds", d.EndTime.Seconds)
                                                                      )
                                            )
                             )
               );
        }

        public static XElement toXML(this Child c)
        {
            return new XElement("Child",
                new XElement("Id", c.Id),
                new XElement("MotherId", c.MotherId),
                new XElement("FirstName", c.FirstName),
                new XElement("DateOfBirth", c.DateOfBirth.ToShortDateString()),
                new XElement("SpecialNeeds", c.SpecialNeeds),
                new XElement("TheSpecialNeeds", c.TheSpecialNeeds),
                new XElement("ContractSigned", c.ContractSigned)
                );
        }

        public static XElement toXML(this Contract c)
        {
            return new XElement("Contract",
                new XElement("ContractNumber", c.ContractNumber),
                new XElement("NannyId", c.NannyId),
                new XElement("NannyIdAndName", c.NannyIdAndName),
                new XElement("ChildId", c.ChildId),
                new XElement("ChildIdAndName", c.ChildIdAndName),
                new XElement("MotherId", c.MotherId),
                new XElement("ContractSigned", c.ContractSigned),
                new XElement("DateOfBeginning", c.DateOfBeginning.ToShortDateString()),
                new XElement("DateOfEnd", c.DateOfEnd.ToShortDateString()),
                new XElement("HadInterview", c.HadInterview),
                new XElement("PayForHour", c.PayForHour),
                new XElement("PayForMonth", c.PayForMonth),
                new XElement("TypeOfPayment", c.TypeOfPayment),
                new XElement("FinalPayment", c.FinalPayment),
                new XElement("RangeOfDistance", c.RangeOfDistance)
                );
        }

        public static Nanny toNannyInstance(this XElement nannyXML)
        {
            Nanny nanny;
            if (nannyXML == null)
            {
                nanny = null;
            }
            try
            {
                nanny = new Nanny
                {
                    Id = int.Parse(nannyXML.Element("Id").Value),
                    Address = nannyXML.Element("Address").Value,
                    DateOfBirth = DateTime.Parse(nannyXML.Element("DateOfBirth").Value),
                    Elevator = bool.Parse(nannyXML.Element("Elevator").Value),
                    FirstName = nannyXML.Element("FirstName").Value,
                    LastName = nannyXML.Element("LastName").Value,
                    Floor = int.Parse(nannyXML.Element("Floor").Value),
                    HourlyRate = double.Parse(nannyXML.Element("HourlyRate").Value),
                    MonthlyRate = double.Parse(nannyXML.Element("MonthlyRate").Value),
                    MaxChildren = int.Parse(nannyXML.Element("MaxChildren").Value),
                    MinimumAgeOfChild = int.Parse(nannyXML.Element("MinimumAgeOfChild").Value),
                    MaximumAgeOfChild = int.Parse(nannyXML.Element("MaximumAgeOfChild").Value),
                    PhonNumber = int.Parse(nannyXML.Element("PhonNumber").Value),
                    Recommendations = nannyXML.Element("Recommendations").Value,
                    YearsOfExperience = int.Parse(nannyXML.Element("YearsOfExperience").Value),
                    TypeOfPayment = (Payment)Enum.Parse(typeof(Payment), (nannyXML.Element("TypeOfPayment").Value)),
                    VacationDays = (VacationDaysBy)Enum.Parse(typeof(VacationDaysBy), (nannyXML.Element("VacationDays").Value)),
                    WorkDays = (from d in nannyXML.Element("WorkDays").Elements("ifWork")
                                select bool.Parse(d.Value)).ToArray(),
                    WorkHours = (from d in nannyXML.Element("WorkHours").Elements("HoursOfDay")
                                 select new Day
                                 {
                                     TheDay = d.Element("TheDay").Value,
                                     BeginningTime = new TimeSpan(
                                                     int.Parse(d.Element("BeginningTime").Element("Hours").Value),
                                                     int.Parse(d.Element("BeginningTime").Element("Minutes").Value),
                                                     int.Parse(d.Element("BeginningTime").Element("Seconds").Value)),
                                     EndTime = new TimeSpan(
                                                     int.Parse(d.Element("EndTime").Element("Hours").Value),
                                                     int.Parse(d.Element("EndTime").Element("Minutes").Value),
                                                     int.Parse(d.Element("EndTime").Element("Seconds").Value))
                                 }).ToArray()
                };
            }
            catch (Exception)
            {
                nanny = null;
            }
            return nanny;
        }

        public static Mother toMotherInstance(this XElement motherXML)
        {
            Mother mother;
            if (motherXML == null)
            {
                mother = null;
            }
            try
            {
                mother = new Mother
                {
                    Id = int.Parse(motherXML.Element("Id").Value),
                    Address = motherXML.Element("Address").Value,
                    FirstName = motherXML.Element("FirstName").Value,
                    LastName = motherXML.Element("LastName").Value,
                    PhonNumber = int.Parse(motherXML.Element("PhonNumber").Value),
                    Comments = motherXML.Element("Comments").Value,
                    LocationForNanny = motherXML.Element("LocationForNanny").Value,
                    WantedRange = int.Parse(motherXML.Element("WantedRange").Value),
                    NeededDays = (from d in motherXML.Element("WantedDays").Elements("ifNeed")
                                  select bool.Parse(d.Value)).ToArray(),
                    NeededHours = (from d in motherXML.Element("NeededHours").Elements("HoursOfDay")
                                   select new Day
                                   {
                                       TheDay = d.Element("TheDay").Value,
                                       BeginningTime = new TimeSpan(
                                                       int.Parse(d.Element("BeginningTime").Element("Hours").Value),
                                                       int.Parse(d.Element("BeginningTime").Element("Minutes").Value),
                                                       int.Parse(d.Element("BeginningTime").Element("Seconds").Value)),
                                       EndTime = new TimeSpan(
                                                       int.Parse(d.Element("EndTime").Element("Hours").Value),
                                                       int.Parse(d.Element("EndTime").Element("Minutes").Value),
                                                       int.Parse(d.Element("EndTime").Element("Seconds").Value))
                                   }).ToArray()
                };
            }
            catch (Exception)
            {
                mother = null;
            }
            return mother;
        }

        public static Child toChildInstance(this XElement childXML)
        {
            Child child;
            if (childXML == null)
            {
                child = null;
            }
            try
            {
                child = new Child
                {
                    Id = int.Parse(childXML.Element("Id").Value),
                    MotherId = int.Parse(childXML.Element("MotherId").Value),
                    FirstName = childXML.Element("FirstName").Value,
                    DateOfBirth = DateTime.Parse(childXML.Element("DateOfBirth").Value),
                    ContractSigned = bool.Parse(childXML.Element("ContractSigned").Value),
                    SpecialNeeds = bool.Parse(childXML.Element("SpecialNeeds").Value),
                    TheSpecialNeeds = childXML.Element("TheSpecialNeeds").Value,
                };
            }
            catch (Exception)
            {
                child = null;
            }
            return child;
        }

        public static Contract toContractInstance(this XElement contractXML)
        {
            Contract contract;
            if (contractXML == null)
            {
                contract = null;
            }
            try
            {
                contract = new Contract
                {
                    ContractNumber = int.Parse(contractXML.Element("ContractNumber").Value),
                    MotherId = int.Parse(contractXML.Element("MotherId").Value),
                    NannyId = int.Parse(contractXML.Element("NannyId").Value),
                    NannyIdAndName = contractXML.Element("NannyIdAndName").Value,
                    ChildId = int.Parse(contractXML.Element("ChildId").Value),
                    ChildIdAndName = contractXML.Element("ChildIdAndName").Value,
                    ContractSigned = bool.Parse(contractXML.Element("ContractSigned").Value),
                    DateOfBeginning = DateTime.Parse(contractXML.Element("DateOfBeginning").Value),
                    DateOfEnd = DateTime.Parse(contractXML.Element("DateOfEnd").Value),
                    HadInterview = bool.Parse(contractXML.Element("HadInterview").Value),
                    TypeOfPayment = (Payment)Enum.Parse(typeof(Payment), (contractXML.Element("TypeOfPayment").Value)),
                    PayForHour = double.Parse(contractXML.Element("PayForHour").Value),
                    PayForMonth = double.Parse(contractXML.Element("PayForMonth").Value),
                    FinalPayment = double.Parse(contractXML.Element("FinalPayment").Value),
                    RangeOfDistance = int.Parse(contractXML.Element("RangeOfDistance").Value)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                contract = null;
            }
            return contract;
        }

        public static Nanny Clone(this Nanny source)
        {
            return new Nanny
            {
                Address = source.Address,
                DateOfBirth = source.DateOfBirth,
                MaximumAgeOfChild = source.MaximumAgeOfChild,
                MinimumAgeOfChild = source.MinimumAgeOfChild,
                Elevator = source.Elevator,
                FirstName = source.FirstName,
                Id = source.Id,
                Floor = source.Floor,
                HourlyRate = source.HourlyRate,
                TypeOfPayment = source.TypeOfPayment,
                LastName = source.LastName,
                MaxChildren = source.MaxChildren,
                MonthlyRate = source.MonthlyRate,
                PhonNumber = source.PhonNumber,
                Recommendations = source.Recommendations,
                VacationDays = source.VacationDays,
                YearsOfExperience = source.YearsOfExperience,
                WorkDays = source.WorkDays.ToArray(),
                WorkHours = source.WorkHours.ToArray()
            };
        }

        public static Mother Clone(this Mother source)
        {
            return new Mother
            {
                Address = source.Address,
                Comments = source.Comments,
                FirstName = source.FirstName,
                Id = source.Id,
                LastName = source.LastName,
                LocationForNanny = source.LocationForNanny,
                PhonNumber = source.PhonNumber,
                NeededDays = source.NeededDays.ToArray(),
                NeededHours = source.NeededHours.ToArray()
            };
        }

        public static Child Clone(this Child source)
        {
            return new Child
            {
                DateOfBirth = source.DateOfBirth,
                FirstName = source.FirstName,
                Id = source.Id,
                MotherId = source.MotherId,
                SpecialNeeds = source.SpecialNeeds,
                TheSpecialNeeds = source.TheSpecialNeeds
            };
        }

        public static Contract Clone(this Contract source)
        {
            return new Contract
            {
                ChildId = source.ChildId,
                ContractNumber = source.ContractNumber,
                ContractSigned = source.ContractSigned,
                DateOfBeginning = source.DateOfBeginning,
                DateOfEnd = source.DateOfEnd,
                TypeOfPayment = source.TypeOfPayment,
                HadInterview = source.HadInterview,
                NannyId = source.NannyId,
                PayForHour = source.PayForHour,
                PayForMonth = source.PayForMonth
            };
        }

        public static IEnumerable<Nanny> Clone(this IEnumerable<Nanny> source)
        {
            var newList = from nanny in source
                          select nanny;
            return newList.ToList();
        }

        public static IEnumerable<Mother> Clone(this IEnumerable<Mother> source)
        {
            var newList = from mother in source
                          select mother;
            return newList.ToList();
        }

        public static IEnumerable<Child> Clone(this IEnumerable<Child> source)
        {
            var newList = from child in source
                          select child;
            return newList.ToList();
        }

        public static IEnumerable<Contract> Clone(this IEnumerable<Contract> source)
        {
            var newList = from contract in source
                          select contract;
            return newList.ToList();
        }
    }
}
