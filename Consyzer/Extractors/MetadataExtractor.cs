using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Consyzer.Extractors
{
    internal class MetadataExtractor
    {
        protected readonly MetadataReader _mdReader;

        public MetadataExtractor(FileInfo fileInfo)
        {
            var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            this._mdReader = new PEReader(fileStream).GetMetadataReader();
        }

        public IEnumerable<MethodDefinition> GetMethodDefinitions()
        {
            var methodDefHandles = this._mdReader.MethodDefinitions;
            return methodDefHandles.Select(h => this._mdReader.GetMethodDefinition(h));
        }

    }
}