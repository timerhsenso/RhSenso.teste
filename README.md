# 🏢 RhSenso - Sistema ERP de Gestão de RH

Sistema ERP de Gestão de Recursos Humanos construído com **ASP.NET Core 8**, seguindo os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**.

## 🎯 Características Principais

- **Multi-tenant (IdSaas)**: Suporte nativo a múltiplos clientes
- **Clean Architecture**: Separação clara de responsabilidades
- **CQRS com MediatR**: Separação entre comandos e consultas
- **Entity Framework Core**: ORM com Code-First e Migrations
- **Segurança Granular**: Controle de permissões por usuário, grupo e funcionalidade
- **Auditoria Automática**: Rastreamento de criação e modificação de registros

## 🏗️ Arquitetura

```
RhSenso/
├── src/
│   ├── RhS.Web/                    # Interface MVC
│   ├── RhS.Api/                    # API REST
│   ├── RhS.SharedKernel/           # Biblioteca compartilhada
│   ├── RhS.Infrastructure/         # Persistência e integrações
│   └── Modules/                    # Módulos de negócio
│       ├── RhS.RHU.Core/          # Folha de Pagamento - Domínio
│       └── RhS.RHU.Application/   # Folha de Pagamento - Aplicação
├── tests/                          # Testes unitários e integração
└── docs/                          # Documentação técnica
```

## 🚀 Tecnologias

- **.NET 8**: Framework principal
- **ASP.NET Core MVC**: Interface web
- **Entity Framework Core**: ORM
- **MediatR**: Implementação CQRS
- **FluentValidation**: Validação de comandos
- **AutoMapper**: Mapeamento entre objetos
- **xUnit**: Framework de testes

## 📋 Pré-requisitos

- .NET 8 SDK
- SQL Server 2019+ (ou SQL Server LocalDB)
- Visual Studio 2022 ou VS Code

## ⚡ Início Rápido

1. **Clone o repositório**
   ```bash
   git clone https://github.com/seu-usuario/rhsenso.git
   cd rhsenso
   ```

2. **Configure a string de conexão**
   ```bash
   # Edite appsettings.json em RhS.Web e RhS.Api
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RhSenso;Trusted_Connection=true;MultipleActiveResultSets=true"
   }
   ```

3. **Execute as migrações**
   ```bash
   dotnet ef database update --project src/RhS.Infrastructure --startup-project src/RhS.Web
   ```

4. **Execute a aplicação**
   ```bash
   dotnet run --project src/RhS.Web
   ```

## 🧪 Executando Testes

```bash
# Testes unitários
dotnet test tests/RhS.UnitTests

# Testes de integração
dotnet test tests/RhS.IntegrationTests

# Todos os testes
dotnet test
```

## 📚 Documentação

- [Guia de Configuração](docs/SETUP.md)
- [Arquitetura Detalhada](docs/ARCHITECTURE.md)
- [Como Contribuir](docs/CONTRIBUTING.md)
- [Criação de Módulos](docs/MODULE_CREATION.md)

## 🔐 Multi-tenancy

O sistema implementa multi-tenancy através do campo `IdSaas` em todas as entidades:

```csharp
public class MinhaEntidade : BaseEntity
{
    // IdSaas é herdado de BaseEntity
    public string Nome { get; set; }
}
```

O filtro global é aplicado automaticamente em todas as consultas.

## 🛡️ Segurança

Sistema de permissões granulares baseado em claims:

```csharp
[Permission("RHU.Funcionario.Create")]
public IActionResult Create() { }
```

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

## 👥 Contribuindo

Consulte [CONTRIBUTING.md](docs/CONTRIBUTING.md) para diretrizes de contribuição.

---

**RhSenso** - Gestão de RH Inteligente e Escalável

