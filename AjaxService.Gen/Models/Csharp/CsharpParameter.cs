using AjaxService.Gen.Extensions;

namespace AjaxService.Gen.Models.Csharp;

public sealed class CsharpParameter : CsharpTypeBase,ICsharpTypeConversion<CsharpParameter>
{
    public string Name { get; set; } = CommonExtensions.GenerateRandomName();
    public ParameterTypeEnum ParameterTypeEnum { get; set; } = ParameterTypeEnum.None;
    public CsharpParameter FromBase(CsharpTypeBase typeBase) {
        
        return new CsharpParameter
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