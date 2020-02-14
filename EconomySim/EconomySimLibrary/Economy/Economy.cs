using EconomySimLibrary.Bankrupt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomySim
{
	/// <summary>
	/// Top level object.  Itended to be extended for the specific economy a game implements.
	/// </summary>
    public class Economy : ISignalBankrupt
    {
	    private List<Market> markets;

		//Automatically called when the inherited class is instantiated
        public Economy()
	    {
		    markets = new List<Market>();
	    }

	    public void AddMarket(Market m)
	    {
		    if (markets.IndexOf(m) == -1)
		    {
			    markets.Add(m);
		    }
	    }

	    public Market GetMarket(string name)
	    {
		    foreach (var m in markets)
		    {
			    if (m.Name == name) return m;
		    }
		    return null;
	    }

	    public void Simulate(int rounds)
	    {
		    foreach (var m in markets)
		    {
			    m.Simulate(rounds);
		    }
	    }


	    public virtual void SignalBankrupt(Market market, BasicAgent agent)
	    {
		    //no implemenation -- provide your own in a subclass
	    }

    }

}
