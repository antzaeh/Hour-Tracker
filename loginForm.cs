using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace TuntiporttiUser
{
    public partial class loginForm : Form
    {

        public loginForm()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
           

            string userId = await AuthenticateUser(username, password); // Get the Firebase ID

            if (!string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("Login successful!");
                this.Tag = userId;  // Store the user ID in Tag
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private async Task<string> AuthenticateUser(string username, string password)
        {
            using (var client = new HttpClient())
            {
                string firebaseUrl = "https://tuntiportti-cb7ed-default-rtdb.europe-west1.firebasedatabase.app";
                string usersPath = "/users.json";

                try
                {
                    var response = await client.GetAsync($"{firebaseUrl}{usersPath}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        var usersData = JObject.Parse(jsonData);

                        foreach (var user in usersData)
                        {
                            string dbUsername = user.Value["username"]?.ToString();
                            string dbPassword = user.Value["password"]?.ToString();

                            if (dbUsername == username && dbPassword == password)
                            {
                                return username;  // Return the username as the unique key
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return null; // Return null if authentication fails
        }
    }
}