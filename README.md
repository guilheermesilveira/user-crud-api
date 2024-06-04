# User CRUD API ✅

## 🎯 Objetivo
Esse projeto é uma API Rest que permite o gerenciamento de usuários por um administrador, incluindo operações CRUD.

## 🏛️ Arquitetura
O projeto está dividido nas seguintes camadas: API, Application, Domain, Infra.Data.

## 💻 Tecnologias e dependências utilizadas
- C# e .NET 6
- Entity Framework Core
- MySQL
- AutoMapper
- FluentValidation
- ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher

## ▶️ Como rodar o projeto
1. Clone este repositório.
2. Abra o projeto em sua IDE favorita.
3. Navegue até o arquivo [appsettings.Example.json](src/UserCrud.API/appsettings.Example.json).
4. Configure a conexão com o banco de dados MySQL na seção ``ConnectionStrings``.
5. Insira todas as informações solicitadas na seção ``Jwt``.
6. Você usará as informações de nome de usuário e senha preenchidas na seção ``Jwt`` para realizar a autenticação na API.
7. Após terminar a configuração do ``appsettings.Example.json``, lembre-se de modificar a extensão "Example" para o nome do ambiente desejado (por exemplo, appsettings.Development.json).
8. Faça o restore dos pacotes NuGet. Use o comando: ``dotnet restore``.
9. Utilize um sistema gerenciador de banco de dados como o MySQL Workbench.
10. Certifique-se de que o Entity Framework Core Tools está instalado. Caso não esteja, instale com o comando: ``dotnet tool install --global dotnet-ef``.
11. Aplique as migrações do Entity Framework Core para atualizar o banco de dados. Utilize o comando: ``dotnet ef database update``.
12. Abra o terminal e navegue até a pasta UserCrud.API.
13. Execute o comando ``dotnet run`` para iniciar a aplicação.
14. Acesse a API documentada pelo Swagger.
