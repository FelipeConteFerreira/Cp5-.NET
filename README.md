# CP5 — FitTrack API (.NET 8)

API REST para gerenciamento de planos de treino e exercícios, desenvolvida como base acadêmica para o CP5 de Advanced Business Development with .NET.



## Tecnologias
- .NET 8 / ASP.NET Core Controllers
- Entity Framework Core + SQL Server
- Clean Architecture: Domain, Application, Infrastructure e API
- Operation Result, erros customizados e paginação `PageResult`
- JWT Bearer para autenticação e autorização
- `HttpClientFactory` para integração externa
- xUnit para testes

## Estrutura
- `FitTrack.Domain`: entidades e erros de domínio.
- `FitTrack.Application`: contratos, casos de uso, DTOs, Operation Result e PageResult.
- `FitTrack.Infrastructure`: EF Core, repositórios, serviço externo e autenticação.
- `FitTrack.API`: controllers, configuração HTTP e middleware.
- `FitTrack.Tests`: testes unitários de componentes puros.

## Pré-requisitos
- .NET SDK 8
- SQL Server local, Docker ou instância compatível
- (Opcional) chave da API externa configurada por variável de ambiente

## Configuração
A API usa `ConnectionStrings__DefaultConnection` como variável de ambiente. Exemplo para PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=FitTrackDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True"
$env:Jwt__Key="troque-por-uma-chave-com-pelo-menos-32-caracteres"
dotnet run --project src/FitTrack.API
```

O `appsettings.json` contém valores locais de exemplo; nunca use segredos de exemplo em produção. Para aplicar migrations, instale `dotnet-ef` se necessário e execute:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project src/FitTrack.Infrastructure --startup-project src/FitTrack.API
dotnet ef database update --project src/FitTrack.Infrastructure --startup-project src/FitTrack.API
```

## Executar testes
```bash
dotnet restore
dotnet test
```

Os testes não usam SQLite; os testes unitários incluídos verificam os componentes sem banco. Para testes de integração com banco, configure uma instância SQL Server de teste e adicione testes que usem o mesmo provedor da aplicação.

## Endpoints principais
- `GET /api/exercises?page=1&pageSize=10` — listagem paginada, com metadados e links HATEOAS.
- `GET /api/exercises/{id}` — detalhe.
- `POST /api/exercises` — cria exercício (autenticado).
- `PUT /api/exercises/{id}` — atualiza (autenticado).
- `DELETE /api/exercises/{id}` — remove (autenticado).
- `GET /api/external/workout-tip` — consulta serviço externo de dicas de treino.
- `POST /api/auth/token` — emite token de demonstração para usuário configurado.



