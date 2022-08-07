# Rewards System

## Goal
A retailer offers a rewards program to its customers awarding points based on each recorded purchase as follows:

For every dollar spent over $50 on the transaction, the customer receives one point.
In addition, for every dollar spent over $100, the customer receives another point.

Ex: for a $120 purchase, the customer receives (120 - 50) x 1 + (120 - 100) x 1 = 90 points.

Given a record of every transaction during a three-month period, calculate the reward points earned for each customer per month and total.

## Quick Start
Clone project:
```console
git clone "https://github.com/DenisTishkevich/RewardsSystem.git"
```

Change DefaultConnection connectionString in file along path RewardsSystem\api\RewardsSystem.API\appsettings.json to your SQL server and any new database name (there will be automatic creation of the database) **(will not be relevant when I figure out Docker)**:
```console
"ConnectionStrings": {
    "DefaultConnection": ""
},
```

If you have Visual Studio, then by opening the solution just run it, if not, then you need to have the .NET 6 platform installed and in the console go to the folder along the path RewardsSystem\api\RewardsSystem.API\ and execute:
```console
dotnet run -p RewardsSystem.API --configuration Development
```

## Used Technologies
.NET 6, C#10, .NET Core Web API, MS SQL, Entity Framework Core, MediatR, Fluent Validations, AutoMapper, Health Check, Logger, Swagger.

## Database Diagram
![image](https://user-images.githubusercontent.com/110542997/183072349-d3dbb8dd-721a-42d8-ac32-d194e983413c.png)

## Swagger
![image](https://user-images.githubusercontent.com/110542997/183075551-3e08d7e7-f0c1-47f2-bc53-5b3792e4da08.png)

## Create Customer API
![image](https://user-images.githubusercontent.com/110542997/183077915-c8b1b401-bf05-4599-8554-3df9c86591c4.png)
To create a customer, the API only accepts a name and, if successful, returns the customer ID. With Fluent Validators, the name cannot be empty and must be between 1 and 80 characters.

## Create Transaction API
![image](https://user-images.githubusercontent.com/110542997/183080387-2916e074-142e-40f7-a472-1b0cdfad86f3.png)
To create a transaction, the API takes a customer ID and a price, and returns the transaction ID if successful. With Fluent Validators, price can be floating point, but limited to 2 decimal places. Also, the price cannot be less than 0. There is also a check for the customer ID, if such a customer does not exist in the database, then we will get an error with the text "There is no customer with this ID.".

## Get Customers With Transactions API
![image](https://user-images.githubusercontent.com/110542997/183082117-d9b0a666-3920-4dce-b8ce-f7d067c1eaf9.png)
To get all customers with their transactions, there is an API that accepts nothing and displays all customers with their transactions. The API, in addition to the ID, the name of the customer and all his transactions for the last 3 months, returns the sum of points for 3 months and the sum of points for each month during the last 3 months.
Below is the response from this API for an example:
```console
[
  {
    "id": 1,
    "name": "Denis",
    "pointsAmountPerMonth": [
      {
        "year": 2022,
        "month": 8,
        "points": 340
      }
    ],
    "pointsAmount": 340,
    "transactions": [
      {
        "date": "2022-08-05T00:00:00",
        "price": 120,
        "points": 90
      },
      {
        "date": "2022-08-05T00:00:00",
        "price": 200,
        "points": 250
      }
    ]
  },
  {
    "id": 5,
    "name": "Eva",
    "pointsAmountPerMonth": [
      {
        "year": 2022,
        "month": 8,
        "points": 0
      }
    ],
    "pointsAmount": 0,
    "transactions": [
      {
        "date": "2022-08-05T00:00:00",
        "price": 10,
        "points": 0
      }
    ]
  },
  {
    "id": 8,
    "name": "Artem",
    "pointsAmountPerMonth": [
      {
        "year": 2022,
        "month": 8,
        "points": 1850
      }
    ],
    "pointsAmount": 1850,
    "transactions": [
      {
        "date": "2022-08-05T00:00:00",
        "price": 1000,
        "points": 1850
      }
    ]
  }
]
```
## Data From The Table Of Customers
![image](https://user-images.githubusercontent.com/110542997/183083483-cb00ed05-6116-4e0b-a625-930ca6982333.png)

## Data From The Table Of Transactions
![image](https://user-images.githubusercontent.com/110542997/183083607-36bee971-4880-4808-9b7a-1e110ea11ca5.png)

## Health Check
To view the status of services, I used Health Check. It is available at the URL + after the port number /hc-ui (to get information about the operation of services in the form of UI) or + /hc-json (to obtain information about the operation of services in the form of JSON). In the screenshot below, I gave an example of how Memory, DBContext and SqlServer work.

UI:
![image](https://user-images.githubusercontent.com/110542997/183087998-b7ac7a2a-829d-47ad-a5d4-5db57146f477.png)

JSON:
![image](https://user-images.githubusercontent.com/110542997/183088079-45bcb4e1-a42e-46d5-ba64-6306c4654075.png)

## Logging
For logging information, I used the standard tool in .NET - ILogger. Below in the screenshot you can see an example of the log after successfully adding a customer.

![image](https://user-images.githubusercontent.com/110542997/183085742-301aa897-55b6-476b-958f-e24d66eea9e9.png)

## MediatR
Using MediatR, I implemented the CQRS pattern, that is, I separated the logic of commands (adding, updating, deleting) and queries (getting data).

## AutoMapper
By using AutoMapper, I have reduced the amount of code that would have to be written to create an object and populate all of its fields.

## Unit Tests
Soon...

## Docker
I have added the docker-compose.yml and DockerFile files to the project, but unfortunately there are some problems and I am trying to figure it out. The database is created, but there are problems with copying the project. I will try to fix this problem and then only one command will remain in the project launch after cloning - docker-compose up.
