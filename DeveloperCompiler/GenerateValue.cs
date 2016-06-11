//#define EXPERIMENTAL
//#undef EXPERIMENTAL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.IO;
//
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;
//
using AST;

namespace ConsoleFrontEnd
{
    public abstract class Value
    {
        //public abstract void Evaluate(StreamWriter strWriter);
        public abstract RegularExpList Evaluate();
    }

    public abstract class Generator
    {
        public object rootAST;
        public ILGenerator ILGenEvaluate;

        public abstract void EmitEvaluate();


    }
    public
    class GeneratorCIL : Generator
    {
        public static string getStringStringFromTokenLine(string tokenLine)
        {
            string stringString = "";
            int iSmc_1 = tokenLine.IndexOf(':');
            string startString = tokenLine.Substring(iSmc_1 + 1).TrimStart(' ', '<');
            int iEnd = startString.IndexOf("\">");

            stringString = startString.Substring(1, iEnd - 1);
            return stringString;

        }


        public static string getStringCharFromTokenLine(string tokenLine)
        {
            string stringChar = "";
            int iSmc_1 = tokenLine.IndexOf(':');
            string startChar = tokenLine.Substring(iSmc_1 + 1).TrimStart(' ', '<');
            int iEnd = startChar.IndexOf("'>");

            stringChar = startChar.Substring(1, iEnd - 1);
            return stringChar;

        }

        public static char StringHex12ToChar(string shex12)
        {
            //shex12 without '...' 

            if (shex12.Length == 1)
                return char.Parse(shex12);

            if (shex12.Length == 0)
                return '\x0';

            if (shex12.Length > 4) //||(shex12.Length == 2))
                return '\x0';
            if (shex12.Length == 2)
            {
                if (shex12[0] == '\\')
                    return shex12[1];
                else
                    return '\x0';
            }

            //3<=shex12.Length<=4

            if ((shex12[0] != '\\') || (char.ToLower(shex12[1]) != 'x'))
                return '\x0';

            string thex12 = shex12.Substring(2).ToLower();
            //1<=thex12.Length<=2
            int h0 = 0;
            if (char.IsDigit(thex12, 0))
                h0 = thex12[0] - '0';
            else if (('a' <= thex12[0]) && (thex12[0] <= 'f'))
                h0 = thex12[0] - 'a' + 10;
            else
                return '\x0';
            if (thex12.Length == 1)
                return (char)h0;
            //thex12.Length == 2
            int h1 = 0;
            if (char.IsDigit(thex12, 1))
                h1 = thex12[1] - '0';
            else if (('a' <= thex12[1]) && (thex12[1] <= 'f'))
                h1 = thex12[1] - 'a' + 10;
            else
                return '\x0';

            return (char)(h0 * 16 + h1);

        }

/*
        public static char StringHex12ToChar(string shex12)
        {

            if (shex12.Length == 1)
                return char.Parse(shex12);

            if (shex12.Length == 0)
                return '\x0';

            if ((shex12.Length > 4) || (shex12.Length == 2))
                return '\x0';

            //3<=shex12.Length<=4

            if ((shex12[0] != '\\') || (char.ToLower(shex12[1]) != 'x'))
                return '\x0';

            string thex12 = shex12.Substring(2).ToLower();
            //1<=thex12.Length<=2
            int h0 = 0;
            if (char.IsDigit(thex12, 0))
                h0 = thex12[0] - '0';
            else if (('a' <= thex12[0]) && (thex12[0] <= 'f'))
                h0 = thex12[0] - 'a' + 10;
            else
                return '\x0';

            if (thex12.Length == 1)
                return (char)h0;

            //thex12.Length == 2
            int h1 = 0;
            if (char.IsDigit(thex12, 1))
                h1 = thex12[1] - '0';
            else if (('a' <= thex12[1]) && (thex12[1] <= 'f'))
                h1 = thex12[1] - 'a' + 10;
            else
                return '\x0';

            return (char)(h0 * 16 + h1); // 0<=c<256

        }
 */
        public override void EmitEvaluate()
        {


            this.DoLeftDerive(rootAST);

            ILGenEvaluate.Emit(OpCodes.Ret);

        } //public override void EmitEvaluate()

        public GeneratorCIL(object rootAST, ILGenerator ILGenEvaluate)
        {
            this.rootAST = rootAST;
            this.ILGenEvaluate = ILGenEvaluate;
        }

