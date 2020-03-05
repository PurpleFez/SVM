using System;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using SVM;
using SVM.VirtualMachine.Debug;

namespace Debuggers
{
    public class Debugger : IDebugger
    {
        #region TASK 5 - TO BE IMPLEMENTED BY THE STUDENT
        private static Window window;
        private static SvmVirtualMachine virtualMachine;
        public SvmVirtualMachine VirtualMachine
        {
            set
            {
                virtualMachine = value;
            }
        }

        public void Break(IDebugFrame debugFrame)
        {
            string domain = AppDomain.CurrentDomain.FriendlyName;
            AppDomain debugDomain = AppDomain.CreateDomain("DebuggerDomain");
            //Type type = typeof(DebuggerWindow);
            //debugDomain.CreateInstance("Debugger", type.FullName);
            //window = (Window)debugDomain.CreateInstanceAndUnwrap("Debugger", type.FullName, false, System.Reflection.BindingFlags.CreateInstance, null, new object[] { debugFrame, virtualMachine }, null, null);
            window = new DebuggerWindow(debugFrame, virtualMachine);
            window.ShowDialog();
        }
        #endregion
    }
}
