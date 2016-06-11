using System;
//
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//
//using System.IO;
using System.Threading;
//
using Const_LALR_Tables;

namespace ConsoleFrontEnd
{
    class Lexer
    {

        private StreamWriter streamWriter;
        private Queue<string> tokenStringQueue;
        private Queue<string> terminalTokenStringQueue;
        public Lexer(string nameScaneLexParseDir, Queue<string> tokenStringQueue, Queue<string> terminalTokenStringQueue)
        {
            //Program.nameScaneLexParseDir Exists!
            string fileLexerToken = nameScaneLexParseDir + "\\LexerToken.txt";
            File.Delete(fileLexerToken);     //relative(to the current working directory) or absolute path                   
            streamWriter = File.CreateText(fileLexerToken);
            this.tokenStringQueue = tokenStringQueue;
            this.terminalTokenStringQueue = terminalTokenStringQueue;

        }
        public void methodLexem()
        {
            int cnt_tokenStringQueue = 0;

            string numCurrLine = "";
            string terminal = "";
            bool endStream = false;
            bool bEnqueue = false;

            while (!endStream)
            {
                //1)
                lock (((ICollection)this.tokenStringQueue).SyncRoot) //lineQueueSync.SyncRoot obSync
                {
                    cnt_tokenStringQueue = this.tokenStringQueue.Count;
                    if (this.tokenStringQueue.Count == 0)
                    {
                        Thread.Sleep(0);
                        continue; //==> numCurrLine is not gotten and endStream==false,so try again to get it!
                    }
                    else //cnt_tokenStringQueue > 0
                    {
                        numCurrLine = this.tokenStringQueue.Dequeue();
                    }

                }//lock

                //2)
                if (numCurrLine[0] == '#')
                {
                    //Program.strWriter.WriteLine(numCurrLine);
                    ////?Program.strWriter.WriteLine(string.Format("{0}{1}", numCurrLine + "//+", cnt_tokenStringQueue));
                    terminal = string.Format("{0} //+{1}", numCurrLine, cnt_tokenStringQueue);

                    //Program.strWriter.WriteLine(terminal);
                    //
                    streamWriter.WriteLine(terminal);
                    //
                    endStream = true;
                    ////?continue;
                }
                else
                {
                    if (LALR_Tables.IsCommNoTail(numCurrLine))
                        continue;
                    if (LALR_Tables.IsTailDelim(numCurrLine))
                        continue;
                    if (LALR_Tables.IsRegular(numCurrLine))
                        terminal = LALR_Tables.NamesTA[16];
                    else if (LALR_Tables.IsAlphabet(numCurrLine))
                        terminal = LALR_Tables.NamesTA[1];
                    else if (LALR_Tables.IsExpression(numCurrLine))
                        terminal = LALR_Tables.NamesTA[6];
                    else if (LALR_Tables.IsIdentifier(numCurrLine))
                        terminal = LALR_Tables.NamesTA[9];
                    else if (LALR_Tables.IsChar(numCurrLine))
                        terminal = LALR_Tables.NamesTA[3];
                    else if (LALR_Tables.IsSTR(numCurrLine))
                        terminal = LALR_Tables.NamesTA[20];
                    else if (LALR_Tables.IsOp(numCurrLine))
                        terminal = LALR_Tables.NamesTA[LALR_Tables.OpTerminal(numCurrLine)];
                    else
                        terminal = "qwst";

                    ////?Program.strWriter.WriteLine(string.Format("{0}{1}{2}", terminal.PadLeft(4, ' ').PadRight(5, ' '), numCurrLine + "//+", cnt_tokenStringQueue));
                    //terminal=string.Format("{0}{1}{2}", terminal.PadLeft(4, ' ').PadRight(5, ' '), numCurrLine+ "//+", cnt_tokenStringQueue);
                    terminal = string.Format("{0}{1}", terminal.PadLeft(4, ' ').PadRight(5, ' '), numCurrLine);//+ "//+", cnt_tokenStringQueue);

                    //Program.strWriter.WriteLine(terminal);
                    //
                   streamWriter.WriteLine(terminal);
                    //

                    //terminal = string.Format("{0} //+{1}", terminal, cnt_tokenStringQueue);
                }

                //3)endStream ==? and terminal are ready!!!

                bEnqueue = false;
                do
                {
                    lock (((ICollection)this.terminalTokenStringQueue).SyncRoot) //lineQueueSync.SyncRoot obSync
                    {
                        if (this.terminalTokenStringQueue.Count < Program.len_terminalTokenStringQueue)
                        {
                            this.terminalTokenStringQueue.Enqueue(terminal);
                            bEnqueue = true;
                        }
                        else
                        {
                            Thread.Sleep(0);
                            continue;//do{...}while (!bEnqueue);
                        }
                    }

                } while (!bEnqueue);


            }//while (!endStream)
            streamWriter.Close();
        }//Lexem

