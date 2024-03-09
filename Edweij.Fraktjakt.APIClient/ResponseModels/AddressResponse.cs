using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents a response containing address information, including postal code, street, city, and country code.
/// </summary>
public record AddressResponse(string PostalCode, string Street, string City, CountryCode Country)
{ }
