# PRN231 & PRM392's Back End Repository
This is a simple backend repository with a simple database. Its purpose is to practice ASP.NET Web API with 3-Layer Architecture, Entity Framework and show proof of concept for many third party services such as AWS's S3, RDS, EC2, Firebase Cloud Messaging.... Along with it also contains a .yml file for CI/CD on EC2

## 1. Background
Simple E-Commerce app intended for restaurant to put up their food with tags, and users can buy from the app

## 2. Requirements
- You must have an IDE like Visual Studio / Rider
- You must have a DBMS such as Microsoft SQL Server / pgAdmin

## 3. Guideline
- Clone this repo
- Replace connection strings in appsettings.json with your connection strings
- Add an IAM account (through AWS Extension on Visual Studio) to use S3 service to upload images (*in AWSHelper.class*)
- Add a Google Security account (usually a json file) to the API project to use Firebase Cloud Messaging
- Register your Google account with this line of code in Program.cs

  *FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("mykey.json")
});*
- In Program.cs, register your Google Account 
- Open Package Manager Console and use *Update-Database* to create a database

## 4. Clone the front end repo if you like, which can be found here
https://github.com/thanhnd3103/PRM392_FE/tree/main
