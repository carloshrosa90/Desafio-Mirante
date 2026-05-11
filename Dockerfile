# Build na raiz do repositório: docker build -f Dockerfile .
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Desafio.Dominio/Desafio.Dominio.csproj Desafio.Dominio/
COPY Desafio.Infraestrutura/Desafio.Infraestrutura.csproj Desafio.Infraestrutura/
COPY Desafio.Aplicacao/Desafio.Aplicacao.csproj Desafio.Aplicacao/
COPY Desafio/Desafio.Apresentacao.csproj Desafio/

RUN dotnet restore Desafio/Desafio.Apresentacao.csproj

COPY Desafio.Dominio/ Desafio.Dominio/
COPY Desafio.Infraestrutura/ Desafio.Infraestrutura/
COPY Desafio.Aplicacao/ Desafio.Aplicacao/
COPY Desafio/ Desafio/

WORKDIR /src/Desafio
RUN dotnet publish Desafio.Apresentacao.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Desafio.Apresentacao.dll"]
