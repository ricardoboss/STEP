FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StepLang.LibApi/StepLang.LibApi.csproj", "StepLang.LibApi/"]
RUN dotnet restore "StepLang.LibApi/StepLang.LibApi.csproj"
COPY . .
WORKDIR "/src/StepLang.LibApi"
RUN dotnet build "StepLang.LibApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StepLang.LibApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StepLang.LibApi.dll"]
