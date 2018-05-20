using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
==================================
This is Invoice class.
==================================
*/

namespace ComHRInvoiceSystem
{
    class Invoice
    {
        private String invoiceNumber;
        private String invoiceDate;
        private String invoiceStatus;
        private String paymentMethod;
        private String shippingMethod;
        private String store;
        private double shippingIncludeTax;
        private double totalIncludeTax;
        private double totalTaxAmount;
        private double payment;
        private double balanceOwing;

        //Array used to store items bought by customer stated in the invoice.
        private Item[] itemArray;
        private int indexItemArray = 0;

        #region /*All Constructors*/
        //Default Constructor
        public Invoice()
        {
            invoiceNumber = "";
            invoiceDate = "";
            invoiceStatus = "";
            paymentMethod = "";
            shippingMethod = "";
            store = "";
            shippingIncludeTax = 0.0;
            totalIncludeTax = 0.0;
            totalTaxAmount = 0.0;
            payment = 0.0;
            balanceOwing = 0.0;

            //Initialise array size
            itemArray = new Item[50];
        }

        //Alternative Constructor only without Item[] as input parameter (add item to list manually)
        public Invoice(String inInvoiceNumber, String inInvoiceDate, String inInvoiceStatus, String inPaymentMethod,
                       String inShippingMethod, String inStore, double inShippingIncludeTax,
                       double inTotalIncludeTax, double inTotalTaxAmount,
                       double inPayment, double inBalanceOwing)
        {
            itemArray = new Item[50];
            invoiceNumber = inInvoiceNumber;
            invoiceDate = inInvoiceDate;
            invoiceStatus = inInvoiceStatus;
            paymentMethod = inPaymentMethod;
            shippingMethod = inShippingMethod;
            store = inStore;
            shippingIncludeTax = inShippingIncludeTax;
            totalIncludeTax = inTotalIncludeTax;
            totalTaxAmount = inTotalTaxAmount;
            payment = inPayment;
            balanceOwing = inBalanceOwing;
        }


        //Alternative Constructor without Item[] as input parameter (add item to list manually)
        public Invoice(String inInvoiceNumber, String inInvoiceDate, String inInvoiceStatus, String inPaymentMethod,
                       String inShippingMethod, String inStore, double inPayment)
        {
            itemArray = new Item[50];
            invoiceNumber = inInvoiceNumber;
            invoiceDate = inInvoiceDate;
            invoiceStatus = inInvoiceStatus;
            paymentMethod = inPaymentMethod;
            shippingMethod = inShippingMethod;
            store = inStore;
            setShippingIncludeTax();
            setTotalIncludeTax();
            setTotalTaxAmount();
            payment = inPayment;
            setBalanceOwing();           
        }

        //Alternative Constructor with Item[] as input parameter
        public Invoice(String inInvoiceNumber, String inInvoiceDate, String inInvoiceStatus, String inPaymentMethod,
                       String inShippingMethod, String inStore, double inPayment, Item[] inItemArray)
        {
            itemArray = new Item[50];
            itemArray = inItemArray;

            invoiceNumber = inInvoiceNumber;
            invoiceDate = inInvoiceDate;
            invoiceStatus = inInvoiceStatus;
            paymentMethod = inPaymentMethod;
            shippingMethod = inShippingMethod;
            store = inStore;
            setShippingIncludeTax();
            setTotalIncludeTax();
            setTotalTaxAmount();
            payment = inPayment;
            setBalanceOwing();           
        }

        //Copy Constructor
        public Invoice(Invoice invoice)
        {
            itemArray = invoice.getItemArray();
            invoiceNumber = invoice.getInvoiceNumber();
            invoiceDate = invoice.getInvoiceDate();
            invoiceStatus = invoice.getInvoiceStatus();
            paymentMethod = invoice.getPaymentMethod();
            shippingMethod = invoice.getShippingMethod();
            store = invoice.getStore();
            shippingIncludeTax = invoice.getShippingIncludeTax();
            totalIncludeTax = invoice.getTotalIncludeTax();
            totalTaxAmount = invoice.getTotalTaxAmount();
            payment = invoice.getPayment();
            balanceOwing = invoice.getBalanceOwing();        
        }
        #endregion


