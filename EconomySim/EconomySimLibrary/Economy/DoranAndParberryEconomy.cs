using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace EconomySim
{
    public class DoranAndParberryEconomy : Economy
    {
        /// <summary>
        /// Sets up a default economy and initializes.
        /// </summary>
	    public DoranAndParberryEconomy()
	    {
		    var market = new Market("default",this);

            MarketData data = getMarketData(); //Gets default market data below
            market.Init(data); // market.init(MarketData.fromJSON(Json.parse(Assets.getText("assets/settings.json")), getAgent));
		    AddMarket(market);
	    }

        //Gets default market data
        //Should we pull from DB or cache and call each iteration to accomodate outages?
        private MarketData getMarketData()
        {
            List<Good> goods = new List<Good>();
	        List<AgentData> agentTypes = new List<AgentData>();
	        List<BasicAgent> agents = new List<BasicAgent>();

            goods.Add(new Good("food", 0.5));
            goods.Add(new Good("wood", 1.0));
            goods.Add(new Good("ore", 1.0));
            goods.Add(new Good("metal", 1.0));
            goods.Add(new Good("tools", 1.0));
            goods.Add(new Good("work", 0.1));

            agentTypes.Add(new AgentData("farmer",100,"farmer"));
            agentTypes.Add(new AgentData("miner",100,"miner"));
            agentTypes.Add(new AgentData("refiner",100,"refiner"));
            agentTypes.Add(new AgentData("woodcutter",100,"woodcutter"));
            agentTypes.Add(new AgentData("blacksmith", 100, "blacksmith"));
            agentTypes.Add(new AgentData("worker", 10, "worker"));

            InventoryData ii;
            ii = new InventoryData(20, //farmer
                new Dictionary<string,double>{{"food",0},{"tools",1},{"wood",3},{"work",3}},
                new Dictionary<string,double>{{"food",1},{"tools",1},{"wood",0},{"work",0}},
                null
                );
            agentTypes[0].Inventory = ii; 
            ii = new InventoryData(20, //miner
                new Dictionary<string, double> { { "food", 3 }, { "tools", 1 }, { "ore", 0 } },
                new Dictionary<string, double> { { "food", 1 }, { "tools", 1 }, { "ore", 0 } },
                null
                );
            agentTypes[1].Inventory = ii;
            ii = new InventoryData(20, //refiner
                new Dictionary<string, double> { { "food", 3 }, { "tools", 1 }, { "ore", 5 } },
                new Dictionary<string, double> { { "food", 1 }, { "tools", 1 }, { "ore", 0 } },
                null
                );
            agentTypes[2].Inventory = ii;
            ii = new InventoryData(20, //woodcutter
                new Dictionary<string, double> { { "food", 3 }, { "tools", 1 }, { "wood", 0 } },
                new Dictionary<string, double> { { "food", 1 }, { "tools", 1 }, { "wood", 0 } },
                null
                );
            agentTypes[3].Inventory = ii;
            ii = new InventoryData(20, //blacksmith
                new Dictionary<string, double> { { "food", 3 }, { "tools", 1 }, { "metal", 5 }, { "ore", 0 } },
                new Dictionary<string, double> { { "food", 1 }, { "tools", 0 }, { "metal", 0 }, { "ore", 0 } },
                null
                );
            agentTypes[4].Inventory = ii;
            ii = new InventoryData(20, //worker
                new Dictionary<string, double> { { "food", 3 }  },
                new Dictionary<string, double> { { "food", 1 } },
                null
                );
            agentTypes[5].Inventory = ii;


            int idc = 0;
            for (int iagent = 0; iagent < agentTypes.Count; iagent++)
            {
                for (int i = 0; i < 5; i++)
                {
                    agents.Add(getAgent(agentTypes[iagent]));
                    agents[agents.Count - 1].Id = idc++;
                }
            }


            MarketData data = new MarketData(goods, agentTypes, agents);

            return data;
        }

        /// <summary>
        /// An agent ran out of money and now will be replaced!
        /// </summary>
        /// <param name="market"></param>
        /// <param name="agent"></param>
	    public override void SignalBankrupt(Market market, BasicAgent agent)
	    {
		    replaceAgent(market, agent);
	    }

        /// <summary>
        /// Figures out which which agent is currently most profitable.  
        /// This is currently used for bankrupt agents.
        /// </summary>
        /// <param name="market"></param>
        /// <param name="agent"></param>
	    private void replaceAgent(Market market, BasicAgent agent)
	    {
		    var bestClass = market.GetMostProfitableAgentClass();

		    //Special case to deal with very high demand-to-supply ratios
		    //This will make them favor entering an underserved market over
		    //Just picking the most profitable class
		    var bestGood = market.GetHottestGood();

		    if (bestGood != "")
		    {
			    var bestGoodClass = GetAgentClassThatMakesMost(bestGood);
			    if (bestGoodClass != "")
			    {
				    bestClass = bestGoodClass;
			    }
		    }

		    var newAgent = getAgent(market.GetAgentClass(bestClass));
		    market.ReplaceAgent(agent, newAgent);
	    }


	    /**
	     * Find the agent class that produces the most of a given good
	     * @param	good
	     * @return
	     */
	    public string GetAgentClassThatMakesMost(string good)
	    {
            string res = "";
		    if (good == "food" )      {res = "farmer";      }
            else if (good == "wood")  { res = "woodcutter"; }
            else if (good == "ore")   { res = "miner"; }
            else if (good == "metal") {res = "refiner"; }
            else if (good == "tools") { res = "blacksmith"; }
            else if (good == "work") { res = "worker"; }
            return res;
	    }

        /// <summary>
        /// Get an agent from AgentData by matching the logic name to the logic associated with the agent.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private BasicAgent getAgent(AgentData data)
        {
            data.Logic = getLogic(data.logicName);
            return new DoranParberryAgent(0, data);
        }

        /// <summary>
        /// Get a Logic entity from the string that describes it.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private Logic getLogic(string str)
        {
            switch (str)
            {
                case "blacksmith": return new BlacksmithLogic();
                case "farmer": return new FarmerLogic();
                case "miner": return new MinerLogic();
                case "refiner": return new RefinerLogic();
                case "woodcutter": return new WoodcutterLogic();
                case "worker": return new WorkerLogic();
            }
            return null;
        }
    }

}
