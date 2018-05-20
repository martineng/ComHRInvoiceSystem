using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//MySql reference
using MySql.Data.MySqlClient;
using databaseDLL;

namespace ComHRInvoiceSystem
{
    public partial class FormHomePage : Form
    {
        /* Creating Object used across different panel */
        User theUser = new User();
        Invoice[] invoiceArray = new Invoice[50];
        Customer[] customerArray = new Customer[50];
        Item[] itemArray = new Item[50];
        int counterItemArray = 0;
        Invoice tempInvoiceForAddInvoice = new Invoice();

        Item item = new Item();
        Invoice invoice = new Invoice();
        Customer customer = new Customer();
        Address billingAddress = new Address();
        Address shippingAddress = new Address();
        string CID;


        //CHECKED
        public FormHomePage()
        {
            InitializeComponent();
            customizeDateTimeFormat();

            //Hide character entered in password section
            txtBoxCurrentPassword.PasswordChar = '*';
            txtBoxNewPassword.PasswordChar = '*';
            txtBoxConfirmNewPassword.PasswordChar = '*';
            
            //customizeListViewSize();
        }


#region  /*Panel Setup*/
        /* This function set the size of the List View*/
        private void customizeListViewSize()
        {
            listViewItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        /* Function to that setup UI for Add Invoice */
        private void setupAddInvoicePage()
        {
            clearPanelAddInvoice();

            panelChangePassword.Visible = false;
            panelAddCustomer.Visible = false;
            panelAddInvoice.Visible = true;
            panelAddInvoice.Dock = DockStyle.Fill;

            allowEditAddInvoicePage();
        }

        /* Function to setup UI for Add Customer */
        private void setupAddCustomerPage()
        {
            clearPanelAddCustomer();

            panelChangePassword.Visible = false;
            panelAddCustomer.Visible = true;
            panelAddInvoice.Visible = true;
            panelAddInvoice.Dock = DockStyle.Fill;
            panelAddCustomer.Dock = DockStyle.Fill;
        }

        /* Function to setup UI for Change Password */
        private void setupChangePasswordPage()
        {
            clearPanelChangePassword();

            panelAddInvoice.Visible = false;
            panelChangePassword.Dock = DockStyle.Fill;
        }

        /* This function set the format of the Date Time Picker to dd/MM/yyyy */
        private void customizeDateTimeFormat()
        {
            dateTimePickerCustomerDOBToAdd.Format = DateTimePickerFormat.Custom;
            dateTimePickerCustomerDOBToAdd.CustomFormat = "dd/MM/yyyy";

            dateTimePickerInvoiceToAdd.Format = DateTimePickerFormat.Custom;
            dateTimePickerInvoiceToAdd.CustomFormat = "dd/MM/yyyy";
        }

        /* This function change Customer UI in Search */
        private void radioBtnCustomer_CheckedChanged(object sender, EventArgs e)
        {
            clearPanelSearch();
            clearPanelAddCustomer();

            //groupBoxSearch.Visible = true;

            //Show customerToSearch Panel upon radioBtn Activated
            panelCustomerToSearch.Visible = true;
            panelAddCustomer.Visible = true;
            panelAddCustomer.Dock = DockStyle.Fill;
        }

        /* This function change Invoice UI in Search  */
        private void radioBtnInvoice_CheckedChanged(object sender, EventArgs e)
        {
            clearPanelSearch();
            clearPanelAddInvoice();

            //Hide customerToSearch Panel upon radioBtn Activated
            panelCustomerToSearch.Visible = false;
            panelAddCustomer.Visible = false;
        }

        //CHECKED
        //Set shipping address is the same as billing address
        private void checkBoxSameAddress_CheckedChanged(object sender, EventArgs e)
        {
            txtBoxShippingAddress1ToAdd.Text = txtBoxBillingAddress1ToAdd.Text;
            txtBoxShippingAddress2ToAdd.Text = txtBoxBillingAddress2ToAdd.Text;
            txtBoxShippingSurburbToAdd.Text = txtBoxBillingSurburbToAdd.Text;
            txtBoxShippingPostcodeToAdd.Text = txtBoxBillingPostcodeToAdd.Text;
            comboBoxShippingStateToAdd.Text = comboBoxBillingStateToAdd.Text;
        }

        #endregion


#region /*All Clear Txtbox*/
        private void clearPanelSearch()
        {
            //panel customer search
            txtBoxCustomerIDToSearch.Clear();
            txtBoxCustomerNameToSearch.Clear();
            txtBoxMobileNumToSearch.Clear();
            txtBoxEmailToSearch.Clear();
            txtBoxInvoiceNumToSearch.Clear();
        }

        private void clearPanelChangePassword()
        {
            //panel change password
            txtBoxCurrentPassword.Clear();
            txtBoxNewPassword.Clear();
            txtBoxConfirmNewPassword.Clear();
        }

        private void clearPanelAddInvoice()
        {
            //panel add invoice
            txtBoxInvoiceNumToAdd.Clear();
            txtBoxCustomerIDtoLink.Clear();
            txtBoxCustomerNameToLink.Clear();

            //groupbox item details
            txtBoxItemCodeToAdd.Clear();
            txtBoxDescriptionToAdd.Clear();
            txtBoxUnitPriceIncTaxToAdd.Clear();
            txtBoxPaymentToAdd.Clear();

            //group box reciept
            txtBoxShippingIncTax.Clear();
            txtBoxTotalIncTax.Clear();
            txtBoxTotalTax.Clear();
            txtBoxPayment.Clear();
            txtBoxBalanceOwning.Clear();

            listViewItems.Items.Clear();
        }

        private void clearPanelAddCustomer()
        {
            //panel add customer
            txtBoxCustomerIDToAdd.Clear();
            txtBoxCustomerFirstNameToAdd.Clear();
            txtBoxCustomerLastNameToAdd.Clear();
            txtBoxCustomerLandlineToAdd.Clear();
            txtBoxCustomerMobileToAdd.Clear();
            txtBoxCustomerFaxToAdd.Clear();
            txtBoxCustomerEmailToAdd.Clear();

            //group box billing address
            txtBoxBillingAddress1ToAdd.Clear();
            txtBoxBillingAddress2ToAdd.Clear();
            txtBoxBillingSurburbToAdd.Clear();
            txtBoxBillingPostcodeToAdd.Clear();

            //group box shipping address
            txtBoxShippingAddress1ToAdd.Clear();
            txtBoxShippingAddress2ToAdd.Clear();
            txtBoxShippingSurburbToAdd.Clear();
            txtBoxShippingPostcodeToAdd.Clear();
        }

        private void clearItemDetail()
        {
            txtBoxItemCodeToAdd.Text = "";
            txtBoxDescriptionToAdd.Text = "";
            numericUpDownQuantity.Value = 0;
            txtBoxUnitPriceIncTaxToAdd.Text = "";
        }

        /* When called, reset all the control */
        private void resetControl(Control control)
        {
            if (control.HasChildren)
            {
                foreach (var ctl in control.Controls)
                {
                    resetControl((Control)ctl);
                }
            }

            if (control is TextBox)
            {
                control.ResetText();
            }
        }


        #endregion


#region /*All ToolStripMenuItem and Shortcut Action*/

        /* Open Panel Change Password from the menu */
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearPanelChangePassword();

            panelAddInvoice.Visible = false;
            panelChangePassword.Visible = true;
            panelChangePassword.Dock = DockStyle.Fill;
        }

