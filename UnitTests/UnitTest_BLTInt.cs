using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM;
using SVM.VirtualMachine;

namespace SML_Extensions
{
    [TestClass]
    public class UnitTest_BLTInt
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
        public void BLTInt_StackGT()
        {
            BLTInt bLTInt = new BLTInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            bLTInt.VirtualMachine.Stack.Push(1);
            bLTInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Once());
        }

        [TestMethod]
        public void BLTInt_StackLT()
        {
            BLTInt bLTInt = new BLTInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            bLTInt.VirtualMachine.Stack.Push(-1);
            bLTInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Never());
        }

        [TestMethod]
        public void BLTInt_OperandGT()
        {
            BLTInt bLTInt = new BLTInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "1", "jump_location" }
            };

            bLTInt.VirtualMachine.Stack.Push(0);
            bLTInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Never());
        }

        [TestMethod]
        public void BLTInt_OperandLT()
        {
            BLTInt bLTInt = new BLTInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "-1", "jump_location" }
            };

            bLTInt.VirtualMachine.Stack.Push(0);
            bLTInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Once());
        }

        [TestMethod]
        public void BLTInt_StackOperandEQ()
        {
            BLTInt bLTInt = new BLTInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            bLTInt.VirtualMachine.Stack.Push(0);
            bLTInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void BLTInt_StackNull()
        {
            BLTInt bLTInt = new BLTInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            bLTInt.VirtualMachine.Stack.Push(null);
            bLTInt.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void BLTInt_OperandNull()
        {
            BLTInt bLTInt = new BLTInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { null, "jump_location" }
            };

            bLTInt.VirtualMachine.Stack.Push(0);
            bLTInt.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void BLTInt_Underflow()
        {
            BLTInt bLTInt = new BLTInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            bLTInt.Run();
        }
    }
}
