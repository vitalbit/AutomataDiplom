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
using System.Text.RegularExpressions;


namespace ConsoleFrontEnd
{
    class Scanner
    {
        private StreamWriter streamWriter;
        private StreamReader strReader;
        private Queue<string> tokenStringQueue;
        public Scanner(string nameScaneLexParseDir, StreamReader strReader, Queue<string> tokenStringQueue)
        {
            //Program.nameScaneLexParseDir Exists!
            string fileScannerLexeme = nameScaneLexParseDir + "\\ScannerLexeme.txt";
            File.Delete(fileScannerLexeme);     //relative(to the current working directory) or absolute path                   
            streamWriter = File.CreateText(fileScannerLexeme);
            this.strReader = strReader;
            this.tokenStringQueue = tokenStringQueue;

        }
        //Regular Expression for Tokens
        //Without '^' all identifiers are replaced!
        // const string ID1 = @"^(?<id>[_A-Za-z][_A-Za-z0-9]*)";

        const string ID = @"(?<id>[_A-Za-z][_A-Za-z0-9]*)"; //\s*

        const string OP = "(?<op>\\+|\\-|\\*|{|}|\\^|;|=|\\.|\\(|\\)|\\||,|\\]|\\[)";    //|\""   // /|

        // const string COMM1 = @"(?<comm>\s*//.*)";
        const string COMM = @"(?<comm>//.*)";

        // const string XCHAR = "(?<xchar>'\\\\x[0-9a-fA-F]{1,2}')";

        //const string CHAR = "(?<char>'[\\x21-\\x7e]'|'\\\\[xX][0-9a-fA-F]{1,2}')";//1
        //const string CHAR = @"(?<char>'[\x21-\x7e]'|'\\[xX][0-9a-fA-F]{1,2}')";//2

        const string CHAR = @"(?<char>'\\[\x21-\x7e]'|'[\x21-\x7e]'|'\\[xX][0-9a-fA-F]{1,2}')";//3ERROR
        //const string CHAR = @"(?<char>'(\?)[\x21-\x7e]'|'\\[xX][0-9a-fA-F]{1,2}')";//4Error
        /*
         * const string CHAR = @"(?<char>'[\x21-\x7e]'|'\[xX][0-9a-fA-F]{1,2}')";
                //Second
                const string CHAR = "(?<char>('\\[\\x21-\\x7e]')|('[\\x21-\\x7e]'|'\\\\[xX][0-9a-fA-F]{1,2}'))";
        */
        //const string STR1 = "(?<STR1>\"\\\"\")";
        const string STR = "(?<STR>\"[\\x20-\\x21\\x23-\\x5B\\x5F-\\x7e]*\")"; // except "=0x22,\=0x5C

        // const string WS = @"(?<WS>\s*)";

        const string ALPHABET = "(?<ALPHABET>[\t\v\b\\x20-\\x7e]*)";

        //const string TAIL = "(?<ERROR>([\t\v\b\\x20]*[^\t\v\b\\x20]+)+)";
        //const string TAIL = "(?<ERROR>([\t\v\b\\x20]*[^\t\v\b\\x20]+[\t\v\b\\x20]*)+)";
        const string TAIL = @"(?<ERROR>([\t\v\b\x20]*[^\t\v\b\x20]+[\t\v\b\x20]*)+)";

        //const string DELIM = "(?<ERROR>([\t\v\b\\x20]*[^\t\v\b\\x20]+[\t\v\b\\x20]*)+)";
        const string DELIM = @"(?<ERROR>([\t\v\b\x20]*[^\t\v\b\x20]+[\t\v\b\x20]*)+)";

        const string REGEXPR = ID + "|" + OP + "|" + COMM + "|" + STR + "|" + CHAR;

        // static ManualResetEvent eventScanner;       
        // public static System.Threading.AutoResetEvent autoResetEvent;

        static string ReadMultiLine(StreamReader streamReader,ref int  cntLine)
        {
            string currLine = "";
            string multiLine = "";
            //bool bEnd=true;
            do{
                currLine = streamReader.ReadLine();               

                cntLine++;
                if (currLine.Length == 0)//Empty line
                    continue;
                if (currLine[currLine.Length - 1] != '\\')
                {
                    multiLine = multiLine + currLine;
                    return multiLine;
                }
               currLine= currLine.Remove(currLine.Length - 1);
                multiLine = multiLine + currLine;

            } while (true);

            //return multiLine;
        }

