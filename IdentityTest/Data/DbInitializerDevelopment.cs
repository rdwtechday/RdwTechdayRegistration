using Bogus;
using IdentityTest.Data;
using RdwTechdayRegistration.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RdwTechdayRegistration.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Deelnemers.Any())
            {
                return;   // DB has been seeded
            }
            var maxima = new List<Maxima>();
            maxima.Add(new Maxima { MaxRDW = 200, MaxNonRDW = 50 });
            context.Maxima.AddRange(maxima);

            var ruimtes = new List<Ruimte>();
            ruimtes.Add(new Ruimte { Naam = "Groningen", Capacity = 70 });
            ruimtes.Add(new Ruimte { Naam = "Amsterdam", Capacity = 70 });
            ruimtes.Add(new Ruimte { Naam = "Rotterdam", Capacity = 70 });
            ruimtes.Add(new Ruimte { Naam = "Utrecht", Capacity = 70 });
            ruimtes.Add(new Ruimte { Naam = "Nijmegen", Capacity = 22 });
            ruimtes.Add(new Ruimte { Naam = "Leiden", Capacity = 22 });
            ruimtes.Add(new Ruimte { Naam = "Delft", Capacity = 22 });
            context.Ruimtes.AddRange(ruimtes);

            var tijdvakken = new List<Tijdvak>();
            var tv11 = new Tijdvak { Start = "10:00", Einde = "11:00", Order = 1 };
            tijdvakken.Add(tv11);
            var tv12 = new Tijdvak { Start = "11:15", Einde = "12:15", Order = 2 };
            tijdvakken.Add(tv12);
            var tv13 = new Tijdvak { Start = "13:15", Einde = "14:15", Order = 3 };
            tijdvakken.Add(tv13);
            var tv14 = new Tijdvak { Start = "14:30", Einde = "15:30", Order = 4 };
            tijdvakken.Add(tv14);
            var tv21 = new Tijdvak { Start = "10:00", Einde = "12:15", Order = 1 };
            tijdvakken.Add(tv21);
            var tv22 = new Tijdvak { Start = "13:15", Einde = "15:30", Order = 2 };
            tijdvakken.Add(tv22);
            context.Tijdvakken.AddRange(tijdvakken);

            var tracks = new List<Track>();
            tracks.Add(new Track { Naam = "Infra", Rgb = "FFC548" });
            tracks.Add(new Track { Naam = "Apps", Rgb = "B0A0C5" });
            tracks.Add(new Track { Naam = "Collaboratie", Rgb = "9CD2E0" });
            tracks.Add(new Track { Naam = "Inspiratie", Rgb = "FFAC59" });
            tracks.Add(new Track { Naam = "Workshops I", Rgb = "D99690" });
            tracks.Add(new Track { Naam = "Workshops II", Rgb = "D99690" });
            context.Tracks.AddRange(tracks);

            var faker = new Faker("nl");
            Randomizer.Seed = new Random(8675309);

            int maxDeelnemers = 50;
            int maxRegistratieVerzoeken = maxDeelnemers + 20;
            int maxDeelnemersMetSessies = maxDeelnemers - 20;

            // creeer registtatieverzoeken
            var registratieVerzoeken = new List<RegistratieVerzoek>();
            for (int i = 0; i < maxRegistratieVerzoeken; i++)
            {
                registratieVerzoeken.Add(new RegistratieVerzoek { Naam = faker.Name.FullName(), Email = faker.Internet.Email(), Organisatie = faker.Company.CompanyName(), Telefoon = faker.Phone.PhoneNumberFormat() });
            }
            context.RegistratieVerzoeken.AddRange(registratieVerzoeken);

            context.SaveChanges();

            // Creeer TRack met bijbehorende tijdvakken, workshop duren 2 uur, daarom aparte loop voor de workshops
            List<string> lecturenames = new List<string>() { "Infra", "Apps", "Inspiratie", "Collaboratie" };
            foreach (string naam in lecturenames)
            {
                context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv11.Id });
                context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv12.Id });
                context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv13.Id });
                context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv14.Id });
            }
            List<string> workshopnames = new List<string>() { "Workshops I", "Workshops II" };
            foreach (string naam in workshopnames)
            {
                context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv21.Id });
                context.TrackTijdvakken.Add(new TrackTijdvak { TrackID = tracks.Single(i => i.Naam == naam).Id, TijdvakID = tv22.Id });
            }
            context.SaveChanges();

            // creeer sessies, itereer over Tracks en de Tijdvakken in een Track heen
            var sessies = new List<Sessie>();
            foreach (string naam in lecturenames)
            {
                Track track = tracks.Single(i => i.Naam == naam);
                foreach (TrackTijdvak tracktijdvak in track.Tijdvakken)
                {
                    sessies.Add(new Sessie { Naam = faker.Lorem.Sentence(3), Ruimte = faker.PickRandom<Ruimte>(ruimtes), Track = track, Tijdvak = tracktijdvak.Tijdvak });
                }
            }

            foreach (string naam in workshopnames)
            {
                Track track = tracks.Single(i => i.Naam == naam);
                foreach (TrackTijdvak tracktijdvak in track.Tijdvakken)
                {
                    sessies.Add(new Sessie { Naam = faker.Lorem.Sentence(3), Ruimte = faker.PickRandom<Ruimte>(ruimtes), Track = track, Tijdvak = tracktijdvak.Tijdvak });
                }
            }
            context.Sessies.AddRange(sessies);

            // creeer deelnemers die al gekozen hebben voor sessies, hergebruik data uit de registratie verzoeken
            int iRegistratieVerzoek = 0;
            for (int i = 0; i < maxDeelnemersMetSessies; i++)
            {
                RegistratieVerzoek rv = registratieVerzoeken[iRegistratieVerzoek++];
                var deelnemerSessies = faker.PickRandom(sessies, 5).ToList<Sessie>();
                context.Deelnemers.Add(new Deelnemer
                {
                    RegistratieVerzoek = rv,
                    Naam = rv.Naam,
                    Email = rv.Email,
                    Organisatie = rv.Organisatie,
                    Telefoon = rv.Telefoon,
                    DeelnemerSessies = deelnemerSessies.Select(d => new RdwTechdayRegistration.Models.DeelnemerSessies
                    {
                        SessieId = d.Id
                    }).ToList()
                });
            }

            // creeer deelnemers die nog niet gekozen hebben voor sessies, hergebruik data uit de registratie verzoeken
            for (int i = 0; i < (maxDeelnemers - maxDeelnemersMetSessies); i++)
            {
                RegistratieVerzoek rv = registratieVerzoeken[iRegistratieVerzoek++];
                var deelnemerSessies = faker.PickRandom(sessies, 5).ToList<Sessie>();
                context.Deelnemers.Add(new Deelnemer { RegistratieVerzoek = rv, Naam = rv.Naam, Email = rv.Email, Organisatie = rv.Organisatie, Telefoon = rv.Telefoon });
            }
            context.SaveChanges();
        }
    }
}