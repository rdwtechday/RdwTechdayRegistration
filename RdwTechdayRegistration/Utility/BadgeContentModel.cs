using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RdwTechdayRegistration.Utility
{
    public enum BadgePersonType { user, organizer, speaker }

    public class BadgeContentModel
    {
        public string name { get; set; }
        public string organisation { get; set; }
        public BadgePersonType PersonType { get; set; }
    }
}
