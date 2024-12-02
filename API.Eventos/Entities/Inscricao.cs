﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Eventos.Entities
{
    public class Inscricao
    {
        public Inscricao()
        {
            IsDeleted = false;
        }
        
        public int InscricaoId { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "A data de inscrição é obrigatória.")]
        public DateTime DataInscricao { get; set; }

        [Required(ErrorMessage = "O evento é obrigatório.")]
        public int EventoId { get; set; }

        [NotMapped]
        public Evento Evento { get; set; }

        // Chave estrangeira e navegação para Participante
        [Required(ErrorMessage = "O participante é obrigatório.")]
        public int ParticipanteId { get; set; }

        [NotMapped]
        public Participante Participante { get; set; }
        public bool IsDeleted { get; set; }
    }

}