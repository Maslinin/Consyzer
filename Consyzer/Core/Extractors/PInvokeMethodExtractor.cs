using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Core.Models;
using Consyzer.Core.Resources;

namespace Consyzer.Core.Extractors;

internal sealed class PInvokeMethodExtractor(
    IResourceAccessor<FileInfo, PEReader> peReaderManager
) : IExtractor<FileInfo, IEnumerable<PInvokeMethodsGroup>>
{
    public IEnumerable<PInvokeMethodsGroup> Extract(FileInfo file)
    {
        var peReader = peReaderManager.Get(file);
        var mdReader = peReader.GetMetadataReader();

        var pinvokeMethods = GetPInvokeMethods(mdReader);
        if (!pinvokeMethods.Any())
        {
            yield break;
        }

        yield return new PInvokeMethodsGroup
        {
            File = file,
            Methods = pinvokeMethods
        };
    }

    private static List<PInvokeMethod> GetPInvokeMethods(MetadataReader mdReader)
    {
        var result = new List<PInvokeMethod>();

        foreach (var handle in mdReader.MethodDefinitions)
        {
            var definition = mdReader.GetMethodDefinition(handle);

            if (IsPInvokeMethod(definition))
            {
                result.Add(ToPInvokeMethod(mdReader, definition));
            }
        }

        return result;
    }

    private static bool IsPInvokeMethod(MethodDefinition methodDef)
    {
        return methodDef.Attributes.HasFlag(MethodAttributes.PinvokeImpl);
    }

    private static PInvokeMethod ToPInvokeMethod(MetadataReader mdReader, MethodDefinition methodDef)
    {
        var signatureExtractor = new MethodSignatureExtractor(mdReader);

        var import = methodDef.GetImport();
        var module = mdReader.GetModuleReference(import.Module);

        return new PInvokeMethod
        {
            Signature = signatureExtractor.Extract(methodDef),
            DllLocation = mdReader.GetString(module.Name),
            DllImportFlags = import.Attributes
        };
    }
}