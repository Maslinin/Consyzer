namespace Consyzer.Core.Resources;

internal interface IResourceAccessor<in TIn, out TOut> : IDisposable
{
    TOut Get(TIn input);
}