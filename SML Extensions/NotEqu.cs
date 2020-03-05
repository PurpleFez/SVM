namespace SML_Extensions
{
    #region Using directives
    using System;
    using SVM.VirtualMachine;
    #endregion

    public class NotEqu : BaseInstructionWithOperand
    {
        public override void Run()
        {
            if (VirtualMachine.Stack.Count < 2)
            {
                throw new SvmRuntimeException(String.Format(BaseInstruction.StackUnderflowMessage,
                                                this.ToString(), VirtualMachine.ProgramCounter));
            }
            if (this.Operands[0].GetType() != typeof(string))
            {
                throw new SvmRuntimeException(String.Format(BaseInstruction.OperandOfWrongTypeMessage,
                                                this.ToString(), VirtualMachine.ProgramCounter));
            }
            string location = this.Operands[0];

            try
            {
                object op1 = this.VirtualMachine.Stack.Pop();
                object op2 = this.VirtualMachine.Stack.Pop();
                if (!op1.Equals(op2))
                {
                    this.VirtualMachine.Branch(location);
                }
                this.VirtualMachine.Stack.Push(op2);
                this.VirtualMachine.Stack.Push(op1);
            } catch (InvalidCastException e)
            {
                throw new SvmRuntimeException(String.Format(BaseInstructionWithOperand.OperandOfWrongTypeMessage,
                                                this.ToString(), VirtualMachine.ProgramCounter),
                                                e);

            }
        }
    }
}