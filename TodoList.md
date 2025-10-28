✅ Implementation Plan (Markdown TODOs)
Phase 0 — Scope & Environment

- [x] Confirm scope and constraints (ASP.NET Core 8/9, EF Core Code-First, SQL Server, no auth, no JS/AJAX, no cascade delete, custom PACKAGE ticket view, Dutch UI; code/DB in English).

- [x] Install tools (Visual Studio 2022 or Rider, .NET SDK 8/9, SQL Server + SSMS v21).

- [x] Create solution structure
  - [x] Web project: FestivalConfigurator.Web (ASP.NET Core MVC)
  - [x] Class library: FestivalConfigurator.Domain (entities, enums)
  - [x] Optional library: FestivalConfigurator.Infrastructure (EF Core DbContext, configs)

- [x] Set up connection string in appsettings.json for SQL Server (local dev)

- [ ] Add Bootstrap for responsive, good-looking UI (Nice to Have)

Phase 1 — Domain Model & EF Core

- [x] Define enum ItemType with exactly 6 values: Camping, Food_and_Drinks, Parking, Merchandise, VIPAccess, Other

- [x] Create entities in Domain library (English names)
  - [x] Festival (Name, Place, Logo, Description, BasicPrice, StartDate, EndDate, ICollection<Package>)
  - [x] Package (Id, FestivalId FK, Name, ICollection<PackageItem>)
  - [x] Item (Id, Name, ItemType, Price)
  - [x] PackageItem (PackageId, ItemId, Quantity) — join entity

- [x] Create ApplicationDbContext in Infrastructure (or Web) with DbSets and relationships

- [x] Configure Fluent API: keys, relationships, decimal precision for money, and DeleteBehavior.Restrict to forbid cascade delete

- [x] Add initial migration and update database

Phase 2 — Seed Data

- [x] Implement seeding (on first DB creation)
  - [x] 1 Festival with realistic dates and BasicPrice
  - [x] 2 Packages attached to that Festival
  - [x] At least 3 Items per ItemType (total ≥ 18)
  - [ ] Optionally seed ItemType icons & Festival logo references

- [x] Re-run migrations if needed and verify seed presence

Phase 3 — Controllers & CRUD (Scaffolding where allowed)

- [x] Add navigation menu: FESTIVAL, PACKAGE, ITEM. Menu routes to their list pages

- [x] FESTIVAL: Scaffold Controller + Views (Index/List, Create, Edit, Delete, Details). Show relevant columns; enable per-row actions

- [x] ITEM: Scaffold Controller + Views with list filter by ItemType and sorting by Type/Name/Price; per-row actions

- [x] PACKAGE list per Festival: Implement page that lists packages for selected Festival, with Create/Edit/Delete and link to Ticket view (custom Ticket view must be hand-built)

Phase 4 — ViewModels & Ticket (CUSTOM, no scaffolding)

- [x] Design Ticket ViewModel (for reading & editing a Package)
  - [x] Include Festival info (all fields)
  - [x] Six panels (one per ItemType) each showing: type name + icon, selected Item or “geen geselecteerd”, unit price, quantity, computed line total, dropdown of available Items of that type, quantity input, and a “Kies” (select/deselect) button

- [x] Implement Ticket GET: fetch Package + Festival + selected items + item catalogs grouped by ItemType (LINQ)

- [x] Implement Ticket POST (server-side): handle “Kies” actions to select/deselect an Item per ItemType, update PackageItem rows, and recompute totals; re-render the Ticket (full postbacks; no JS/AJAX)

- [x] Compute totals: PackageTotal = Festival.BasicPrice + Σ(Item.Price × Quantity); recalc on each “Kies” press

- [ ] Enforce rule: max 1 Item per ItemType per Package (0 or 1). Validate server-side logic accordingly

Phase 5 — Image Handling

- [x] Choose image storage approach (file on disk in wwwroot, URL, or DB blob). Start with files/URLs; add sample icons per ItemType and a Festival logo

- [x] Render icons on the Ticket per ItemType; render Festival logo on Ticket and Festival list

Phase 6 — Deletion Behavior & Friendly Errors

- [x] Disable cascade delete (already configured) and verify FK constraints block unsafe deletes

- [x] Catch FK exceptions on Delete for FESTIVAL/PACKAGE/ITEM and show a friendly Dutch error message in the UI

Phase 7 — Language, UX, and Polish

- [x] Dutch UI text everywhere (labels, buttons like “Kies”, messages). Code + DB naming in English

- [x] Responsive layout with Bootstrap; clean list columns and Ticket layout (Nice to Have)

- [x] Display relevant columns in list views (Festival, Package, Item)

Phase 8 — LINQ Queries & Performance

- [x] Use LINQ for all data queries (group Items by ItemType, filter, sort)

- [x] Eager loading where appropriate for Ticket view (Include/ThenInclude)

Phase 9 — Should/Nice Haves

- [x] DataAnnotations (Display names in Dutch, formatting for money/dates) (Should Have)

- [x] Fluent API refinements (constraints, indexes) (Should Have)

- [ ] Optional tools: ReSharper usage, Rider (Allowed)

Phase 10 — Testing & Demo

- [ ] Manual test matrix
  - [ ] CRUD for Festival and Item
  - [ ] Create/Update Package; select/deselect items per type; set quantities; recompute totals correctly on each “Kies”
  - [ ] Verify 0/1 item per ItemType rule
  - [ ] Delete with FK references shows friendly message
  - [ ] Item list filters and sorts correctly
  - [ ] Menu navigates to all lists; Package list limited to selected Festival

