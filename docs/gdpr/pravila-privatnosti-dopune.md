# Pravila privatnosti — prijedlog dopuna

> **Status:** NACRT za pravnu provjeru (DPO / odvjetnik). Tekst niže dopunjuje
> postojeća pravila na `/pravila-privatnosti`. Sadržaj se uređuje u Orchard
> administraciji (nije u repozitoriju). Pravne osnove niže su **predložene na
> temelju podataka koje aplikacija stvarno prikuplja** i moraju biti potvrđene.

---

## 1. Datum ažuriranja / verzija (dodati na vrh ili dno dokumenta)

> Postojeća pravila nemaju datum. Dodati:

**Posljednje ažuriranje: 14. srpnja 2026.**

Zadržavamo pravo izmjene ovih Pravila privatnosti. O svakoj značajnoj izmjeni
obavijestit ćemo članove putem elektroničke pošte i/ili obaviješću na ovoj
stranici. Datum posljednjeg ažuriranja uvijek je naveden na vrhu dokumenta.

---

## 2. Kategorije podataka i pravna osnova obrade (zamijeniti/proširiti postojeći odjeljak)

> Postojeći tekst navodi „ime i prezime, datum rođenja, OIB i adresu
> elektroničke pošte". Aplikacija zapravo prikuplja i sljedeće (vidljivo iz
> obrazaca za članstvo, pravne osobe i donacije). Predlaže se jasan prikaz
> po kategoriji, svrsi i pravnoj osnovi:

| Kategorija podataka | Svrha | Pravna osnova (GDPR čl. 6) |
|---|---|---|
| **Podaci člana (fizička osoba):** ime, prezime, OIB, datum rođenja, spol, adresa, grad, županija, telefon, e-adresa, vještine/područja doprinosa | Vođenje registra članova i ostvarivanje članskih prava i obveza Udruge | čl. 6. st. 1. t. (b) — nužno za članski odnos; te t. (c) — zakonska obveza vođenja evidencije prema Zakonu o udrugama |
| **Podaci pravne osobe / obrta:** naziv, OIB, ovlaštena osoba i njena funkcija, djelatnost, broj zaposlenih, prihod, logotip | Evidencija članova — pravnih osoba | čl. 6. st. 1. t. (b) i (f) — legitimni interes vođenja članstva |
| **Podaci o donacijama:** naziv/ime donatora, OIB, iznos, e-adresa, napomena | Zaprimanje i knjiženje donacija te izdavanje potvrda | čl. 6. st. 1. t. (c) — zakonska obveza (računovodstveni i porezni propisi) |
| **E-adresa za newsletter** | Slanje obavijesti i newslettera | čl. 6. st. 1. t. (a) — privola |
| **Podaci pri registraciji korisničkog računa:** e-adresa (ujedno korisničko ime) | Otvaranje i vođenje korisničkog računa | čl. 6. st. 1. t. (b) |

> **Napomena za DPO:**
> - **OIB** je nacionalni identifikacijski broj — AZOP traži poseban oprez;
>   potvrditi da je obrada nužna i razmjerna za svaku navedenu svrhu.
> - **Datum rođenja i spol** trenutačno se prikupljaju (model `Member`).
>   Ako nisu nužni za članstvo, razmotriti prestanak prikupljanja
>   (načelo smanjenja količine podataka, čl. 5. st. 1. t. (c)).
> - **Donacije gostiju** (bez računa) prikupljaju OIB i e-adresu — potvrditi
>   pravnu osnovu i rok čuvanja u skladu s računovodstvenim propisima.

---

## 3. Pravo na prenosivost podataka i ostala prava (dodati u odjeljak o pravima)

> Postojeća pravila spominju pristup, ispravak i brisanje. Nedostaje
> **prenosivost (čl. 20.)**; predlaže se dopuna cijelog popisa prava:

Sukladno Općoj uredbi o zaštiti podataka (GDPR) imate sljedeća prava:

- **Pravo na pristup** (čl. 15.) — dobiti potvrdu obrađuju li se vaši podaci i
  kopiju tih podataka.
- **Pravo na ispravak** (čl. 16.) — ispraviti netočne ili nepotpune podatke.
- **Pravo na brisanje** („pravo na zaborav", čl. 17.) — zatražiti brisanje
  podataka kada više ne postoji osnova za obradu. *(Napomena: podaci vezani uz
  donacije i knjigovodstvo mogu se morati zadržati do isteka zakonskih rokova
  čuvanja.)*
- **Pravo na ograničenje obrade** (čl. 18.).
- **Pravo na prenosivost podataka** (čl. 20.) — zaprimiti podatke koje ste nam
  dostavili u strukturiranom, uobičajeno upotrebljavanom i strojno čitljivom
  obliku te ih prenijeti drugom voditelju obrade, kada je obrada utemeljena na
  privoli ili ugovoru i provodi se automatiziranim putem.
- **Pravo na prigovor** (čl. 21.) — prigovoriti obradi utemeljenoj na
  legitimnom interesu.
- **Pravo na povlačenje privole** u svakom trenutku, bez utjecaja na zakonitost
  obrade prije povlačenja (npr. odjava s newslettera).
- **Pravo na pritužbu** nadzornom tijelu — Agenciji za zaštitu osobnih podataka
  (AZOP), Selska cesta 136, 10000 Zagreb.

Za ostvarivanje bilo kojeg prava obratite se na **info@glaspoduzetnika.hr**.
Na zahtjev odgovaramo bez nepotrebnog odgađanja, a najkasnije u roku od
mjesec dana (čl. 12. st. 3.).

---

## 4. Preostale preporuke (nije tekst za objavu — zadaci)

- **Izjava o kolačićima** (`Izjava o kolačićima`) — potvrditi da opisuje samo
  nužne kolačiće + Facebook prijavu (nema analitike/marketinga, potvrđeno u kodu).
- **Registracija putem Facebooka** — privola za obradu prikuplja se web-obrascem,
  ali vanjska (Facebook) registracija zaobilazi tu potvrdu. Razmotriti dodavanje
  privole i za taj tok.
- **Evidencija privole** — trenutačno se privola pri registraciji provjerava,
  ali se ne pohranjuje s vremenskom oznakom. Za dokazivost (čl. 7. st. 1.)
  razmotriti bilježenje datuma/verzije prihvaćenih pravila uz korisnika.
