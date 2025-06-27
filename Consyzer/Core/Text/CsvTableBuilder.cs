namespace Consyzer.Core.Text;

internal sealed class CsvTableBuilder(string delimiter)
{
    private readonly List<string> lines = [];

    public CsvTableBuilder Header(IEnumerable<string> cols)
    {
        lines.Add(string.Join(delimiter, cols));
        return this;
    }

    public CsvTableBuilder Record(IEnumerable<string> cols)
    {
        lines.Add(string.Join(delimiter, cols));
        return this;
    }

    public CsvTableBuilder Records<T>(IEnumerable<T> items, Func<object?, string> serializer)
    {
        var props = typeof(T).GetProperties();
        Header(props.Select(p => p.Name));

        foreach (var item in items)
        {
            Record(props.Select(p => serializer(p.GetValue(item))));
        }

        return this;
    }

    public override string ToString() => string.Join(Environment.NewLine, lines) + Environment.NewLine;
}