# FIAP Cloud Games – Catalog API

Microsserviço responsável pelo gerenciamento do catálogo de jogos (CRUD) da plataforma FIAP Cloud Games.

---

## 🚀 Tecnologias Utilizadas

- **.NET 9** (Web API)
- **Entity Framework Core**
- **SQL Server**
- **Docker & Docker Compose**
- **JWT** (autenticação)
- **Swagger** (documentação da API)
- **Temporal Tables** (histórico de alterações)

---

## 🏗️ Como Funciona

O serviço de Catalog expõe endpoints HTTP para gerenciamento de jogos e é acessado via **API Gateway**.

### Fluxo de Acesso

```
Cliente (App/Web)
       │
       ▼
 API Gateway (/games/**)
       │
       ▼
   Catalog API
       │
       ▼
   DB Games
```

---

## ⚙️ Como Executar o Projeto

### 🔧 Pré-requisitos

- **Docker Desktop**
- **.NET 9 SDK**
- Ter o `appsettings.Development.json` com o valor:
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "FIAPCloudGames": "Data Source=localhost;Persist Security Info=False;User ID=usuario;Password=senha;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True;Application Name=SQL Server Management Studio;Command Timeout=30"
  },
  "JwtSettings": {
    "Secret": "FiapCloudGames_#2025_TechChallenge_JWT_0x3E8fG7H9jK2L5mN8pQ1rT4uV7wX0yZ",
    "ExpiryMinutes": 60
  },
  "Aws": {
    "Region": "us-east-1",
    "ServiceUrl": "http://host.docker.internal:4566",
    "AwsAccessKeyId": "",
    "AwsSecretAccessKey": "",
    "Token": ""
  }
}
---

### **1. Subir toda a infraestrutura (via Orchestration)**

A infraestrutura completa é gerenciada pelo projeto Orchestration. Na raiz do projeto `FIAP.Cloud.Games.Orchestration`, execute:

```bash
# Liberar execução de scripts (caso necessário)
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

# Subir toda a aplicação (build, migrations, docker compose)
.\run_all.sh
```

Este comando executa automaticamente:
- Build dos projetos
- Aplicação das migrations no banco de dados
- Execução do Docker Compose

---

### **2. Configurar o API Gateway**

Ainda na raiz do Orchestration, execute:

```bash
./create-gateways.sh
```

O script irá criar o gateway e exibir o endpoint gerado. As rotas do Catalog serão apresentadas no terminal.

---

### Autenticação

As requisições exigem autenticação **JWT**. Obtenha o token pelo microsserviço de **Users** e inclua no header:

```
Authorization: Bearer <token>
```

---

## 📘 Observações Importantes

- O serviço **possui Swagger** para documentação e testes dos endpoints.
- A autenticação **JWT é validada internamente** pela própria API — o API Gateway atua apenas como roteador.
- A persistência é feita no **SQL Server**, com dados mantidos entre reinicializações.
- O histórico de alterações é registrado via **Temporal Tables** no banco de dados.
