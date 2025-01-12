FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet

COPY Core Core
RUN dotnet restore Core/Core.csproj

COPY Persistence Persistence
RUN dotnet restore Persistence/Persistence.csproj

COPY Api Api
RUN dotnet restore Api/Api.csproj


FROM dotnet AS build
RUN dotnet publish Api/Api.csproj -c Release --no-restore --output /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY --from=build /app/published-app /app
WORKDIR "/app"

ENTRYPOINT [ "dotnet", "Api.dll" ]

#docker run -d -p  8080:8080 --name mycontainer myapp:latest