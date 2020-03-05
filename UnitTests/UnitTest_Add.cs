using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM.SimpleMachineLanguage;
using SVM.VirtualMachine;

namespace SVM
{
    [TestClass]
    public class UnitTest_Add
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
        public void Add_1_2()
        {
            Add add = new Add() {
                VirtualMachine = this.vm.Object
            };

            add.VirtualMachine.Stack.Push(1);
            add.VirtualMachine.Stack.Push(2);
            add.Run();

            int result = (int)add.VirtualMachine.Stack.Pop();
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Add_IntMax()
        {
            Add add = new Add() {
                VirtualMachine = this.vm.Object
            };

            add.VirtualMachine.Stack.Push(int.MaxValue);
            add.VirtualMachine.Stack.Push(int.MaxValue);
            add.Run();

            long result = (int)add.VirtualMachine.Stack.Pop();
            Assert.AreEqual((long)int.MaxValue * 2, result);
        }

        [TestMethod]
        public void Add_IntMin()
        {
            Add add = new Add() {
                VirtualMachine = this.vm.Object
            };

            add.VirtualMachine.Stack.Push(int.MinValue);
            add.VirtualMachine.Stack.Push(int.MinValue);
            add.Run();

            long result = (int)add.VirtualMachine.Stack.Pop();
            Assert.AreEqual((long)int.MinValue * 2, result);
        }

        [TestMethod]
        public void AddRandom()
        {
            Add add = new Add() {
                VirtualMachine = this.vm.Object
            };

            const int SEED = 0;
            Random random = new Random(SEED);
            int a = random.Next();
            int b = random.Next();

            add.VirtualMachine.Stack.Push(a);
            add.VirtualMachine.Stack.Push(b);
            add.Run();

            int result = (int)add.VirtualMachine.Stack.Pop();
            Assert.AreEqual(a + b, result);
        }
        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void AddUnderflowOneOperand()
        {
            Add add = new Add() {
                VirtualMachine = this.vm.Object
            };

            add.VirtualMachine.Stack.Push(0);

            add.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void AddUnderflowNoOperand()
        {
            Add add = new Add() {
                VirtualMachine = this.vm.Object
            };

            add.Run();
        }
    }
}