- [ ] Screenshots: Ticket page, lists, filters

- [ ] Readme with run steps (migrate, seed, credentials if any) and screenshots

Must-add gaps — Hardening and Business Rules

Domain boundaries

- [ ] Decide whether Items are global or festival-specific
- [ ] If festival-specific, add linking entity `FestivalItem(FestivalId, ItemId, IsActive)` or add `FestivalId` to `Item`
- [ ] Ensure Ticket only shows items applicable to the selected festival
- [ ] Clarify and encode per-type quantity caps (e.g., VIPAccess max 1; Parking non-negative integer)

Business rules & validation (server-side + DB)

- [ ] Festival: Name required; Place required; BasicPrice >= 0; StartDate <= EndDate (use DateOnly in EF Core 8/9)
- [ ] Item: Name unique per ItemType (and per Festival if scoped); Price >= 0 with decimal(18,2)
- [ ] Package: Name unique per Festival
- [ ] PackageItem: Quantity >= 0 (or > 0 when selected); PRIMARY KEY(PackageId, ItemId)
- [ ] Enforce “max 1 item per ItemType per Package” in app logic
- [ ] Persist ItemType onto PackageItem and add unique index on (PackageId, ItemType) to enforce at DB level

Rounding, currency & culture

- [ ] Use decimal(18,2) for money; round line totals with MidpointRounding.ToEven
- [ ] Configure request localization to nl-NL via UseRequestLocalization (supported cultures nl-NL; set CurrentCulture/CurrentUICulture)
- [ ] Decide VAT strategy: prices incl./excl. btw; label appropriately or show subtotal + VAT + total

Snapshot vs live pricing

- [ ] Decide whether Item.Price changes affect existing packages
- [ ] If snapshot needed, add UnitPriceAtSelection to PackageItem and compute totals from it
- [ ] If live, document behavior to avoid demo surprises

Concurrency & request flow

- [ ] Add optimistic concurrency tokens (RowVersion byte[] on Festival/Item/Package)
- [ ] Show Dutch-friendly message on concurrency conflicts
- [ ] Use PRG (Post-Redirect-Get) on Ticket POST to avoid double submit on refresh

Security & robustness (no auth assumed)

- [ ] Add Anti-Forgery on all forms ([ValidateAntiForgeryToken] + form token)
- [ ] Prevent over-posting: use ViewModels for Create/Edit and map to entities
- [ ] File uploads: restrict content types and max size; generate random file names; store under wwwroot/uploads; never trust user-supplied paths

Deletion UX

- [ ] Pre-check FK usage; show confirmation with dependents (e.g., “Item is gebruikt in 7 pakketten”) before delete
- [ ] Keep friendly messages on blocked deletes for discoverability

Performance & paging

- [ ] Add paging for FESTIVAL/PACKAGE/ITEM lists; remember filter/sort state
- [ ] Add indexes: (Festival.Name), (Item.ItemType, Item.Name), FKs, and if snapshotting, (PackageItem.PackageId)

Testing beyond manual

- [ ] Unit tests: price math/rounding, 0/1 item per ItemType rule, deselect behavior
- [ ] Integration tests: EF mappings, restrict deletes, Ticket GET/POST happy path and invalid paths
- [ ] Seed tests: idempotent; no duplicates on rerun

DevOps & hygiene

- [ ] Turn on nullable reference types and use required members
- [ ] Add .editorconfig + analyzers; run dotnet format
- [ ] Optional: Docker Compose (SQL Server + Web) for clean setup
- [ ] README: include localization note, seed strategy, sample credentials (if any), and known limitations

Edge cases to handle in code/UI

- [ ] Show “Geen opties beschikbaar” for empty catalogs per type; disable “Kies”
- [ ] Server-side guard: no cross-festival selections for a package’s festival
- [ ] Quantity resets to 0 on deselect; reject POSTs with qty > 0 when not selected
- [ ] Enforce per-type quantity caps (VIPAccess max 1; Parking non-negative integer) with field validation
- [ ] Validate EndDate inclusive for same-day festivals; make rule explicit in UI/validation
- [ ] Cap quantity (e.g., 0–1000) to prevent overflow and silly totals
- [ ] Detect deleted Items on POST and show “Dit item bestaat niet meer; je selectie is gewist.”
- [ ] Show placeholders when logo/icon paths are null or files missing
- [ ] Avoid N+1 on Ticket: Include(p => p.PackageItems).ThenInclude(pi => pi.Item) and load catalogs grouped by type in a single query
- [ ] Validate all route ids; return 404 with Dutch message on tampering
- [ ] Accessibility: alt text on logos/icons; button labels not just icons; proper heading structure
- [ ] Pagination + sorting stability: stable sort keys; sanitize sort params to known values

Small but valuable polish

- [ ] Use DateOnly for Festival dates (EF Core 8/9)
- [ ] Global DeleteBehavior: set ClientNoAction/Restrict on relationships in OnModelCreating
- [ ] DataAnnotations with Dutch Display(Name="…"), Range, Required, and custom validators for per-type rules
- [ ] Show computed totals with a sticky summary on the Ticket (repeat totals server-side)
- [ ] Add a not-found / problem page in Dutch with guidance