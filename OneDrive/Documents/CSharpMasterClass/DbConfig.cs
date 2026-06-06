using System;

public static class DbConfig
{
    private static readonly string? _envConnectionString = Environment.GetEnvironmentVariable("MYSQL_CONN");

    public static string ConnectionString { get; } =
        !string.IsNullOrWhiteSpace(_envConnectionString)
            ? _envConnectionString!
            : "Server=localhost;Database=sql_basics;User Id=root;Password=Rohit;SslMode=Disabled;AllowPublicKeyRetrieval=True;";

    public static bool IsUsingFallback => string.IsNullOrWhiteSpace(_envConnectionString);
}
