using System;
using System.Collections.Generic;
using SVM.VirtualMachine;
using SVM.VirtualMachine.Debug;

namespace Debuggers
{
    public class DebugFrame : IDebugFrame
    {
        private IInstruction currentInstruction;
        private List<IInstruction> codeFrame;

        public DebugFrame(IInstruction currentInstruction, List<IInstruction> codeFrame)
        {
            this.currentInstruction = currentInstruction;
            this.codeFrame = codeFrame;
        }

        public IInstruction CurrentInstruction
        {
            get
            {
                return this.currentInstruction;
            }
        }

        public List<IInstruction> CodeFrame
        {
            get
            {
                return this.codeFrame;
            }
        }
    }
}