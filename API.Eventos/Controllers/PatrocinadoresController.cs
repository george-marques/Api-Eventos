using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Eventos.Entities;
using API.Eventos.Persistence;
using Microsoft.AspNetCore.Authorization;

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

        /// <summary>
        /// Obtém todos os patrocinadores ativos disponíveis.
        /// </summary>
        /// <remarks>
        /// ### Descrição
        /// Este método retorna uma lista de patrocinadores ativos. 
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "patrocinadorId": 0,
        ///     "nome": "George Daniel",
        ///     "contato": "(69)30511-0131",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        ///
        /// ### Notas
        /// - Apenas patrocinadores não excluídos (`IsDeleted = false`) serão retornados.
        /// </remarks>
        /// <response code="200">Retorna um IEnumerable de patrocinadores ativos.</response>
        /// <response code="400">Se encontrar um erro.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Patrocinador>>> GetPatrocinadores()
        {
            return await _context.Patrocinadores
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um patrocinador específico pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador do patrocinador.</param>
        /// <returns>Retorna os detalhes completos do patrocinador solicitado, ou um código de erro caso o patrocinador não seja encontrado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método busca um patrocinador específico pelo ID. 
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "patrocinadorId": 1,
        ///     "nome": "George Daniel",
        ///     "contato": "(69)30511-0131",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **404 Not Found**: O patrocinador com o ID especificado não foi encontrado.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado no servidor.
        ///
        /// ### Notas
        /// - Apenas patrocinadores não excluídos (`IsDeleted = false`) serão retornadas.
        /// </remarks>
        /// <response code="200">Retorna o patrocinador com o identificador especificado.</response>
        /// <response code="404">Se o patrocinador não for encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Patrocinador>> GetPatrocinador(int id)
        {
            var patrocinador = await _context.Patrocinadores
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.PatrocinadorId == id);

            if (patrocinador == null)
            {
                return NotFound();
            }

            return patrocinador;
        }

        /// <summary>
        /// Atualiza os dados de um patrocinador existente.
        /// </summary>
        /// <param name="id">O identificador do patrocinador a ser atualizado.</param>
        /// <param name="patrocinador">O objeto do patrocinador com os novos dados.</param>
        /// <returns>Retorna o status da operação, indicando sucesso ou erro.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método atualiza as informações de um patrocinador existente.  
        /// Certifique-se de que o ID fornecido no parâmetro corresponde ao ID do objeto patrocinador enviado no corpo da requisição.
        ///
        /// ### Regras e Validações
        /// - O ID do patrocinador no parâmetro **deve** ser igual a `patrocinadorId` no corpo da requisição.
        /// - Se o patrocinador não existir, será retornado **404 Not Found**.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "patrocinadorId": 1,
        ///     "nome": "George Daniel",
        ///     "contato": "(69)30511-0131",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **400 Bad Request**: O ID do patrocinador no parâmetro não corresponde ao ID no corpo da requisição.
        /// - **404 Not Found**: O patrocinador com o ID especificado não foi encontrado.
        ///
        /// ### Notas
        /// - Certifique-se de fornecer todos os campos obrigatórios do patrocinador.
        /// </remarks>
        /// <response code="204">O patrocinador foi atualizado com sucesso.</response>
        /// <response code="400">Se o identificador do patrocinador não coincidir.</response>
        /// <response code="404">Se o patrocinador não for encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Cria um novo patrocinador.
        /// </summary>
        /// <param name="patrocinador">Os dados do patrocinador a ser criado.</param>
        /// <returns>Retorna o patrocinador criado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método permite criar um novo patrocinador.  
        ///
        /// ### Regras e Validações
        /// - É necessário fornecer todas as propriedades obrigatórias do patrocinador para garantir a integridade dos dados.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "patrocinadorId": 0,
        ///     "nome": "George Daniel",
        ///     "contato": "(69)30511-0131",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **400 Bad Request**: A requisição é inválida ou faltam informações obrigatórias.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado ao salvar o patrocinador.
        ///
        /// </remarks>
        /// <response code="201">Retorna o patrocinador criado.</response>
        /// <response code="400">Se encontrar um erro nos dados fornecidos.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Patrocinador>> PostPatrocinador(Patrocinador patrocinador)
        {
            _context.Patrocinadores.Add(patrocinador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatrocinador", new { id = patrocinador.PatrocinadorId }, patrocinador);
        }

        /// <summary>
        /// Marca um patrocinador como excluído, definindo a propriedade <c>IsDeleted</c> como <c>true</c>.
        /// </summary>
        /// <param name="id">O identificador do patrocinador a ser excluído.</param>
        /// <returns>Retorna um status HTTP indicando o resultado da operação.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método realiza a exclusão lógica de um patrocinador, atualizando sua propriedade `IsDeleted` para `true`.  
        /// O patrocinador não será mais retornado nas consultas subsequentes, mas seus dados permanecem no banco de dados.
        ///
        /// ### Regras e Validações
        /// - O patrocinador deve existir no banco de dados e não pode já estar marcado como excluído.
        /// - Caso o patrocinador não seja encontrado, o método retornará o status **404 Not Found**.
        ///
        /// ### Exemplo de Requisição
        /// **DELETE** `/api/patrocinadores/1`
        ///
        /// ### Possíveis Erros
        /// - **404 Not Found**: O patrocinador com o `id` especificado não foi encontrado ou já está excluído.
        /// - **500 Internal Server Error**: Ocorreu um erro ao tentar atualizar o patrocinador no banco de dados.
        ///
        /// ### Notas
        /// - Este método realiza uma exclusão lógica, alterando o estado do patrocinador no banco de dados, mas sem removê-lo fisicamente.
        /// - O patrocinador será atualizado utilizando o `EntityState.Modified`, e o contexto será salvo após a alteração.
        /// </remarks>
        /// <response code="204">O patrocinador foi marcado como excluído com sucesso.</response>
        /// <response code="404">Se o patrocinador não for encontrado.</response>
        /// <response code="500">Erro interno ao tentar excluir o patrocinador.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePatrocinador(int id)
        {
            var patrocinador = await _context.Patrocinadores
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.PatrocinadorId == id);
            if (patrocinador == null)
            {
                return NotFound();
            }

            // Atualiza o valor de IsDeleted para true
            patrocinador.IsDeleted = true;
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

        private bool PatrocinadorExists(int id)
        {
            return _context.Patrocinadores.Any(e => e.PatrocinadorId == id);
        }
    }
}
