namespace AjaxService.Gen.Models.Csharp;

public sealed class CsharpReturnType : CsharpTypeBase , ICsharpTypeConversion<CsharpReturnType>
{
    public CsharpReturnType FromBase(CsharpTypeBase typeBase)
    {
        return new()
        {
            IsCollection = typeBase.IsCollection,
            IsPredefined = typeBase.IsPredefined,
            Child = typeBase.Child,
            ClassModel = typeBase.ClassModel,
            PredefinedType = typeBase.PredefinedType,
            NestedLevel = typeBase.NestedLevel
        };
    }
}