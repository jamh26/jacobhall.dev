#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["jacobhall.dev/jacobhall.dev.csproj", "jacobhall.dev/"]
RUN dotnet restore "jacobhall.dev/jacobhall.dev.csproj"
COPY . .
WORKDIR "/src/jacobhall.dev"
RUN dotnet build "jacobhall.dev.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "jacobhall.dev.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "jacobhall.dev.dll"]