using System;
using System.ComponentModel.Composition.Primitives;
using IronRuby.Builtins;

namespace Microsoft.ComponentModel.Composition.Scripting
{
    public class RubyImportDefinition : ContractBasedImportDefinition
    {
        readonly Proc _mutator;

        public RubyImportDefinition(string contractName, Proc mutator)
            : base(contractName, null, null, ImportCardinality.ZeroOrOne, false, true, System.ComponentModel.Composition.CreationPolicy.Any)
        {
            if (mutator == null)
                throw new ArgumentNullException("mutator");
            _mutator = mutator;
        }

        public void SetImportedObject(object instance, object value)
        {
            // TODO?
            _mutator.Call(_mutator, instance, value);
        }
    }
}
