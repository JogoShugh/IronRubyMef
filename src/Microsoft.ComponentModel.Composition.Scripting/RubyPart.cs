using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using IronRuby.Builtins;
using Microsoft.ComponentModel.Composition.Scripting.Util;

namespace Microsoft.ComponentModel.Composition.Scripting
{
    public class RubyPart : ComposablePart
    {
        readonly RubyPartDefinition _definition;
        DelayedInit<object> _instance;

        public RubyPart(RubyPartDefinition definition, Proc createInstance)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");
            if (createInstance == null)
                throw new ArgumentNullException("createInstance");

            _definition = definition;
            _instance = new DelayedInit<object>(() => createInstance.Call(createInstance));
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get { return _definition.ExportDefinitions; }
        }

        //public override object GetExportedObject(ExportDefinition definition)
        //{
        //    if (definition == null)
        //        throw new ArgumentNullException("definition");

        //    var rdef = (RubyExportDefinition)definition;

        //    var instance = GetOrCreateInstance();

        //    return rdef.GetExportedObject(instance);
        //}

        private object GetOrCreateInstance()
        {
            return _instance.Value;
        }

        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get { return _definition.ImportDefinitions; }
        }

        public override void SetImport(ImportDefinition definition, IEnumerable<Export> exports)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            var rdef = (RubyImportDefinition)definition;

            var instance = GetOrCreateInstance();

            rdef.SetImportedObject(instance, exports.FirstOrDefault());
        }

        public override object GetExportedValue(ExportDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            var rdef = (RubyExportDefinition)definition;

            var instance = GetOrCreateInstance();

            return rdef.GetExportedObject(instance);
        }
    }
}
