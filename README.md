## GardenIt

GardenIt is an online garden management system that lets you keep track of your house plants and see how often you need to water them

It is intended for educational purposes to demonstrate how to build a full-stack web application. The application is built using .NET Core 3.1 and C#.

## Components

### Engine
The engine, or logic layer of the application, consists of 'Plants', and 'Waterings' models. Each plant can have many waterings associated with it. Any time the user waters a plant, a 'Watering' instance is added to the plant's collection of waterings. 

The NextWateringDate for a plant is calculated by taking the most recent watering date, and adding the DaysBetweenWaterings value onto it. This is displayed to the user.

The 'Garden' class is the main engine class. This is where the main actions of the application are defined. This includes CRUD operations on the Plant object, as well as the ability to Water a plant. 


### Database
The application uses a Postgres database provisioned using a free tier of [ElephantSQL](https://www.elephantsql.com/). There are separate databases for dev and prod. Entity Framework Core is used for the database and a storage interface layer sits on top of the EF layer.

The Garden engine class specifies only the interface, and is not tied to the EF Core implementation, so it can be swapped out without affecting the logic.


### Views and Controllers
The operations exposed to the user include the CRUD operations on a plant as well as the ability to water a plant. This is all handled from the PlantController.

The Form.cshtml view is re-used for both creation and editing, and is toggled using a trigger on the ViewBag. 

### Identity
The identity was scaffolded using the .NET core identity tool. It largely uses the default setup, however, integration with SendGrid has been added for confirming email addresses on registration. 

The following command can be used to generate the custom identity pages:
`dotnet aspnet-codegenerator identity -dc GardenIt.Models.Storage.ApplicationDbContext --files "Account.Register;Account.Login;Account.RegisterConfirmation"`


## Deploying
To deploy this application:
- Create an App Service in Azure
- Ensure the Azure App Service extension is installed in VS Code
- Ensure the extension is synced to your Azure account
- Create a publish folder: `dotnet public -c Release -o ./publish`
- Deploy by right clicking on the publish folder and deploying to the web app

The application is available at: https://www.gardenit.azurewebsites.net


## Resources

[Uploading images tutorial](https://www.youtube.com/watch?v=QpJvqiHl1Fo)

[Scaffolding Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-3.1&tabs=netcore-cli)

[SendGrid integration](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-3.1&tabs=visual-studio)

[Andrew Lock blog](https://andrewlock.net/customising-aspnetcore-identity-without-editing-the-pagemodel/)

