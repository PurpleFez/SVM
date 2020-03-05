using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM.SimpleMachineLanguage;
using SVM.VirtualMachine;

namespace SVM
{
    [TestClass]
    public class UnitTest_Incr
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
        public void IncrZero()
        {
            Incr incr = new Incr() {
                VirtualMachine = this.vm.Object
            };

            incr.VirtualMachine.Stack.Push(0);
            incr.Run();

            int result = (int)incr.VirtualMachine.Stack.Pop();
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void IncrRandom()
        {
            Incr incr = new Incr() {
                VirtualMachine = this.vm.Object
            };

            const int SEED = 0;
            Random random = new Random(SEED);
            int a = random.Next();

            incr.VirtualMachine.Stack.Push(a);
            incr.Run();

            int result = (int)incr.VirtualMachine.Stack.Pop();
            Assert.AreEqual(a + 1, result);
        }

        [TestMethod]
        public void IncrIntMax()
        {
            Incr incr = new Incr() {
                VirtualMachine = this.vm.Object
            };

            incr.VirtualMachine.Stack.Push(int.MaxValue);
            incr.Run();

            long result = (long)incr.VirtualMachine.Stack.Pop();
            Assert.AreEqual((long)int.MaxValue + 1, result);
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void IncrUnderflowCaught()
        {
            Incr incr = new Incr() {
                VirtualMachine = this.vm.Object
            };

            incr.Run();
        }
    }
}