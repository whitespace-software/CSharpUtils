# RisksList

Calls the /api/risks endpoint and then prints details, optionally matching a simple filter

- RisksList.exe [settings file] 
- RisksList.exe [settings file] UMR B0999JC12345678
- RisksList.exe [settings file] ID IC12345678
- RisksList.exe [settings file] Status Signed
- RisksList.exe [settings file] InsuredName Acem

The settings file has the form:
```
{
  "root": "https://sandbox.whitespace.co.uk",
  "bucket": "sandboxbucket",
  "renewableToken": "Open platform and copy from the user settings panel, replacing this text"
}
```
