# ShopNServe using ABP Framework

This project contains two apps: Auth Server, and Admin Panel.

And it contains two services: Product Catalog service, and Identity Service.

This project was not finished. But I was able to make Auth Server work with the Product Catalog service and the Admin Panel web app.

A user can login through the Admin Panel, and he is able to view the Product Catalog, and the Administrator UI for Tenant management, Users and Roles management, and Settings management.


## How to run

1. Run Docker Desktop


2. Run `up.ps1` inside folder `\etc\docker\`

   This is to start instances of Postgres (on port 5432) and Redis (on port 6379).

   Be sure to terminate existing processes running on those ports before executing the `up.ps1` script.

   Wait for all the containers to finish downloading and running.


3. Open `\ShopNServe\apps\auth-server\ShopNServe.AuthServer.sln` solution in Visual Studio 2022.

   (**Note:** be sure that all the containers in step #2 have finished downloading and are all running before proceeding with the below steps.)

   From the Solution Explorer in VS 2022, right click on the project `ShopNServe.AuthServer.DbMigrator`, then select Debug -> Start New Instance

   This creates two databases: `ShopNServe_AuthServer` and `ShopNServe_Identity`


4. Set two startup projects in `ShopNServe.AuthServer` solution: `ShopNServe.AuthServer.AuthServer` and `ShopNServe.AuthServer.HttpApi.Host`

   Then run the project.


5. Open `\ShopNServe\services\product-catalog\ShopNServe.ProductCatalog.sln` solution in Visual Studio 2022.

   Execute `update-database` inside `ShopNServe.ProductCatalog.EntityFrameworkCore` project. This creates a database named `ShopNServe_ProductCatalog`

   Then run the project.


6. Open `\ShopNServe\services\identity\ShopNServe.Identity.sln` project in Visual Studio 2022 then run.


7. Open `ShopNServe\apps\admin-panel` directory.

   Open terminal from that directory then run command `abp install-libs`.


8. Open `\ShopNServe\apps\admin-panel\ShopNServe.AdminPanel.sln` project in Visual Studio 2022 then run.


9. Open `https://localhost:44331/` in browser.

   Login using username: `admin`, and password: `1q2w3E*`
