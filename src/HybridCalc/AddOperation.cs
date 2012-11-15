using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using HybridCalc.Contracts;

namespace HybridCalc
{
    [Export(typeof(IOperation))]
    public class AddOperation : IOperation
    {
        public string Symbol { get { return "+"; } }

        public double Apply(double a, double b) { return a + b; }
    }
}
