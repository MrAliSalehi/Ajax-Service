namespace Test.Console;
#nullable disable
public class FirstDto
{
    public List<AnotherDto> FirstDtoList { get; set; }
    public Person AnotherPerson { get; set; }
    public IReadOnlyCollection<AnotherDto> ReadOnlyCollection { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public ushort Height { get; set; }
    public bool IsDumb { get; set; }
}
public sealed class Person
{
    public string Name { get; set; }
}