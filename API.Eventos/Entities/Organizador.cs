using System.ComponentModel.DataAnnotations;

namespace API.Eventos.Entities
{
    public class Organizador
    {
        public Organizador()
        {
            IsDeleted = false;
        }
       
        public int OrganizadorId { get; set; }

        [Required(ErrorMessage = "O nome do organizador é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O contato é obrigatório.")]
        [RegularExpression(@"^\(\d{2}\)\d{5}-\d{4}$", ErrorMessage = "O Contato deve estar no formato (99)99999-9999.")]
        public string Contato { get; set; }
        public bool IsDeleted { get; set; }

        // Relacionamento com eventos (1:N)
        public ICollection<Evento> Eventos { get; set; } = new List<Evento>();
    }

}
