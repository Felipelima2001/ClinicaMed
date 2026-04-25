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

        public PacienteController(IPacienteRepository repository)
        {
            _repository = repository;
        }

        // 🔍 LISTAR TODOS
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.GetAll());
        }

        // 🔍 BUSCAR POR ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var paciente = _repository.GetById(id);

            if (paciente == null)
                return NotFound("Paciente não encontrado");

            return Ok(paciente);
        }

        // ➕ CRIAR PACIENTE
        [HttpPost]
        public IActionResult Post(Paciente paciente)
        {
            if (string.IsNullOrWhiteSpace(paciente.Nome))
                return BadRequest("Nome é obrigatório");

            _repository.Add(paciente);

            return Ok(paciente);
        }

        // ❌ DELETAR PACIENTE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var paciente = _repository.GetById(id);

            if (paciente == null)
                return NotFound("Paciente não encontrado");

            _repository.Delete(id);

            return Ok("Paciente removido");
        }
    }
}