# �tape 1 : Utilisation de l'image runtime pour la production
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# �tape 2 : Utilisation de l'image SDK pour la construction
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copie du fichier .csproj de GateWay (ou categorieService) et restauration des d�pendances
COPY ["GateWay/GateWay.csproj", "GateWay/"]
RUN dotnet restore "GateWay/GateWay.csproj"

# Copie du reste du code source dans le conteneur
COPY . .

# �tape 3 : Construction de l'application
WORKDIR "/src/GateWay"
RUN dotnet build "GateWay.csproj" -c Release -o /app/build

# �tape 4 : Publication de l'application
FROM build AS publish
RUN dotnet publish "GateWay.csproj" -c Release -o /app/publish

# �tape 5 : Image finale avec l'application publi�e
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GateWay.dll"]
