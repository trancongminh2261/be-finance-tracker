# ----- Build Stage -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /MonaDotNetTemplate.API
COPY . .
RUN dotnet publish -c Release -o /publish

# ----- Runtime Stage -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /MonaDotNetTemplate.API
COPY --from=build /publish .
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "MonaDotNetTemplate.API.dll"]