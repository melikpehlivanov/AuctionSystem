# AuctionSystem

## Automatically generated users
| Username        	| Password 	| Role          	|
|-----------------	|----------	|---------------	|
| admin@admin.com 	| admin123 	| Administrator 	|
| test1@test.com  	| test123  	| User          	|
| test2@test.com  	| test123  	| User          	|

## Set up Cloudinary (required)
1. Register a [Cloudinary](https://cloudinary.com/) account.
2. [Create a Cloud, API key and API secret](https://cloudinary.com/documentation/solution_overview#account_and_api_setup).
2. In the *Web/AuctionSystem.Web/appsettings.json* configuration file insert the Cloud name, API key and API secret.

Example:
```
"Cloudinary": {
  "CloudName": "AuctionSystemCloud",
  "ApiKey": "488*********516",
  "ApiSecret": "3m7******************KdS"
}
```

## Set up email functionality (optional)
1. Register a [SendGrid](https://sendgrid.com/) account.
2. [Create an API key](https://sendgrid.com/docs/ui/account-and-settings/api-keys/#creating-an-api-key).
3. Insert the API key in the following files:
    * *Web/AuctionSystem.Web/appsettings.json*
    * *Workers/AuctionSystem.Worker.Runner/appsettings.json*
4. If you're using Visual Studio on Windows, set the *Workers/AuctionSystem.Worker.Runner/appsettings.json* file to be always copied to the /bin directory.
5. After running the web app, run the worker in the background in order to automatically send emails to winners of auctions

Example:
```
"SendGrid": {
  "ApiKey": "SG.5******************************************************DO-zfRp"
}
```

## Development Timeline Visualisation
### [Video:](https://youtu.be/ZdQfUgGFWas)
[![](http://img.youtube.com/vi/ZdQfUgGFWas/0.jpg)](https://youtu.be/ZdQfUgGFWas)
