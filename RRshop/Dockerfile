FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RRshop.csproj", "RRshop.csproj"]
RUN dotnet restore "RRshop.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "RRshop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RRshop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV BOT_TOKEN="5814797478:AAG-PMenYnjT4QnmeMc0l2SDBm9IZ7CPb6Q"
ENV ASPNETCORE_ENVIRONMENT="Development"
ENTRYPOINT ["dotnet", "RRshop.dll"]