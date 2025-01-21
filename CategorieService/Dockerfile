# Étape 1 : Utilisation de l'image runtime pour la production
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Étape 2 : Utilisation de l'image SDK pour la construction
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copie du fichier .csproj de GateWay (ou categorieService) et restauration des dépendances
COPY ["CategorieService/CategorieService.csproj", "CategorieService/"]
RUN dotnet restore "CategorieService/CategorieService.csproj"

# Copie du reste du code source dans le conteneur
COPY . .

# Étape 3 : Construction de l'application
WORKDIR "/src/CategorieService"
RUN dotnet build "CategorieService.csproj" -c Release -o /app/build

# Étape 4 : Publication de l'application
FROM build AS publish
RUN dotnet publish "CategorieService.csproj" -c Release -o /app/publish

# Étape 5 : Image finale avec l'application publiée
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CategorieService.dll"]
