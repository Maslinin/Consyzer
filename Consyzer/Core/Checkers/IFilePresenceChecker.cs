namespace Consyzer.Core.Checkers;

internal interface IFilePresenceChecker<out TOut>
{
    TOut Check(string file);
}