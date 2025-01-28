using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static hospital.Form1;

namespace hospital
{
    public partial class Form3 : Form
    {
        SqlConnection sql;

        // Use UserSession class to access the logged-in user's ID
        private int userId = UserSession.UserId; // Use UserSession to get the logged-in user's ID

        public Form3()
        {
            InitializeComponent();
            sql = new SqlConnection(@"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True");

            Label titleLabel = new Label
            {
                Text = "User Panel",
                Font = new Font("Arial", 14, FontStyle.Bold | FontStyle.Italic),
                AutoSize = true,
                Location = new Point(400, 20)
            };
            this.Controls.Add(titleLabel);

            flowLayoutPanelDoctors.Location = new Point(20, 60);
            flowLayoutPanelDoctors.Size = new Size(900, 400);
            flowLayoutPanelDoctors.AutoScroll = true;
            flowLayoutPanelDoctors.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanelDoctors.WrapContents = true;

            Button btnLogout = new Button
            {
                Text = "Logout",
                Size = new Size(100, 40),
                Location = new Point(800, 500)
            };
            btnLogout.Click += BtnLogout_Click;
            this.Controls.Add(btnLogout);

            Button btnAppointments = new Button
            {
                Text = "My Appointments",
                Size = new Size(150, 40),
                Location = new Point(600, 500)
            };
            btnAppointments.Click += BtnAppointments_Click;
            this.Controls.Add(btnAppointments);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadDoctors();
        }

        private void LoadDoctors()
        {
            try
            {
                sql.Open();
                string query = "SELECT DoctorID, Name, DATEDIFF(YEAR, BirthDate, GETDATE()) AS Age, Email, Gender, Specialty FROM Doctors";
                SqlCommand cmd = new SqlCommand(query, sql);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int doctorId = Convert.ToInt32(reader["DoctorID"]);
                    string name = reader["Name"].ToString();
                    int age = Convert.ToInt32(reader["Age"]);
                    string email = reader["Email"].ToString();
                    string gender = reader["Gender"].ToString();
                    string specialty = reader["Specialty"].ToString();

                    Panel card = CreateDoctorCard(doctorId, name, age, email, gender, specialty);
                    flowLayoutPanelDoctors.Controls.Add(card);
                }

                reader.Close();
                flowLayoutPanelDoctors.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                sql.Close();
            }
        }

        private Panel CreateDoctorCard(int doctorId, string name, int age, string email, string gender, string specialty)
        {
            Panel panel = new Panel
            {
                Width = 280,
                Height = 180,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            Label lblName = new Label { Text = $"Name: {name}", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true };
            Label lblAge = new Label { Text = $"Age: {age}", Location = new Point(10, 40), AutoSize = true };
            Label lblEmail = new Label { Text = $"Email: {email}", Location = new Point(10, 60), AutoSize = true };
            Label lblGender = new Label { Text = $"Gender: {gender}", Location = new Point(10, 80), AutoSize = true };
            Label lblSpecialty = new Label { Text = $"Specialty: {specialty}", Location = new Point(10, 100), AutoSize = true };

            Button btnMakeAppointment = new Button
            {
                Text = "Make Appointment",
                Size = new Size(120, 30),
                Location = new Point(80, 130)
            };
            btnMakeAppointment.Click += (sender, e) => OpenAppointmentForm(doctorId, name);

            panel.Controls.Add(lblName);
            panel.Controls.Add(lblAge);
            panel.Controls.Add(lblEmail);
            panel.Controls.Add(lblGender);
            panel.Controls.Add(lblSpecialty);
            panel.Controls.Add(btnMakeAppointment);

            return panel;
        }

        private void OpenAppointmentForm(int doctorId, string doctorName)
        {
            FormAppointment form = new FormAppointment(userId, doctorId, doctorName);
            form.ShowDialog();
        }

        private void BtnAppointments_Click(object sender, EventArgs e)
        {
            try
            {
                sql.Open();
                string query = @"
                    SELECT d.Name AS DoctorName, a.AppointmentDateTime
                    FROM Appointments a
                    INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
                    WHERE a.UserID = @UserID";

                SqlCommand cmd = new SqlCommand(query, sql);
                cmd.Parameters.AddWithValue("@UserID", userId); // Use the session userId
                SqlDataReader reader = cmd.ExecuteReader();

                string appointments = "My Appointments:\n";
                while (reader.Read())
                {
                    string doctorName = reader["DoctorName"].ToString();
                    DateTime appointmentDate = Convert.ToDateTime(reader["AppointmentDateTime"]);
                    appointments += $"{doctorName} - {appointmentDate}\n";
                }

                reader.Close();
                MessageBox.Show(appointments, "Appointments");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                sql.Close();
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Logging out...");
            this.Close();
        }
    }
}
