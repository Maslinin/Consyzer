namespace Consyzer.Core.Checkers;

internal interface IFileClassificationChecker<out TOut>
{
    TOut Check(IEnumerable<FileInfo> files);
}