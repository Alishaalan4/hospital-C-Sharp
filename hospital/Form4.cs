using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static hospital.Form1;

namespace hospital
{
    public partial class Form4 : Form
    {
        SqlConnection sql;
        private int doctorId = UserSession.UserId;
        private string doctorName;
        private string doctorSpecialty;
        private string doctorEmail;

        public Form4()
        {
            InitializeComponent();
            sql = new SqlConnection(@"Data Source=ALI-SHAALAN\SQLEXPRESS01;Initial Catalog=hospital;Integrated Security=True");

            this.Text = "Doctor Panel";
            this.Size = new Size(1000, 600);

            Label titleLabel = new Label
            {
                Text = "Doctor Panel",
                Font = new Font("Arial", 16, FontStyle.Bold | FontStyle.Italic),
                AutoSize = true,
                Location = new Point(400, 20)
            };
            this.Controls.Add(titleLabel);

            Panel profilePanel = new Panel
            {
                Size = new Size(950, 100),
                Location = new Point(20, 50),
                BorderStyle = BorderStyle.FixedSingle
            };

            LoadDoctorProfile(profilePanel);
            this.Controls.Add(profilePanel);

            FlowLayoutPanel flowAppointments = new FlowLayoutPanel
            {
                Location = new Point(20, 170),
                Size = new Size(950, 300),
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            this.Controls.Add(flowAppointments);

            Button btnAddAppointment = new Button
            {
                Text = "Add Appointment",
                Size = new Size(150, 40),
                Location = new Point(20, 500)
            };
            btnAddAppointment.Click += BtnAddAppointment_Click;
            this.Controls.Add(btnAddAppointment);

            Button btnLogout = new Button
            {
                Text = "Logout",
                Size = new Size(100, 40),
                Location = new Point(820, 500)
            };
            btnLogout.Click += BtnLogout_Click;
            this.Controls.Add(btnLogout);

            LoadAppointments(flowAppointments);
        }

        private void LoadDoctorProfile(Panel panel)
        {
            try
            {
                sql.Open();
                string query = "SELECT Name, Specialty, Email FROM Doctors WHERE DoctorId = @DoctorID";
                SqlCommand cmd = new SqlCommand(query, sql);
                cmd.Parameters.AddWithValue("@DoctorID", doctorId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    doctorName = reader["Name"].ToString();
                    doctorSpecialty = reader["Specialty"].ToString();
                    doctorEmail = reader["Email"].ToString();
                }
                reader.Close();

                Label lblName = new Label { Text = "Doctor Name: " + doctorName, Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true };
                Label lblSpecialty = new Label { Text = "Specialty: " + doctorSpecialty, Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(250, 10), AutoSize = true };
                Label lblEmail = new Label { Text = "Email: " + doctorEmail, Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(500, 10), AutoSize = true };

                panel.Controls.Add(lblName);
                panel.Controls.Add(lblSpecialty);
                panel.Controls.Add(lblEmail);
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

        private void BtnAddAppointment_Click(object sender, EventArgs e)
        {
            FormSelectUser form = new FormSelectUser(doctorId);
            form.ShowDialog();
            LoadAppointments((FlowLayoutPanel)this.Controls[2]);
        }

        private void LoadAppointments(FlowLayoutPanel panel)
        {
            panel.Controls.Clear();
            try
            {
                sql.Open();
                string query = "SELECT u.Name, a.AppointmentDateTime FROM Appointments a INNER JOIN Users u ON a.UserID = u.UserID WHERE a.DoctorID = @DoctorID";
                SqlCommand cmd = new SqlCommand(query, sql);
                cmd.Parameters.AddWithValue("@DoctorID", doctorId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string userName = reader["Name"].ToString();
                    string date = Convert.ToDateTime(reader["AppointmentDateTime"]).ToString("yyyy-MM-dd HH:mm");

                    Panel card = new Panel
                    {
                        Width = 280,
                        Height = 120,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(10)
                    };

                    Label lblUser = new Label { Text = $"User: {userName}", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true };
                    Label lblDate = new Label { Text = $"Date: {date}", Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(10, 40), AutoSize = true };

                    card.Controls.Add(lblUser);
                    card.Controls.Add(lblDate);
                    panel.Controls.Add(card);
                }
                reader.Close();
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
