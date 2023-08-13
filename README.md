# Task Management App [coding challenge]

The application allows users to perform the following tasks:
1. Add new tasks
2. Update task status
3. Show the list of tasks and their status

## Launch the project

1. Open a terminal in the root directory
2. Spin up Docker containers for RabbitMQ and SQL Server using docker-compose:
```
docker-compose -f docker-compose.yml up -d
```
3. Launch the app in your IDE (e.g. Visual Studio) or using dotnet CLI:
```
dotnet run --project TaskManagement.API
```
4. Open `/swagger` URL in the browser on the specified port (`localhost:5026` by default)
