# UGP Web 

UGP Web projekt je open source implementacija web sustava za vođenje članova i web stranice Udruge Glas poduzetnika.

## Instalacija

Kako bi mogli pokrenuti projekt morate imati instaliran .NET 5. Poželjno je imati Visual Studio 2019 ili Visual Studio Code.
Klonirajte kod lokalno, otvorite u konzoli projekt UgpWeb, i upišite dotnet run. To je to!

Za login je default sifra: admin/Admin123!
 
Projekt je napravljen na platformi Orchard Core.

## Landing stranice (autosync recepti)

Recepti u `UgpTheme/Recipes/` čije ime završava na `.autosync.recipe.json` (npr. `ugp-gala.autosync.recipe.json`) primjenjuju se automatski pri pokretanju tenanta, kad im se sadržaj promijeni (SHA-256 datoteke se uspoređuje sa zadnje primijenjenim). Ručno pokretanje u adminu (Configuration → Recipes) i dalje radi.

- Smiju sadržavati samo `content` korake. Tip sadržaja `LandingPage` i njegovi Templates su u `UgpTheme/Migrations/landing-page.recipe.json` i primjenjuju se data migracijom; za promjenu tipa ili predložaka dodajte novi `UpdateFromN` u `UgpTheme/Migrations.cs`.
- Stranice iz autosync recepata uređuju se samo kroz git. Ako je stranica u međuvremenu mijenjana u adminu, recept se preskače i u logu se ispisuje upozorenje. Za prepisivanje admin izmjena postavite `OrchardCore:UgpTheme:AutosyncRecipes:OverwriteAdminEdits` na `true`; `...:Enabled` = `false` isključuje autosync.
- Za novu stranicu dodajte `nova-stranica.autosync.recipe.json` s fiksnim `ContentItemId`.
