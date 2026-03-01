# Runtime base image (ASP.NET 8)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Build image (SDK 8)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RL.Backend/RL.Backend.csproj", "RL.Backend/"]
COPY ["RL.Data/RL.Data.csproj", "RL.Data/"]

RUN dotnet restore "RL.Backend/RL.Backend.csproj"
COPY . .

WORKDIR "/src/RL.Backend"
RUN dotnet build "RL.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RL.Backend.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RL.Backend.dll"]