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

        /// <summary>
        /// Obtém todos os locais ativos disponíveis.
        /// </summary>
        /// <remarks>
        /// ### Descrição
        /// Este método retorna uma lista de locais ativos. 
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "localId": 0,
        ///     "nome": "Capim Dourado Shopping",
        ///     "endereco": "Palmas-TO",
        ///     "participanteId": 0,
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        ///
        /// ### Notas
        /// - Apenas locais não excluídos (`IsDeleted = false`) serão retornados.
        /// </remarks>
        /// <response code="200">Retorna um IEnumerable de locais ativos.</response>
        /// <response code="400">Se encontrar um erro.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Local>>> GetLocais()
        {
            return await _context.Locais
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um local específico pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador do local.</param>
        /// <returns>Retorna os detalhes completos do local solicitado, ou um código de erro caso o local não seja encontrado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método busca um local específico pelo ID. 
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "localId": 0,
        ///     "nome": "Capim Dourado Shopping",
        ///     "endereco": "Palmas-TO",
        ///     "participanteId": 0,
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **404 Not Found**: O local com o ID especificado não foi encontrado.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado no servidor.
        ///
        /// ### Notas
        /// - Apenas locais não excluídos (`IsDeleted = false`) serão retornadas.
        /// </remarks>
        /// <response code="200">Retorna o local com o identificador especificado.</response>
        /// <response code="404">Se o local não for encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Atualiza os dados de um local existente.
        /// </summary>
        /// <param name="id">O identificador do local a ser atualizado.</param>
        /// <param name="local">O objeto do local com os novos dados.</param>
        /// <returns>Retorna o status da operação, indicando sucesso ou erro.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método atualiza as informações de um local existente.  
        /// Certifique-se de que o ID fornecido no parâmetro corresponde ao ID do objeto local enviado no corpo da requisição.
        ///
        /// ### Regras e Validações
        /// - O ID do local no parâmetro **deve** ser igual a `LocalId` no corpo da requisição.
        /// - Se o local não existir, será retornado **404 Not Found**.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "localId": 0,
        ///     "nome": "Capim Dourado Shopping",
        ///     "endereco": "Palmas-TO",
        ///     "participanteId": 0,
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **400 Bad Request**: O ID do local no parâmetro não corresponde ao ID no corpo da requisição.
        /// - **404 Not Found**: O local com o ID especificado não foi encontrado.
        ///
        /// ### Notas
        /// - Certifique-se de fornecer todos os campos obrigatórios do local.
        /// </remarks>
        /// <response code="204">O local foi atualizado com sucesso.</response>
        /// <response code="400">Se o identificador do local não coincidir.</response>
        /// <response code="404">Se o local não for encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Cria um novo local.
        /// </summary>
        /// <param name="local">Os dados do local a ser criado.</param>
        /// <returns>Retorna o local criado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método permite criar um novo local.  
        ///
        /// ### Regras e Validações
        /// - É necessário fornecer todas as propriedades obrigatórias do local para garantir a integridade dos dados.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "localId": 0,
        ///     "nome": "Capim Dourado Shopping",
        ///     "endereco": "Palmas-TO",
        ///     "participanteId": 0,
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **400 Bad Request**: A requisição é inválida ou faltam informações obrigatórias.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado ao salvar o local.
        ///
        /// </remarks>
        /// <response code="201">Retorna o local criado.</response>
        /// <response code="400">Se encontrar um erro nos dados fornecidos.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Local>> PostLocal(Local local)
        {
            _context.Locais.Add(local);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocal", new { id = local.LocalId }, local);
        }

        /// <summary>
        /// Marca um local como excluído, definindo a propriedade <c>IsDeleted</c> como <c>true</c>.
        /// </summary>
        /// <param name="id">O identificador do local a ser excluído.</param>
        /// <returns>Retorna um status HTTP indicando o resultado da operação.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método realiza a exclusão lógica de um local, atualizando sua propriedade `IsDeleted` para `true`.  
        /// O local não será mais retornado nas consultas subsequentes, mas seus dados permanecem no banco de dados.
        ///
        /// ### Regras e Validações
        /// - O local deve existir no banco de dados e não pode já estar marcado como excluído.
        /// - Caso o local não seja encontrado, o método retornará o status **404 Not Found**.
        ///
        /// ### Exemplo de Requisição
        /// **DELETE** `/api/locais/1`
        ///
        /// ### Possíveis Erros
        /// - **404 Not Found**: O local com o `id` especificado não foi encontrado ou já está excluído.
        /// - **500 Internal Server Error**: Ocorreu um erro ao tentar atualizar o local no banco de dados.
        ///
        /// ### Notas
        /// - Este método realiza uma exclusão lógica, alterando o estado do local no banco de dados, mas sem removê-lo fisicamente.
        /// - O local será atualizado utilizando o `EntityState.Modified`, e o contexto será salvo após a alteração.
        /// </remarks>
        /// <response code="204">O local foi marcado como excluído com sucesso.</response>
        /// <response code="404">Se o local não for encontrado.</response>
        /// <response code="500">Erro interno ao tentar excluir o local.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
