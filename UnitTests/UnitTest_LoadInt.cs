using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM.SimpleMachineLanguage;
using SVM.VirtualMachine;

namespace SVM
{
    [TestClass]
    public class UnitTest_LoadInt
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
        public void LoadInt_0()
        {
            LoadInt loadint = new LoadInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0" }
            };

            loadint.Run();

            int result = (int)loadint.VirtualMachine.Stack.Pop();
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void LoadInt_Max()
        {
            LoadInt loadint = new LoadInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { int.MaxValue.ToString() }
            };

            loadint.Run();

            int result = (int)loadint.VirtualMachine.Stack.Pop();
            Assert.AreEqual(int.MaxValue, result);
        }

        [TestMethod]
        public void LoadInt_Min()
        {
            LoadInt loadint = new LoadInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { int.MinValue.ToString() }
            };

            loadint.Run();

            int result = (int)loadint.VirtualMachine.Stack.Pop();
            Assert.AreEqual(int.MinValue, result);
        }

        [TestMethod]
        public void LoadIntRandom()
        {
            const int SEED = 0;
            Random random = new Random(SEED);
            int a = random.Next();

            LoadInt loadint = new LoadInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { a.ToString() }
            };

            loadint.Run();

            int result = (int)loadint.VirtualMachine.Stack.Pop();
            Assert.AreEqual(a, result);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void LoadInt_NoOperand()
        {
            LoadInt loadint = new LoadInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { }
            };

            loadint.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmCompilationException))]
        public void LoadInt_NullOperand()
        {
            LoadInt loadint = new LoadInt() {
                VirtualMachine = this.vm.Object,
                Operands = null
            };

            loadint.Run();
        }
    }
}