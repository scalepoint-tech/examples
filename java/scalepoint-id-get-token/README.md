# Scalepoint Id Get Token

This example shows the principles behind [Scalepoint Authentication](https://dev.scalepoint.com/authentication)

## Prerequisites

* Java 8
* Self signed X.509 certificate. [Here is a documentation](https://dev.scalepoint.com/authentication/#self-signed-certificate-generation) how you could generate your own.
* import private key of the generated certificate into Java Key Storage (JKS) file 

## Getting started

* Run application specifying the following 4 parameters:
    * "client identifier" - string
        * provided by Scalepoint
    * "certificateFile" - string
        * path to file with self-signed certificate
    * "certificatePassword" - string
        * password for self-signed certificate provided below
    * "scope" - string
        * OAuth2 scope(s) to request access to. I.e. case_integration