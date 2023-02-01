using AjaxService.Gen.Extensions;

namespace AjaxService.Gen.Models.Csharp;

public sealed class CsharpProperty : CsharpTypeBase,ICsharpTypeConversion<CsharpProperty>
{
    public string Name { get; set; } = CommonExtensions.GenerateRandomName();

    public CsharpProperty FromBase(CsharpTypeBase typeBase)
    {
        return new CsharpProperty
        {
            IsPredefined = typeBase.IsPredefined,
            IsCollection = typeBase.IsCollection,
            Child = typeBase.Child,
            PredefinedType = typeBase.PredefinedType,
            ClassModel = typeBase.ClassModel,
            NestedLevel = typeBase.NestedLevel
        };
    }
}