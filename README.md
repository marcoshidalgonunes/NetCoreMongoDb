# NetCoreMongoDb

This project contains a front-end app written using [React](https://reactjs.net) and a back-end Web API written in .NET Core.

There is also a test project showing how to use [xUnit with .NET Core](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test), covering test of classes like Controllers and void methods. There are examples of [Fixture](https://xunit.net/docs/shared-context) and [Mock](https://referbruv.com/blog/posts/writing-mocking-unit-tests-in-aspnet-core-using-xunit-and-moq).

## Containers

The components of the project run in containers. The solution of back-end project contains a docker-compose file to orchestrate the front-end, back-end and MongoDb repository. The Mongo image was pulled from [Docker Hub](https://hub.docker.com) and docker-compose has the volume to be created with `docker volume` command.

The project supports both Visual Studio + Docker Desktop on Windows and VS Code on Linux. For Linux it is needed to install docker and docker-compose using the package manager of chosen distro.
