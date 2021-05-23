namespace Healin.Application.Requests
{
    public class AddressRequest : RequestBase
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Complement { get; set; }
    }
}
