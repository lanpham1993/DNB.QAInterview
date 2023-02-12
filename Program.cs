using DNB.QAInterview.Entities;
using DNB.QAInterview.Models;
using DNB.QAInterview.PersonService;
using Microsoft.AspNetCore.Http.HttpResults;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPersonRepository, DummyPersonRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/persons", async (IPersonRepository repository, int? startIndex, int? pageSize, CancellationToken cancellationToken) =>
{
    var persons = await repository.GetPeople(startIndex, pageSize, cancellationToken);
    var personModels = persons.Select(p => new PersonModel
    {
        Id = p.Id,
        Name = p.Name,
        Address = p.Address,
        PhoneNumber = p.PhoneNumber,
        Email = p.Email
    });
    return Results.Ok(personModels);
})
.WithName("GetPeople")
.AllowAnonymous()
.WithOpenApi();

app.MapGet("/persons/{personId:int}", async (IPersonRepository repository, int personId, CancellationToken cancellationToken) =>
{
    var person = await repository.GetPerson(personId, cancellationToken);

    if (person is null)
        return Results.NotFound();
    else
        return Results.Ok(new PersonModel
        {
            Id = person.Id,
            Address = person.Address,
            Email = person.Email,
            Name = person.Name,
            PhoneNumber = person.PhoneNumber
        });
})
.WithName("GetPerson")
.AllowAnonymous()
.WithOpenApi();

app.MapPost("/persons", async (IPersonRepository respository, PersonModel newPerson, CancellationToken cancellationToken) =>
{
    var person = new Person
    {
        PhoneNumber = newPerson.PhoneNumber,
        Email = newPerson.Email,
        Address = newPerson.Address,
        Id = newPerson.Id,
        Name = newPerson.Name
    };

    await respository.AddPerson(person, cancellationToken);

    return Results.CreatedAtRoute("GetPerson", new { personId = person.Id });
})
.WithName("CreatePerson")
.AllowAnonymous()
.WithOpenApi();

app.MapPut("/persons/{personId:int}", async (IPersonRepository respository, int personId, PersonModel updatedPerson, CancellationToken cancellationToken) =>
{
    if (personId != updatedPerson.Id)
        return Results.BadRequest(new { message = "Invalid person id" });
    
    var person = new Person
    {
        PhoneNumber = updatedPerson.PhoneNumber,
        Email = updatedPerson.Email,
        Address = updatedPerson.Address,
        Id = updatedPerson.Id,
        Name = updatedPerson.Name
    };
    
    await respository.UpdatePerson(person, cancellationToken);

    return Results.NoContent();
})
.WithName("UpdatePerson")
.AllowAnonymous()
.WithOpenApi();


app.MapDelete("/persons/{personId:int}", async (IPersonRepository respository, int personId, CancellationToken cancellationToken) =>
{ 
    await respository.DeletePerson(personId, cancellationToken);

    return Results.NoContent();
})
.WithName("DeletePerson")
.AllowAnonymous()
.WithOpenApi();

app.Run();


