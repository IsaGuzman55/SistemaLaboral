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


        //LOGIN
        public IActionResult Login(){
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password){
            var Empleado = _context.Empleados.FirstOrDefault(e => e.Email == email && e.Password == password);
            if (Empleado!= null)
            {
                /* Empleado.EntryTime = DateTime.Now; */
                HttpContext.Session.SetString("Empleado", Empleado.Id.ToString());
                _context.Empleados.Update (Empleado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else {
                ViewBag.Error = "usuario o contraseña incorrectos";
                return View();
            }
        }

        public IActionResult Logout(){

            string id = HttpContext.Session.GetString("Empleado");
            var Empleado = _context.Empleados.FirstOrDefault(e => e.Id == Convert.ToInt32(id));
            /*  Empleado.ExitTime = DateTime.Now; */
            _context.Empleados.Update(Empleado);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }



    }
}