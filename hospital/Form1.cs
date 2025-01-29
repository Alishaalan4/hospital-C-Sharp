using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace hospital
{
    public partial class Form1 : Form
    {
        SqlConnection sql;
        SqlCommand cmd;

        public Form1()
        {
            InitializeComponent();
            sql = new SqlConnection(@"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True");
        }

        // Static class to handle user and doctor sessions
        public static class UserSession
        {
            public static int UserId { get; set; }  // Stores User ID or Doctor ID
            public static string UserName { get; set; }
            public static string Role { get; set; }
            public static string Email { get; set; }
            public static string Specialty { get; set; } // Only for doctors
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Form2().ShowDialog(); // Open registration form (Form2)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate login fields
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text)
                || (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            string username = textBox1.Text;
            string password = textBox2.Text;
            string role;

            // Determine the role based on the selected radio button
            if (radioButton1.Checked)
            {
                role = "User";
            }
            else if (radioButton2.Checked)
            {
                role = "Admin";
            }
            else
            {
                role = "Doctor";
            }

            try
            {
                sql.Open();

                // Query based on role
                string query;
                if (role == "User")
                {
                    query = "SELECT UserId, Name, Email FROM Users WHERE Email = @Email AND Password = @Password";
                }
                else if (role == "Admin")
                {
                    query = "SELECT AdminId, Name, Email FROM Admins WHERE Email = @Email AND Password = @Password";
                }
                else // Doctor
                {
                    query = "SELECT DoctorId, Name, Email, Specialty FROM Doctors WHERE Email = @Email AND Password = @Password";
                }

                cmd = new SqlCommand(query, sql);
                cmd.Parameters.AddWithValue("@Email", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();  // Read user data
                    int userId = reader.GetInt32(0);
                    string userName = reader.GetString(1);
                    string email = reader.GetString(2);

                    // Store session data
                    UserSession.UserId = userId;
                    UserSession.UserName = userName;
                    UserSession.Role = role;
                    UserSession.Email = email;

                    // If doctor, also store specialty
                    if (role == "Doctor")
                    {
                        UserSession.Specialty = reader.GetString(3);
                    }

                    reader.Close();

                    // Redirect based on role
                    if (role == "User")
                    {
                        Form3 userPanel = new Form3();
                        userPanel.Show();
                    }
                    else if (role == "Admin")
                    {
                        Form5 adminPanel = new Form5();
                        adminPanel.Show();
                    }
                    else // Doctor
                    {
                        Form4 doctorPanel = new Form4();
                        doctorPanel.Show();
                    }

                    this.Hide(); // Hide the login form
                }
                else
                {
                    MessageBox.Show($"{role} not found. Please check your credentials.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                sql.Close();
            }
        }
    }
}
