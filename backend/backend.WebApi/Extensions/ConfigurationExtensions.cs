using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.WebApi.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetConnectionStringOrThrow(this IConfiguration configuration, string sectionName)
        {
            return configuration.GetConnectionString(sectionName) ?? throw new InvalidOperationException($"Connection string '{sectionName}' not found.");
        }
    }
}