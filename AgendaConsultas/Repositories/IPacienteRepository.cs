using AgendaConsultas.Models;

namespace AgendaConsultas.Repositories
{
    public interface IPacienteRepository
    {
        List<Paciente> GetAll();
        Paciente? GetById(int id);

        void Add(Paciente paciente);
        void Delete(int id);
    }
}