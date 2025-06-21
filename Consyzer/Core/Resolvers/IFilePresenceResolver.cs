namespace Consyzer.Core.Resolvers;

internal interface IFilePresenceResolver<out TOut>
{
    TOut Check(string file);
}