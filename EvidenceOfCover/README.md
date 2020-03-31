# EvidenceOfCover
Creates an Evidence of Cover pdf using a template
Creates a folder with the PDF version of a contract and all attachments

```
EvidenceOfCover.exe [settings file] [json template] [riskID]
```
where riskID is the unique document ID for the risk. It starts "IC" and is, for instance, taken from the address bar of the browser.

```
EvidenceOfCover.exe --example settings.json 
```
writes out an example settings file:
```
{
  "root": "https://sandbox.whitespace.co.uk",
  "bucket": "sandboxbucket",
  "renewableToken": "Login to the platform and copy from the user settings panel, replacing this text"
}
```
This project includes a sample template file.

- [[Date]] will be replaced by today's date
- [[UMR]], [[Insured]] etc will be replaced by the text of a line item with that MRC Heading
- "table": "signed_lines_table" will be replaced by a table of signed line percentages 
```
    {
        "text": "Date: [[Date]]",
        "alignment": "right",
        "margin": [0, 20, 20, 20]
    }
    ...
    "body": [
        [{
            "text": "Policy Number:",
            "style": "second_title"
        },
        {
            "text": "[[UMR]]"
        }
    ],
    ...
    {
      "layout": {
        "defaultBorder": false
      },
      "style":"table_down",
      "table": "signed_lines_table"
    },
```
 
