namespace AjaxService.Gen.Models.Typescript;

public class TsFileBase
{
    protected TsFileBase(string path)
    {
        Path = path;
    }
    public string Path { get; }
    public static implicit operator string(TsFileBase file) => file.Path;
}