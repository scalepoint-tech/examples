# Scalepoint Id Get Token

This example shows the principles behind [Scalepoint Authentication](https://dev.scalepoint.com/authentication)

## Prerequisites

* [.NET](https://www.microsoft.com/net/download)
  > This example uses modern .NET tooling, also known as `.NET Core SDK`. You can still target classic Windows-only `.NET Framework` with this SDK and this example targets both `.NET Framework 4.8` and `.NET Standard 2.0`.
* Self signed X.509 certificate. [Here it is documentation](https://dev.scalepoint.com/authentication/#self-signed-certificate-generation) how you could generate your own.

## Getting started

* To build and run this example execute comand `dotnet run --framework ?` with 4 parameters:
  * "client identifier" - string
    * provided by Scalepoint
  * "certificateFile" - string
    * path to file with self-signed certificate
  * "certificatePassword" - string
    * password for self-signed certificate provided below
  * "scope" - string
    * OAuth2 scope(s) to request access to. I.e. case_integration

For example:

```cmd
dotnet run --framework net48 "future_insurance" "ClientCertificate.pfx" "password" "case_integration"
```