        private void btnToAddCustomer_Click(object sender, EventArgs e)
        {
            clearPanelAddCustomer();

            resetControl(this);
            setupAddCustomerPage();
            allowEditAddCustomerPage();
        }

        private void btnToAddInvoice_Click(object sender, EventArgs e)
        {
            clearPanelAddInvoice();

            resetControl(this);
            setupAddInvoicePage();
            allowEditAddInvoicePage();
        }

        private void btnToSearchCustomer_Click(object sender, EventArgs e)
        {
            clearPanelSearch();
            clearPanelAddCustomer();
            clearPanelAddInvoice();

            resetControl(this);
            setupAddInvoicePage();
            restrictEditAddInvoicePage();
            restrictEditAddCustomer();
            panelAddInvoice.Dock = DockStyle.Bottom;
        }

        private void btnToSearchInvoice_Click(object sender, EventArgs e)
        {
            clearPanelSearch();
            clearPanelAddCustomer();
            clearPanelAddInvoice();

            resetControl(this);
            setupAddInvoicePage();
            restrictEditAddInvoicePage();
            restrictEditAddCustomer();
            panelAddInvoice.Dock = DockStyle.Bottom;
        }

        private void btnToEditCustomer_Click(object sender, EventArgs e)
        {
            clearPanelSearch();
            clearPanelAddCustomer();
            clearPanelAddInvoice();

            resetControl(this);
            setupAddInvoicePage();
            allowEditAddInvoicePage();
            allowEditAddCustomerPage();
            panelAddInvoice.Dock = DockStyle.Bottom;
            btnEditCustomer.Visible = true;
            btnEditInvoice.Visible = true;
            btnSaveCustomer.Visible = false;
            btnSaveInvoice.Visible = false;
        }

        private void btnToEditInvoice_Click(object sender, EventArgs e)
        {
            clearPanelSearch();
            clearPanelAddCustomer();
            clearPanelAddInvoice();

            resetControl(this);
            setupAddInvoicePage();
            allowEditAddInvoicePage();
            allowEditAddCustomerPage();
            panelAddInvoice.Dock = DockStyle.Bottom;
            btnEditCustomer.Visible = true;
            btnEditInvoice.Visible = true;
            btnSaveCustomer.Visible = false;
            btnSaveInvoice.Visible = false;
        }


        #endregion


#region /*All Restriction Setup*/
        /* Function to allow Control in Add Invoice Panel */
        private void allowEditAddInvoicePage()
        {
            //Textbox 
            txtBoxInvoiceNumToAdd.ReadOnly = false;
            txtBoxCustomerIDtoLink.ReadOnly = false;
            txtBoxCustomerNameToLink.ReadOnly = false;
            txtBoxItemCodeToAdd.ReadOnly = false;
            txtBoxDescriptionToAdd.ReadOnly = false;
            txtBoxUnitPriceIncTaxToAdd.ReadOnly = false;
            txtBoxPaymentToAdd.ReadOnly = false;

            //Enabled
            dateTimePickerInvoiceToAdd.Enabled = true;
            comboBoxInvoiceStatus.Enabled = true;
            comboBoxPaymentMethod.Enabled = true;
            comboBoxShippingMethod.Enabled = true;
            comboBoxStore.Enabled = true;
            numericUpDownQuantity.Enabled = true;
            listViewItems.Enabled = true;

            //Button visible
            btnNextItemToAdd.Visible = true;
            btnDeleteItemSelect.Visible = true;
            btnConfirmPayment.Visible = true;
            btnSaveInvoice.Visible = true;
            btnEditCustomer.Visible = false;
            btnEditInvoice.Visible = false;

            //Item field hide
            label15.Visible = true;
            label16.Visible = true;
            label17.Visible = true;
            label18.Visible = true;
            label19.Visible = true;
            txtBoxItemCodeToAdd.Visible = true;
            txtBoxDescriptionToAdd.Visible = true;
            numericUpDownQuantity.Visible = true;
            txtBoxUnitPriceIncTaxToAdd.Visible = true;
            txtBoxPaymentToAdd.Visible = true;
        }

        /* Function to disable Control in Add Invoice Panel */
        private void restrictEditAddInvoicePage()
        {
            //Textbox
            txtBoxInvoiceNumToAdd.ReadOnly = true;
            txtBoxCustomerIDtoLink.ReadOnly = true;
            txtBoxCustomerNameToLink.ReadOnly = true;
            txtBoxItemCodeToAdd.ReadOnly = true;
            txtBoxDescriptionToAdd.ReadOnly = true;
            txtBoxUnitPriceIncTaxToAdd.ReadOnly = true;
            txtBoxPaymentToAdd.ReadOnly = true;
            txtBoxShippingIncTax.ReadOnly = true;
            txtBoxTotalIncTax.ReadOnly = true;
            txtBoxTotalTax.ReadOnly = true;
            txtBoxPayment.ReadOnly = true;
            txtBoxBalanceOwning.ReadOnly = true;

            //Enabled
            dateTimePickerInvoiceToAdd.Enabled = false;
            comboBoxInvoiceStatus.Enabled = false;
            comboBoxPaymentMethod.Enabled = false;
            comboBoxShippingMethod.Enabled = false;
            comboBoxStore.Enabled = false;
            numericUpDownQuantity.Enabled = false;

            //Button Visible
            btnNextItemToAdd.Visible = false;
            btnDeleteItemSelect.Visible = false;
            btnConfirmPayment.Visible = false;
            btnSaveInvoice.Visible = false;
            btnEditCustomer.Visible = false;
            btnEditInvoice.Visible = false;

            //Item field hide
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
            txtBoxItemCodeToAdd.Visible = false;
            txtBoxDescriptionToAdd.Visible = false;
            numericUpDownQuantity.Visible = false;
            txtBoxUnitPriceIncTaxToAdd.Visible = false;
            txtBoxPaymentToAdd.Visible = false;
        }

        /* Function to allow Control in Add Customer Panel */
        private void allowEditAddCustomerPage()
        {
            //Textbox
            txtBoxCustomerIDToAdd.ReadOnly = false;
            txtBoxCustomerFirstNameToAdd.ReadOnly = false;
            txtBoxCustomerLastNameToAdd.ReadOnly = false;
            txtBoxCustomerLandlineToAdd.ReadOnly = false;
            txtBoxCustomerMobileToAdd.ReadOnly = false;
            txtBoxCustomerFaxToAdd.ReadOnly = false;
            txtBoxCustomerEmailToAdd.ReadOnly = false;
            txtBoxBillingAddress1ToAdd.ReadOnly = false;
            txtBoxBillingAddress2ToAdd.ReadOnly = false;
            txtBoxBillingSurburbToAdd.ReadOnly = false;
            txtBoxBillingPostcodeToAdd.ReadOnly = false;
            txtBoxShippingAddress1ToAdd.ReadOnly = false;
            txtBoxShippingAddress2ToAdd.ReadOnly = false;
            txtBoxShippingSurburbToAdd.ReadOnly = false;
            txtBoxShippingPostcodeToAdd.ReadOnly = false;

            //Enabled
            comboBoxGenderToAdd.Enabled = true;
            dateTimePickerCustomerDOBToAdd.Enabled = true;
            comboBoxBillingStateToAdd.Enabled = true;
            comboBoxShippingStateToAdd.Enabled = true;

            //Button Visible
            btnSaveCustomer.Visible = true;
            btnEditCustomer.Visible = false;
            btnEditInvoice.Visible = false;
        }

