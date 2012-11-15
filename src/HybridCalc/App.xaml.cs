using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using System.Windows;
using Microsoft.ComponentModel.Composition.Scripting;

namespace HybridCalc
{
    public partial class App : Application
    {
        private System.ComponentModel.Composition.Hosting.CompositionContainer _container;

        [Import("MainWindow")]
        public new Window MainWindow
        {
            get { return base.MainWindow; }
            set { base.MainWindow = value; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (Compose())
            {
                MainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (_container != null)
            {
                //_container.Dispose();
            }
        }

        private bool Compose()
        {
            var catalog = new System.ComponentModel.Composition.Hosting.AggregateCatalog();
//            var catalog = new AggregatingComposablePartCatalog();
            catalog.Catalogs.Add(
                new RubyCatalog(new RubyPartFile("calculator_ops.rb")));
            catalog.Catalogs.Add(
                new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            _container = new System.ComponentModel.Composition.Hosting.CompositionContainer(catalog);
            //_container. AddPart(this);
            var batch = new System.ComponentModel.Composition.Hosting.CompositionBatch();
            batch.AddPart(this);
            //_container.AddPart(this);
            //_container.Compose(this);

            try
            {
                _container.Compose(batch);
            }
            catch (CompositionException compositionException)
            {
                MessageBox.Show(compositionException.ToString());
                return false;
            }
            return true;
        }
    }
}
