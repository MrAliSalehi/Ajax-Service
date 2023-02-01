namespace AjaxService.Gen.Models.Typescript;

public sealed class AjaxCall
{
    public List<string> HeaderParameters { get; set; } = new();
    public List<QueryParameter> QueryParameters { get; set; } = new();
    public string? BodyParameter { get; set; }
}