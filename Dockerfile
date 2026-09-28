# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["AcademicTrack.API/AcademicTrack.API.csproj", "AcademicTrack.API/"]
COPY ["AcademicTrack.Application/AcademicTrack.Application.csproj", "AcademicTrack.Application/"]
COPY ["AcademicTrack.Domain/AcademicTrack.Domain.csproj", "AcademicTrack.Domain/"]
COPY ["AcademicTrack.Infrastructure/AcademicTrack.Infrastructure.csproj", "AcademicTrack.Infrastructure/"]

RUN dotnet restore "AcademicTrack.API/AcademicTrack.API.csproj"

# Copy full source code and build
COPY . .
WORKDIR "/src/AcademicTrack.API"
RUN dotnet build "AcademicTrack.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "AcademicTrack.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "AcademicTrack.API.dll"]
