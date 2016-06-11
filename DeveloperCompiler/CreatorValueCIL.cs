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
    class CreatorValueCIL
    {
        public static Value CreateValueAST(bool isSave)
        {
            AppDomain currentAppDomain = Thread.GetDomain();

            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = "ValueAST";
            AssemblyBuilder assemblyBuilder = currentAppDomain.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.RunAndSave
                );

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("ValueAST.dll", "ValueAST.dll");

            TypeBuilder typeBuilder = moduleBuilder.DefineType("ValueCIL",  //Derived Class <<ValueCIL>>
                TypeAttributes.Public | TypeAttributes.Class,
                typeof(Value)                                               // Base Class <<Value>>
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
                typeof(RegularExpList),
                new Type[0]
                );

            ILGenerator evaluateMethodBuilder_ILGen = evaluateMethodBuilder.GetILGenerator();


            ////.
            GeneratorCIL genAST = new GeneratorCIL(Parser.RootAST, evaluateMethodBuilder_ILGen);

            genAST.EmitEvaluate();

            //1) CreateType()
            //2) assemblyBuilder.Save("Func.dll");
            //3) Create new instance of ValueCIL ==> void Evaluate(StreamWriter)

            Type type = typeBuilder.CreateType(); //type is ValueCIL == true
            if (isSave)
            {
                assemblyBuilder.Save("ValueAST.dll");
            }
            ConstructorInfo ctor = type.GetConstructor(new Type[0]);//ctor for class <<ValueCIL>> 

            return ctor.Invoke(null) as Value; // Instance of <<ValueCIL>> as <<Value>>

        }
    }
}