        #region /*All Mutators*/
        public void setInvoiceNumber(String inInvoiceNumber)
        {
            invoiceNumber = inInvoiceNumber;
        }

        public void setInvoiceDate(String inInvoiceDate)
        {
            invoiceDate = inInvoiceDate;
        }

        public void setInvoiceStatus(String inInvoiceStatus)
        {
            invoiceStatus = inInvoiceStatus;
        }

        public void setPaymentMethod(String inPaymentMethod)
        {
            paymentMethod = inPaymentMethod;
        }

        public void setShippingMethod(String inShippingMethod)
        {
            shippingMethod = inShippingMethod;
        }

        public void setStore(String inStore)
        {
            store = inStore;
        }

        //Mutator of shipping include tax has no input parameter
        public void setShippingIncludeTax()
        {
            //The value of shipping include tax will be determined by
            //user's selection. 
            if (getShippingMethod() == "Standard Shipping")
            {
                shippingIncludeTax = 30;
            }
            else if (getShippingMethod() == "Express Shipping")
            {
                shippingIncludeTax = 100;
            }
            else
            {
                shippingIncludeTax = 0;
            }
        }
        
        //Mutator total include tax will obtain data from Item class
        //in the itemArray, get each total item price from each item in the array.
        public void setTotalIncludeTax()
        {
            for (int arrayCounter = 0; itemArray[arrayCounter] != null; arrayCounter++)
            {
                totalIncludeTax += itemArray[arrayCounter].getTotalItemPrice();
            }
        }

        //Mutator total Tax Amount obtain data from the Item class
        //in the itemArray, get each total item tax from each item in the array.
        public void setTotalTaxAmount()
        {
            Item tempItem = new Item();

            for (int arrayCounter = 0; getItemArray()[arrayCounter] != null; arrayCounter++)
            {
                tempItem = getItemArray()[arrayCounter];
                totalTaxAmount += tempItem.getTotalItemTax();
            }
        }

        public void setPayment(double inPayment)
        {
            payment = inPayment;
        }

        //Mutator balance owing determine by the total price deduct by
        //the Customer's payment.
        public void setBalanceOwing()
        {
            balanceOwing =  getTotalIncludeTax() - getPayment();
        }

        //Mutator Item Array import the Item Array
        public void setItemArray(Item[] inItemArray)
        {
            itemArray = inItemArray;
        }

        //Adding Item into the array
        public void addItem(Item inItem)
        {
            itemArray[indexItemArray] = inItem;
            indexItemArray++;
        }
        #endregion


        #region /*All Accessors*/
        //Accessor
        public String getInvoiceNumber()
        {
            return invoiceNumber;
        }

        public String getInvoiceDate()
        {
            return invoiceDate;
        }

        public String getInvoiceStatus()
        {
            return invoiceStatus;
        }

        public String getPaymentMethod()
        {
            return paymentMethod;
        }

        public String getShippingMethod()
        {
            return shippingMethod;
        }

        public String getStore()
        {
            return store;
        }

        public double getShippingIncludeTax()
        {
            return Math.Round(shippingIncludeTax, 2);
        }

        public double getTotalIncludeTax()
        {
            return Math.Round(totalIncludeTax, 2);
        }

        public double getTotalTaxAmount()
        {
            return Math.Round(totalTaxAmount, 2);
        }

        public double getPayment()
        {
            return Math.Round(payment, 2);
        }

        public double getBalanceOwing()
        {
            return Math.Round(balanceOwing, 2);
        }

        public Item[] getItemArray()
        {
            return itemArray;
        }
        #endregion

    }//END class
}//END 
