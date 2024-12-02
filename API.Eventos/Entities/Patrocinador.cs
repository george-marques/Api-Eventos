using System.ComponentModel.DataAnnotations;

namespace API.Eventos.Entities
{
    public class Patrocinador
    {
        public Patrocinador()
        {
            IsDeleted = false;
        }

        public int PatrocinadorId { get; set; }

        [Required(ErrorMessage = "O nome do patrocinador é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O contato do patrocinador é obrigatório.")]
        [RegularExpression(@"^\(\d{2}\)\d{5}-\d{4}$", ErrorMessage = "O Contato deve estar no formato (99)99999-9999.")]
        public string Contato { get; set; }
        public bool IsDeleted { get; set; }

        // Relacionamento 
        public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
    }
}
