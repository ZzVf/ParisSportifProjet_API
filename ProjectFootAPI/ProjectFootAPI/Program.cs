using Microsoft.EntityFrameworkCore;
using ProjectFootAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProjectFootContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=ProjectFootAPI;Username=postgres;Password=1234"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// =====================
// INITIALISATION DB
// =====================

// On crée un "scope" de services.
// Cela permet de résoudre les dépendances de type "scoped" (comme le DbContext) 
// en dehors d'une requête HTTP, ici au démarrage de l'application.
using (var scope = app.Services.CreateScope())
{
    // On récupère le conteneur de services du scope.
    var services = scope.ServiceProvider;

    try
    {
        // On demande une instance de notre DbContext (PokISPOBowlContext),
        // qui servira à appliquer les migrations et insérer les données.
        var context = services.GetRequiredService<ProjectFootContext>();

        // On récupère la fabrique de loggers.
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();

        // On crée un logger spécifique pour la catégorie "DbInitializer".
        // Cela nous permettra de suivre les messages générés pendant l'initialisation.
        var logger = loggerFactory.CreateLogger("DbInitializer");

        // On lance l'initialisation de la base de données :
        // - Application des migrations
        // - Vérification si la table Clients est vide
        // - Lecture du fichier clients.json et insertion des données si nécessaire
        DbInitializer.Initialize(context, logger);
    }
    catch (Exception ex)
    {
        // Si une erreur se produit (connexion DB, JSON introuvable, etc.),
        // on récupère un logger pour la catégorie "Program" afin de tracer l'erreur.
        var logger = services.GetRequiredService<ILogger<Program>>();

        // On enregistre l'erreur avec un message explicite et la stacktrace.
        logger.LogError(ex, "Erreur lors de l'initialisation de la base de données.");
    }
}
// =====================

app.Run();
