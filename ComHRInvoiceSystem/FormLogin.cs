using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//extra references
using MySql.Data.MySqlClient;
using databaseDLL;
using System.Collections;

namespace ComHRInvoiceSystem
{
    public partial class FormLogin : Form
    {
        //Create User object
        User userLocal = new User();

        public FormLogin()
        {
            InitializeComponent();
            txtBoxPassword.PasswordChar = '*';
            //Test case, to check if user's ID and password are correct
            //MessageBox.Show(user01.getUserID() + ", " + user01.getPassword());
        }

        //Trigger login button
        private void btnLogin_Click(object sender, EventArgs e)
        {

            checkDifferentInput();
            //Create User to login
            userLocal.setUser(txtBoxID.Text, txtBoxPassword.Text);

            //Open the connection function
            //openTheConnection();

            //Passing values to verify user
            loginAuthentication();
        }

        //When user trigger 'enter' on password textbox
        private void txtBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(this, new EventArgs());
            }//END if
        }

        public void checkDifferentInput()
        {
            if (txtBoxID.Text != userLocal.getUserID())
            {
                userLocal.setLoginAttempt(4);
            }
        }


        //Method to check user's authentication
        public void loginAuthentication()
        {         
            Boolean isMatch = false;   //To find matched user
            Boolean isFreeze = false;    //To identify user status
            User userTemp;

            //MySqlCommand cmdUpdateLoginStatus = ConnectionState.CreateCommand();
            //cmdUpdateLoginStatus.CommandText = "UPDATE ";

            MySqlConnection connection = databaseConnection.openConnection();

            try
            {
                //SQL COMMAND to obtain information
                MySqlCommand cmdLogin = connection.CreateCommand();
                cmdLogin.CommandText = "Select * FROM user";

                //Data Adapter to run COMMAND
                MySqlDataAdapter loginAdapter = new MySqlDataAdapter(cmdLogin);
                DataSet loginDS = new DataSet();

                loginAdapter.Fill(loginDS);

                for (int counter = 0; counter < loginDS.Tables[0].Rows.Count  ; counter++)
                {
                    userTemp = new User(loginDS.Tables[0].Rows[counter].ItemArray[0].ToString(), 
                                        loginDS.Tables[0].Rows[counter].ItemArray[1].ToString());

                    if (userLocal.getUserID() == userTemp.getUserID() && 
                        userLocal.getPassword() == userTemp.getPassword())
                    {
                        //User did NOT failed to login more than 4 times
                        if (userLocal.getLoginAttempt() != 0)
                        {
                            //User login success, user is valid
                            isMatch = true;
                        }//END IF

                        //User's username and password matched with the system,
                        //but user have failed to login more or equal to 4 times.
                        else
                        {
                            isFreeze = true;
                        }//END ELSE
                    }//END IF 
                }//END for

                //Display messages
                if (isMatch == true && isFreeze != true)
                {
                    //User is valid, status not freezed
                    MessageBox.Show("Login Successful." +
                                    "\nPlease wait while the system is preparing for you.");

                    //User granted permission to the main system
                    this.Hide();
                    FormHomePage formHome = new FormHomePage();
                    formHome.Closed += (s, arg) => this.Close();
                    formHome.Show();
                }//END if
                else //Wrong username or password
                {
                    userLocal.setLoginAttempt(userLocal.getLoginAttempt() - 1);

                    //Status if Freeze or 
                    if (isFreeze == true || userLocal.getLoginAttempt() <= 0)
                    {
                        MessageBox.Show("Your User ID have been freeze." +
                                    "Please contact IT Department.");

                        //Update Query
                        String qrySuspendUser = "UPDATE user ";
                              qrySuspendUser += "SET userStatus = true ";
                              qrySuspendUser += "WHERE userID ='" + userLocal.getUserID() + "';";

                        MySqlCommand cmdSuspendUser = new MySqlCommand(qrySuspendUser, connection);
                        MySqlDataReader theReader;

                        theReader = cmdSuspendUser.ExecuteReader();

                    }
                    else
                    {
                        MessageBox.Show("Login failed. ID or password might be incorrect." +
                                        "\nLogin Attempt Left: " + Convert.ToString(userLocal.getLoginAttempt()));
                    }
                }

            }//END try
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }//END catch
            finally //Close Connection
            {
                //Close the connection
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }//END if
            }//END finally

        }//END BUTTON

        
    }//END CLASS
}//END FORM
