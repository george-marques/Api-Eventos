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
    public class OrganizadoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrganizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os organizadores ativos disponíveis.
        /// </summary>
        /// <remarks>
        /// ### Descrição
        /// Este método retorna uma lista de organizadores ativos. 
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "organizadorId": 0,
        ///     "nome": "UNITINS",
        ///     "contato": "(35)27633-3714",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        ///
        /// ### Notas
        /// - Apenas organizadores não excluídos (`IsDeleted = false`) serão retornados.
        /// </remarks>
        /// <response code="200">Retorna um IEnumerable de organizadores ativos.</response>
        /// <response code="400">Se encontrar um erro.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Organizador>>> GetOrganizadores()
        {
            return await _context.Organizadores
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um organizador específico pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador do organizador.</param>
        /// <returns>Retorna os detalhes completos do organizador solicitado, ou um código de erro caso o organizador não seja encontrado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método busca um organizador específico pelo ID. 
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "organizadorId": 1,
        ///     "nome": "UNITINS",
        ///     "contato": "(35)27633-3714",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **404 Not Found**: O organizador com o ID especificado não foi encontrado.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado no servidor.
        ///
        /// ### Notas
        /// - Apenas organizadores não excluídos (`IsDeleted = false`) serão retornadas.
        /// </remarks>
        /// <response code="200">Retorna o organizador com o identificador especificado.</response>
        /// <response code="404">Se o organizador não for encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Organizador>> GetOrganizador(int id)
        {
            var organizador = await _context.Organizadores
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.OrganizadorId == id);

            if (organizador == null)
            {
                return NotFound();
            }

            return organizador;
        }

        /// <summary>
        /// Atualiza os dados de um organizador existente.
        /// </summary>
        /// <param name="id">O identificador do organizador a ser atualizado.</param>
        /// <param name="organizador">O objeto do organizador com os novos dados.</param>
        /// <returns>Retorna o status da operação, indicando sucesso ou erro.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método atualiza as informações de um organizador existente.  
        /// Certifique-se de que o ID fornecido no parâmetro corresponde ao ID do objeto organizador enviado no corpo da requisição.
        ///
        /// ### Regras e Validações
        /// - O ID do organizador no parâmetro **deve** ser igual a `organizadorId` no corpo da requisição.
        /// - Se o organizador não existir, será retornado **404 Not Found**.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "organizadorId": 1,
        ///     "nome": "UNITINS",
        ///     "contato": "(35)27633-3714",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **400 Bad Request**: O ID do organizador no parâmetro não corresponde ao ID no corpo da requisição.
        /// - **404 Not Found**: O organizador com o ID especificado não foi encontrado.
        ///
        /// ### Notas
        /// - Certifique-se de fornecer todos os campos obrigatórios do organizador.
        /// </remarks>
        /// <response code="204">O organizador foi atualizado com sucesso.</response>
        /// <response code="400">Se o identificador do organizador não coincidir.</response>
        /// <response code="404">Se o organizador não for encontrado.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutOrganizador(int id, Organizador organizador)
        {
            if (id != organizador.OrganizadorId)
            {
                return BadRequest();
            }

            _context.Entry(organizador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizadorExists(id))
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
        /// Cria um novo organizador.
        /// </summary>
        /// <param name="organizador">Os dados do organizador a ser criado.</param>
        /// <returns>Retorna o organizador criado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método permite criar um novo organizador.  
        ///
        /// ### Regras e Validações
        /// - É necessário fornecer todas as propriedades obrigatórias do organizador para garantir a integridade dos dados.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "organizadorId": 0,
        ///     "nome": "UNITINS",
        ///     "contato": "(35)27633-3714",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **400 Bad Request**: A requisição é inválida ou faltam informações obrigatórias.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado ao salvar o organizador.
        ///
        /// </remarks>
        /// <response code="201">Retorna o organizador criado.</response>
        /// <response code="400">Se encontrar um erro nos dados fornecidos.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Organizador>> PostOrganizador(Organizador organizador)
        {
            _context.Organizadores.Add(organizador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrganizador", new { id = organizador.OrganizadorId }, organizador);
        }

        /// <summary>
        /// Marca um organizador como excluído, definindo a propriedade <c>IsDeleted</c> como <c>true</c>.
        /// </summary>
        /// <param name="id">O identificador do organizador a ser excluído.</param>
        /// <returns>Retorna um status HTTP indicando o resultado da operação.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método realiza a exclusão lógica de um organizador, atualizando sua propriedade `IsDeleted` para `true`.  
        /// O organizador não será mais retornado nas consultas subsequentes, mas seus dados permanecem no banco de dados.
        ///
        /// ### Regras e Validações
        /// - O organizador deve existir no banco de dados e não pode já estar marcado como excluído.
        /// - Caso o organizador não seja encontrado, o método retornará o status **404 Not Found**.
        ///
        /// ### Exemplo de Requisição
        /// **DELETE** `/api/organizadores/1`
        ///
        /// ### Possíveis Erros
        /// - **404 Not Found**: O organizador com o `id` especificado não foi encontrado ou já está excluído.
        /// - **500 Internal Server Error**: Ocorreu um erro ao tentar atualizar o organizador no banco de dados.
        ///
        /// ### Notas
        /// - Este método realiza uma exclusão lógica, alterando o estado do organizador no banco de dados, mas sem removê-lo fisicamente.
        /// - O organizador será atualizado utilizando o `EntityState.Modified`, e o contexto será salvo após a alteração.
        /// </remarks>
        /// <response code="204">O organizador foi marcado como excluído com sucesso.</response>
        /// <response code="404">Se o organizador não for encontrado.</response>
        /// <response code="500">Erro interno ao tentar excluir o organizador.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOrganizador(int id)
        {
            var organizador = await _context.Organizadores
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.OrganizadorId == id);
            if (organizador == null)
            {
                return NotFound();
            }

            // Atualiza o valor de IsDeleted para true
            organizador.IsDeleted = true;
            _context.Entry(organizador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizadorExists(id))
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

        private bool OrganizadorExists(int id)
        {
            return _context.Organizadores.Any(e => e.OrganizadorId == id);
        }
    }
}
