using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HybridCalc.Contracts
{
    public interface IErrorLog
    {
        void AddMessage(string message);
    }
}
