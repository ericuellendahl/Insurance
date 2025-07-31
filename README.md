# Sistema de Proposta de Seguro

Um sistema de microserviços para gerenciamento de Propost de seguro e suas contratações, construído com .NET 8, PostgreSQL e arquitetura hexagonal.

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Execução](#execução)
- [Testes](#testes)
- [API Documentation](#api-documentation)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Exemplos de Uso](#exemplos-de-uso)
- [Contribuição](#contribuição)

## 🎯 Visão Geral

O sistema permite que usuários criem Propost de seguro, consultem seu status e efetuem a contratação quando aprovadas. É dividido em dois microserviços principais:

### 🏢 Propostervice
- ✅ Criar Propost de seguro
- ✅ Listar Propost
- ✅ Consultar Propost específica
- ✅ Alterar status da Propost (Em Análise, Aprovada, Rejeitada)
- ✅ Publicar eventos de mudança de status

### 🤝 ContractService
- ✅ Contratar uma Propost (somente se aprovada)
- ✅ Armazenar informações da contratação
- ✅ Comunicar-se com Propostervice para verificar status
- ✅ Consumir eventos de mudança de status

## 🏗️ Arquitetura

O projeto segue a **Arquitetura Hexagonal (Ports & Adapters)** com princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**:

```
┌─────────────────────────────────────────────────────────────┐
│                    MICROSERVIÇOS                            │
├─────────────────────────┬───────────────────────────────────┤
│      Propostervice     │        ContractService            │
├─────────────────────────┼───────────────────────────────────┤
│ ┌─────────────────────┐ │ ┌─────────────────────────────────┐ │
│ │        API          │ │ │              API                │ │
│ │   (Controllers)     │ │ │         (Controllers)           │ │
│ └─────────────────────┘ │ └─────────────────────────────────┘ │
│ ┌─────────────────────┐ │ ┌─────────────────────────────────┐ │
│ │     Adapters        │ │ │           Adapters              │ │
│ │  (Repository,       │ │ │   (Repository, HTTP Client,     │ │
│ │   EventPublisher)   │ │ │      Message Consumer)          │ │
│ └─────────────────────┘ │ └─────────────────────────────────┘ │
│ ┌─────────────────────┐ │ ┌─────────────────────────────────┐ │
│ │    Application      │ │ │          Application            │ │
│ │   (Use Cases,       │ │ │        (Use Cases,              │ │
│ │     DTOs)           │ │ │           DTOs)                 │ │
│ └─────────────────────┘ │ └─────────────────────────────────┘ │
│ ┌─────────────────────┐ │ ┌─────────────────────────────────┐ │
│ │      Domain         │ │ │            Domain               │ │
│ │   (Entities,        │ │ │         (Entities,              │ │
│ │   Value Objects)    │ │ │        Value Objects)           │ │
│ └─────────────────────┘ │ └─────────────────────────────────┘ │
└─────────────────────────┴───────────────────────────────────┘
            │                               │
            └───────────────┬───────────────┘
                            │
            ┌───────────────────────────────┐
            │         Infraestrutura        │
            │  ┌─────────────────────────┐  │
            │  │      PostgreSQL         │  │
            │  │   (Bancos Separados)    │  │
            │  └─────────────────────────┘  │
            │  ┌─────────────────────────┐  │
            │  │       RabbitMQ          │  │
            │  │    (Mensageria)         │  │
            │  └─────────────────────────┘  │
            └───────────────────────────────┘
```

### Comunicação entre Microserviços
- **HTTP**: ContractService consulta Propostervice via Flurl
- **Mensageria**: Propostervice publica eventos via MassTransit/RabbitMQ
- **Bancos Separados**: Cada microserviço possui sua própria tabela

## 🛠️ Tecnologias

### Core
- **.NET 8** - Framework principal
- **C#** - Linguagem de programação
- **PostgreSQL** - Banco de dados relacional
- **Entity Framework Core** - ORM

### Comunicação
- **MassTransit** - Message broker abstraction
- **RabbitMQ** - Message broker
- **Flurl.Http** - Cliente HTTP para comunicação entre serviços

### Validação e Qualidade
- **FluentValidation** - Validação de DTOs
- **xUnit** - Framework de testes
- **Moq** - Mock framework
- **Flurl.Http.Testing** - Testes de HTTP

### DevOps
- **Docker** - Containerização
- **Docker Compose** - Orquestração de containers

## 📋 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [Git](https://git-scm.com/)

## 🚀 Instalação

### 1. Clonar o Repositório
```bash
git clone https://github.com/seu-usuario/insurance-system.git
cd insurance-system
```

### 2. Configurar Variáveis de Ambiente
```bash
# Copiar arquivos de configuração de exemplo
cp src/Propostervice/Propost.API/appsettings.example.json src/Propostervice/Propost.API/appsettings.json
cp src/ContractService/Contract.API/appsettings.example.json src/ContractService/Contract.API/appsettings.json
```

### 3. Subir Infraestrutura
```bash
# Subir PostgreSQL e RabbitMQ
docker-compose up -d postgres rabbitmq

# Aguardar inicialização (cerca de 30 segundos)
docker-compose logs postgres
```

### 4. Executar Migrations
```bash
# Propostervice
dotnet ef database update -p src/Propostervice/Propost.Infrastructure -s src/Propostervice/Propost.API

# ContractService  
dotnet ef database update -p src/ContractService/Contract.Infrastructure -s src/ContractService/Contract.API
```

## ▶️ Execução

### Opção 1: Local Development
```bash
# Terminal 1 - Propostervice
dotnet run --project src/ProposttService/Propost.API

# Terminal 2 - ContractService
dotnet run --project src/ContractService/Contract.API
```

### URLs dos Serviços
- **Propostervice**: http://localhost:5001
- **ContractService**: http://localhost:5002
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **Swagger Propostervice**: http://localhost:5001/swagger
- **Swagger ContractService**: http://localhost:5002/swagger

## 🧪 Testes

### Executar Todos os Testes
```bash
# Script automatizado
chmod +x build-and-test.sh
./build-and-test.sh

# Ou manualmente
dotnet test
```

### Testes por Camada
```bash
# Domain Tests
dotnet test tests/Propost.Tests/Propost.Domain.Tests/
dotnet test tests/Contract.Tests/Contract.Domain.Tests/

# Application Tests  
dotnet test tests/Propost.Tests/Propost.Application.Tests/
dotnet test tests/Contract.Tests/Contract.Application.Tests/

# Adapter Tests
dotnet test tests/Propost.Tests/Propost.Adapters.Tests/
dotnet test tests/Contract.Tests/Contract.Adapters.Tests/
```

### Cobertura de Código
```bash
# Gerar relatório de cobertura
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage

# Instalar ferramenta de relatório (primeira vez)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Gerar relatório HTML
reportgenerator -reports:"coverage/**/coverage.cobertura.xml" -targetdir:"coverage/html"
```

## 📚 API Documentation

### Propostervice Endpoints

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/Propost` | Criar nova Propost |
| GET | `/api/Propost` | Listar todas as Propost |
| GET | `/api/Propost/{id}` | Obter Propost específica |
| PUT | `/api/Propost/{id}/status` | Alterar status da Propost |

### ContractService Endpoints

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/contract` | Contratar Propost |
| GET | `/api/contract` | Listar todas as contratações |
| GET | `/api/contract/{id}` | Obter contratação específica |

### Status de Propost
- `1` - Em Análise
- `2` - Aprovada  
- `3` - Rejeitada

## 📁 Estrutura do Projeto

```
InsuranceSystem/
├── src/
│   ├── Propostervice/
│   │   ├── Propost.API/              # Controllers, Program.cs
│   │   ├── Propost.Application/      # Use Cases, DTOs, Validators
│   │   ├── Propost.Domain/           # Entities, Value Objects, Ports
│   │   ├── Propost.Infrastructure/   # DbContext, Configurations
│   │   └── Propost.Adapters/         # Repositories, Event Publishers
│   └── ContractService/
│       ├── Contract.API/           # Controllers, Program.cs
│       ├── Contract.Application/   # Use Cases, DTOs, Validators
│       ├── Contract.Domain/        # Entities, Value Objects, Ports
│       ├── Contract.Infrastructure/# DbContext, Configurations
│       └── Contract.Adapters/      # Repositories, HTTP Clients, Consumers
├── tests/
│   ├── Propost.Tests/
│   │   ├── Propost.Domain.Tests/     # Testes do domínio
│   │   ├── Propost.Application.Tests/# Testes de casos de uso
│   │   └── Propost.Adapters.Tests/   # Testes de adaptadores
│   └── Contract.Tests/
│       ├── Contract.Domain.Tests/  # Testes do domínio
│       ├── Contract.Application.Tests/# Testes de casos de uso
│       └── Contract.Adapters.Tests/# Testes de adaptadores
├── docker-compose.yml                 # Orquestração de containers
├── build-and-test.sh                  # Script de build e teste
├── setup-databases.sh                 # Script de setup do banco
└── README.md                          # Este arquivo
```

## 💡 Exemplos de Uso

### 1. Criar uma Propost
```bash
curl -X POST http://localhost:5001/api/Propost \
  -H "Content-Type: application/json" \
  -d '{
    "nomeCliente": "João Silva",
    "email": "joao@email.com", 
    "valorCobertura": 10000.00,
    "tipoSeguro": "Vida"
  }'
```

### 2. Listar Propost
```bash
curl http://localhost:5001/api/Propost
```

### 3. Aprovar Propost
```bash
curl -X PUT http://localhost:5001/api/Propost/{id}/status \
  -H "Content-Type: application/json" \
  -d '{
    "PropostId": "{id}",
    "novoStatus": 2
  }'
```

### 4. Contratar Propost
```bash
curl -X POST http://localhost:5002/api/contract \
  -H "Content-Type: application/json" \
  -d '{
    "PropostId": "{id}"
  }'
```

### Cenário Completo com HTTPie
```bash
# 1. Criar Propost
http POST localhost:5001/api/Propost \
  nomeCliente="Maria Silva" \
  email="maria@email.com" \
  valorCobertura:=15000 \
  tipoSeguro="Auto"

# 2. Aprovar Propost (substitua {id})
http PUT localhost:5001/api/Propost/{id}/status \
  PropostId="{id}" \
  novoStatus:=2

# 3. Contratar Propost
http POST localhost:5002/api/contract \
  PropostId="{id}"
```

#### 3. Problemas de Migration
```bash
# Resetar migrations
dotnet ef database drop -p src/Propostervice/Propost.Infrastructure -s src/Propostervice/Propost.API
dotnet ef database update -p src/Propostervice/Propost.Infrastructure -s src/Propostervice/Propost.API
```

## 🤝 Contribuição

### Padrões de Código
- Siga os princípios SOLID
- Use Clean Code practices
- Mantenha cobertura de testes > 80%
- Documente APIs com XML comments
- Use conventional commits
