namespace Consyzer.Core.Text;

internal sealed class CsvTableBuilder(string delimiter)
{
    private readonly List<string> lines = [];

    public CsvTableBuilder WithHeader(params string[] cols)
    {
        return WithHeader((IEnumerable<string>)cols);
    }

    public CsvTableBuilder WithHeader(IEnumerable<string> cols)
    {
        lines.Add(string.Join(delimiter, cols));
        return this;
    }

    public CsvTableBuilder AddRow(params string[] cols)
    {
        return AddRow((IEnumerable<string>)cols);
    }

    public CsvTableBuilder AddRow(IEnumerable<string> cols)
    {
        lines.Add(string.Join(delimiter, cols));
        return this;
    }

    public CsvTableBuilder WithObjectRows<T>(IEnumerable<T> items, Func<object?, string> serializer)
    {
        var props = typeof(T).GetProperties();
        WithHeader(props.Select(p => p.Name));

        foreach (var item in items)
        {
            AddRow(props.Select(p => serializer(p.GetValue(item))));
        }

        return this;
    }

    public override string ToString() => string.Join(Environment.NewLine, lines) + Environment.NewLine;
}