        /* Function to diable Control in Add Customer Panel */
        private void restrictEditAddCustomer()
        {
            //Textbox
            txtBoxCustomerIDToAdd.ReadOnly = true;
            txtBoxCustomerFirstNameToAdd.ReadOnly = true;
            txtBoxCustomerLastNameToAdd.ReadOnly = true;
            txtBoxCustomerLandlineToAdd.ReadOnly = true;
            txtBoxCustomerMobileToAdd.ReadOnly = true;
            txtBoxCustomerFaxToAdd.ReadOnly = true;
            txtBoxCustomerEmailToAdd.ReadOnly = true;
            txtBoxBillingAddress1ToAdd.ReadOnly = true;
            txtBoxBillingAddress2ToAdd.ReadOnly = true;
            txtBoxBillingSurburbToAdd.ReadOnly = true;
            txtBoxBillingPostcodeToAdd.ReadOnly = true;
            txtBoxShippingAddress1ToAdd.ReadOnly = true;
            txtBoxShippingAddress2ToAdd.ReadOnly = true;
            txtBoxShippingSurburbToAdd.ReadOnly = true;
            txtBoxShippingPostcodeToAdd.ReadOnly = true;

            //Enabled
            comboBoxGenderToAdd.Enabled = false;
            dateTimePickerCustomerDOBToAdd.Enabled = false;
            comboBoxBillingStateToAdd.Enabled = false;
            comboBoxShippingStateToAdd.Enabled = false;

            //Button Visible
            btnSaveCustomer.Visible = false;
            btnEditCustomer.Visible = false;
            btnEditInvoice.Visible = false;
        }

        #endregion
 

#region /*All Buttons Actions and Event*/
        //CHECKED
        private void btnConfirmNewPassword_Click(object sender, EventArgs e)
        {
            String currentPassword;
            String newPassword;
            String newPassword2;

            currentPassword = txtBoxCurrentPassword.Text;
            newPassword = txtBoxNewPassword.Text;
            newPassword2 = txtBoxConfirmNewPassword.Text;

            if (newPassword2 == newPassword)
            {
                if (currentPassword == theUser.getPassword())
                {
                    theUser.setPassword(newPassword);
                    MessageBox.Show("Password changed sucessful!");
                    clearPanelChangePassword();
                }//END IF
                else
                {
                    MessageBox.Show("Wrong password.");
                }
            }//END IF
            else
            {
                MessageBox.Show("New password do not match");
            }
        }//END btnConfirmNewPassword

        //CHECKED
        private void btnSaveCustomer_Click(object sender, EventArgs e)
        {
            billingAddress = new Address(newBillingAddress());
            shippingAddress = new Address(newShippingAddress());

            customer =  new Customer(newCustomer());
            addNewCustomerBillingAddress();
            addNewCustomerShippingAddress();
            addNewCustomer();
        }//END btnSaveCustomer_Click()

        //Search for Customer/Invoices
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Clean up
            clearPanelAddInvoice();
            clearPanelAddCustomer();

            Boolean isFound = false;

            //Search for Customer
            if (radioBtnCustomer.Checked == true)
            {
                customerArray = new Customer[50];
                Customer theCustomer = new Customer();

                //Run query to get all Customers
                customerArray = selectAllCustomer();

                //Store the information for customer to be searched
                Customer customerToSearch = new Customer(txtBoxCustomerIDToSearch.Text, txtBoxCustomerNameToSearch.Text,
                                                         txtBoxMobileNumToSearch.Text, txtBoxEmailToSearch.Text);

                //Compare and find the target
                for (int counter = 0; customerArray[counter]!=null; counter++)
                {
                    if ((customerToSearch.getCustomerID() == customerArray[counter].getCustomerID()) ||
                        (customerToSearch.getCustomerFirstName() == customerArray[counter].getCustomerFirstName()) ||
                        (customerToSearch.getMobile() == customerArray[counter].getMobile()) ||
                        (customerToSearch.getEmailAddress() == customerArray[counter].getEmailAddress())
                       )
                    {
                        theCustomer = new Customer(customerArray[counter]);
                        isFound = true;
                    }//END if 
                }//END for
                 
                //Print result
                if (isFound == true)
                {
                    printCustomerDetail(theCustomer);
                }
                else
                {
                    MessageBox.Show("Customer not found");
                } //END if
            }//END if radioBtnCustomer == checked
            //====================================Below is search invoice=============================================================
            else if (radioBtnInvoice.Checked == true)
            {
                Invoice theInvoice = new Invoice();
                Item[] theItemArray = new Item[50];

                Invoice invoiceToSearch = searchInvoice(txtBoxInvoiceNumToSearch.Text);
                theItemArray = searchAllItem(searchInvoiceItem(invoiceToSearch.getInvoiceNumber()));

                for (int counter = 0; theItemArray[counter]!= null; counter++)
                {
                    printItemToList(theItemArray[counter]);
                }//END for

                theInvoice = searchInvoice(invoiceToSearch.getInvoiceNumber());
                printInvoiceDetail(theInvoice);

                Customer customerFromInvoice = searchCustomerById(CID);
                printCustomerForInvoice(customerFromInvoice);
            }//END else if
        }

        //CHECKED
        private void btnNextItemToAdd_Click(object sender, EventArgs e)
        {
            item = new Item(newItem());
            addNewItem();
            printItemToList(item);
            itemArray[counterItemArray] = item;
            counterItemArray++;

            clearItemDetail();     
        }

        //Delete the record of selected Item. (upon search)
        private void btnDeleteItemSelect_Click(object sender, EventArgs e)
        {
            item = new Item(searchItem(listViewItems.FocusedItem.Text));
            deleteItem();
        }

        //Confirm amount of payment and do a little bit of calculation
        private void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            invoice = new Invoice(newInvoice());
            printInvoicePaymenDetail(invoice);
        }

        private void btnSaveInvoice_Click(object sender, EventArgs e)
        {
            invoice = new Invoice(newInvoice());
            customer = new Customer(newCustomerToLink());

            addNewInvoice(customer.getCustomerID());
            addInvoiceItem(invoice.getInvoiceNumber(), itemArray);
         }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            int BAID = billingAddress.getAddressId();
            int SAID = shippingAddress.getAddressId();

            billingAddress = new Address(newBillingAddress());
            billingAddress.setAddressId(BAID);

            shippingAddress = new Address(newShippingAddress());
            shippingAddress.setAddressId(SAID);

            customer = new Customer(newCustomer());

            updateBillingAddress();
            updateShippingAddress();
            updateCustomer();

            MessageBox.Show(customer.getCustomerFirstName() + " Updated!");
        }

        private void btnEditInvoice_Click(object sender, EventArgs e)
        {
            Invoice editInvoice = new Invoice();
            int indexLocation = 0;

            for (int index = 0; invoiceArray[index] != null; index++)
            {
                if (invoiceArray[index].getInvoiceNumber() == txtBoxInvoiceNumToAdd.Text)
                {
                    editInvoice = new Invoice(invoiceArray[index]);
                    indexLocation = index;
                }
            }

            editInvoice.setInvoiceNumber(txtBoxInvoiceNumToAdd.Text);
            editInvoice.setInvoiceDate(dateTimePickerInvoiceToAdd.Text);
            editInvoice.setInvoiceStatus(comboBoxInvoiceStatus.Text);
            editInvoice.setPaymentMethod(comboBoxPaymentMethod.Text);
            editInvoice.setShippingMethod(comboBoxShippingMethod.Text);
            editInvoice.setStore(comboBoxStore.Text);
            editInvoice.setItemArray(itemArray);


            editInvoice.setShippingIncludeTax();
            editInvoice.setTotalIncludeTax();
            editInvoice.setPayment(double.Parse(txtBoxPayment.Text));
            editInvoice.setBalanceOwing();

            //Add invoice into invoice array
            invoiceArray[indexLocation] = editInvoice;

            //find customer and add it into customer's item array
            //assignInvoice();

            MessageBox.Show("Invoice: " + editInvoice.getInvoiceNumber() + " Edited!");
        }

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            item = newItem();

            if (txtBoxInvoiceNumToAdd.Text == null)
            {
                MessageBox.Show("Please enter Invoice Number");
            }//END if
            else
            {
                updateItem();
            }//END else
        }


        //When the listView is clicked
        private void listViewItems_ItemActivate(object sender, EventArgs e)
        {
            //MessageBox.Show("Reading Item detail...");
            //Get clicked item index
            item = new Item(searchItem(listViewItems.FocusedItem.Text));

            //retrieve information on screen
            printItemDetail(item);
        }

        private void txtBoxPaymentToAdd_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnToAbout_Click(object sender, EventArgs e)
        {
            DialogResult about = MessageBox.Show("Thank you!\nIf you like this program, please leave a comment on my Linkedin.\n\n" +
                                                "By clicking Yes, it will bring you to my profile!", "THANK YOU", MessageBoxButtons.YesNo);

            if (about == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://linkedin.com/in/martin-eca");

            }//END if
            else if (about == DialogResult.No)
            {
            } //END if
        }

        #endregion




