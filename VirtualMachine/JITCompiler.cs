namespace SVM.VirtualMachine
{
    #region Using directives
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    #endregion
    /// <summary>
    /// Utility class which generates compiles a textual representation
    /// of an SML instruction into an executable instruction instance
    /// </summary>
    internal static class JITCompiler
    {
        #region Constants
        private const string InvalidInstructionMessage = "An invalid SML instruction has been found in the SML source";
        private const string DllSearchPattern = "*.dll";
        private const string SML_EXTENSION_KEY = "24c35dca537373a7";
        private const string MultipleInstructionsMessage = "More than one implementation of the same SML instruction.";
        #endregion

        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Public methods
        [System.Runtime.InteropServices.DllImport("mscoree.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        static extern bool StrongNameSignatureVerificationEx(string wszFilePath, bool fForceVerification, ref bool pfWasVerified);
        #endregion

        #region Non-public methods
        internal static IInstruction CompileInstruction(string opcode)
        {
            IInstruction instruction = null;

            #region TASK 6
            Assembly self = Assembly.GetCallingAssembly();
            List<Type> all_types = new List<Type>();
            // try every dll file in the working directory
            string[] files = Directory.GetFiles(Environment.CurrentDirectory, DllSearchPattern);
            foreach (string file in files)
            {
                try
                {
                    // check if library was signed
                    bool notForced = false;
                    if (!StrongNameSignatureVerificationEx(file, false, ref notForced))
                    {
                        continue;
                    }
                    // attempt to load - will throw BadImageFormatException if not valid assembly
                    Assembly library = Assembly.LoadFile(file);
                    // get the library public key token and compare with the current assembly public key
                    byte[] libToken = library.GetName().GetPublicKeyToken();
                    byte[] selfToken = self.GetName().GetPublicKeyToken();
                    if (!libToken.SequenceEqual(selfToken))
                    {
                        continue;
                    }
                    // add all IInstructionWithOperand types to the list
                    all_types.AddRange(
                        from t in library.GetTypes()
                        where t.GetInterfaces().Contains(typeof(IInstruction))
                        select t
                    );
                } catch (BadImageFormatException)
                {

                }
            }
            #endregion
            #region TASK 1 - TO BE IMPLEMENTED BY THE STUDENT
            // fetch a list of all the types and match the opcode to the class
            all_types.AddRange(self.GetTypes());
            IEnumerable<Type> types = (
                from t in all_types
                // case insensitivity and IInstruction implementation
                where t.Name.ToLower() == opcode.ToLower() && t.GetInterfaces().Contains(typeof(IInstruction))
                select t
            );

            int results = types.Count();
            if (results < 1) {
                throw new SvmCompilationException(InvalidInstructionMessage);
            } else if (results > 1) {
                throw new SvmCompilationException(MultipleInstructionsMessage);
            }

            // extract the instance class and dynamically instantiate it
            Type type = types.First();
            object o = Activator.CreateInstance(type);
            instruction = (IInstruction)o;
            instruction.VirtualMachine = new SvmVirtualMachine();
            #endregion

            return instruction;
        }

        internal static IInstruction CompileInstruction(string opcode, params string[] operands)
        {
            IInstructionWithOperand instruction = null;

            #region TASK 6
            Assembly self = Assembly.GetCallingAssembly();
            List<Type> all_types = new List<Type>();
            // try every dll file in the working directory
            string[] files = Directory.GetFiles(Environment.CurrentDirectory, DllSearchPattern);
            foreach (string file in files)
            {
                try
                {
                    // check if library was signed
                    bool notForced = false;
                    if (!StrongNameSignatureVerificationEx(file, false, ref notForced))
                    {
                        continue;
                    }
                    // attempt to load - will throw BadImageFormatException if not valid assembly
                    Assembly library = Assembly.LoadFile(file);
                    // get the library public key token and compare with the current assembly public key
                    byte[] libToken = library.GetName().GetPublicKeyToken();
                    byte[] selfToken = self.GetName().GetPublicKeyToken();
                    if (!libToken.SequenceEqual(selfToken))
                    {
                        continue;
                    }
                    // add all IInstructionWithOperand types to the list
                    all_types.AddRange(
                        from t in library.GetTypes()
                        where t.GetInterfaces().Contains(typeof(IInstructionWithOperand))
                        select t
                    );
                } catch (BadImageFormatException)
                {
                    
                }
            }
            #endregion
            #region TASK 1 - TO BE IMPLEMENTED BY THE STUDENT
            // fetch a list of all the types and match the opcode to the class
            //Assembly assembly = Assembly.GetCallingAssembly();
            all_types.AddRange(self.GetTypes());
            IEnumerable<Type> types = (
                from t in all_types
                // case insensitivity and IInstruction implementation
                where t.Name.ToLower() == opcode.ToLower() && t.GetInterfaces().Contains(typeof(IInstructionWithOperand))
                select t
            );

            int results = types.Count();
            if (results < 1) {
                throw new SvmCompilationException(InvalidInstructionMessage);
            } else if (results > 1) {
                throw new SvmCompilationException(MultipleInstructionsMessage);
            }

            // extract the instance class and dynamically instantiate it
            Type type = types.First();
            object o = Activator.CreateInstance(type);
            instruction = (IInstructionWithOperand)o;
            instruction.Operands = operands;
            #endregion

            return instruction;
        }
        #endregion

    }
}