        public bool DoLeftDerive(object rootAST)
        {
            if ((rootAST is Terminal) || (rootAST is Production))
            {
                LeftDerive(rootAST);
                //streamWriter.Close();
                return true;
            }
            //streamWriter.Close();
            return false;
        }

        void LeftDerive(object root)
        {
            MethodInfo writeLineMI = typeof(StreamWriter).GetMethod(
                "WriteLine",
                new Type[] { typeof(string) }
                );

            if (root is Terminal) //Never descending to <<root as Terminal>> when traversing AST-tree
            {
                //int terminal_iTA = (root as Terminal).iTA; //0

                
                //string out_terminal = string.Format("{0,3:d}{1}", (root as Terminal).iTA, (root as Terminal).tokenLine);


                ////streamWriter.WriteLine("{0,3:d}{1}", (root as Terminal).iTA, (root as Terminal).tokenLine); //EMPTY token????
                //ILGenEvaluate.Emit(OpCodes.Ldarg_1);
                //ILGenEvaluate.Emit(OpCodes.Ldstr, out_terminal);
                //ILGenEvaluate.Emit(OpCodes.Callvirt, writeLineMI);

                return;
            }
            if (root is Production) //Allways descending to <<root as Production>> when traversing AST-tree
            {

                switch ((root as Production).rule)
                {
                    case 8: EmitRule_8(root as Production);
                        break;
                    case 9: EmitRule_9(root as Production);
                        break;
                    case 0: EmitRule_0(root as Production);
                        break;
                    case 10: EmitRule_10(root as Production);
                        break;
                    case 12: EmitRule_12(root as Production);
                        break;
                    case 16: EmitRule_16(root as Production);
                        break;
                    case 11: EmitRule_11(root as Production);
                        break;
                    case 13: EmitRule_13(root as Production);
                        break;
                    case 17: EmitRule_17(root as Production);
                        break;

                    case 1:
                    case 15: EmitRule_1_15(root as Production);
                        break;
                    case 2:
                    case 14:
                    case 20: EmitRule_2_14_20(root as Production);
                        break;
                    case 19:
                    case 22: EmitRule_19_22(root as Production);
                        break;
                    case 18: EmitRule_18(root as Production);//EmitRule_18 is congruent to EmitRule_21
                        break;
                    case 21: EmitRule_21(root as Production); //EmitRule_3
                        break;
                    case 3: EmitRule_3(root as Production);
                        break;
                    case 6: EmitRule_6(root as Production);
                        break;
                    case 5: EmitRule_5(root as Production);
                        break;
                    case 4: 
                        EmitRule_4(root as Production);
                        break;
                    case 7: EmitRule_7(root as Production);
                        break;

                    default: //Never executing this branch because of handling rule nodes of AST
                        ////string begin_rule = string.Format("<{0,3:d}>", (root as Production).rule);
                        ////string end_rule = string.Format("</{0,3:d}>", (root as Production).rule);

                        //////streamWriter.WriteLine("<{0,3:d}>", (root as Production).rule); //Begin (root as Production).rule
                        ////ILGenEvaluate.Emit(OpCodes.Ldarg_1);
                        ////ILGenEvaluate.Emit(OpCodes.Ldstr, begin_rule);
                        ////ILGenEvaluate.Emit(OpCodes.Callvirt, writeLineMI);

                        ////if ((root as Production).alpha != null)
                        ////{
                        ////    int len_alpha = (root as Production).alpha.GetLength(0);
                        ////    object alpha_i;
                        ////    for (int i = 0; i < len_alpha; i++)
                        ////    {
                        ////        alpha_i = (root as Production).alpha[i];
                        ////        LeftDerive(alpha_i);
                        ////    }
                        ////}

                        //////streamWriter.WriteLine("</{0,3:d}>", (root as Production).rule);//End (root as Production).rule
                        ////ILGenEvaluate.Emit(OpCodes.Ldarg_1);
                        ////ILGenEvaluate.Emit(OpCodes.Ldstr, end_rule);
                        ////ILGenEvaluate.Emit(OpCodes.Callvirt, writeLineMI);
                        break;
                }
                return;
            }

        }



