using Microsoft.VisualStudio.TestTools.UnitTesting;
using SVM;
using SVM.VirtualMachine;
using System.Drawing;

namespace SML_Extensions
{
    [TestClass]
    public class UnitTest_DisplayImage
    {
        [TestMethod]
        public void DisplayImage_Image()
        {
            DisplayImage displayImage = new DisplayImage() {
                VirtualMachine = new SvmVirtualMachine()
            };

            Image image = Image.FromFile("..\\..\\images\\Tux.png");
            displayImage.VirtualMachine.Stack.Push(image);
            displayImage.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void DisplayImage_Null()
        {
            DisplayImage displayImage = new DisplayImage() {
                VirtualMachine = new SvmVirtualMachine()
            };

            displayImage.VirtualMachine.Stack.Push(null);
            displayImage.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void DisplayImage_InvalidImage()
        {
            DisplayImage displayImage = new DisplayImage() {
                VirtualMachine = new SvmVirtualMachine()
            };

            displayImage.VirtualMachine.Stack.Push(new object());
            displayImage.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void DisplayImage_ImageNotExist()
        {
            DisplayImage displayImage = new DisplayImage() {
                VirtualMachine = new SvmVirtualMachine()
            };

            displayImage.VirtualMachine.Stack.Push("image_that_does_not_exist.png");
            displayImage.Run();
        }

        [TestMethod]
        [ExpectedException(typeof(SvmRuntimeException))]
        public void DisplayImage_Underflow()
        {
            DisplayImage displayImage = new DisplayImage() {
                VirtualMachine = new SvmVirtualMachine()
            };

            displayImage.Run();
        }
    }
}