using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomySim
{
    public class InventoryData
    {
	    public double MaxSize;
	    public Dictionary<String, double> Ideal;
	    public Dictionary<String, double> Start;
	    public Dictionary<String, double> Size;

	    public InventoryData(double maxSize, Dictionary<String,double> ideal, Dictionary<String,double> start, Dictionary<String,double> size)
	    {
		    this.MaxSize = maxSize;
		    this.Ideal = ideal;
		    this.Start = start;
		    this.Size = size;

            if (this.Size == null)
            {
                this.Size = new Dictionary<string, double>();
                foreach (KeyValuePair<String, double> entry in start)
                {
                    this.Size[entry.Key] = 1;
                }
            }
	    }
    }
}