        void EmitRule_9(Production root)
        {
            //9: I -> c1-c2,  Elem(c1) Elem(c2) NewObjInterval             
            //9==root.rule

            string tc1 = (root.alpha[0] as Terminal).tokenLine;
            string tc2 = (root.alpha[2] as Terminal).tokenLine;
/*
            string[] arrTStr = tc1.Split(':');
            string s_c1 = arrTStr[1].Trim().Trim('<', '>', '\''); //, '\''
            arrTStr = tc2.Split(':');
            string s_c2 = arrTStr[1].Trim().Trim('<', '>', '\''); //, '\''
*/
            string s_c1 = getStringCharFromTokenLine(tc1);
            string s_c2 = getStringCharFromTokenLine(tc2);

            MethodInfo StringHex12ToCharMI = typeof(GeneratorCIL).GetMethod(
               "StringHex12ToChar", new Type[] { typeof(string) }
               );

          
            Type[] IntervalCtorParams = new Type[] { typeof(char), typeof(char) };
            Type IntervalType = Type.GetType("ConsoleFrontEnd.Interval");
            ConstructorInfo IntervalCtor = IntervalType.GetConstructor(IntervalCtorParams);

            ILGenEvaluate.Emit(OpCodes.Ldstr, s_c1);
            ILGenEvaluate.EmitCall(OpCodes.Call, StringHex12ToCharMI, null);
            ILGenEvaluate.Emit(OpCodes.Ldstr, s_c2);
            ILGenEvaluate.EmitCall(OpCodes.Call, StringHex12ToCharMI, null);            
            ILGenEvaluate.Emit(OpCodes.Newobj, IntervalCtor);

        }// void EmitRule_9(Production root)

        void EmitRule_8(Production root)
        {
            ////            //8: I -> c1,  Elem(c1) Dup NewObjInterval             
            ////            //8==root.rule
            string tc1 = (root.alpha[0] as Terminal).tokenLine;
/*
            string[] arrTStr ;//= tc1.Split(':');
            string s_c1;// = arrTStr[1].Trim().Trim('<', '>', '\''); //, '\''

            int i_clmn=tc1.IndexOf(':');
            int i_less = i_clmn+tc1.Substring(i_clmn).IndexOf('<');
            int i_greater;
            char c_greater = tc1[i_less+2];
            if (c_greater == '>')
                s_c1 = tc1.Substring(i_less + 1, 1);
            else
            {
                i_greater = i_less + 2 + tc1.Substring(i_less + 2).IndexOf('>');
                s_c1 = tc1.Substring(i_less, i_greater - i_less + 1).Trim('<', '>', '\''); //, '\''
            }
*/
            string s_c1 = getStringCharFromTokenLine(tc1);

            MethodInfo StringHex12ToCharMI = typeof(GeneratorCIL).GetMethod(
               "StringHex12ToChar", new Type[] { typeof(string) }
               );
            Type[] IntervalCtorParams = new Type[] { typeof(char), typeof(char) };
            Type IntervalType = Type.GetType("ConsoleFrontEnd.Interval");
            ConstructorInfo IntervalCtor = IntervalType.GetConstructor(IntervalCtorParams);

            ILGenEvaluate.Emit(OpCodes.Ldstr, s_c1);
            ILGenEvaluate.EmitCall(OpCodes.Call, StringHex12ToCharMI, null);
            ILGenEvaluate.Emit(OpCodes.Dup);
            ILGenEvaluate.Emit(OpCodes.Newobj, IntervalCtor);

        }//void EmitRule_8(Production root)

        void EmitRule_0(Production root)
        {
            //0: <> -> L,  L //L-RegularExpList->              
            //0==root.rule   

            LeftDerive(root.alpha[0]);
            //GenVerifyRun(L-RegularExpList)-> (true,Diagramm)|(false,ErrorList)
            //L-RegularExpList==RegularExpList is Result!!! 

        }//     void EmitRule_0(Production root)

        void EmitRule_10(Production root)
        {
            //10: L -> E, NewObjRegularExpList E AddRegularExp 
            //10==root.rule
           
            Type RegularExpListType = Type.GetType("ConsoleFrontEnd.RegularExpList");
            ConstructorInfo RegularExpListCtor = RegularExpListType.GetConstructor(new Type[0]);
            ILGenEvaluate.Emit(OpCodes.Newobj, RegularExpListCtor);//NewObjRegularExpList

            LeftDerive(root.alpha[0]); //E
           
            MethodInfo AddRegularExpMI = RegularExpListType.GetMethod(
               "AddRegularExp", new Type[] { typeof(RegularExp) }
               );
            ILGenEvaluate.Emit(OpCodes.Call, AddRegularExpMI); //AddRegularExp

        }//void EmitRule_10(Production root)

