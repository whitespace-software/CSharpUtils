# RiskPack

Creates a folder with the PDF version of a contract and all attachments

```
RiskPack.exe settings.json [riskID]
```
where riskID is the unique document ID for the risk. It starts "IC" and is, for instance, taken from the address bar of the browser.

```
RiskPack.exe --example settings.json 
```
writes out an example settings file:
```
{
  "root": "https://sandbox.whitespace.co.uk",
  "bucket": "sandboxbucket",
  "renewableToken": "Login to the platform and copy from the user settings panel, replacing this text"
}
```
