namespace Dashboard.Api.ViewModels
{
    public class OwnerViewModel
    {
        public OwnerViewModel(string name, string address, string vehiclesUri)
        {
            Name = name;
            Address = address;
            Vehicles = new Link(vehiclesUri);
        }

        public string Name { get; private set; }
        public string Address { get; private set; }
        public Link Vehicles { get; private set; }
    }
}