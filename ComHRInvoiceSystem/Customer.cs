using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHRInvoiceSystem
{
    class Customer
    {
        private String customerFirstName;
        private String customerLastName;
        private String customerID;
        private String gender;
        private String landline;
        private String mobile;
        private String fax;
        private String emailAddress;
        private String dateOfBirth;

        //Creating an array to store address and invoice
        private Address[] addressArray;
        private Invoice[] invoiceArray;
        int indexInvoice = 0;

        #region /*All Constructors*/
        //Default Constructor
        public Customer()
        {
            customerID = "";
            customerFirstName = "";
            customerLastName = "";
            gender = "";
            landline = "";
            mobile = "";
            fax = "";
            emailAddress = ""; 
            dateOfBirth = "";

            //initialising array
            addressArray = new Address[2];
            invoiceArray = new Invoice[50];
        }

        //Alternate Contructor
        public Customer(String inCustomerID, String inCustomerFirstName, String inCustomerLastName,
                        String inGender, String inLandline, String inMobile, String inFax, 
                        String inEmailAddress, String inDateOfBirth)
        {
            customerID = inCustomerID;
            customerFirstName = inCustomerFirstName;
            customerLastName = inCustomerLastName;
            gender = inGender;
            landline = inLandline;
            mobile = inMobile;
            fax = inFax;
            emailAddress = inEmailAddress;
            dateOfBirth = inDateOfBirth;

            addressArray = new Address[2];
            invoiceArray = new Invoice[50];
        }

        //Alternate Constructor used for search
        public Customer(String inCustomerID, String inCustomerFirstName, String inMobile, String inEmailAddress)
        {
            setCustomerID(inCustomerID);
            setCustomerFirstName(inCustomerFirstName);
            setMobile(inMobile);
            setEmailAddress(inEmailAddress);

            customerLastName = "";
            gender = "";
            landline = "";
            fax = "";
            dateOfBirth = "";
        }

        //Alternate Constructor used to link with Invoice
        public Customer(String inCustomerID, String inCustomerFirstName)
        {
            setCustomerID(inCustomerID);
            setCustomerFirstName(inCustomerFirstName);

            customerLastName = "";
            gender = "";
            landline = "";
            mobile = "";
            fax = "";
            emailAddress = "";
            dateOfBirth = "";
        }


        //Copy Constructor
        public Customer(Customer customer)
        {
            customerID = customer.getCustomerID();
            customerFirstName = customer.getCustomerFirstName();
            customerLastName = customer.getCustomerLastName();
            gender = customer.getGender();
            landline = customer.getLandline();
            mobile = customer.getMobile();
            fax = customer.getFax();
            emailAddress = customer.getEmailAddress();
            addressArray = customer.getAddressArray();
            dateOfBirth = customer.getDateOfBirth();

            addressArray = customer.getAddressArray();
            invoiceArray = customer.getInvoiceArray();
        }
        #endregion


        #region /*All Mutators*/
        //Mutator of Object
        public void setCustomer(String inCustomerID, String inCustomerFirstName, String inCustomerLastName,
                                String inGender, String inLandline, String inMobile, String inFax,
                                String inEmailAddress, String inDateOfBirth)
        {
            setCustomerID(inCustomerID);
            setCustomerFirstName(inCustomerFirstName);
            setCustomerLastName(inCustomerLastName);
            setGender(inGender);
            setLandline(inLandline);
            setMobile(inMobile);
            setFax(inFax);
            setEmailAddress(inEmailAddress);
            setDateOfBirth(inDateOfBirth);
        }

        public void setCustomerID(String inCustomerID)
        {
            customerID = inCustomerID;
        }

        public void setCustomerFirstName(String inCustomerName)
        {
            customerFirstName = inCustomerName;
        }

        public void setCustomerLastName(String inCustomerLastName)
        {
            customerLastName = inCustomerLastName;
        }

        public void setGender(String inGender)
        {
            gender = inGender;
        }


        public void setLandline(String inLandline)
        {
            landline = inLandline;
        }

        public void setMobile(String inMobile)
        {
            mobile = inMobile;
        }

        public void setFax(String inFax)
        {
            fax = inFax;
        }

        public void setEmailAddress(String inEmailAddress)
        {
            emailAddress = inEmailAddress;
        }
  

        public void setDateOfBirth(String inDateOfBirth)
        {
            dateOfBirth = inDateOfBirth;
        }

        public void setBillingAddress(Address inAddress)
        {
            addressArray[0] = inAddress;
        }

        public void setShippingAddress(Address inAddress)
        {
            addressArray[1] = inAddress;
        }
        #endregion


        #region /*Interaction with Arrays*/
        //Adding billing address into the address array with index 0
        public void addBillingAddress(Address inAddress)
        {
            addressArray[0] = inAddress;
        }

        //Adding shippinf address into the address array with index 1
        public void addShippingAddress(Address inAddress)
        {
            addressArray[1] = inAddress;
        }

        //Adding invoice into invoice array
        public void addInvoice(Invoice inInvoice)
        {
            invoiceArray[indexInvoice] = inInvoice;
            indexInvoice++;
        }
        #endregion


        #region /*All Accessors*/
        public String getCustomerFirstName()
        {
            return customerFirstName;
        }

        public String getCustomerLastName()
        {
            return customerLastName;
        }

        public String getCustomerID()
        {
            return customerID;
        }

        public String getGender()
        {
            return gender;
        }

        public String getLandline()
        {
            return landline;
        }

        public String getMobile()
        {
            return mobile;
        }

        public String getFax()
        {
            return fax;
        }

        public String getEmailAddress()
        {
            return emailAddress;
        }   

        public String getDateOfBirth()
        {
            return dateOfBirth;
        }

        public Address[] getAddressArray()
        {
            return addressArray;
        }

        public Invoice[] getInvoiceArray()
        {
            return invoiceArray;
        }
        #endregion

    }//END class
}//END