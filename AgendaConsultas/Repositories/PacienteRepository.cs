using AgendaConsultas.Data;
using AgendaConsultas.Models;

namespace AgendaConsultas.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly AppDbContext _context;

        public PacienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Paciente> GetAll()
        {
            return _context.Pacientes.ToList();
        }

        public Paciente? GetById(int id)
        {
            return _context.Pacientes.Find(id);
        }

        public void Add(Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var paciente = GetById(id);

            if (paciente == null)
                return;

            _context.Pacientes.Remove(paciente);
            _context.SaveChanges();
        }

        public void Update(Paciente paciente)
        {
            _context.Pacientes.Update(paciente);
            _context.SaveChanges();
        }
    }
}