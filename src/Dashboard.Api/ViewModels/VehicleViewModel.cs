namespace Dashboard.Api.ViewModels
{
    public class VehicleViewModel
    {
        public VehicleViewModel(string regNumber, bool connected, string ownerUri)
        {
            RegNumber = regNumber;
            Connected = connected;
            Owner = new Link(ownerUri);
        }

        public string RegNumber { get; private set; }
        public bool Connected { get; private set; }
        public Link Owner { get; private set; }
    }
}