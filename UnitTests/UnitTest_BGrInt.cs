using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM;
using SVM.VirtualMachine;

namespace SML_Extensions
{
    [TestClass]
    public class UnitTest_BGrInt
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
        public void BGrInt_StackGT()
        {
            BGrInt bGrInt = new BGrInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            bGrInt.VirtualMachine.Stack.Push(1);
            bGrInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Never());
        }

        [TestMethod]
        public void BGrInt_StackLT()
        {
            BGrInt bGrInt = new BGrInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            bGrInt.VirtualMachine.Stack.Push(-1);
            bGrInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Once());
        }

        [TestMethod]
        public void BGrInt_OperandGT()
        {
            BGrInt bGrInt = new BGrInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "1", "jump_location" }
            };

            bGrInt.VirtualMachine.Stack.Push(0);
            bGrInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Once());
        }

        [TestMethod]
        public void BGrInt_OperandLT()
        {
            BGrInt bGrInt = new BGrInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "-1", "jump_location" }
            };

            bGrInt.VirtualMachine.Stack.Push(0);
            bGrInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Never());
        }

        [TestMethod]
        public void BGrInt_StackOperandEQ()
        {
            BGrInt bGrInt = new BGrInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            bGrInt.VirtualMachine.Stack.Push(0);
            bGrInt.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void BGrInt_StackNull()
        {
            BGrInt bGrInt = new BGrInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            bGrInt.VirtualMachine.Stack.Push(null);
            bGrInt.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void BGrInt_OperandNull()
        {
            BGrInt bGrInt = new BGrInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { null, "jump_location" }
            };

            bGrInt.VirtualMachine.Stack.Push(0);
            bGrInt.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void BGrInt_Underflow()
        {
            BGrInt bGrInt = new BGrInt() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "0", "jump_location" }
            };

            bGrInt.Run();
        }
    }
}
