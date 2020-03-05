namespace SML_Extensions
{
    #region Using directives
    using System;
    using System.Drawing;
    using System.IO;
    using SVM.VirtualMachine;
    #endregion
    public class LoadImage : BaseInstructionWithOperand
    {
        #region Constants
        private const string InvalidImageFormatMessage = "The file does not have a valid image format.";
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
            object opValue = this.Operands[0];
            if (this.Operands[0].GetType() != typeof(string)) {
                throw new SvmRuntimeException(String.Format(BaseInstruction.OperandOfWrongTypeMessage,
                                                this.ToString(), this.VirtualMachine.ProgramCounter));
            }
            string filename = opValue.ToString();
            if (filename == null) {
                throw new SvmRuntimeException(string.Format(BaseInstructionWithOperand.InvalidOperandMessage,
                                                this.ToString(), this.VirtualMachine.ProgramCounter));
            }
            if (!File.Exists(filename)) {
                throw new SvmRuntimeException(string.Format(BaseInstructionWithOperand.InvalidOperandMessage,
                                                this.ToString(), this.VirtualMachine.ProgramCounter),
                                                new FileNotFoundException());
            }

            try
            {
                Image image = Image.FromFile(filename);
                this.VirtualMachine.Stack.Push(image);
            } catch (OutOfMemoryException e)
            {
                throw new SvmRuntimeException(string.Format(InvalidImageFormatMessage,
                                                this.ToString(), this.VirtualMachine.ProgramCounter),
                                                e);
            }
        }
        #endregion
    }
}