namespace TupMaterias.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Profesor Profesor { get; set; }
        public List<Alumno> Alumnos { get; set; } = new List<Alumno>();
    }
}
