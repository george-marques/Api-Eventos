﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Eventos.Entities
{
    public class Evento
    {
        public int EventoId { get; set; } // O EF infere como chave primária pelo nome padrão.

        [Required(ErrorMessage = "O nome do evento é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do evento deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição do evento é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "A data do evento é obrigatória.")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O local do evento é obrigatório.")]
        public int LocalId { get; set; }

        // Propriedades de navegação
        [NotMapped]
        [JsonIgnore]
        public virtual Local? Local { get; set; } // Navegação para o Local do evento

        [Range(1, 10000, ErrorMessage = "A capacidade deve ser entre 1 e 10.000 pessoas.")]
        [Required(ErrorMessage = "A capacidade é obrigatória.")]
        public int Capacidade { get; set; }

        [Required(ErrorMessage = "O organizador do evento é obrigatório.")]
        public int OrganizadorId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual Organizador? Organizador { get; set; } // Navegação para o Organizador do evento

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        // Propriedades de navegação para relacionamentos
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
        [NotMapped]
        public ICollection<Patrocinador> Patrocinadores { get; set; } = new List<Patrocinador>();
    }

}
