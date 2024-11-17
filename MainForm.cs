using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TuntiPorttiUser
{
    public partial class MainForm : Form
    {
        private List<WorkEntry> workEntries = new List<WorkEntry>();
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private TimeSpan totalFlex = TimeSpan.Zero;  // Cumulative flex time
        private bool timerRunning = false;
        private FirebaseService firebaseService;
        private string _userId;
        public MainForm(string userId)
        {
            InitializeComponent();

            _userId = userId; // Store userId locally
            firebaseService = new FirebaseService(_userId); // Initialize firebaseService with the userId

            LoadWorkEntries(userId); // Load entries when the form initializes
            RefreshEntriesGrid();
            // Add welcome label with userId
            lblWelcome.Text = $"Welcome {userId}"; // Assuming lblWelcome is the name of your Label
        }

        private async void LoadWorkEntries(string userId)
        {
            try
            {
                workEntries = await firebaseService.GetWorkEntriesAsync(userId);
                //MessageBox.Show($"Entries loaded: {workEntries.Count}");

                RefreshEntriesGrid();  // Refresh grid after loading entries
                CalculateTotalFlex(workEntries);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading work entries: " + ex.Message);
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
            lblElapsedTime.Text = $"Elapsed Time: {elapsedTime.Hours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}";
        }

        private void btnStartTimer_Click(object sender, EventArgs e)
        {
            if (!timerRunning)
            {
                txtStart.Text = DateTime.Now.ToString("HH\\:mm");
                elapsedTime = TimeSpan.Zero;
                timer.Start();
                timerRunning = true;
                lblElapsedTime.Text = $"Elapsed Time: 00:00:00";
            }
        }

        private void btnStopTimer_Click(object sender, EventArgs e)
        {
            if (timerRunning)
            {
                timer.Stop();
                timerRunning = false;
                txtEnd.Text = DateTime.Now.ToString("HH\\:mm");
            }
        }

        private async void btnAddEntry_Click(object sender, EventArgs e)
        {
            try
            {
                // Parsing the times
                TimeSpan start = TimeSpan.ParseExact(txtStart.Text, "hh\\:mm", null);
                TimeSpan end = TimeSpan.ParseExact(txtEnd.Text, "hh\\:mm", null);
                TimeSpan totalHours = end - start;
                // Create the work entry
                var entry = new WorkEntry
                {
                    Date = dtpDate.Value.Date,
                    StartTime = start,
                    EndTime = end,
                    ProjectName = comboBoxNames.SelectedItem.ToString(),
                };
                // Update the project's HoursWorked
                double hoursWorked = totalHours.TotalHours;
                await firebaseService.UpdateProjectHours(entry.ProjectName, hoursWorked);
                // Add the entry to the list
                workEntries.Add(entry);

                // Update the total flex time label
                CalculateTotalFlex(workEntries);

                // Send to Firebase
                await firebaseService.AddWorkEntryAsync(entry);

                // Refresh the grid or list view
                RefreshEntriesGrid();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid time values in HH:mm format.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void CalculateTotalFlex(List<WorkEntry> workEntries)
        {
            // Define standard daily work hours
            TimeSpan standardDailyHours = TimeSpan.FromHours(8);

            // Group entries by date
            var groupedEntries = workEntries
                .GroupBy(e => e.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalHours = TimeSpan.FromHours(g.Sum(e => e.TotalHours.TotalHours)), // Sum up total hours for the day
                    Entries = g.ToList()
                });

            // Calculate total flex time
            TimeSpan totalFlexTime = TimeSpan.Zero;

            foreach (var group in groupedEntries)
            {
                // Calculate daily flex as the difference between total hours and standard daily hours
                TimeSpan dailyFlex = group.TotalHours - standardDailyHours;
                totalFlexTime = totalFlexTime.Add(dailyFlex);
            }

            // Update the flex time display
            lblTotalFlex.Text = $"Total Flex Hours: {(totalFlexTime < TimeSpan.Zero ? "-" : "")}{totalFlexTime.ToString(@"hh\:mm")}";
        }

        // Event handler for deleting the selected entry
        private async void btnDeleteEntry_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dgvEntries.SelectedRows.Count > 0)
            {
                // Get the selected row
                var selectedRow = dgvEntries.SelectedRows[0];

                // Get the Firebase key of the selected entry
                var firebaseKey = selectedRow.Cells["FirebaseKey"].Value?.ToString();

                if (string.IsNullOrEmpty(firebaseKey))
                {
                    MessageBox.Show("Cannot find Firebase key for the selected entry.");
                    return;
                }

                // Confirm delete action
                var confirmResult = MessageBox.Show("Are you sure you want to delete this entry?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        // Call delete method in FirebaseService
                        await firebaseService.DeleteWorkEntryAsync(firebaseKey);

                        // Optionally, update the local data source by removing the entry.
                        var entryToRemove = workEntries.FirstOrDefault(entry => entry.FirebaseKey == firebaseKey);
                        if (entryToRemove != null)
                        {
                            workEntries.Remove(entryToRemove);  // Remove from the data source

                            // Re-bind the data source to reflect changes
                            dgvEntries.DataSource = null;
                            dgvEntries.DataSource = workEntries;  // Resetting the data source updates the DataGridView'
                            CalculateTotalFlex(workEntries);
                            RefreshEntriesGrid();
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete entry: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void RefreshEntriesGrid()
        {
            dgvEntries.DataSource = null;
            dgvEntries.DataSource = workEntries;

            // Set custom headers for better readability
            dgvEntries.Columns["Date"].HeaderText = "Date";
            dgvEntries.Columns["StartTime"].HeaderText = "Start Time";
            dgvEntries.Columns["EndTime"].HeaderText = "End Time";
            dgvEntries.Columns["TotalHours"].HeaderText = "Total Hours";
            if (dgvEntries.Columns.Contains("flex"))
            {
                dgvEntries.Columns["Flex"].Visible = false;  // Hide the column from the DataGridView
            }

            //dgvEntries.Columns["Project Name"].HeaderText = "Project Name";
            if (dgvEntries.Columns.Contains("FirebaseKey"))
            {
                dgvEntries.Columns["FirebaseKey"].Visible = false;  // Hide the column from the DataGridView
            }
        }

        // Load project names into dropdown when form loads
        private async void MainForm_Load(object sender, EventArgs e)
        {
            // Fetch project names from the secondary Firebase
            List<string> projectNames = await firebaseService.GetProjectNames();

            // Populate the ComboBox with project names
            foreach (var projectName in projectNames)
            {
                comboBoxNames.Items.Add(projectName);  // Assuming projectComboBox is your ComboBox name
            }
            
        }

        private void ExelButton_Click(object sender, EventArgs e)
        {
            ExportExel.ExportToExcel(workEntries, totalFlex, _userId);
        }
    }
}