#region /*Store txtBox input into Objects Functions*/

        //Store user input with Object, Billing Address
        private Address newBillingAddress()
        {
            Address theBillingAddress = new Address(txtBoxBillingAddress1ToAdd.Text, txtBoxBillingAddress2ToAdd.Text, txtBoxBillingSurburbToAdd.Text,
                                                 comboBoxBillingStateToAdd.Text, Convert.ToInt32(txtBoxBillingPostcodeToAdd.Text));
            return theBillingAddress;
        }//END newBillingAddress()

        //Store user input with Object, Shipping Address
        private Address newShippingAddress()
        {
            Address theShippingAddress = new Address(txtBoxShippingAddress1ToAdd.Text, txtBoxShippingAddress2ToAdd.Text, txtBoxShippingSurburbToAdd.Text,
                                                 comboBoxShippingStateToAdd.Text, Convert.ToInt32(txtBoxShippingPostcodeToAdd.Text));

            return theShippingAddress;
        }//END newShippingAddress()

        //Store user input with Object, Customer
        private Customer newCustomer()
        {
            Customer theCustomer = new Customer(txtBoxCustomerIDToAdd.Text, txtBoxCustomerFirstNameToAdd.Text, txtBoxCustomerLastNameToAdd.Text,
                                             comboBoxGenderToAdd.Text, txtBoxCustomerLandlineToAdd.Text, txtBoxCustomerMobileToAdd.Text,
                                             txtBoxCustomerFaxToAdd.Text, txtBoxCustomerEmailToAdd.Text, dateTimePickerCustomerDOBToAdd.Text);

            return theCustomer;
        }//END newCustomer()

        //Store user input with Object, Item
        private Item newItem()
        {
            Item theItem = new Item(txtBoxItemCodeToAdd.Text, 
                                    txtBoxDescriptionToAdd.Text,
                                    Convert.ToInt32(numericUpDownQuantity.Value.ToString()),
                                    Convert.ToDouble(txtBoxUnitPriceIncTaxToAdd.Text));
            

            return theItem;
        }//END newItem()

        //Store user input with Object, Invoice
        private Invoice newInvoice()
        {
            Invoice theInvoice = new Invoice(txtBoxInvoiceNumToAdd.Text,
                                             dateTimePickerInvoiceToAdd.Text,
                                             comboBoxInvoiceStatus.Text,
                                             comboBoxPaymentMethod.Text,
                                             comboBoxShippingMethod.Text,
                                             comboBoxStore.Text,                                           
                                             double.Parse(txtBoxPaymentToAdd.Text),
                                             itemArray);

            return theInvoice;
        }//END newInvoice()

        //Store user input with Obeject, Customer for Invoice
        private Customer newCustomerToLink()
        {
            Customer theCustomer = new Customer(txtBoxCustomerIDtoLink.Text, 
                                                   txtBoxCustomerNameToLink.Text);

            return theCustomer;
        }//END customerToLink


#endregion


#region /*Print and output to fields function*/

        private void printCustomerDetail(Customer inCustomer)
        {
            Address theBillingAddress = inCustomer.getAddressArray()[0];
            Address theShippingAddress = inCustomer.getAddressArray()[1];

            //Print all information
            txtBoxCustomerIDToAdd.Text = inCustomer.getCustomerID().ToString();
            txtBoxCustomerFirstNameToAdd.Text = inCustomer.getCustomerFirstName();
            txtBoxCustomerLastNameToAdd.Text = inCustomer.getCustomerLastName();
            txtBoxCustomerLandlineToAdd.Text = inCustomer.getLandline();
            txtBoxCustomerMobileToAdd.Text = inCustomer.getMobile();
            txtBoxCustomerFaxToAdd.Text = inCustomer.getFax();
            txtBoxCustomerEmailToAdd.Text = inCustomer.getEmailAddress();
            txtBoxBillingAddress1ToAdd.Text = theBillingAddress.getAddressLine1();
            txtBoxBillingAddress2ToAdd.Text = theBillingAddress.getAddressLine2();
            txtBoxBillingSurburbToAdd.Text = theBillingAddress.getSuburb();
            txtBoxBillingPostcodeToAdd.Text = theBillingAddress.getPostcode().ToString();
            txtBoxShippingAddress1ToAdd.Text = theShippingAddress.getAddressLine1();
            txtBoxShippingAddress2ToAdd.Text = theShippingAddress.getAddressLine2();
            txtBoxShippingSurburbToAdd.Text = theShippingAddress.getSuburb();
            txtBoxShippingPostcodeToAdd.Text = theShippingAddress.getPostcode().ToString();

            comboBoxGenderToAdd.Text = inCustomer.getGender();
            dateTimePickerCustomerDOBToAdd.Text = inCustomer.getDateOfBirth();
            comboBoxBillingStateToAdd.Text = theBillingAddress.getState();
            comboBoxShippingStateToAdd.Text = theBillingAddress.getState();

        }//END printCustomerDetail()

        private void printItemDetail(Item inItem)
        {
            //Print all information
            txtBoxItemCodeToAdd.Text = inItem.getItemCode().ToString();
            txtBoxDescriptionToAdd.Text = inItem.getItemDescription().ToString();
            numericUpDownQuantity.Value = inItem.getQuantity();
            txtBoxUnitPriceIncTaxToAdd.Text = inItem.getUnitPriceWithTax().ToString();
        }//END printItemDetail()

        private void printItemToList(Item inItem)
        {
            //Pushing item into the list
            ListViewItem lvi = new ListViewItem(inItem.getItemCode());
            lvi.SubItems.Add(inItem.getItemDescription());
            lvi.SubItems.Add(inItem.getQuantity().ToString());
            lvi.SubItems.Add(inItem.getUnitPriceWithTax().ToString());
            lvi.SubItems.Add(inItem.getTotalItemTax().ToString());
            lvi.SubItems.Add(inItem.getTotalItemPrice().ToString());
            listViewItems.Items.Add(lvi);

        }//END printItemToList()

        private void printItemArrayDetail(Item[] inItemArray)
        {
            Item theItem = new Item();

            for (int counter = 0; inItemArray[counter] != null; counter++)
            {
                theItem = new Item(inItemArray[counter]);
                printItemToList(theItem);
            }//END for
        }//END printItemArrayDetail()

        private void printInvoiceDetail(Invoice inInvoice)
        {
            //Retirve data from the target Invoice
            txtBoxInvoiceNumToAdd.Text = inInvoice.getInvoiceNumber().ToString();
            dateTimePickerInvoiceToAdd.Text = inInvoice.getInvoiceDate();
            comboBoxInvoiceStatus.Text = inInvoice.getInvoiceStatus();
            comboBoxPaymentMethod.Text = inInvoice.getPaymentMethod();
            comboBoxShippingMethod.Text = inInvoice.getShippingMethod();
            comboBoxStore.Text = inInvoice.getStore();
            txtBoxShippingIncTax.Text = inInvoice.getShippingIncludeTax().ToString();
            txtBoxTotalIncTax.Text = inInvoice.getTotalIncludeTax().ToString();
            txtBoxTotalTax.Text = inInvoice.getTotalTaxAmount().ToString();
            txtBoxPayment.Text = inInvoice.getPayment().ToString();
            txtBoxBalanceOwning.Text = inInvoice.getBalanceOwing().ToString();
        }//END printInvoiceDetail()

        private void printCustomerForInvoice(Customer inCustomer)
        {
            txtBoxCustomerIDtoLink.Text = inCustomer.getCustomerID();
            txtBoxCustomerNameToLink.Text = inCustomer.getCustomerFirstName();
        }//END printCustomerForInvoice()

        private void printInvoicePaymenDetail(Invoice inInvoice)
        {
            txtBoxShippingIncTax.Text = inInvoice.getShippingIncludeTax().ToString();
            txtBoxTotalIncTax.Text = inInvoice.getTotalIncludeTax().ToString();
            txtBoxTotalTax.Text = inInvoice.getTotalTaxAmount().ToString();
            txtBoxPayment.Text = inInvoice.getPayment().ToString();
            txtBoxBalanceOwning.Text = inInvoice.getBalanceOwing().ToString();
        }//END printInvoicePaymentDetail()

