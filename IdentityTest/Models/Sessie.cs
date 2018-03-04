using RdwTechdayRegistration.Models;
using System.Collections.Generic;

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
    }
}