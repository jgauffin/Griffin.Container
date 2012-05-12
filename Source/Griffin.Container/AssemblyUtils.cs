using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Griffin.Container
{
    /// <summary>
    /// Helpers for <see cref="Assembly"/>
    /// </summary>
    public static class AssemblyUtils
    {
        /// <summary>
        /// Load all assemblies, try to use assemblies previously loaded into the AppDomain.
        /// </summary>
        /// <param name="path">Directory to scan</param>
        /// <param name="filePattern">Pattern to match. Same format as for <see cref="Directory.GetFiles(string, string)"/></param>
        /// <returns>All matching assemblies</returns>
        public static IEnumerable<Assembly> LoadAssemblies(string path, string filePattern)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (filePattern == null) throw new ArgumentNullException("filePattern");

            foreach (var fullPath in Directory.GetFiles(path, filePattern))
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.Load(fullPath);
                    if (assembly.IsDynamic)
                        continue;
                }
                catch (ReflectionTypeLoadException err)
                {
                    throw new InvalidOperationException(
                        string.Format("Failed to load assembly named '{0}'.", fullPath), err);
                }

                yield return assembly;
            }
        }
    }
}