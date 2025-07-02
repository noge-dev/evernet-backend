using Projects;

var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5432)
    .WithDataVolume(isReadOnly: false);
var postgresdb = postgres.AddDatabase("postgresdb");

builder.AddProject<Evernet_WebApi>("webapi")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.Build().Run();