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

        /// <summary>
        /// Obtém todas as inscrições ativas.
        /// </summary>
        /// <remarks>
        /// ### Descrição
        /// Este método retorna uma lista de inscriçõe ativas. 
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "inscricaoId": 0,
        ///     "dataInscricao": "2024-12-03",
        ///     "eventoId": 0,
        ///     "participanteId": 0,
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Notas
        /// - Apenas inscrições não excluídas (`IsDeleted = false`) serão retornadas.
        /// </remarks>
        /// <response code="200">Retorna uma lista de inscrições ativas.</response>
        /// <response code="400">Se encontrar um erro.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Inscricao>>> GetInscricoes()
        {
            return await _context.Inscricoes
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém uma inscrição específica pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador da inscrição.</param>
        /// <returns>Retorna os detalhes completos da inscrição solicitada, ou um código de erro caso o evento não seja encontrado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método busca uma inscrição específica pelo ID. 
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// {
        ///   "inscricaoId": 0,
        ///   "dataInscricao": "2024-12-03",
        ///   "eventoId": 0,
        ///   "participanteId": 0,
        ///   "isDeleted": false
        /// }
        /// ```
        /// ### Possíveis Erros
        /// - **404 Not Found**: A inscrição com o ID especificado não foi encontrado.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado no servidor.
        ///
        /// ### Notas
        /// - Apenas inscrições não excluídos (`IsDeleted = false`) serão retornadas.
        /// </remarks>
        /// <response code="200">Retorna a inscrição com o identificador especificado.</response>
        /// <response code="404">Se a inscrição não for encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Atualiza os dados de uma inscrição existente.
        /// </summary>
        /// <param name="id">O identificador da inscrição a ser atualizada.</param>
        /// <param name="inscricao">O objeto da inscrição com os novos dados.</param>
        /// <returns>Retorna o status da operação, indicando sucesso ou erro.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método atualiza as informações de uma inscrição existente.  
        /// Certifique-se de que o ID fornecido no parâmetro corresponde ao ID do objeto inscrição enviado no corpo da requisição.
        ///
        /// ### Regras e Validações
        /// - O ID da inscrição no parâmetro **deve** ser igual a `InscricaoId` no corpo da requisição.
        /// - Se a inscrição não existir, será retornado **404 Not Found**.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// {
        ///   "inscricaoId": 1,
        ///   "dataInscricao": "2024-12-03",
        ///   "eventoId": 1,
        ///   "participanteId": 2,
        ///   "isDeleted": false
        /// }
        /// ```
        /// ### Possíveis Erros
        /// - **400 Bad Request**: O ID da inscrição no parâmetro não corresponde ao ID no corpo da requisição.
        /// - **404 Not Found**: A inscrição com o ID especificado não foi encontrado.
        ///
        /// ### Notas
        /// - Certifique-se de fornecer todos os campos obrigatórios da inscrição.
        /// </remarks>
        /// <response code="204">A inscrição foi atualizada com sucesso.</response>
        /// <response code="400">Se o identificador da inscrição não coincidir.</response>
        /// <response code="404">Se a inscrição não for encontrada.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Cria uma nova inscrição.
        /// </summary>
        /// <param name="inscricao">Os dados da inscrição a ser criada.</param>
        /// <returns>Retorna a inscrição criada e o local onde ele pode ser acessado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método permite criar uma nova inscrição.  
        ///
        /// ### Regras e Validações
        /// - É necessário fornecer todas as propriedades obrigatórias da inscrição para garantir a integridade dos dados.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// {
        ///   "inscricaoId": 0,
        ///   "dataInscricao": "2024-12-03",
        ///   "eventoId": 0,
        ///   "participanteId": 0,
        ///   "isDeleted": false
        /// }
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **400 Bad Request**: A requisição é inválida ou faltam informações obrigatórias.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado ao salvar a inscrição.
        ///
        /// </remarks>
        /// <response code="201">Retorna a inscrição criada.</response>
        /// <response code="400">Se encontrar um erro nos dados fornecidos.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Inscricao>> PostInscricao(Inscricao inscricao)
        {
            _context.Inscricoes.Add(inscricao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInscricao", new { id = inscricao.InscricaoId }, inscricao);
        }

        /// <summary>
        /// Marca uma inscrição como excluída, definindo a propriedade <c>IsDeleted</c> como <c>true</c>.
        /// </summary>
        /// <param name="id">O identificador da inscrição a ser excluída.</param>
        /// <returns>Retorna um status HTTP indicando o resultado da operação.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método realiza a exclusão lógica de uma inscrição, atualizando sua propriedade `IsDeleted` para `true`.  
        /// A inscrição não será mais retornado nas consultas subsequentes, mas seus dados permanecem no banco de dados.
        ///
        /// ### Regras e Validações
        /// - A inscrição deve existir no banco de dados e não pode já estar marcado como excluído.
        /// - Caso a inscrição não seja encontrado, o método retornará o status **404 Not Found**.
        ///
        /// ### Exemplo de Requisição
        /// **DELETE** `/api/inscricoes/1`
        ///
        /// ### Possíveis Erros
        /// - **404 Not Found**: A inscrição com o `id` especificado não foi encontrado ou já está excluído.
        /// - **500 Internal Server Error**: Ocorreu um erro ao tentar atualizar a inscrição no banco de dados.
        ///
        /// ### Notas
        /// - Este método realiza uma exclusão lógica, alterando o estado da inscrição no banco de dados, mas sem removê-lo fisicamente.
        /// - A inscrição será atualizado utilizando o `EntityState.Modified`, e o contexto será salvo após a alteração.
        /// </remarks>
        /// <response code="204">A inscrição foi marcada como excluída com sucesso.</response>
        /// <response code="404">Se a inscrição não for encontrada.</response>
        /// <response code="500">Erro interno ao tentar excluir o evento.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
