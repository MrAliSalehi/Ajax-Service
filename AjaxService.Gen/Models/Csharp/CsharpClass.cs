namespace AjaxService.Gen.Models.Csharp;

public sealed class CsharpClass
{
    public CsharpClass(string className)
    {
        Name = className;
    }
    public string Name { get; }

    public List<CsharpFunction> CsharpFunctions { get; set; } = new();
}