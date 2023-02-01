namespace AjaxService.Gen.Models.Typescript;

public class AssociatedTypeList : List<string>
{
    public new void Add(string type)
    {
        if (this.All(p=>p!=type))
            base.Add(type);
    }
}