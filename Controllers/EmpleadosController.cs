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
            string nombre = HttpContext.Session.GetString("EmpleadoName");
            ViewBag.Nombre = nombre;
            return View(await _context.Empleados.ToListAsync());
        }
        [HttpPost]
        public IActionResult Index(int? id, Empleado empleado)
        {
            _context.Empleados.Update(empleado);
            _context.SaveChanges();
            
            // Crear un objeto Historial con los datos necesarios
            var historial = new Historial
            {
                Empleado_Id = empleado.Id,
                Entrada = DateTime.Now // Puedes establecer la fecha y hora de entrada como la fecha y hora actual
            };

            return RedirectToAction("Index");
        }
    

        // SALIDA DEL EMPLEADO
       [HttpPost]
        public IActionResult SalidaHorario(int? id, Empleado empleado)
        {
            _context.Empleados.Update(empleado);
            _context.SaveChanges();
            // Retornar la vista con el ViewModel
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SalidaHorario(int? id){
            // Obtener el empleado correspondiente al ID proporcionado
            return View (await _context.Empleados.FirstOrDefaultAsync(m => m.Id == id));
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
                /* Empleado.EntryTime = DateTime.Now; */
                HttpContext.Session.SetString("EmpleadoId", Empleado.Id.ToString());
                HttpContext.Session.SetString("EmpleadoName", Empleado.Names);
                _context.Empleados.Update(Empleado);
                _context.SaveChanges();

                string id = HttpContext.Session.GetString("EmpleadoId");
                 Console.WriteLine($"El ID:{id}");
                return RedirectToAction("Index");

            }
            else {
                ViewBag.Error = "usuario o contraseña incorrectos";
                return View();
            }

        }

        public IActionResult Logout(){

            string id = HttpContext.Session.GetString("EmpleadoId");
            var Empleado = _context.Empleados.FirstOrDefault(e => e.Id == Convert.ToInt32(id));
            /*  Empleado.ExitTime = DateTime.Now; */
            /* _context.Empleados.Update(Empleado); */
           /*  _context.SaveChanges(); */
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Eliminar(int id){
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction("Eliminar");
        }


    }
}