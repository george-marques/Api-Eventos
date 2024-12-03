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

        /// <summary>
        /// Obtém todos os eventos ativos disponíveis no sistema.
        /// </summary>
        /// <remarks>
        /// ### Descrição
        /// Este método retorna uma lista de eventos ativos. 
        /// Inclui informações sobre os patrocinadores associados a cada evento.  
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// {
        ///   "eventoId": 1,
        ///   "nome": "Evento de Tecnologia",
        ///   "descricao": "Evento para discutir inovações tecnológicas.",
        ///   "data": "2024-12-03",
        ///   "localId": 2,
        ///   "capacidade": 500,
        ///   "organizadorId": 3,
        ///   "isDeleted": false,
        ///   "patrocinadores": [
        ///     {
        ///       "patrocinadorId": 10,
        ///       "nome": "TechCorp",
        ///       "contato": "(11)98765-4321",
        ///       "isDeleted": false
        ///     }
        ///   ]
        /// }
        /// ```
        ///
        /// ### Notas
        /// - Apenas eventos não excluídos (`IsDeleted = false`) serão retornados.
        /// </remarks>
        /// <response code="200">Retorna um IEnumerable de eventos ativos.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            return await _context.Eventos
                .Where(e => !e.IsDeleted)
                .Include(e => e.Patrocinadores)
                .ToListAsync();
        }


        /// <summary>
        /// Obtém os detalhes de um evento ativo específico.
        /// </summary>
        /// <param name="id"> O identificador único do evento.</param>
        /// <returns>Retorna os detalhes completos do evento solicitado, incluindo seus patrocinadores, ou um código de erro caso o evento não seja encontrado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método busca um evento específico pelo ID. 
        /// Inclui informações detalhadas sobre o evento, como patrocinadores associados.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// {
        ///   "eventoId": 1,
        ///   "nome": "Evento de Tecnologia",
        ///   "descricao": "Evento para discutir inovações tecnológicas.",
        ///   "data": "2024-12-03",
        ///   "localId": 2,
        ///   "capacidade": 500,
        ///   "organizadorId": 3,
        ///   "isDeleted": false,
        ///   "patrocinadores": [
        ///     {
        ///       "patrocinadorId": 10,
        ///       "nome": "TechCorp",
        ///       "contato": "(11)98765-4321",
        ///       "isDeleted": false
        ///     }
        ///   ]
        /// }
        /// ```
        ///
        /// ### Possíveis Erros
        /// - **404 Not Found**: O evento com o ID especificado não foi encontrado.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado no servidor.
        ///
        /// ### Notas
        /// - Apenas eventos não excluídos (`IsDeleted = false`) serão retornados.
        /// </remarks>
        /// <response code="404">Evento não encontrado.</response>
        /// <response code="200">Retorna os detalhes do evento solicitado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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


        /// <summary>
        /// Atualiza os dados de um evento existente.
        /// </summary>
        /// <param name="id">O identificador único do evento que será atualizado.</param>
        /// <param name="evento">Os dados atualizados do evento.</param>
        /// <returns>Retorna o status da operação, indicando sucesso ou erro.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método atualiza as informações de um evento existente.  
        /// Certifique-se de que o ID fornecido no parâmetro corresponde ao ID do objeto evento enviado no corpo da requisição.
        ///
        /// ### Regras e Validações
        /// - O ID do evento no parâmetro **deve** ser igual ao `EventoId` no corpo da requisição.
        /// - Se o evento não existir, será retornado **404 Not Found**.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// {
        ///   "eventoId": 1,
        ///   "nome": "Evento Atualizado",
        ///   "descricao": "Descrição atualizada do evento.",
        ///   "data": "2024-12-10",
        ///   "localId": 3,
        ///   "capacidade": 200,
        ///   "organizadorId": 5,
        ///   "isDeleted": false,
        ///   "patrocinadores": [
        ///     {
        ///       "patrocinadorId": 2,
        ///       "nome": "Nova Empresa",
        ///       "contato": "(99)87654-3210",
        ///       "isDeleted": false
        ///     }
        ///   ]
        /// }
        /// ```
        ///
        /// ### Possíveis Erros
        /// - **400 Bad Request**: O ID do evento no parâmetro não corresponde ao ID no corpo da requisição.
        /// - **404 Not Found**: O evento com o ID especificado não foi encontrado.
        ///
        /// ### Notas
        /// - Certifique-se de fornecer todos os campos obrigatórios do evento.
        /// </remarks>
        /// <response code="204">Os dados do evento foram atualizados com sucesso.</response>
        /// <response code="400">O ID do evento não corresponde ou a requisição é inválida.</response>
        /// <response code="404">O evento com o ID especificado não foi encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.EventoId)
            {
                return BadRequest();
            }
            _context.Entry(evento).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
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
        /// Cria um novo evento e seus patrocinadores associados.
        /// </summary>
        /// <param name="evento">Os dados do evento a ser criado.</param>
        /// <returns>Retorna o evento criado e o local onde ele pode ser acessado.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método permite criar um novo evento.  
        /// Caso o evento possua patrocinadores associados, os patrocinadores que não possuem `PatrocinadorId` serão adicionados automaticamente ao banco de dados.
        ///
        /// ### Regras e Validações
        /// - Se a propriedade `Patrocinadores` do evento contiver itens, os patrocinadores com `PatrocinadorId = 0` serão criados como novos registros.
        /// - É necessário fornecer todas as propriedades obrigatórias do evento para garantir a integridade dos dados.
        ///
        /// ### Exemplo de Retorno
        /// ```json
        /// {
        ///   "nome": "Novo Evento",
        ///   "descricao": "Descrição do novo evento.",
        ///   "data": "2024-12-15",
        ///   "localId": 2,
        ///   "capacidade": 500,
        ///   "organizadorId": 3,
        ///   "isDeleted": false,
        ///   "patrocinadores": [
        ///     {
        ///       "patrocinadorId": 0,
        ///       "nome": "Empresa X",
        ///       "contato": "(11)99999-9999",
        ///       "isDeleted": false
        ///     }
        ///   ]
        /// }
        /// ```
        ///
        /// ### Possíveis Erros
        /// - **400 Bad Request**: A requisição é inválida ou faltam informações obrigatórias.
        /// - **500 Internal Server Error**: Ocorreu um erro inesperado ao salvar o evento ou seus patrocinadores.
        ///
        /// ### Notas
        /// - O método cria um novo evento e o retorna junto com os patrocinadores cadastrados.
        /// - Patrocinadores novos (com `PatrocinadorId = 0`) serão adicionados ao banco de dados antes de salvar o evento.
        ///
        /// </remarks>
        /// <response code="201">O evento foi criado com sucesso.</response>
        /// <response code="400">A requisição é inválida ou contém dados inconsistentes.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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


        /// <summary>
        /// Marca um evento como excluído, definindo sua propriedade <c>IsDeleted</c> como <c>true</c>.
        /// </summary>
        /// <param name="id">O identificador único do evento a ser excluído.</param>
        /// <returns>Retorna um status HTTP indicando o resultado da operação.</returns>
        /// <remarks>
        /// ### Descrição
        /// Este método realiza a exclusão lógica de um evento, atualizando sua propriedade `IsDeleted` para `true`.  
        /// O evento não será mais retornado nas consultas subsequentes, mas seus dados permanecem no banco de dados.
        ///
        /// ### Regras e Validações
        /// - O evento deve existir no banco de dados e não pode já estar marcado como excluído.
        /// - Caso o evento não seja encontrado, o método retornará o status **404 Not Found**.
        ///
        /// ### Exemplo de Requisição
        /// **DELETE** `/api/eventos/1`
        ///
        /// ### Possíveis Erros
        /// - **404 Not Found**: O evento com o `id` especificado não foi encontrado ou já está excluído.
        /// - **500 Internal Server Error**: Ocorreu um erro ao tentar atualizar o evento no banco de dados.
        ///
        /// ### Notas
        /// - Este método realiza uma exclusão lógica, alterando o estado do evento no banco de dados, mas sem removê-lo fisicamente.
        /// - O evento será atualizado utilizando o `EntityState.Modified`, e o contexto será salvo após a alteração.
        ///
        /// </remarks>
        /// <response code="204">O evento foi marcado como excluído com sucesso.</response>
        /// <response code="404">O evento não foi encontrado.</response>
        /// <response code="500">Erro interno ao tentar excluir o evento.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(e => e.EventoId == id);
            if (evento == null)
            {
                return NotFound();
            }

            // Atualiza o valor de IsDeleted para true
            evento.IsDeleted = true;
            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
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

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.EventoId == id);
        }
    }
}
