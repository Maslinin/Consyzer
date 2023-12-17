using System.Collections.Generic;
using Consyzer.Extractors.Models;

namespace Consyzer.Extractors;

internal interface IEcmaImportedMethodExtractor
{
    IEnumerable<ImportedMethodInfo> GetImportedMethodInfos();
}