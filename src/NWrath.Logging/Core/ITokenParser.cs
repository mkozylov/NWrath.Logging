namespace NWrath.Logging
{
    public interface ITokenParser
    {
        string KeyPattern { get; set; }

        Token[] Parse(string template);
    }
}