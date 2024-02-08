using System;
using System.Reflection;

namespace Shared.Services
{
    public class UriService
    {
        private static readonly string? _assemblyName;
        static UriService()
        {
            _assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
        }
        public static Uri GetAbsoluteUri(string filePath, string? assemblyName = null)
        {
            if (assemblyName == null)
            {
                assemblyName = _assemblyName;
            }

            return new Uri(@"pack://application:,,,/" + assemblyName + ";component/" + filePath, UriKind.Absolute);
        }
    }
}
