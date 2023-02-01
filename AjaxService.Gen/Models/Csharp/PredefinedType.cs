namespace AjaxService.Gen.Models.Csharp;

public class PredefinedType
{
    private PredefinedType(string value)
    {
        Value = value;
    }
    public string Value { get; }

    public static PredefinedType From(string type) => new(type);

    public string ToTypescript() => Value switch
    {
        "int" or "short" or "float" or "double" or "uint" or "ushort" => "number",
        "long" or "ulong"                                             => "bigint",
        "char" or "string"                                            => "string",
        "bool"                                                        => "boolean",
        _                                                             => "void"
    };
    public static string DefaultValue(string type) => type switch
    {
        "number"  => "0",
        "bigint"  => "undefined",
        "string"  => "\"\"",
        "boolean" => "false",
        _         => throw new NotSupportedException($"the type[{type}] does not have a default value.")
    };
    public static bool IsPredefined(string type) => type switch
    {
        "boolean" or "string" or "number" or "bigint" => true,
        _                                             => false
    };
}