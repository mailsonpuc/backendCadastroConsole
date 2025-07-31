## Definir a variável de ambiente no Arch Linux
No terminal do Arch Linux, exporte a variável assim:

```bash
export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=SuaDatabase;User ID=sa;Password=SuaSenha;Trusted_Connection=False;TrustServerCertificate=True"
```

Isso só dura enquanto o terminal está aberto. Para ser permanente, adicione no seu `~/.bashrc` ou `~/.zshrc`:

## Ler a variável no código 
```c#
var builder = new ConfigurationBuilder()
    .AddEnvironmentVariables();

var configuration = builder.Build();
string connectionString = configuration["ConnectionStrings:DefaultConnection"];

Console.WriteLine(connectionString); // teste
```

## Rodar
`dotnet run`
