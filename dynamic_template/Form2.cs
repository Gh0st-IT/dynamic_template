using Newtonsoft.Json.Linq;
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

namespace dynamic_template
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Assuming conn is your SqlConnection object
            using (SqlConnection conn = new SqlConnection($"Data Source=DESKTOP-RCNCNNE\\MSSQLSERVER01;Initial Catalog=DynamicTemplate;Integrated Security=True;"))
            {
                conn.Open();

                // Create a SqlCommand to fetch IDs from the database
                string query = "SELECT ID FROM template";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Execute the command and read the IDs
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = reader.GetInt64(0); // Assuming ID is an int
                            comboBox1.Items.Add(id);
                        }
                    }
                }
            }
        }


        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select an ID from the dropdown.");
                return;
            }

            // Assuming conn is your SqlConnection object
            using (SqlConnection conn = new SqlConnection($"Data Source=DESKTOP-RCNCNNE\\MSSQLSERVER01;Initial Catalog=DynamicTemplate;Integrated Security=True;"))
            {
                conn.Open();

                // Create a SqlCommand to fetch the JSON data from the database based on the selected ID
                string query = $"SELECT template FROM template WHERE Id = {comboBox1.SelectedItem}";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Execute the command and read the JSON data
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string jsonData = reader.GetString(0); // Assuming the JSON data is stored as a string

                            // Manually parse the JSON array string
                            JArray jsonArray = JArray.Parse(jsonData);

                            // Iterate over the JSON array items and create controls
                            foreach (JObject item in jsonArray)
                            {
                                JObject panelObject = (JObject)item["Panel"];
                                string labelValue = panelObject["Label"].ToString();
                                string textBoxValue = panelObject["TextBox"].ToString();

                                Label label = new Label();
                                label.Text = labelValue;

                                TextBox textBox = new TextBox();
                                textBox.Text = textBoxValue;
                                textBox.Size = new Size(100, 20);
                                textBox.Dock = DockStyle.Bottom;

                                Panel panel = new Panel();
                                panel.BorderStyle = BorderStyle.FixedSingle;
                                panel.Controls.Add(label);
                                panel.Controls.Add(textBox);

                                flowLayoutPanel1.Controls.Add(panel);
                            }
                        }
                    }
                }
            }
        }















    }
}
