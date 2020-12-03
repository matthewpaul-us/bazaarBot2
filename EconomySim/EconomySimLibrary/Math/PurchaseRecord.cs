using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomySim
{ 
    //TODO: convert to a struct
    public class PurchaseRecord
    {
        public double Amount;
        public double Price;
        public PurchaseRecord(double amount, double price)
        {
            this.Amount = amount;
            this.Price = price;
        }
    }
}
