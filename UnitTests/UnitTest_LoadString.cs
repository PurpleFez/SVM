using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM.SimpleMachineLanguage;
using SVM.VirtualMachine;

namespace SVM
{
    [TestClass]
    public class UnitTest_LoadString
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
        public void LoadString_HelloWorld()
        {
            LoadString loadstring = new LoadString() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "Hello, world!" }
            };

            loadstring.Run();

            string result = (string)loadstring.VirtualMachine.Stack.Pop();
            Assert.AreEqual("Hello, world!", result);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void LoadString_NoOperand()
        {
            LoadString loadstring = new LoadString() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { }
            };

            loadstring.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmCompilationException))]
        public void LoadString_NullOperand()
        {
            LoadString loadstring = new LoadString() {
                VirtualMachine = this.vm.Object,
                Operands = null
            };

            loadstring.Run();
        }
    }
}