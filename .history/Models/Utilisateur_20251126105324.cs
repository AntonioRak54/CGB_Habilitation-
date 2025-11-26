public class Utilisateur
{
    public Guid UserId { get; set; }

    // enlever "required"
    public string Login { get; set; }
    public string Email { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Telephone { get; set; }

    public DateTime? DateCreation { get; set; }
    public bool EstActif { get; set; } = true;

    public ICollection<UtilisateurRole> UtilisateurRoles { get; set; } = new();
    public ICollection<Habilitation> Habilitations { get; set; } = new();
}
