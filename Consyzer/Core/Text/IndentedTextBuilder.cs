using System.Text;

namespace Consyzer.Core.Text;

internal sealed class IndentedTextBuilder
{
    private const char Indent = '\t';

    private readonly StringBuilder _sb = new();
    private int _level;

    public IndentedTextBuilder PushIndent()
    {
        ++_level;
        return this;
    }

    public IndentedTextBuilder PopIndent()
    {
        if (_level > 0) --_level;
        return this;
    }

    public IndentedTextBuilder Title(string title)
    {
        _sb.AppendLine(title);
        return this;
    }

    public IndentedTextBuilder Line(string label, object? value)
    {
        _sb.AppendLine($"{IndentString()}{label}: {value}");
        return this;
    }

    public IndentedTextBuilder Line(string label)
    {
        _sb.AppendLine($"{IndentString()}{label}");
        return this;
    }

    public IndentedTextBuilder IndexedItems<T>(IEnumerable<T> items, Func<T, string> formatter)
    {
        int index = 0;
        var indent = IndentString();
        foreach (var item in items)
        {
            _sb.AppendLine($"{indent}[{index++}] {formatter(item)}");
        }

        return this;
    }

    public IndentedTextBuilder IndexedSection<T>(IEnumerable<T> items, Action<IndentedTextBuilder, T> renderer)
    {
        int index = 0;
        var indent = IndentString();
        foreach (var item in items)
        {
            _sb.AppendLine($"{indent}[{index++}]");
            PushIndent();
            renderer(this, item);
            PopIndent();
        }

        return this;
    }

    public string Build() => _sb.ToString();

    private string IndentString() => new(Indent, _level);
}