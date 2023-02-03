namespace Test.Console;

[AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
public class ApiController : Attribute
{
    
}

[AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
public class FromBody : Attribute
{
    
}
[AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
public class FromHeader : Attribute
{
    
}
[AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
public class FromQuery : Attribute
{
    
}
//HttpGet, HttpPost, HttpPut, HttpDelete
[AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
public class HttpGet : Attribute
{
    
}
[AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
public class HttpPost : Attribute
{
    
}

[AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
public class HttpPut : Attribute
{
    
}

[AttributeUsage(AttributeTargets.All,AllowMultiple = true)]
public class HttpDelete : Attribute
{
    
}

