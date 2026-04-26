using Microsoft.AspNetCore.Mvc;
using AgendaConsultas.Models;
using AgendaConsultas.Repositories;

namespace AgendaConsultas.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteRepository _repository;
        private readonly IConsultaRepository _consultaRepository;

        public PacienteController(
            IPacienteRepository repository,
            IConsultaRepository consultaRepository)
        {
            _repository = repository;
            _consultaRepository = consultaRepository;
        }

        // LISTAR TODOS
        [HttpGet]
        public IActionResult Get()
        {
            var pacientes = _repository.GetAll();

            //  retornar lista vazia se não tiver
            return Ok(pacientes ?? new List<Paciente>());
        }
        
        [HttpPut("{id}")]
            public IActionResult Put(int id, Paciente paciente)
            {
                var existente = _repository.GetById(id);
    
                if (existente == null)
                    return NotFound("Paciente não encontrado");
    
                existente.Nome = paciente.Nome;
    
                _repository.Update(existente);
    
                return Ok(existente);
            }
        // BUSCAR POR ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var paciente = _repository.GetById(id);

            if (paciente == null)
                return NotFound("Paciente não encontrado");

            return Ok(paciente);
        }

        // CRIAR PACIENTE
        [HttpPost]
        public IActionResult Post(Paciente paciente)
        {
            if (string.IsNullOrWhiteSpace(paciente.Nome))
                return BadRequest("Nome é obrigatório");

            if (paciente.Nome.Length < 2)
                return BadRequest("Nome deve ter no mínimo 2 caracteres");

            if (paciente.Nome.Length > 100)
                return BadRequest("Nome deve ter no máximo 100 caracteres");

            _repository.Add(paciente);

            return Ok(paciente);
        }

        // DELETAR PACIENTE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var paciente = _repository.GetById(id);

            if (paciente == null)
                return NotFound("Paciente não encontrado");

            // não excluir paciente com consultas
            var possuiConsultas = _consultaRepository
                .GetByPacienteId(id)
                .Any();

            if (possuiConsultas)
                return BadRequest("Não é possível excluir paciente com consultas cadastradas");

            _repository.Delete(id);

            return Ok("Paciente removido");
        }
    }
}