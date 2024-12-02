using System.ComponentModel.DataAnnotations;

namespace API.Eventos.Entities
{
    public class Local
    {
        public Local()
        {
            IsDeleted = false;
        }

        public int LocalId { get; set; }

        [Required(ErrorMessage = "O nome do local é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres.")]
        public string Endereco { get; set; }

        [Range(1, 10000, ErrorMessage = "A capacidade máxima deve ser entre 1 e 10.000 pessoas.")]
        public int CapacidadeMaxima { get; set; }
        public bool IsDeleted { get; set; }

    }

}
