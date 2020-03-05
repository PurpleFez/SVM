using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM.SimpleMachineLanguage;
using SVM.VirtualMachine;

namespace SVM
{
    [TestClass]
    public class UnitTest_WriteString
    {
        Mock<IVirtualMachine> vm;

        [TestInitialize]
        public void Init()
        {
            this.vm = new Mock<IVirtualMachine>();
            Stack stack = new Stack();
            this.vm.SetupGet(x => x.Stack).Returns(stack);
        }

        [TestMethod]
        public void WriteString_HelloWorld()
        {
            WriteString writestring = new WriteString() {
                VirtualMachine = this.vm.Object
            };

            writestring.VirtualMachine.Stack.Push("Hello, world!");
            writestring.Run();
        }

        [TestMethod]
        public void WriteString_Int()
        {
            WriteString writestring = new WriteString() {
                VirtualMachine = this.vm.Object
            };

            const int SEED = 0;
            Random random = new Random(SEED);
            writestring.VirtualMachine.Stack.Push(random.Next());
            writestring.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void WriteString_NullOperand()
        {
            WriteString writestring = new WriteString() {
                VirtualMachine = this.vm.Object
            };

            writestring.VirtualMachine.Stack.Push(null);
            writestring.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void WriteString_Underflow()
        {
            WriteString writestring = new WriteString() {
                VirtualMachine = this.vm.Object
            };

            writestring.Run();
        }
    }
}