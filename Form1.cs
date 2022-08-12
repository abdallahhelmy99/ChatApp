using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using MySql.Data.Common;
using MySqlX.Protocol;
using System.Data.SqlClient;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ChatApp_DB
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            string connetionString = null;
            MySqlConnection cnn;
            connetionString = "server=localhost;Port=3306;Database=chatapp;Uid=Abdallah;pwd=Aliakhaled_2002;";
            cnn = new MySqlConnection(connetionString);
            try
            {
                cnn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed Connecting To Database.");
            }
            //
            MySql.Data.MySqlClient.MySqlCommand sqlCommand = new MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM users", cnn);
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                User f = new User();
                f.fname = (string)reader["fname"];
                f.lname = (string)reader["lname"];
                f.userID = (int)reader["user_id"];
                f.mobnum = (string)reader["phone_num"];
                f.userPass = (string)reader["password"];
                f.desc = (string)reader["userDescription"];
                Temp.users.Add(f);
            }
            

        }
        private void txtbox_phone_Click_1(object sender, EventArgs e)
        {
            txtbox_phone.Text = "";
        }

        private void txtbox_pass_Click_1(object sender, EventArgs e)
        {
            txtbox_pass.Text = "";
        }

        private void materialButton2_Click_1(object sender, EventArgs e)
        {
            login_pn.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            int x = Temp.users.Count;
            for (int i = 0; i < Temp.users.Count; i++)
            {
                if (txtbox_phone.Text == Temp.users[i].mobnum && txtbox_pass.Text == Temp.users[i].userPass)
                {
                    Temp.globalid = i;
                    int u = Temp.chatrooms.Count;
                    u++;
                    
                        User.loadChatrooms(Temp.globalid);
                        User.tempload();
                    User.loadMessages(Temp.globalid);
                    
                    for (int j = 0; j < u; j++)
                    {
                        if (Temp.chatrooms.Count != 0)
                        {

                            if (ListBox1.Items.Equals(Temp.chatrooms[j].name))
                                continue;
                            else
                                ListBox1.AddItem(Temp.chatrooms[j].name);

                        }
                    }
                    main_pn.BringToFront();
                    break;
                }
                else if (txtbox_phone.Text != Temp.users[i].mobnum && txtbox_pass.Text != Temp.users[i].userPass && i == x)
                {
                    MessageBox.Show("Wrong Credentials, Please Try Again");
                    break;
                }
            }
        }

        private void materialButton4_Click_1(object sender, EventArgs e)
        {
            welcom_pn.BringToFront();
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            welcom_pn.BringToFront();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            signup_pn.BringToFront();
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            string connetionString = null;
            MySqlConnection cnn;
            connetionString = "server=localhost;Port=3306;Database=chatapp;Uid=Abdallah;pwd=Aliakhaled_2002;";
            cnn = new MySqlConnection(connetionString);
            try
            {
                cnn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Internet, Check Your Internet Connection.");
            }
            int x = Temp.users.Count;
            User f = new User();
            f.fname = materialTextBox23.Text;
            f.lname = materialTextBox24.Text;
            f.userID = x+1;
            f.mobnum = materialTextBox21.Text;
            f.userPass = materialTextBox22.Text;
            Temp.users.Add(f);
            x++;
            string Command = "INSERT INTO users (user_id, fname, lname, password, phone_num) VALUES (@userID, @FirstName, @LastName, @pass, @phnum);";
            using (MySqlCommand myCmd = new MySqlCommand(Command, cnn)){
                myCmd.CommandType = CommandType.Text;
                myCmd.Parameters.AddWithValue("@userID", x+1);
                myCmd.Parameters.AddWithValue("@FirstName", materialTextBox23.Text);
                myCmd.Parameters.AddWithValue("@LastName", materialTextBox24.Text);
                myCmd.Parameters.AddWithValue("@phnum", materialTextBox21.Text);
                myCmd.Parameters.AddWithValue("@pass", materialTextBox22.Text);
                myCmd.ExecuteNonQuery();
                MessageBox.Show("Registration Successfull !");
            }
        }

        private void materialButton9_Click(object sender, EventArgs e)
        {
            Temp.contacts.Clear();
            materialListBox2.Clear();
            main_pn.BringToFront();
            
        }

        private void materialFloatingActionButton1_Click(object sender, EventArgs e) //zorar el contacts
        {
            User.loadContacts(Temp.globalid);
            contacts_pn.BringToFront();
            
            int u = Temp.contacts.Count;
            for (int j = 0; j < u; j++)
            {
              if ( materialListBox2.Items.Count == u )
              {
                continue;
              }
              else
                   materialListBox2.AddItem(Temp.contacts[j].fname);
            }
        
        }  

        private void materialButton11_Click(object sender, EventArgs e)
        {
            contacts_pn.BringToFront();
        }

        private void materialButton8_Click(object sender, EventArgs e) //new contact save button
        {
            Contact x = new Contact();
            x.fname = materialTextBox26.Text;
            x.lname = materialTextBox25.Text;
            x.phone_num = materialTextBox28.Text;
            x.his_id = Int32.Parse(materialTextBox27.Text);

            for (int i = 0; i < Temp.users.Count; i++)
            {
                if (materialTextBox27.Text == Temp.users[i].userID.ToString())
                {
                    
                    string connetionString = null;
                    MySqlConnection cnn;
                    connetionString = "server=localhost;Port=3306;Database=chatapp;Uid=Abdallah;pwd=Aliakhaled_2002;";
                    cnn = new MySqlConnection(connetionString);
                    try
                    {
                        cnn.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No Internet, Check Your Internet Connection.");
                    }

                    string Command = "INSERT INTO contacts (fname, lname, phnum, usid_con, contact_id) VALUES (@FirstName, @LastName, @phnum , @uid ,@contid);";
                    using (MySqlCommand myCmd = new MySqlCommand(Command, cnn))
                    {
                        myCmd.CommandType = CommandType.Text;
                        myCmd.Parameters.AddWithValue("@FirstName", materialTextBox26.Text);
                        myCmd.Parameters.AddWithValue("@LastName", materialTextBox25.Text);
                        myCmd.Parameters.AddWithValue("@phnum", materialTextBox28.Text);
                        myCmd.Parameters.AddWithValue("@uid", Temp.users[Temp.globalid].userID);
                        myCmd.Parameters.AddWithValue("@contid", materialTextBox27.Text);
                        myCmd.ExecuteNonQuery();
                        MessageBox.Show("Contact Added Successfully !");
                    }
                }
                else if (i == Temp.users.Count && materialTextBox27.Text != Temp.users[i].userID.ToString() )
                {
                    MessageBox.Show("Contact Isn't Using This App, You Can Invite Him.");
                    break;
                }
            }
        }

        private void materialButton7_Click(object sender, EventArgs e)
        {
            newcontact_pn.BringToFront();
        }

        private void materialFloatingActionButton3_Click(object sender, EventArgs e)
        {
            Temp.chatrooms.Clear();
            Temp.contacts.Clear();
            Temp.loadedrooms = false;
            ListBox1.Items.Clear();
            materialListBox1.Clear();
            welcom_pn.BringToFront();

        }

        private void materialButton10_Click(object sender, EventArgs e)
        {
            materialListBox2.Items.Remove(materialListBox2.SelectedItem);
            
            for (int i = 0; i <= Temp.contacts.Count; i++)
            {
                if (materialListBox2.SelectedItem.Text.StartsWith(Temp.contacts[i].fname))
                {
                    Temp.contacts.RemoveAt(i);
                }
            }
            
            string connetionString = null;
            MySqlConnection cnn;
            connetionString = "server=localhost;Port=3306;Database=chatapp;Uid=Abdallah;pwd=Aliakhaled_2002;";
            cnn = new MySqlConnection(connetionString);
            try
            {
                cnn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Internet, Check Your Internet Connection.");
            }
            MySql.Data.MySqlClient.MySqlCommand sqlCommand = new MySql.Data.MySqlClient.MySqlCommand("delete from contacts where fname=@name", cnn);
            sqlCommand.Parameters.Add("@name", MySqlDbType.String);
            sqlCommand.Parameters["@name"].Value = materialListBox2.SelectedItem.Text;
            MySqlDataReader reader = sqlCommand.ExecuteReader();
            
        }

        private void newchat_btn_Click(object sender, EventArgs e)
        {
            User.loadContacts(Temp.globalid);
            choosecontact_pn.BringToFront();
            int u = Temp.contacts.Count;
            for (int j = 0; j < u; j++)
            {
                if (materialListBox1.Items.Count == u)
                {
                    continue;
                }
                else
                    materialListBox1.AddItem(Temp.contacts[j].fname);
            }

        }

        private void materialButton13_Click(object sender, EventArgs e)
        {
            main_pn.BringToFront();
        }

        private void materialButton12_Click(object sender, EventArgs e) //CREATE NEW CHAT ROOM
        {
            int glob = 0;
            chat_pn.BringToFront();
            materialLabel10.Text = materialListBox1.SelectedItem.Text;
            ChatRoom x = new ChatRoom();
            x.roomID = Temp.chatrooms.Count + 1;
            Temp.chatroomid = x.roomID;
            x.roomType = "Single";
            x.name = materialListBox1.SelectedItem.Text;
            Temp.roomname = x.name;
            Temp.chatrooms.Add(x);

            for (int i = 0; i < Temp.contacts.Count; i++)
            {
                if (materialListBox1.SelectedItem.Text == Temp.contacts[i].fname)
                {
                    glob = i;
                    break;
                }
            }

            string connetionString = null;
            MySqlConnection cnn;
            connetionString = "server=localhost;Port=3306;Database=chatapp;Uid=Abdallah;pwd=Aliakhaled_2002;";
            cnn = new MySqlConnection(connetionString);
            try
            {
                cnn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Internet, Check Your Internet Connection.");
            }

            string Command = "INSERT INTO chatrooms (room_id, name, room_type) VALUES (@roomid, @Name, @type);";
            using (MySqlCommand myCmd = new MySqlCommand(Command, cnn))
            {
                myCmd.CommandType = CommandType.Text;
                myCmd.Parameters.AddWithValue("@roomid", x.roomID);
                myCmd.Parameters.AddWithValue("@Name", x.name);
                myCmd.Parameters.AddWithValue("@type", x.roomType);
                myCmd.ExecuteNonQuery();
                MessageBox.Show("Chat Created Successfully !");
            }

            string Commande = "INSERT INTO chatrooms (room_id, name, room_type) VALUES (@roomid, @Name, @type);";
            using (MySqlCommand myCmd = new MySqlCommand(Commande, cnn))
            {
                myCmd.CommandType = CommandType.Text;
                myCmd.Parameters.AddWithValue("@roomid", x.roomID);
                myCmd.Parameters.AddWithValue("@Name", Temp.users[Temp.globalid].fname);
                myCmd.Parameters.AddWithValue("@type", x.roomType);
                myCmd.ExecuteNonQuery();
                
            }

            string Com = "INSERT INTO user_room_relation (usID, rmID) VALUES (@id, @roomid);";
            using (MySqlCommand myCmd = new MySqlCommand(Com, cnn))
            {
                myCmd.CommandType = CommandType.Text;
                myCmd.Parameters.AddWithValue("@id", Temp.users[Temp.globalid].userID);
                myCmd.Parameters.AddWithValue("@roomid", x.roomID);
                myCmd.ExecuteNonQuery();
            }

            string Com1 = "INSERT INTO user_room_relation (usID, rmID) VALUES (@id, @roomid);";
            using (MySqlCommand myCmd = new MySqlCommand(Com1, cnn))
            {
                myCmd.CommandType = CommandType.Text;
                myCmd.Parameters.AddWithValue("@id", Temp.contacts[glob].his_id);
                myCmd.Parameters.AddWithValue("@roomid", x.roomID);
                myCmd.ExecuteNonQuery();
            }

            

        }

        private void materialFloatingActionButton4_Click(object sender, EventArgs e)
        {
            main_pn.BringToFront();
            
            
        }
        private void send_Click_1(object sender, EventArgs e)
        {
            if ( materialTextBox1.Text != "")
            {
                materialListBox3.AddItem(Temp.users[Temp.globalid].fname + " : " + materialTextBox1.Text);
            }

            for (int i = 0; i < Temp.chatrooms.Count; i++) {
                
                if ( materialLabel10.Text == Temp.chatrooms[i].name)
                {
                    Temp.chatroomid = Temp.chatrooms[i].roomID;
                }
             
            }

            string connetionString = null;
            MySqlConnection cnn;
            connetionString = "server=localhost;Port=3306;Database=chatapp;Uid=Abdallah;pwd=Aliakhaled_2002;";
            cnn = new MySqlConnection(connetionString);
            try
            {
                cnn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Internet, Check Your Internet Connection.");
            }

            string Command = "INSERT INTO messages (text, chid, uid) VALUES (@text, @roomid, @usid);";
            using (MySqlCommand myCmd = new MySqlCommand(Command, cnn))
            {
                myCmd.CommandType = CommandType.Text;
                myCmd.Parameters.AddWithValue("@text", materialTextBox1.Text);
                myCmd.Parameters.AddWithValue("@roomid", Temp.chatroomid);
                myCmd.Parameters.AddWithValue("@usid", Temp.users[Temp.globalid].userID);
                myCmd.ExecuteNonQuery();
                MessageBox.Show("Message Sent Successufully !");
            }


        }

        private void ListBox1_SelectedIndexChanged(object sender, MaterialSkin.MaterialListBoxItem selectedItem)
        {
            chat_pn.BringToFront();
            materialLabel10.Text = ListBox1.SelectedItem.Text;

            

            for (int i = 0; i < Temp.chatrooms.Count; i++) {
                    
                if ( Temp.chatrooms[i].name == ListBox1.SelectedItem.Text)
                {
                    for (int j = 0; j < Temp.chatrooms[i].messages.Count; j++)
                    {
                        if (Temp.chatrooms[i].messages[j].userID == Temp.users[Temp.globalid].userID)
                        {
                            materialListBox3.AddItem(Temp.users[Temp.globalid].fname+ " : " +Temp.chatrooms[i].messages[j].text);
                        }
                        else
                            materialListBox3.AddItem(materialLabel10.Text + " : " + Temp.chatrooms[i].messages[j].text);
                    }
                }
            }
                    
            
        }

        private void materialFloatingActionButton5_Click(object sender, EventArgs e)
        {
            materialListBox3.RemoveItem(materialListBox3.SelectedItem);
        }

        private void materialLabel13_Click(object sender, EventArgs e)
        {
            
        }

        private void materialFloatingActionButton2_Click(object sender, EventArgs e)
        {
            profile_pn.BringToFront();
            materialLabel15.Text = Temp.users[Temp.globalid].desc;

        }

        private void materialButton14_Click(object sender, EventArgs e)
        {
            Temp.users[Temp.globalid].desc = materialTextBox29.Text;
        }

        private void materialButton15_Click(object sender, EventArgs e)
        {
            main_pn.BringToFront();
        }
    }
}
