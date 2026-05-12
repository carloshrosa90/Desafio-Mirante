# Desafio - API

Documentação rápida para subir a aplicação localmente.

## Pré-requisitos

- **Rodar sem Docker:** .NET SDK 8 e SQL Server local (ou acessível na connection string).
- **Rodar com Docker:** apenas [Docker Desktop](https://www.docker.com/products/docker-desktop/) (o compose sobe o SQL Server no contêiner; não precisa de SQL instalado no Windows).

## Configuração do banco

1. Abra o arquivo `Desafio/appsettings.json`.
2. Ajuste a string de conexão `DefaultConnection` para o seu ambiente.

Exemplo atual:

`Server=localhost;Database=Mirante;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True;`

## Criar o banco de dados (SQL Server)

Execute no **SQL Server Management Studio** ou no **sqlcmd** (ajuste servidor e autenticação conforme seu ambiente). O script abaixo cria a base **Mirante** (se ainda não existir), as tabelas **Status** e **Tarefas** e a chave estrangeira de **int_status** para **Status**.

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

**Observação:** se você já usa **Entity Framework Migrations** (`Update-Database`), evite rodar este script no mesmo banco para não duplicar objetos. Use **ou** migration **ou** este script manual, conforme o fluxo do time.

## Como rodar (Visual Studio)

1. Abra o Visual Studio.
2. **Arquivo** > **Abrir** > **Projeto ou solução** e selecione `Desafio.slnx` na pasta do repositório.
3. No **Gerenciador de Soluções**, clique com o botão direito no projeto **Desafio.Apresentacao** (pasta `Desafio`) e escolha **Definir como Projeto de Inicialização**.
4. (Opcional) Confira o perfil de execução na barra de ferramentas (**http**, **https** ou **IIS Express**), conforme `Desafio/Properties/launchSettings.json`.
5. Pressione **F5** (com depuração) ou **Ctrl+F5** (sem depuração) para iniciar a API.

O navegador pode abrir automaticamente na URL do Swagger (configurada no `launchSettings.json`).

## Como rodar (linha de comando)

Alternativa, no diretório raiz da solução:

```bash
dotnet restore
dotnet build Desafio.slnx
dotnet run --project Desafio/Desafio.Apresentacao.csproj
```

## Swagger

Com a API rodando, abra:

- `http://localhost:5035/swagger`
- ou `https://localhost:7143/swagger`

## Docker (teste local)

Pré-requisito: [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução (WSL 2 atualizado no Windows).

Na **raiz do repositório** (pasta que contém `docker-compose.yml` e `Dockerfile`):

```bash
docker compose up --build
```

Na primeira execução o download das imagens pode demorar. Quando a API subir:

- Swagger: `http://localhost:8080/swagger`
- SQL Server no host (para ferramentas externas, opcional): `localhost,14333` (usuário `sa`, mesma senha definida no `docker-compose.yml`)

O compose sobe o SQL Server e a API; a API aplica **migrations pendentes** na subida (`APPLY_MIGRATIONS_AT_STARTUP`). A senha do `sa` e a connection string estão no arquivo `docker-compose.yml` (uso **somente** para desenvolvimento local).

Após a primeira subida, cadastre registros na tabela **Status** (ou use seed/migration de dados) se precisar testar inclusão/alteração de tarefas com FK de status.

Para encerrar: `Ctrl+C` ou, em outro terminal, `docker compose down`. Para apagar também os dados do banco no volume: `docker compose down -v`.

## Endpoints principais

- `GET /api/status` - lista todos os status
- `GET /api/tarefa` - lista todas as tarefas
- `GET /api/tarefa/filtro?status=1&dataVencimento=2026-05-08T00:00:00` - filtra por status e/ou data de vencimento (filtro por dia ignora hora)
- `POST /api/tarefa` - inclui uma tarefa (corpo JSON: `str_titulo`, `str_descricao`, `int_status`, `dat_vencimento`; `int_id` pode ser `0` ou omitido para novo registro)
- `PUT /api/tarefa/{id}` - altera a tarefa (corpo JSON **sem** `int_id`: apenas `str_titulo`, `str_descricao`, `int_status`, `dat_vencimento`; o id é só na URL); resposta `204` ou `404` se não existir
- `DELETE /api/tarefa/{id}` - exclui a tarefa; resposta `204` ou `404` se não existir

## Migrations (quando necessário)

Se precisar criar nova migration:

```powershell
Add-Migration NomeDaMigration -Project Desafio.Apresentacao -StartupProject Desafio.Apresentacao
```

Se precisar aplicar migrations no banco:

```powershell
Update-Database -Project Desafio.Apresentacao -StartupProject Desafio.Apresentacao
```
