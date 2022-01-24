using System;
using System.Collections.Generic;

namespace congestion.calculator
{
    public class CongestionTaxCalculator
    {
        /**
        * Calculate the total toll fee for one day
        *
        * @param vehicle - the vehicle
        * @param dates   - date and time of all passes on one day
        * @return - the total congestion tax for that day
        */

        private readonly string vehicle;
        private readonly DateTime[] dates;
        private readonly Dictionary<DateTime, int> tolls;

        public CongestionTaxCalculator(string vehicle, DateTime[] dates)
        {
            this.vehicle = vehicle;
            this.dates = dates;
            this.tolls = new Dictionary<DateTime, int>()
            {
                // key: upper limit, value: toll
                {new DateTime(2013, 1, 1, 6, 30, 00), 8 },
                {new DateTime(2013, 1, 1, 7, 00, 00), 13 },
                {new DateTime(2013, 1, 1, 8, 00, 00), 18 },
                {new DateTime(2013, 1, 1, 8, 30, 00), 13 },
                {new DateTime(2013, 1, 1, 15, 00, 00), 8 },
                // etc etc
            };
        }

        public int GetTaxForDay()
        {
            if (IsTollFreeVehicle() || IsTollFreeDate(dates[0]))
            {
                return 0;
            }

            DateTime intervalStart = dates[0];
            int totalFee = 0;
            int tempFee, nextFee;
            for (int i = 0; i < dates.Length; i++)
            {
                nextFee = GetTollFee(dates[i]);
                tempFee = GetTollFee(intervalStart);

                if ((i != dates.Length - 2) && areTimesWithinHour(intervalStart, dates[i+1])) 
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                    intervalStart = dates[i];
                }

                if (totalFee >= 60) return 60;

            }
            return totalFee;
        }

        private bool areTimesWithinHour(DateTime intervalStart, DateTime date)
        {
            var upperlimit = intervalStart.AddHours(1);
            return date < upperlimit;
        }

        private bool IsTollFreeVehicle()
        {
            TollFreeVehicles vehicleEnum;
            return Enum.TryParse(vehicle, ignoreCase: true, out vehicleEnum);
        }

        private int GetTollFee(DateTime date)
        {
            foreach(KeyValuePair<DateTime, int> entry in tolls)
            {
                if (date.Hour > entry.Key.Hour && date.Minute > entry.Key.Minute) continue;
                return entry.Value;
            }

            return 0;
        }


        private bool IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (date.Month == 7) return true;

            if (IsHoliday(date)) return true;
            
            return false;
        }

        private bool IsHoliday(DateTime date)
        {
            var holidays = new HashSet<DateTime>()
            {
                new DateTime(2013,1,1),
                new DateTime(2013,12,25),
                // etc etc
            };

            // if the next day of date is a holiday, day is also free tolls
            if (holidays.Contains(date) || holidays.Contains(date.AddDays(1))) return true;

            return false;
        }


        private enum TollFreeVehicles
        {
            Motorcycle,
            Emergency,
            Diplomat,
            Foreign,
            Military,
            Bus
        }
    }

}