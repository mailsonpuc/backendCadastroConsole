using System;
using Microsoft.Data.SqlClient;
using Dapper;
using App.Models;
using Microsoft.Extensions.Configuration;


var builder = new ConfigurationBuilder()
    .AddEnvironmentVariables();

var configuration = builder.Build();
string? connectionString = configuration["ConnectionStrings:DefaultConnection"];

// Console.WriteLine(connectionString); 



while (true)
{
    Console.WriteLine("1 - Cadastrar pessoa");
    Console.WriteLine("2 - Listar pessoas");
    Console.WriteLine("3 - Atualizar pessoa");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha: ");
    var opcao = Console.ReadLine();

    using var connection = new SqlConnection(connectionString);

    switch (opcao)
    {
        case "1":
            var pessoa = ValidarPessoa();
            CreatePessoa(connection, pessoa);
            break;

        case "2":
            ListPessoas(connection);
            break;

        case "3":
            UpdatePessoa(connection);
            break;

        case "0":
            return;

        default:
            Console.WriteLine("Opção inválida!");
            break;
    }
}

#pragma warning disable CS8602
static Pessoa ValidarPessoa()
{
    var p1 = new Pessoa();

    Console.WriteLine("Nome");
    p1.Nome = Console.ReadLine().Trim();

    Console.WriteLine("Idade");
    p1.Idade = Convert.ToInt16(Console.ReadLine());

    p1.Sexo = string.Empty;
    while (p1.Sexo != "M" && p1.Sexo != "F")
    {
        Console.WriteLine("Sexo [M/F]");
        p1.Sexo = Console.ReadLine().Trim().ToUpper();
    }

    p1.Id = Guid.NewGuid();
    return p1;
}

static void ListPessoas(SqlConnection connection)
{
    var pessoas = connection.Query<Pessoa>("SELECT [Id], [Nome], [Idade], [Sexo] FROM Pessoa");
    foreach (var item in pessoas)
    {
        Console.WriteLine($"{item.Id} - {item.Nome} ({item.Idade}) - {item.Sexo}");
    }
}

static void CreatePessoa(SqlConnection connection, Pessoa pessoa)
{
    var insertSql = @"INSERT INTO [Pessoa] (Id, Nome, Idade, Sexo)
                          VALUES (@Id, @Nome, @Idade, @Sexo)";

    var rows = connection.Execute(insertSql, pessoa);
    Console.WriteLine($"{rows} linhas inseridas");
}

#pragma warning disable CS8602
#pragma warning disable CS8604
static void UpdatePessoa(SqlConnection connection)
{
    Console.WriteLine("Informe o ID da pessoa para atualizar:");
    var id = Guid.Parse(Console.ReadLine());

    Console.WriteLine("Novo nome:");
    var novoNome = Console.ReadLine();

    var updateQuery = "UPDATE [Pessoa] SET [Nome] = @Nome WHERE [Id] = @Id";
    var rows = connection.Execute(updateQuery, new { Id = id, Nome = novoNome });

    Console.WriteLine($"{rows} registros atualizados");
}

