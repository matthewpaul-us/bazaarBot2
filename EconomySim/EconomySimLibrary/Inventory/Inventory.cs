using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomySim
{
	/// <summary>
	/// An inventory used by agents.
	/// </summary>
    public class Inventory
    {
	    public double maxSize = 0;

	    //private static var _index:Map<String, Commodity>;

	    private Dictionary<String, PurchaseRecord> stuff;		// key:commodity_id, val:(x:amount, y:original_cost)
	    private Dictionary<String, double> ideal;		// ideal counts for each thing
	    private Dictionary<String, double> sizes;		// how much space each thing takes up


		/// <summary>
		/// Create an inventory to hold items.
		/// </summary>
	    public Inventory()
	    {
		    sizes = new Dictionary<String, double>();
		    stuff = new Dictionary<String, PurchaseRecord>();
		    ideal = new Dictionary<String, double>();
		    maxSize = 0;
	    }

	    public void FromData(InventoryData data)
	    {
		    var sizes = new List<string>();
		    var amountsp = new List<PurchaseRecord>();
		    foreach (string key in data.Start.Keys)
		    {
			    sizes.Add(key);
			    amountsp.Add(new PurchaseRecord(data.Start[key],0));
		    }
		    SetStuff(sizes, amountsp);
		    sizes = new List<string>();
		    var amounts = new List<double>();
		    foreach (string key in data.Size.Keys)
		    {
			    sizes.Add(key);
			    amounts.Add(data.Size[key]);
		    }
		    SetSizes(sizes, amounts);
		    sizes = new List<string>();
		    amounts = new List<double>();
		    foreach (string key in data.Ideal.Keys)
		    {
			    sizes.Add(key);
			    amounts.Add(data.Ideal[key]);
			    SetIdeal(sizes, amounts);
		    }
		    maxSize = data.MaxSize;
	    }

		/// <summary>
		/// Prepares this inventory for cleanup.
		/// </summary>
	    public void Destroy()
	    {
            stuff.Clear();
            ideal.Clear();
            sizes.Clear();
		    stuff = null;
		    ideal = null;
		    sizes = null;
	    }

	    /**
	     * Set amounts of various commodities
	     * @param	stuff_
	     * @param	amounts_
	     */

	    public void SetStuff(List<String>stuff, List<PurchaseRecord>amounts)
	    {
		    for (int i=0; i<stuff.Count; i++)
		    {
			    this.stuff[stuff[i]] = amounts[i];
		    }
	    }

	    /**
	     * Set how much of each commodity to stockpile
	     * @param	stuff_
	     * @param	amounts_
	     */

	    public void SetIdeal(List<String>ideal, List<double>amounts)
	    {
		    for (int i=0; i<ideal.Count; i++)
		    {
			    this.ideal[ideal[i]] = amounts[i];
		    }
	    }

	    public void SetSizes(List<String>sizes, List<double>amounts)
	    {
		    for(int i=0; i<sizes.Count; i++)
		    {
			    this.sizes[sizes[i]] = amounts[i];
		    }
	    }

		/// <summary>
		/// Returns how much of a specific good
		/// </summary>
		/// <param name="good">The good to check.</param>
		/// <returns>The amount contained. Cannot be negative.</returns>
	    public double Query(String good)
	    {
		    if (stuff.ContainsKey(good))
		    {
			    return stuff[good].Amount;
		    }
		    return 0;
	    }

		/// <summary>
		/// Returns the cost of a specific good
		/// </summary>
		/// <param name="good">The good to check.</param>
		/// <returns>The original cost of the good. Cannot be negative</returns>
        public double QueryCost(String good)
        {
            if (stuff.ContainsKey(good))
            {
                return stuff[good].Price;
            }
            return 0;
        }

		/// <summary>
		/// Gets the amount of free space the inventory has.
		/// </summary>
		/// <returns>Amount of space free</returns>
	    public double GetEmptySpace()
	    {
		    return maxSize - GetUsedSpace();
	    }

		/// <summary>
		/// Gets the amount of used space the inventory has.
		/// </summary>
		/// <returns>The count of all goods in the inventory, taking into account the size of the good.</returns>
	    public double GetUsedSpace()
	    {
		    double space_used = 0;
		    foreach (string key in stuff.Keys)
		    {
                if (!sizes.ContainsKey(key)) continue;
			    space_used += stuff[key].Amount * sizes[key];
		    }
		    return space_used;
	    }

		/// <summary>
		/// Gets the size of a good.
		/// </summary>
		/// <param name="good">The good to check the size of.</param>
		/// <returns>The Size of the good</returns>
	    public double GetSizeFor(string good)
	    {
		    if (sizes.ContainsKey(good))
		    {
			    return sizes[good];
		    }
		    return -1;
	    }

		/// <summary>
		/// Change the amount of the given good by delta amount.
		/// </summary>
		/// <param name="good">The good to change.</param>
		/// <param name="delta">The amount to change the good by.</param>
		/// <param name="unit_cost">The amount the delta was bought for. Leave as 0 or negative to use the old price.</param>
		/// <returns>The current unit's cost.</returns>
	    public double Change(string good, double delta, double unit_cost)
	    {
		    PurchaseRecord result = new PurchaseRecord(0,0);

		    if (stuff.ContainsKey(good))
		    {
			    var amount = stuff[good];
                if (unit_cost > 0)
                {
                    if (amount.Amount <= 0)
                    {
                        result.Amount = delta;
                        result.Price = unit_cost;
                    }
                    else
                    {
						// The new buy price is the weighted average of the old and new price
                        result.Price = (amount.Amount * amount.Price + delta * unit_cost) / (amount.Amount + delta);
                        result.Amount = amount.Amount + delta;
                    }
                }
                else
                {
                    result.Amount = amount.Amount + delta;
                    result.Price = amount.Price; //just copy from old value?
                }
		    }
		    else
		    {
			    result.Amount = delta;
                result.Price = unit_cost;
		    }

		    if (result.Amount < 0)
		    {
			    result.Amount = 0;
                result.Price = 0;
		    }

		    stuff[good] = result;
            return result.Price; //return current unit cost
	    }

		///<summary>
		///	Returns number of units above the desired inventory level.
		///</summary>
		///<returns>Number of units above, or 0 if inventory is at level or below.</returns>
	    public double Surplus(string good)
	    {
		    var amt = Query(good);
            double ideal = 0;
            if (this.ideal.ContainsKey(good))
                ideal = this.ideal[good];
		    if (amt > ideal)
		    {
			    return (amt - ideal);
		    }
		    return 0;
	    }

		///<summary>Returns number of units below the desired inventory level.</summary>
		///<returns>Number of units below, or 0 if inventory is at level or above.</returns>
	    public double Shortage(string good)
	    {
		    if (!stuff.ContainsKey(good))
		    {
			    return 0;
		    }
		    var amt = Query(good);
            double ideal = 0;
            if (this.ideal.ContainsKey(good))
                ideal = this.ideal[good];
		    if (amt < ideal)
		    {
			    return (ideal - amt);
		    }
		    return 0;
	    }

    }
}
