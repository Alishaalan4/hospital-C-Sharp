using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace hospital
{
    public partial class FormSelectUser : Form
    {
        private int doctorId;
        SqlConnection sql;

        public FormSelectUser(int doctorId)
        {
            InitializeComponent();
            this.doctorId = doctorId;
            sql = new SqlConnection(@"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True");

            this.Text = "Select Patient and Schedule Appointment";
            this.Size = new System.Drawing.Size(400, 300);

            Label lblUser = new Label { Text = "Select Patient:", Location = new System.Drawing.Point(20, 20) };
            ComboBox cmbUsers = new ComboBox { Location = new System.Drawing.Point(150, 20), Width = 200 };
            LoadUsers(cmbUsers);

            Label lblDate = new Label { Text = "Select Date & Time:", Location = new System.Drawing.Point(20, 60) };
            DateTimePicker datePicker = new DateTimePicker { Location = new System.Drawing.Point(150, 60), Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };

            Button btnConfirm = new Button { Text = "Confirm", Location = new System.Drawing.Point(150, 100) };
            btnConfirm.Click += (sender, e) => MakeAppointment(cmbUsers, datePicker);

            this.Controls.Add(lblUser);
            this.Controls.Add(cmbUsers);
            this.Controls.Add(lblDate);
            this.Controls.Add(datePicker);
            this.Controls.Add(btnConfirm);
        }

        private void LoadUsers(ComboBox comboBox)
        {
            try
            {
                sql.Open();
                string query = "SELECT UserID, Name FROM Users";
                SqlCommand cmd = new SqlCommand(query, sql);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBox.Items.Add(new ComboBoxItem { Text = reader["Name"].ToString(), Value = (int)reader["UserID"] });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
            finally
            {
                sql.Close();
            }
        }

        private void MakeAppointment(ComboBox cmbUsers, DateTimePicker datePicker)
        {
            if (cmbUsers.SelectedItem == null)
            {
                MessageBox.Show("Please select a patient.");
                return;
            }

            int userId = ((ComboBoxItem)cmbUsers.SelectedItem).Value;
            DateTime appointmentDate = datePicker.Value;

            try
            {
                sql.Open();
                string checkQuery = "SELECT COUNT(*) FROM Appointments WHERE DoctorID = @DoctorID AND AppointmentDateTime = @Date";
                SqlCommand checkCmd = new SqlCommand(checkQuery, sql);
                checkCmd.Parameters.AddWithValue("@DoctorID", doctorId);
                checkCmd.Parameters.AddWithValue("@Date", appointmentDate);

                int count = (int)checkCmd.ExecuteScalar();
                if (count == 0)
                {
                    string insertQuery = "INSERT INTO Appointments (UserID, DoctorID, AppointmentDateTime) VALUES (@UserID, @DoctorID, @Date)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, sql);
                    insertCmd.Parameters.AddWithValue("@UserID", userId);
                    insertCmd.Parameters.AddWithValue("@DoctorID", doctorId);
                    insertCmd.Parameters.AddWithValue("@Date", appointmentDate);
                    insertCmd.ExecuteNonQuery();

                    MessageBox.Show("Appointment booked successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("This time slot is already taken. Please select another.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                sql.Close();
            }
        }
    }

    public class ComboBoxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public override string ToString() { return Text; }
    }
}
