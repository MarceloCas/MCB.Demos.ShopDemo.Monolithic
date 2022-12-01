FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/", "."]
RUN dotnet restore "/src/MCB.Demos.ShopDemo.Monolithic.Services.WebApi/MCB.Demos.ShopDemo.Monolithic.Services.WebApi.csproj"
WORKDIR "/src/MCB.Demos.ShopDemo.Monolithic.Services.WebApi"
RUN dotnet build "MCB.Demos.ShopDemo.Monolithic.Services.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MCB.Demos.ShopDemo.Monolithic.Services.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MCB.Demos.ShopDemo.Monolithic.Services.WebApi.dll"]