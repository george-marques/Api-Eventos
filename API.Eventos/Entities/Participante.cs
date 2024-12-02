﻿using System.ComponentModel.DataAnnotations;

namespace API.Eventos.Entities
{
    public class Participante
    {
        public Participante()
        {
            IsDeleted = false;
        }

        public int ParticipanteId { get; set; }

        [Required(ErrorMessage = "O nome do participante é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato 999.999.999-99.")]
        public string CPF { get; set; }
        public bool IsDeleted { get; set; }

        // Relacionamento com inscrições (1:N)
        public ICollection<Inscricao> Inscricoes { get; set; } = new List<Inscricao>();
    }

}