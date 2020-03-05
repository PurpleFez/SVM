using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM.SimpleMachineLanguage;
using SVM.VirtualMachine;

namespace SVM
{
    [TestClass]
    public class UnitTest_Decr
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
        public void DecrZero()
        {
            Decr decr = new Decr() {
                VirtualMachine = this.vm.Object
            };

            decr.VirtualMachine.Stack.Push(0);
            decr.Run();

            int result = (int)decr.VirtualMachine.Stack.Pop();
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void DecrRandom()
        {
            Decr decr = new Decr() {
                VirtualMachine = this.vm.Object
            };

            const int SEED = 0;
            Random random = new Random(SEED);
            int a = random.Next();

            decr.VirtualMachine.Stack.Push(a);
            decr.Run();

            int result = (int)decr.VirtualMachine.Stack.Pop();
            Assert.AreEqual(a - 1, result);
        }

        [TestMethod]
        public void DecrIntMin()
        {
            Decr decr = new Decr() {
                VirtualMachine = this.vm.Object
            };

            decr.VirtualMachine.Stack.Push(int.MinValue);
            decr.Run();

            int result = (int)decr.VirtualMachine.Stack.Pop();
            Assert.AreEqual((long)int.MinValue - 1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void DecrUnderflowCaught()
        {
            Decr decr = new Decr() {
                VirtualMachine = this.vm.Object
            };

            decr.Run();
        }
    }
}