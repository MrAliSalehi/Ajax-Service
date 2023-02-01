using System.Text;
using System.Text.RegularExpressions;
using AjaxService.Gen.Models.Csharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AjaxService.Gen.Extensions;

public static class CommonExtensions
{
    private const string ValidChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM_";
    public static readonly Regex ClassNameRegex = new("class (?'name'.+) *{", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
    internal static T FirstParent<T>(this AttributeSyntax attributeSyntax)
    {
        var parent = attributeSyntax.Parent;
        while (true)
            switch (parent)
            {
                case null:
                    throw new ArgumentNullException($"the {typeof(T).Name} was not found");
                case T t:
                    return t;
                default:
                    parent = parent.Parent;
                    break;
            }
    }
    internal static string GenerateRandomName()
    {
        var output = new StringBuilder();
        var rnd = new Random();
        for (var i = 0; i < 5; i++)
        {
            output.Append(ValidChars[rnd.Next(0, ValidChars.Length)]);
        }

        output.Append(rnd.Next(60));
        return output.ToString();
    }
    internal static void AfterTab(this StringBuilder builder, string str, ushort tabCount = 1)
    {
        builder.Tab(tabCount);
        builder.Append(str);
    }
    internal static void AfterTab(this StringBuilder builder, char ch)
    {
        builder.Tab();
        builder.Append(ch);
    }
    internal static void Tab(this StringBuilder builder, ushort count = 1) => builder.Append(new string(' ', count * 4));
    internal static ParameterTypeEnum GetParameterType(this string str)
    {
        return str switch
        {
            nameof(ParameterTypeEnum.FromBody)   => ParameterTypeEnum.FromBody,
            nameof(ParameterTypeEnum.FromHeader) => ParameterTypeEnum.FromHeader,
            nameof(ParameterTypeEnum.FromQuery)  => ParameterTypeEnum.FromQuery,
            _                                    => ParameterTypeEnum.None
        };
    }
    internal static string GetRequestType(this RequestTypeEnum type)
    {
        return type switch
        {
            RequestTypeEnum.HttpPost   => "POST",
            RequestTypeEnum.HttpPut    => "PUT",
            RequestTypeEnum.HttpDelete => "DELETE",
            RequestTypeEnum.HttpGet    => "GET",
            _                          => "GET"
        };
    }
    internal static void ForEachAttr(this SyntaxList<AttributeListSyntax> attributeList, Action<string> action)
    {
        foreach (var attribute in attributeList.SelectMany(attributeListSyntax => attributeListSyntax.Attributes))
        {
            if (attribute.Name is IdentifierNameSyntax identifierNameSyntax)
            {
                action(identifierNameSyntax.Identifier.ValueText);
            }
        }
    }
}