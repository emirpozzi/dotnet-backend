using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace congestion.calculator
{
    static public class DateParser
    {
        public static DateTime[] GetSortedDatesFromStringList(string datesList)
        {
            var dates = datesList.Split(",");

            var dateTimes = new List<DateTime>();
            foreach(var datestring in dates)
            {
                dateTimes.Add(DateTime.ParseExact(datestring, "yyyy.MM.dd-H:mm", CultureInfo.InvariantCulture));
            }

            dateTimes.Sort();
            return dateTimes.ToArray();
        }

       
    }
}

