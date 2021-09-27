# NetCoreMongoDb

This project is adapted from [Create a web API with ASP.NET Core and MongoDB](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-5.0&tabs=visual-studio) to use [TDD](https://en.wikipedia.org/wiki/Test-driven_development) process.

There is a test project showing how to use [xUnit with .NET Core](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test), covering test of classes like Controllers and void methods. There are examples of [Fixture](https://xunit.net/docs/shared-context) and [Mock](https://referbruv.com/blog/posts/writing-mocking-unit-tests-in-aspnet-core-using-xunit-and-moq).

The components of the project run in containers. The solution of back-end project contains a docker-compose file to orchestrate the front-end, back-end and MongoDb repository. The Mongo image was pulled from [Docker Hub](https://hub.docker.com) and docker-compose has the volume to be created with `docker volume` command.