        void EmitRule_12(Production root)
        {
            //12: P -> C, NewObjAlphabetPartList C AddClassPart 

            Type AlphabetPartListType = Type.GetType("ConsoleFrontEnd.AlphabetPartList");
            ConstructorInfo AlphabetPartListCtor = AlphabetPartListType.GetConstructor(new Type[0]);
            ILGenEvaluate.Emit(OpCodes.Newobj, AlphabetPartListCtor);//NewObjAlphabetPartList

            LeftDerive(root.alpha[0]); //C

            MethodInfo AddClassPartMI = AlphabetPartListType.GetMethod(
              "AddClassPart", new Type[] { typeof(ClassPart) }
              );
            ILGenEvaluate.Emit(OpCodes.Call, AddClassPartMI); //AddClassPart

        }//void EmitRule_12(Production root)

        void EmitRule_16(Production root)
        {

            //16: S -> I,  NewObjSubsetClass I AddInterval 

            Type SubsetClassType = Type.GetType("ConsoleFrontEnd.SubsetClass");
            ConstructorInfo SubsetClassCtor = SubsetClassType.GetConstructor(new Type[0]);
            ILGenEvaluate.Emit(OpCodes.Newobj, SubsetClassCtor);//NewObjSubsetClass           
         
            LeftDerive(root.alpha[0]);// I

            MethodInfo AddIntervalMI = SubsetClassType.GetMethod(
            "AddInterval", new Type[] { typeof(Interval) }
            );
            ILGenEvaluate.Emit(OpCodes.Call, AddIntervalMI); //AddInterval

        }//void EmitRule_16(Production root)

        void EmitRule_11(Production root)
        {
            //11: L -> L E , L E AddRegularExp 

            LeftDerive(root.alpha[0]);//L-RegularExpList
            LeftDerive(root.alpha[1]);//E-RegularExp

            Type RegularExpListType = Type.GetType("ConsoleFrontEnd.RegularExpList");
            MethodInfo AddRegularExpMI = RegularExpListType.GetMethod(
             "AddRegularExp", new Type[] { typeof(RegularExp) }
             );
            ILGenEvaluate.Emit(OpCodes.Call, AddRegularExpMI); //AddRegularExp 

        }

        void EmitRule_13(Production root)
        {
            
            //13: P -> P C , P C AddClassToPart 

            LeftDerive(root.alpha[0]);//P-AlphabetPartList
            LeftDerive(root.alpha[1]);//C-ClassPart

            Type AlphabetPartListType = Type.GetType("ConsoleFrontEnd.AlphabetPartList");
            MethodInfo AddClassPartMI = AlphabetPartListType.GetMethod(
             "AddClassPart", new Type[] { typeof(ClassPart) }
             );
            ILGenEvaluate.Emit(OpCodes.Call, AddClassPartMI); //AddClassPart

        }

        void EmitRule_17(Production root)
        {
            
            //17: S -> S,I , S I AddIntervalToSubset             

            LeftDerive(root.alpha[0]); //S-SubsetClass          
            LeftDerive(root.alpha[2]); //I-Interval

            Type SubsetClassType = Type.GetType("ConsoleFrontEnd.SubsetClass");
            MethodInfo AddIntervalMI = SubsetClassType.GetMethod(
           "AddInterval", new Type[] { typeof(Interval) }
           );
            ILGenEvaluate.Emit(OpCodes.Call, AddIntervalMI); //AddInterval

        }

        void EmitRule_1_15(Production root)
        {
            // 1: U -> U * T , U T Concat      
            //15: R -> R | U , R U  Join
            //root.rule in {1,15}

            LeftDerive(root.alpha[0]); //R|U-ExpPolish          
            LeftDerive(root.alpha[2]); //U|T-ExpPolish

            Type ExpDiagrammType = Type.GetType("ConsoleFrontEnd.ExpPolish");
            MethodInfo ConcatMI = ExpDiagrammType.GetMethod(
           "Concat", new Type[] { typeof(ExpPolish) }
           );
            MethodInfo JoinMI = ExpDiagrammType.GetMethod(
          "Join", new Type[] { typeof(ExpPolish) }
          );

           
            if(root.rule == 1)
                ILGenEvaluate.Emit(OpCodes.Call, ConcatMI); //Concat
            else //root.rule == 15
                ILGenEvaluate.Emit(OpCodes.Call, JoinMI); //Join

        }

