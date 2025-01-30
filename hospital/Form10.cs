using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace hospital
{
    public partial class Form10 : Form
    {
        SqlConnection sql;
        SqlCommand cmd;
        public Form10()
        {
            InitializeComponent();
            sql = new SqlConnection(@"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate all required fields 
            if (string.IsNullOrEmpty(textBox1.Text) ||
                string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) ||
                comboBloodType.SelectedItem == null ||
                (!radioMale.Checked && !radioFemale.Checked))
            {
                MessageBox.Show("All fields are required!");
                return;
            }
            string name = textBox1.Text;
            string email = textBox2.Text;
            string password = textBox3.Text;
            string bloodType = comboBloodType.SelectedItem.ToString();
            string gender = radioMale.Checked ? "Male" : "Female";

         

            try
            {
                sql.Open();

                // Check if the user already exists
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                cmd = new SqlCommand(checkQuery, sql);
                cmd.Parameters.AddWithValue("@Email", email);

                int userCount = (int)cmd.ExecuteScalar();

                if (userCount > 0)
                {
                    MessageBox.Show($"A user with the email {email} already exists.");
                }
                else
                {
                    // Insert new user
                    string insertQuery = @"INSERT INTO Admins (Name, Email, Password,BloodType, Gender) 
                                   VALUES (@Name, @Email, @Password,@BloodType, @Gender)";
                    cmd = new SqlCommand(insertQuery, sql);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password); 
                    cmd.Parameters.AddWithValue("@BloodType", bloodType);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Admin added successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to add the Admin.");
                    }
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





        private void Form10_Load(object sender, EventArgs e)
        {
            comboBloodType.Items.Add("A+");
            comboBloodType.Items.Add("A-");
            comboBloodType.Items.Add("AB+");
            comboBloodType.Items.Add("AB-");
            comboBloodType.Items.Add("B+");
            comboBloodType.Items.Add("B-");
            comboBloodType.Items.Add("O+");
            comboBloodType.Items.Add("O-");
        }
    }
}
