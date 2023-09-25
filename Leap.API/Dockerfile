FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Leap.Api/Leap.Api.csproj", "Leap.Api/"]
COPY ["Directory.Packages.props", "./"]
RUN dotnet restore "Leap.Api/Leap.Api.csproj"
COPY . .
WORKDIR "/src/StepLang.LibApi"
RUN dotnet build "Leap.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Leap.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Leap.Api.dll"]