        public /*static*/ void Scane()
        {
            int curr_lineCount = 0;
            int start_lineCount = 0;
            string currLine = "";
            string numCurrLine = "";
            ArrayList tokenList;
            //Thread currThread = Thread.CurrentThread;
            bool bEnqueue = false;
            while (!this.strReader.EndOfStream)
            {
                //currLine = Program.strReader.ReadLine();
                //1)
                curr_lineCount = start_lineCount;
                currLine = ReadMultiLine(this.strReader, ref curr_lineCount);
                //2)
                tokenList = GetTokens(currLine, REGEXPR);
               
                start_lineCount++;
                
                for (int i = 0; i < tokenList.Count; i++)
                {

                    numCurrLine = string.Format("{0} : ({1},{2})", (string)tokenList[i], start_lineCount,curr_lineCount);
                    //Console.WriteLine("1) token.Value[{0}]: {1} \n", i, (string)tokenList[i]);                    
                    /* (EnQueue)
                    Program.strWriter.WriteLine(numCurrLine);
                    */
                    //
                    streamWriter.WriteLine(numCurrLine);
                    //
                    //3.i)
                    bEnqueue = false;
                    do
                    {
                        lock (((ICollection)this.tokenStringQueue).SyncRoot) //lineQueueSync.SyncRoot obSync
                        {
                            if (this.tokenStringQueue.Count < Program.len_tokenStringQueue)
                            {
                                this.tokenStringQueue.Enqueue(numCurrLine);
                                ////streamWriter.WriteLine(numCurrLine);
                                bEnqueue = true;
                            }
                            else
                            {
                                Thread.Sleep(0);
                                continue;
                            }
                        }

                    } while (!bEnqueue);
                }
                //go to 1)
                start_lineCount = curr_lineCount;

            }//while (!Program.strReader.EndOfStream)
            
            /* (EnQueue)
            Program.strWriter.WriteLine("#");
            */
            //
            streamWriter.WriteLine("#");
            //

            //(Program.strReader.EndOfStream == true)
            bEnqueue = false;
            do
            {
                lock (((ICollection)this.tokenStringQueue).SyncRoot) //lineQueueSync.SyncRoot obSync
                {
                    if (this.tokenStringQueue.Count < Program.len_tokenStringQueue)
                    {
                        this.tokenStringQueue.Enqueue("#");
                        ////streamWriter.WriteLine("#");
                        bEnqueue = true;
                    }
                    else
                    {
                        Thread.Sleep(0);
                        continue;
                    }
                }
            } while (!bEnqueue);

            streamWriter.Close();
        }//Scanner

        static ArrayList GetTokens(string line, string REGEXPR)
        {
            ArrayList res = new ArrayList();

            Regex rgx = new Regex(REGEXPR);
            string[] groupNames = rgx.GetGroupNames();
            ///*
            //            int lenGroupNames = groupNames.GetLength(0);
            //            Console.WriteLine("\n<{0}> is a groupNames.GetLength(0).\n", groupNames.GetLength(0));
            //            for (int i = 0; i < lenGroupNames; i++)
            //                Console.WriteLine(" groupNames[{0}] is <{1}>.\n", i, groupNames[i]);
            //*/

            Match token = rgx.Match(line);

            string str_delimiter = "";
            string tail = "";

            int prevPos = 0;
            int currPos = 0;

            while (true)
            {
                if (!token.Success)
                {

                    tail = line.Substring(currPos);
                    Regex rgxTail = new Regex(TAIL);
                    Match tokTail = rgxTail.Match(tail);
                    if (tokTail.Success)
                    {
                        res.Add(string.Format("<{0}> : <{1}> : ({2},{3})", "tail", tokTail.Value, currPos, tokTail.Length));
                    }
                    else
                    {
                        res.Add(string.Format("<{0}> : <{1}> : ({2},{3})", "no_tail", tail, currPos, tail.Length));

                    }
                    break;

                }
                else //token.Success skipping white-space delimiter before token
                {
                    //token.Captures[1].Value;
                    //token.Captures[1].Index;
                    //token.Captures[1].Length;

                    //token.Groups.Count;          

                    //token.Groups[1].Value;
                    //token.Groups[1].Index;
                    //token.Groups[1].Length;
                    //token.Groups[1].Success;

                    prevPos = currPos;
                    currPos = token.Index;

                    str_delimiter = line.Substring(prevPos, currPos - prevPos);
                    Regex rgxDelim = new Regex(DELIM);
                    Match tokDelim = rgxDelim.Match(str_delimiter);
                    if (tokDelim.Success)
                    {
                        res.Add(string.Format("<{0}> : <{1}> : ({2},{3})", "delim", tokDelim.Value, prevPos, tokDelim.Length));
                    }


                    for (int n = 1; n < token.Groups.Count; n++)
                        if (token.Groups[n].Success)

                            res.Add(string.Format("<{0}> : <{1}> : ({2},{3},{4})",  //: <{5}>
                                groupNames[n], token.Value, prevPos, token.Index, token.Length));//, str_delimiter
                        else continue;

                    currPos += token.Length;
                    if (!(currPos < line.Length))
                        break;

                    token = token.NextMatch();

                }
            }

            return res;
        }//GetTokens

    }
}
