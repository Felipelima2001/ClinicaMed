using AgendaConsultas.Models;

namespace AgendaConsultas.Repositories
{
    public interface IConsultaRepository
    {
        List<Consulta> GetAll();
        Consulta? GetById(int id);
        List<Consulta> GetByPacienteId(int pacienteId);

        void Add(Consulta consulta);
        void Update(Consulta consulta);
        void Delete(int id);
    }
}