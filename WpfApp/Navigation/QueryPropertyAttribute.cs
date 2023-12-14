namespace WpfApp.Navigation;

[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
public class QueryPropertyAttribute : Attribute
{
    public QueryPropertyAttribute( string name, string queryId )
    {
        Name = name;
        QueryId = queryId;
    }

    public string Name { get; }
    public string QueryId { get; }
}
