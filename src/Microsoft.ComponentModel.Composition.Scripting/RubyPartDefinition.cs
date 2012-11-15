using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

using IronRuby.Builtins;

namespace Microsoft.ComponentModel.Composition.Scripting
{
    public class RubyPartDefinition : ComposablePartDefinition
    {
        readonly Proc _createPart;
        readonly string _displayName;
        readonly IList<ExportDefinition> _exportDefinitions = new List<ExportDefinition>();
        readonly IList<ImportDefinition> _importDefinitions = new List<ImportDefinition>();

        public RubyPartDefinition(string displayName, Proc createPart)
        {
            if (createPart == null)
                throw new ArgumentNullException("createPart");

            if (displayName == null)
                throw new ArgumentNullException("displayName");

            _createPart = createPart;
            _displayName = displayName;
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public override ComposablePart CreatePart()
        {
            return (ComposablePart)_createPart.Call(_createPart);
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get { return _exportDefinitions; }
        }

        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get { return _importDefinitions; }
        }

        public void AddExportDefinition(RubyExportDefinition exportDefinition)
        {
            if (exportDefinition == null)
                throw new ArgumentNullException("exportDefinition");
            _exportDefinitions.Add(exportDefinition);
        }

        public void AddImportDefinition(RubyImportDefinition importDefinition)
        {
            if (importDefinition == null)
                throw new ArgumentNullException("importDefinition");
            _importDefinitions.Add(importDefinition);
        }
    }
}
