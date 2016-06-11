using System;
//
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.IO;
using System.Threading;
//
using System.Reflection;
using System.Reflection.Emit;
//
using System.Text.RegularExpressions;
//
using Const_LALR_Tables;
using AST;


namespace ConsoleFrontEnd
{
    public class Program
    {
        public const string nameCurrentDir = "IdOctRegExs";

        public string nameScaneLexParseDir = "ScaneLexParseDir";

        public string currDir;

        static string inputFile;

        static string outputFile; //for Parser only USING
        //
        static string valueASTFile;//after Parser if Root!=null
        //
        static Encoding inputFileEncoding;

        public StreamReader strReader;//for Scanner only USING

        public StreamWriter strWriter;//for Parser outputFile only; for Parser only USING
        //
        public const int len_tokenStringQueue = 2;
        public Queue<string> tokenStringQueue;
        //
        public const int len_terminalTokenStringQueue = 4;
        public Queue<string> terminalTokenStringQueue;
        //
        public AppDomain appDom;

        //
        public RegularExpList regularExpList;

        public string Main(string[] args)
        {
            ////Set Current Dir in UserProfile

            //string currDir = SetAppCurrentDir(nameCurrentDir);

            try
            {
                TestInputArgs(args);
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine(argEx.Message);
                //Console.WriteLine("\nPress any Key to Exit!\n");
                //Console.ReadKey();
               // return;
                //Directory.
                 appDom = AppDomain.CurrentDomain;

                Console.WriteLine("\nappDom.BaseDirectory: <{0}>\nappDom.FriendlyName: <{1}>\n",
                    appDom.BaseDirectory, appDom.FriendlyName);

                Console.WriteLine("\nPress any Key to Exit!\n");
                //Console.ReadKey();
                return argEx.Message;

            }
            ////Console.WriteLine("\nPress any Key to Exit!\n");
            ////Console.ReadKey();
            ////return;

            ////====
           
            //1) Create StreamReader

            strReader = File.OpenText(inputFile); //for Scanner only USING
            
            inputFileEncoding = strReader.CurrentEncoding;
            Console.WriteLine("\ninputFile: {0} ; File Encoding: {1}", inputFile, inputFileEncoding);
            ////
            //AppDomain BaseDirectory
            AppDomain appDom1 = AppDomain.CurrentDomain;
            
            Console.WriteLine("\nappDom.BaseDirectory: <{0}>\nappDom.FriendlyName: <{1}>\n", 
                appDom1.BaseDirectory,appDom1.FriendlyName);           
            /////
            //2)Create StreamWriter
            //
            //(a)Create one Dir for StreamWriter of Lexer,Scaner and Parser Outputs (after Analisis: options for debugging)
            // and Parser Output as AST Value after Context Analisis with Diagnostics
            //(b)Create one Dir for each Expressions:
            //   Private Partition,Polish,Diagramm,Automaton(not built up yet!)(after Good Context Diagnostics :options)
            //
            //(b==>c)Create one Dir for each Expressions( 0<=i<=n-1):
            // (i)  Private Partition,Polish,Diagramm,Automaton(not built up yet!)
            // (i) Public Partition, Representation of Private on Public,Diagram on Public, Automaton in Public(after Good Context Diagnostics :options)
            //
            //(d)Create one Dir for Public Partition,Cartesian Product,Classification States and Transitiona(RESULT)
            //
            this.nameScaneLexParseDir = currDir + "/" + nameScaneLexParseDir;
            if (!Directory.Exists(this.nameScaneLexParseDir))
                Directory.CreateDirectory(nameScaneLexParseDir);
            
            //Program.nameScaneLexParseDir Exists!

            string fileParserOutput = (this.nameScaneLexParseDir + "\\") + outputFile;
            File.Delete(fileParserOutput);     //relative(to the current working directory) or absolute path                   
            strWriter = File.CreateText(fileParserOutput); //for Parser only USING


            Console.WriteLine("\nNew currDirect: {0}\n", currDir);
            Console.WriteLine("\nfileParserOutput: {0}\n", fileParserOutput);
             Console.WriteLine("\nPress any Key to Exit!\n");
            //Console.ReadKey();
            ////return;

            //3)Create tokenStringQueue and terminalTokenStringQueue

            tokenStringQueue = new Queue<string>(len_tokenStringQueue);
            terminalTokenStringQueue = new Queue<string>(len_terminalTokenStringQueue);

            //4)Create thrScanner
            Scanner scanner = new Scanner(nameScaneLexParseDir, this.strReader, this.tokenStringQueue);

            ThreadStart delegateStartScanner = new ThreadStart(scanner.Scane);
            Thread thrScanner = new Thread(delegateStartScanner);

            //5)Create thrLexer
            //
            Lexer lexer = new Lexer(nameScaneLexParseDir, tokenStringQueue, terminalTokenStringQueue);
            ThreadStart delegateStartLexer = new ThreadStart(lexer.methodLexem); //Instance Caller method

            //// ThreadStart delegateStartLexer = new ThreadStart(Lexer.Lexem); //Static method
            Thread thrLexer = new Thread(delegateStartLexer);

            //6)Create thrParser
            Parser parser = new Parser(this.strWriter, this.terminalTokenStringQueue);
            ThreadStart delegateStartParser = new ThreadStart(parser.Parse); ////Parser.TestParse
            Thread thrParser = new Thread(delegateStartParser);


            ThreadState thrScannerState = thrScanner.ThreadState;

            //eventScanner = new ManualResetEvent(false);
            ////Scanner.autoResetEvent = new AutoResetEvent(false);

            if (thrScannerState == ThreadState.Unstarted)
            {
                thrScanner.Start();
                Console.WriteLine("\nThread Scanner has just started!\n");
            }

            ////ControlThreadScane(thrScanner);

            thrLexer.Start();
            Console.WriteLine("\nThread Lexer has just started!\n");


            thrParser.Start();
            Console.WriteLine("\nThread Parser has just started!\n");

            thrScanner.Join();
            Console.WriteLine("\nThread Scanner has just ended!\n");

            thrLexer.Join();
            Console.WriteLine("\nThread Lexer has just ended!\n");

            thrParser.Join();
            Console.WriteLine("\nThread Parser has just ended!\n");
            
            strReader.Close(); //after Scanner Return
            strWriter.Close(); //after Parser Return; 
            //1) - 6) Front End of Compiler

            //bool isLeftDerive = true;
            //Derive.TestDerivator(isLeftDerive, Parser.RootAST, "Derivation.txt");
            //
            //7)Back End of Compiler
            int res=EvaluateAST_Verify_GetAutomataInBasicPartition_CartesianProduct(valueASTFile);           
            //===Public Automata from Diagramms are ready if  Verification is succeded! 
            //                  
            //
            //**(8)**Test for <Equals_Id1_Id2>: Boath <Private Partitions> and <Regular Expressions> are different,
            //but they are in the public alphabet!!!
            //
            //TODO: 0)Implement Writing File of Automation with its Private(Public too) Alphabet Partition (Modify old Writing);
            //      1)Implement Loading up Automation with its Private Alphabet Partition (without Verification)(Modify old Writing);
            //      2)And Implement Equals for Two Automata with their Private Alphabet Partitions but
            //       before that converting them into Public Alphabet Partition;
            //      Note: Think of '-' (minus symbol in <patterns> at Writing and Reading file)!!!
            //
            if (res > 0)
            {
                Console.WriteLine("\nres: ({0})\n",res);
                Console.WriteLine("\nPress any Key to Exit!\n");
                //Console.ReadKey();
                return res.ToString();

            }

            goto labelNoEquals;

            string nameRegularExp_1 = "Identifier";
            string fileNameAutomatonFromDiagrammPublic1 = (nameRegularExp_1 + "\\") + (nameRegularExp_1 + "_Private_Automaton.txt");
            string fileAut1 = (currDir + "\\") + fileNameAutomatonFromDiagrammPublic1; 

            TestLoadingAutomataAndEquals(fileAut1, fileAut1);

            string nameRegularExp_2 = "AOctodecimal";
            string fileNameAutomatonFromDiagrammPublic2 = (nameRegularExp_2 + "\\") + (nameRegularExp_2 + "_Private_Automaton.txt");
            string fileAut2 = (currDir + "\\") + fileNameAutomatonFromDiagrammPublic2;

            TestLoadingAutomataAndEquals(fileAut1, fileAut2);

            labelNoEquals:
            Console.WriteLine("\nres: ({0})\n", res);
            Console.WriteLine("\nPress any Key to Exit!\n");
            //Console.ReadKey();
            return res.ToString();

        }

