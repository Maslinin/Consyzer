using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Core.Models;
using Consyzer.Core.Resources;

namespace Consyzer.Core.Checkers;

internal sealed class FileClassificationChecker(
    IResourceAccessor<FileInfo, PEReader> accessor
) : IFileClassificationChecker<AnalysisFileClassification>
{
    public AnalysisFileClassification Check(IEnumerable<FileInfo> files)
    {
        var nonEcmaModules = new List<FileInfo>();
        var ecmaAssemblies = new List<FileInfo>();
        var nonEcmaAssemblies = new List<FileInfo>();

        foreach (var file in files)
        {
            if (!TryCheckEcmaModule(file, out var peReader))
            {
                nonEcmaModules.Add(file);
                continue;
            }

            (TryCheckEcmaAssembly(peReader) ? ecmaAssemblies : nonEcmaAssemblies).Add(file);
        }

        return new AnalysisFileClassification
        {
            NonEcmaModules = nonEcmaModules,
            EcmaAssemblies = ecmaAssemblies,
            NonEcmaAssemblies = nonEcmaAssemblies
        };
    }

    private bool TryCheckEcmaModule(FileInfo file, out PEReader peReader)
    {
        try
        {
            peReader = accessor.Get(file);
            return peReader.HasMetadata && peReader.PEHeaders is not null;
        }
        catch (BadImageFormatException)
        {
            peReader = null!;
            return false;
        }
    }

    private static bool TryCheckEcmaAssembly(PEReader peReader)
    {
        return peReader.GetMetadataReader().IsAssembly;
    }
}