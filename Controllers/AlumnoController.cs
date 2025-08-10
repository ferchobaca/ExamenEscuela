using EscuelaDB;
using EscuelaDB.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamenEscuela.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {
        private EscuelaContext _context;

        public AlumnoController(EscuelaContext context)
        {
            _context = context;
        }
        [HttpPost("CrearAlumno")]
        public async Task<IActionResult> CrearAlumno([FromBody] Alumnos alumno)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Alumnos_CRUD @Operacion, @idAlumno, @nombre, @apellido, @fechaNacimiento",
                new SqlParameter("@Operacion", "CREATE"),
                new SqlParameter("@idAlumno", DBNull.Value),
                new SqlParameter("@nombre", alumno.nombre ?? (object)DBNull.Value),
                new SqlParameter("@apellido", alumno.apellido ?? (object)DBNull.Value),
                new SqlParameter("@fechaNacimiento", alumno.fechaNacimiento )
            );

            return Ok("Alumno creado");
        }
        [HttpGet]
        public async Task<ActionResult<List<Alumnos>>> ObtenerAlumnos()
        {
            var alumnos = await _context.Set<Alumnos>()
                .FromSqlRaw("EXEC sp_Alumnos_CRUD @Operacion, @idAlumno, @nombre, @apellido, @fechaNacimiento",
                    new SqlParameter("@Operacion", "READ"),
                    new SqlParameter("@idAlumno", DBNull.Value),
                    new SqlParameter("@nombre", DBNull.Value),
                    new SqlParameter("@apellido", DBNull.Value),
                    new SqlParameter("@fechaNacimiento", DBNull.Value))
                .ToListAsync();

            return Ok(alumnos);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Alumnos>> ObtenerAlumnoPorId(int id)
        {
            var alumno = await _context.Set<Alumnos>()
                .FromSqlRaw("EXEC sp_Alumnos_CRUD @Operacion, @idAlumno, @nombre, @apellido, @fechaNacimiento",
                    new SqlParameter("@Operacion", "READ"),
                    new SqlParameter("@idAlumno", id),
                    new SqlParameter("@nombre", DBNull.Value),
                    new SqlParameter("@apellido", DBNull.Value),
                    new SqlParameter("@fechaNacimiento", DBNull.Value))
                .ToListAsync();

            if (alumno == null)
                return NotFound();

            return Ok(alumno);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarAlumno(int id, [FromBody] Alumnos alumno)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Alumnos_CRUD @Operacion, @idAlumno, @nombre, @apellido, @fechaNacimiento",
                new SqlParameter("@Operacion", "UPDATE"),
                new SqlParameter("@idAlumno", id),
                new SqlParameter("@nombre", alumno.nombre),
                new SqlParameter("@apellido", alumno.apellido),
                new SqlParameter("@fechaNacimiento", alumno.fechaNacimiento)
            );

            return Ok("Alumno actualizado");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAlumno(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Alumnos_CRUD @Operacion, @idAlumno, @nombre, @apellido, @fechaNacimiento",
                new SqlParameter("@Operacion", "DELETE"),
                new SqlParameter("@idAlumno", id),
                new SqlParameter("@nombre", DBNull.Value),
                new SqlParameter("@apellido", DBNull.Value),
                new SqlParameter("@fechaNacimiento", DBNull.Value)
            );

            return Ok("Alumno eliminado");
        }
    }
}