        //public static void Lexem()
        //{
        //    int cnt_tokenStringQueue = 0;

        //    string numCurrLine = "";
        //    string terminal = "";
        //    bool endStream = false;
        //    bool bEnqueue = false;

        //    while (!endStream)
        //    {
        //        //1)
        //        lock (((ICollection)Program.tokenStringQueue).SyncRoot) //lineQueueSync.SyncRoot obSync
        //        {
        //            cnt_tokenStringQueue = Program.tokenStringQueue.Count;
        //            if (Program.tokenStringQueue.Count == 0)
        //            {
        //                Thread.Sleep(0);
        //                continue; //==> numCurrLine is not gotten and endStream==false,so try again to get it!
        //            }
        //            else //cnt_tokenStringQueue > 0
        //            {
        //                numCurrLine = Program.tokenStringQueue.Dequeue();
        //            }

        //        }//lock

        //        //2)
        //        if (numCurrLine[0] == '#')
        //        {
        //            //Program.strWriter.WriteLine(numCurrLine);
        //            ////?Program.strWriter.WriteLine(string.Format("{0}{1}", numCurrLine + "//+", cnt_tokenStringQueue));
        //            terminal = string.Format("{0} //+{1}", numCurrLine, cnt_tokenStringQueue);

        //            Program.strWriter.WriteLine(terminal);

        //            endStream = true;
        //            ////?continue;
        //        }
        //        else
        //        {
        //            if (LALR_Tables.IsCommNoTail(numCurrLine))
        //                continue;
        //            if (LALR_Tables.IsTailDelim(numCurrLine))
        //                continue;
        //            if (LALR_Tables.IsRegular(numCurrLine))
        //                terminal = LALR_Tables.NamesTA[16];
        //            else if (LALR_Tables.IsAlphabet(numCurrLine))
        //                terminal = LALR_Tables.NamesTA[1];
        //            else if (LALR_Tables.IsExpression(numCurrLine))
        //                terminal = LALR_Tables.NamesTA[6];
        //            else if (LALR_Tables.IsIdentifier(numCurrLine))
        //                terminal = LALR_Tables.NamesTA[9];
        //            else if (LALR_Tables.IsChar(numCurrLine))
        //                terminal = LALR_Tables.NamesTA[3];
        //            else if (LALR_Tables.IsSTR(numCurrLine))
        //                terminal = LALR_Tables.NamesTA[20];
        //            else if (LALR_Tables.IsOp(numCurrLine))
        //                terminal = LALR_Tables.NamesTA[LALR_Tables.OpTerminal(numCurrLine)];
        //            else
        //                terminal = "qwst";

        //            ////?Program.strWriter.WriteLine(string.Format("{0}{1}{2}", terminal.PadLeft(4, ' ').PadRight(5, ' '), numCurrLine + "//+", cnt_tokenStringQueue));
        //         //terminal=string.Format("{0}{1}{2}", terminal.PadLeft(4, ' ').PadRight(5, ' '), numCurrLine+ "//+", cnt_tokenStringQueue);
        //            terminal=string.Format("{0}{1}", terminal.PadLeft(4, ' ').PadRight(5, ' '), numCurrLine);//+ "//+", cnt_tokenStringQueue);
                    
        //            Program.strWriter.WriteLine(terminal);

        //            //terminal = string.Format("{0} //+{1}", terminal, cnt_tokenStringQueue);
        //        }

        //        //3)endStream ==? and terminal are ready!!!

        //        bEnqueue = false;
        //        do
        //        {
        //            lock (((ICollection)Program.terminalTokenStringQueue).SyncRoot) //lineQueueSync.SyncRoot obSync
        //            {
        //                if (Program.terminalTokenStringQueue.Count < Program.len_terminalTokenStringQueue)
        //                {
        //                    Program.terminalTokenStringQueue.Enqueue(terminal);
        //                    bEnqueue = true;
        //                }
        //                else
        //                {
        //                    Thread.Sleep(0);
        //                    continue;//do{...}while (!bEnqueue);
        //                }
        //            }

        //        } while (!bEnqueue);


        //    }//while (!endStream)

        //}//Lexem
    }
}
