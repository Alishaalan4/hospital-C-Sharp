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

namespace hospital
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fillgridview();
        }

        private void fillgridview()
        {
            string connection = @"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                DataTable dt = new DataTable();

                if (string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    MessageBox.Show("Please enter an ID, Name, or Email to search.");
                    return;
                }

                string query = "SELECT * FROM Admins WHERE AdminID LIKE @Search OR Name LIKE @Search OR Email LIKE @Search";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Search", "%" + textBox5.Text.Trim() + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    conn.Open();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        // Display data in GridView
                        dataGridView1.DataSource = dt;

                        // Fill textboxes with first matching user
                        textBox1.Text = dt.Rows[0]["Name"].ToString();
                        textBox2.Text = dt.Rows[0]["Email"].ToString();
                        //textBox3.Text = dt.Rows[0]["Height"].ToString();
                        //textBox4.Text = dt.Rows[0]["Weight"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No data found for the entered criteria.");
                        dataGridView1.DataSource = null;

                        // Clear textboxes
                        textBox1.Clear();
                        textBox2.Clear();
                        //textBox3.Clear();
                        //textBox4.Clear();
                        textBox5.Clear();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connection = @"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connection))
            {
                DataTable dt = new DataTable();
                string query = "SELECT * FROM Admins";

                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        conn.Open();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dt;
                        }
                        else
                        {
                            MessageBox.Show("No users found.");
                            dataGridView1.DataSource = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connection = @"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connection);

            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter an ID, Name, or Email to delete.");
                return;
            }

            string deleteQuery = "DELETE FROM Admins WHERE AdminID = @Search OR Name = @Search OR Email = @Search";
            SqlCommand deleteCommand = new SqlCommand(deleteQuery, conn);
            deleteCommand.Parameters.AddWithValue("@Search", textBox5.Text.Trim());

            conn.Open();
            int rowsAffected = deleteCommand.ExecuteNonQuery();
            conn.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Record deleted successfully.");
            }
            else
            {
                MessageBox.Show("No record found with the entered details.");
            }

            textBox1.Clear();
            textBox2.Clear();
            //textBox3.Clear();
            //textBox4.Clear();
            textBox5.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connection = @"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connection);

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) )
            {
                MessageBox.Show("Please fill in all the fields before updating.");
                return;
            }

            string updateQuery = "UPDATE Admins SET " +
             "Name = @Name, " +
            "Email = @Email " +  
            "WHERE AdminID = @Search OR Name = @Search OR Email = @Search";

            SqlCommand updateCommand = new SqlCommand(updateQuery, conn);
            updateCommand.Parameters.AddWithValue("@Name", textBox1.Text.Trim());
            updateCommand.Parameters.AddWithValue("@Email", textBox2.Text.Trim());
            //updateCommand.Parameters.AddWithValue("@Height", textBox3.Text.Trim());s
            //updateCommand.Parameters.AddWithValue("@Weight", textBox4.Text.Trim());
            updateCommand.Parameters.AddWithValue("@Search", textBox5.Text.Trim());

            conn.Open();
            int rowsAffected = updateCommand.ExecuteNonQuery();
            conn.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Updated successfully.");
                button1.PerformClick(); // Refresh data
            }
            else
            {
                MessageBox.Show("No record was updated. Please check the entered details.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Form10().Show();
        }
    }
}
