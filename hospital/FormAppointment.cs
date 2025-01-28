using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace hospital
{
    public partial class FormAppointment : Form
    {
        private int userId;
        private int doctorId;
        private string doctorName;
        private SqlConnection sql;

        // Constructor to initialize the form with user and doctor details
        public FormAppointment(int userId, int doctorId, string doctorName)
        {
            InitializeComponent();
            this.userId = userId;
            this.doctorId = doctorId;
            this.doctorName = doctorName;
            this.Text = $"Appointment with Dr. {doctorName}"; // Set form title

            sql = new SqlConnection(@"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True");

            // UI setup for date picker and confirm button
            Label lblDate = new Label { Text = "Choose Date & Time:", Location = new System.Drawing.Point(20, 20) };
            DateTimePicker datePicker = new DateTimePicker
            {
                Location = new System.Drawing.Point(20, 50),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm"
            };
            Button btnConfirm = new Button
            {
                Text = "Confirm",
                Location = new System.Drawing.Point(20, 100)
            };

            btnConfirm.Click += (sender, e) => MakeAppointment(datePicker.Value);

            this.Controls.Add(lblDate);
            this.Controls.Add(datePicker);
            this.Controls.Add(btnConfirm);
        }

        // Method to book the appointment
        private void MakeAppointment(DateTime date)
        {
            try
            {
                using (sql)
                {
                    sql.Open();

                    // Check if the selected time slot is already booked
                    string checkQuery = "SELECT COUNT(*) FROM Appointments WHERE DoctorID = @DoctorID AND AppointmentDateTime = @Date";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, sql);
                    checkCmd.Parameters.AddWithValue("@DoctorID", doctorId);
                    checkCmd.Parameters.AddWithValue("@Date", date);

                    int count = (int)checkCmd.ExecuteScalar();

                    // If no appointment exists for this time, create a new appointment
                    if (count == 0)
                    {
                        string insertQuery = "INSERT INTO Appointments (UserID, DoctorID, AppointmentDateTime) VALUES (@UserID, @DoctorID, @Date)";
                        SqlCommand insertCmd = new SqlCommand(insertQuery, sql);
                        insertCmd.Parameters.AddWithValue("@UserID", userId);
                        insertCmd.Parameters.AddWithValue("@DoctorID", doctorId);
                        insertCmd.Parameters.AddWithValue("@Date", date);
                        insertCmd.ExecuteNonQuery();

                        MessageBox.Show("Appointment booked successfully!");
                    }
                    else
                    {
                        MessageBox.Show("This time is already taken! Please choose another.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                this.Close(); // Close the form after appointment attempt
            }
        }
        private void FormAppointment_Load(object sender, EventArgs e)
        {
            // You can use this event to do any setup required when the form loads
            // For example, showing initial information or setting default values
            Console.WriteLine("Appointment form loaded.");
        }
    }
}
