# HelloWorld

This echoes the result of /user/myDetails in order to demonstrate successful connection to the API.
```
HelloWorld.exe broker.ajc.json
... returns ...
username:broker.ajc@wspt.co.uk companyId:AJC uniqueID:MUD38EC011-780A-42D3-94DA-FD9063F5DAF9 1 team
teamId:ALL name:All Risks channel:ajc_ALL
```

The settings JSON file has the form
```
{
  "root": "https://sandbox.whitespace.co.uk",
  "bucket": "sandboxbucket",
  "renewableToken": "Login to the platform and copy from the user settings panel, replacing this text"
}
```
