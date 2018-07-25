# Scalepoint Id Get Token

This sample shows the principles behind [Scalepoint Authentification](https://dev.scalepoint.com/authentication)

## Prerequisites

* [.NET](https://www.microsoft.com/net/download)
* Self signed X.509 certificate. [Here it is documentation](https://dev.scalepoint.com/authentication/#self-signed-certificate-generation) how you could generate your own.

## Getting started

* To build this project run `dotnet build` command
* To run this sample execute comand `dotnet run` with 4 parameters:
  * "client identifier" - string
    * Will be provided by Scalepoint
  * "certificateFile" - string
    * path to file with self-signed certificate
  * "certificatePassword" - string
    * password for self-signed certificate provided below
  * "scope" - string
    * OAuth2 scope(s) to request access to. I.e. case_integration

For example:

```cmd
dotnet run "future_insurance" "ClientCertificate.pfx" "password" "case_integration"
```
