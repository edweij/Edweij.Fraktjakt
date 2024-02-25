using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record AddressResponse(string PostalCode, string street, string City, CountryCode Country)
{ }
