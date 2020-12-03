using EconomySim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomySim
{
    /**
     * The most fundamental agent class, and has as little implementation as possible.
     * In most cases you should start by extending Agent instead of this.
     * @author larsiusprime
     */

    ///<summary>
    ///Fundamental agent class. Contains no logic and acts as a common container for Agent data.
    ///</summary>
    public class AgentData
    {
        /// <summary>
        /// The Class of the agent. E.g. Farmer, Refiner
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// The amount of money the agent has
        /// </summary>
        public double Money;
        /// <summary>
        /// The agent's inventory
        /// </summary>
        public InventoryData Inventory;
        /// <summary>
        /// The name of the logic used to run the agent. E.g. Farmer, Refiner
        /// </summary>
        public string logicName;
        /// <summary>
        /// The logic object that runs the agent
        /// </summary>
        public Logic Logic;
        /// <summary>
        /// The amount of look-back for the agent.
        /// </summary>
        public int? LookBack;

        /// <summary>
        /// Creates a new agent data.
        /// </summary>
        /// <param name="className">The name of the class of agent. Usually the same as Logic Name.</param>
        /// <param name="money">The amount of money the agent starts out with.</param>
        /// <param name="logicName">The name of the logic used by the agent. Usually the same as Class Name.</param>
        public AgentData(string className, double money, string logicName)
        {
            this.ClassName = className;
            this.Money = money;
            this.logicName = logicName;
        }
    }
}
