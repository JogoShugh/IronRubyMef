using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using IronRuby.Builtins;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.ComponentModel.Composition.Scripting.Tests
{
    [TestClass]
    public class RubyCatalogTests
    {
        [TestMethod]
        public void CanConstructInstance()
        {
            var inst = new RubyCatalog(new RubyPartString(""));
        }

        [TestMethod]
        public void LoadsPartsFromScript()
        {
            var script = @"
                class MyPart < PartDefinition
                    export 'test_contract'
                end
            ";

            var inst = new RubyCatalog(new RubyPartString(script));
            Assert.AreEqual(1, inst.Parts.Count());
            Assert.AreEqual("MyPart", inst.Parts.Cast<RubyPartDefinition>().Single().DisplayName);
        }

        [TestMethod]
        [DeploymentItem("my_part.rb")]
        public void LoadsPartsFromFile()
        {
            var inst = new RubyCatalog(new RubyPartFile("my_part.rb"));
            Assert.AreEqual(1, inst.Parts.Count());
            Assert.AreEqual("MyPart", inst.Parts.Cast<RubyPartDefinition>().Single().DisplayName);
        }

        [TestMethod]
        public void ExportsSelf()
        {
            var script = @"
                class MyPart < PartDefinition
                    export 'test_contract'
                end
            ";

            var pd = CreateMyPart(script);
            var exports = pd.ExportDefinitions;

            Assert.AreEqual(1, exports.Count());
            var ed = exports.Single();
            Assert.AreEqual("test_contract", ed.ContractName);
            var p = pd.CreatePart();
            var eo = p.GetExportedValue(ed);
            var ro = (RubyObject)eo;
            Assert.AreEqual(ro.ImmediateClass.Name, "MyPart");
        }

        [TestMethod]
        public void ExportsMultipleContracts()
        {
            var script = @"
                class MyPart < PartDefinition
                    export 'test_contract'
                    export_attr 'test_contract2'
                    export_attr 'test_contract3'
                end
            ";

            var exports = CreateMyPart(script).ExportDefinitions;

            Assert.AreEqual(3, exports.Count());
        }

        [TestMethod]
        public void ExportsSimpleValue()
        {
            var script = @"
                class MyPart < PartDefinition
                    def initialize
                        @value = 42
                    end

                    export_attr 'simple_value' do
                        @value
                    end
                end
            ";

            var partDef = CreateMyPart(script);
            var part = partDef.CreatePart();
            var simpleValueDef = part.ExportDefinitions.Where(ed => ed.ContractName == "simple_value").Single();
            var exportedValue = part.GetExportedValue(simpleValueDef);
            Assert.AreEqual(42, exportedValue);
        }

        [TestMethod]
        public void ExportsMethod()
        {
            var script = @"
                class MyPart < PartDefinition
                    def initialize
                        @value = 42
                    end

                    export_method 'method' do |i|
                        @value + i
                    end
                end
            ";

            var partDef = CreateMyPart(script);
            var part = partDef.CreatePart();
            var methodDef = part.ExportDefinitions.Where(ed => ed.ContractName == "method").Single();
            var method = part.GetExportedValue(methodDef);
            Assert.IsInstanceOfType(method, typeof(Proc));
            var methodAsProc = (Proc)method;
            Assert.AreEqual(52, methodAsProc.Call(methodAsProc, 10));
        }

        [TestMethod]
        public void ImportsValues()
        {
            var script = @"
                class MyPart < PartDefinition
                    import 'input_value' do |input|
                        @input_value = input
                    end

                    export_attr 'output_value' do
                        @input_value.get_exported_object
                    end
                end
            ";

            ComposablePart part = CreateMyPart(script).CreatePart();
            Assert.AreEqual(1, part.ImportDefinitions.Count());

            var inputImport = part.ImportDefinitions.Single(
                i => (i).ContractName == "input_value");
            Assert.AreEqual(1, part.ExportDefinitions.Count());

            var outputExport = part.ExportDefinitions.Single();
            var testValue = "Hello, world";
            part.SetImport(inputImport, new[] { new Export(
                new ExportDefinition("input_value", null),
                () => testValue)});
            var output = part.GetExportedValue(outputExport);
            Assert.AreEqual(testValue, output);
        }

        private static RubyPartDefinition CreateMyPart(string script)
        {
            var inst = new RubyCatalog(new RubyPartString(script));
            return inst.Parts
                .Cast<RubyPartDefinition>()
                .Where(p => p.DisplayName == "MyPart")
                .Single();
        }

        [TestMethod]
        public void ExportingInterfaceTypeIncludesInterface()
        {
            var script = @"
                class MyPart < PartDefinition
                    export System::IDisposable

                    def dispose
                    end
                end
            ";

            var pd = CreateMyPart(script);
            var exports = pd.ExportDefinitions;
            Assert.AreEqual(1, exports.Count());
            
            var ed = exports.Single();
            Assert.AreEqual(
                AttributedModelServices.GetContractName(typeof(IDisposable)),
                ed.ContractName);
            
            var p = pd.CreatePart();
            var eo = p.GetExportedValue(ed);

            Assert.IsInstanceOfType(eo, typeof(IDisposable));
        }

        [TestMethod]
        public void ImportsIntoAttributes()
        {
            var script = @"
                class MyPart < PartDefinition
                    import 'input_value', :into => :@input_value

                    export_attr 'output_value' do
                        @input_value.get_exported_object
                    end
                end
            ";

            System.ComponentModel.Composition.Primitives.ComposablePart part = CreateMyPart(script).CreatePart();
            Assert.AreEqual(1, part.ImportDefinitions.Count());

            var inputImport = part.ImportDefinitions.Single(
                i => ((ContractBasedImportDefinition)i).ContractName == "input_value");
            Assert.AreEqual(1, part.ExportDefinitions.Count());

            var outputExport = part.ExportDefinitions.Single();
            var testValue = "Hello, world";
            part.SetImport(inputImport, new[] { new Export(
                new ExportDefinition("input_value", null),
                () => testValue)});
            var output = part.GetExportedValue(outputExport);
            Assert.AreEqual(testValue, output);
        }
    }
}
