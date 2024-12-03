# Sistema de Gerenciamento de Eventos e Inscrições (API de Eventos)

## 📋 Descrição
O e-Evento é uma aplicação voltada para o gerenciamento de eventos e inscrições, permitindo a criação, edição, listagem e exclusão de eventos e inscrições de participantes. O sistema utiliza autenticação JWT para proteger endpoints sensíveis e oferece uma documentação interativa via Swagger.

## 🚀 Tecnologias Utilizadas
- **ASP.NET Core Web API 8.0**
- **Entity Framework Core** (Code First)
- **SQL Server** (Banco de Dados)
- **JWT** (JSON Web Token) para autenticação
- **Swagger** para documentação interativa
- **C#** como linguagem de programação

## 🛠️ Funcionalidades
1. **Gerenciamento de Eventos**
2. **Gerenciamento de Inscrições**
3. **Gerenciamento de Locais**
4. **Gerenciamento de Participantes**
5. **Gerenciamento de Organizadores**
6. **Gerenciamento de Patrocinadores**
7. **Autenticação e Autorização**
   - JWT para proteger endpoints.
8. **Documentação Interativa**
   - Swagger UI para explorar os endpoints disponíveis.


## ⚙️ Pré-requisitos
Antes de executar o projeto, certifique-se de ter instalado:
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server)
- Ferramenta para gerenciar o banco de dados (SSMS)
- IDE como Visual Studio 2022 ou Visual Studio Code


## 🏗️ Configuração do Ambiente

1. **Clone o Repositório**
   
   Abra seu terminal e execute o seguinte comando:

   ```bash
   git clone <URL_DO_REPOSITORIO>
2. **Abra o Projeto**

   Abra o projeto no Visual Studio ou Visual Studio Code. Certifique-se de abrir a solução (`.sln`) para carregar todas as dependências corretamente.

3. **Restaurar Pacotes NuGet**

   No Visual Studio, clique com o botão direito do mouse no projeto na **Solution Explorer** e selecione **Restore NuGet Packages**. Isso garantirá que todas as dependências necessárias sejam baixadas e instaladas.

4. **Configurar a Conexão com o Banco de Dados**

   - Localize o arquivo de configuração `appsettings.json` e edite a string de conexão para o seu banco de dados SQL Server.

   ```json
   {
     "ConnectionStrings": {
       "ApplicationDbContext": "Server=SEU_SERVIDOR;Initial Catalog=EventosDb;Integrated Security=True;TrustServerCertificate=True"
     }
   }

5. **Criar o Banco de Dados**

   Execute as migrações para criar o banco de dados e suas tabelas. Abra o terminal da aplicação e execute:

    ```bash
     dotnet ef database update
    
6. **Executar o Projeto**

    No Visual Studio, pressione F5 ou clique em Run para iniciar o projeto. O aplicativo será aberto em seu navegador padrão.
