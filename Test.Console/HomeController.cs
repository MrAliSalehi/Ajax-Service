using System.Text;
using AjaxService.Gen.Dependencies;

namespace Test.Console;

[ApiController]
public class HomeController : Controller
{
    [Ajax("d/da"),HttpGet]
    public static List<AnotherDto> Endpoint_1([FromQuery] string[] strArr,[FromBody] FirstDto[] intArr,[FromQuery]bool someBool)
    {
        return new List<AnotherDto>();
    }

    [Ajax("test/path"),HttpPost]
    public AnotherDto Endpoint2([FromHeader]string fun,[FromHeader]int num,[FromHeader] ulong n, [FromBody] bool someBool)
    {
        return new AnotherDto();
    }

    [Ajax("/api/home"),HttpPut]
    public FirstDto Endpoint3([FromHeader] string strs, [FromQuery] int f, string d)
    {
        return new FirstDto();
    }

    [Ajax("another/Api/d"),HttpDelete]
    public void LetsTestCamelCaseShallWe()
    {
        
    }
    
}

/*[ApiController]
public class AnotherController
{
    [Ajax("test/async")]
    public void TestAsync() { }
}

public class YetAnotherController : ControllerBase
{
    [Ajax("test/async")]
    public void TestAsync() { }
}*/