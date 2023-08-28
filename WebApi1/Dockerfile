#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApi1/WebApi1.csproj", "WebApi1/"]
RUN dotnet restore "WebApi1/WebApi1.csproj"
COPY . .
WORKDIR "/src/WebApi1"
RUN dotnet build "WebApi1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libldap-2.4-2 \
    && rm -rf /var/lib/apt/lists/*
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        krb5-user software-properties-common python3-software-properties packagekit \
    && rm -rf /var/lib/apt/lists/*
RUN realm join contoso.com -v --no-password
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi1.dll"]