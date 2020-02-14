using EconomySim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomySimLibrary.Bankrupt
{    public interface ISignalBankrupt
    {
        void SignalBankrupt(Market m, BasicAgent agent);
    }
}
