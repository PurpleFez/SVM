using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM;
using SVM.VirtualMachine;

namespace SML_Extensions
{
    [TestClass]
    public class UnitTest_EquInt
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
        public void EquInt_StackItemEQ()
        {
            EquInt equInt = new EquInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            equInt.VirtualMachine.Stack.Push(0);
            equInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Once());
        }

        [TestMethod]
        public void EquInt_StackItemNEQ()
        {
            EquInt equInt = new EquInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            equInt.VirtualMachine.Stack.Push(1);
            equInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void EquInt_OperandAndStackNull()
        {
            EquInt equInt = new EquInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { null, "jump_location" }
            };

            equInt.VirtualMachine.Stack.Push(null);
            equInt.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void EquInt_OperandNull()
        {
            EquInt equInt = new EquInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { null, "jump_location" }
            };

            equInt.VirtualMachine.Stack.Push(1);
            equInt.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void EquInt_StackItemNull()
        {
            EquInt equInt = new EquInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            equInt.VirtualMachine.Stack.Push(null);
            equInt.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void EquInt_Underflow()
        {
            EquInt equInt = new EquInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            equInt.Run();
        }
    }
}
