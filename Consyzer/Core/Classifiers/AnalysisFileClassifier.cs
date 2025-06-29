using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Core.Models;
using Consyzer.Core.Resources;

namespace Consyzer.Core.Classifiers;

internal sealed class AnalysisFileClassifier(
    IResourceAccessor<FileInfo, PEReader> peReaderAccessor
) : IFileClassifier<AnalysisFileClassification>
{
    public AnalysisFileClassification Check(IEnumerable<FileInfo> files)
    {
        var nonEcmaModules = new List<FileInfo>();
        var ecmaAssemblies = new List<FileInfo>();
        var nonEcmaAssemblies = new List<FileInfo>();

        foreach (var file in files)
        {
            if (!IsEcma(file, out var peReader))
            {
                nonEcmaModules.Add(file);
                continue;
            }

            (IsEcmaAssembly(peReader) ? ecmaAssemblies : nonEcmaAssemblies).Add(file);
        }

        return new AnalysisFileClassification
        {
            NonEcmaModules = nonEcmaModules,
            EcmaAssemblies = ecmaAssemblies,
            NonEcmaAssemblies = nonEcmaAssemblies
        };
    }

    private bool IsEcma(FileInfo file, out PEReader peReader)
    {
        try
        {
            peReader = peReaderAccessor.Get(file);
            return peReader.HasMetadata && peReader.PEHeaders is not null;
        }
        catch (BadImageFormatException)
        {
            peReader = null!;
            return false;
        }
    }

    private static bool IsEcmaAssembly(PEReader peReader)
    {
        return peReader.GetMetadataReader().IsAssembly;
    }
}