namespace AjaxService.Gen.Dependencies;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class Ajax : Attribute
{
    //[StringSyntax(StringSyntaxAttribute.Uri)]
    public Ajax(string url)
    {
        Url = url ?? throw new ArgumentNullException($"{nameof(url)} cannot be null");
    }
    public string Url { get;}
}
