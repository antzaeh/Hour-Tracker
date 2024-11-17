using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Database;
using Firebase.Database.Query;

namespace TuntiPorttiUser
{
    public class FirebaseService
    {
        private readonly FirebaseClient _firebaseClient;
        
        private string _userId;

        public FirebaseService(string userId)
        {
            //Add url here
            string firebaseClient = "";
            
            _userId = userId; // Store the userId for this service instance


            // Initialize both Firebase clients
            _firebaseClient= new FirebaseClient(firebaseClient);
            
        }

        // Async method to load data when MainForm loads
        public async Task<List<WorkEntry>> LoadWorkEntriesAsync()
        {
            return await GetWorkEntriesAsync(_userId);
        }

        public async Task<List<WorkEntry>> GetWorkEntriesAsync(string userId)
        {
            try
            {
                // Retrieve entries for the specified user
                var firebaseEntries = await _firebaseClient
                    .Child("workEntries")
                    .Child(userId)  // Access user-specific entries
                    .OnceAsync<dynamic>();

                var workEntries = new List<WorkEntry>();

                foreach (var item in firebaseEntries)
                {
                    try
                    {
                        // Parse and create work entry
                        string rawDate = item.Object.date ?? throw new FormatException("Date is null");
                        string rawStartTime = item.Object.startTime ?? throw new FormatException("StartTime is null");
                        string rawEndTime = item.Object.endTime ?? throw new FormatException("EndTime is null");
                        string rawFlexHours = item.Object.flexHours ?? throw new FormatException("FlexHours is null");
                        string rawProjectName =  item.Object.projectName ?? throw new FormatException("ProjectName is null");

                        DateTime date = DateTime.ParseExact(rawDate, "yyyy-MM-dd", null);
                        TimeSpan startTime = TimeSpan.ParseExact(rawStartTime, @"hh\:mm", null);
                        TimeSpan endTime = TimeSpan.ParseExact(rawEndTime, @"hh\:mm", null);
                        TimeSpan flexHours = TimeSpan.ParseExact(rawFlexHours, @"hh\:mm", null);
                        
                        

                        // Calculate total hours and create entry
                        var entry = new WorkEntry
                        {
                            FirebaseKey = item.Key,
                            Date = date,
                            StartTime = startTime,
                            EndTime = endTime,
                            ProjectName = rawProjectName,
                        };

                        workEntries.Add(entry);
                    }
                    catch (FormatException fe)
                    {
                        Console.WriteLine($"Error parsing entry: {fe.Message}");
                    }
                }

                return workEntries;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while loading work entries: " + ex.Message);
                return new List<WorkEntry>();
            }
        }

