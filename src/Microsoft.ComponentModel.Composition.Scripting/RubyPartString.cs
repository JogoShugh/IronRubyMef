using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;

namespace Microsoft.ComponentModel.Composition.Scripting
{
    public class RubyPartString : RubyPartSource
    {
        string _partDefinitions;

        public RubyPartString(string partDefinitions)
        {
            _partDefinitions = partDefinitions;
        }

        public override void Execute(ScriptEngine scriptEngine)
        {
            if (scriptEngine == null)
                throw new ArgumentNullException("scriptEngine");

            scriptEngine.Execute(_partDefinitions);
        }
    }
}
