using System;
using System.Collections;

namespace SVM
{
    public interface IVirtualMachine
    {
        Stack Stack { get; }
        int ProgramCounter { get; }

        void Branch(string branch_location);
    }
}
