using Microsoft.AspNetCore.Mvc;
using TupMaterias.Data;

namespace TupMaterias.Controllers
{
    public class MateriasController : Controller
    {
        private readonly MateriaRepository _repo;

        public MateriasController(IConfiguration config)
        {
            var connStr = config.GetConnectionString("TupMateriasDb")!;
            _repo = new MateriaRepository(connStr);
        }

        // GET: /Materias  → listado de todas las materias
        public IActionResult Index()
        {
            var materias = _repo.ObtenerTodas();
            return View(materias);
        }

        // GET: /Materias/Detalle/5  → detalle con alumnos
        public IActionResult Detalle(int id)
        {
            var materia = _repo.ObtenerPorId(id);
            if (materia == null)
                return NotFound();

            return View(materia);
        }
    }
}
