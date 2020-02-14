using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomySim
{
    public class MarketData
    {
	    public List<Good> goods;
	    public List<AgentData> agentTypes;
	    public List<BasicAgent> agents;

	    public MarketData(List<Good> goods, List<AgentData> agentTypes, List<BasicAgent> agents)
	    {
		    this.goods = goods;
		    this.agentTypes = agentTypes;
		    this.agents = agents;
	    } 
    }
}
