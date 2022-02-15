## Build and Run instructions
- Set `ConnectionStrings:DefaultConnection` in `appsettings.Development.json` to desired value. By default it is using `(localdb)\\MSSQLLocalDB` as SQL server.
- Run `WebUI` project from Visual Studio or using command line from its directory by `dotnet run`.
- Swagger UI should be reachable via https://localhost:7225/swagger/index.html

## Some thoughts
- I have spent approximately 12 hours in total. In 8 hours I have managed to write working solution, after that I have worked on code clean up, middleware that cancels request after 5 seconds, integrations tests.
- The design is inspired by https://github.com/jasontaylordev/CleanArchitecture. 
- I used Entity Framework since it was convenient in limited time quickly define my database. The database was normalized so that it is easier to manage the data. However, currently the structure is not ideal as I wanted Square table to use foreign keys for each point. I got stuck while implementing this and decided to have explicitly stored values. 
- Since I followed good clean architectural pattern, I think Squares API can easily evolve to Shapes API. It would be pretty straightforward to add new services that could deal with variety of shapes. Each shape can receive their own controller, have their own commands/queries. So if bussiness comes and dictates a new trendy shape must be used in the API. In domain layer, which does not have any dependencies we define our new entity and so on.
- For fun, I parallelized most inner loop in algortihm that calculates squares. After providing a list with 50 points, it was couple seconds faster but computer froze as it took all threads for calculations. 
- For non-functional requirement of preventing waiting more than 5 seconds for a request, I could not find a nice way to do it properly with status code and useful message information. Instead, I used middleware that simply has a one-off timer and aborts the connection after it elapses.
- Integration tests were done following https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0 
- In integration tests, I could not find a nice way to use non-hardcoded URIs for request types with [FromQuery]. Ideally, dedicated commands/queries should be used.
