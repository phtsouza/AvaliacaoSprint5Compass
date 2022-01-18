![DotNet](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

# Projeto Sprint 5 - Compass

## Objetivos

O projeto tem como objetivo o controle de dois endpoints, as Cidades e os Clientes. No entanto foi desenvolvido um CRUD, para os dois endpoints além de uma integração
com a API ViaCep para o registro dos Clientes.

## Como executar

Criar banco de dados com o nome `ClienteCidadeDb`

Configurar conexão com o banco de dados no arquivo `Startup.cs`

```
  services.AddDbContext<ClienteCidadeDbContext>(Op => Op.UseSqlServer("Data Source=YOUR_LOCALHOST\SQLEXPRESS;
                                                Initial Catalog=ClienteCidadeDb;
                                                Integrated Security=True;
                                                Connect Timeout=5;
                                                Encrypt=False;
                                                TrustServerCertificate=False;
                                                ApplicationIntent=ReadWrite;
                                                MultiSubnetFailover=False"
                                                ));
```

## Funcionalidades Endpoint Cidades
- ✅Post 
- ✅Get 
- ✅Get por Id 
- ✅Delete
- ✅Put 

## Funcionalidades Endpoint Clientes
- ✅Post 
- ✅Get 
- ✅Get por Id 
- ✅Delete
- ✅Put 

## Desafio Extra
Como desafio extra proposto na sprint, foi adicionado o Fluent Validation para realizar as verificações dos dados enviados para as requisições Post e Put.

