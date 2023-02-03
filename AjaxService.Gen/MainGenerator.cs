using System.Text;
using AjaxService.Gen.Extensions;
using AjaxService.Gen.Models.Csharp;
using AjaxService.Gen.Models.Typescript;
using AjaxService.Gen.SyntaxReceivers;
using AjaxService.Gen.TypeUtilities;
using Microsoft.CodeAnalysis;

namespace AjaxService.Gen;

[Generator]
public class MainGenerator : ISourceGenerator
{
    //bug : string,number ..etc cant be a valid name in ts for var so do something about it 
    //bug Argument of type 'null' is not assignable to parameter of type 'HeadersInit | undefined'. replace null with {} in FetchAsync parameters
    //note to my self
    //Get cant have body
    //objects can only be [FromBody]
    //simple types by default use [FromQuery]
    //FromBody should use : body:JSON.stringify(objectThatICreated); 

    //array , in query can be like this : https://localhost:7176/Home?intArr=1&intArr=45&intArr=645&intArr=76 
    //result will be an array with items:[1,45,645..etc]
    //jagged arrays is not allowed here, it would perform the same as a normal array ->
    // &anotherArr=ww&anotherArr=wstringwwe&anotherArr=rtew3&anotherArr=a3453451&anotherArr=aada v

    ///CHECKED BigInt(2) def value 
    ///CHECKED if it does not have any Attr, Simple types should go with [FromQuery] and complex objects should use [FromBody]
    ///CHECKED FromQuery(Another?age=12) and FromRoute(Another/12/str) can be infinite.
    ///CHECKED throw if there is a complex object and another simple type flagged as [FromBody]
    ///CHECKED throw if there is more than 1 complex object
    ///CHECKED only one FromBody
    ///CHECKED always Content-Type : application/json
    ///CHECKED dont support FromForm , not worth it
    /// 
    private static string? _basePath;

    private static string? _tsDir;
    public void Execute(GeneratorExecutionContext context)
    {
        Init(context);

        var classes = ((MainSyntaxReceiver)context.SyntaxReceiver!).MethodFinder.CsharpClasses;

        var strBuilder = new StringBuilder(); //start module
        foreach (var csharpClass in classes)
        {
            CreateClass(strBuilder, csharpClass);
            strBuilder.AppendLine();
            strBuilder.AppendLine();
        }

        strBuilder.AppendLine("}"); //end module

        AppendImports(strBuilder);

        File.AppendAllText(Ts.TsDirectory.AjaxServiceFile!, strBuilder.ToString());
    }
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new MainSyntaxReceiver());
/*#if DEBUG
        if (!Debugger.IsAttached) Debugger.Launch();
#endif*/
    }
    private static void CreateClass(StringBuilder strBuilder, CsharpClass csharpClass)
    {
        strBuilder.AfterTab("export class ");
        strBuilder.Append(csharpClass.Name);
        strBuilder.Append(" {\n"); //start class

        foreach (var function in csharpClass.CsharpFunctions)
        {
            Ts.CreateFunction(strBuilder, function);
        }

        strBuilder.AfterTab('}'); //end class
    }
     private static void AppendImports(StringBuilder strBuilder)
    {
        strBuilder.Append("import {"); //start imports
        strBuilder.Append(string.Join(",", Ts.AssociatedTypes));
        strBuilder.Append("} from \"./Models\";"); //end imports
    }
    private static void Init(GeneratorExecutionContext context)
    {
        _basePath ??= context.Compilation.SyntaxTrees.First(x => x.HasCompilationUnitRoot).FilePath;

        _tsDir ??= Path.Combine(Path.GetDirectoryName(_basePath)!, "tsFiles");

        if (!Directory.Exists(_tsDir))
            Directory.CreateDirectory(_tsDir);

        var ajaxServicePath = Path.Combine(_tsDir!, "AjaxService.ts");
        var modelFilePath = Path.Combine(_tsDir!, "Models.ts");

        File.Create(ajaxServicePath).Dispose();
        File.Create(modelFilePath).Dispose();

        Ts.TsDirectory.AjaxServiceFile = Models.Typescript.AjaxService.From(ajaxServicePath);
        Ts.TsDirectory.ModelFile = ModelsTs.From(modelFilePath);
    }
}