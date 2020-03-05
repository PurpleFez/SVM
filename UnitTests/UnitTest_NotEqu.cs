using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM;
using SVM.VirtualMachine;

namespace SML_Extensions
{
    [TestClass]
    public class UnitTest_NotEqu
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
        public void NotEqu_StackItemsOneNull()
        {
            NotEqu notEqu = new NotEqu() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "jump_location" }
            };

            notEqu.VirtualMachine.Stack.Push(null);
            notEqu.VirtualMachine.Stack.Push(1);
            notEqu.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void NotEqu_StackItemsNulls()
        {
            NotEqu notEqu = new NotEqu() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "jump_location" }
            };

            notEqu.VirtualMachine.Stack.Push(null);
            notEqu.VirtualMachine.Stack.Push(null);
            notEqu.Run();
        }

        [TestMethod]
        public void NotEqu_StackItemsEQ()
        {
            NotEqu notEqu = new NotEqu() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "jump_location" }
            };

            notEqu.VirtualMachine.Stack.Push(0);
            notEqu.VirtualMachine.Stack.Push(0);
            notEqu.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Never());
        }

        [TestMethod]
        public void NotEqu_StackItemsNEQ()
        {
            NotEqu notEqu = new NotEqu() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "jump_location" }
            };

            notEqu.VirtualMachine.Stack.Push(0);
            notEqu.VirtualMachine.Stack.Push(1);
            notEqu.Run();

            this.vm.Verify(x => x.Branch("jump_location"), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void NotEqu_Underflow()
        {
            NotEqu notEqu = new NotEqu() {
                VirtualMachine = this.vm.Object,
                Operands = new string[] { "jump_location" }
            };

            notEqu.Run();
        }
    }
}