        // Add a work entry to Firebase under the user's ID
        public async Task AddWorkEntryAsync(WorkEntry entry)
        {
            try
            {
                // Ensure entry is added under the user's ID in Firebase
                var firebaseEntry = await _firebaseClient
                    .Child("workEntries")
                    .Child(_userId)  // Use the userId here to target the user's entries
                    .PostAsync(new
                    {
                        date = entry.Date.ToString("yyyy-MM-dd"),
                        startTime = entry.StartTime.ToString(@"hh\:mm"),
                        endTime = entry.EndTime.ToString(@"hh\:mm"),
                        totalHours = entry.TotalHours.ToString(@"hh\:mm"),
                        flexHours = entry.Flex.ToString(@"hh\:mm"),
                        projectName = entry.ProjectName.ToString()
                    });
                

                entry.FirebaseKey = firebaseEntry.Key; // Set the key returned from Firebase
                Console.WriteLine("Work entry added successfully to Firebase with key: " + entry.FirebaseKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding work entry: " + ex.Message);
            }
        }

        // Delete a specific work entry from Firebase
        public async Task DeleteWorkEntryAsync(string firebaseKey)
        {
            try
            {
                // Delete entry specific to this user
                await _firebaseClient
                    .Child("workEntries")
                    .Child(_userId)  // Include user ID in the path to ensure it's user-specific
                    .Child(firebaseKey)  // Use the entry's Firebase key
                    .DeleteAsync();

                Console.WriteLine("Work entry deleted successfully from Firebase!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while deleting work entry: " + ex.Message);
            }
        }
        public async Task UpdateProjectHours(string projectName, double hoursWorked)
        {
            try
            {
                //MessageBox.Show("Starting UpdateProjectHours...");

                // Fetch project entries from the secondary Firebase database
                var projectEntries = await _firebaseClient
                    .Child("Project")
                    .OnceAsync<dynamic>();

                //MessageBox.Show("Fetched project entries from Firebase.");

                // Find the project by name
                var project = projectEntries.FirstOrDefault(p => (string)p.Object.Name == projectName);

                if (project != null)
                {
                    //MessageBox.Show("Project found.");

                    // Get the current HoursLogged, stored as a string (hh:mm)
                    string hoursLoggedStr = project.Object.HoursLogged;

                    if (string.IsNullOrEmpty(hoursLoggedStr))
                    {
                       //MessageBox.Show("HoursLogged is empty or null.");
                        return; // Exit if HoursLogged is not available
                    }

                    // Show the HoursLogged value before parsing
                    //MessageBox.Show($"Current HoursLogged: {hoursLoggedStr}");

                    // Parse the HoursLogged string into a TimeSpan object
                    TimeSpan currentHoursLogged = TimeSpan.ParseExact(hoursLoggedStr, @"hh\:mm", null);
                   // MessageBox.Show($"Parsed current HoursLogged: {currentHoursLogged}");

                    // Convert the hoursWorked to a TimeSpan (assumes hoursWorked is in decimal hours)
                    TimeSpan workedTime = TimeSpan.FromHours(hoursWorked);
                    //MessageBox.Show($"Worked time in TimeSpan: {workedTime}");

                    // Add the worked time to the current logged hours
                    TimeSpan updatedHoursLogged = currentHoursLogged.Add(workedTime);
                    //MessageBox.Show($"Updated HoursLogged: {updatedHoursLogged}");

                    // Convert the updated TimeSpan back to "hh:mm" format
                    string updatedHoursLoggedStr = updatedHoursLogged.ToString(@"hh\:mm");
                    //MessageBox.Show($"Updated HoursLogged as string: {updatedHoursLoggedStr}");

                    // Update the project entry with the new HoursLogged value
                    await _firebaseClient
                        .Child("Project")
                        .Child(project.Key)
                        .PatchAsync(new { HoursLogged = updatedHoursLoggedStr });

                    //MessageBox.Show("Successfully updated the project in Firebase.");
                    //MessageBox.Show($"Successfully updated HoursLogged for project '{projectName}' to {updatedHoursLoggedStr}.");
                }
                else
                {
                    MessageBox.Show("Project not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating project hours: " + ex.Message);
            }
        }


        // Method to fetch project names from the secondary Firebase database
        public async Task<List<string>> GetProjectNames()
        {
            try
            {
                var projectNames = new List<string>();
               // MessageBox.Show("EKA");
                // Fetch all project entries from the secondary Firebase project
                var projectEntries = await _firebaseClient
                    .Child("Project")  // Ensure this matches the node name in Firebase
                    .OnceAsync<dynamic>();

                // Extract the 'Name' of each project and add it to the list
                foreach (var item in projectEntries)
                {
                    // Log retrieved item
                    Console.WriteLine("Retrieved item: " + item.Object);

                    if (item.Object.Name != null)  // Ensure it matches Firebase JSON structure
                    {
                        projectNames.Add((string)item.Object.Name);
                        Console.WriteLine("Added project name: " + item.Object.Name);
                    }
                    else
                    {
                        Console.WriteLine("No 'Name' field found in item.");
                    }
                }
              // MessageBox.Show("TOKA");
                Console.WriteLine("Project names successfully fetched from the secondary Firebase.");
                return projectNames;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching project names: " + ex.Message);
                return new List<string>();
            }
        }




    }
}
