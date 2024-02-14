#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DotNetCompiler/DotNetCompiler.csproj", "Services/DotNetCompiler/"]
RUN dotnet restore "DotNetCompiler/DotNetCompiler.csproj"
COPY . .
WORKDIR "/src/DotNetCompiler"
RUN dotnet build "DotNetCompiler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetCompiler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetCompiler.dll"]