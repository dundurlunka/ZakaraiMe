﻿namespace ZakaraiMe.Web.Infrastructure.Mapping
{
    using AutoMapper;
    using Common.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Custom automapper profile which automatically registers all mappings and their custom settings.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            IEnumerable<Type> allTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a => a.GetName().Name.Contains("ZakaraiMe"))
                .SelectMany(a => a.GetTypes());

            var mappings = allTypes
                .Where(t => t.IsClass && !t.IsAbstract && t
                    .GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Select(i => i.GetGenericTypeDefinition())
                    .Contains(typeof(IMapFrom<>)))
                .Select(t => new
                {
                    Destination = t,
                    Source = t.GetInterfaces()
                        .Where(i => i.IsGenericType)
                        .Select(i => new
                        {
                            Definition = i.GetGenericTypeDefinition(),
                            Arguments = i.GetGenericArguments()
                        })
                        .Where(i => i.Definition == typeof(IMapFrom<>))
                        .SelectMany(i => i.Arguments)
                        .First()
                })
                .ToList();

            foreach (var mapping in mappings)
            {
                CreateMap(mapping.Source, mapping.Destination);
            }

            List<IHaveCustomMapping> customMappings = allTypes
                    .Where(t => t.IsClass && !t.IsAbstract && typeof(IHaveCustomMapping).IsAssignableFrom(t))
                    .Select(Activator.CreateInstance)
                    .Cast<IHaveCustomMapping>()
                    .ToList();

            foreach (IHaveCustomMapping map in customMappings)
            {
                map.ConfigureMapping(this);
            }
        }
    }
}