        string SetAppCurrentDir(string nameCurrDir)
        {
            string varUserProfile = "USERPROFILE";
            string valueUserProfile;
            //string nameRegExsDir = "ListRegExs";
            string newCurrDirFullPath;

            valueUserProfile = Environment.GetEnvironmentVariable(varUserProfile);
            newCurrDirFullPath = (valueUserProfile + "\\") + nameCurrDir;

            bool bCurrDir = Directory.Exists(newCurrDirFullPath);//Exists any Directory: here Current Directory!!!
            if (!bCurrDir)
            {
                Directory.CreateDirectory(newCurrDirFullPath);
                
            }
            Directory.SetCurrentDirectory(newCurrDirFullPath);
            return newCurrDirFullPath;
        }

        int  EvaluateAST_Verify_GetAutomataInBasicPartition_CartesianProduct(string fileNameValue)
        {
            if (Parser.RootAST == null)
            {
                Console.WriteLine("Parser not succeeded. Parser.countERRORS: ({0})", Parser.countERRORS);
                return 1;
            }
            //
            //Parser.countERRORS==0 ==> 
            //Parser.RootAST != null ==>
            //Parsing succeeded!!!
            //
            //2)Create StreamWriter
            //
            #region Create streamWriterValueAST
            StreamWriter streamWriterValueAST = null;
            string fileValueAST = (this.nameScaneLexParseDir + "\\") + fileNameValue;
            File.Delete(fileValueAST);
            streamWriterValueAST = File.CreateText(fileValueAST);            
            #endregion            //
            //3)Translate: evaluate AST into the list of Not verified ( boath Context and free Polish Expression)
             // 
            Value valueAST = CreatorValueCIL.CreateValueAST(false );    // true                 
             regularExpList = valueAST.Evaluate();
            //
             //RESULT of valueAST.Evaluate(): regularExpList presents the list of (Context, free Polish Expression).
            //a) Context is the list of not verified equations.
            //b) Free Polish Expression is a well-defined and free one against Context.
            //
            //4)Verify Evaluated AST
            //
             bool bVerificationSucceded = regularExpList.Run_Verification_RegularExpListToStream(streamWriterValueAST);            
            //
            //?????
            //Result of <<RegularExpList.Run_Verification_RegularExpListToStream(StreamWriter)>>: 
            //a)RegularExpList regularExpList;
             //b)StreamWriter   streamWriterValueAST;
           
            //All Info See in RegularExpList regularExpList and use it.   
            //We use  <<ValuePolish valuePolish = CreatorValuePolishCIL.CreateValuePolish(string TypeName, List<string> polishExpression, bool isSave);>>                    
            //it creates Assembly that exports the <<type ValuePolishCIL:ValuePolish>>
            //and <<class GeneratorPolishCIL : GeneratorPolish>>
            //////////////////////////////////////////////// 

            //5)For each definition of a regular expression in the Context generate 
            // three files: a) alphabet partition; b)polish expression; c)diagram.
            
            //a)If there are alphabet partition errors then generate b)free polish expression; c)free diagram.
            //b)If there are no alphabet partition errors and no polish expression errors 
            //then generate a) alphabet partition; b)polish expression; c)diagram.
            //c)If there are no alphabet partition errors and there are polish expression errors 
            //then generate a) alphabet partition; b)free polish expression; c)free diagram.
            //??????????

             if (bVerificationSucceded)
                 streamWriterValueAST.WriteLine("Correct regularExpList! Do Intersection of Partions!");
             else
             {
                 streamWriterValueAST.WriteLine("Incorrect regularExpList! No Intersection of Partions!");

             }

            streamWriterValueAST.Close();

            //////////////////////////////////////
            //bVerificationSucceded  ==>AST_Value_Input ... ==> Do Public Alphabet ==>Diagramm and Automaton in Public ==> Cartesian Product 
            //!bVerificationSucceded ==>AST_Value_Input ...
            //1)In any case either bVerificationSucceded or !bVerificationSucceded
            //
            //Polish and Diagramm are written to their files ignoring Context as Polish is syntactically correct!!!
            regularExpList.Print_Ready_Verified_RegularExpList_ContextWithDiagnostics_Polish_Diagram(this.currDir);//Only to files
            //
            if (!bVerificationSucceded)
            {
                //Clean solution for <RegularExpList this>
                //All Private_Automaton
                //All Public_Diagramm
                //All Public_Automation
                regularExpList.Clean_RegularExpList_Diagramm_Automaton_PrivateAndPublic(this.currDir);
                File.Delete("Intersection" + "_Partition.txt");
                File.Delete("Cartesian" + "_Automaton.txt");//Clean
                return 2;
            }
             ///// 
                       
            //bVerificationSucceded  
            regularExpList.Get_RegularExpList_Target(currDir);//(bVerificationSucceded)==>Target is  All Public Automata 

                     
            regularExpList.GetCartesianProduct(currDir);

            return 0;
        }

