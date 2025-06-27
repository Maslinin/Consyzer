using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Consyzer.Core.Models;
using Consyzer.Core.Resources;

namespace Consyzer.Core.Extractors;

internal sealed class PInvokeMethodExtractor(
    IResourceAccessor<FileInfo, PEReader> peReaderAccessor
) : IExtractor<FileInfo, IEnumerable<PInvokeMethod>>
{
    public IEnumerable<PInvokeMethod> Extract(FileInfo file)
    {
        var peReader = peReaderAccessor.Get(file);
        var mdReader = peReader.GetMetadataReader();

        foreach (var method in GetPInvokeMethods(mdReader))
        {
            yield return method;
        }
    }

    private static List<PInvokeMethod> GetPInvokeMethods(MetadataReader mdReader)
    {
        var methods = new List<PInvokeMethod>();

        foreach (var handle in mdReader.MethodDefinitions)
        {
            var definition = mdReader.GetMethodDefinition(handle);

            if (IsPInvokeMethod(definition))
            {
                methods.Add(ToPInvokeMethod(mdReader, definition));
            }
        }

        return methods;
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
            ImportName = mdReader.GetString(module.Name),
            ImportFlags = import.Attributes
        };
    }
}