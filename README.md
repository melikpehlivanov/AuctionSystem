# AuctionSystem
**Auction System** is an open-source web project where users can create multiple auctions, set the date of the auction and then users are able to bid for the given item. When the auction ends the user with the highest bid wins the item.

# Azure DevOps - Build Status
[![Build status](https://dev.azure.com/melikpehlivanov/AuctionSystem/_apis/build/status/AuctionSystem-ASP.NET%20Core-CI)](https://dev.azure.com/melikpehlivanov/AuctionSystem/_build/latest?definitionId=-1)

## Branches
1. **master** - stable version of the application. Used Clean Architecture principles with CQRS and MediatR. This branch also includes SPA application with ASP.NET Core Api. Down below you will find more information about Clean Architecture.
2. **mvc-with-services**

## Clean Architecture

***

**Here's the basic architecture of this microservice template:**
* Respecting policy rules, with dependencies always pointing inward
* Separation of technology details from the rest of the system
* SOLID
* Single responsibility of each layer
    
***

![CQRS diagram](https://user-images.githubusercontent.com/28671510/85227195-957af480-b3e4-11ea-9898-8dfa42c84117.png)

## Automatically generated users
| Username        	| Password 	| Role          	|
|-----------------	|----------	|---------------	|
| admin@admin.com 	| admin123 	| Administrator 	|
| test1@test.com  	| test123  	| User          	|
| test2@test.com  	| test123  	| User          	|

# Getting started
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

## Set up email notification functionality (required)
1. Register a [SendGrid](https://sendgrid.com/) account.
2. [Create an API key](https://sendgrid.com/docs/ui/account-and-settings/api-keys/#creating-an-api-key).
3. Insert the API key in the following files:
    * *Presentation/Api/appsettings.json*
    * *Presentation/MvcWeb/appsettings.json*
4. If you're using Visual Studio on Windows, set the *Workers/AuctionSystem.Worker.Runner/appsettings.json* file to be always copied to the /bin directory.
5. After running the web app, run the worker in the background in order to automatically send emails to winners of auctions

Example:
```
"SendGrid": {
  "ApiKey": "SG.5******************************************************DO-zfRp"
}
```

## Set up redis cache in Api project(optional)
1. Follow the [docs](https://redis.io/topics/quickstart)
2. Set RedisCacheSettings Enabled property to true
Example:
```
"RedisCacheSettings": {
    "Enabled": true,
    "ConnectionString": "localhost"
  },
```

## I suggest runing the SPA project since it has better UI.
***

**To run the SPA project:**
1. In the /Presentation/Api folder, run in terminal:
```
dotnet run
```
and the project should be running now on https://localhost:5001

2. In the /Presentation/SpaWeb folder, run in terminal:
```
npm start
```
the project should be running on http://localhost:3000

3. Enjoy!!!

**To run the MVC project:**
1. In the /Presentation/MvcWeb folder, run in terminal:
```
dotnet run
```
2. Enjoy!!!
***
