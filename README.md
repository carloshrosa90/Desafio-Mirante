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

## Criar o banco de dados (SQL Server)

Execute no **SQL Server Management Studio** ou no **sqlcmd** (ajuste servidor e autenticacao conforme seu ambiente). O script abaixo cria a base **Mirante** (se ainda nao existir), as tabelas **Status** e **Tarefas** e a chave estrangeira de **int_status** para **Status**.

```sql
IF DB_ID(N'Mirante') IS NULL
    CREATE DATABASE [Mirante];
GO

USE [Mirante];
GO

CREATE TABLE [dbo].[Status](
    [int_id] [int] IDENTITY(1,1) NOT NULL,
    [str_nome] [varchar](50) NOT NULL,
    [sta_ativo] [bit] NOT NULL,
    PRIMARY KEY CLUSTERED ([int_id] ASC)
        WITH (
            PAD_INDEX = OFF,
            STATISTICS_NORECOMPUTE = OFF,
            IGNORE_DUP_KEY = OFF,
            ALLOW_ROW_LOCKS = ON,
            ALLOW_PAGE_LOCKS = ON,
            OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
        ) ON [PRIMARY]
) ON [PRIMARY];
GO

CREATE TABLE [dbo].[Tarefas](
    [int_id] [int] IDENTITY(1,1) NOT NULL,
    [str_titulo] [varchar](250) NOT NULL,
    [str_descricao] [varchar](max) NOT NULL,
    [int_status] [int] NOT NULL,
    [dat_vencimento] [datetime] NOT NULL,
    PRIMARY KEY CLUSTERED ([int_id] ASC)
        WITH (
            PAD_INDEX = OFF,
            STATISTICS_NORECOMPUTE = OFF,
            IGNORE_DUP_KEY = OFF,
            ALLOW_ROW_LOCKS = ON,
            ALLOW_PAGE_LOCKS = ON,
            OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
        ) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

ALTER TABLE [dbo].[Tarefas] WITH CHECK ADD CONSTRAINT [FK_int_status]
    FOREIGN KEY ([int_status]) REFERENCES [dbo].[Status] ([int_id]);
GO

ALTER TABLE [dbo].[Tarefas] CHECK CONSTRAINT [FK_int_status];
GO
```

**Observacao:** se voce ja usa **Entity Framework Migrations** (`Update-Database`), evite rodar este script no mesmo banco para nao duplicar objetos. Use **ou** migration **ou** este script manual, conforme o fluxo do time.

## Como rodar (Visual Studio)

1. Abra o Visual Studio.
2. **Arquivo** > **Abrir** > **Projeto ou solucao** e selecione `Desafio.slnx` na pasta do repositorio.
3. No **Gerenciador de Solucoes**, clique com o botao direito no projeto **Desafio.Apresentacao** (pasta `Desafio`) e escolha **Definir como Projeto de Inicializacao**.
4. (Opcional) Confira o perfil de execucao na barra de ferramentas (**http**, **https** ou **IIS Express**), conforme `Desafio/Properties/launchSettings.json`.
5. Pressione **F5** (com depuracao) ou **Ctrl+F5** (sem depuracao) para iniciar a API.

O navegador pode abrir automaticamente na URL do Swagger (configurada no `launchSettings.json`).

## Como rodar (linha de comando)

Alternativa, no diretorio raiz da solucao:

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
- `POST /api/tarefa` - inclui uma tarefa (corpo JSON: `str_titulo`, `str_descricao`, `int_status`, `dat_vencimento`; `int_id` pode ser `0` ou omitido para novo registro)
- `PUT /api/tarefa/{id}` - altera a tarefa (corpo JSON **sem** `int_id`: apenas `str_titulo`, `str_descricao`, `int_status`, `dat_vencimento`; o id e so na URL); resposta `204` ou `404` se nao existir
- `DELETE /api/tarefa/{id}` - exclui a tarefa; resposta `204` ou `404` se nao existir

## Migrations (quando necessario)

Se precisar criar nova migration:

```powershell
Add-Migration NomeDaMigration -Project Desafio.Apresentacao -StartupProject Desafio.Apresentacao
```

Se precisar aplicar migrations no banco:

```powershell
Update-Database -Project Desafio.Apresentacao -StartupProject Desafio.Apresentacao
```
