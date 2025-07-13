# ----- Build Stage -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish MonaDotNetTemplate.API/MonaDotNetTemplate.API.csproj -c Release -o /publish

# ----- Runtime Stage -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Tạo thư mục Uploads và các thư mục con
RUN mkdir -p /app/Uploads
RUN mkdir -p /app/Uploads/Excel
RUN mkdir -p /app/Uploads/Image
RUN mkdir -p /app/Uploads/Other
RUN mkdir -p /app/Uploads/PDF
RUN mkdir -p /app/Uploads/Word

# Tạo thư mục Template và các thư mục con
RUN mkdir -p /app/Template
RUN mkdir -p /app/Template/Email

COPY --from=build /publish .
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "MonaDotNetTemplate.API.dll"]
