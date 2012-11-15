using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;

namespace Microsoft.ComponentModel.Composition.Scripting
{
    public class RubyPartFile : RubyPartSource
    {
        string _scriptPath;

        public RubyPartFile(string scriptPath)
        {
            _scriptPath = scriptPath;
        }

        public override void Execute(ScriptEngine scriptEngine)
        {
            if (scriptEngine == null)
                throw new ArgumentNullException("scriptEngine");

            scriptEngine.ExecuteFile(_scriptPath);
        }
    }
}
