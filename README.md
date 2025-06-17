---

````markdown
# 🛠️ Projeto: RefitGit - Integração com GitHub API usando Refit, Polly, Cache e Header Propagation

Este projeto demonstra como consumir a API pública do GitHub utilizando:

✅ **Refit** (HTTP Client type-safe)  
✅ **Polly** (Resiliência: Retry + Circuit Breaker)  
✅ **MemoryCache** (Cache em memória)  
✅ **HeaderPropagation** (Propagação de headers como Correlation ID)  
✅ **OpenAPI (Swagger)** com agrupamento por versão  
✅ **Minimal API com ASP.NET Core**

---

## 📚 Tecnologias e Pacotes Usados

- **ASP.NET Core Minimal API**
- **Refit**
- **Polly**
- **Swashbuckle (OpenAPI / Swagger)**
- **MemoryCache**
- **Microsoft.AspNetCore.HeaderPropagation**

---

## ✅ Funcionalidades implementadas

| Rota                                   | Descrição                                  |
|----------------------------------------|-------------------------------------------|
| `GET /v1/users/{username}`             | Retorna os dados do usuário GitHub        |
| `GET /v1/users/{username}/repos`       | Lista os repositórios públicos do usuário |
| `GET /v1/repos/{owner}/{repo}`         | Detalhes de um repositório específico     |
| `GET /v1/users/{username}/followers`   | Lista de seguidores do usuário            |
| `GET /v1/users/{username}/following`   | Lista de usuários que o usuário está seguindo |

---

## ✅ Principais Recursos Técnicos

- ✅ Cache em memória por 5 minutos por endpoint
- ✅ Retry com Exponential Backoff (3 tentativas)
- ✅ Circuit Breaker (após 2 falhas consecutivas)
- ✅ Logging de requisições HTTP com DelegatingHandler
- ✅ Propagação de Correlation ID via HeaderPropagation
- ✅ Documentação via Swagger UI, com agrupamento por versão (`v1`)

---

## ✅ Pré-requisitos

- .NET 8 SDK
- Token GitHub (Opcional, mas recomendado para evitar throttling)

---

## ✅ Configuração

1. **Clonar o projeto:**

```bash
git clone https://github.com/seu-repo/refitgit.git
````

2. **Configurar o Token GitHub (Opcional):**

No `appsettings.json` ou como variável de ambiente:

```json
{
  "GitHub": {
    "Token": "seu-token-aqui"
  }
}
```

---

## ✅ Executando o projeto

```bash
dotnet run
```

---

## ✅ Acessando o Swagger

Acesse via navegador:

```
https://localhost:{porta}/swagger
```

Você verá a interface Swagger com a documentação da API agrupada por versão (v1).

---

## ✅ Exemplo de chamada via curl

```bash
curl https://localhost:{porta}/v1/users/octocat
```

---

## ✅ Principais extensões configuradas

* `AddRefitClient`
* `AddHeaderPropagation`
* `AddMemoryCache`
* `AddPolicyHandler` (Polly Retry + CircuitBreaker)
* `Swagger/OpenAPI` com agrupamento por versão

---

## ✅ Observações Importantes

* As respostas da API GitHub são cacheadas por 5 minutos para reduzir chamadas externas.
* O header `X-Correlation-ID` é propagado entre requisições (útil em cenários distribuídos).
* A configuração de Retry e Circuit Breaker aumenta a resiliência da aplicação.

---

## ✅ Melhorias futuras sugeridas

* Implementar paginação para os endpoints que retornam listas.
* Adicionar autenticação OAuth se desejar usar APIs privadas do GitHub.
* Persistir cache em um mecanismo distribuído como Redis.
* Criar testes automatizados (Unit e Integration Tests).

---

## ✅ Licença

Este projeto é open source. Sinta-se à vontade para usar, estudar e adaptar.

---
