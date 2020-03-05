using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM.SimpleMachineLanguage;
using SVM.VirtualMachine;

namespace SVM
{
    [TestClass]
    public class UnitTest_Subtract
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
        public void Subtract_1_2()
        {
            Subtract subtract = new Subtract() {
                VirtualMachine = this.vm.Object
            };

            subtract.VirtualMachine.Stack.Push(1);
            subtract.VirtualMachine.Stack.Push(2);
            subtract.Run();

            int result = (int)subtract.VirtualMachine.Stack.Pop();
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void Subtract_IntMaxFromItself()
        {
            Subtract subtract = new Subtract() {
                VirtualMachine = this.vm.Object
            };

            subtract.VirtualMachine.Stack.Push(int.MaxValue);
            subtract.VirtualMachine.Stack.Push(int.MaxValue);
            subtract.Run();

            long result = (int)subtract.VirtualMachine.Stack.Pop();
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Subtract_IntMinFromItself()
        {
            Subtract subtract = new Subtract() {
                VirtualMachine = this.vm.Object
            };

            subtract.VirtualMachine.Stack.Push(int.MinValue);
            subtract.VirtualMachine.Stack.Push(int.MinValue);
            subtract.Run();

            long result = (int)subtract.VirtualMachine.Stack.Pop();
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Subtract_IntMinFromIntMax()
        {
            Subtract subtract = new Subtract() {
                VirtualMachine = this.vm.Object
            };

            subtract.VirtualMachine.Stack.Push(int.MinValue);
            subtract.VirtualMachine.Stack.Push(int.MaxValue);
            subtract.Run();

            long result = (int)subtract.VirtualMachine.Stack.Pop();
            Assert.AreEqual((long)int.MaxValue - (long)int.MinValue, result);
        }

        [TestMethod]
        public void Subtract_IntMaxFromIntMin()
        {
            Subtract subtract = new Subtract() {
                VirtualMachine = this.vm.Object
            };

            subtract.VirtualMachine.Stack.Push(int.MaxValue);
            subtract.VirtualMachine.Stack.Push(int.MinValue);
            subtract.Run();

            long result = (int)subtract.VirtualMachine.Stack.Pop();
            Assert.AreEqual((long)int.MinValue - (long)int.MaxValue, result);
        }

        [TestMethod]
        public void SubtractRandom()
        {
            Subtract subtract = new Subtract() {
                VirtualMachine = this.vm.Object
            };

            const int SEED = 0;
            Random random = new Random(SEED);
            int a = random.Next();
            int b = random.Next();

            subtract.VirtualMachine.Stack.Push(a);
            subtract.VirtualMachine.Stack.Push(b);
            subtract.Run();

            int result = (int)subtract.VirtualMachine.Stack.Pop();
            Assert.AreEqual(a - b, result);
        }
        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void SubtractUnderflowOneOperand()
        {
            Subtract subtract = new Subtract() {
                VirtualMachine = this.vm.Object
            };

            subtract.VirtualMachine.Stack.Push(0);
            subtract.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void SubtractUnderflowNoOperand()
        {
            Subtract subtract = new Subtract() {
                VirtualMachine = this.vm.Object
            };

            subtract.Run();
        }
    }
}