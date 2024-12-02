using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Eventos.Entities;
using API.Eventos.Persistence;

namespace API.Eventos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            return await _context.Eventos
                .Where(e => !e.IsDeleted) 
                .Include(e => e.Patrocinadores) 
                .ToListAsync();
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _context.Eventos
                .Where(e => !e.IsDeleted)
                .Include(e => e.Patrocinadores)
                .FirstOrDefaultAsync(e => e.EventoId == id);

            if (evento == null)
            {
                return NotFound();
            }

            return evento;
        }


        // PUT: api/Eventos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.EventoId)
            {
                return BadRequest("O ID do evento na URL não corresponde ao ID no corpo da requisição.");
            }

            // Verificar se o evento existe
            var eventoExistente = await _context.Eventos
                .Include(e => e.Patrocinadores)
                .FirstOrDefaultAsync(e => e.EventoId == id);

            if (eventoExistente == null)
            {
                return NotFound("O evento especificado não foi encontrado.");
            }

            // Atualizar propriedades principais do evento
            eventoExistente.Nome = evento.Nome;
            eventoExistente.Descricao = evento.Descricao;
            eventoExistente.Data = evento.Data;
            eventoExistente.LocalId = evento.LocalId;
            eventoExistente.Capacidade = evento.Capacidade;
            eventoExistente.OrganizadorId = evento.OrganizadorId;

            // Atualizar patrocinadores
            if (evento.Patrocinadores != null && evento.Patrocinadores.Any())
            {
                // Limpar patrocinadores antigos
                eventoExistente.Patrocinadores.Clear();

                foreach (var patrocinador in evento.Patrocinadores)
                {
                    if (patrocinador.PatrocinadorId == 0)
                    {
                        // Adicionar patrocinadores novos ao contexto
                        _context.Patrocinadores.Add(patrocinador);
                    }
                    else
                    {
                        // Anexar patrocinadores existentes ao evento
                        var patrocinadorExistente = await _context.Patrocinadores.FindAsync(patrocinador.PatrocinadorId);
                        if (patrocinadorExistente != null)
                        {
                            eventoExistente.Patrocinadores.Add(patrocinadorExistente);
                        }
                    }
                }
            }

            try
            {
                // Salvar alterações no banco
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
                {
                    return NotFound("Conflito de atualização: o evento foi excluído.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // POST: api/Eventos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {
            if (evento.Patrocinadores != null && evento.Patrocinadores.Any())
            {
                foreach (var patrocinador in evento.Patrocinadores)
                {
                    if (patrocinador.PatrocinadorId == 0)
                    {
                        _context.Patrocinadores.Add(patrocinador);
                    }
                }
            }

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvento", new { id = evento.EventoId }, evento);
        }



        // DELETE: api/Eventos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            evento.IsDeleted = false;
            _context.Eventos.Update(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.EventoId == id);
        }
    }
}
