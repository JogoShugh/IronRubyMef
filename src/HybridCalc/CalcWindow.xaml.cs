using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using HybridCalc.Contracts;

namespace HybridCalc
{
    [Export("MainWindow")]
    [Export(typeof(IErrorLog))]
    public partial class CalcWindow : Window, IErrorLog
    {
        [Import]
        public IList<IOperation> Operations { get; private set; }

        public CalcWindow()
        {
            InitializeComponent();
            Operations = new List<IOperation>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var oper in Operations)
                op.Items.Add(oper.Symbol);

            op.SelectedIndex = 0;

            Calculate();
        }

        private void calculate_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void Calculate()
        {
            errorMessage.Text = "";

            result.Text = Operations[op.SelectedIndex]
                .Apply(Double.Parse(a.Text), Double.Parse(b.Text))
                .ToString();
        }

        public void AddMessage(string message)
        {
            errorMessage.Text = message;
        }
    }
}
