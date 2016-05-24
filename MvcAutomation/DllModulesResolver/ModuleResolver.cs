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
        public static ITestEndpoints GetAppDll(string dllPath, string type)
        {
            return (ITestEndpoints)ModuleResolver.GetDll(dllPath, type);
        }

        public static IImageTestEndpoints GetImageDll(string dllPath, string type)
        {
            return (IImageTestEndpoints)ModuleResolver.GetDll(dllPath, type);
        }

        private static object GetDll(string dllPath, string type)
        {
            var DLL = Assembly.LoadFile(dllPath);
            Type theType = DLL.GetType(type);
            return Activator.CreateInstance(theType);
        }
    }
}