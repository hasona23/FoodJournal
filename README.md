# Food Journal
A place where you can track dietary habits.

# Features
    - Track your meals by storing Food and Meals.
    - Meal Types 
    - Reports
    - Quick recording to frequent meals.
 

# App Structure
the app consist of 3 main parts.
1- FoodJournal.API: Back-end logic resides like storing information and db communication and fetching data
2- FoodJournal.Client: Front-end made with blazor. acts as an interface to help communicate with api.
3- FoodJournal.Shared: Shared data like MODELS

# Tech Stack
1- Lang/Framework: C#/.NET_9
2- Backend: .NET API
3- Endpoints Exploration: SCALAR
4- Frontend: Blazor wasm
5- Database: EntityFramework + SQL Server

#How to Run

### API
-> Go to the Api Directory ("FoodJournal.Api") where the .csproj file is located
-> open terminal then type 	``` dotnet run ```

### Client
-> Go to the Client Direcotry ("FoodJournal.Client")
-> Open Terminal then Type ``` dotnet run ```

NOTE: The Client is useless without API so run API first
