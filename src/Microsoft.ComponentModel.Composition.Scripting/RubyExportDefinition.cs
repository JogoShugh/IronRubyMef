using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using IronRuby.Builtins;

namespace Microsoft.ComponentModel.Composition.Scripting
{
    public class RubyExportDefinition : ExportDefinition
    {
        readonly Proc _accessor;

        public RubyExportDefinition(string contractName, IDictionary<string, object> metadata, Proc accessor)
            : base(contractName, metadata)
        {
            _accessor = accessor;
        }

        public object GetExportedObject(object rubyPartInstance)
        {
            // TODO?
            return _accessor.Call(_accessor, rubyPartInstance);
        }
    }
}
