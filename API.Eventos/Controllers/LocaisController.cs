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
    public class LocaisController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Locais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Local>>> GetLocais()
        {
            return await _context.Locais
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        // GET: api/Locais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Local>> GetLocal(int id)
        {
            var local = await _context.Locais
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.LocalId == id);

            if (local == null)
            {
                return NotFound();
            }

            return local;
        }

        // PUT: api/Locais/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocal(int id, Local local)
        {
            if (id != local.LocalId)
            {
                return BadRequest();
            }

            _context.Entry(local).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalExists(id))
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

        // POST: api/Locais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Local>> PostLocal(Local local)
        {
            _context.Locais.Add(local);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocal", new { id = local.LocalId }, local);
        }

        // DELETE: api/Locais/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocal(int id)
        {
            var local = await _context.Locais
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.LocalId == id);
            if (local == null)
            {
                return NotFound();
            }

            // Atualiza o valor de IsDeleted para true
            local.IsDeleted = true;
            _context.Entry(local).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalExists(id))
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

        private bool LocalExists(int id)
        {
            return _context.Locais.Any(e => e.LocalId == id);
        }
    }
}
