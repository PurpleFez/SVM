namespace SML_Extensions
{
    #region Using directives
    using System;
    using SVM.VirtualMachine;
    #endregion

    public class BGrInt : BaseInstructionWithOperand
    {
        public override void Run()
        {
            if (VirtualMachine.Stack.Count < 1)
            {
                throw new SvmRuntimeException(String.Format(BaseInstruction.StackUnderflowMessage,
                                                this.ToString(), VirtualMachine.ProgramCounter));
            }
            if (!Int32.TryParse(this.Operands[0].ToString(), out int opValue))
            {
                throw new SvmRuntimeException(String.Format(BaseInstruction.OperandOfWrongTypeMessage,
                                                this.ToString(), VirtualMachine.ProgramCounter));
            }
            if (this.Operands[1].GetType() != typeof(string))
            {
                throw new SvmRuntimeException(String.Format(BaseInstruction.OperandOfWrongTypeMessage,
                                                this.ToString(), VirtualMachine.ProgramCounter));
            }
            try
            {
                string location = this.Operands[1];
                int comparator = (int)this.VirtualMachine.Stack.Peek();
                if (opValue > comparator)
                {
                    this.VirtualMachine.Branch(location);
                }
            } catch (InvalidCastException e)
            {
                throw new SvmRuntimeException(String.Format(BaseInstruction.OperandOfWrongTypeMessage,
                                this.ToString(), VirtualMachine.ProgramCounter),
                                e);
            }
        }
    }
}
