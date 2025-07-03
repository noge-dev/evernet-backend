using Projects;
using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5432)
    .WithDataVolume(isReadOnly: false);
var postgresdb = postgres.AddDatabase("postgresdb");

var webapi = builder.AddProject<Evernet_WebApi>("webapi")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.AddScalarApiReference(options => { options.WithTheme(ScalarTheme.Purple); })
    .WithApiReference(webapi);

builder.AddMailPit("mailpit", 8025, 1025);

builder.Build().Run();