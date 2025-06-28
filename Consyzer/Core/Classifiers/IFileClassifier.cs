namespace Consyzer.Core.Classifiers;

internal interface IFileClassifier<out TOut>
{
    TOut Check(IEnumerable<FileInfo> files);
}