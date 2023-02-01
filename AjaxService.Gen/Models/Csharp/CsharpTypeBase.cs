namespace AjaxService.Gen.Models.Csharp;

public class CsharpTypeBase
{
    public bool IsPredefined { get; set; }
    public bool IsCollection { get; set; }
    public int NestedLevel { get; set; }
    public CsharpTypeBase? Child { get; set; }
    public PredefinedType? PredefinedType { get; set; }
    public CsharpClassModel? ClassModel { get; set; }
}