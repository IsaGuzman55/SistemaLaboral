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


/* LOGIN O INICIAR SESION */
        public IActionResult Login(){
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password){
            var Empleado = _context.Empleados.FirstOrDefault(e => e.Email == email && e.Password == password);
            if (Empleado!= null)
            {
                var ultimoRegistro = _context.Historiales.FirstOrDefault(x => x.Empleado_Id == Empleado.Id && x.Salida == null);
                HttpContext.Session.SetString("EmpleadoId", Empleado.Id.ToString());
                HttpContext.Session.SetString("EmpleadoName", Empleado.Names);
                HttpContext.Session.SetString("EmpleadoApellidos", Empleado.LastNames);
                if(ultimoRegistro != null){
                    HttpContext.Session.SetString("HistorialId", ultimoRegistro.Id.ToString());
                }
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            else {
                ViewBag.Error = "¡Correo o contraseña incorrectos!";
                return View();
            }
        }


/* LISTADO DE LOS EMPLEADOS */
        public async Task<IActionResult> Index(){
            string nombre = HttpContext.Session.GetString("EmpleadoName");
            string apellidos = HttpContext.Session.GetString("EmpleadoApellidos");
            var EmployeId = HttpContext.Session.GetString("EmpleadoId");
            ViewBag.Nombre = $"{nombre} {apellidos}";
            ViewBag.VerificacionEntrada = $"{nombre}, ¡Ya registraste tu hora de entrada!";
            ViewBag.VerificacionSalida = $"{nombre}, ¡Ya registraste tu hora de salida!";

            if(EmployeId != null){
                return View(await _context.Empleados.ToListAsync());

            }
            else{
                return RedirectToAction("Index", "Historiales");
            }
        }


/* CERRAR SESION */
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");            
        }


/* ELIMINAR EMPLEADOS */
        public async Task<IActionResult> Delete(int id){
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


/* REGISTRAR EMPLEADOS */
        [HttpGet]
        public IActionResult Register(){
            return View();
        }

       [HttpPost]
        public IActionResult Register(Empleado r){
            _context.Empleados.Add(r);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }


/* CREAR EMPLEADOS CUANDO YA SE INICIO SESION */
        [HttpGet]
        public IActionResult Create(){
            return View();
        }

       [HttpPost]
        public IActionResult Create(Empleado r){
            _context.Empleados.Add(r);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

/* BUSCADOR DE EMPLEADOS */
       public IActionResult Search(string search){
        var empleados =  _context.Empleados.AsQueryable();
            if (!string.IsNullOrEmpty(search)){
                empleados = empleados.Where(e => e.Names.Contains(search) || e.LastNames.Contains(search) || e.Email.Contains(search) );
            }
            return View("Index", empleados.ToList());
       }

    }
}