        //static Value  CreateValueAST(bool isSave)
        //{
        //    AppDomain currentAppDomain = Thread.GetDomain(); 

        //    AssemblyName assemblyName = new AssemblyName();
        //    assemblyName.Name = "ValueAST";
        //    AssemblyBuilder assemblyBuilder = currentAppDomain.DefineDynamicAssembly(
        //        assemblyName,
        //        AssemblyBuilderAccess.RunAndSave
        //        );

        //    ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("ValueAST.dll", "ValueAST.dll");

        //    TypeBuilder typeBuilder = moduleBuilder.DefineType("ValueCIL",  //Derived Class <<ValueCIL>>
        //        TypeAttributes.Public | TypeAttributes.Class,
        //        typeof(Value)                                               // Base Class <<Value>>
        //        );

        //    ConstructorBuilder constructorBuilder1 = typeBuilder.DefineConstructor(
        //        MethodAttributes.Public,
        //        CallingConventions.Standard,
        //        new Type[] { }
        //        );

        //    ILGenerator constructorBuilder1_ILGen = constructorBuilder1.GetILGenerator();
        //    constructorBuilder1_ILGen.Emit(OpCodes.Ldarg_0);
        //    constructorBuilder1_ILGen.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));
        //    constructorBuilder1_ILGen.Emit(OpCodes.Ret);

