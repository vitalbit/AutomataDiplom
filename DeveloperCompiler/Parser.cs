using System;
//
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
//using System.IO;
using System.Threading;
//
using Const_LALR_Tables;
using AST;
using System.IO;

namespace ConsoleFrontEnd
{
    class Parser
    {
        public static object RootAST =null;
        public static int countERRORS = 0;
        private StreamWriter strWriter;
        private Queue<string> terminalTokenStringQueue;

        public Parser(StreamWriter strWriter, Queue<string> terminalTokenStringQueue)
        {
            this.strWriter = strWriter;
            this.terminalTokenStringQueue = terminalTokenStringQueue;
        }

        public void Parse()
        {
            int cnt_terminal = 0;
 
            string numCurrLine = "";
            string terminal = "";
            bool endStream = false;

            Stack stack = new Stack(50);
            Stack stack_AST = new Stack(50);


            LALR_Tables.InitStack(stack);
            LALR_Tables.InitStack(stack_AST);

            int q_next = -1;
            int q_state = 0;  //state on top of stack

            int a_input = -1; //input token (terminal)
            object tokenTerminal = null;

            int A_input = -1; // input nonterminal for r-REDUCTION 
            object objectProduction = null;

            int ACTION = -2;  //ACTIONS:  -2-ERROR, -1-MOVE, 0-ACCEPT, r-REDUCTION(r>0)
            int rule = 0;

            

            bool b_get_CurrentLine = true;

            while (!endStream)
            {
                //1)
                //get <numCurrLine> ==> a_input
                if (b_get_CurrentLine)
                {
                    lock (((ICollection)this.terminalTokenStringQueue).SyncRoot) //lineQueueSync.SyncRoot obSync
                    {
                        cnt_terminal = this.terminalTokenStringQueue.Count;
                        if (this.terminalTokenStringQueue.Count == 0)
                        {
                            Thread.Sleep(0);
                            continue;
                        }
                        //cnt>0                    
                        numCurrLine = this.terminalTokenStringQueue.Dequeue();

                    }//lock

                    //numCurrLine -> a_input
                    a_input = getInputTerminal(numCurrLine);
                    //(a_input == 0) ==> @ is for Empty String ==  (endStream = true;)
                    //(a_input < 0) ==> no terminal, ERROR
                    //1 <= a_input <= 20==LALR_Tables.NamesTA.GetLength(0) ==> terminal
                }


                if (a_input < 0)
                    ACTION = -2; //-2-ERROR
                else//a_input == 0
                    ACTION = LALR_Tables.AT_LALR_CANONICAL[q_state, a_input];

                //<stack|q_state, a_input.terminalTokenStringQueue,strWriter.> - current configuration 
                switch (ACTION)
                {
                    case -2: //-2-ERROR
                        countERRORS++;

                        this.strWriter.WriteLine("*** ACTION = -2-ERROR (see stack)");
                        this.strWriter.WriteLine("*** q_state ={0}, a_input ={1}", q_state, a_input);
                        this.strWriter.WriteLine("*** {0}", numCurrLine);
                        SkipInputTerminals();
                        endStream = true;
                        continue;
                        //break;

                    case -1: //-1-MOVE

                        stack.Push(a_input);
                        tokenTerminal = new Terminal(a_input, numCurrLine);
                        stack_AST.Push(tokenTerminal);

                        q_state = LALR_Tables.TGO_LALR[q_state, a_input];
                        stack.Push(q_state);
                        stack_AST.Push(q_state);

                        //endStream == false
                        b_get_CurrentLine = true; //after -1-MOVE
                        continue; //!!! get next <numCurrLine> from <terminalTokenStringQueue>
                        //break;
                    case 0: //0-ACCEPT
                        ////========objectProduction is Root of AST=============
                        //stack ==(q0,L,q_state)
                        stack_AST.Pop();//q_state
                        objectProduction=new Production(0,new object[]{stack_AST.Pop()});//0: <> = L

                        this.strWriter.WriteLine("{0}: {1}", 0, LALR_Tables.Grammar_Prod[0]);
                        endStream = true;
                        continue;
                        //break;
                    default:
                        {
                            //ACTION = r-REDUCTION(r>0)
                            rule =ACTION;

                            //A_input-1000 == nonterminal index in the array NamesNA
                            A_input = doReductions(stack, rule, stack_AST, out objectProduction); //Apply the production rule: A = alpha

                            if (A_input < 1001)
                            {
                                //the production rule==ACTION was not applyed to <stack>
                                countERRORS++;

                                //ERRORS: -3,-2,-1, 0, ... ,1000
                                //<stack> was not changed:<alpha> was not poped up 
                                //Error Configuration before rule==ACTION not applying to <stack>
                                this.strWriter.WriteLine("*** ERROR (see stack): not applying to <stack>\n? {0}: {1}", rule, LALR_Tables.Grammar_Prod[rule]);
                                SkipInputTerminals();

                                endStream = true;
                                continue;
                                //break;

                            }

                            //the production rule==ACTION was applyed to <stack>
                            //<stack> was  changed: <alpha> was poped up 

                            q_state = (int)stack.Peek();
                            q_next = LALR_Tables.NGO_LALR[q_state, A_input - 1000]; //A_input-1000==nonterminal index in the array NamesNA
                            if (q_next == 1)
                            {
                                countERRORS++;
                                //error state q_next == 1
                                //<stack> was changed
                                //Error Configuration after rule==ACTION applying to <stack>
                                this.strWriter.WriteLine("*** ERROR (q_next == 1, see stack): after applying to <stack>\n? {0}: {1}", rule, LALR_Tables.Grammar_Prod[rule]);
                                SkipInputTerminals();
                                endStream = true;
                                continue;
                                //break;
                            }

                            stack.Push(A_input);
                            stack_AST.Push(objectProduction);

                            stack.Push(q_next);
                            stack_AST.Push(q_next);

                            q_state = q_next;

                            //<stack> was changed
                            //a_input not changed
                            //Next Configuration is ready :
                            //(q_state,a_input)

                            //!!!not get next <numCurrLine> from <terminalTokenStringQueue> as "a_input" not changed
                            //CONTINUE PARSING with new "q_state" and old "a_input"
                            //endStream == false

                            this.strWriter.WriteLine("{0}: {1}", rule, LALR_Tables.Grammar_Prod[rule]);

                            b_get_CurrentLine = false;
                            continue;
                            
                            //break;
                        }

                }//switch (ACTION)
              
            }//while (!endStream)
            ////========objectProduction is Root of AST=============
            //(countERRORS == 0) ==> objectProduction is Root of AST

            if (countERRORS == 0)
                RootAST = objectProduction;
            else
            {
                RootAST = null;
                this.strWriter.WriteLine("*** countERRORS :{0}", countERRORS);
            }

            this.strWriter.WriteLine("#");

        }//Parse