        void EmitRule_2_14_20(Production root)
        {
            // 2: U -> T , T
            //14: R -> U , U
            //20: T -> (R) , R 
            //root.rule in {10,12,16}
            
            //LeftDerive(root.alpha[0]);
            int iChild = (root.rule == 20) ? 1 : 0;
            LeftDerive(root.alpha[iChild]); //    ExpPolish -> ExpPolish      

        }

        void EmitRule_19_22(Production root)
        {
            //19: T -> {R}, R Star 
            //22: T -> T^ , T Star 
            //root.rule in {19,22}
         
            int iChild = (root.rule == 19) ? 1 : 0;
            LeftDerive(root.alpha[iChild]); //T|R-ExpPolish 

            Type ExpDiagrammType = Type.GetType("ConsoleFrontEnd.ExpPolish");
            MethodInfo StarMI = ExpDiagrammType.GetMethod(
           "Star", new Type[0] 
           );
            ILGenEvaluate.Emit(OpCodes.Call, StarMI); //Star
            
        }

        void EmitRule_18(Production root)
        {
            //18: T -> c,  DiagramChar(c)              
            //18==root.rule

            string tc1 = (root.alpha[0] as Terminal).tokenLine;            
/*       
            string[] arrTStr = tc1.Split(':');
            string s_c1 = arrTStr[1].Trim().Trim('<', '>', '\'');
*/
            string s_c1 = getStringCharFromTokenLine(tc1);

            MethodInfo StringHex12ToCharMI = typeof(GeneratorCIL).GetMethod(
               "StringHex12ToChar", new Type[] { typeof(string) }
               );

            Type[] ExpDiagrammCtorParams = new Type[] { typeof(char)};
            Type ExpDiagrammType = Type.GetType("ConsoleFrontEnd.ExpPolish");
            ConstructorInfo ExpDiagrammCtor = ExpDiagrammType.GetConstructor(ExpDiagrammCtorParams);

            ILGenEvaluate.Emit(OpCodes.Ldstr, s_c1);
            ILGenEvaluate.EmitCall(OpCodes.Call, StringHex12ToCharMI, null);
            ILGenEvaluate.Emit(OpCodes.Newobj, ExpDiagrammCtor);           

        }
        void EmitRule_21(Production root)
        {
            //21: T -> s,  DiagramString(s)              
            //21==root.rule
         
            string tc1 = (root.alpha[0] as Terminal).tokenLine;
/*
            string[] arrTStr = tc1.Split(':');
            string s = arrTStr[1].Trim().Trim('<', '>', '"');
*/
            string s = getStringStringFromTokenLine(tc1);
            Type[] ExpDiagrammCtorParams = new Type[] { typeof(string) };
            Type ExpDiagrammType = Type.GetType("ConsoleFrontEnd.ExpPolish");
            ConstructorInfo ExpDiagrammCtor = ExpDiagrammType.GetConstructor(ExpDiagrammCtorParams);

            ILGenEvaluate.Emit(OpCodes.Ldstr, s);          
            ILGenEvaluate.Emit(OpCodes.Newobj, ExpDiagrammCtor);          

        }
        void EmitRule_3(Production root)
        {
            //3: A -> alphabet P,  P              
            //3==root.rule

            LeftDerive(root.alpha[1]);//P-AlphabetPartList == A-AlphabetPartList     

        }
        void EmitRule_6(Production root)
        {
            //6: D -> expression id =  R. ,  GetName(id) R NewObjNamedExp             
            //6==root.rule        

            string t_id = (root.alpha[1] as Terminal).tokenLine;
            string[] arrTStr = t_id.Split(':');
            string s_id = arrTStr[1].Trim().Trim('<', '>');

            ILGenEvaluate.Emit(OpCodes.Ldstr, s_id);

            LeftDerive(root.alpha[3]);//R-ExpPolish

            Type[] ExpDiagrammCtorParams = new Type[] { typeof(string), typeof(ExpPolish) };
            Type NamedExpType = Type.GetType("ConsoleFrontEnd.NamedExp");
            ConstructorInfo NamedExpCtor = NamedExpType.GetConstructor(ExpDiagrammCtorParams);
            ILGenEvaluate.Emit(OpCodes.Newobj, NamedExpCtor);           

        }

