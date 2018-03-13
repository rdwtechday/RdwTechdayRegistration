/*  Copyright (C) 2018, RDW 
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Affero General Public License for more details.
 *
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/.  
 *  
 */

using Microsoft.EntityFrameworkCore;
using RdwTechdayRegistration.Data;
using RdwTechdayRegistration.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            ForceChangeCount = 0;
        }

        public int Id { get; set; }
        public string Naam { get; set; }
        public int? RuimteId { get; set; }
        public Ruimte Ruimte { get; set; }
        public int? TrackId { get; set; }
        public Track Track { get; set; }

        // this field is used to create a dummy change so this record is forcibly saved
        // to trigger a concurrency check in register session
        public int ForceChangeCount { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
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