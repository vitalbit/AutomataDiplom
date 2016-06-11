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

namespace ConsoleFrontEnd
{
    class CreatorValuePolishCIL
    {
        public static ValuePolish CreateValuePolish(string TypeName, List<string> polishExpression, bool isSave)
        {
            AppDomain currentAppDomain = Thread.GetDomain();

            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = TypeName ;
            AssemblyBuilder assemblyBuilder = currentAppDomain.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.RunAndSave
                );

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("Value" + TypeName + ".dll", "Value" + TypeName + ".dll");

            TypeBuilder typeBuilder = moduleBuilder.DefineType("ValuePolishCIL",  //Derived Class <<ValuePolishCIL>>
                TypeAttributes.Public | TypeAttributes.Class,
                typeof(ValuePolish)                                               // Base Class <<ValuePolish>>
                );

            ConstructorBuilder constructorBuilder1 = typeBuilder.DefineConstructor(
               MethodAttributes.Public,
               CallingConventions.Standard,
               new Type[] { }
               );

            ILGenerator constructorBuilder1_ILGen = constructorBuilder1.GetILGenerator();

            constructorBuilder1_ILGen.Emit(OpCodes.Ldarg_0);
            constructorBuilder1_ILGen.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));
            constructorBuilder1_ILGen.Emit(OpCodes.Ret);

            MethodBuilder evaluateMethodBuilder = typeBuilder.DefineMethod(
             "Evaluate",
             MethodAttributes.Public | MethodAttributes.Virtual,
                        //typeof(void),
                        //new Type[] { typeof(StreamWriter) }
             typeof(Diagramm),
             new Type[0]
             );

            ILGenerator evaluateMethodBuilder_ILGen = evaluateMethodBuilder.GetILGenerator();

            GeneratorPolishCIL genPolishCIL = new GeneratorPolishCIL(polishExpression, evaluateMethodBuilder_ILGen);
            genPolishCIL.EmitEvaluate();

            //1) CreateType()
            //2) assemblyBuilder.Save("Value" + TypeName + ".dll");
            //3) Create new instance of ValuePolishCIL ==> void Evaluate()

            Type type = typeBuilder.CreateType(); //type is ValueCIL == true
            //////if (isSave)
            //////{
            //////    assemblyBuilder.Save("Value" + TypeName + ".dll");
            //////}

            ConstructorInfo ctor = type.GetConstructor(new Type[0]);//ctor for class <<ValuePolishCIL>> 

            return ctor.Invoke(null) as ValuePolish; // Instance of <<ValuePolishCIL>> as <<ValuePolish>>
        }
    }
}
