using AgendaConsultas.Enums;

namespace AgendaConsultas.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }

        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public StatusConsulta Status { get; set; }
    }
}