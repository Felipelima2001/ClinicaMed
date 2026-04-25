using Microsoft.AspNetCore.Mvc;
using AgendaConsultas.Models;
using AgendaConsultas.Repositories;
using AgendaConsultas.Enums;

namespace AgendaConsultas.Controllers
{
    [ApiController]
    [Route("api/consultas")]
    public class ConsultaController : ControllerBase
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IPacienteRepository _pacienteRepository;

        public ConsultaController(
            IConsultaRepository consultaRepository,
            IPacienteRepository pacienteRepository)
        {
            _consultaRepository = consultaRepository;
            _pacienteRepository = pacienteRepository;
        }

        //  LISTAR TODAS
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_consultaRepository.GetAll());
        }

        // BUSCAR POR ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var consulta = _consultaRepository.GetById(id);

            if (consulta == null)
                return NotFound("Consulta não encontrada");

            return Ok(consulta);
        }

        //  CRIAR CONSULTA
        [HttpPost]
        public IActionResult Post(Consulta consulta)
        {
            //  REGRA 1 — NÃO NO PASSADO
            if (consulta.Data < DateTime.Now)
                return BadRequest("Não pode agendar no passado");

            //  REGRA 2 — PACIENTE EXISTE
            var paciente = _pacienteRepository.GetById(consulta.PacienteId);
            if (paciente == null)
                return BadRequest("Paciente não encontrado");

            //  REGRA 3 — CONFLITO DE HORÁRIO
            var conflito = _consultaRepository
                .GetByPacienteId(consulta.PacienteId)
                .Any(c => c.Data == consulta.Data);

            if (conflito)
                return BadRequest("Conflito de horário");

            //  REGRA 4 — STATUS AUTOMÁTICO
            consulta.Status = StatusConsulta.Agendada;

            _consultaRepository.Add(consulta);

            return Ok("Consulta agendada");
        }

        //  ATUALIZAR CONSULTA
        [HttpPut("{id}")]
        public IActionResult Put(int id, Consulta novaConsulta)
        {
            var consulta = _consultaRepository.GetById(id);

            if (consulta == null)
                return NotFound("Consulta não encontrada");

            // 🔢 REGRA 5 — NÃO ALTERAR CONCLUÍDA
            if (consulta.Status == StatusConsulta.Concluida)
                return BadRequest("Não pode alterar consulta concluída");

            // 🔢 REGRA 1 (REAPLICADA)
            if (novaConsulta.Data < DateTime.Now)
                return BadRequest("Data inválida");

            //  REGRA 3 (REAPLICADA)
            var conflito = _consultaRepository
                .GetByPacienteId(consulta.PacienteId)
                .Any(c => c.Data == novaConsulta.Data && c.Id != id);

            if (conflito)
                return BadRequest("Conflito de horário");

            consulta.Data = novaConsulta.Data;

            _consultaRepository.Update(consulta);

            return Ok("Consulta atualizada");
        }

        //  ATUALIZAR STATUS
        [HttpPut("{id}/status")]
        public IActionResult AtualizarStatus(int id, StatusConsulta status)
        {
            var consulta = _consultaRepository.GetById(id);

            if (consulta == null)
                return NotFound("Consulta não encontrada");

            // 🔢 REGRA 6 — NÃO ALTERAR CONCLUÍDA
            if (consulta.Status == StatusConsulta.Concluida)
                return BadRequest("Consulta já concluída não pode ser alterada");

            consulta.Status = status;

            _consultaRepository.Update(consulta);

            return Ok("Status atualizado");
        }

        //  DELETAR CONSULTA
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var consulta = _consultaRepository.GetById(id);

            if (consulta == null)
                return NotFound("Consulta não encontrada");

            if (consulta.Status == StatusConsulta.Concluida)
                return BadRequest("Não é possível excluir consulta concluída");

            if (consulta.Status != StatusConsulta.Agendada)
                return BadRequest("Apenas consultas agendadas podem ser excluídas");

            _consultaRepository.Delete(id);

            return Ok("Consulta removida");
        }
    }
}