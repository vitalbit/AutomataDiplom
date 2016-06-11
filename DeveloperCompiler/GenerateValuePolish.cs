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

namespace ConsoleFrontEnd
{
    public abstract class ValuePolish
    {
        //public abstract void Evaluate(StreamWriter strWriter);
        public abstract Diagramm  Evaluate();
    }

    public abstract class GeneratorPolish
    {
        public List<string> polishExpression;
        public ILGenerator ILGenEvaluate;

        public abstract void EmitEvaluate();


    }
    public
    class GeneratorPolishCIL : GeneratorPolish
    {

        public GeneratorPolishCIL(List<string> polishExpression, ILGenerator ILGenEvaluate)
        {
            this.polishExpression = polishExpression;
            this.ILGenEvaluate = ILGenEvaluate;
        }
        public override void EmitEvaluate()
        {
            //TODO:
            foreach (string token in polishExpression)
            {
                switch (token)
                {
                    case "Star": //basic operator
                        EmitStar();
                        break;

                    case "Concat":
                        EmitConcat();
                        break;

                    case "Join":
                        EmitJoin();
                        break;

                    default://string-constants as "string-constant" <--> string-constant should be, but they are not in such form!!!

                        EmitConstant(token);
                        break;

                }

            }             

            ILGenEvaluate.Emit(OpCodes.Ret);

        } //public override void EmitEvaluate()
        private void EmitStar()
        {
            Type DiagrammType = Type.GetType("ConsoleFrontEnd.Diagramm");
            MethodInfo StarMI = DiagrammType.GetMethod(
               "Star",
               new Type[0] 
               );
            ILGenEvaluate.Emit(OpCodes.Call, StarMI);
        }
        private void EmitConcat()
        {
            //Type[] ConcatParams = new Type[] { typeof(Diagramm) };
            Type DiagrammType = Type.GetType("ConsoleFrontEnd.Diagramm");
            MethodInfo ConcatMI = DiagrammType.GetMethod(
               "Concat",
               new Type[] { typeof(Diagramm) }
               );
            ILGenEvaluate.Emit(OpCodes.Call, ConcatMI);
        }
        private void EmitJoin()
        {
            Type DiagrammType = Type.GetType("ConsoleFrontEnd.Diagramm");
            MethodInfo JoinMI = DiagrammType.GetMethod(
               "Join",
               new Type[] { typeof(Diagramm) }
               );
            ILGenEvaluate.Emit(OpCodes.Call, JoinMI);

        }
        private void EmitConstant(string token)
        {
            
            Type[] DiagrammCtorParams = new Type[] { typeof(string) };
            Type DiagrammType = Type.GetType("ConsoleFrontEnd.Diagramm");
            ConstructorInfo DiagrammCtor = DiagrammType.GetConstructor(DiagrammCtorParams);

            ILGenEvaluate.Emit(OpCodes.Ldstr, token);
            ILGenEvaluate.Emit(OpCodes.Newobj, DiagrammCtor);
        }

    }


}
