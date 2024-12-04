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
    public class ParticipantesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParticipantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os participantes ativos disponíveis.
        /// </summary>
        /// <remarks>
        /// ### Descrição
        /// Este método retorna uma lista de participantes ativos. 
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "participanteId": 0,
        ///     "nome": "Itamar Junior",
        ///     "email": "itamar@unitins.br",
        ///     "cpf": "063.143.371-87",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        ///
        /// ### Notas
        /// - Apenas participantes não excluídos (`IsDeleted = false`) serão retornados.
        /// </remarks>
        /// <response code="200">Retorna um IEnumerable de participantes ativos.</response>
        /// <response code="400">Se encontrar um erro.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Participante>>> GetParticipantes()
        {
            return await _context.Participantes
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um participante específico pelo seu identificador.
        /// </summary>
        /// <param name="id">O identificador do participante.</param>
        /// <returns>Retorna os detalhes completos do participante solicitado, ou um código de erro caso o participante não seja encontrado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método busca um participante específico pelo ID. 
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "participanteId": 1,
        ///     "nome": "Itamar Junior",
        ///     "email": "itamar@unitins.br",
        ///     "cpf": "063.143.371-87",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **404 Not Found**: O participante com o ID especificado não foi encontrado.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado no servidor.
        ///
        /// ### Notas
        /// - Apenas participantes não excluídos (`IsDeleted = false`) serão retornadas.
        /// </remarks>
        /// <response code="200">Retorna o participante com o identificador especificado.</response>
        /// <response code="404">Se o participante não for encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Participante>> GetParticipante(int id)
        {
            var participante = await _context.Participantes
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.ParticipanteId == id);

            if (participante == null)
            {
                return NotFound();
            }

            return participante;
        }

        /// <summary>
        /// Atualiza os dados de um participante existente.
        /// </summary>
        /// <param name="id">O identificador do participante a ser atualizado.</param>
        /// <param name="participante">O objeto do participante com os novos dados.</param>
        /// <returns>Retorna o status da operação, indicando sucesso ou erro.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método atualiza as informações de um participante existente.  
        /// Certifique-se de que o ID fornecido no parâmetro corresponde ao ID do objeto participante enviado no corpo da requisição.
        ///
        /// ### Regras e Validações
        /// - O ID do participante no parâmetro **deve** ser igual a `participanteId` no corpo da requisição.
        /// - Se o participante não existir, será retornado **404 Not Found**.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "participanteId": 1,
        ///     "nome": "Itamar Junior",
        ///     "email": "itamar@unitins.br",
        ///     "cpf": "063.143.371-87",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **400 Bad Request**: O ID do participante no parâmetro não corresponde ao ID no corpo da requisição.
        /// - **404 Not Found**: O participante com o ID especificado não foi encontrado.
        ///
        /// ### Notas
        /// - Certifique-se de fornecer todos os campos obrigatórios do participante.
        /// </remarks>
        /// <response code="204">O participante foi atualizado com sucesso.</response>
        /// <response code="400">Se o identificador do participante não coincidir.</response>
        /// <response code="404">Se o participante não for encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutParticipante(int id, Participante participante)
        {
            if (id != participante.ParticipanteId)
            {
                return BadRequest();
            }

            _context.Entry(participante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipanteExists(id))
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
        /// Cria um novo participante.
        /// </summary>
        /// <param name="participante">Os dados do participante a ser criado.</param>
        /// <returns>Retorna o participante criado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método permite criar um novo participante.  
        ///
        /// ### Regras e Validações
        /// - É necessário fornecer todas as propriedades obrigatórias do participante para garantir a integridade dos dados.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// [
        ///   {
        ///     "participanteId": 0,
        ///     "nome": "Itamar Junior",
        ///     "email": "itamar@unitins.br",
        ///     "cpf": "063.143.371-87",
        ///     "isDeleted": false
        ///   }
        /// ]
        /// ```
        /// 
        /// ### Possíveis Erros
        /// - **400 Bad Request**: A requisição é inválida ou faltam informações obrigatórias.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado ao salvar o participante.
        ///
        /// </remarks>
        /// <response code="201">Retorna o participante criado.</response>
        /// <response code="400">Se encontrar um erro nos dados fornecidos.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Participante>> PostParticipante(Participante participante)
        {
            _context.Participantes.Add(participante);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParticipante", new { id = participante.ParticipanteId }, participante);
        }

        /// <summary>
        /// Marca um participante como excluído, definindo a propriedade <c>IsDeleted</c> como <c>true</c>.
        /// </summary>
        /// <param name="id">O identificador do participante a ser excluído.</param>
        /// <returns>Retorna um status HTTP indicando o resultado da operação.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método realiza a exclusão lógica de um participante, atualizando sua propriedade `IsDeleted` para `true`.  
        /// O participante não será mais retornado nas consultas subsequentes, mas seus dados permanecem no banco de dados.
        ///
        /// ### Regras e Validações
        /// - O participante deve existir no banco de dados e não pode já estar marcado como excluído.
        /// - Caso o participante não seja encontrado, o método retornará o status **404 Not Found**.
        ///
        /// ### Exemplo de Requisição
        /// **DELETE** `/api/participantes/1`
        ///
        /// ### Possíveis Erros
        /// - **404 Not Found**: O participante com o `id` especificado não foi encontrado ou já está excluído.
        /// - **500 Internal Server Error**: Ocorreu um erro ao tentar atualizar o participante no banco de dados.
        ///
        /// ### Notas
        /// - Este método realiza uma exclusão lógica, alterando o estado do participante no banco de dados, mas sem removê-lo fisicamente.
        /// - O participante será atualizado utilizando o `EntityState.Modified`, e o contexto será salvo após a alteração.
        /// </remarks>
        /// <response code="204">O participante foi marcado como excluído com sucesso.</response>
        /// <response code="404">Se o participante não for encontrado.</response>
        /// <response code="500">Erro interno ao tentar excluir o participante.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteParticipante(int id)
        {
            var participante = await _context.Participantes
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.ParticipanteId == id);
            if (participante == null)
            {
                return NotFound();
            }

            // Atualiza o valor de IsDeleted para true
            participante.IsDeleted = true;
            _context.Entry(participante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipanteExists(id))
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

        private bool ParticipanteExists(int id)
        {
            return _context.Participantes.Any(e => e.ParticipanteId == id);
        }
    }
}
