using System.Xml;
using System.Text;
using Consyzer.Options;
using Consyzer.Core.Models;
using Microsoft.Extensions.Options;
using static Consyzer.Constants.Output;

namespace Consyzer.Output.Reporting;

internal sealed class XmlReportWriter(
    IOptions<AppOptions> options
) : IReportWriter
{
    private const string ReportName = "ConsyzerReport";

    private AppOptions.ReportOptions.XmlOptions Options => options.Value.Report.Xml;

    public string Write(AnalysisOutcome outcome)
    {
        Directory.CreateDirectory(Destination.TargetDirectory);
        var fullPath = Path.Combine(Destination.TargetDirectory, Destination.Xml);

        var encoding = Encoding.GetEncoding(Options.Encoding);

        using var writer = XmlWriter.Create(fullPath, new XmlWriterSettings
        {
            Indent = true,
            Encoding = encoding,
            IndentChars = Options.IndentChars
        });

        writer.WriteStartDocument();
        writer.WriteStartElement(ReportName);

        WriteAssemblyMetadata(writer, outcome.AssemblyMetadataList);
        WritePInvokeGroups(writer, outcome.PInvokeMethodGroups);
        WriteLibraryPresence(writer, outcome.LibraryPresences);
        WriteSummary(writer, outcome.Summary);

        writer.WriteEndElement();
        writer.WriteEndDocument();

        return fullPath;
    }

    private static void WriteAssemblyMetadata(XmlWriter writer, IEnumerable<AssemblyMetadata> assemblyInfos)
    {
        writer.WriteStartElement(Structure.Section.Name.AssemblyMetadataList);

        foreach (var info in assemblyInfos)
        {
            writer.WriteStartElement(Structure.Element.Assembly);
            writer.WriteElementString(Structure.Label.Assembly.File, info.File.Name);
            writer.WriteElementString(Structure.Label.Assembly.Version, info.Version);
            writer.WriteElementString(Structure.Label.Assembly.CreationDateUtc, info.CreationDateUtc.ToString());
            writer.WriteElementString(Structure.Label.Assembly.Sha256, info.Sha256);
            writer.WriteEndElement();
        }

        writer.WriteEndElement();
    }

    private static void WritePInvokeGroups(XmlWriter writer, IEnumerable<PInvokeMethodGroup> methodGroups)
    {
        writer.WriteStartElement(Structure.Section.Name.PInvokeMethodGroups);

        foreach (var group in methodGroups)
        {
            writer.WriteStartElement(Structure.Element.Group);
            writer.WriteAttributeString(Structure.Label.PInvoke.File, group.File.Name);

            foreach (var method in group.Methods)
            {
                writer.WriteStartElement(Structure.Element.Method);
                writer.WriteElementString(Structure.Label.PInvoke.Signature, method.Signature.ToString());
                writer.WriteElementString(Structure.Label.PInvoke.ImportName, method.ImportName);
                writer.WriteElementString(Structure.Label.PInvoke.ImportFlags, method.ImportFlags.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        writer.WriteEndElement();
    }

    private static void WriteLibraryPresence(XmlWriter writer, IEnumerable<LibraryPresence> presences)
    {
        writer.WriteStartElement(Structure.Section.Name.LibraryPresences);

        foreach (var presence in presences)
        {
            writer.WriteStartElement(Structure.Element.Library);
            writer.WriteElementString(Structure.Label.Library.Name, presence.LibraryName);
            writer.WriteElementString(Structure.Label.Library.ResolvedPath, presence.ResolvedPath);
            writer.WriteElementString(Structure.Label.Library.LocationKind, presence.LocationKind.ToString());
            writer.WriteEndElement();
        }

        writer.WriteEndElement();
    }

    private static void WriteSummary(XmlWriter writer, AnalysisSummary summary)
    {
        writer.WriteStartElement(Structure.Section.Name.Summary);

        writer.WriteElementString(Structure.Label.Summary.TotalFiles, summary.TotalFiles.ToString());
        writer.WriteElementString(Structure.Label.Summary.EcmaAssemblies, summary.EcmaAssemblies.ToString());
        writer.WriteElementString(Structure.Label.Summary.AssembliesWithPInvoke, summary.AssembliesWithPInvoke.ToString());
        writer.WriteElementString(Structure.Label.Summary.TotalPInvokeMethods, summary.TotalPInvokeMethods.ToString());
        writer.WriteElementString(Structure.Label.Summary.MissingLibraries, summary.MissingLibraries.ToString());

        writer.WriteEndElement();
    }
}