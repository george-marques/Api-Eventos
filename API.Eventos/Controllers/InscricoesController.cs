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
    public class InscricoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InscricoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Inscricoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inscricao>>> GetInscricoes()
        {
            return await _context.Inscricoes
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        // GET: api/Inscricoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inscricao>> GetInscricao(int id)
        {
            var inscricao = await _context.Inscricoes
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.InscricaoId == id);

            if (inscricao == null)
            {
                return NotFound();
            }

            return inscricao;
        }

        // PUT: api/Inscricoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscricao(int id, Inscricao inscricao)
        {
            if (id != inscricao.InscricaoId)
            {
                return BadRequest();
            }

            _context.Entry(inscricao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscricaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Inscricoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Inscricao>> PostInscricao(Inscricao inscricao)
        {
            _context.Inscricoes.Add(inscricao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInscricao", new { id = inscricao.InscricaoId }, inscricao);
        }

        // DELETE: api/Inscricoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInscricao(int id)
        {
            var inscricao = await _context.Inscricoes
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.InscricaoId == id);
            if (inscricao == null)
            {
                return NotFound();
            }

            // Atualiza o valor de IsDeleted para true
            inscricao.IsDeleted = true;
            _context.Entry(inscricao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscricaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool InscricaoExists(int id)
        {
            return _context.Inscricoes.Any(e => e.InscricaoId == id);
        }
    }
}
