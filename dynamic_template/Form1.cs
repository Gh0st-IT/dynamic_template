using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dynamic_template
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected List<Control> controlList = new List<Control>();

        private StringBuilder jsonBuilder = new StringBuilder("[");

        private void AppendWithComma(string labelValue)
        {
            if (jsonBuilder.Length > 1)
            {
                jsonBuilder.Append(",");
            }

            jsonBuilder.Append($"{{\"Panel\": {{\"Label\":\"{labelValue}\", \"TextBox\":\"\"}}}}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create controls
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            label.Name = $"label{controlList.Count}";
            label.Size = new Size(35, 13);
            label.Text = $"Label {controlList.Count}";
            label.Dock = DockStyle.Top;

            TextBox textBox = new TextBox();
            textBox.Name = $"textBox{controlList.Count}";
            textBox.Size = new Size(100, 20);
            textBox.Dock = DockStyle.Bottom;

            Panel panel = new Panel();
            panel.Name = $"panel{controlList.Count}";
            panel.BorderStyle = BorderStyle.FixedSingle;

            panel.Controls.Add(label);
            panel.Controls.Add(textBox);

            controlList.Add(panel);

            // Build JSON object for each panel
            AppendWithComma(label.Text);

            flowLayoutPanel1.Controls.Add(panel);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            jsonBuilder.Append("]");
            Console.WriteLine(jsonBuilder.ToString());

            // Assuming conn is your SqlConnection object
            using (SqlConnection conn = new SqlConnection($"Data Source=DESKTOP-RCNCNNE\\MSSQLSERVER01;Initial Catalog=DynamicTemplate;Integrated Security=True;"))
            {
                conn.Open();

                // Create a SqlCommand with parameterized query
                string query = "INSERT INTO template (template) VALUES (@template)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Assuming jsonBuilder is your StringBuilder
                    cmd.Parameters.AddWithValue("@template", jsonBuilder.ToString());

                    // Execute the command
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                }
            }

            jsonBuilder.Clear();
            jsonBuilder.Append("[");
        }



        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
