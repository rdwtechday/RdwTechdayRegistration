using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Models.UserViewModels
{
    public class EditSessionSelection
    {
        public List<Sessie> Sessies { get; set; }
        public int? TijdvakId { get; set; }
        public int SelectedSessieId { get; set; }
        public int CurrentSessionId { get; set; }
    }
}
