namespace Consyzer.Core.Resolvers;

internal interface IFilePresenceResolver<out TOut>
{
    TOut Resolve(string file);
}