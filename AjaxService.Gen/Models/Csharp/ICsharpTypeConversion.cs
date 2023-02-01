namespace AjaxService.Gen.Models.Csharp;

public interface ICsharpTypeConversion<T> where T : CsharpTypeBase
{
    public abstract T FromBase(CsharpTypeBase typeBase);
}