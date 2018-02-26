using RdwTechdayRegistration.Models;

namespace RdwTechdayRegistration.Models
{
    public class DeelnemerSessies
    {
        public Sessie Sessie { get; set; }
        public int SessieId { get; set; }
        public int DeelnemerId { get; set; }
        public Deelnemer Deelnemer { get; set; }
    }
}