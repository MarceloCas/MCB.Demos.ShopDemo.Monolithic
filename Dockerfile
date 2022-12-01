FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /temp
COPY *.sln .
COPY src/*.csproj ./temp/
RUN dotnet restore
COPY src/. ./temp/
WORKDIR /temp/src
RUN dotnet publish -c release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /temp/src/out ./
ENTRYPOINT ["dotnet", "MCB.Demos.ShopDemo.Monolithic.Services.WebApi.dll"]