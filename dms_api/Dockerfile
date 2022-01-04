#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DepartmentManagementSystem/dms_api/dms_api.csproj", "DepartmentManagementSystem/dms_api/"]
RUN dotnet restore "DepartmentManagementSystem/dms_api/dms_api.csproj"
COPY . .
WORKDIR "/src/DepartmentManagementSystem/dms_api"
RUN dotnet build "dms_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dms_api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dms_api.dll"]