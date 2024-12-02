using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Eventos.Entities
{
    public class Inscricao
    {        
        public int InscricaoId { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "A data de inscrição é obrigatória.")]
        public DateTime DataInscricao { get; set; }

        [Required(ErrorMessage = "O evento é obrigatório.")]
        public int EventoId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public Evento? Evento { get; set; }

        // Chave estrangeira e navegação para Participante
        [Required(ErrorMessage = "O participante é obrigatório.")]
        public int ParticipanteId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public Participante? Participante { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }

}