        public void TestParse()
        {
            int cnt_terminal = 0;

            string numCurrLine = "";
            string terminal = "";
            bool endStream = false;

            int a_input = -1; //input token (terminal)            

            while (!endStream)
            {
                //1)
                //get <numCurrLine> ==> a_input
              
                    lock (((ICollection)this.terminalTokenStringQueue).SyncRoot) //lineQueueSync.SyncRoot obSync
                    {
                        cnt_terminal = this.terminalTokenStringQueue.Count;
                        if (this.terminalTokenStringQueue.Count == 0)
                        {
                            Thread.Sleep(0);
                            continue;
                        }
                        //cnt>0                    
                        numCurrLine = this.terminalTokenStringQueue.Dequeue();

                    }//lock

                    //numCurrLine -> a_input
                    a_input = getInputTerminal(numCurrLine);
                    //(a_input == 0) ==> @ is for Empty String <==>  (endStream = true;)
                    //(a_input <  0) ==> no terminal, ERROR
                    //1 <= a_input <= 20==LALR_Tables.NamesTA.GetLength(0) ==> terminal
               


                    if (a_input == 0)
                    {
                        endStream = true;
                        //terminal = string.Format("{0}={1} //+{2}", numCurrLine, a_input, cnt_terminal);
                        continue;
                    }
                    if (a_input < 0)
                    {
                        endStream = true;
                        //terminal = string.Format("{0}={1} //+{2}", numCurrLine, a_input, cnt_terminal);
                        continue;
                    }

                    terminal = string.Format("{0}={1} //+{2}", a_input, numCurrLine, cnt_terminal);
                    this.strWriter.WriteLine(terminal);
                    continue;

            }//while (!endStream)

            this.strWriter.WriteLine("#");

        }//TestParse()

        void SkipInputTerminals()
        {
            int cnt_terminal = 0;
            string numCurrLine = "";
            int a_input = -1;

            bool endStream = false ;
            while (!endStream)
            {
                //1)
                //get <numCurrLine> ==> a_input
                
                    lock (((ICollection)this.terminalTokenStringQueue).SyncRoot) //lineQueueSync.SyncRoot obSync
                    {
                        cnt_terminal = this.terminalTokenStringQueue.Count;
                        if (this.terminalTokenStringQueue.Count == 0)
                        {
                            Thread.Sleep(0);
                            continue;
                        }
                        //cnt>0                    
                        numCurrLine = this.terminalTokenStringQueue.Dequeue();

                    }//lock

                    //numCurrLine -> a_input
                    a_input = getInputTerminal(numCurrLine);
                    //(a_input == 0) ==> @ is for Empty String ==  (endStream = true;)
                    //(a_input < 0) ==> no terminal, ERROR
                    //1 <= a_input <= 20==LALR_Tables.NamesTA.GetLength(0) ==> terminal



                    if (a_input != 0)
                        continue;
                    else
                       endStream=true;
            }//while (!endStream)
        }//SkipInputTerminals()

