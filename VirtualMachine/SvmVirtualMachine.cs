namespace SVM
{
    #region Using directives
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using SVM.VirtualMachine;
    using SVM.VirtualMachine.Debug;
    #endregion

    /// <summary>
    /// Implements the Simple Virtual Machine (SVM) virtual machine 
    /// </summary>
    public sealed class SvmVirtualMachine : IVirtualMachine
    {
        #region Constants
        private const string CompilationErrorMessage = "An SVM compilation error has occurred at line {0}.\r\n\r\n{1}";
        private const string RuntimeErrorMessage = "An SVM runtime error has occurred.\r\n\r\n{0}";
        private const string InvalidOperandsMessage = "The instruction \r\n\r\n\t{0}\r\n\r\nis invalid because there are too many operands. An instruction may have no more than one operand.";
        private const string InvalidLabelMessage = "Invalid label: the label {0} at line {1} is not associated with an instruction.";
        private const string ProgramCounterMessage = "Program counter violation; the program counter value is out of range";
        private const string DllSearchPattern = "*.dll";
        #endregion

        #region Fields
        private IDebugger debugger = null;
        private IDebugFrame debugFrame = null;
        private List<IInstruction> program = new List<IInstruction>();
        private readonly Stack stack = new Stack();
        private int programCounter = 0;
        private readonly List<int> breakpoint_lines = new List<int>();
        private readonly Hashtable labels = new Hashtable();
        #endregion

        #region Constructors
        // Do something here to find and create an instance of a type which implements 
        // the IDebugger interface, and assign it to the debugger field
        public SvmVirtualMachine()
        {
            #region Task 5 - Debugging
            // try every dll file in the working directory
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), DllSearchPattern);
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
                    byte[] selfToken = Assembly.GetCallingAssembly().GetName().GetPublicKeyToken();
                    if (!libToken.SequenceEqual(selfToken))
                    {
                        continue;
                    }
                    // retrieve any types that implement IDebugger
                    IEnumerable<Type> types =
                        from t in library.GetTypes()
                        where t.GetInterfaces().Contains(typeof(IDebugger))
                        select t
                    ;
                    // retrieve only if a single type
                    if (types.Count() == 1)
                    {
                        Type type = types.First();
                        // create an instance
                        object o = Activator.CreateInstance(type);
                        this.debugger = (IDebugger)o;
                        return;
                    }
                } catch (BadImageFormatException)
                {

                }
            }
            #endregion
        }
        #endregion

        #region Entry Point
        [STAThread]
        static void Main(string[] args)
        {
            if (CommandLineIsValid(args))
            {
                SvmVirtualMachine vm = new SvmVirtualMachine();
                try
                {
                    vm.Compile(args[0]);
                    vm.Run();
                } catch (SvmCompilationException)
                {
                } catch (SvmRuntimeException err)
                {
                    Console.WriteLine(RuntimeErrorMessage, err.Message);
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        ///  Gets a reference to the virtual machine stack.
        ///  This is used by executing instructions to retrieve
        ///  operands and store results
        /// </summary>
        public Stack Stack => this.stack;

        /// <summary>
        /// Accesses the virtual machine 
        /// program counter (see programCounter in the Fields region).
        /// This can be used by executing instructions to 
        /// determine their order (ie. line number) in the 
        /// sequence of executing SML instructions
        /// </summary>
        public int ProgramCounter => this.programCounter;
        #endregion

        #region Public Methods
        [System.Runtime.InteropServices.DllImport("mscoree.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        static extern bool StrongNameSignatureVerificationEx(string wszFilePath, bool fForceVerification, ref bool pfWasVerified);

        public void Branch(string branch_location)
        {
            this.programCounter = (int)this.labels[branch_location] - 1; // the -1 is added as the program counter will be incremented before the next execution
        }
        #endregion

        #region Non-public Methods
        /// <summary>
        /// Reads the specified file and tries to 
        /// compile any SML instructions it contains
        /// into an executable SVM program
        /// </summary>
        /// <param name="filepath">The path to the 
        /// .sml file containing the SML program to
        /// be compiled</param>
        /// <exception cref="SvmCompilationException">
        /// If file is not a valid SML program file or 
        /// the SML instructions cannot be compiled to an
        /// executable program</exception>
        private void Compile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new SvmCompilationException("The file " + filepath + " does not exist");
            }

            int lineNumber = 0;
            try
            {
                using (StreamReader sourceFile = new StreamReader(filepath))
                {
                    while (!sourceFile.EndOfStream)
                    {
                        string instruction = sourceFile.ReadLine();
                        if (!string.IsNullOrEmpty(instruction) &&
                            !string.IsNullOrWhiteSpace(instruction))
                        {
                            this.ParseInstruction(instruction, lineNumber);
                            lineNumber++;
                        }
                    }
                }
            } catch (SvmCompilationException err)
            {
                Console.WriteLine(CompilationErrorMessage, lineNumber, err.Message);
                throw;
            }
        }

        /// <summary>
        /// Executes a compiled SML program 
        /// </summary>
        /// <exception cref="SvmRuntimeException">
        /// If an unexpected error occurs during
        /// program execution
        /// </exception>
        private void Run()
        {
            DateTime start = DateTime.Now;
            Type type = null;

            #region TASK 2 - TO BE IMPLEMENTED BY THE STUDENT
            #region TASKS 5 & 7 - MAY REQUIRE MODIFICATION BY THE STUDENT
            // For task 5 (debugging), you should construct a IDebugFrame instance and
            // call the Break() method on the IDebugger instance stored in the debugger field
            if (this.breakpoint_lines.Count > 0)
            {
                // check working directory of currently running assembly, and return location of any dll files
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), DllSearchPattern);
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
                        byte[] selfToken = Assembly.GetCallingAssembly().GetName().GetPublicKeyToken();
                        if (!libToken.SequenceEqual(selfToken))
                        {
                            continue;
                        }
                        // check if it contains a type that implements IDebugFrame
                        IEnumerable<Type> types =
                            from t in library.GetTypes()
                            where t.GetInterfaces().Contains(typeof(IDebugFrame))
                            select t
                        ;
                        // dynamically instantiate type if it exists
                        if (types.Count() > 0)
                        {
                            type = types.First();
                            break;
                        }
                    } catch (BadImageFormatException)
                    {

                    }
                }
            }

            #endregion
            // iterates over all instructions and executes them
            for (this.programCounter = 0; this.programCounter < this.program.Count; this.programCounter++)
            {
                if (this.breakpoint_lines.Contains(this.programCounter))
                {
                    // current instruction
                    IInstruction ci = this.program[this.programCounter];
                    // get the start position of the frame
                    int index = (this.programCounter - 4) < 0 ? 0 : this.programCounter - 4;
                    // calculate how far onwards can be read from (to prevent out of bounds error)
                    int count_behind = this.programCounter - index;
                    int count_ahead = (this.programCounter + 4) >= this.program.Count ? this.program.Count - this.programCounter : 4;
                    // retrieve frame
                    List<IInstruction> cf = this.program.GetRange(index, count_ahead + count_behind);
                    // instantiate
                    object o = Activator.CreateInstance(type, new object[] { ci, cf });
                    this.debugFrame = (IDebugFrame)o;
                    this.debugger.VirtualMachine = this;
                    this.debugger.Break(this.debugFrame);
                }
                this.program[this.programCounter].VirtualMachine = this;
                this.program[this.programCounter].Run();
            }
            #endregion

            long memUsed = System.Environment.WorkingSet;
            TimeSpan elapsed = DateTime.Now - start;
            Console.WriteLine(string.Format(
                                        "\r\n\r\nExecution finished in {0} milliseconds. Memory used = {1} bytes",
                                        elapsed.Milliseconds,
                                        memUsed));
        }

        /// <summary>
        /// Parses a string from a .sml file containing a single
        /// SML instruction
        /// </summary>
        /// <param name="instruction">The string representation
        /// of an instruction</param>
        private void ParseInstruction(string instruction, int lineNumber)
        {
            #region TASK 5 & 7 - MAY REQUIRE MODIFICATION BY THE STUDENT
            // asterisk signifies breakpoint
            if (instruction[0] == '*')
            {
                this.breakpoint_lines.Add(lineNumber);
                instruction = instruction.Substring(2);
            }

            // check label as location
            // check label in conditional
            // it will either be last or first
            // only when it is at the start will it have the percent signs
            string[] label_split = instruction.Split('%');
            if (label_split.Length > 1 && label_split[0] == "")
            {
                this.labels.Add(label_split[1], lineNumber);
                if (label_split.Length > 3)
                {
                    instruction = label_split[2] + label_split[3];
                } else
                {
                    instruction = label_split[2];
                }
            }

            #endregion

            string[] tokens = null;
            if (instruction.Contains("\""))
            {
                tokens = instruction.Split(new char[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);

                // Remove any unnecessary whitespace
                for (int i = 0; i < tokens.Length; i++)
                {
                    tokens[i] = tokens[i].Trim();
                }
            } else
            {
                // Tokenize the instruction string by separating on spaces
                tokens = instruction.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }


            // Ensure the correct number of operands
            if (tokens.Length > 3)
            {
                throw new SvmCompilationException(string.Format(InvalidOperandsMessage, instruction));
            }

            switch (tokens.Length)
            {
                case 1:
                    this.program.Add(JITCompiler.CompileInstruction(tokens[0]));
                    break;
                case 2:
                    this.program.Add(JITCompiler.CompileInstruction(tokens[0], tokens[1].Trim('\"')));
                    break;
                case 3:
                    this.program.Add(JITCompiler.CompileInstruction(tokens[0], tokens[1].Trim('\"'), tokens[2].Trim('\"')));
                    break;
            }
        }


        #region Validate command line
        /// <summary>
        /// Verifies that a valid command line has been supplied
        /// by the user
        /// </summary>
        private static bool CommandLineIsValid(string[] args)
        {
            bool valid = true;

            if (args.Length != 1)
            {
                DisplayUsageMessage("Wrong number of command line arguments");
                valid = false;
            }

            if (valid && !args[0].EndsWith(".sml", StringComparison.CurrentCultureIgnoreCase))
            {
                DisplayUsageMessage("SML programs must be in a file named with a .sml extension");
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Displays comamnd line usage information for the
        /// SVM virtual machine 
        /// </summary>
        /// <param name="message">A custom message to display
        /// to the user</param>
        static void DisplayUsageMessage(string message)
        {
            Console.WriteLine("The command line arguments are not valid. {0} \r\n", message);
            Console.WriteLine("USAGE:");
            Console.WriteLine("svm program_name.sml");
        }
        #endregion
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

    }
}
