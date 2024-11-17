using System;
using System.Collections.Generic;

namespace TuntiPorttiUser
{
    public class WorkEntry
    {

        public string FirebaseKey { get; set; } // Add this property to store the Firebase key
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string ProjectName { get; set; }

        // Calculated property
        public TimeSpan TotalHours => EndTime - StartTime;

        public TimeSpan Flex => TotalHours - new TimeSpan(8, 0, 0); // 8 hours is the standard workday
        //public TimeSpan Flex => TotalHours - new TimeSpan(7, 45, 0); // 7.45


        private TimeSpan CalculateTotalFlex(List<WorkEntry> workEntries)
        {
            TimeSpan totalFlex = new TimeSpan();

            // Loop through all work entries and sum the Flex time
            foreach (var entry in workEntries)
            {
                totalFlex = totalFlex.Add(entry.Flex);
            }

            return totalFlex;
        }

        public override string ToString()
        {
            return $"{Date.ToShortDateString()} | Start: {StartTime.Hours:D2}:{StartTime.Minutes:D2} | End: {EndTime.Hours:D2}:{EndTime.Minutes:D2}";
        }
    }
}
