namespace IdentityTest.Models
{
    public class ApplicationUserSessie
    {
        public Sessie Sessie { get; set; }
        public int SessieId { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}