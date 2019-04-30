using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FaisalLeague.Data.Access.Maps.Common;
using FaisalLeague.Data.Access.Maps.Main;

namespace FaisalLeague.Data.Access.DAL
{
    public static class MappingsHelper
    {
        public static IEnumerable<IMap> GetMainMappings()
        {
            var assemblyTypes = typeof(UserMap).GetTypeInfo().Assembly.DefinedTypes;
            var mappings = assemblyTypes
                // ReSharper disable once AssignNullToNotNullAttribute
                .Where(t => t.Namespace != null && t.Namespace.Contains(typeof(UserMap).Namespace))
                .Where(t => typeof(IMap).GetTypeInfo().IsAssignableFrom(t));
            mappings = mappings.Where(x => !x.IsAbstract);
            return mappings.Select(m => (IMap) Activator.CreateInstance(m.AsType())).ToArray();
        }
    }
}