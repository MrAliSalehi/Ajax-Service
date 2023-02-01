namespace AjaxService.Gen.Models.Csharp;
#nullable disable
public sealed class CsharpFunction
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (value.EndsWith("async"))
            {
                value = value.Replace("async", "Async");
            }
            else if (!value.EndsWith("async", StringComparison.OrdinalIgnoreCase))
            {
                value += "Async";
            }

            _name = value;
        }
    }
    public CsharpReturnType ReturnType { get; set; } = new();
    public List<CsharpParameter> Parameters { get; set; } = new();
    public string Url { get; set; } = "http://127.0.0.1/";
    /// <summary>
    /// GET,POST,PUT,DELETE
    /// </summary>
    public string RequestType { get; set; } = "GET";
}