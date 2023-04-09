using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Form_Lab1
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        string connectionString;
        string parentTable;
        string childTable;
        string parentKey;
        string childKey;
        string foreignKey;
        List<String> parentColumns = new List<String>();
        List<String> childColumns = new List<String>();

        // initializing the form
        public Form1()
        {
            InitializeComponent();
            configureForm();
            configureDropDown();
            inputReset();
        }

        // configuring the version-current Year labels
        private void configureForm()
        {
            // current year
            string currentYear = DateTime.Now.Year.ToString();
            yearLabel.Text = "© " + currentYear;

            // current version
            currentVersion.Text = ConfigurationManager.AppSettings["currentVersion"];

            // blocking edit on grid view
            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;
        }

        // building the dropdown menu names from app.config
        private void configureDropDown()
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var sectionKey in configuration.Sections.Keys)
            {
                var section = configuration.GetSection(sectionKey.ToString());
                var appSettings = section as AppSettingsSection;
                if (appSettings == null) continue;
                foreach (var key in appSettings.Settings.AllKeys)
                {
                    if (key.ToString() == "table")
                    {
                        tableDropDown.Items.Add(appSettings.Settings[key].Value);
                    }
                }
            }
            tableDropDown.SelectedIndex = 0;
        }

        // selecting the corresponding table name from the dropdown menu
        private void tableDropDown_SelectTable(object sender, EventArgs e)
        {
            String str = tableDropDown.Text;

            // separating the string into parent and child table
            char[] spearator = { '_' };
            String[] strlist = str.Split(spearator, StringSplitOptions.RemoveEmptyEntries);

            parentTable = strlist[0];
            childTable = strlist[1];
        }

        // loading configuration and information about the table
        private void configure()
        {
            // configuration data is loaded from the App.config file
            connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            connection = new SqlConnection(connectionString);
            SqlCommand sqlCommand;

            // string for an sql command which selects the name of the parent table's key
            string selectParentKey = "select COLUMN_NAME " + "from INFORMATION_SCHEMA.KEY_COLUMN_USAGE " + "where TABLE_NAME like '" + parentTable + "' " + "and CONSTRAINT_NAME like 'PK%'";

            // string for an sql command which selects the name of the child table's key
            string selectChildKey = "select COLUMN_NAME " + "from INFORMATION_SCHEMA.KEY_COLUMN_USAGE " + "where TABLE_NAME like '" + childTable + "' " + "and CONSTRAINT_NAME like 'PK%'";

            // string for an sql command which selects the name of the child table's foreign key
            string selectForeignKey = "select COLUMN_NAME " + "from INFORMATION_SCHEMA.KEY_COLUMN_USAGE " + "where TABLE_NAME like '" + childTable + "' " + "and CONSTRAINT_NAME like 'FK%'";

            // string for an sql command which selects the names of the parent table's columns
            string selectParentColumns = "select COLUMN_NAME " + "from INFORMATION_SCHEMA.COLUMNS " + "where TABLE_NAME = '" + parentTable + "'";

            // string for an sql command which selects the names of the child table's columns
            string selectChildColumns = "select COLUMN_NAME " + "from INFORMATION_SCHEMA.COLUMNS " + "where TABLE_NAME = '" + childTable + "'";

            try
            {
                connection.Open();

                // parent table's key is selected,
                // result is processed as a scalar
                sqlCommand = new SqlCommand(selectParentKey, connection);
                parentKey = (string)sqlCommand.ExecuteScalar();

                // child table's key is selected,
                // result is processed as a scalar
                sqlCommand = new SqlCommand(selectChildKey, connection);
                childKey = (string)sqlCommand.ExecuteScalar();

                // child table's foreign key is selected,
                // result is processed as a scalar
                sqlCommand = new SqlCommand(selectForeignKey, connection);
                foreignKey = (string)sqlCommand.ExecuteScalar();

                // parent table's columns are selected,
                // result is transformed into a list of strings
                sqlCommand = new SqlCommand(selectParentColumns, connection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    if (parentColumns.Count == 0)
                        parentColumns.Add(sqlDataReader.GetString(0));
                }
                sqlDataReader.Close();

                // child table's columns are selected,
                // result is transformed into a list of strings
                sqlCommand = new SqlCommand(selectChildColumns, connection);
                sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    childColumns.Add(sqlDataReader.GetString(0));
                }
                sqlDataReader.Close();

                // connection is closed
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                connection.Close();
            }
        }

        // creates the necessary labels and text boxes
        private void buildPanelInput()
        {
            // an adequate number of labels and text boxes must be created
            for (int i = 0; i < childColumns.Count; i++)
            {
                // each label displays the name of its respective column
                Label label = new Label();
                label.Name = childColumns[i];
                label.Text = childColumns[i] + ":";
                panel1.Controls.Add(label);

                // each text box is named after its respective column
                TextBox textBox = new TextBox();
                textBox.Name = childColumns[i];
                panel1.Controls.Add(textBox);

                label.Location = new System.Drawing.Point(0, 4 + 24 * i);
                textBox.Location = new System.Drawing.Point(120, 24 * i);
            }
        }

        // disables all text fields and buttons, clears text
        private void inputReset()
        {
            foreach (Control control in panel1.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                    ((TextBox)control).Enabled = false;
                }
            }

            insertButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
        }

        // clears all controls within the panel and initialize the parent/child columns
        private void panelReset()
        {
            parentColumns.Clear();
            childColumns.Clear();
            panel1.Controls.Clear();
        }

        // loads and displays data from parent -> child table (mapped to the "Load Database" button)
        private void loadDatabaseClick(object sender, EventArgs e)
        {

            inputReset();
            panelReset();
            configure();
            buildPanelInput();

            DataSet dataSet = new DataSet();

            SqlDataAdapter parentAdapter = new SqlDataAdapter("select * from " + parentTable, connectionString);
            SqlDataAdapter childAdapter = new SqlDataAdapter("select * from " + childTable, connectionString);

            parentAdapter.Fill(dataSet, "parent");
            childAdapter.Fill(dataSet, "child");

            DataRelation dataRelation = new DataRelation("FK_parent_child", dataSet.Tables["parent"].Columns[parentKey], dataSet.Tables["child"].Columns[foreignKey]);

            try
            {
                dataSet.Relations.Add(dataRelation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error while displaying the database. For more information, read the message below:\n \n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            BindingSource bindingSource1 = new BindingSource();
            bindingSource1.DataSource = dataSet;
            bindingSource1.DataMember = "parent";

            BindingSource bindingSource2 = new BindingSource();
            bindingSource2.DataSource = bindingSource1;
            bindingSource2.DataMember = "FK_parent_child";

            // saving the last selected row for refreshing the database
            int saveRow = 0;
            if (dataGridView1.Rows.Count > 0 && dataGridView1.CurrentCell != null)
                saveRow = dataGridView1.CurrentCell.RowIndex;
            listBox1.DataSource = bindingSource1;
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "name";
            dataGridView2.DataSource = bindingSource2;
            if (saveRow != 0 && saveRow < dataGridView1.Rows.Count)
                dataGridView1.CurrentCell = dataGridView1.Rows[saveRow].Cells[0];
        }

        //private void listBox1_CellClick(object sender, Lisbo)

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                inputReset();

                // searching for the parent key's position
                int keyPosition = -1;

                for (int i = 0; i < parentColumns.Count; i++)
                {
                    if (parentColumns[i].Equals(parentKey))
                        keyPosition = i;
                }

                // the parent key's value is selected from the clicked row
                string parentKeyValue = dataGridView1[keyPosition, dataGridView1.CurrentCell.RowIndex].Value.ToString();

                // clicking an invalid row must not enable interaction or fill in information -> the foreign key text box needs to be filled and remains disabled
                if (!String.IsNullOrWhiteSpace(parentKeyValue))
                {
                    foreach (Control control in panel1.Controls)
                    {
                        if (control is TextBox)
                        {
                            if (control.Name.Equals(foreignKey))
                            {
                                ((TextBox)control).Text = parentKeyValue;
                                ((TextBox)control).Enabled = false;
                            }
                            else
                            {
                                ((TextBox)control).Clear();
                                ((TextBox)control).Enabled = true;
                            }
                        }
                    }
                    insertButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error while displaying the database. For more information, read the message below:\n \n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                inputReset();

                int rowNumber = dataGridView2.CurrentCell.RowIndex;

                // searching for the child key's and foreign key's position
                int keyPosition = -1;
                int foreignKeyPosition = -1;

                for (int i = 0; i < childColumns.Count; i++)
                {
                    if (childColumns[i].Equals(childKey))
                        keyPosition = i;
                    if (childColumns[i].Equals(foreignKey))
                        foreignKeyPosition = i;
                }

                // clicking an invalid row must not enable interaction or fill in information -> the foreign key text box needs to be filled and remains disabled
                if (!String.IsNullOrWhiteSpace(dataGridView2[keyPosition, rowNumber].Value.ToString()))
                {
                    int controlNumber = 0;
                    foreach (Control control in panel1.Controls)
                    {
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = dataGridView2[controlNumber, rowNumber].Value.ToString();
                            ((TextBox)control).Enabled = true;
                            controlNumber++;

                            if (control.Name.Equals(childKey) || control.Name.Equals(foreignKey))
                            {
                                ((TextBox)control).Enabled = false;
                            }
                        }
                    }
                    updateButton.Enabled = true;
                    deleteButton.Enabled = true;
                }
                // if an invalid row was clicked, the foreign key may be filled in as it is still relevant
                else
                {
                    foreach (Control control in panel1.Controls)
                    {
                        if (control is TextBox)
                        {
                            if (control.Name.Equals(foreignKey))
                            {
                                ((TextBox)control).Text = dataGridView2[foreignKeyPosition, rowNumber]
                                    .Value.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error while displaying the database. For more information, read the message below:\n \n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // inserts a new tuple into the child table (mapped to the "Insert" button) 
        private void insertClick(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter();

                List<string> parametrs = new List<string>();
                foreach (string childColumn in childColumns)
                {
                    parametrs.Add("@" + childColumn);
                }
                // building the insert command's string
                string insert = "insert into " + childTable + " values(" + string.Join(", ", parametrs) + ")";
                dataAdapter.InsertCommand = new SqlCommand(insert, connection);

                // extracting parameter values from the text boxes
                foreach (Control control in panel1.Controls)
                {
                    if (control is TextBox)
                    {
                        if (!String.IsNullOrWhiteSpace(((TextBox)control).Text))
                        {
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@" + control.Name, ((TextBox)control).Text);
                        }
                        // empty text boxes message
                        else
                        {
                            MessageBox.Show("Please fill in the '" + control.Name + "' field.", "Invalid Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            connection.Close();
                            return;
                        }
                    }
                }

                connection.Open();
                dataAdapter.InsertCommand.ExecuteNonQuery();
                // reloading the data base
                inputReset();
                loadDatabaseClick(sender, e);
                MessageBox.Show("A new entry was successfully added.", "Insert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error while trying to insert a new entry. For more information, read the message below:\n \n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();
            }
        }

        // updates a tuple from the child table (mapped to the "Update" button) 
        private void updateClick(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter();

                List<string> parametrs = new List<string>();
                foreach (string childColumn in childColumns)
                {
                    if (childColumn != childKey && childColumn != foreignKey)
                        parametrs.Add(childColumn + " = @" + childColumn);
                }

                // building the update command's string
                string update = "update " + childTable + " set " + string.Join(", ", parametrs) + " where " + childKey + "= @" + childKey;
                dataAdapter.UpdateCommand = new SqlCommand(update, connection);

                // extracting parameter values from the text boxes
                foreach (Control control in panel1.Controls)
                {
                    if (control is TextBox)
                    {
                        // the foreign key is not part of this command's parameter set
                        if (control.Name != foreignKey)
                        {
                            if (!String.IsNullOrWhiteSpace(((TextBox)control).Text))
                            {
                                dataAdapter.UpdateCommand.Parameters.AddWithValue("@" + control.Name, ((TextBox)control).Text);
                            }
                            // empty text boxes message
                            else
                            {
                                MessageBox.Show("Please fill in the '" + control.Name + "' field.", "Invalid Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                connection.Close();
                                return;
                            }
                        }
                    }
                }
                connection.Open();
                dataAdapter.UpdateCommand.ExecuteNonQuery();
                // reloading the data base
                inputReset();
                loadDatabaseClick(sender, e);
                MessageBox.Show("The selected entry was successfully updated.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error while trying to update the selected entry. For more information, read the message below:\n \n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();
            }
        }

        // deletes a tuple from the child table (mapped to the "Delete" button) 
        private void deleteClick(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the selected entry?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                SqlConnection connection = new SqlConnection(connectionString);

                try
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();

                    // building the delete command's string
                    dataAdapter.DeleteCommand = new SqlCommand("delete from " + childTable + " where " + childKey + "= @" + childKey, connection);

                    // searching for the key's value
                    foreach (Control control in panel1.Controls)
                    {
                        if (control is TextBox)
                        {
                            if (control.Name.Equals(childKey))
                            {
                                dataAdapter.DeleteCommand.Parameters.AddWithValue("@" + childKey, ((TextBox)control).Text);
                            }
                        }
                    }
                    connection.Open();
                    dataAdapter.DeleteCommand.ExecuteNonQuery();
                    connection.Close();
                    // reloading the database
                    inputReset();
                    loadDatabaseClick(sender, e);
                    MessageBox.Show("The selected entry was successfully deleted.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error while trying to delete the selected entry. For more information, read the message below.\n \n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connection.Close();
                }
            }
        }

        // function for warning the user before closing
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            foreach (Control control in panel1.Controls)
            {
                if (control is TextBox)
                {
                    if (!String.IsNullOrWhiteSpace(((TextBox)control).Text))
                    {
                        if (MessageBox.Show("You still have data in your fields. Are you sure you want to close the form?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                        {
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            e.Cancel = false;
                            return;
                        }
                    }
                }
            }
            if (MessageBox.Show("Are you sure you want to close the form?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                e.Cancel = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                inputReset();

                // searching for the parent key's position
                int keyPosition = -1;

                for (int i = 0; i < parentColumns.Count; i++)
                {
                    if (parentColumns[i].Equals(parentKey))
                        keyPosition = i;
                }

                // the parent key's value is selected from the clicked row
                //string parentKeyValue = listBox1[keyPosition, listBox1.SelectedItem].Value.ToString();
                //string parentKeyValue = listBox1[keyPosition, ];
                string parentKeyValue = listBox1.SelectedItem.ToString();

                // clicking an invalid row must not enable interaction or fill in information -> the foreign key text box needs to be filled and remains disabled
                if (!String.IsNullOrWhiteSpace(parentKeyValue))
                {
                    foreach (Control control in panel1.Controls)
                    {
                        if (control is TextBox)
                        {
                            if (control.Name.Equals(foreignKey))
                            {
                                ((TextBox)control).Text = parentKeyValue;
                                ((TextBox)control).Enabled = false;
                            }
                            else
                            {
                                ((TextBox)control).Clear();
                                ((TextBox)control).Enabled = true;
                            }
                        }
                    }
                    insertButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error while displaying the database. For more information, read the message below:\n \n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}



