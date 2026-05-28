using Microsoft.Data.SqlClient;
using TupMaterias.Models;

namespace TupMaterias.Data
{
    public class MateriaRepository
    {
        // Cambia "TupMateriasDb" por el nombre que pongas en appsettings.json
        private readonly string _connectionString;

        public MateriaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Obtener todas las materias con su profesor
        public List<Materia> ObtenerTodas()
        {
            var materias = new List<Materia>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var sql = @"
                SELECT m.Id, m.Nombre, m.Descripcion,
                       p.Id AS ProfesorId, p.Nombre AS ProfesorNombre, p.Email
                FROM Materias m
                JOIN Profesores p ON m.ProfesorId = p.Id
                ORDER BY m.Nombre";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                materias.Add(new Materia
                {
                    Id          = reader.GetInt32(reader.GetOrdinal("Id")),
                    Nombre      = reader.GetString(reader.GetOrdinal("Nombre")),
                    Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion"))
                                    ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                    Profesor = new Profesor
                    {
                        Id     = reader.GetInt32(reader.GetOrdinal("ProfesorId")),
                        Nombre = reader.GetString(reader.GetOrdinal("ProfesorNombre")),
                        Email  = reader.IsDBNull(reader.GetOrdinal("Email"))
                                    ? "" : reader.GetString(reader.GetOrdinal("Email"))
                    }
                });
            }

            return materias;
        }

        // -------------------------------------------------------
        // Obtener una materia por Id, con profesor Y alumnos
        // -------------------------------------------------------
        public Materia? ObtenerPorId(int id)
        {
            Materia? materia = null;

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            // Primero traemos la materia y el profesor
            var sqlMateria = @"
                SELECT m.Id, m.Nombre, m.Descripcion,
                       p.Id AS ProfesorId, p.Nombre AS ProfesorNombre, p.Email
                FROM Materias m
                JOIN Profesores p ON m.ProfesorId = p.Id
                WHERE m.Id = @Id";

            using (var cmd = new SqlCommand(sqlMateria, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    materia = new Materia
                    {
                        Id          = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre      = reader.GetString(reader.GetOrdinal("Nombre")),
                        Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion"))
                                        ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                        Profesor = new Profesor
                        {
                            Id     = reader.GetInt32(reader.GetOrdinal("ProfesorId")),
                            Nombre = reader.GetString(reader.GetOrdinal("ProfesorNombre")),
                            Email  = reader.IsDBNull(reader.GetOrdinal("Email"))
                                        ? "" : reader.GetString(reader.GetOrdinal("Email"))
                        }
                    };
                }
            }

            if (materia == null) return null;

            // Luego traemos los alumnos inscriptos en esa materia
            var sqlAlumnos = @"
                SELECT a.Id, a.Nombre, a.Legajo
                FROM Alumnos a
                JOIN Inscripciones i ON a.Id = i.AlumnoId
                WHERE i.MateriaId = @MateriaId
                ORDER BY a.Nombre";

            using (var cmd = new SqlCommand(sqlAlumnos, conn))
            {
                cmd.Parameters.AddWithValue("@MateriaId", id);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    materia.Alumnos.Add(new Alumno
                    {
                        Id     = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Legajo = reader.GetString(reader.GetOrdinal("Legajo"))
                    });
                }
            }

            return materia;
        }
    }
}
