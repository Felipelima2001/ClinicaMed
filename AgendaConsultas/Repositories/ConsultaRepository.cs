using AgendaConsultas.Data;
using AgendaConsultas.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaConsultas.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly AppDbContext _context;

        public ConsultaRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Consulta> GetAll()
        {
            return _context.Consultas
                .Include(c => c.Paciente)
                .ToList();
        }

        public Consulta? GetById(int id)
        {
            return _context.Consultas
                .Include(c => c.Paciente)
                .FirstOrDefault(c => c.Id == id);
        }

        public List<Consulta> GetByPacienteId(int pacienteId)
        {
            return _context.Consultas
                .Where(c => c.PacienteId == pacienteId)
                .ToList();
        }

        public void Add(Consulta consulta)
        {
            _context.Consultas.Add(consulta);
            _context.SaveChanges();
        }

        public void Update(Consulta consulta)
        {
            _context.Consultas.Update(consulta);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var consulta = GetById(id);

            if (consulta == null)
                return;

            _context.Consultas.Remove(consulta);
            _context.SaveChanges();
        }
    }
}