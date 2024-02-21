using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient.responseModels;

public record AddressResponse(string PostalCode, string street, string City, CountryCode Country)
{ }
