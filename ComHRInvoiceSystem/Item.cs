using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComHRInvoiceSystem
{
    class Item
    {
        private String itemCode;
        private String itemDescription;
        private int quantity;
        private double unitPriceWithTax;
        private double totalItemTax;
        private double totalItemPrice;

        #region /*All Constructors*/
        //Default Constructor
        public Item()
        {
            itemCode = "";
            itemDescription = "";
            quantity = 0;
            unitPriceWithTax = 0.0;
            totalItemTax = 0.0;
            totalItemPrice = 0.0;
        }

        //Alternate Constructor
        public Item(String inItemCode, String inItemDescription, int inQuantity,
                    double inUnitPriceWithTax)
        {
            itemCode = inItemCode;
            itemDescription = inItemDescription;
            quantity = inQuantity;
            unitPriceWithTax = inUnitPriceWithTax;
            setTotalItemTax();
            setTotalItemPrice();
        }

        //Copy Constructor
        public Item(Item item)
        {
            itemCode = item.getItemCode();
            itemDescription = item.getItemDescription();
            quantity = item.getQuantity();
            unitPriceWithTax = item.getUnitPriceWithTax();
            totalItemTax = item.getTotalItemTax();
            totalItemPrice = item.getTotalItemPrice();
        }
        #endregion


        #region /*All Mutators*/
        //Mutator of Object
        public void setItem(String inItemCode, String inItemDescription, int inQuantity,
                    double inUnitPriceWithTax)
        {
            setItemCode(inItemCode);
            setItemDescription(inItemDescription);
            setQuantity(inQuantity);
            setUnitPriceWithTax(inUnitPriceWithTax);
            setTotalItemTax();
            setTotalItemPrice();
        }

        public void setItemCode(String inItemCode)
        {
            itemCode = inItemCode;
        }

        public void setItemDescription(String inItemDescription)
        {
            itemDescription = inItemDescription;
        }

        public void setQuantity(int inQuantity)
        {
            quantity = inQuantity;
        }

        public void setUnitPriceWithTax(double inUnitPriceWithTax)
        {
            unitPriceWithTax = inUnitPriceWithTax;
        }

        public void setTotalItemTax()
        {
            //Good & Services tax are 10% in Australia
            //Hence item's tax will be the good price with tax taken
            //by the good price divided by 1.1
            //It is a total goods tax, hence multiply by quantity.
            totalItemTax = (getUnitPriceWithTax() - (getUnitPriceWithTax()/1.1)) * getQuantity();
        }

        public void setTotalItemPrice()
        {
            //Total price is unit price with tax mutipl
            totalItemPrice = getUnitPriceWithTax() * getQuantity();
        }
        #endregion


        #region /*All Accessors*/
        //Accessor
        public String getItemCode()
        {
            return itemCode;
        }

        public String getItemDescription()
        {
            return itemDescription;
        }

        public int getQuantity()
        {
            return quantity;
        }

        public double getUnitPriceWithTax()
        {
            return Math.Round(unitPriceWithTax, 2);
        }

        public double getTotalItemTax()
        {
            return Math.Round(totalItemTax, 2);
        }

        public double getTotalItemPrice()
        {
            return Math.Round(totalItemPrice, 2);
        }
        #endregion

    }//END class
}//END
