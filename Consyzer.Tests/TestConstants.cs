using System;
using System.IO;
using System.Reflection;

namespace Consyzer.Tests;

internal static class TestConstants
{
    public static string NotMetadataAssemblyFile => "Consyzer.exe";
    public static string MetadataAssemblyLocation => Assembly.GetExecutingAssembly().Location;
    public static string NotMetadataAssemblyLocation => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NotMetadataAssemblyFile);
    public static FileInfo MetadataAssemblyFileInfo => new(MetadataAssemblyLocation);
    public static FileInfo NotMetadataAssemblyFileInfo => new(NotMetadataAssemblyLocation);
}