#endregion


#region All function to do queries
/* 
* All function are arrangeded by its' specific function.
*/

#region /*Search Customer Function*/

        private Customer[] selectAllCustomer()
        {
            try
            {
                string querySelectCustomer = "SELECT * FROM customer";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdSelectCustomer = new MySqlCommand(querySelectCustomer, myConn);
                MySqlDataAdapter adapterSelectCustomer = new MySqlDataAdapter(cmdSelectCustomer);
                DataSet dsSelectCustomer = new DataSet();
                adapterSelectCustomer.Fill(dsSelectCustomer);

                for (int counter = 0; counter < dsSelectCustomer.Tables[0].Rows.Count; counter++)
                {
                    customer = new Customer(dsSelectCustomer.Tables[0].Rows[counter].ItemArray[0].ToString(),
                                            dsSelectCustomer.Tables[0].Rows[counter].ItemArray[3].ToString(),
                                            dsSelectCustomer.Tables[0].Rows[counter].ItemArray[4].ToString(),
                                            dsSelectCustomer.Tables[0].Rows[counter].ItemArray[5].ToString(),
                                            dsSelectCustomer.Tables[0].Rows[counter].ItemArray[6].ToString(),
                                            dsSelectCustomer.Tables[0].Rows[counter].ItemArray[7].ToString(),
                                            dsSelectCustomer.Tables[0].Rows[counter].ItemArray[8].ToString(),
                                            dsSelectCustomer.Tables[0].Rows[counter].ItemArray[9].ToString(),
                                            dsSelectCustomer.Tables[0].Rows[counter].ItemArray[10].ToString());

                    //Retrieve information of Addresses
                    billingAddress = selectBillingAddress(Convert.ToInt32(dsSelectCustomer.Tables[0].Rows[counter].ItemArray[1]), myConn);
                    shippingAddress = selectShippingAddress(Convert.ToInt32(dsSelectCustomer.Tables[0].Rows[counter].ItemArray[2]), myConn);

                    //Linking address to the Customer
                    customer.setBillingAddress(billingAddress);
                    customer.setShippingAddress(shippingAddress);

                    //Push the Customer to an Array
                    customerArray[counter] = customer;
                }//END for

            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return customerArray;
        }//END searchCustomer()

        //Retrieve Billing Address as an Object
        private Address selectBillingAddress(int inBAddressID, MySqlConnection inMyConn)
        {
            try
            {
                string querySelectBAddress = "SELECT * FROM billingAddress WHERE billingAddressID='" +
                                             inBAddressID + "';";

                MySqlCommand cmdSelectBAddress = new MySqlCommand(querySelectBAddress, inMyConn);
                MySqlDataAdapter adapterBAddress = new MySqlDataAdapter(cmdSelectBAddress);
                DataSet dsSelect = new DataSet();
                adapterBAddress.Fill(dsSelect);

                billingAddress = new Address(dsSelect.Tables[0].Rows[0].ItemArray[1].ToString(),
                                             dsSelect.Tables[0].Rows[0].ItemArray[2].ToString(),
                                             dsSelect.Tables[0].Rows[0].ItemArray[3].ToString(),
                                             dsSelect.Tables[0].Rows[0].ItemArray[4].ToString(),
                                             Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[5]),
                                             inBAddressID);
            }//END try
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return billingAddress;
        }//END selectBillingAddress()

        //Retrieve Shipping Address as an Obejct
        private Address selectShippingAddress(int inSAddressID, MySqlConnection inMyConn)
        {
            try
            {
                string querySelectSAddress = "SELECT * FROM shippingAddress WHERE shippingAddressID='" +
                                             inSAddressID + "';";

                MySqlCommand cmdSelectSAddress = new MySqlCommand(querySelectSAddress, inMyConn);
                MySqlDataAdapter adapterSAddress = new MySqlDataAdapter(cmdSelectSAddress);
                DataSet dsSelect = new DataSet();
                adapterSAddress.Fill(dsSelect);

                shippingAddress = new Address(dsSelect.Tables[0].Rows[0].ItemArray[1].ToString(),
                                              dsSelect.Tables[0].Rows[0].ItemArray[2].ToString(),
                                              dsSelect.Tables[0].Rows[0].ItemArray[3].ToString(),
                                              dsSelect.Tables[0].Rows[0].ItemArray[4].ToString(),
                                              Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[5]),
                                              inSAddressID);
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return shippingAddress;
        }//END selectShippingAddress()

        //Retreieve Customer as an Object by Id
        private Customer searchCustomerById(string inCustomerId)
        {
            Customer theCustomer = new Customer();

            try
            {
                string querySelect = "SELECT * FROM customer WHERE customerId='" +
                                     inCustomerId + "';";


                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdSelect = new MySqlCommand(querySelect, myConn);
                MySqlDataAdapter adapterSelect = new MySqlDataAdapter(cmdSelect);
                DataSet dsSelect = new DataSet();

                adapterSelect.Fill(dsSelect);

                theCustomer = new Customer(dsSelect.Tables[0].Rows[0].ItemArray[0].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[3].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[4].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[5].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[6].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[7].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[8].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[9].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[10].ToString());

                //Retrieve information of Addresses
                billingAddress = selectBillingAddress(Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[1]), myConn);
                shippingAddress = selectShippingAddress(Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[2]), myConn);

                //Linking address to the Customer
                customer.setBillingAddress(billingAddress);
                customer.setShippingAddress(shippingAddress);

            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return theCustomer;
        }//END searchCustomerById()

        //Retreieve Customer as an Object by Name
        private Customer searchCustomerByName(string inCustomerName)
        {
            Customer theCustomer = new Customer();

            try
            {
                string querySelect = "SELECT * FROM customer WHERE customerFirstName='" +
                                     inCustomerName + "';";


                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdSelect = new MySqlCommand(querySelect, myConn);
                MySqlDataAdapter adapterSelect = new MySqlDataAdapter(cmdSelect);
                DataSet dsSelect = new DataSet();

                adapterSelect.Fill(dsSelect);

                theCustomer = new Customer(dsSelect.Tables[0].Rows[0].ItemArray[0].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[3].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[4].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[5].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[6].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[7].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[8].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[9].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[10].ToString());

                //Retrieve information of Addresses
                billingAddress = selectBillingAddress(Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[1]), myConn);
                shippingAddress = selectShippingAddress(Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[2]), myConn);

                //Linking address to the Customer
                customer.setBillingAddress(billingAddress);
                customer.setShippingAddress(shippingAddress);
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return theCustomer;
        }//END searchCustomerByName()

        //Retreieve Customer as an Object by 
        private Customer searchCustomerByMobile(string inCustomerMobile)
        {
            Customer theCustomer = new Customer();

            try
            {
                string querySelect = "SELECT * FROM customer WHERE mobile='" +
                                     inCustomerMobile + "';";


                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdSelect = new MySqlCommand(querySelect, myConn);
                MySqlDataAdapter adapterSelect = new MySqlDataAdapter(cmdSelect);
                DataSet dsSelect = new DataSet();

                adapterSelect.Fill(dsSelect);

                theCustomer = new Customer(dsSelect.Tables[0].Rows[0].ItemArray[0].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[3].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[4].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[5].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[6].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[7].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[8].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[9].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[10].ToString());

                //Retrieve information of Addresses
                billingAddress = selectBillingAddress(Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[1]), myConn);
                shippingAddress = selectShippingAddress(Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[2]), myConn);

                //Linking address to the Customer
                customer.setBillingAddress(billingAddress);
                customer.setShippingAddress(shippingAddress);
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return theCustomer;
        }//END searchCustomerByMobile()

        //Retreieve Customer as an Object by Email
        private Customer searchCustomerByEmail(string inCustomerEmail)
        {
            Customer theCustomer = new Customer();

            try
            {
                string querySelect = "SELECT * FROM customer WHERE emailAddress='" +
                                     inCustomerEmail + "';";


                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdSelect = new MySqlCommand(querySelect, myConn);
                MySqlDataAdapter adapterSelect = new MySqlDataAdapter(cmdSelect);
                DataSet dsSelect = new DataSet();

                adapterSelect.Fill(dsSelect);

                theCustomer = new Customer(dsSelect.Tables[0].Rows[0].ItemArray[0].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[3].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[4].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[5].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[6].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[7].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[8].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[9].ToString(),
                                           dsSelect.Tables[0].Rows[0].ItemArray[10].ToString());

                //Retrieve information of Addresses
                billingAddress = selectBillingAddress(Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[1]), myConn);
                shippingAddress = selectShippingAddress(Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[2]), myConn);

                //Linking address to the Customer
                customer.setBillingAddress(billingAddress);
                customer.setShippingAddress(shippingAddress);
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return theCustomer;
        }//END searchCustomerByEmail()


        #endregion

