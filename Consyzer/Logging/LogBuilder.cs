using System.Text;

namespace Consyzer.Logging;

internal sealed class LogBuilder
{
    private const string Indent = "\t";

    private readonly StringBuilder _sb = new();

    public LogBuilder Title(string title)
    {
        this._sb.AppendLine(title);
        return this;
    }

    public LogBuilder Line(string label, object? value)
    {
        this._sb.AppendLine($"{label}: {value}");
        return this;
    }

    public LogBuilder InnerLine(string text)
    {
        this._sb.AppendLine($"{Indent}{Indent}{text}");
        return this;
    }

    public LogBuilder IndexedItems<T>(IEnumerable<T> items, Func<T, string> formatter)
    {
        int index = 0;
        foreach (var item in items)
        {
            this._sb.AppendLine($"{Indent}[{index++}] {formatter(item)}");
        }
        return this;
    }

    public LogBuilder IndexedSection<T>(IEnumerable<T> items, Action<LogBuilder, T> renderer)
    {
        int index = 0;
        foreach (var item in items)
        {
            this._sb.AppendLine($"{Indent}[{index++}]");
            renderer(this, item);
        }

        return this;
    }

    public LogBuilder Raw(string raw)
    {
        this._sb.Append(raw);
        return this;
    }

    public string Build() => this._sb.ToString();
}
