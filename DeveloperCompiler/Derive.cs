using System;
//
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.IO;
//
using Const_LALR_Tables;
using AST;

namespace ConsoleFrontEnd
{
    class Derive
    {
        public string fileName;
        StreamWriter streamWriter;

        public Derive(string fileName)
        {
            this.fileName = fileName;
            //2)Create StreamWriter 
            File.Delete(this.fileName);
            streamWriter = File.CreateText(this.fileName);
        }

        void LeftDerive(object root)
        {
            if (root is Terminal)
            {
                streamWriter.WriteLine("{0,3:d}{1}",(root as Terminal).iTA,(root as Terminal).tokenLine); //EMPTY token????
                return;
            }
            if (root is Production)
            {
                streamWriter.WriteLine("<{0,3:d}>", (root as Production).rule); //Begin (root as Production).rule
                    if ((root as Production).alpha != null)
                    {
                        int len_alpha = (root as Production).alpha.GetLength(0);
                        object alpha_i;
                        for (int i = 0; i < len_alpha; i++)
                        {
                            alpha_i = (root as Production).alpha[i];
                            LeftDerive(alpha_i);
                        }
                    }
                streamWriter.WriteLine("</{0,3:d}>", (root as Production).rule);//End (root as Production).rule
                return;
            }

        }
        public bool DoLeftDerive(object rootAST)
        {
            if ((rootAST is Terminal) || (rootAST is Production))
            {
                LeftDerive(rootAST);
                streamWriter.Close();
                return true;
            }
            streamWriter.Close();
            return false;
        }

        void RightDerive(object root)
        {
            if (root is Terminal)
            {
                streamWriter.WriteLine((root as Terminal).tokenLine);//EMPTY token????
                return;
            }
            if (root is Production)
            {
                int len_alpha = (root as Production).alpha.GetLength(0);
                object alpha_i;
                for (int i = 1; i <= len_alpha; i++)
                {
                    alpha_i = (root as Production).alpha[len_alpha - i];
                    RightDerive(alpha_i);
                }
                return;
            }

        }
        public bool DoRightDerive(object rootAST)
        {
            if ((rootAST is Terminal) || (rootAST is Production))
            {
                RightDerive(rootAST);
                streamWriter.Close();
                return true;
            }
            streamWriter.Close();
            return false;
        }

        public static void TestDerivator(bool is_left, object rootAST, string fileName)
        {
            Production root = rootAST as Production;

            Console.WriteLine("\n TestDerivator( {0} , {1}, {2} )!\n", is_left, rootAST, fileName);
            //rootAST --> "AST.Production" as a result of Console.WriteLine
            if (root == null)
                Console.WriteLine("\n rootAST == null!\n Parser.countERRORS == {0}", Parser.countERRORS);
            else
            {
                Console.WriteLine("\n root.rule: {0}\n Parser.countERRORS == {1}", root.rule, Parser.countERRORS);

                Derive derive = new Derive(fileName);

                bool b_Derive = true;

                if(is_left)

                    b_Derive = derive.DoLeftDerive(root);
                else
                    b_Derive = derive.DoRightDerive(root);


                Console.WriteLine("\n {0} is {1}!\n", is_left ? "DoLeftDerive(rootAST)" : "DoRightDerive(rootAST)", b_Derive);
               

            }

        }

    }
}
