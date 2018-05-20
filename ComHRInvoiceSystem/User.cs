using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Header
==========================================
This is the User class.
==========================================
In this case of the invoice system, we will only have one user, 
which is "nichola.kerr". Hence the userID will not have a mutator
to update it.

 This class consi variables of:
 (1) userID,                a String to store user's ID
 (2) password,              a String to store user's password
 (3) userStatus,            if the Boolean return false, userID has been freeze
 (4) loginStatus,           if the Boolean return ture, this user is currently online
 (5) userLoginAttempt       an int to track the attempt user were given to fail the login

This class has:
(1) 4 Constuctors
    - default Constructor
    - alternate Constructor that take password as parameter.
    - alternate Constructor that take userStatus and loginStatus as parameters.
    - copy Consturctor 
    
(2) 4 Mutators
    - setUser()     to get the ability to update existing User object 
    - setPassword() allow user to change it's password
    - setUserSatus()   to keep track of user authentication
    - setLoginStatus() to update if user successfully login to the system
 
(3) Accessor for all variables for internal use and external use.

(4) A toString method to print userID and Password for testing purposes.
================================================================================
*/


namespace ComHRInvoiceSystem
{
    class User
    {
        private String userID;
        private String password;
        private Boolean isSuspended;
        private int loginAttempt;

        //Default Constructor 
        public User()
        {
            userID = "";
            password = "";
            isSuspended = false;
            loginAttempt = 4;
        }

        //Alternative Constructor with userID and password as parameters
        public User(String inUserID, String inPassword)
        {
            userID = inUserID;
            password = inPassword;
            isSuspended = false;
            loginAttempt = 4;
        }

        //Copy Constructor 
        public User(User user)
        {
            userID = user.getUserID();
            password = user.getPassword();
            isSuspended = user.getIsSuspended();
            loginAttempt = user.getLoginAttempt();           
        }

        //Mutator of the object. To update overall fields.
        public void setUser(String inUserID, String inPassword)
        {
            setUserID(inUserID);
            setPassword(inPassword);
        }

        //Update userID
        public void setUserID(String inUserID)
        {
            userID = inUserID;
        }

        //Update password.
        public void setPassword(String inPassword)
        {
            //Allow user to change password
            password = inPassword;
        }

        //Update userStatus
        public void setIsSuspended(Boolean inIsSuspended)
        {
            isSuspended = inIsSuspended;
        }

        //Update loginAttempt
        public void setLoginAttempt(int inLoginAttempt)
        {
            loginAttempt = inLoginAttempt;
        }


        //Accessors
        public String getUserID()
        {
            return userID;
        }

        public String getPassword()
        {
            return password;
        }

        public Boolean getIsSuspended()
        {
            return isSuspended;
        }

        public int getLoginAttempt()
        {
            return loginAttempt;
        }

        //toString method.
        public String printInfo()
        {
            String printUserDetails;

            printUserDetails = "UserID: " + getUserID() +
                               "\nStatus: " + getIsSuspended();

            return printUserDetails;
        }
    }
}