        //    MethodBuilder evaluateMethodBuilder = typeBuilder.DefineMethod(
        //        "Evaluate",
        //        MethodAttributes.Public | MethodAttributes.Virtual,
        //        typeof(void),
        //        new Type[] { typeof(StreamWriter) }
        //        );

        //    ILGenerator evaluateMethodBuilder_ILGen = evaluateMethodBuilder.GetILGenerator();
           

        //    ////.
        //    GeneratorCIL genAST = new GeneratorCIL(Parser.RootAST, evaluateMethodBuilder_ILGen);

        //    genAST.EmitEvaluate();

        //    //1) CreateType()
        //    //2) assemblyBuilder.Save("Func.dll");
        //    //3) Create new instance of ValueCIL ==> void Evaluate(StreamWriter)

        //    Type type = typeBuilder.CreateType(); //type is ValueCIL == true
        //    if (isSave)
        //    {
        //        assemblyBuilder.Save("ValueAST.dll");
        //    }
        //    ConstructorInfo ctor = type.GetConstructor(new Type[0]);//ctor for class <<ValueCIL>> 

        //    return ctor.Invoke(null) as Value; // Instance of <<ValueCIL>> as <<Value>>

        //}
/*
 * lock(obj) {...} is equal to 
 * Monitor.Enter(obj);
 * try {...}
 * finally {Monitor.Exit(obj);}
 * // obj is not of type-value but is of type-reference!
 */
        //static void Test_getInputTerminal()
        //{
        //    //getInputTerminal(numCurrLine);
        //    Console.WriteLine("\n Test for getInputTerminal(numCurrLine)");
        //    int i = 0;
        //    foreach (string nameTerminal in LALR_Tables.NamesTA)
        //    {
        //        Console.WriteLine("{0} [{1}]<{2}>", i++, Parser.getInputTerminal(nameTerminal.PadRight(6,' ')), nameTerminal);
        //    }
        //    Console.WriteLine("{0} [{1}]<{2}>", i++, Parser.getInputTerminal("#".PadRight(6, ' ')), "#".PadRight(6, ' '));
        //    Console.WriteLine("{0} [{1}]<{2}>", i++, Parser.getInputTerminal("qw".PadRight(6, ' ')), "qw".PadRight(6, ' '));
        //}

