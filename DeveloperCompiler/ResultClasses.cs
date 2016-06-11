using System;
using System.Collections.Generic;
//
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleFrontEnd
{
    //Classes used to implement Evaluate
    public class Interval
    {
        public BitArray interval;
        public int nc1;
        public int nc2;
        public Interval(char c1, char c2)
        {
            interval = new BitArray(256);////0x10000
            nc1 = c1;
            nc2 = c2;
            if (nc1 <= nc2)
                for (int i = nc1; i <= nc2; i++)
                    interval[i] = true;
            //else Empty => Error Empty
        }
    }

    public class SubsetClass
    {//TODO: SubsetClass as BitArray
        public BitArray subset;
        public SubsetClass() 
        {
            subset = new BitArray(256); ////0x10000
        }
        public SubsetClass(SubsetClass subsetClass)
        {
            //subset = new BitArray(256); ////0x10000
            subset = new BitArray(subsetClass.subset);
        }
        public SubsetClass(BitArray bitArray)
        {
            //subset = new BitArray(256); ////0x10000
            subset = new BitArray(bitArray);
        }
        
        public SubsetClass AddInterval(Interval interval1)
        {
            //TODO: tp test (this.subset).And(interval1.interval)
            SubsetClass resSubsetClass = new SubsetClass();
            resSubsetClass.subset = new BitArray((this.subset).Or(interval1.interval));
            return resSubsetClass;

        }
        public static string CharToString(char ci)
        {
            
            if ((ci < ' ') || (ci > '\x7d'))
                return string.Format("\\x{0:x2}", (int)ci);
            if (ci == ' ')
                return "' '";
            return ci.ToString();
        }
        private int aSubset(int i)
        {
            if(i>= subset.Count)
            return -1;
            if (subset[i])
                return 1;
            else
                return 0;
        }
        public string  subsetToString()
        {
            bool isEmpty = IsEmpty();
            if (isEmpty)
                return "[]";//????
            //Init
            int q = 0;
            int a = aSubset(0);
            int i = 0;
            int l = -1, r = -1;
            string sSubset = "[";
            int subsetCount=subset.Count;
            while (i<= subsetCount)
                switch (q)
                {
                    case 0:
                        if (a == 0)
                        {//(q0,0,q4)
                            i++;
                            q = 4;
                            a = aSubset(i);
                            continue;
                        }
                        else if (a == 1)
                        {//(q0,1,q1)
                            l = r = i;//init:store start
                            i++;
                            q = 1;
                            a = aSubset(i);
                            continue;
                        }
                        else
                        {//(q0,n,q5)
                            //Impossible  break;
                        }
                        break;
                    case 1:
                        if (a == 0)
                        {//(q1,0,q3)
                            //write "l-r" ; CharToString(char ci)
                            if (l == r)
                                //sSubset = sSubset + string.Format("\\0x{0:X}", r);
                                sSubset = sSubset + CharToString((char)r);
                            else if(l<r)
                                sSubset = sSubset + string.Format("\\0x{0:X}-\\0x{1:X}", l, r);//Impossible
                            else
                                sSubset = sSubset + string.Format("\\0x{0:X}-\\0x{1:X}", l, r);//Impossible
                            i++;
                            q = 3;
                            a = aSubset(i);
                            continue;
                        }
                        else if (a == 1)
                        {//(q1,1,q2)
                            r = i;//extend to right
                            i++;
                            q = 2;
                            a = aSubset(i);
                            continue;
                        }
                        else
                        {//(q1,n,q5)
                            //End
                            //write "l-r"
                            if (l == r)
                                //sSubset = sSubset + string.Format("\\0x{0:X}", r);
                                sSubset = sSubset + CharToString((char)r);
                            else if (l < r)
                                sSubset = sSubset + string.Format("\\0x{0:X}-\\0x{1:X}", l, r);//Impossible
                            else
                                sSubset = sSubset + string.Format("\\0x{0:X}-\\0x{1:X}", l, r);//Impossible

                            sSubset = sSubset + "]";
                            i++;
                            break;//End
                        }
                        //break;
                    case 2:
                        if (a == 0)
                        {//(q2,0,q4)
                            //write "l-r"
                            if (l == r)
                                sSubset = sSubset + string.Format("\\0x{0:X}", r);//Impossible
                            else if (l < r)
                             //sSubset = sSubset + string.Format("\\0x{0:X}-\\0x{1:X}", l, r);
                            {
                                sSubset = sSubset + SubsetClass.CharToString((char)l);
                                sSubset = sSubset + "-";
                                sSubset = sSubset + SubsetClass.CharToString((char)r);
                            }
                            else
                                sSubset = sSubset + string.Format("\\0x{0:X}-\\0x{1:X}", l, r);//Impossible
                            i++;
                            q = 4;
                            a = aSubset(i);
                            continue;
                        }
                        else if (a == 1)
                        {//(q2,1,q2)
                            r = i;//extend to right
                            i++;
                            q = 2;
                            a = aSubset(i);
                            continue;
                        }
                        else
                        {//(q2,n,q5)
                            //End
                            //write "l-r"
                            if (l == r)
                                sSubset = sSubset + string.Format("\\0x{0:X}", r);//Impossible
                            else if (l < r)
                                //sSubset = sSubset + string.Format("\\0x{0:X}-\\0x{1:X}", l, r);
                                sSubset = sSubset + CharToString((char)l) + "-" + CharToString((char)r);
                            else
                                sSubset = sSubset + string.Format("\\0x{0:X}-\\0x{1:X}", l, r);//Impossible

                            sSubset = sSubset + "]";
                            i++;
                            break;//End
                        }
                        //break;
                    case 3:
                        if (a == 0)
                        {//(q3,0,q3)Skip 0
                            i++;
                            q = 3;
                            a = aSubset(i);
                            continue;
                        }
                        else if (a == 1)
                        {//(q3,1,q1)
                            l = r = i;//init: store start
                            i++;
                            q = 1;
                            a = aSubset(i);
                            continue;
                        }
                        else
                        {//(q3,n,q5)
                            sSubset = sSubset + "]";
                            i++;
                            break;//End
                        }

                        //break;
                    case 4:
                        if (a == 0)
                        {//(q4,0,q4)Skip 0
                            i++;
                            q = 4;
                            a = aSubset(i);
                            continue;
                        }
                        else if (a == 1)
                        {//(q4,1,q1)
                            l = r = i;//init: store start
                            i++;
                            q = 1;
                            a = aSubset(i);
                            continue;
                        }
                        else
                        {//(q4,n,q5)
                            sSubset = sSubset + "]";
                            i++;
                            break;//End
                        }

                        //break;
                    default: //q==5//Impossible
                        break;

                }
            return sSubset;
        }
        //SubsetClass.subset to streamWriter
        public void ToStreamWrite(StreamWriter streamWriter,bool bEnum)
        {
            int n = 0;
            if (bEnum)
            {
                streamWriter.Write("( ");                
                for (int i = 0; i < subset.Count; i++)
                    if (subset[i])
                    {
                        streamWriter.Write("0x{0:x}:{1},", i, CharToString((char)i));
                        n++;
                    }
                    else
                        continue;
                streamWriter.Write(" )");

                streamWriter.WriteLine("\nsubset.Count :({0})", n);
                return;
            }
            //bEnum == false 
            //////streamWriter.Write("( ");
            //int n = 0;
            for (int i = 0; i < subset.Count; i++)
                if (subset[i])
                {
                   // streamWriter.Write("0x{0:x}:{1},", i, CharToString((char)i));
                    n++;
                }
                else
                    continue;
            string ss = this.subsetToString();
            streamWriter.WriteLine("{0}",ss);
            //////streamWriter.Write(" )");
            streamWriter.WriteLine("subset.Count :({0})", n);
            return;
        }
        public bool IsEmpty()
        {
            bool res = false;
            foreach (bool b in subset)
                res = res || b;
            return !res;
        }
        public int IsFirstChar()
        {
            int res = -1;
            
            for(int i=0; i<subset.Count; i++)
                if(!subset.Get(i))
                    continue;
                else 
                    return i;
            return res;
        }
        public int IsLastChar()
        {
            int res = -1;

            for (int i = subset.Count-1; i >= 0; i--)
                if (!subset.Get(i))
                    continue;
                else
                    return i;
            return res;
        }
        public SubsetClass Intersection(SubsetClass Y)
        {
            SubsetClass res = new SubsetClass();
            res.subset = new BitArray(this.subset);//.And(Y.subset));

            //res.subset = 
                res.subset.And(Y.subset);

            return res;
        }
    }
    public class ClassPart
    {
        public char representChar;
        public SubsetClass subsetClass;

        public ClassPart(char c, SubsetClass subClass)
        {
            representChar = c;
            subsetClass = subClass;
        }


    }
    public class AlphabetPartList
    {
        public SortedList<char, SubsetClass> classPartList;//Private Partition Or Public Partition
        public SortedList<char, List<char>> classPartPresentationList;//Private Alphabet through Public Alphabet if 
        //<classPartList> is of Public Partition

        public int ErrorsCount; // for Partition
        public SortedList<char, string> errorsDiagnostic;
        public bool isComplement;

        public AlphabetPartList()
        {
            classPartList = new SortedList<char, SubsetClass>();
            ErrorsCount = 0;
        }
        public AlphabetPartList(SortedList<char, SubsetClass> classPartList)
        {
            this.classPartList = classPartList; // new SortedList<char, SubsetClass>();
            ErrorsCount = 0;
        }
        public AlphabetPartList AddClassPart(ClassPart classPart)
        {
            if (classPartList.ContainsKey(classPart.representChar))
            {//TODO: Error 1:
                return this;//No Change
            }
            else
            {
                this.classPartList.Add(classPart.representChar, classPart.subsetClass);
                return this;
            }

        }

        //public static List<char> ==> Polish Additive Expression

        private BitArray IsCorrectInclusion()
        {
            BitArray res = new BitArray(classPartList.Count);

            char c;
            SubsetClass subsetClass;

            for (int i = 0; i < classPartList.Count; i++)
            {
                c = classPartList.Keys[i];
                subsetClass = classPartList.Values[i]; //(No Empty) and (c in subsetClass.subset) == res[i]
                if (subsetClass.IsEmpty())
                {
                    res.Set(i, false);
                    continue;
                };
                //not subsetClass.IsEmpty() 
                res.Set(i, subsetClass.subset[(int)c]);
            }

            return res;
        }

        private bool IsCorrectRepresentation()
        {
            bool res = true;
            BitArray c_Subset = IsCorrectInclusion();
            foreach (bool b in c_Subset)
                res = res && b;
            return res;

        }

        private bool IsCorrectPartition(out int icp, out int jcp)
        {
            
            bool Is_CorrectRepresentation = IsCorrectRepresentation();
            if (!Is_CorrectRepresentation)
            {
                icp = jcp = -1;
                return false;
            };
            //Is_CorrectRepresentation == true
            for (int i = 0; i < classPartList.Count; i++)
                for (int j = i + 1; j < classPartList.Count; j++)
                    if (classPartList.Values[i].Intersection(classPartList.Values[j]).IsEmpty())
                        continue;
                    else
                    {
                        icp = i;
                        jcp = j;
                        return false;
                    }
            icp = jcp = classPartList.Count;
            return true;

        }

        public bool BuildComplement()
        {
            BitArray Union = new BitArray(classPartList.Values[0].subset);
            char c0 = classPartList.Keys[0];
            foreach (SubsetClass set in classPartList.Values)
                Union.Or(set.subset);
            Union.Not();
            if (!Union[0])
                return false;

            SubsetClass complement = new SubsetClass();
            complement.subset.Or(Union);

            //Add(0,complement)
            if (classPartList.ContainsKey('\x00'))
            {//TODO: Error :
                return false;
            }
            else
            {
               // this.classPartList.Add('\x00', complement);
                this.classPartList.Add('\xFF', complement);
                return true;
            }           
           
        }

        
        ////Here are realized Verification, detailed Diagnostics, storing Errors and Show of
        ////the field-object SortedList<char, SubsetClass> AlphabetPartList.classPartList
       

        public SortedList<char, string> ErrorsDiagnostic()
        {
            //Assert_2: Verify (IsCorrectInclusion()):
            //For All (c,S)(S is not Empty) and (c in S)<==> (c in S)

            BitArray Is_CorrectInclusion = IsCorrectInclusion();

            //Assert_3: Verify (IsCorrectRepresentation()): IsCorrectInclusion For Each (cj,Sj)

            bool Is_CorrectRepresentation = IsCorrectRepresentation();

            //Assert_4: Verify (IsCorrectPartition(out icp, out jcp)) : 
            //IsCorrectRepresentation() and For All (ci,Si),(cj,Sj),i<j (Si && Sj)is Empty

            int icp;
            int jcp;
            bool Is_CorrectPartition = IsCorrectPartition(out icp, out jcp);

            //5: Begin  Applying detailed diagnostics for each (ci,Si)

            char c;
            SubsetClass subsetClass;

            ErrorsCount = 0;

            SortedList<char, string> errorsDiagnostic = new SortedList<char, string>();

            for (int i = 0; i < classPartList.Count; i++)
            {
                c = classPartList.Keys[i];
                subsetClass = classPartList.Values[i];

                //2) Diagnostics(ci,Si)

                if (Is_CorrectRepresentation && Is_CorrectPartition)
                    continue;
                if (Is_CorrectRepresentation && !Is_CorrectPartition && (i == icp))
                {
                    //(I)Store and Count Errors
                    errorsDiagnostic.Add(c, string.Format("Error: Is_CorrectRepresentation and not Is_CorrectPartition at ({0},{1})", icp, jcp));
                    ErrorsCount++;
                    continue;
                }
                if (Is_CorrectRepresentation && !Is_CorrectPartition && (i != icp))
                    continue;
                if (!Is_CorrectRepresentation && Is_CorrectInclusion[i])
                    continue;
                if (!Is_CorrectRepresentation && !Is_CorrectInclusion[i])
                {
                    //(II)Store and  Count Errors
                    errorsDiagnostic.Add(c, string.Format("Error: not Is_CorrectRepresentation, not Is_CorrectInclusion"));
                    ErrorsCount++;
                    continue;
                }

            }

            return errorsDiagnostic;
        }

        private void ShowAlphabetClass(StreamWriter streamWriter,int i, char c, SubsetClass subsetClass)
        {
            streamWriter.WriteLine();

            //1) Show (ci,Si)
            //Show (ci)
            streamWriter.Write("Class({0}) : {1} = ", i, SubsetClass.CharToString(c));
/*
            if ((int)c != 0)
                //streamWriter.Write(" Class({0,3:d}): {1}=", i, c);
                streamWriter.Write(" Class({0,3:d}): {1}=", i, SubsetClass.CharToString(c));
            else
                streamWriter.Write(" Class({0,3:d}): {1}=", i, SubsetClass.CharToString(c));// "0x00"
*/
            //Show (Si)
            subsetClass.ToStreamWrite(streamWriter,false );//true
            //////streamWriter.WriteLine(); 

        }
        
        public void ShowClassPartList(StreamWriter streamWriter, SortedList<char, string> errorsDiagnostic)
        {
            char c;
            SubsetClass subsetClass;

            for (int i = 0; i < classPartList.Count; i++)
            {

                c = classPartList.Keys[i];
                subsetClass = classPartList.Values[i];

                //1) Show (ci,Si)
                ShowAlphabetClass(streamWriter, i, c, subsetClass);

                //2)Show Diagnostics(ci,Si)
                if (errorsDiagnostic == null)
                    continue;

                if (errorsDiagnostic.ContainsKey(c))
                {
                    string sErr;
                    if (errorsDiagnostic.TryGetValue(c, out sErr))
                        streamWriter.WriteLine(sErr);
                    else
                        streamWriter.WriteLine("errorsDiagnostic.TryGetValue(c,out sErr)");
                };
            }

            streamWriter.WriteLine("*****");

        }

        //Verifying and Build Complement and Streaming the Context

        public void ShowContextComplementToStream(StreamWriter streamWriter)
        {
            ////this==AlphabetPartList
            ////this.ErrorsCount includes Diagnostic Errors
            ////this.isComplement

            ////this.errorsDiagnostic is result of this.ErrorsDiagnostic
            ////
           

            streamWriter.WriteLine("Alphabet Partion List");

            //Assert_1: Verify (classPartList.Count == 0)
            if (classPartList.Count == 0)
            {
                streamWriter.WriteLine("Error:classPartList.Count == 0");
                return;
            }

            streamWriter.WriteLine("classPartList.Count={0}", classPartList.Count);
            //errorsDiagnostic is ready
            //////    Build Complement if ErrorCount==0
            //==== bool isComplement;
            if (ErrorsCount == 0)
            {
                //isComplement = BuildComplement(); is ready

                //With or Without Complement the Context passes as correct one!!!
                //==Complement is ready at this time!!!
                ShowClassPartList(streamWriter, errorsDiagnostic);

                if (isComplement)
                    streamWriter.WriteLine("Complement built");
                else
                    streamWriter.WriteLine("No Complement built"); //if some error was

            }
            else //ErrorsCount > 0
            {
                ShowClassPartList(streamWriter, errorsDiagnostic);

                streamWriter.WriteLine("No Complement as there are Errors");
            }

            if (ErrorsCount == 0)
            {
                streamWriter.WriteLine(" Alphabet Partion List as a context of the Named Expression is correct!");
                streamWriter.WriteLine(" Partition Errors ({0})", ErrorsCount);

            }
            else
            {
                streamWriter.WriteLine(" Alphabet Partion List as a context of the Named Expression is not correct:");
                streamWriter.WriteLine(" Partition Errors ({0})", ErrorsCount);

            }
        }
        public void ShowPublicContextToStream(StreamWriter streamWriter)
        {
            //For Public Alphabet Partition List as <Result of Intersection> of Private Alphabet Partition List
            //this==AlphabetPartList            
            //this.classPartList is for <Result of Intersection> of Private Alphabet Partition List
            //this.classPartList.Count > 0
            //this.ErrorsCount == 0 including Diagnostic Errors
            //this.errorsDiagnostic == null 
            //this.isComplement = false;

            streamWriter.WriteLine("Public Alphabet Partion List");

            //Assert_1: Verify (classPartList.Count == 0)
            if (classPartList.Count == 0)
            {
                streamWriter.WriteLine("Error:classPartList.Count == 0");
                return;
            }

            streamWriter.WriteLine("classPartList.Count={0}", classPartList.Count);
            //errorsDiagnostic is ready
            //////    Build Complement if ErrorCount==0
            //==== bool isComplement;
            if (ErrorsCount == 0)
            {
                //isComplement = BuildComplement(); is ready

                //With or Without Complement the Context passes as correct one!!!
                //==Complement is ready at this time!!!
                ShowClassPartList(streamWriter, errorsDiagnostic);

                //////if (isComplement)
                //////    streamWriter.WriteLine("Complement built");
                //////else
                //////    streamWriter.WriteLine("No Complement built"); //if some error was

            }
            else //ErrorsCount > 0
            {
                ShowClassPartList(streamWriter, errorsDiagnostic);

                streamWriter.WriteLine("No Complement as there are Errors");
            }

            if (ErrorsCount == 0)
            {
                streamWriter.WriteLine(" Public Alphabet Partion List as a context of the Named Expressions is correct!");
                streamWriter.WriteLine(" Partition Errors ({0})", ErrorsCount);

            }
            else
            {
                streamWriter.WriteLine(" Public Alphabet Partion List as a context of the Named Expressions is not correct:");
                streamWriter.WriteLine(" Partition Errors ({0})", ErrorsCount);

            }
        }

        public void VerifyContextComplementStream(StreamWriter streamWriter)
        {

            streamWriter.WriteLine("Alphabet Partion List");

            //Assert_1: Verify (classPartList.Count == 0)

            if (classPartList.Count == 0)
            {
                //Impossible as Parser detects this "errors". See 
                //ErrorsCount = 1;//and isComplement==false!!!
                streamWriter.WriteLine("Error:classPartList.Count == 0");
                return;
            }

            streamWriter.WriteLine("classPartList.Count={0}", classPartList.Count);

//////1) First to verify
            ////SortedList<char, string> 
                errorsDiagnostic = ErrorsDiagnostic();
         
////// Second to stream
//////2)    Build Complement if ErrorCount==0
           //==== bool isComplement;
            if (ErrorsCount == 0)
            {
                isComplement = BuildComplement();

                //With or Without Complement the Context passes as correct one!!!
                ShowClassPartList(streamWriter, errorsDiagnostic);               

                if (isComplement)                
                    streamWriter.WriteLine("Complement built");                
                else                
                    streamWriter.WriteLine("No Complement built");                

            }
            else //ErrorsCount > 0
            {
                ShowClassPartList(streamWriter, errorsDiagnostic);  

                streamWriter.WriteLine("\nNo Complement as there are Errors");
            }

            if (ErrorsCount == 0)
            {
                streamWriter.WriteLine(" Alphabet Partion List as a context of the Named Expression is correct!");
                streamWriter.WriteLine(" Partition Errors ({0})", ErrorsCount);

            }
            else
            {
                streamWriter.WriteLine(" Alphabet Partion List as a context of the Named Expression is not correct:");
                streamWriter.WriteLine(" Partition Errors ({0})", ErrorsCount);

            }
            //(III) Summarize(End of) Diagnostics For field (SortedList<char, SubsetClass> classPartList) of
            //this object of AlphabetPartList 
               
        }

        public void GetPrivateAlphabetPresentation(AlphabetPartList infAlphabetPartList)
        {
            this.classPartPresentationList = new SortedList<char, List<char>>(5);
            for (int i = 0; i < this.classPartList.Count; i++)
            {
                char cPrivate = this.classPartList.Keys[i];
                SubsetClass c_subsetClassPrivate = this.classPartList.Values[i];

                List<char> cPrivateThrouthPublicCharList = new List<char>(10);

                for (int j = 0; j < infAlphabetPartList.classPartList.Count; j++)
                {
                    char cPublic = infAlphabetPartList.classPartList.Keys[j];
                    //infAlphabetPartList.classPartList;
                    bool b_PublicInPrivate = c_subsetClassPrivate.subset.Get((int)cPublic);
                    if (!b_PublicInPrivate)
                        continue;
                    //cPublic is in cPrivate 
                    cPrivateThrouthPublicCharList.Add(cPublic);
                }
                this.classPartPresentationList.Add(cPrivate, cPrivateThrouthPublicCharList);
            }


        }

        public void PrintPrivateAlphabetPresentation(StreamWriter streamWriter)
        {
            for (int i = 0; i < this.classPartPresentationList.Count; i++)
            {
                char cPrivate = this.classPartPresentationList.Keys[i];
                List<char> cPublicList = this.classPartPresentationList.Values[i];
                string cPrivateString = SubsetClass.CharToString(cPrivate);
                string additiveExprInPublic = "";
                for (int j = 0; j < (cPublicList.Count - 1); j++)
                    additiveExprInPublic = additiveExprInPublic + (SubsetClass.CharToString(cPublicList[j]) + "|");
                additiveExprInPublic = additiveExprInPublic + (SubsetClass.CharToString(cPublicList[cPublicList.Count - 1]) + ";");
                streamWriter.WriteLine("Class({0}) : {1} = {2}", i, cPrivateString, additiveExprInPublic);
            }
        }

    }

    public class NamedExp //Result of Transformation :Expression -> (id,Free Polish)
    {
        public string idExpr;       
        public ExpPolish expPolish;

        // diagrammForExpPolish ==> machinePrivate 
        public Diagramm diagrammForExpPolish = null;//There is no Automaton in Private Alphabet
        public Automaton machinePrivate = null;
        //

        // diagrammForExpPolishPublic ==> machinePublic 
        public Diagramm diagrammForExpPolishPublic = null;
        public Automaton machinePublic = null;//For <diagrammForExpPolishPublic>. This automaton has to be minimizated!
        //

        public uint ContextErrors = 0;

        public NamedExp(string idExpr, ExpPolish expressionPolish)
        {
            this.idExpr = idExpr;
            this.expPolish = expressionPolish;
        }

        //Only Streaming
        public void ToStreamFreePolish(StreamWriter streamWriter)
        {
            ////this.ContextErrors

            //streamWriter.WriteLine("\nNamed Expression\n"); //Old
            streamWriter.WriteLine("Polish of Named Expression");
            streamWriter.WriteLine(idExpr + " = ");

            foreach (string sDiagramm in expPolish.polishExp)
                streamWriter.WriteLine("<" + sDiagramm + ">"); //\t" +
            //streamWriter.WriteLine("\nNo Diagnostics  as the Context is not Correct!");
        }

        //Only Streaming
        public void ToStreamFreePolishDiagramm(StreamWriter streamWriterValueAST)
        {
           // streamWriterValueAST.WriteLine("\nDiagramm of Named Expression\n");//Old
            streamWriterValueAST.WriteLine("Diagramm in Private Alphabet of Named Expression");
            streamWriterValueAST.WriteLine(idExpr + " = ");
            diagrammForExpPolish.PrintStream(streamWriterValueAST);
            
            //streamWriter.WriteLine("\nNo Diagnostics  as the Context is not Correct!");
        }

        public void ToStreamFreePolishDiagrammPublic(bool b_Succedded_i,StreamWriter streamWriter)
        {
            // streamWriter.WriteLine("\nDiagramm in Public Alphabet of Named Expression\n");//Old
            streamWriter.WriteLine("Diagramm in Public Alphabet of Named Expression");
            streamWriter.WriteLine(idExpr + " = ");
            if (b_Succedded_i)
                this.diagrammForExpPolishPublic.PrintStream(streamWriter);
            else
                streamWriter.WriteLine("Not succeded!");

            //streamWriter.WriteLine("\nNo Diagnostics  as the Context is not Correct!");
        }
        //TODO
        public void ToStreamFreePolishDiagrammAutomatonPublic(bool b_Succedded_Mi, StreamWriter streamWriter)
        {
            //this.machinePublic;
            //this.machinePublic.go;
            //this.machinePublic.Final;
            ////this.machinePublic.;
            streamWriter.WriteLine("Automaton in Public Alphabet of Named Expression");
            streamWriter.WriteLine(idExpr + " = ");
            this.machinePublic.PrintGo(streamWriter);
            this.machinePublic.PrintFinals(streamWriter);
            this.machinePublic.PrintActiveErrors(streamWriter);
            this.machinePublic.PrintFGo(streamWriter);
        }
        public void ToStreamFreePolishDiagrammAutomatonPrivate(bool b_Succedded_Mi, StreamWriter streamWriter)
        {
            //this.machinePrivate;
            //this.machinePrivate.go;
            //this.machinePrivate.Final;
            ////this.machinePrivate.;
            streamWriter.WriteLine("Automaton in Private Alphabet of Named Expression");
            streamWriter.WriteLine(idExpr + " = ");
            this.machinePrivate.PrintGo(streamWriter);
            this.machinePrivate.PrintFinals(streamWriter);
            this.machinePrivate.PrintActiveErrors(streamWriter);
            this.machinePrivate.PrintFGo(streamWriter);
        }


        private bool IsOperatorOrNullString(string token)
        {
            if (token.Length == 0) //|| (token.Length > 1))
                return true;
            //token.Length >= 1)
            if ((token == "Star") || (token == "Join") || (token == "Concat"))
                return true;
            //(token.Length >= 1)
            return false;

        }

        //Verifying in the Correct Context and Streaming
        public string  VerifyFreePolishInContextStream(StreamWriter streamWriter,AlphabetPartList alphabetPartList)
        {
            streamWriter.WriteLine("Named Expression");

            streamWriter.WriteLine(idExpr + " = ");

            foreach (string simpleDiagrammToken in expPolish.polishExp)
            {
                //1)Diagnostics of "operators" or ""
                if (IsOperatorOrNullString(simpleDiagrammToken))
                {
                    streamWriter.WriteLine("\t" + "<" + simpleDiagrammToken + ">");
                    continue;

                };

                //2)Diagnostics of "a"
                if (simpleDiagrammToken.Length == 1)
                {
                    if (alphabetPartList.classPartList.Keys.Contains(simpleDiagrammToken[0]))
                    {
                        //Symbol is in Context
                        streamWriter.WriteLine("\t" + "<" + simpleDiagrammToken + ">");
                        continue;
                    }
                    else
                    {
                        //Symbol is not in Context
                        streamWriter.WriteLine("\t" + "<" + simpleDiagrammToken + "> <-- is no symbol");
                        ContextErrors++;
                        continue;

                    }

                };
                //(simpleDiagrammToken.Length > 1)

                //3)Diagnostics of "a...b"

                //(I)Show simpleDiagrammToken
                streamWriter.WriteLine("\t" + "<" + simpleDiagrammToken + ">");

                for (int i = 0; i < simpleDiagrammToken.Length; i++)
                    if (alphabetPartList.classPartList.Keys.Contains(simpleDiagrammToken[i]))
                    {    
                        //Symbol is in Context
                        continue;
                    }
                    else
                    {
                        //Symbol is not in Context

                        //(II)Show Error Diagnostic
                        streamWriter.WriteLine("\t-->" + "<" + simpleDiagrammToken[i] + "> is no symbol");
                        ContextErrors++;
                        continue;

                    }

            }

            if (ContextErrors == 0)
            {
                streamWriter.WriteLine(" Named Expression is Correct in the Correct Context!");
                streamWriter.WriteLine(" Context Errors ({0})", ContextErrors);

            }
            else
            {
                streamWriter.WriteLine(" Named Expression is not Correct in the Correct Context:");
                streamWriter.WriteLine(" Context Errors ({0})", ContextErrors);

            }
            return  string.Format("{0:d5}", ContextErrors);
        }

    }
    //Here Verifying and Streaming
    public class AlphabetPartNamedExp
    {
        public AlphabetPartList infAlphabetPartList; //Public Context
        public AlphabetPartList alphabetPartList; //Private Context of namedExp
        public NamedExp namedExp; //intermediate free Polish form of a source expression

        //ctor
        //First PASS to get alphabetPartList as Local
        //Store alphabetPartList as Local
        //Verify alphabetPartList as Local
        //Load alphabetPartList as Local
        //Second PASS to emit CIL-code to evaluate NamedExp namedExp as Local in Context of alphabetPartList
        //Third PASS to transform NamedExp namedExp
        //Or
        // Second+Third -> Second
        public AlphabetPartNamedExp(AlphabetPartList alphabetPartList, NamedExp namedExp)
        {   //TODO?: Verify(AlphabetPartList alphabetPartList)
            this.alphabetPartList = alphabetPartList;
            //TODO?: Verify(AlphabetPartList alphabetPartList,NamedExp namedExp )
            this.namedExp = namedExp;
        }

        public string VerifyContextAndPolish(StreamWriter streamWriter)
        {
            ////bool isComplement;
            alphabetPartList.VerifyContextComplementStream(streamWriter);
            //AlphabetPartList alphabetPartList has been COMPLETED for streaming only,so it was verifyed
           
            //////Build Complement if AlphabetPartList.ErrorCount==0 
            ////if (alphabetPartList.ErrorsCount == 0)
            ////    isComplement = alphabetPartList.BuildComplement();

            string boathErrors;
            string strContextErrors;
            if (alphabetPartList.ErrorsCount == 0)
            {
                strContextErrors = namedExp.VerifyFreePolishInContextStream(streamWriter, alphabetPartList);
                //NamedExp namedExp has been verifyed, so it is  COMPLETED for streaming only,so it was verifyed
                boathErrors = string.Format("{0:d5}", 0) + strContextErrors;
            }
            else//alphabetPartList.ErrorsCount > 0
            {
                namedExp.ToStreamFreePolish(streamWriter);
                //NamedExp namedExp has not been verifyed, so it is not COMPLETED for streaming only,so it was not verifyed
                boathErrors = string.Format("{0:d5}{1:d5}", alphabetPartList.ErrorsCount,0);
            }
            return boathErrors;
        }

        public void ToStreamDiagrammInPublicPartition(bool b_Succedded_i, StreamWriter streamWriter)
        {
            streamWriter.WriteLine("Diagramm in Public Alphabet of Named Expression");
            streamWriter.WriteLine(this.namedExp.idExpr + " = ");
            if (b_Succedded_i)
            {
                this.namedExp.diagrammForExpPolishPublic.PrintStream(streamWriter);
                //Public Partition To streamWriter
                streamWriter.WriteLine();
                streamWriter.WriteLine("Public Partition");
                this.infAlphabetPartList.ShowClassPartList(streamWriter, null);//null for SortedList<char, string> errorsDiagnostic


            }
            else
                streamWriter.WriteLine("Not succeded!");

            //streamWriter.WriteLine("\nNo Diagnostics  as the Context is not Correct!");
        }

        public void ToStreamAutomatonInPrivatePartition(bool b_Succedded_Mi, StreamWriter streamWriter)
        {
            //this.machinePrivate;
            //this.machinePrivate.go;
            //this.machinePrivate.Final;
            ////this.machinePrivate.;
            streamWriter.WriteLine("Automaton in Private Alphabet of Named Expression");
            streamWriter.WriteLine(this.namedExp.idExpr + " = ");
            this.namedExp.machinePrivate.PrintGo(streamWriter);

            this.namedExp.machinePrivate.PrintFinals(streamWriter);
            this.namedExp.machinePrivate.PrintActiveErrors(streamWriter);
            this.namedExp.machinePrivate.PrintFGo(streamWriter);
            //Private Partition To streamWriter
            streamWriter.WriteLine();
            streamWriter.WriteLine("Private Partition");
            this.alphabetPartList.ShowClassPartList(streamWriter, null);//null for SortedList<char, string> errorsDiagnostic
        }
        public void ToStreamAutomatonInPublicPartition(bool b_Succedded_Mi, StreamWriter streamWriter)
        {
            //this.machinePublic;
            //this.machinePublic.go;
            //this.machinePublic.Final;
            ////this.machinePublic.;
            streamWriter.WriteLine("Automaton in Public Alphabet of Named Expression");
            streamWriter.WriteLine(this.namedExp.idExpr + " = ");
            this.namedExp.machinePublic.PrintGo(streamWriter);
            this.namedExp.machinePublic.PrintFinals(streamWriter);
            this.namedExp.machinePublic.PrintActiveErrors(streamWriter);
            this.namedExp.machinePublic.PrintFGo(streamWriter);
            //Public Partition To streamWriter
            streamWriter.WriteLine();
            streamWriter.WriteLine("Public Partition");
            this.infAlphabetPartList.ShowClassPartList(streamWriter, null);//null for SortedList<char, string> errorsDiagnostic

        }

    }

    public class RegularExp
    {
        public string nameRegularExp;
        public AlphabetPartNamedExp alphabetPartNamedExp;
        public RegularExp(string nameRegularExp, AlphabetPartNamedExp alphabetPartNamedExp)
        {
            this.nameRegularExp = nameRegularExp;
            this.alphabetPartNamedExp = alphabetPartNamedExp;
        }
        
        //TODO
        public bool GetAutomatonFromDiagrammPublic()
        {
            bool bSucceded = true;
            Automaton automaton = new Automaton(alphabetPartNamedExp.namedExp.idExpr,
                                                alphabetPartNamedExp.namedExp.diagrammForExpPolishPublic,
                //Program.regularExpList.infAlphabetPartList.classPartList);

            alphabetPartNamedExp.infAlphabetPartList.classPartList);

            automaton.GetGo();
            int n=automaton.GetFinals();
            automaton.GetActive();
            automaton.GetErrors();
            automaton.GetFGo();

            alphabetPartNamedExp.namedExp.machinePublic = automaton;

            bSucceded = n > 0;
            //Here we have to construct minimal machinePublic
            return bSucceded;

        }

        //TODO       
        public bool GetAutomatonFromDiagrammPrivate()
        {
            bool bSucceded = true;
            Automaton automaton = new Automaton(alphabetPartNamedExp.namedExp.idExpr,
                                                alphabetPartNamedExp.namedExp.diagrammForExpPolish,//Private Diagramm
                                                alphabetPartNamedExp.alphabetPartList.classPartList);
                
            //Program.regularExpList.infAlphabetPartList.classPartList);
            
            automaton.GetGo();
            int n = automaton.GetFinals();
            automaton.GetActive();
            automaton.GetErrors();
            automaton.GetFGo();

            //alphabetPartNamedExp.namedExp.machinePublic = automaton;
            alphabetPartNamedExp.namedExp.machinePrivate = automaton;

            bSucceded = n > 0;
            //Here we have to construct minimal machinePublic
            return bSucceded;

        }


        public bool GetDiagrammPublic()
        {
            bool bSucceded = true;
            //1)
            //////RegularExp regularExp_i = this.regularExpList.Values[i];

            //////string nameRegularExp_i = regularExp_i.nameRegularExp;
            string nameRegularExp_i = this.nameRegularExp;

            //////AlphabetPartNamedExp alphabetPartNamedExp_i = regularExp_i.alphabetPartNamedExp;           
            //////AlphabetPartNamedExp alphabetPartNamedExp_i = this.regularExpList.Values[i].alphabetPartNamedExp;

            AlphabetPartNamedExp alphabetPartNamedExp_i = this.alphabetPartNamedExp;
            //2)
            AlphabetPartList alphabetPartListExp_i = alphabetPartNamedExp_i.alphabetPartList;
            NamedExp namedExp_i = alphabetPartNamedExp_i.namedExp;

            //

            //Express   Private Partition           throuth     Pablic Partition:
            //          Abstract Private Alphabet   through     Abstract Public Alphabet

            //Given: AlphabetPartList alphabetPartListExp_i
            //alphabetPartListExp_i.classPartList
            //      SortedList<char, SubsetClass> classPartList;
            //Private Partition is ready

            //this.infAlphabetPartList; 
            ////int n = this.IntersectionForAll();
            //Public Partition is ready

            //alphabetPartListExp_i.classPartPresentationList
            ////Done: alphabetPartList_Expr_i.GetPrivateAlphabetPresentation(this.infAlphabetPartList);
            //     SortedList<char, List<char>> classPartPresentationList;
            //Abstract Private Alphabet   through     Abstract Public Alphabet is ready

            //Given:  NamedExp namedExp_i
            /*
                            namedExp_i.idExpr;
                            namedExp_i.expPolish;//on Private
                            namedExp_i.diagrammForExpPolish;//on Private
                            namedExp_i.ToStreamFreePolish;
                            namedExp_i.ToStreamFreePolishDiagramm;
            */
            //TO DO:
            // namedExp_i.diagrammForExpPolish;//on Private
            //    ==> namedExp_i.diagrammForExpPolishPublic;//on Public
            // public bool RegularExp regularExp_i.GetDiagrammPublic();
            //

            Diagramm diagrammNamedExp_i = namedExp_i.diagrammForExpPolish;
            Diagramm diagrammNamedExp_i_Public = new Diagramm();         //namedExp_i.diagrammForExpPolish;
            SortedList<char, List<char>> privateExp_iOnPublicList = alphabetPartListExp_i.classPartPresentationList;

            diagrammNamedExp_i_Public.q_end = diagrammNamedExp_i.q_end;

            foreach (string edge in diagrammNamedExp_i.edgeList)
            {
                if (edge.Length == 10)
                    diagrammNamedExp_i_Public.edgeList.Add(edge);
                else//edge.Length != 10 so it is not lambda-edge
                {
                    char c_Private = char.Parse(edge.Substring(10));
                    bool is_c_Private = privateExp_iOnPublicList.ContainsKey(c_Private);
                    List<char> publicCharList_c_Private = null;

                    if (is_c_Private)//c_Private in Keys
                    {                        
                        privateExp_iOnPublicList.TryGetValue(c_Private, out publicCharList_c_Private);
                        int count_List=publicCharList_c_Private.Count;
                        string newEdge="";
                        if (count_List > 0)
                            for (int i = 0; i < count_List; i++)
                            {
                                newEdge = string.Format("{0}{1}", edge.Substring(0, 10), publicCharList_c_Private[i].ToString());
                                diagrammNamedExp_i_Public.edgeList.Add(newEdge);
                            }
                        else//count_List <= 0
                        {
                            //NOTHING
                            //diagrammNamedExp_i_Public.edgeList.Add(newEdge1);
                            bSucceded = false;
                        }
                    }
                    else//c_Private is not in Keys
                    {
                        bSucceded = false;
                    }


                }

            }
            namedExp_i.diagrammForExpPolishPublic = diagrammNamedExp_i_Public;//Diagramm in Abstract Public Alphabet
            
            return bSucceded;
        }
    }
    //
    //Root
    //
    public class RegularExpList //Result of Evaluate
    {
        public AlphabetPartList infAlphabetPartList; //Public Context
        /// <summary>
        /// infAlphabetPartList is inf of <regularExpList> that is Intersection of All Partitions in <regularExpList>
        /// </summary>
        
        public SortedList<string, RegularExp> regularExpList;

        public Cartesian cartesian;

        public RegularExpList()
        {
            regularExpList = new SortedList<string, RegularExp>();
        }
        public RegularExpList AddRegularExp(RegularExp regularExp)
        {
            if (regularExpList.ContainsKey(regularExp.nameRegularExp))
            {//TODO: Error 2 Repeated "regular" Name
                return this;// Not Changed
            }
            else
            {
                this.regularExpList.Add(regularExp.nameRegularExp, regularExp);
                return this;
            }

        }
  
        //After Run_Verification_RegularExpListToStream is succeseded the free polish expression is correct in the correct Context,
        //so there is no necessity to verify the Polish in the Context.
        
        //Advanced Solution:
        //But the Context is generally needed for deriving operators which operate on the Context!!!
        //Here we could construct inialization operators for all subclasses of ASCII-alphabet.
        //It is <<int array[256] Partition = {...,nSymbol,...},where <<nSymbol>> is <<classPartList.Keys.IndexOf(Symbol)>>
        //So Partition[(int)inputChar] == nClassOfInputChar can be used in Automation_Diagramm to recognize input lexeme!!!
        //The edge (p,a,q) derived is (p,nClassOfInputChar,q)!
        //In that solution we have to derive type Scanner with method Scane with body
        //Constructor Partition (Context)
        //and Constructor RDiagramm(Diagramm)
        //and Constructor Automation_Diagramm (Partition,RDiagramm) and
        //operator-method Automation_Diagramm.Do (string inputStreamOfChar) !

        public void Print_Ready_Verified_RegularExpList_ContextWithDiagnostics_Polish_Diagram(string currDir)
        {
            //(bool bVerificationSucceded)
            //bVerificationSucceded  ==>AST_Value_Input ... ==> Do Public Alphabet ==>Diagramm and Automaton in Public ==> Cartesian Product 
            //!bVerificationSucceded ==>AST_Value_Input ...
            string nameRegExp;
            StreamWriter streamWriter = null;

            int partitionErrors; //==AlphabetPartNamedExp.regularExpList.Values[i].alphabetPartNamedExp.alphabetPartList.ErrorsCount
            int contextErrors; //= (int)alphabetPartNamedExp_i.namedExp.ContextErrors;   
            //string strBoathErrors;

            for (int i = 0; i < this.regularExpList.Count; i++)
            {
                nameRegExp = this.regularExpList.Values[i].nameRegularExp; //good solution

                if (!Directory.Exists(currDir + "/" + nameRegExp))
                    Directory.CreateDirectory(currDir + "/" + nameRegExp);

                AlphabetPartNamedExp alphabetPartNamedExp_i = this.regularExpList.Values[i].alphabetPartNamedExp;

                //1)===Context Verification is ready

                partitionErrors = alphabetPartNamedExp_i.alphabetPartList.ErrorsCount;
                //alphabetPartNamedExp_i.alphabetPartList.isComplement;
                //alphabetPartNamedExp_i.alphabetPartList.errorsDiagnostic;

                #region Get the file _Partition.txt
                string fileTypeNamePart = currDir + "/" + (nameRegExp + "\\") + (nameRegExp + "_Partition.txt");
                File.Delete(fileTypeNamePart);
                streamWriter = File.CreateText(fileTypeNamePart);

                alphabetPartNamedExp_i.alphabetPartList.ShowContextComplementToStream(streamWriter);
                streamWriter.Close();
                
                #endregion

                //2)===Polish Verification against Context is ready

                ////polishExpression is ready as free polish expression if (partitionErrors > 0)
                contextErrors = (int)alphabetPartNamedExp_i.namedExp.ContextErrors;

                #region Get the file _Polish.txt
                string fileTypeNamePolish = currDir + "/" + (nameRegExp + "\\") + (nameRegExp + "_Polish.txt");
                File.Delete(fileTypeNamePolish);
                streamWriter = File.CreateText(fileTypeNamePolish);
                alphabetPartNamedExp_i.namedExp.ToStreamFreePolish(streamWriter);
                bool bSucceded_Exp_i = (partitionErrors == 0) && (contextErrors == 0);
                if (bSucceded_Exp_i)
                {
                    streamWriter.WriteLine();
                    streamWriter.WriteLine("Private Partition");
                    alphabetPartNamedExp_i.alphabetPartList.ShowClassPartList(streamWriter, null);

                }
                else
                {
                    streamWriter.WriteLine();
                    streamWriter.WriteLine("Private Partition");
                    streamWriter.WriteLine("Partition Errors:({0})", partitionErrors);//fileTypeNamePart
                    streamWriter.WriteLine("Look into the file :<{0}>", fileTypeNamePart);
                }
                streamWriter.Close();
                
                #endregion

                //3)===Diagramm is ready

                #region Get the file _Diagramm.txt
                string fileTypeNameDiagramm = currDir + "/" + (nameRegExp + "\\") + (nameRegExp + "_Diagramm.txt");
                File.Delete(fileTypeNameDiagramm);
                streamWriter = File.CreateText(fileTypeNameDiagramm);
                alphabetPartNamedExp_i.namedExp.ToStreamFreePolishDiagramm(streamWriter);
                if (bSucceded_Exp_i)
                {
                    streamWriter.WriteLine();
                    streamWriter.WriteLine("Private Partition");
                    alphabetPartNamedExp_i.alphabetPartList.ShowClassPartList(streamWriter, null);
                }
                else
                {
                    streamWriter.WriteLine();
                    streamWriter.WriteLine("Private Partition");
                    streamWriter.WriteLine("Partition Errors:({0})", partitionErrors);//fileTypeNamePart
                    streamWriter.WriteLine("Look into the file :<{0}>", fileTypeNamePart);
                }
                streamWriter.Close();
            }
            
                #endregion
        }
  
        public void Get_IntersectionAndRepresentationPrivateAlphabetsInPublic(string currDir)
        {

            //if (!b_succeded)
            //{
            //    File.Delete("Intersection" + "_Partition.txt");
            //    return;
            //}
            //b_succeded

            //(1) Get this.infAlphabetPartList;

            int n = this.IntersectionForAll();

            // ==>  this.infAlphabetPartList;
            //==>this.infAlphabetPartList.classPartList; //RESULT is Intersection of All Partitons!!!
            //==> this.infAlphabetPartList.classPartPresentationList==null; 

            //(2) Get Representation Private Alphabet Partition through this.infAlphabetPartList
            StreamWriter streamWriter = null;
            string fileInfAlphabetPartList = currDir + "/" + "Intersection" + "_Partition.txt";
            File.Delete(fileInfAlphabetPartList);
            streamWriter = File.CreateText(fileInfAlphabetPartList);

            //Public Partition this.infAlphabetPartList ==> streamWriter
            //this.infAlphabetPartList.ShowContextComplementToStream(streamWriter);//Old
            this.infAlphabetPartList.ShowPublicContextToStream(streamWriter);

            for (int i = 0; i < this.regularExpList.Count; i++)
            {

                string nameRegExp_i = this.regularExpList.Values[i].nameRegularExp; //good solution              

                AlphabetPartNamedExp alphabetPartNamedExp_i = this.regularExpList.Values[i].alphabetPartNamedExp;

                alphabetPartNamedExp_i.infAlphabetPartList = this.infAlphabetPartList;//Public Partition Add to i_Exp

                AlphabetPartList alphabetPartList_Expr_i = alphabetPartNamedExp_i.alphabetPartList;//Get Private Partition
                //Express   Private Partition           throuth     Pablic Partition:
                //          Abstract Private Alphabet   through     Abstract Public Alphabet
                alphabetPartList_Expr_i.GetPrivateAlphabetPresentation(this.infAlphabetPartList);

                streamWriter.WriteLine();
                streamWriter.WriteLine("Private Alphabet Presentation for Regular Expression <{0}>", nameRegExp_i);
                alphabetPartList_Expr_i.PrintPrivateAlphabetPresentation(streamWriter);

                //SortedList<char, List<char>> alphabetPartition_Expr_i_PresentationList = alphabetPartList_Expr_i.classPartPresentationList;

            }
            //
            streamWriter.Close(); //for string fileInfAlphabetPartList = "Intersection" + "_Partition.txt";

        }

        public void Get_RegularExpList_Diagramm_Public(string currDir)
        {
            //Given:
            //b_succeded ==>    
            //this.infAlphabetPartList; //Public Partition is ready
            //==>this.infAlphabetPartList.classPartList; //RESULT is Intersection of All Partitons!!!
            //==> this.infAlphabetPartList.classPartPresentationList==null; 
            //
            StreamWriter streamWriter;
            for (int i = 0; i < this.regularExpList.Count; i++)
            {
                //1)
                RegularExp regularExp_i = this.regularExpList.Values[i];

                string nameRegularExp_i = regularExp_i.nameRegularExp;

                //2)Get Diagramm in Public Alphabet
                bool b_Succedded_i = regularExp_i.GetDiagrammPublic();

                //3)===Public Diagramm is ready

                string fileTypeNameDiagrammPublic = currDir + "/" + (nameRegularExp_i + "\\") + (nameRegularExp_i + "_Public_Diagramm.txt");
                File.Delete(fileTypeNameDiagrammPublic);
                streamWriter = File.CreateText(fileTypeNameDiagrammPublic);

                //regularExp_i.alphabetPartNamedExp.namedExp.ToStreamFreePolishDiagrammPublic(b_Succedded_i, streamWriter);
                regularExp_i.alphabetPartNamedExp.ToStreamDiagrammInPublicPartition(b_Succedded_i, streamWriter);
                //////alphabetPartNamedExp_i.namedExp.ToStreamFreePolishDiagramm(streamWriter);
                streamWriter.Close();

            }
        }

        public void Get_RegularExpList_Automaton_Public(string currDir)
        {
            //So GET AUTOMATA in Abstract Public(COMMON) Alphabet!!!
            // And Construct Cartesian Product of all automata!!!
            //Using:
            //SortedList<char,SubsetClass> this.infAlphabetPartList.classPartList;//Public(COMMON) Partition is ready
            //So 
            //int i_c_char = this.infAlphabetPartList.classPartList.IndexOfKey(c_char);
            //i_c_char is a Number of c_char in Public Alphabet:
            //c_char is a Name of Abstract Symbol;
            //int n = this.infAlphabetPartList.classPartList.Count;
            // n is Cardinality of Abstract Public Alphabet
            //
            StreamWriter streamWriter;
            for (int i = 0; i < this.regularExpList.Count; i++)
            {
                //1)
                RegularExp regularExp_i = this.regularExpList.Values[i];
                string nameRegularExp_i = regularExp_i.nameRegularExp;

                //2)
                ///// bool b_Succedded_i = regularExp_i.GetDiagrammPublic();
                bool b_Succedded_Mi;
                //
                b_Succedded_Mi = regularExp_i.GetAutomatonFromDiagrammPublic();
                //
                //
                //3)===Public Automaton from Diagramm is ready
                //
                //Here Minimal Automaton has to be constructed!
                //
                string fileTypeNameGetAutomatonFromDiagrammPublic = currDir + "/" + (nameRegularExp_i + "\\") + (nameRegularExp_i + "_Public_Automaton.txt");
                File.Delete(fileTypeNameGetAutomatonFromDiagrammPublic);
                streamWriter = File.CreateText(fileTypeNameGetAutomatonFromDiagrammPublic);

                //regularExp_i.alphabetPartNamedExp.namedExp.ToStreamFreePolishDiagrammAutomatonPublic(b_Succedded_Mi, streamWriter);
                regularExp_i.alphabetPartNamedExp.ToStreamAutomatonInPublicPartition(b_Succedded_Mi, streamWriter);

                //////alphabetPartNamedExp_i.namedExp.ToStreamFreePolishDiagramm(streamWriter);
                streamWriter.Close();

            }
        }

        public void Get_RegularExpList_Automaton_Private(string currDir)
        {
            //So GET AUTOMATA in Abstract Public(COMMON) Alphabet!!!
            // And Construct Cartesian Product of all automata!!!
            //Using:
            //SortedList<char,SubsetClass> this.infAlphabetPartList.classPartList;//Public(COMMON) Partition is ready
            //So 
            //int i_c_char = this.infAlphabetPartList.classPartList.IndexOfKey(c_char);
            //i_c_char is a Number of c_char in Public Alphabet:
            //c_char is a Name of Abstract Symbol;
            //int n = this.infAlphabetPartList.classPartList.Count;
            // n is Cardinality of Abstract Public Alphabet
            //
            StreamWriter streamWriter;
            for (int i = 0; i < this.regularExpList.Count; i++)
            {
                //1)
                RegularExp regularExp_i = this.regularExpList.Values[i];
                string nameRegularExp_i = regularExp_i.nameRegularExp;

                //2)
               
                bool b_Succedded_Mi;
                //
                b_Succedded_Mi = regularExp_i.GetAutomatonFromDiagrammPrivate();
                //
                //
                //3)===Private Automaton from Private Diagramm is ready
                //
                //Here Minimal Automaton has to be constructed!
                //
                string fileTypeNameGetAutomatonFromDiagrammPrivate = currDir + "/" + (nameRegularExp_i + "\\") + (nameRegularExp_i + "_Private_Automaton.txt");
                File.Delete(fileTypeNameGetAutomatonFromDiagrammPrivate);
                streamWriter = File.CreateText(fileTypeNameGetAutomatonFromDiagrammPrivate);

                //regularExp_i.alphabetPartNamedExp.namedExp.ToStreamFreePolishDiagrammAutomatonPrivate(b_Succedded_Mi, streamWriter);
               
                regularExp_i.alphabetPartNamedExp.ToStreamAutomatonInPrivatePartition(b_Succedded_Mi, streamWriter);
                //////alphabetPartNamedExp_i.namedExp.ToStreamFreePolishDiagramm(streamWriter);
                streamWriter.Close();

            }
        }


        public void Clean_RegularExpList_Diagramm_Automaton_PrivateAndPublic(string currDir)
        {
            
            ////StreamWriter streamWriter;
            for (int i = 0; i < this.regularExpList.Count; i++)
            {
                //1)
                RegularExp regularExp_i = this.regularExpList.Values[i];
                string nameRegularExp_i = regularExp_i.nameRegularExp;

                //1.1)===Private Automaton from Private Diagramm is ready
                string fileTypeNameAutomatonPrivate= currDir + "/" + (nameRegularExp_i + "\\") + (nameRegularExp_i + "_Private_Automaton.txt");
                File.Delete(fileTypeNameAutomatonPrivate);
                //2)
                ///// bool b_Succedded_i = regularExp_i.GetDiagrammPublic();
                ////bool b_Succedded_Mi;
                ////b_Succedded_Mi = regularExp_i.GetAutomatonFromDiagrammPublic();

                //3)===Public Diagramm is ready

                string fileTypeNameDiagrammPublic = currDir + "/" + (nameRegularExp_i + "\\") + (nameRegularExp_i + "_Public_Diagramm.txt");
                File.Delete(fileTypeNameDiagrammPublic);


                //3)===Public Automaton from Public Diagramm is ready

                string fileTypeNameGetAutomatonFromDiagrammPublic = currDir + "/" + (nameRegularExp_i + "\\") + (nameRegularExp_i + "_Public_Automaton.txt");
                File.Delete(fileTypeNameGetAutomatonFromDiagrammPublic);
                ////streamWriter = File.CreateText(fileTypeNameGetAutomatonFromDiagrammPublic);

                ////regularExp_i.alphabetPartNamedExp.namedExp.ToStreamFreePolishDiagrammAutomatonPublic(b_Succedded_Mi, streamWriter);

                //////alphabetPartNamedExp_i.namedExp.ToStreamFreePolishDiagramm(streamWriter);
                ////streamWriter.Close();

            }
        }

        public void Get_RegularExpList_Target(string currDir)
        {

            //bVerificationSucceded

            //
            //1.1)At this point Context, Polish and Diagramm in Context have been passed over succesfully!
            //So we can construct Automaton in Private Alphabet(==Private Context)            
            this.Get_RegularExpList_Automaton_Private(currDir);
            //

            //2)
            this.Get_IntersectionAndRepresentationPrivateAlphabetsInPublic(currDir);
            //
            //////(3) Transform Polish Expression in Private Alphabet to New One in Public Alphabet using
            ////// substitution a --> e(a)!
            //////(3)Get New Diagramm from New Polish Expression in in Public Alphabet using 
            //////class GeneratorPolishCIL: 
            //////see class RegularExpList
            //////      RegularExpList.Run_Verification_RegularExpListToStream(StreamWriter streamWriter)
            //
            //(3)Get New Diagramm in Public Alphabet from Diagramm in Private Alphabet 
            //   using substitution a --> e(a):
            this.Get_RegularExpList_Diagramm_Public(currDir);
            //
            //4) Diagramms in Abstract Public Alphabet for all Input Expressions are Ready!!!
            //So GET AUTOMATA in Abstract Public(COMMON) Alphabet!!!
            //
            this.Get_RegularExpList_Automaton_Public(currDir);
            //
            // And Construct Cartesian Product of all automata if it is OK!!!           
        }

        public bool Run_Verification_RegularExpListToStream(StreamWriter streamWriterValueAST)
        {
            bool bVerificationSucceded = true;
            //this.regularExpList.            
            streamWriterValueAST.WriteLine("Result of Verification of all Expressions:");
            //int partitionErrors, contextErrors;

            List<string> polishExpression = null;

            string nameRegularExp_i;//TypeName

            string strBoathErrors;
            for (int i = 0; i < this.regularExpList.Count; i++)     
            {
                nameRegularExp_i = this.regularExpList.Values[i].nameRegularExp; //good solution
                streamWriterValueAST.WriteLine("Name Regular Expression: {0}", nameRegularExp_i);

                AlphabetPartNamedExp alphabetPartNamedExp_i = this.regularExpList.Values[i].alphabetPartNamedExp;
                //
                //1)Verify  Context and Polish Expression in Correct Partition(Context)
                //
                strBoathErrors = alphabetPartNamedExp_i.VerifyContextAndPolish(streamWriterValueAST);
                //
              int partitionErrors    = alphabetPartNamedExp_i.alphabetPartList.ErrorsCount;
              int contextErrors = (int)alphabetPartNamedExp_i.namedExp.ContextErrors;                            
              polishExpression = alphabetPartNamedExp_i.namedExp.expPolish.polishExp;

              bool isSave = (partitionErrors == 0) && (contextErrors == 0);
             
               bVerificationSucceded = bVerificationSucceded && isSave;

               //2)Create type  valuePolishCIL: valuePolish to Get Free Diagramm from Free Polish
                //
               ValuePolish valuePolish = CreatorValuePolishCIL.CreateValuePolish(nameRegularExp_i, polishExpression, isSave);
                //
               //3)Get Free Diagramm from Free Polish
                //
               Diagramm diagramm_valuePolish = valuePolish.Evaluate();               
               alphabetPartNamedExp_i.namedExp.diagrammForExpPolish = diagramm_valuePolish; //Store instance of Diagramm
               
                //RESULT of valuePolish.Evaluate() IS Free Diagramm from Free Polish(Correct)
                //                
               //4)Free Diagramm to Stream
                //
               alphabetPartNamedExp_i.namedExp.ToStreamFreePolishDiagramm(streamWriterValueAST);//Stream instance of Diagramm
                //
               //Summary for alphabetPartNamedExp_i
                //
               streamWriterValueAST.WriteLine("Summary");
               streamWriterValueAST.WriteLine("Partition Errors: {0}", strBoathErrors.Substring(0, 5));
               streamWriterValueAST.WriteLine("Context Errors: {0}", strBoathErrors.Substring(5));

               // streamWriter.WriteLine("partitionErrors: {0}", partitionErrors);
               //streamWriter.WriteLine("contextErrors: {0}", contextErrors);

               streamWriterValueAST.WriteLine("*****");

            }//for

            return bVerificationSucceded;
        }

        public static AlphabetPartList IntersectionTwoAlphabetPart(AlphabetPartList P1,AlphabetPartList P2)
        {
            AlphabetPartList R12=new AlphabetPartList();
            ////SortedList <char,SubsetClass> R12.classPartList is Empty List
            //SubsetClass sr_SubsetClass=new SubsetClass();
            ////BitArray sr_SubsetClass.subset is Empty SubsetClass
            //ClassPart r_ClassPart = new ClassPart('c',sr_SubsetClass);//With c in SubsetClass
            ////r_ClassPart is never Empty!!!
            //R12.AddClassPart(r_ClassPart);//to R12.classPartList

            for (int i = 0; i < P1.classPartList.Count; i++)
            {
                SubsetClass P1_i = P1.classPartList.Values[i];

                for(int j=0; j< P2.classPartList.Count; j++)
                {
                    SubsetClass P2_j = P2.classPartList.Values[j];
                    SubsetClass Qij=P1_i.Intersection(P2_j);
                    if(Qij.IsEmpty())
                        continue;
                    //Qij is not Empty! So get c in Qij

                    //int i_min = Qij.IsFirstChar();
                    int i_min = Qij.IsLastChar();
                    //256 > i_min >=0
                    ClassPart r_Qij = new ClassPart((char)i_min, Qij);
                    R12.AddClassPart(r_Qij);
                    continue ;                    
                }

            }

            return R12;
        }
       
        public int IntersectionForAll()
        {
            int n = this.regularExpList.Count;
            if (n == 1)
            {
                this.infAlphabetPartList = this.regularExpList.Values[0].alphabetPartNamedExp.alphabetPartList;
                return 1;
            }
            //n>=2
            this.infAlphabetPartList = RegularExpList.IntersectionTwoAlphabetPart(this.regularExpList.Values[0].alphabetPartNamedExp.alphabetPartList,
                this.regularExpList.Values[1].alphabetPartNamedExp.alphabetPartList);

            for (int i = 3; i < n; i++)
            {
                this.infAlphabetPartList = RegularExpList.IntersectionTwoAlphabetPart(this.infAlphabetPartList,
               this.regularExpList.Values[i].alphabetPartNamedExp.alphabetPartList);
            }
            return n;
        }

        public bool AreSeparated()
        {
            return this.cartesian.bNoIntersection;

        }
       
        public void GetCartesianProduct(string currDir)
        {
            //Get Cartesian Product
            int n = this.regularExpList.Count;
            List<Automaton> automatonList = new List<Automaton>(n);
            for (int i = 0; i < n; i++)
            {
                Automaton machine_i = this.regularExpList.Values[i].alphabetPartNamedExp.namedExp.machinePublic;
                automatonList.Add(machine_i);
            }

            this.cartesian = new Cartesian(automatonList, this.infAlphabetPartList.classPartList);

            cartesian.GetCgo();
            cartesian.GetCFinals();
            //Separated Automata

            cartesian.GetCActive();
            cartesian.GetCErrors();
            cartesian.GetFCgo();

            string fileCartesian = currDir + "/Cartesian_Automaton.txt";
            File.Delete(fileCartesian);
            StreamWriter streamWriter = File.CreateText(fileCartesian);

            //regularExp_i.alphabetPartNamedExp.namedExp.ToStreamFreePolishDiagrammAutomatonPublic(b_Succedded_Mi, streamWriter);
            this.cartesian.ToStreamCartesian(streamWriter);
            
            streamWriter.Close();
        }
    }

    public class ExpPolish
    {
        public List<string> polishExp; // in Private Alphabet
        //////public List<string> polishExpInPublic;

        //////public void PolishPrivateToPublic(AlphabetPartList alphabetPartList)
        //////{
        //////    //Input:
        //////    //this.polishExp , SortedList<char,List<char>> alphabetPartList.classPartPresentationList
        //////    //Output:
        //////    // a) <char c,List<char> listChar> ==> <string cString, List<string> additivePolish>
        //////    // b) polishExp ==>(<string cString, List<string> additivePolish>)==>polishExpInPublic
        //////}
        
        //a)generate NamedExp storing context <<AlphabetPartList alphabetPartList;>> 
        //as local object Evaluate; 
        //b)generate Diagramm after Evaluate has calculated <<the object of type RegularExpList>>
        //using the verified context <<AlphabetPartList alphabetPartList;>> and the polish free form of <<ExpPolish>>
        //Case b) is preferred because of using a special transforming thread 
        //(the polish free form ExpPolish to Diagramm!!!
        //There is no back links with source code. They are necessery when errors encounter by
        //traversing AST to emit Evaluate , evaluating RegularExpList or verifying and transforming 
        //ExpPolish(RegularExpList) to Diagramm (DiagrammList)!!!

        //Case b) corresponds the later binding of the polish free form of expression with the context!!!
        //Case a) is the early binding of the polish free form of expression with the context!!!

        //Errors can be stored in ErrorStream for reporting 
        //at the end of the process of transforming <<source code>> to DiagrammList or AutomationList or
        //object of type <<Regex>> as that implemented in the ctor of class <<Regex>>!!!

        //Constructor : char c --> string c.ToString()
        //c
        //Load alphabetPartList as Local
        public ExpPolish(char c)
        {
            polishExp = new List<string>();
            polishExp.Add(c.ToString());
        }
        //Constructor : string s --> D
        //s
        //Load alphabetPartList as Local 
        //if there is object,  we have no object but emitter of it
        public ExpPolish(string s)
        {
            polishExp = new List<string>();
            polishExp.Add(s);
        }
        //Constructor :   D "Star"-->D
        public ExpPolish Star()
        {
            this.polishExp.Add("Star");
            return this;
        }
        //Constructor :   D T "Concat"-->D
        public ExpPolish Concat(ExpPolish T)
        {
            this.polishExp.AddRange(T.polishExp);
            this.polishExp.Add("Concat");
            return this;
        }
        //Constructor :   D U "Join" --> D 
        public ExpPolish Join(ExpPolish U)
        {
            this.polishExp.AddRange(U.polishExp);
            this.polishExp.Add("Join");
            return this;
        }
    }

    //Diagramm.edgeList in Private 
    //==>(AlphabetPartList RegularExpList[i].RegularExp.AlphabetPartNamedExp.alphabetPartList )
    //==> Diagramm.edgeListPublic 
    public class Diagramm
    {
        public int q_end;
        public List<string> edgeList;
        public Diagramm()
        {
            q_end = 0;
            edgeList = new List<string>();
        }
        public Diagramm(Diagramm d)
        {
            q_end = d.q_end;
            edgeList = new List<string>(d.edgeList);
        }


        //...,SortedList<char, SubsetClass> classPartList ,string sSymbols|--> Diagramm(classPartList, sSymbols)
        public Diagramm(string sSymbols)
        {
            edgeList = new List<string>();
            if (sSymbols.Length < 2)
            {
                edgeList.Add(string.Format("{0:d5}{1:d5}{2}", 0, 1, sSymbols));
                q_end = 1;
                //edgeList.Sort();
                return;
            };
            //(str.Length >= 2)
            //char ci;

            for (int i = 0; i < sSymbols.Length; i++)
            {
                //ci = sSymbols[i];
                //(p,ci,q)=(i,"ci",i-1)
                edgeList.Add(string.Format("{0:d5}{1:d5}{2}", i, i + 1, sSymbols[i].ToString()));

            }
            edgeList.Sort();
            q_end = sSymbols.Length;
        }

        public void ConsolePrint()
        {
            foreach (string sEdge in edgeList)
                Console.WriteLine("({0},{1},{2})",
                    sEdge.Substring(0, 5),
                    sEdge.Length == 10 ? "\"\"" : sEdge.Substring(10),
                    sEdge.Substring(5, 5));

        }
        public void PrintStream(StreamWriter streamWriter)
        {
            foreach (string sEdge in edgeList)
                streamWriter.WriteLine("({0},{1},{2})",
                    sEdge.Substring(0, 5),
                    sEdge.Length == 10 ? "\"\"" : sEdge.Substring(10),
                    sEdge.Substring(5, 5));

        }

        public Diagramm Concat(Diagramm d)
        {
            //this * d -> this
            int p, q;
            //string sa;

            //int q_endThis = this.q_end;

            foreach (string sDedge in d.edgeList)
            {
                p = int.Parse(sDedge.Substring(0, 5));
                q = int.Parse(sDedge.Substring(5, 5));
                //sa = sDedge.Substring(10);
                this.edgeList.Add(string.Format("{0:d5}{1:d5}{2}",
                    this.q_end + p,
                    this.q_end + q,
                    sDedge.Substring(10)));

            }
            this.q_end = this.q_end + d.q_end;
            return this;
        }

        // this.edgeList.Sort();?
        public Diagramm Join(Diagramm d)
        {
            //this | d -> this
            int p, q;
            string sEdge;
            //int q_endThis = this.q_end;

            int q_end_Join = this.q_end + d.q_end - 1;
            //Modify this.edgeList
            for (int i = 0; i < this.edgeList.Count; i++)
            {
                sEdge = this.edgeList[i];
                p = int.Parse(sEdge.Substring(0, 5));
                q = int.Parse(sEdge.Substring(5, 5));
                if (q == this.q_end)
                {
                    //this.edgeList[i] = q_end_Join;
                    this.edgeList[i] = string.Format("{0:d5}{1:d5}{2}",
                    p,
                    q_end_Join,
                    sEdge.Substring(10));
                }
            }
            //this.edgeList modified
            //Get,Modify and Add <<sDedge>> to <<this.edgeList>>
            foreach (string sDedge in d.edgeList)
            {
                p = int.Parse(sDedge.Substring(0, 5));
                q = int.Parse(sDedge.Substring(5, 5));
                //sa = sDedge.Substring(10);

                if ((p == 0) && (q != d.q_end))
                {
                    this.edgeList.Add(string.Format("{0:d5}{1:d5}{2}",
                       p,//0
                        this.q_end - 1 + q,//q+n-1-1
                        sDedge.Substring(10)));
                };
                if ((p == 0) && (q == d.q_end))
                {
                    this.edgeList.Add(string.Format("{0:d5}{1:d5}{2}",
                       p,//0
                        q_end_Join,//n-1+m-1-1
                        sDedge.Substring(10)));
                };
                if ((p != 0) && (q != d.q_end))
                {
                    this.edgeList.Add(string.Format("{0:d5}{1:d5}{2}",
                       this.q_end - 1 + p,//p+n-1-1
                        this.q_end - 1 + q,//q+n-1-1
                        sDedge.Substring(10)));
                };
                if ((p != 0) && (q == d.q_end))
                {
                    this.edgeList.Add(string.Format("{0:d5}{1:d5}{2}",
                       this.q_end - 1 + p,//p+n-1-1
                        this.q_end - 1 + q,//q+n-1-1
                        sDedge.Substring(10)));
                };


            }

            this.q_end = q_end_Join;
            this.edgeList.Sort();
            return this;
        }

        public Diagramm Star()
        {
            int p, q;
            string sEdge;
            //int q_endThis = this.q_end;

            int q_end_Star = this.q_end + 1;

            //Modify this.edgeList
            for (int i = 0; i < this.edgeList.Count; i++)
            {
                sEdge = this.edgeList[i];
                p = int.Parse(sEdge.Substring(0, 5));
                q = int.Parse(sEdge.Substring(5, 5));
                if (p == 0)
                {
                    //this.edgeList[i] = q_end_Join;
                    this.edgeList[i] = string.Format("{0:d5}{1:d5}{2}",
                    this.q_end,
                    q,
                    sEdge.Substring(10));
                }
            }
            //this.edgeList modified
            //Add (0,"",n-1)
            this.edgeList.Add(string.Format("{0:d5}{1:d5}{2}",
                       0,//0
                        this.q_end,//n-1
                        ""));

            //Add (n-1,"",n)
            this.edgeList.Add(string.Format("{0:d5}{1:d5}{2}",
                        this.q_end,// n-1
                        q_end_Star,//
                        ""));

            this.edgeList.Sort();
            this.q_end = q_end_Star;
            return this;
        }

        //...,SortedList<char, SubsetClass> classPartList ,string sSymbols|--> Diagramm(classPartList, sSymbols)
        //<<SortedList<char, SubsetClass> classPartList>> has be as <<argument>> of EvaluatePolish:
        //Diagramm ValuePolish.EvaluatePolish(SortedList<char, SubsetClass> classPartList) is
        //a method of the abstract class ValuePolish.
        //ValuePolishCIL : ValuePolish is the derived class and ValuePolish is its the base class!!!
        //
    }


}