        void EmitRule_5(Production root)
        {//TODO
            //5: C ->  c = [ S ] ; ,  GetElem(c)  S  NewObjClassPart          
            //5==root.rule

            string t_c = (root.alpha[0] as Terminal).tokenLine;
/*            //

            string[] arrTStr = t_c.Split(':');
            string s_c = arrTStr[1].Trim().Trim('<', '>','\'');
            //
*/
            string s_c = getStringCharFromTokenLine(t_c);

            MethodInfo StringHex12ToCharMI = typeof(GeneratorCIL).GetMethod(
               "StringHex12ToChar", new Type[] { typeof(string) }
               );
            ILGenEvaluate.Emit(OpCodes.Ldstr, s_c);
            ILGenEvaluate.EmitCall(OpCodes.Call, StringHex12ToCharMI, null);

            LeftDerive(root.alpha[3]);//S-SubsetClass

            Type[] ClassPartCtorParams = new Type[] { typeof(char), typeof(SubsetClass) };
            Type ClassPartType = Type.GetType("ConsoleFrontEnd.ClassPart");
            ConstructorInfo ClassPartCtor = ClassPartType.GetConstructor(ClassPartCtorParams);
            ILGenEvaluate.Emit(OpCodes.Newobj, ClassPartCtor);

        }

        void EmitRule_4(Production root)
        {
            //4: C -> c = ;,  GetQuest(c) NewObjSubsetClass  NewObjClassPart              
            //4==root.rule
     
            string t_c = (root.alpha[0] as Terminal).tokenLine;
/*
            string[] arrTStr = t_c.Split(':');
            string s_c = arrTStr[1].Trim().Trim('<', '>', '\'');
*/
            string s_c = getStringCharFromTokenLine(t_c);

            MethodInfo StringHex12ToCharMI = typeof(GeneratorCIL).GetMethod(
               "StringHex12ToChar", new Type[] { typeof(string) }
               );
            ILGenEvaluate.Emit(OpCodes.Ldstr, s_c);
            ILGenEvaluate.EmitCall(OpCodes.Call, StringHex12ToCharMI, null);

            Type SubsetClassType = Type.GetType("ConsoleFrontEnd.SubsetClass");
            ConstructorInfo SubsetClassCtor = SubsetClassType.GetConstructor(new Type[0]);
            ILGenEvaluate.Emit(OpCodes.Newobj, SubsetClassCtor);

            Type[] ClassPartCtorParams = new Type[] { typeof(char), typeof(SubsetClass) };
            Type ClassPartType = Type.GetType("ConsoleFrontEnd.ClassPart");
            ConstructorInfo ClassPartCtor = ClassPartType.GetConstructor(ClassPartCtorParams);
            ILGenEvaluate.Emit(OpCodes.Newobj, ClassPartCtor);
          
        }

        void EmitRule_7(Production root)
        {
            //7: E -> regular id ; A D ,  GetName(id) A D NewObjAlphabetPartNamedExp NewObjRegularExp 
            //7==root.rule         

            string t_id = (root.alpha[1] as Terminal).tokenLine; //1-id
            string[] arrTStr = t_id.Split(':');
            string s_id = arrTStr[1].Trim().Trim('<', '>');

            ILGenEvaluate.Emit(OpCodes.Ldstr, s_id);

            LeftDerive(root.alpha[3]);//A-AlphabetPartList
            LeftDerive(root.alpha[4]);//D-NamedExp

            Type[] AlphabetPartNamedExpCtorParams = new Type[] { typeof(AlphabetPartList), typeof(NamedExp) };
            Type AlphabetPartNamedExpType = Type.GetType("ConsoleFrontEnd.AlphabetPartNamedExp");
            ConstructorInfo AlphabetPartNamedExpCtor = AlphabetPartNamedExpType.GetConstructor(AlphabetPartNamedExpCtorParams);
            ILGenEvaluate.Emit(OpCodes.Newobj, AlphabetPartNamedExpCtor);

            Type[] RegularExpParams = new Type[] { typeof(string), typeof(AlphabetPartNamedExp) };
            Type RegularExpType = Type.GetType("ConsoleFrontEnd.RegularExp");
            ConstructorInfo RegularExpCtor = RegularExpType.GetConstructor(RegularExpParams);
            ILGenEvaluate.Emit(OpCodes.Newobj, RegularExpCtor);
           
        }

    }

}
