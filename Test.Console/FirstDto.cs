namespace Test.Console;
#nullable disable
public class FirstDto
{
    public List<AnotherDto> FirstDtos { get; set; }
    public AnotherDto AnotherDtossssss { get; set; }
    public IReadOnlyCollection<AnotherDto> ReadOnlyCollection { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public ushort Height { get; set; }
    public bool IsDumb { get; set; }
}