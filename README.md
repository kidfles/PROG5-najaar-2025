Festival Configurator — ASP.NET Core MVC (EF Core)

Snelle start

1) Vereisten
- .NET SDK 9.0+
- Docker Desktop (voor SQL Server container)

2) Start SQL Server in Docker (eerste keer)
```
docker pull mcr.microsoft.com/mssql/server:2022-latest
docker run -d --name festival-sql \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=Pass@word1!" \
  -p 1433:1433 \
  -v mssql-data:/var/opt/mssql \
  mcr.microsoft.com/mssql/server:2022-latest
```

3) Configureer connection string
De app gebruikt `appsettings.Development.json`:
```
"ConnectionStrings": {
  "Default": "Server=localhost,1433;Database=FestivalConfigurator;User Id=sa;Password=Pass@word1!;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

4) Database setup (migrations + seed)
Run eenmaal vanuit repo root (als DB nog niet bestaat):
```
dotnet dotnet-ef database update \
  -p .\FestivalConfigurator.Infrastructure \
  -s .\FestivalConfigurator.Web
```
De DB wordt aangemaakt met:
- 1 Festival (Lowlands)
- 2 Pakketten (Weekend Basic/Plus)
- ≥18 Items (3 per ItemType)

5) Start de app
```
dotnet run --project .\FestivalConfigurator.Web
```
Open de geprinte URL (bijv. http://localhost:5162).

Project structuur

- FestivalConfigurator.Domain — Entiteiten en enums (Engels), DataAnnotations (Nederlandse weergave/formaat)
- FestivalConfigurator.Infrastructure — EF Core `ApplicationDbContext`, mappings, indexes, seed
- FestivalConfigurator.Web — MVC app (Controllers, Views, Models/ViewModels, static assets)

Geïmplementeerde functies

- CRUD voor Festival en Item
- Pakketlijst per Festival + link naar custom Ticket pagina
- Custom Ticket (geen JS/AJAX): selecteer/deselecteer één Item per ItemType, stel hoeveelheden in, herbereken totalen
- Totalen: `Festival.BasicPrice + Σ(Item.Price × Quantity)`
- Delete gedrag: restrictief (geen cascade); vriendelijke FK foutmeldingen
- Nederlandse UI labels en berichten; Bootstrap layout
- Iconen per ItemType en festival logo weergave
- Indexes: `Festivals(Name)`, `Items(ItemType, Name)`

Handmatige test checklist

- Festival/Item CRUD werkt
- Pakketten per festival: aanmaken/bewerken/verwijderen; Ticket link zichtbaar
- Ticket: kiezen/deselecteren over alle 6 types; hoeveelheden gerespecteerd; totalen updaten na postback
- Handhaaf 0/1 per ItemType in Ticket (nieuwe selectie vervangt vorige)
- Item lijst filter/sorteer op Type/Naam/Prijs werkt
- Verwijderen van gerefereerde entiteiten toont vriendelijke Nederlandse fout
- Navigatiemenu leidt naar alle pagina's

Opmerkingen

- Lokalisatie: basis Nederlandse labels via DataAnnotations; formattering voor valuta/datum
- Prijzen: live prijzen van `Item.Price` (geen snapshot)
- Connection string is voor lokale Docker SQL; pas aan voor andere omgevingen