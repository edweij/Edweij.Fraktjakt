# Fraktjakt API Client in C#
 
Fraktjakt.APIClient is a library written in c# to integrate with Fraktjakt.

Currently supporting the following endpoints:  
- Query API to search for shipping services
- Requery API to get a cached response which is much quicker 
- Order API to create an order from a previously created shipment; or to create and order directly
- Shipment API to create a preliminary shipment in Fraktjakt
- Track & Trace API to track created shipments
- Shipping Documents API to get hold of shipping documents associated with created shipments

Download latest manuals from faktjakt to read about theie API
https://www.fraktjakt.se/om_fraktjakt/documentation?locale=en
 
## Installation

.NET CLI
	> dotnet add package Edweij.Fraktjakt.APIClient

Package Manager
	PM> NuGet\Install-Package Edweij.Fraktjakt.APIClient

PackageReference
	&lt;PackageReference Include="Edweij.Fraktjakt.APIClient" Version="**latest version**" /&gt;
 
## Usage
 
To use this API client you need an integration configured for your/your companys account on fraktjakt.se, please read their manuals for instructions.

Add your integrations Consignor ID and Consignor key to your project in a way that fits you best.

In your program.cs file setup DI with the clients helper method
	builder.Services.AddFraktjaktClient(**your id**, **your key**);

Inject IFraktjaktClient where needed

 
## Credits
 
Edvin Weijmers (@edweij)
 
## License
 
The MIT License (MIT)

Copyright (c) 2024 Edvin Weijmers

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.