#region /*Create New Customer functions*/

        //Run query to INSERT billing address
        private void addNewCustomerBillingAddress()
        {
            try
            {
                string queryAddress = "INSERT INTO billingAddress(addressLine1, addressLine2, suburb, state, postcode) " +
                                      "VALUES('" +
                                      billingAddress.getAddressLine1() + "','" +
                                      billingAddress.getAddressLine2() + "','" +
                                      billingAddress.getSuburb() + "','" +
                                      billingAddress.getState() + "','" +
                                      billingAddress.getPostcode() + "');";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmd = new MySqlCommand(queryAddress, myConn);
                MySqlDataReader reader;

                reader = cmd.ExecuteReader();
                myConn.Close();

            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch
        }//END addNewCustomerBillingAddress()

        //Run query to INSERT shpping address
        private void addNewCustomerShippingAddress()
        {
            try
            {
                string queryAddress = "INSERT INTO shippingAddress(addressLine1, addressLine2, suburb, state, postcode) " +
                                      "VALUES('" +
                                      shippingAddress.getAddressLine1() + "','" +
                                      shippingAddress.getAddressLine2() + "','" +
                                      shippingAddress.getSuburb() + "','" +
                                      shippingAddress.getState() + "','" +
                                      shippingAddress.getPostcode() + "');";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmd = new MySqlCommand(queryAddress, myConn);
                MySqlDataReader reader;

                reader = cmd.ExecuteReader();
                myConn.Close();

            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch
        }//END addNewCustomerShippingAddress()

        //Run query to INSERT customer
        private void addNewCustomer()
        {
            try
            {
                int theBAddressID;
                int theSAddressID;

                string queryAddNewCustomer;

                string queryTheBAddressID = "SELECT billingAddressID from billingAddress WHERE addressLine1='" +
                                            billingAddress.getAddressLine1() + "';";

                string queryTheSAddressID = "SELECT shippingAddressID from shippingAddress WHERE addressLine1='" +
                                            shippingAddress.getAddressLine1() + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdGetBAddressID = new MySqlCommand(queryTheBAddressID, myConn);
                MySqlCommand cmdGetSAddressID = new MySqlCommand(queryTheSAddressID, myConn);
                MySqlDataAdapter adapterBAddressID = new MySqlDataAdapter(cmdGetBAddressID);
                MySqlDataAdapter adapterSAddressID = new MySqlDataAdapter(cmdGetSAddressID);
                DataSet dsBAddressID = new DataSet();
                DataSet dsSAddressID = new DataSet();
                adapterBAddressID.Fill(dsBAddressID);
                adapterSAddressID.Fill(dsSAddressID);

                theBAddressID = Convert.ToInt32(dsBAddressID.Tables[0].Rows[0].ItemArray[0]);
                theSAddressID = Convert.ToInt32(dsSAddressID.Tables[0].Rows[0].ItemArray[0]);

                queryAddNewCustomer = "INSERT INTO customer VALUES('" +
                                      txtBoxCustomerIDToAdd.Text + "','" +
                                      theBAddressID + "','" +
                                      theSAddressID + "','" +
                                      customer.getCustomerFirstName() + "','" +
                                      customer.getCustomerLastName() + "','" +
                                      customer.getGender() + "','" +
                                      customer.getLandline() + "','" +
                                      customer.getMobile() + "','" +
                                      customer.getFax() + "','" +
                                      customer.getEmailAddress() + "','" +
                                      customer.getDateOfBirth() + "');";

                MySqlCommand cmdNewCustomer = new MySqlCommand(queryAddNewCustomer, myConn);
                MySqlDataReader reader;

                reader = cmdNewCustomer.ExecuteReader();
                MessageBox.Show(customer.getCustomerFirstName() + " has been added!");

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch
        }//END addNewCustomer()
        #endregion

#region /*Update Customer functions*/

        private void updateCustomer()
        {
            try
            {
                string queryUpdateCustomer = "UPDATE customer SET customerFirstName='" + customer.getCustomerFirstName() + "'," +
                                             "customerLastName='" + customer.getCustomerLastName() + "'," +
                                             "gender='" + customer.getGender() + "'," +
                                             "landline='" + customer.getLandline() + "'," +
                                             "mobile='" + customer.getMobile() + "'," +
                                             "fax='" + customer.getFax() + "'," +
                                             "emailAddress='" + customer.getEmailAddress() + "'," +
                                             "dateBirth='" + customer.getDateOfBirth() + "'" +
                                             "WHERE customerID='" + customer.getCustomerID() + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdUpdate = new MySqlCommand(queryUpdateCustomer, myConn);
                MySqlDataReader reader;

                reader = cmdUpdate.ExecuteReader();

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

        }//END updateCustomer()

        private void updateBillingAddress()
        {
            try
            {
                string queryUpdate = "UPDATE billingaddress SET addressLine1='" + billingAddress.getAddressLine1() + "'," +
                                     "addressLine2='" + billingAddress.getAddressLine2() + "'," +
                                     "suburb='" + billingAddress.getSuburb() + "'," +
                                     "state='" + billingAddress.getState() + "'," +
                                     "postcode='" + billingAddress.getPostcode() +"'" +
                                     "WHERE billingAddressID='" + billingAddress.getAddressId() + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, myConn);
                MySqlDataReader reader;

                reader = cmdUpdate.ExecuteReader();
                myConn.Close();

            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch
        }//END updateBillingAddress()

        private void updateShippingAddress()
        {
            try
            {
                string queryUpdate = "UPDATE shippingaddress SET addressLine1='" + shippingAddress.getAddressLine1() + "'," +
                                     "addressLine2='" + shippingAddress.getAddressLine2() + "'," +
                                     "suburb='" + shippingAddress.getSuburb() + "'," +
                                     "state='" + shippingAddress.getState() + "'," +
                                     "postcode='" + shippingAddress.getPostcode() + "'" +
                                     "WHERE shippingAddressID='" + shippingAddress.getAddressId() + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, myConn);
                MySqlDataReader reader;

                reader = cmdUpdate.ExecuteReader();
                myConn.Close();

            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch
        }//END updateShippingAddress()



        #endregion

#region /*Delete Customer function*/
        private void deleteCustomer()
        {
            try
            {
                string queryDelete = "DELETE FROM customer WHERE customerID='" + customer.getCustomerID() + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdDelete = new MySqlCommand(queryDelete, myConn);
                MySqlDataReader reader;

                reader = cmdDelete.ExecuteReader();
                MessageBox.Show(customer.getCustomerFirstName() + customer.getCustomerLastName() + 
                                "has been deleted");

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

        }//END deleteCustomer()

        private void deleteBillingAddress()
        {
            try
            {
                string queryDelete = "DELETE FROM billingaddress WHERE billingAddressID='" + billingAddress.getAddressId() + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdDelete = new MySqlCommand(queryDelete, myConn);
                MySqlDataReader reader;

                reader = cmdDelete.ExecuteReader();

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

        }//END deleteBillingAddress()

        private void deleteShippingAddress()
        {
            try
            {
                string queryDelete = "DELETE FROM shippingaddress WHERE shippingAddressID='" + shippingAddress.getAddressId() + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdDelete = new MySqlCommand(queryDelete, myConn);
                MySqlDataReader reader;

                reader = cmdDelete.ExecuteReader();

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

        }//END deleteShippingAddress()

        #endregion


#region /*Search Item Function*/

        
        private Item[] searchAllItem(string[] inItemCodeArray)
        {
            Item theItem = new Item();
            Item[] theItemArray = new Item[50];
            
            try
            {
                MySqlConnection myConn = databaseConnection.openConnection();

                for (int counter = 0; inItemCodeArray[counter] != null; counter++)
                {
                    theItem = searchItemWithConn(inItemCodeArray[counter], myConn);

                    //Placing item into corresponded position
                    theItemArray[counter] = theItem;
                }//END for
                
                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return theItemArray;

        }//END searchAllItem()


        //Search the item and obtain its detail by clicking listViewItem
        private Item searchItemWithConn(string inItemCode, MySqlConnection inMyConn)
        {
            Item theItem = new Item();

            try
            {
                string querySelect = "SELECT * FROM item WHERE itemCode='" +
                                     inItemCode + "';";


                MySqlCommand cmdSelect = new MySqlCommand(querySelect, inMyConn);
                MySqlDataAdapter adapterSelect = new MySqlDataAdapter(cmdSelect);
                DataSet dsSelect = new DataSet();

                adapterSelect.Fill(dsSelect);

                theItem = new Item(dsSelect.Tables[0].Rows[0].ItemArray[0].ToString(),
                                   dsSelect.Tables[0].Rows[0].ItemArray[1].ToString(),
                                   Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[2]),
                                   Convert.ToDouble(dsSelect.Tables[0].Rows[0].ItemArray[3]));
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return theItem;
        }//END searchItem()

        private Item searchItem(string inItemCode)
        {
            Item theItem = new Item();

            try
            {
                string querySelect = "SELECT * FROM item WHERE itemCode='" +
                                     inItemCode + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdSelect = new MySqlCommand(querySelect, myConn);
                MySqlDataAdapter adapterSelect = new MySqlDataAdapter(cmdSelect);
                DataSet dsSelect = new DataSet();

                adapterSelect.Fill(dsSelect);

                theItem = new Item(dsSelect.Tables[0].Rows[0].ItemArray[0].ToString(),
                                   dsSelect.Tables[0].Rows[0].ItemArray[1].ToString(),
                                   Convert.ToInt32(dsSelect.Tables[0].Rows[0].ItemArray[2]),
                                   Convert.ToDouble(dsSelect.Tables[0].Rows[0].ItemArray[3]));

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return theItem;
        }//END searchItem()

        #endregion

#region /*Create New Item function*/

        private void addNewItem()
        {
            try
            {
                string queryInsertItem = "INSERT INTO item VALUES('" +
                                        item.getItemCode() + "','" +
                                        item.getItemDescription() + "','" +
                                        item.getQuantity() + "','" +
                                        item.getUnitPriceWithTax() + "','" +
                                        item.getTotalItemTax() + "','" +
                                        item.getTotalItemPrice() + "');";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdInsert = new MySqlCommand(queryInsertItem, myConn);
                MySqlDataReader reader;

                reader = cmdInsert.ExecuteReader();
                MessageBox.Show("Item Added");

                myConn.Close();

            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

        }//END addNewItem()

#endregion

#region /*Update Item Function*/
        private void updateItem()
        {
            try
            {
                string queryUpdate = "UPDATE item SET itemCode='" + item.getItemCode()+ "'," +
                                     "itemDescription='" + item.getItemDescription() + "'," +
                                     "quantity='" + item.getQuantity() + "'," +
                                     "unitPriceWithTax='" + item.getUnitPriceWithTax() + "'," +
                                     "totalItemTax='" + item.getTotalItemTax() + "'," +
                                     "totalItemPrice='" + item.getTotalItemPrice();

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, myConn);
                MySqlDataReader reader;

                reader = cmdUpdate.ExecuteReader();
                MessageBox.Show("Item Updated");

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

        }//END updateItem()
        

#endregion

#region /*Delete Item Function*/

        private void deleteItem()
        {
            try
            {
                string queryDelete = "DELETE FROM item WHERE itemCode='" + item.getItemCode() + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdDelete = new MySqlCommand(queryDelete, myConn);
                MySqlDataReader reader;

                reader = cmdDelete.ExecuteReader();
                MessageBox.Show("Item deleted");

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

        }//END deleteItem()
        #endregion


#region /*Search Invoice function*/
        private Invoice searchInvoice(string inInvoiceNumber)
        {
            Invoice theInvoice = new Invoice();
            CID = "";

            try
            {
                string querySelect = "Select * FROM invoice WHERE invoiceNumber='" + inInvoiceNumber + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdSelect = new MySqlCommand(querySelect, myConn);
                MySqlDataAdapter adapterSelect = new MySqlDataAdapter(cmdSelect);
                DataSet dsSelect = new DataSet();

                adapterSelect.Fill(dsSelect);

                theInvoice = new Invoice(dsSelect.Tables[0].Rows[0].ItemArray[0].ToString(),
                                         dsSelect.Tables[0].Rows[0].ItemArray[2].ToString(),
                                         dsSelect.Tables[0].Rows[0].ItemArray[3].ToString(),
                                         dsSelect.Tables[0].Rows[0].ItemArray[4].ToString(),
                                         dsSelect.Tables[0].Rows[0].ItemArray[5].ToString(),
                                         dsSelect.Tables[0].Rows[0].ItemArray[6].ToString(),
                                         Convert.ToDouble(dsSelect.Tables[0].Rows[0].ItemArray[7]),
                                         Convert.ToDouble(dsSelect.Tables[0].Rows[0].ItemArray[8]),
                                         Convert.ToDouble(dsSelect.Tables[0].Rows[0].ItemArray[9]),
                                         Convert.ToDouble(dsSelect.Tables[0].Rows[0].ItemArray[10]),
                                         Convert.ToDouble(dsSelect.Tables[0].Rows[0].ItemArray[11]));

                CID = dsSelect.Tables[0].Rows[0].ItemArray[1].ToString();

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return theInvoice;
        }//END searchInvoice()

#endregion

#region /*Create New Invoice function*/

        private void addNewInvoice(String inCustomerID)
        {
            try
            {
                string queryInsertInvoice = "INSERT INTO invoice VALUES('" +
                                            invoice.getInvoiceNumber() + "','" +
                                            inCustomerID + "','" +
                                            invoice.getInvoiceDate() + "','" +
                                            invoice.getInvoiceStatus() + "','" +
                                            invoice.getPaymentMethod() + "','" +
                                            invoice.getShippingMethod() + "','" +
                                            invoice.getStore() + "','" +
                                            invoice.getShippingIncludeTax() + "','" +
                                            invoice.getTotalIncludeTax() + "','" +
                                            invoice.getTotalTaxAmount() + "','" +
                                            invoice.getPayment() + "','" +
                                            invoice.getBalanceOwing() + "');";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdInsert = new MySqlCommand(queryInsertInvoice, myConn);
                MySqlDataReader reader;

                reader = cmdInsert.ExecuteReader();
                MessageBox.Show("Invoice Added");

                myConn.Close();

            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

        }//END addNewInvoice()

#endregion

#region /*Update Invoice function*/

        private void updateInvoice()
        {
            try
            {
                string queryUpdate = "UPDATE invoice SET invoiceNumber='" + invoice.getInvoiceNumber() + "'," +
                                     "customerID='" + customer.getCustomerID() + "','" +
                                     "invoiceDate='" + invoice.getInvoiceDate() + "','" +
                                     "invoiceStatus='" + invoice.getInvoiceStatus() + "','" +
                                     "paymentMethod='" + invoice.getPaymentMethod() + "','" +
                                     "shippingMethod='" + invoice.getShippingMethod() + "','" +
                                     "store='" + invoice.getStore() + "','" +
                                     "shippingIncludeTAX='" + invoice.getShippingIncludeTax() + "','" +
                                     "totalIncludeTax='" + invoice.getTotalIncludeTax() + "','" +
                                     "payment='" + invoice.getPayment() + "','" +
                                     "balanceOwing='" + invoice.getBalanceOwing() + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, myConn);
                MySqlDataReader reader;

                reader = cmdUpdate.ExecuteReader();
                MessageBox.Show("Invoice Updated");

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

        }//END updateInvoice()


        #endregion

#region /*Delete Invoice function*/
        private void deleteInvoice()
        {
            try
            {
                string queryDelete = "DELETE FROM invoice WHERE invoiceNumber='" + invoice.getInvoiceNumber() + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdDelete = new MySqlCommand(queryDelete, myConn);
                MySqlDataReader reader;

                reader = cmdDelete.ExecuteReader();
                MessageBox.Show("Invoice Delete");

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END ctach

        }//END deleteInvoice()

#endregion


#region /*Search InvoiceItem function*/

        private string[] searchInvoiceItem(string inInvoiceNumber)
        {
            string[] theItemCodeArray = new string[50];

            try
            {
                string querySelect = "SELECT * FROM invoice_item WHERE invoiceNumber='" +
                                     inInvoiceNumber + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdSelect = new MySqlCommand(querySelect, myConn);
                MySqlDataAdapter adapterSelect = new MySqlDataAdapter(cmdSelect);
                DataSet dsSelect = new DataSet();

                adapterSelect.Fill(dsSelect);

                for (int counter = 0; counter < dsSelect.Tables[0].Rows.Count; counter++)
                {
                    theItemCodeArray[counter] = dsSelect.Tables[0].Rows[counter].ItemArray[1].ToString();

                }//END for

                myConn.Close();
            }//END try
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

            return theItemCodeArray;
        }//END searchItemCodeArray()

        #endregion

#region /*Create InvoiceItem function*/

        private void addInvoiceItem(string inInvoiceNum, Item[] inItemArray)
        {
            string[] itemCodeArray = new string[50];

            for (int counterGetItem = 0; inItemArray[counterGetItem] != null; counterGetItem++)
            {
                itemCodeArray[counterGetItem] = inItemArray[counterGetItem].getItemCode();
            }//END for
            
            for (int counterToAdd = 0; itemCodeArray[counterToAdd]!=null; counterToAdd++)
            {
                try
                {
                    string queryAdd = "INSERT INTO invoice_item(itemCode, invoiceNumber) VALUES('" +
                                      itemCodeArray[counterToAdd] + "','" +
                                      inInvoiceNum + "');";

                    MySqlConnection myConn = databaseConnection.openConnection();

                    MySqlCommand cmdAdd = new MySqlCommand(queryAdd, myConn);
                    MySqlDataReader reader;

                    reader = cmdAdd.ExecuteReader();

                    myConn.Close();
                }//END try
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }//END catch
            }//END for

        }//END addInvoiceItem()

        #endregion

#region /*Delete InvoiceItem function*/

        private void deleteInvoiceItem(string inInvoice, string inItem)
        {
            try
            {
                string queryDelete = "DELETE FROM invoice_item WHERE invoiceNumber='" +
                                     inInvoice + "' AND itemCode='" +
                                     inItem + "';";

                MySqlConnection myConn = databaseConnection.openConnection();
                MySqlCommand cmdDelete = new MySqlCommand(queryDelete, myConn);
                MySqlDataReader reader;

                reader = cmdDelete.ExecuteReader();

                myConn.Close();
            }//END try
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }//END catch

        }//END deleteInvoiceItem




        #endregion

#endregion

        
    }//END class
}//END