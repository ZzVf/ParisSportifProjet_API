using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProjectFootAPI.Model;

namespace ProjectFootAPI.Data;

public static class DbInitializer
{
    // Options JSON communes (insensibilité à la casse + tolérance espaces/retours)
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public static void Initialize(ProjectFootContext context, ILogger logger)
    {
        // Applique les migrations (équivalent à 'dotnet ef database update')
        context.Database.Migrate();

        // Dossier des fichiers JSON
        var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
        if (!Directory.Exists(dataDir))
        {
            logger.LogError($"Le dossier Data est introuvable : {dataDir}");
            return;
        }

        // IMPORTANT : ordre de seed (du parent vers l’enfant)
        // 1) Ligues
        SeedEntity<Ligue>(context, logger, dataDir, "ligues.json", mustBeEmptyBeforeSeeding: true);

        // 2) Clubs (dépend de LigueId)
        SeedEntity<Club>(context, logger, dataDir, "clubs.json", mustBeEmptyBeforeSeeding: true);

        // 3) Clients (indépendant, mais utilisé par Bets)
        SeedEntity<Client>(context, logger, dataDir, "clients.json", mustBeEmptyBeforeSeeding: true);

        // 4) Matches (dépend de ClubId1/ClubId2)
        SeedEntity<Match>(context, logger, dataDir, "matches.json", mustBeEmptyBeforeSeeding: true);

        // 5) Bets (dépend de ClientId/MatchId — adapte les noms si différents)
        SeedEntity<Bet>(context, logger, dataDir, "bets.json", mustBeEmptyBeforeSeeding: true);

        logger.LogInformation("Initialisation terminée.");
    }

    /// <summary>
    /// Charge un fichier JSON en liste d'entités et insère si la table est vide.
    /// </summary>
    private static void SeedEntity<T>(
        ProjectFootContext context,
        ILogger logger,
        string dataDir,
        string fileName,
        bool mustBeEmptyBeforeSeeding = true) where T : class
    {
        var set = context.Set<T>();

        // Si on veut éviter les doublons, on vérifie que la table est vide
        if (mustBeEmptyBeforeSeeding && set.Any())
        {
            logger.LogInformation($"La table {typeof(T).Name}s contient déjà des données, seed ignoré.");
            return;
        }

        var path = Path.Combine(dataDir, fileName);
        if (!File.Exists(path))
        {
            logger.LogWarning($"Fichier {fileName} introuvable. Seed de {typeof(T).Name} ignoré.");
            return;
        }

        try
        {
            var json = File.ReadAllText(path);
            var items = JsonSerializer.Deserialize<List<T>>(json, JsonOpts);

            if (items is { Count: > 0 })
            {
                // Astuce : si tes JSON contiennent des objets imbriqués (navigations),
                // préfère ne mettre QUE les clés étrangères (ex: LigueId, ClubId1, ClubId2, ClientId, MatchId)
                // et laisser les navigations null/vides, pour éviter des insertions en cascade involontaires.
                set.AddRange(items);
                context.SaveChanges();
                logger.LogInformation($"{items.Count} élément(s) inséré(s) dans {typeof(T).Name}s depuis {fileName}.");
            }
            else
            {
                logger.LogWarning($"Aucun élément détecté dans {fileName} pour {typeof(T).Name}.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Erreur lors du seed de {typeof(T).Name} via {fileName} : {ex.Message}");
        }
    }
}
