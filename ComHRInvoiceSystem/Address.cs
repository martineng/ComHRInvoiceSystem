using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHRInvoiceSystem
{
    class Address
    {
        private String addressLine1;
        private String addressLine2;
        private String suburb;
        private String state;
        private int postcode;
        private int addressId;

        #region /*All Constructors*/
        //Default Constructor 
        public Address()
        {
            addressLine1 = "";
            addressLine2 = "";
            suburb = "";
            state = "";
            postcode = 0;
            addressId = 0;
        }

        //Alternate Constructor
        public Address(String inAddressLine1, String inAddressLine2, String inSuburb,
                       String inState, int inPostcode)
        {
            addressLine1 = inAddressLine1;
            addressLine2 = inAddressLine2;
            suburb = inSuburb;
            state = inState;
            postcode = inPostcode;
            addressId = 0;
        }

        //Alternate Constructor
        public Address(String inAddressLine1, String inAddressLine2, String inSuburb,
                       String inState, int inPostcode, int inAddressId)
        {
            addressLine1 = inAddressLine1;
            addressLine2 = inAddressLine2;
            suburb = inSuburb;
            state = inState;
            postcode = inPostcode;
            addressId = inAddressId;
        }

        //Copy Constructor
        public Address(Address address)
        {
            addressLine1 = address.getAddressLine1();
            addressLine2 = address.getAddressLine2();
            suburb = address.getSuburb();
            state = address.getState();
            postcode = address.getPostcode();
            addressId = address.getAddressId();
        }
        #endregion


        #region /*All Mutators*/
        //Mutator of Object
        public void setAddress(String inAddressLine1, String inAddressLine2, String inSuburb,
                               String inState, int inPostcode, int inAddressId)
        {
            setAddressLine1(inAddressLine1);
            setAddressLine2(inAddressLine2);
            setSuburb(inSuburb);
            setState(inState);
            setPostcode(inPostcode);
            setAddressId(inAddressId);
        }

        public void setAddressLine1(String inAddressLine1)
        {
            addressLine1 = inAddressLine1;
        }

        public void setAddressLine2(String inAddressLine2)
        {
            addressLine2 = inAddressLine2;
        }

        public void setSuburb(String inSuburb)
        {
            suburb = inSuburb;
        }

        public void setState(String inState)
        {
            state = inState;
        }

        public void setPostcode(int inPostcode)
        {
            postcode = inPostcode;
        }

        public void setAddressId(int inAddressId)
        {
            addressId = inAddressId;
        }
        #endregion


        #region /*All Accessor*/
        //Accessor
        public String getAddressLine1()
        {
            return addressLine1;
        }

        public String getAddressLine2()
        {
            return addressLine2;
        }

        public String getSuburb()
        {
            return suburb;
        }

        public String getState()
        {
            return state;
        }

        public int getPostcode()
        {
            return postcode;
        }

        public int getAddressId()
        {
            return addressId;
        }
        #endregion

    }//END class
}//END
