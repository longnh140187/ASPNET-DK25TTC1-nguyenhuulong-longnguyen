# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/RecipeWebsite.Web.csproj .
RUN dotnet restore RecipeWebsite.Web.csproj

COPY src/ .
RUN dotnet publish RecipeWebsite.Web.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "RecipeWebsite.Web.dll"]
