using EscuelaDB;
using EscuelaDB.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ExamenEscuela.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private EscuelaContext _context;

        public CursoController(EscuelaContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CrearCurso([FromBody] Curso curso)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Curso_CRUD @Operacion, @idCurso, @nombre, @descripcion, @idProfesor",
                new SqlParameter("@Operacion", "CREATE"),
                new SqlParameter("@idCurso", DBNull.Value),
                new SqlParameter("@nombre", curso.nombre),
                new SqlParameter("@descripcion", curso.descripcion),
                new SqlParameter("@idProfesor", curso.idProfesor)
            );

            return Ok("Curso creado correctamente");
        }

        [HttpGet]
        public async Task<ActionResult<List<Curso>>> ObtenerCursos()
        {
            var cursos = await _context.Set<Curso>().FromSqlRaw(
                "EXEC sp_Curso_CRUD @Operacion, @idCurso, @nombre, @descripcion, @idProfesor",
                new SqlParameter("@Operacion", "READ"),
                new SqlParameter("@idCurso", DBNull.Value),
                new SqlParameter("@nombre", DBNull.Value),
                new SqlParameter("@descripcion", DBNull.Value),
                new SqlParameter("@idProfesor", DBNull.Value)
            ).ToListAsync();

            return Ok(cursos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> ObtenerCursoPorId(int id)
        {
            var curso = await _context.Set<Curso>().FromSqlRaw(
                "EXEC sp_Curso_CRUD @Operacion, @idCurso, @nombre, @descripcion, @idProfesor",
                new SqlParameter("@Operacion", "READ"),
                new SqlParameter("@idCurso", id),
                new SqlParameter("@nombre", DBNull.Value),
                new SqlParameter("@descripcion", DBNull.Value),
                new SqlParameter("@idProfesor", DBNull.Value)
            ).ToListAsync();

            if (curso == null)
                return NotFound();

            return Ok(curso);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCurso(int id, [FromBody] Curso curso)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Curso_CRUD @Operacion, @idCurso, @nombre, @descripcion, @idProfesor",
                new SqlParameter("@Operacion", "UPDATE"),
                new SqlParameter("@idCurso", id),
                new SqlParameter("@nombre", curso.nombre ?? (object)DBNull.Value),
                new SqlParameter("@descripcion", curso.descripcion ?? (object)DBNull.Value),
                new SqlParameter("@idProfesor", curso.idProfesor)
            );

            return Ok("Curso actualizado correctamente");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCurso(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Curso_CRUD @Operacion, @idCurso, @nombre, @descripcion, @idProfesor",
                new SqlParameter("@Operacion", "DELETE"),
                new SqlParameter("@idCurso", id),
                new SqlParameter("@nombre", DBNull.Value),
                new SqlParameter("@descripcion", DBNull.Value),

                new SqlParameter("@idProfesor", DBNull.Value)
            );

            return Ok("Curso eliminado correctamente");
        }
    }
}