        static int doReductions(Stack stack, int rule, Stack stack_AST, out object objectProduction) //Apply the production rule: A = alpha
        {
            objectProduction = null;
            if (stack.Count != stack_AST.Count)
                return -3;

            int A_rule = -1;

            //TODO
            //rule == ACTION == r-REDUCTION(r>0)
            //rule > 0

            string      grammar_Prod = LALR_Tables.Grammar_Prod[rule];
            string[]  arrProdSymbols = grammar_Prod.Split('=', '*');

            int len_Arule = arrProdSymbols.GetLength(0); //len_Arule >= 2           
             
            for (int i = 0; i < len_Arule; i++)
                arrProdSymbols[i] = arrProdSymbols[i].Trim();

            if(len_Arule==2)
            {
                if (arrProdSymbols[1] == "@")
                {
                    //A = @, |@|==|alpha|==0, 
                    A_rule = getNonTerminal(arrProdSymbols[0]);
                    // stack, rule, stack_AST not changed ; objectProduction==null!
                    return A_rule;
                }

            }
            //A=alpha, |alpha|> 0
            
            //Do pop up stack if it is possible!!!
            //Pop up 2*(len_Arule-1) items

            int len_pathAlpha = 2 * (len_Arule - 1);
            int[]   pathAlpha = new int[len_pathAlpha];

            int  len_path_objectAlpha = len_Arule - 1;
            object[] path_objectAlpha = new object[len_path_objectAlpha];

          //DO POPING UP "stack"

            if(len_pathAlpha >= stack.Count)
                return -2; //ERROR -2

            for (int k = 1; k <= len_pathAlpha; k++)
                pathAlpha[len_pathAlpha-k] = (int)stack.Pop();

          //END DO POPING UP "stack"

            if (!verifyRule(pathAlpha, arrProdSymbols))
                return -1; //ERROR -1

            //OK verifyRule!
            //DO POPING UP "stack_AST"
          
            for (int k = 1; k <= len_path_objectAlpha; k++)
            {
                stack_AST.Pop();
                path_objectAlpha[len_path_objectAlpha - k] = stack_AST.Pop();
            }

            //END DO POPING UP "stack_AST"

            objectProduction = new Production(rule, path_objectAlpha);

            A_rule = getNonTerminal(arrProdSymbols[0]);

            return A_rule;           
        }

        static bool verifyRule(int[] pathAlpha, string[] arrProdSymbols)
        {
            bool isRule = true;
            int len_arrProdSymbols=arrProdSymbols.GetLength(0);
            isRule = 2*(len_arrProdSymbols-1)== pathAlpha.GetLength(0);
            if(!isRule)
                return false;

            for(int i = 0 ; i < len_arrProdSymbols-1; i++)
            {
                int pathSymbol =pathAlpha[2*i];
                if (arrProdSymbols[i + 1] != ((pathSymbol < 1000) ? LALR_Tables.NamesTA[pathSymbol] : LALR_Tables.NamesNA[pathSymbol - 1000]))
                    return false;
            }

            return isRule;
        }
       
        static int getTerminal(string nameTerminal)
        {
            int len = LALR_Tables.NamesTA.GetLength(0);

            for (int i = 0; i < len; i++)
                if (nameTerminal == LALR_Tables.NamesTA[i])
                    return i;
                else
                    continue;
            return -1;
        }
        static int getNonTerminal(string nameNonTerminal)
        {
            int len = LALR_Tables.NamesNA.GetLength(0);

            for (int i = 0; i < len; i++)
                if (nameNonTerminal == LALR_Tables.NamesNA[i])
                    return i + 1000;
                else
                    continue;
            return -1;
        }

       public  static int getInputTerminal(string inputTokenLine)
        {
            if (inputTokenLine[0] == '#')
                return 0; //@ is for the empty string that is the End of Stream of Terminals

            string nameTerminal=inputTokenLine.Substring(0, 5).Trim();

            int len = LALR_Tables.NamesTA.GetLength(0);

            for (int i = 0; i < len; i++)
                if (nameTerminal == LALR_Tables.NamesTA[i])
                    return i;
                else
                    continue;
            return -1;

        }
    }
}
