using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HybridCalc.Contracts
{
    public interface IOperation
    {
        string Symbol { get; }
        double Apply(double a, double b);
    }
}
