using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Other;

internal static class AssemblyLoader
{
    public static void ValidateReferencedAssemblies<T>(ILogger<T> logger)
    {
        var currentDomain = AppDomain.CurrentDomain;
        var loadedAssemblies = currentDomain.GetAssemblies();

        var currentAssembly = typeof(AssemblyLoader).Assembly;
        var currentAssemblyFolder = Path.GetDirectoryName(currentAssembly.Location)!;

        var referencedAssemblyNames = currentAssembly.GetReferencedAssemblies();

        foreach (var referencedAssemblyName in referencedAssemblyNames)
        {
            var loadedAssembly = loadedAssemblies.FirstOrDefault(x => x.GetName().Name.Equals(referencedAssemblyName.Name));

            if (loadedAssembly is not null)
            {
                logger.LogDebug("Referenced Assembly has already been loaded: referenced: \"{ReferencedAssemblyName}\" loaded: \"{LoadedAssemblyName}\"", referencedAssemblyName.FullName, loadedAssembly.FullName);
                ValidateAssemblyVersions(logger, referencedAssemblyName, loadedAssembly.GetName());
                continue;
            }

            var dllPath = Path.Combine(currentAssemblyFolder, $"{referencedAssemblyName.Name}.dll");
            if (File.Exists(dllPath))
            {
                try
                {
                    loadedAssembly = Assembly.LoadFrom(dllPath);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Unable to load Assembly \"{AssemblyFullName}\" from path \"{Path}\"", referencedAssemblyName.FullName, dllPath);
                    loadedAssembly = null;
                }

                if (loadedAssembly is not null)
                {
                    logger.LogDebug("Referenced Assembly got loaded from disk: referenced: \"{ReferencedAssemblyName}\" loaded: \"{LoadedAssemblyName}\" path: \"{Path}\"", referencedAssemblyName.FullName, loadedAssembly.FullName, dllPath);
                    ValidateAssemblyVersions(logger, referencedAssemblyName, loadedAssembly.GetName());
                    continue;
                }
            }
            else
            {
                logger.LogWarning("Referenced Assembly \"{AssemblyFullName}\" was not found at \"{Path}\"", referencedAssemblyName.FullName, dllPath);
            }

            try
            {
                loadedAssembly = Assembly.Load(referencedAssemblyName.FullName);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to load Assembly \"{AssemblyFullName}\"", referencedAssemblyName.FullName);
                loadedAssembly = null;
            }

            if (loadedAssembly is not null)
            {
                logger.LogDebug("Referenced Assembly was loaded by the .NET Framework: referenced: \"{ReferencedAssemblyName}\" loaded: \"{LoadedAssemblyName}\"", referencedAssemblyName.FullName, loadedAssembly.FullName);
                ValidateAssemblyVersions(logger, referencedAssemblyName, loadedAssembly.GetName());
                continue;
            }

            var exception = new Exception($"Missing Assembly \"{referencedAssemblyName.FullName}\"");
            logger.LogError(exception, null);
            throw exception;
        }
    }

    private static void ValidateAssemblyVersions(ILogger logger, AssemblyName expected, AssemblyName actual)
    {
        var expectedVersion = expected.Version;
        var actualVersion = actual.Version;

        if (expectedVersion.Major != actualVersion.Major)
            LogThrow(logger, $"Version mismatch for Assembly \"{expected.Name}\"! expected: {expectedVersion} actual: {actualVersion}");

        if (!expectedVersion.Equals(actualVersion))
        {
            logger.LogWarning("Version mismatch for Assembly \"{AssemblyName}\": expected: {ExpectedVersion} actual: {ActualVersion}", expected.Name, expectedVersion, actualVersion);
        }
    }

    private static void LogThrow(ILogger logger, string msg)
    {
        var e = new Exception(msg);
        logger.LogError(e, null);
        throw e;
    }
}
