FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV TZ="Asia/Shanghai"

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Snap.Hutao.Server.API/Snap.Hutao.Server.API.csproj", "Snap.Hutao.Server.API/"]
RUN dotnet restore "Snap.Hutao.Server.API/Snap.Hutao.Server.API.csproj"
COPY . .
WORKDIR "/src/Snap.Hutao.Server.API"
RUN dotnet build "Snap.Hutao.Server.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Snap.Hutao.Server.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Snap.Hutao.Server.API.dll"]
