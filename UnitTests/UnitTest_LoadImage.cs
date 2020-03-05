using SVM;
using SVM.VirtualMachine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SML_Extensions
{
    [TestClass]
    public class UnitTest_LoadImage
    {

        [TestMethod]
        public void LoadImage_ImageExists()
        {
            LoadImage loadImage = new LoadImage() {
                VirtualMachine = new SvmVirtualMachine(),
                Operands = new string[] { "..\\..\\images\\Tux.png" }
            };

            loadImage.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void LoadImage_InvalidImage()
        {
            LoadImage loadImage = new LoadImage() {
                VirtualMachine = new SvmVirtualMachine(),
                Operands = new string[] { "..\\..\\images\\Fake.png" }
            };

            loadImage.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void LoadImage_ImageNotExist()
        {
            LoadImage loadImage = new LoadImage() {
                VirtualMachine = new SvmVirtualMachine(),
                Operands = new string[] { "image_that_does_not_exist.png" }
            };

            loadImage.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void LoadImage_NoOperand()
        {
            LoadImage loadImage = new LoadImage() {
                VirtualMachine = new SvmVirtualMachine(),
                Operands = new string[] { }
            };

            loadImage.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmCompilationException))]
        public void LoadImage_NullOperand()
        {
            LoadImage loadImage = new LoadImage() {
                VirtualMachine = new SvmVirtualMachine(),
                Operands = null
            };

            loadImage.Run();
        }
    }
}