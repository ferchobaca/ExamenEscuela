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
    public class ProfesorController : ControllerBase
    {
        private EscuelaContext _context;

        public ProfesorController(EscuelaContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CrearProfesor([FromBody] Profesor profesor)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Profesor_CRUD @Operacion, @idProfesor, @nombre, @apellido",
                new SqlParameter("@Operacion", "CREATE"),
                new SqlParameter("@idProfesor", DBNull.Value),
                new SqlParameter("@nombre", profesor.nombre ?? (object)DBNull.Value),
                new SqlParameter("@apellido", profesor.apellido ?? (object)DBNull.Value)
            );

            return Ok("Profesor creado");
        }

        [HttpGet]
        public async Task<ActionResult<List<Profesor>>> ObtenerProfesores()
        {
            var profesores = await _context.Set<Profesor>()
                .FromSqlRaw("EXEC sp_Profesor_CRUD @Operacion, @idProfesor, @nombre, @apellido",
                    new SqlParameter("@Operacion", "READ"),
                    new SqlParameter("@idProfesor", DBNull.Value),
                    new SqlParameter("@nombre", DBNull.Value),
                    new SqlParameter("@apellido", DBNull.Value))
                .ToListAsync();

            return Ok(profesores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Profesor>> ObtenerProfesorPorId(int id)
        {
            var profesor = await _context.Set<Profesor>()
                .FromSqlRaw("EXEC sp_Profesor_CRUD @Operacion, @idProfesor, @nombre, @apellido",
                    new SqlParameter("@Operacion", "READ"),
                    new SqlParameter("@idProfesor", id),
                    new SqlParameter("@nombre", DBNull.Value),
                    new SqlParameter("@apellido", DBNull.Value))
                .ToListAsync();


            if (profesor == null)
                return NotFound();

            return Ok(profesor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProfesor(int id, [FromBody] Profesor profesor)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Profesor_CRUD @Operacion, @idProfesor, @nombre, @apellido",
                new SqlParameter("@Operacion", "UPDATE"),
                new SqlParameter("@idProfesor", id),
                new SqlParameter("@nombre", profesor.nombre ?? (object)DBNull.Value),
                new SqlParameter("@apellido", profesor.apellido ?? (object)DBNull.Value)
            );

            return Ok("Profesor actualizado");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProfesor(int id)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_Profesor_CRUD @Operacion, @idProfesor, @nombre, @apellido",
                new SqlParameter("@Operacion", "DELETE"),
                new SqlParameter("@idProfesor", id),
                new SqlParameter("@nombre", DBNull.Value),
                new SqlParameter("@apellido", DBNull.Value)
            );

            return Ok("Profesor eliminado");
        }
    }
}
