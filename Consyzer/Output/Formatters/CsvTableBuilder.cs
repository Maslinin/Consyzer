namespace Consyzer.Output.Formatters;

internal sealed class CsvTableBuilder(char delimiter)
{
    private readonly List<string> lines = [];

    public CsvTableBuilder Header(IEnumerable<string> fields)
    {
        lines.Add(string.Join(delimiter, fields));
        return this;
    }

    public CsvTableBuilder Record(IEnumerable<string> fields)
    {
        lines.Add(string.Join(delimiter, fields));
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

    public string Build() => string.Join(Environment.NewLine, lines) + Environment.NewLine;
}