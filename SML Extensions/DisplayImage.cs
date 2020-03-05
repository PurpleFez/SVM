namespace SML_Extensions
{
    #region Using directives
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using SVM.VirtualMachine;
    #endregion
    public class DisplayImage : BaseInstruction
    {
        #region Constants
        private const string InvalidImageFormatMessage = "The file does not have a valid image format.";
        private const string FileNotExistMessage = "The file does not exist.";
        private const string NullImageMessage = "The image is null.";
        #endregion

        #region System.Object overrides
        /// <summary>
        /// Determines whether the specified <see cref="System.Object">Object</see> is equal to the current <see cref="System.Object">Object</see>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object">Object</see> to compare with the current <see cref="System.Object">Object</see>.</param>
        /// <returns><b>true</b> if the specified <see cref="System.Object">Object</see> is equal to the current <see cref="System.Object">Object</see>; otherwise, <b>false</b>.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for this type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="System.Object">Object</see>.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String">String</see> that represents the current <see cref="System.Object">Object</see>.
        /// </summary>
        /// <returns>A <see cref="System.String">String</see> that represents the current <see cref="System.Object">Object</see>.</returns>
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

        #region IInstruction Members
        public override void Run()
        {
            // checks for empty stack
            if (this.VirtualMachine.Stack.Count < 1)
            {
                throw new SvmRuntimeException(String.Format(BaseInstruction.StackUnderflowMessage,
                                                this.ToString(), this.VirtualMachine.ProgramCounter));
            }

            object o = this.VirtualMachine.Stack.Pop();
            if (o is null)
            {
                throw new SvmRuntimeException(String.Format(NullImageMessage,
                                this.ToString(), this.VirtualMachine.ProgramCounter));
            }

            try
            {
                Image image = (Image)o;
            } catch (InvalidCastException e)
            {
                throw new SvmRuntimeException(String.Format(BaseInstruction.OperandOfWrongTypeMessage,
                                                this.ToString(), this.VirtualMachine.ProgramCounter),
                                                e);
            }

            Thread thread = new Thread(this.MakeForm);
            thread.Start(o);
        }

        private void MakeForm(object o)
        {
            Image image = (Image)o;
            Form image_form = new Form {
                Size = new Size(image.Size.Width + 40, image.Size.Height + 40)
            };
            PictureBox pictureBox = new PictureBox {
                Location = new Point(0, 0),
                Image = image,
                Size = image.Size
            };
            image_form.Controls.Add(pictureBox);
            image_form.ShowDialog();
        }
        #endregion
    }
}