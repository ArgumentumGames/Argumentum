using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using System.Web;
using Argumentum.AssetConverter.Mindmapper;

namespace Argumentum.AssetConverter
{
    public class CustomTypeProvider : IDynamicLinqCustomTypeProvider
    {

        public IDynamicLinqCustomTypeProvider DefaultProvider { get; set; }


        public HashSet<Type> GetCustomTypes()
        {
            HashSet<Type> types = DefaultProvider.GetCustomTypes();
            types.Add(typeof(HttpUtility));
            types.Add(typeof(MindMapDocumentConfig));
            types.Add(typeof(FallacyMindMapDocumentConfig));
            types.Add(typeof(VirtueMindMapDocumentConfig));
            return types;
        }

        public Dictionary<Type, List<MethodInfo>> GetExtensionMethods()
        {
            return DefaultProvider.GetExtensionMethods();
        }

        public Type ResolveType(string typeName)
        {
            return DefaultProvider.ResolveType(typeName);
        }

        public Type ResolveTypeBySimpleName(string simpleTypeName)
        {
            return DefaultProvider.ResolveTypeBySimpleName(simpleTypeName);
        }
    }
}