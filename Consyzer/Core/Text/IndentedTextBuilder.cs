using System.Text;

namespace Consyzer.Core.Text;

internal sealed class IndentedTextBuilder
{
    private const string Indent = "\t";

    private readonly StringBuilder _sb = new();

    public IndentedTextBuilder Title(string title)
    {
        this._sb.AppendLine(title);
        return this;
    }

    public IndentedTextBuilder Line(string label, object? value)
    {
        this._sb.AppendLine($"{label}: {value}");
        return this;
    }

    public IndentedTextBuilder InnerLine(string text)
    {
        this._sb.AppendLine($"{Indent}{Indent}{text}");
        return this;
    }

    public IndentedTextBuilder IndexedItems<T>(IEnumerable<T> items, Func<T, string> formatter)
    {
        int index = 0;
        foreach (var item in items)
        {
            this._sb.AppendLine($"{Indent}[{index++}] {formatter(item)}");
        }
        return this;
    }

    public IndentedTextBuilder IndexedSection<T>(IEnumerable<T> items, Action<IndentedTextBuilder, T> renderer)
    {
        int index = 0;
        foreach (var item in items)
        {
            this._sb.AppendLine($"{Indent}[{index++}]");
            renderer(this, item);
        }

        return this;
    }

    public IndentedTextBuilder Raw(string raw)
    {
        this._sb.Append(raw);
        return this;
    }

    public string Build() => this._sb.ToString();
}