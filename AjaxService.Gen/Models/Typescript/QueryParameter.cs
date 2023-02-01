namespace AjaxService.Gen.Models.Typescript;

public sealed class QueryParameter
{
    public string Name { get; set; }
    public string Type { get; set; }
    public static QueryParameter New(string name, string type) => new(name, type);
    private QueryParameter(string name, string type)
    {
        Name = name;
        Type = type;
    }
}