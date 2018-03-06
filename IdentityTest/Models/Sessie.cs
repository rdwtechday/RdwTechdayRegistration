using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Data;
using RdwTechdayRegistration.Models;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Models
{
    public class Sessie
    {
        public Sessie ()
        {
            ApplicationUserTijdvakken = new List<ApplicationUserTijdvak>();
            SessieTijdvakken = new List<SessieTijdvak>();
        }

        public int Id { get; set; }
        public string Naam { get; set; }
        public int? RuimteId { get; set; }
        public Ruimte Ruimte { get; set; }
        public int? TrackId { get; set; }
        public Track Track { get; set; }
        public ICollection<ApplicationUserTijdvak> ApplicationUserTijdvakken { get; set; }
        public ICollection<SessieTijdvak> SessieTijdvakken { get; set; }

        public string TimeRange()
        {
            int minorder = int.MaxValue;
            int maxorder = int.MinValue;
            string start = "";
            string einde = "";
            foreach (SessieTijdvak stv in SessieTijdvakken)
            {
                if (stv.Tijdvak.Order < minorder)
                {
                    minorder = stv.Tijdvak.Order;
                    start = stv.Tijdvak.Start;
                }
                if (stv.Tijdvak.Order > maxorder)
                {
                    maxorder = stv.Tijdvak.Order;
                    einde= stv.Tijdvak.Einde;
                }

            }
            return start + " - " + einde;
        }

        public static async Task<Dictionary<int,int>> GetUserCountsAsync(ApplicationDbContext context)
        {

            var counts = new Dictionary<int,int>();
            var conn = context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = "SELECT SessieId, COUNT(DISTINCT ApplicationUserId)  from dbo.ApplicationUserTijdvakken WHERE SessieId IS NOT NULL GROUP BY SessieId";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            counts.Add(reader.GetInt32(0), reader.GetInt32(1));
                        }
                    }
                    reader.Dispose();

                }
            }
            finally
            {
                conn.Close();
            }
            return counts;
        }

        public async Task<int> GetUserCountAsync(ApplicationDbContext context)
        {
            int count = 0;
            var conn = context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    string query = $"SELECT SessieId, COUNT(DISTINCT ApplicationUserId)  from dbo.ApplicationUserTijdvakken WHERE SessieId = {Id.ToString()} GROUP BY SessieId";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        count = reader.GetInt32(1);
                    }
                    reader.Dispose();

                }
            }
            finally
            {
                conn.Close();
            }
            return count;
        }
    }
}