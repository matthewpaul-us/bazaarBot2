using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomySim
{
	/// <summary>
	/// Base class for the logic that agents can perform. Think of jobs and responsibilities.
	/// </summary>
    public abstract class Logic
    {
		/// <summary>
		/// Perform this logic on the given agent. This logic runs once per tick.
		/// </summary>
		/// <param name="agent">The agent to perform this logic on.</param>
		/// <param name="market">The market associated with this agent.</param>
		public abstract void Perform(BasicAgent agent, Market market);

        protected void Produce(BasicAgent agent, String commodity, double amount, double chance = 1.0)
	    {
		    if (chance >= 1.0 || Quick.rnd.NextDouble() < chance)
		    {
			    agent.ProduceInventory(commodity, amount);
		    }
	    }

	    protected void Consume(BasicAgent agent, String commodity, double amount, double chance = 1.0)
	    {
            if (chance >= 1.0 || Quick.rnd.NextDouble() < chance)
		    {
                //if (commodity == "money")
                //{
                //    agent.changeInventory(comm
                //    agent.money -= amount;
                //}
                //else
                //{
				    agent.ConsumeInventory(commodity, -amount);
                //}
		    }
	    }
    } 
}
