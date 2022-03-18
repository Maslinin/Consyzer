using System;

namespace Consyzer.AnalyzerEngine
{
	public class PEFileNotSupportedException : Exception
	{
		public PEFileNotSupportedException(string message = "File is not a PE file.") : base(message) { }
		public PEFileNotSupportedException(string message, Exception ex) : base(message, ex) { }
	}
}