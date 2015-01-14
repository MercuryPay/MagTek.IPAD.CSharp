MagTek.IPAD.CSharp
==================

VS 2010 C# MagTek IPAD integration with Mercury E2E and Tokenization (also tested in VS 2013 Express for Desktop)

3 step process to integrate to Mercury Web Services.

##Step 1: Create the XML Request
  
Using the XMLHelper class, it is very simple to create the XML Request.
  
Create a Dictionary&lt;string, object&gt; variable and add all the Key/Value pairs.
  
```
invoiceNo = DateTime.Now.ToString("yyMMddhhmmssfff");

// Create Request KeyValuePairs
Dictionary<string, object> requestDictionary = new Dictionary<string, object>();
requestDictionary.Add("MerchantID", merchantID);
requestDictionary.Add("LaneID", laneID);
requestDictionary.Add("TranType", "Credit");
requestDictionary.Add("TranCode", "Sale");
requestDictionary.Add("InvoiceNo", invoiceNo);
requestDictionary.Add("RefNo", invoiceNo);
requestDictionary.Add("Memo", memo);
requestDictionary.Add("Frequency", "OneTime");
requestDictionary.Add("RecordNo", "RecordNumberRequested");
requestDictionary.Add("PartialAuth", "Allow");
requestDictionary.Add("EncryptedFormat", "MagneSafe");
requestDictionary.Add("AccountSource", "Keyed");
requestDictionary.Add("EncryptedBlock", encTrack2);
requestDictionary.Add("EncryptedKey", ksn);
requestDictionary.Add("Purchase", purchase);
requestDictionary.Add("CVVData", cvvData);
requestDictionary.Add("OperatorID", operatorID);

string xmlRequest = XMLHelper.BuildXMLRequest(dictionary).ToString();
```
  
##Step 2: Process the Transaction

Create a service reference to https://w1.mercurydev.net/ws/ws.asmx.

Call the CreditTransaction web method with XML Request and merchant’s Password.

Store the xml string response for further processing.

```
using (MercuryWebServices.wsSoapClient client = new MercuryWebServices.wsSoapClient())
{
  string xmlResponse = client.CreditTransaction(xmlRequest, "xyz");
}
```

##Step 3: Parse the XML Response

Parse the XML Response using the XMLHelper.ParseXMLResponse(string xmlResponse) method.

This method returns a Dictionary&lt;string, string&gt;.

Approved transactions will have a CmdStatus equal to "Approved".

```
Dictionary<string, string> dictionary = XMLHelper.ParseXMLResponse(xmlResponse);

if (dictionary.ContainsKey("CmdStatus")
   && dictionary["CmdStatus"] == "Approved")
{
   // Approved logic goes here
}
else
{
   // Declined/Error logic goes here
}
```

###©2014 Mercury Payment Systems, LLC - all rights reserved.

Disclaimer:
This software and all specifications and documentation contained herein or provided to you hereunder (the "Software") are provided free of charge strictly on an "AS IS" basis. No representations or warranties are expressed or implied, including, but not limited to, warranties of suitability, quality, merchantability, or fitness for a particular purpose (irrespective of any course of dealing, custom or usage of trade), and all such warranties are expressly and specifically disclaimed. Mercury Payment Systems shall have no liability or responsibility to you nor any other person or entity with respect to any liability, loss, or damage, including lost profits whether foreseeable or not, or other obligation for any cause whatsoever, caused or alleged to be caused directly or indirectly by the Software. Use of the Software signifies agreement with this disclaimer notice.
