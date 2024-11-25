# Usar la imagen base de .NET para producción
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Usar la imagen SDK de .NET para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto .csproj (reemplaza "SistemaLaboral" con el nombre correcto de tu proyecto)
COPY ["SistemaLaboral/SistemaLaboral.csproj", "SistemaLaboral/"]

# Restaurar las dependencias
RUN dotnet restore "SistemaLaboral/SistemaLaboral.csproj"

# Copiar el resto de los archivos de código
COPY . .

# Establecer el directorio de trabajo y compilar la aplicación
WORKDIR "/src/SistemaLaboral"
RUN dotnet build "SistemaLaboral.csproj" -c Release -o /app/build

# Publicar la aplicación en la carpeta /app/publish
FROM build AS publish
RUN dotnet publish "SistemaLaboral.csproj" -c Release -o /app/publish

# Imagen final para producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SistemaLaboral.dll"]
