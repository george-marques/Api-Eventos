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
    public class PatrocinadoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatrocinadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Patrocinadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patrocinador>>> GetPatrocinadores()
        {
            return await _context.Patrocinadores.ToListAsync();
        }

        // GET: api/Patrocinadores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patrocinador>> GetPatrocinador(int id)
        {
            var patrocinador = await _context.Patrocinadores.FindAsync(id);

            if (patrocinador == null)
            {
                return NotFound();
            }

            return patrocinador;
        }

        // PUT: api/Patrocinadores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatrocinador(int id, Patrocinador patrocinador)
        {
            if (id != patrocinador.PatrocinadorId)
            {
                return BadRequest();
            }

            _context.Entry(patrocinador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatrocinadorExists(id))
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

        // POST: api/Patrocinadores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Patrocinador>> PostPatrocinador(Patrocinador patrocinador)
        {
            _context.Patrocinadores.Add(patrocinador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatrocinador", new { id = patrocinador.PatrocinadorId }, patrocinador);
        }

        // DELETE: api/Patrocinadores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatrocinador(int id)
        {
            var patrocinador = await _context.Patrocinadores.FindAsync(id);
            if (patrocinador == null)
            {
                return NotFound();
            }

            _context.Patrocinadores.Remove(patrocinador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatrocinadorExists(int id)
        {
            return _context.Patrocinadores.Any(e => e.PatrocinadorId == id);
        }
    }
}
