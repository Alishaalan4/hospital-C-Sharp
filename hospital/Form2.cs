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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace hospital
{
    public partial class Form2 : Form
    {
        SqlConnection sql;
        SqlCommand cmd;

        public Form2()
        {
            InitializeComponent();
            sql = new SqlConnection(@"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True");
        }

        private void Form2_Load(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate all required fields
            if (string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtPassword.Text) ||
                string.IsNullOrEmpty(txtHeight.Text) ||
                string.IsNullOrEmpty(txtWeight.Text) ||
                comboBloodType.SelectedItem == null ||
                (!radioMale.Checked && !radioFemale.Checked))
            {
                MessageBox.Show("All fields are required!");
                return;
            }

            string name = txtName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            decimal height;
            decimal weight;
            string bloodType = comboBloodType.SelectedItem.ToString();
            string gender = radioMale.Checked ? "Male" : "Female";
            DateTime birthDate = dateTimePicker1.Value;

            // Validate numeric input
            if (!decimal.TryParse(txtHeight.Text, out height) || !decimal.TryParse(txtWeight.Text, out weight))
            {
                MessageBox.Show("Height and Weight must be valid numbers!");
                return;
            }

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
                    string insertQuery = @"INSERT INTO Users (Name, Email, Password, BirthDate, BloodType, Gender, Height, Weight) 
                                   VALUES (@Name, @Email, @Password, @BirthDate, @BloodType, @Gender, @Height, @Weight)";
                    cmd = new SqlCommand(insertQuery, sql);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@BirthDate", birthDate);
                    cmd.Parameters.AddWithValue("@BloodType", bloodType);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Height", height);
                    cmd.Parameters.AddWithValue("@Weight", weight);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User added successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to add the user.");
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


        private void label7_Click(object sender, EventArgs e)
        {

        }
    }

    }
    

