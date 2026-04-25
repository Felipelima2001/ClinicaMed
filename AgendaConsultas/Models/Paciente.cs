namespace AgendaConsultas.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

       public List<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}