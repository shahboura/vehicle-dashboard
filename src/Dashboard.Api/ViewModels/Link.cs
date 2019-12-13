namespace Dashboard.Api.ViewModels
{
    public class Link
    {
        public Link(string path)
        {
            Href = path;
        }

        public string Href { get; }
    }
}