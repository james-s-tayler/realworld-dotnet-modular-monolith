using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Application.Core.Reflection
{
    public static class AssemblyExtensions
    {
        public static List<TypeInfo> GetOperationContractTypes(this Assembly assembly)
        {
            return assembly.DefinedTypes.Where(type => type.Name.EndsWith("Query") || type.Name.EndsWith("Command")).ToList();
        }
        
        public static List<TypeInfo> GetTypesAssignableTo(this Assembly assembly, Type compareType)
        {
            var typeInfoList = assembly.DefinedTypes.Where(x => x.IsClass
                                                                && !x.IsAbstract
                                                                && x != compareType
                                                                && x.GetInterfaces()
                                                                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == compareType))?.ToList();

            return typeInfoList;
        }
    }
}