using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using TestEndpoints;

namespace MvcAutomation.DllModulesResolver
{
    public static class ModuleResolver
    {
        public static ITestEndpoints GetDll(string dllPath, string type)
        {
            var DLL = Assembly.LoadFile(dllPath);
            Type theType = DLL.GetType(type);
            return (ITestEndpoints)Activator.CreateInstance(theType);
        }
    }
}