using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SVM.SimpleMachineLanguage;
using SVM.VirtualMachine;

namespace SVM
{
    [TestClass]
    public class UnitTest_Remove
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
        public void Remove_String()
        {
            Remove remove = new Remove() {
                VirtualMachine = this.vm.Object
            };

            remove.VirtualMachine.Stack.Push("Hello, world!");
            remove.Run();
        }

        [TestMethod]
        public void Remove_Int()
        {
            Remove remove = new Remove() {
                VirtualMachine = this.vm.Object
            };

            remove.VirtualMachine.Stack.Push(0);
            remove.Run();
        }

        [TestMethod]
        public void Remove_Object()
        {
            Remove remove = new Remove() {
                VirtualMachine = this.vm.Object
            };

            remove.VirtualMachine.Stack.Push(new object());
            remove.Run();
        }

        [TestMethod]
        public void Remove_Null()
        {
            Remove remove = new Remove() {
                VirtualMachine = this.vm.Object
            };

            remove.VirtualMachine.Stack.Push(null);
            remove.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void Remove_Underflow()
        {
            Remove remove = new Remove() {
                VirtualMachine = this.vm.Object
            };

            remove.Run();
        }
    }
}