using System.Text;
using System.Text.RegularExpressions;
using AjaxService.Gen.Extensions;
using AjaxService.Gen.Models.Csharp;
using AjaxService.Gen.Models.Typescript;

namespace AjaxService.Gen.TypeUtilities;

public static class Ts
{
    public static TsDirectory TsDirectory { get; } = new();
    public static AssociatedTypeList AssociatedTypes { get; } = new() { "OneOf", "Left", "Right" };
    public static void CreateFunction(StringBuilder strBuilder, CsharpFunction function)
    {
        strBuilder.AfterTab($"public async {function.Name}(", 2); //start param

        var paramLen = function.Parameters.Count - 1;
        var ajaxCall = new AjaxCall();

        foreach (var (parameter, index) in function.Parameters.Select((parameter, index) => (parameter, index)))
        {
            strBuilder.Append($"{parameter.Name}: {ParseType(parameter)}");

            ExtractInformationAboutParameter(parameter, ajaxCall);

            if (index < paramLen)
            {
                strBuilder.Append(", ");
            }
        }

        strBuilder.Append("): Promise<OneOf<Response, "); //end param + start returnType
        var strReturnType = ParseType(function.ReturnType, false);
        strBuilder.Append($"{strReturnType}>> {{\n"); //start body

        IfNeedQueryParameter(strBuilder, function, ajaxCall);

        strBuilder.AppendLine();
        strBuilder.AfterTab($"const response = await FetchAsync<{strReturnType}>(url,\"{function.RequestType}\",", 3);

        IfNeedHeaderParameter(strBuilder, ajaxCall);

        IfNeedBodyParameter(strBuilder, ajaxCall);

        strBuilder.Append(")\n"); //end Fetch request

        strBuilder.AfterTab("if (response.IsRight()) {\n",3);
        strBuilder.AfterTab("return Right.Create(response.Value)\n",4);
        strBuilder.AfterTab("}\n",3);
        strBuilder.AfterTab("return Left.Create(response.Error);\n",3);
        
        strBuilder.AfterTab("}\n\n", 2); //end body
    }
    private static void ExtractInformationAboutParameter(CsharpParameter parameter, AjaxCall ajaxCall)
    {
        switch (parameter.ParameterTypeEnum)
        {
            case ParameterTypeEnum.FromQuery:
                ajaxCall.QueryParameters.Add(QueryParameter.New(parameter.Name, ParseType(parameter, false)));
                break;
            case ParameterTypeEnum.FromHeader:
                ajaxCall.HeaderParameters.Add(parameter.Name);
                break;
            case ParameterTypeEnum.FromBody:
                ajaxCall.BodyParameter = parameter.Name;
                break;
            case ParameterTypeEnum.None:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private static void IfNeedQueryParameter(StringBuilder strBuilder, CsharpFunction function, AjaxCall ajaxCall)
    {
        if (ajaxCall.QueryParameters.Any())
        {
            CreateQueries(strBuilder, ajaxCall.QueryParameters, function.Url);
        }
        else
        {
            strBuilder.AfterTab($"let url = \"{function.Url}\";", 3);
        }
    }
    private static void IfNeedHeaderParameter(StringBuilder strBuilder, AjaxCall ajaxCall)
    {
        if (ajaxCall.HeaderParameters.Any())
        {
            var header = ajaxCall.HeaderParameters.Aggregate("{", (current, headerParam) => current + $"'{headerParam}':`${{{headerParam}}}`,");
            strBuilder.Append(header);
            strBuilder.Append('}');
        }
        else
        {
            strBuilder.Append("null");
        }
    }
    private static void IfNeedBodyParameter(StringBuilder strBuilder, AjaxCall ajaxCall)
    {
        if (ajaxCall.BodyParameter is not null)
        {
            strBuilder.Append($",JSON.stringify({ajaxCall.BodyParameter})");
        }
    }
    private static void CreateQueries(StringBuilder builder, List<QueryParameter> queries, string functionUrl)
    {
        var functionCallsToAppend = new List<string>();
        var singleQueriesToAppend = new List<string>();
        foreach (var item in queries)
        {
            if (item.Type.EndsWith("]"))
            {
                var cleanType = item.Type.Replace("[]", "");
                functionCallsToAppend.Add($"BuildQuery<{cleanType}>(\"{item.Name}\",{item.Name})");
            }
            else
                singleQueriesToAppend.Add($"?{item.Name}=${{{item.Name}}}");
        }

        builder.AfterTab($"let url = \"{functionUrl}\"", 3);

        foreach (var name in functionCallsToAppend)
            builder.Append($"+ {name}");

        foreach (var str in singleQueriesToAppend)
            builder.Append($"+`{str}`");
    }
    private static string ParseType(CsharpTypeBase typeBase, bool addDefaultValue = true)
    {
        var (type, nestedLevel) = RecursiveParse(typeBase);

        var finalType = type + string.Concat(Enumerable.Repeat("[]", nestedLevel));

        if (addDefaultValue)
            finalType += CreateDefaultValueForType(finalType);


        return finalType;
    }
    private static bool ModelExists(TsFileBase tsFile, string className)
    {
        var content = File.ReadAllText(tsFile);
        return CommonExtensions.ClassNameRegex.Matches(content)
            .Cast<Match>()
            .Any(match => match.Groups["name"].Value.Trim() == className.Trim());
    }
    private static string CreateDefaultValueForType(string type)
    {
        string output;

        if (PredefinedType.IsPredefined(type))
            output = PredefinedType.DefaultValue(type);

        else if (type.Contains("[]"))
            output = "[]";

        else
            output = $"new {type}()";

        return $" = {output}";
    }
    private static void CreateModel(CsharpTypeBase b)
    {
        if (b.ClassModel is null)
        {
            CreateModel(b.Child!);
            return;
        }

        AssociatedTypes.Add(b.ClassModel.Name);

        if (ModelExists(TsDirectory.ModelFile!, b.ClassModel.Name))
            return;

        var builder = new StringBuilder("export class ");
        builder.Append(b.ClassModel.Name);
        builder.AppendLine(" {");

        var constructorInput = new string[b.ClassModel.Properties.Count];
        var constructorBody = new StringBuilder();
        foreach (var (property, index) in b.ClassModel.Properties.Select((v, i) => (v, i)))
        {
            var type = property.Name + ": " + ParseType(property);

            builder.AfterTab($"public readonly {type};\n");

            constructorInput[index] = type;
            constructorBody.Tab();
            constructorBody.AfterTab($"this.{property.Name} = {property.Name};\n");
        }

        builder.AppendLine();
        builder.AfterTab($"constructor({string.Join(", ", constructorInput)}) {{\n");
        builder.Append(constructorBody);
        builder.AfterTab('}'); //end constructor


        builder.AppendLine("\n}//-EndClass"); //end class
        File.AppendAllText(TsDirectory.ModelFile!, builder.ToString(), Encoding.UTF8);
    }
    private static (string, int) RecursiveParse(CsharpTypeBase typeBase)
    {
        if (typeBase.IsPredefined)
        {
            return (typeBase.PredefinedType!.ToTypescript(), typeBase.NestedLevel);
        }

        if (!typeBase.IsCollection)
        {
            CreateModel(typeBase);
            return (typeBase.ClassModel!.Name, typeBase.NestedLevel);
        }

        var (t, n) = RecursiveParse(typeBase.Child!);
        return (t, n + typeBase.NestedLevel);
    }
   
}