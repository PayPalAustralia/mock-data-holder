## Getting Started
This file contains the steps to run and debug this project in a MacOS development environment:

- Download and install dotnet 5 for MacOS.
- Set the environment variable ASPNETCORE_ENVIRONMENT to Development.
- Import the Mock CDR CA cert (Source/CDR.DataHolder.API.Gateway.mTLS/Certificates/ca.crt) with KeyChain Access from MacOS and set it to Always Trust.
- Update the path for databases and logs for: 
    * Source/CDR.DataHolder.Admin.API/appsettings.Development.json (replace with Source/CDR.DataHolder.Admin.API/appsettings.Development.MAC.json)
    * Source/CDR.DataHolder.IdentityServer/appsettings.Development.json (replace with Source/CDR.DataHolder.IdentityServer/appsettings.Development.MAC.json)
    * Source/CDR.DataHolder.IdentityServer/appsettings.json (replace with Source/CDR.DataHolder.IdentityServer/appsettings.MAC.json)
    * Source/CDR.DataHolder.Manage.API/appsettings.Development.json (recplace with Source/CDR.DataHolder.Manage.API/appsettings.Development.MAC.json)
    * Source/CDR.DataHolder.Manage.API/appsettings.json (recplace with Source/CDR.DataHolder.Manage.API/appsettings.MAC.json)
    * Source/CDR.DataHolder.Resource.API/appsettings.Development.json (replace with Source/CDR.DataHolder.Resource.API/appsettings.Development.MAC.json)
- Run Source/Start-DataHolder.sh to start all the sub-projects (you may need to install ttab https://github.com/mklement0/ttab).

NOTE: If an error "No usable version of libssl was found" is being raised by dotnet in the Consent section, after hitting "I Confirm". Please update to the latest version of openssl (LibreSSL 3.4.2)
and if is still not working create a link for the libssl library added (ln -s /usr/local/lib/libssl.50.dylib /usr/local/lib/libssl.1.0.0.dylib)