        void TestInputArgs(string[] args)
        {
            if ((args.GetLength(0) != 2) && (args.GetLength(0) != 4))
                
                throw new System.ArgumentException(string.Format("\nError 1: args count:{0} not in [2,4]", args.GetLength(0)));

            if (args.GetLength(0) == 4)
            {
                if (args[2] != "/d")
                    throw new System.ArgumentException(string.Format("\nError 2: args[2]:{0} != /d", args[2]));
                string projectDir = args[3];
                string patternDir = @"^(?<nameDir>\w+){1}";
                Regex rgxDir = new Regex(patternDir);
                if (rgxDir.IsMatch(projectDir))
                    Console.WriteLine("{0} is an project Directory name .\n", projectDir);
                else
                    throw new System.ArgumentException(string.Format("\nError 4: Invalid project Directory name Format: <{0}>.\n", projectDir));
                //Create project Directory (== Current Dir)
                this.currDir = projectDir;

            }
            //
            if (args[0] != "/c")
                throw new System.ArgumentException(string.Format("\nError 2: args[0]:{0} != /c", args[0]));

            Console.WriteLine("args count:{0}\n", args.GetLength(0));
            Console.WriteLine("args[0]:{0}\nargs[1]:{1}\n", args[0], args[1]);

            inputFile = args[1];

            //Test1

            // Define the regular expression pattern. 
            string pattern;
            pattern = @"^(?<filename>\w+){1}(?<point>\.){1}(?<ext>rgl){1}";

            Regex rgx = new Regex(pattern);
            if (rgx.IsMatch(inputFile))
                Console.WriteLine("{0} is an input file.\n", inputFile);
            else
                throw new System.ArgumentException(string.Format("\nError 4: Invalid Input File Format: <{0}>.\n", inputFile));


            Match match = rgx.Match(inputFile);
            GroupCollection groups = match.Groups;

            //groups["filename"].Captures.Count

            Console.WriteLine("\ngroups.Count is {0} .\n", groups.Count);

            Console.WriteLine("\ngroups[\"filename\"].Captures.Count is {0} .\n", groups["filename"].Captures.Count);

            Console.WriteLine("\ngroups[\"filename\"].Value is '{0}'\nat position groups[\"filename\"].Index = {1}", groups["filename"].Value, groups["filename"].Index);
            Console.WriteLine("\n'{0}' at position {1}", groups["point"].Value, groups["point"].Index);
            Console.WriteLine("\n'{0}' at position {1}\n", groups["ext"].Value, groups["ext"].Index);


            int lenInFl = inputFile.Length;
            string extInFile = inputFile.Substring(lenInFl - 3);

            if (extInFile != "rgl")
                throw new System.ArgumentException(string.Format("\nError 4: Input File Extention:<{0}> !=  <rgl>", extInFile));


            outputFile = inputFile.Substring(0, lenInFl - 3) + "out";
            Console.WriteLine("Output File :<{0}>\n", outputFile);

            valueASTFile = "AST_Value_"+inputFile.Substring(0, lenInFl - 3) + "txt";
            Console.WriteLine("AST Value of Input File :<{0}>\n", valueASTFile);

            string currDir = args[3];
            Console.WriteLine("\nCurrent Dir:\n{0}\n", currDir);
            inputFile = currDir + "/" + inputFile;

            if (!File.Exists(inputFile))
                throw new System.ArgumentException(string.Format("\nError 3: args[1]:<{0}>\n , file not exist in Current Directory:\n <{1}>\n", args[1], currDir));

            Console.WriteLine("\n args[1] - Input file: <{0}>\n exist in Current Directory:\n <{1}>\n ", args[1], currDir);
        }//TestInputArgs

        void TestLoadingAutomataAndEquals(string fileAut1,string fileAut2)
        {
            //Boath Automaton1 and Automaton2 are in Public Alphabet.
            //If they are in their Private Alphabets then they have to be converted in Public Alphabet
            //before comparing!!!
            //Constructor loads up an automation from its file and create it Public Alphabet ignoring
            //its Automaton.classPartList.Values!
            
            //Automaton.EqualsInPublicKeys(m1, m2) 
            //does no control of their alphabet partitions( = mi.classPartList.Values) It has to do that!            
            //It only does control of their alphabet Automaton.classPartList.Keys!

            Automaton m1 = new Automaton(fileAut1); //Loading up Automaton from fileAut1 in Public Keys
            Automaton m2 = new Automaton(fileAut2);
            bool b12 = Automaton.EqualsInPublicKeys(m1, m2);

            Console.WriteLine("m1:<{0}>", fileAut1);
            Console.WriteLine("m2:<{0}>", fileAut2);
            Console.WriteLine("\nAutomatonEqualsInPublicKeyss(m1, m2)=={0}\n", b12);

        }
    }
}
