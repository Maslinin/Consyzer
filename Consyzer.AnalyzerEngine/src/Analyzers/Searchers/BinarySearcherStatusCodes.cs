namespace Consyzer.AnalyzerEngine.Analyzers.Searchers
{
    /// <summary>
    /// Represents the existence state codes of a binary file.
    /// </summary>
    public enum BinarySearcherStatusCodes
    {
        BinaryExistsOnSourcePath = 0,
        BinaryExistsOnAbsolutePath = 1,
        BinaryNotExists = 2
    }
}
