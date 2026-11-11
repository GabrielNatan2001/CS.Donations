FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CS.Donations.Worker/CS.Donations.Worker.csproj", "src/CS.Donations.Worker/"]
COPY ["src/CS.Donations.Application/CS.Donations.Application.csproj", "src/CS.Donations.Application/"]
COPY ["src/CS.Donations.Domain/CS.Donations.Domain.csproj", "src/CS.Donations.Domain/"]
COPY ["src/CS.Donations.Infrastructure/CS.Donations.Infrastructure.csproj", "src/CS.Donations.Infrastructure/"]
RUN dotnet restore "src/CS.Donations.Worker/CS.Donations.Worker.csproj"
COPY . .
RUN dotnet publish "src/CS.Donations.Worker/CS.Donations.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CS.Donations.Worker.dll"]
