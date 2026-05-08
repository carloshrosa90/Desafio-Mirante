# Desafio - API

Documentacao rapida para subir a aplicacao localmente.

## Pre-requisitos

- .NET SDK 8 instalado
- SQL Server local instalado e em execucao

## Configuracao do banco

1. Abra o arquivo `Desafio/appsettings.json`.
2. Ajuste a string de conexao `DefaultConnection` para o seu ambiente.

Exemplo atual:

`Server=localhost;Database=Mirante;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True;`

## Como rodar

No diretorio raiz da solucao (`Desafio`), execute:

```bash
dotnet restore
dotnet build Desafio.slnx
dotnet run --project Desafio/Desafio.Apresentacao.csproj
```

## Swagger

Com a API rodando, abra:

- `http://localhost:5035/swagger`
- ou `https://localhost:7143/swagger`

## Endpoints principais

- `GET /api/status` - lista todos os status
- `GET /api/tarefa` - lista todas as tarefas
- `GET /api/tarefa/filtro?status=1&dataVencimento=2026-05-08T00:00:00` - filtra por status e/ou data de vencimento (filtro por dia ignora hora)

## Migrations (quando necessario)

Se precisar criar nova migration:

```powershell
Add-Migration NomeDaMigration -Project Desafio.Apresentacao -StartupProject Desafio.Apresentacao
```

Se precisar aplicar migrations no banco:

```powershell
Update-Database -Project Desafio.Apresentacao -StartupProject Desafio.Apresentacao
```
