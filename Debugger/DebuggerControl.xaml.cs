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
using SVM;
using SVM.VirtualMachine.Debug;

namespace Debuggers
{
    /// <summary>
    /// Interaction logic for DebuggerControl.xaml
    /// </summary>
    public partial class DebuggerWindow : Window
    {
        public DebuggerWindow(IDebugFrame debugFrame, IVirtualMachine virtualMachine)
        {
            this.InitializeComponent();
            this.listBox_Code.ItemsSource = debugFrame.CodeFrame;
            this.listBox_Code.SelectedItem = debugFrame.CurrentInstruction;
            this.listBox_Stack.ItemsSource = virtualMachine.Stack;
        }

        private void button_Continue_Click(object sender, RoutedEventArgs e)
        {
            string domain = AppDomain.CurrentDomain.FriendlyName;
            this.DialogResult = true;
            // resume execution
        }
    }
}
