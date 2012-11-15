using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;

namespace Microsoft.ComponentModel.Composition.Scripting
{
    public abstract class RubyPartSource
    {
        public abstract void Execute(ScriptEngine scriptEngine);
    }
}
