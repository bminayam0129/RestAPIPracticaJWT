using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPIPractica.RestAPI.Data;
using RestAPIPractica.RestAPI.Models;

namespace RestAPIPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly RestAPIContext _context;

        public UsuariosController(RestAPIContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            // Verifica si hay usuarios disponibles
            var usuarios = await _context.Usuarios.ToListAsync();
            if (usuarios == null || !usuarios.Any())
            {
                return NotFound("No se encontraron usuarios.");
            }
            return Ok(usuarios); // Retorna un código 200 OK si existen usuarios.
        }

        // GET: api/usuarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado.");
            }
            return Ok(usuario); // Retorna 200 OK si se encuentra el usuario.
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // Validar correo electrónico duplicado
            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == usuario.Correo);

            if (usuarioExistente != null)
            {
                return BadRequest("El correo electrónico ya está en uso.");
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Devuelve una respuesta con código 201 Created
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT: api/usuarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest("El ID del usuario no coincide.");
            }

            // Verificar si el usuario existe antes de modificar
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado.");
            }

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // Devuelve 204 No Content, ya que la operación fue exitosa.
        }

        // DELETE: api/usuarios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado.");
            }

            _context.Usuarios.Remove(usuario);
            object value = await _context.SaveChangesAsync();

            return NoContent(); // Devuelve 204 No Content, indicando que se eliminó correctamente.
        }
    }
}
