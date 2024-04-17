using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLaboral.Data;
using SistemaLaboral.Models;

namespace SistemaLaboral.Controllers{
    public class EmpleadosController : Controller {
       public readonly BaseContext _context;

       public EmpleadosController(BaseContext context){
        _context = context;
       }
       


        public async Task<IActionResult> Index(){
            return View(await _context.Empleados.ToListAsync());
        }
        
      
        
        public async Task<IActionResult> Login(string email, string password)
        {
            // Buscar el usuario por correo electrónico y contraseña en la base de datos
            var empleado = await _context.Empleados.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (empleado != null)
            {
                // Autenticación exitosa
                return RedirectToAction("Index");
            }

            // Si no se encuentra el usuario en la base de datos, la autenticación falla
            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View();
        }


        public async Task<IActionResult> EntradaHorario(int? id){
            // Obtener el empleado correspondiente al ID proporcionado
            return View (await _context.Empleados.FirstOrDefaultAsync(m => m.Id == id));
        }


       [HttpPost]
        public IActionResult EntradaHorario(int? id, Empleado empleado)
        {
            _context.Empleados.Update(empleado);
            _context.SaveChanges();
            // Crear un objeto Historial con los datos necesarios
            var historial = new Historial
            {
                Empleado_Id = empleado.Id,
                Entrada = DateTime.Now // Puedes establecer la fecha y hora de entrada como la fecha y hora actual
            };

            // Retornar la vista con el ViewModel
            return RedirectToAction("Index");
        }

        
        // SALIDA DEL EMPLEADO
        public async Task<IActionResult> SalidaHorario(int? id){
            // Obtener el empleado correspondiente al ID proporcionado
            return View (await _context.Empleados.FirstOrDefaultAsync(m => m.Id == id));
        }


       [HttpPost]
        public IActionResult SalidaHorario(int? id, Empleado empleado)
        {
            _context.Empleados.Update(empleado);
            _context.SaveChanges();
            // Retornar la vista con el ViewModel
            return RedirectToAction("Index");
        }



    }
}