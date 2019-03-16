FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview3 AS build_env
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview3
WORKDIR /app
COPY --from=build_env /app/out/ .
ENTRYPOINT ["dotnet", "Starter.Net.Api.dll"]
