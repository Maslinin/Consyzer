namespace Consyzer.Analyzers;

internal interface IAnalyzer<in TIn, out TOut>
{
    TOut Analyze(TIn input);
}