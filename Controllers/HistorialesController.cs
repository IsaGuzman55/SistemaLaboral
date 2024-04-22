using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLaboral.Data;
using SistemaLaboral.Models;

namespace SistemaLaboral.Controllers{
    public class HistorialesController : Controller {
        public readonly BaseContext _context;
       public HistorialesController(BaseContext context){
        _context = context;
       }

/* GUARDAR REGISTRO DE LA HORA DE ENTRADA EN LA TABLA HISTORIAL */
        [HttpPost]
        public IActionResult GuardarEntrada()
        {
            // Obtener la fecha y hora actual y el id del empleado que inicio sesion
            var fechaHoraActual = DateTime.Now;
            var empleadoId = HttpContext.Session.GetString("EmpleadoId");

            // Crear una instancia de Historial con los datos obtenidos
            Historial historial = new()
            {
                Entrada = fechaHoraActual,
                Empleado_Id = Convert.ToInt32(empleadoId),
                Salida = null
            };

            // Agregar el historial al contexto de base de datos y guardar los cambios
            _context.Historiales.Add(historial);
            _context.SaveChanges();

            // Guardar en la session el id del historial que se creo al guardar la hora de entrada
            HttpContext.Session.SetString("HistorialId", historial.Id.ToString());
            HttpContext.Session.SetString("HistorialEntrada", historial.Entrada.ToString());


            // Al oprimir el boton de registrar ENTRADA crear un TemData de confirmacion
            if (Request.Form["IngresoButton"] == "Ingreso"){
                // El botón "Ingreso" se ha presionado
                TempData["MensajeConfirmacion"]= $"Tu hora de entrada fue a las: {fechaHoraActual}";
                return RedirectToAction("Index", "Empleados");
            }

            // Redirigir a la página deseada
            return RedirectToAction("Index", "Empleados");
        }


/* GUARDAR REGISTRO DE LA HORA DE SALIDA EN LA TABLA HISTORIAL */
        [HttpPost]
        public IActionResult GuardarSalida()
        {
            // Obtener el Id que se creo al almacenar la hora de entrada y crear una variable con la fecha del momento
            var historialId = HttpContext.Session.GetString("HistorialId");
            var fechaHoraActual = DateTime.Now;

            // Si la session tiene un Id del historial...
            if(historialId != null){
                // Encontrar en la tabla "Historiales" si el id tenemos coincide con alguno de la tabla 
                var historial = _context.Historiales.FirstOrDefault(h => h.Id == Convert.ToInt32(historialId));

                // Si encontramos el id coincidente en la tabla, que la hora de salida de ese id sea igual a la hora actual
                if(historial != null){
                    historial.Salida = fechaHoraActual;
                    _context.SaveChanges();

                    // Guardar la hora de salida en la session para en el Index hacer la validacion del botón.
                    HttpContext.Session.SetString("HoraSalida", historial.Salida.ToString());
                    // Al oprimir el boton de registrar HORA DE SALIDA crear un TemData de confirmacion
                    if(Request.Form["SalidaButton"] == "Salida"){
                        // El botón "Ingreso" se ha presionado
                        TempData["MensajeIdHistorial"]= $"Tu de hora salida fue a las: {fechaHoraActual}";
                        return RedirectToAction("Index", "Empleados");
                    }
                }
            }
            // Redirigir al Index
            return RedirectToAction("Index", "Empleados");
        }


/* VER HISTORIAL DE UN EMPLEADO DETERMINADO SEGUN EL QUE SE HAYA ELEGIDO EN LA TABLA */
        public async Task<IActionResult> ListarHistorial(int? id){ 
            // Obtener el Id del empleado que inicio sesion
            var empleadoId = HttpContext.Session.GetString("EmpleadoId");

            // Si en la sesion NO hay un id de un empleado registrado
            if (empleadoId == null)
            {
                // El usuario no ha iniciado sesión, redirigir al login
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                // Encontrar el id del empleado al que se le quiere ver el historial
                var empleado = await _context.Empleados.FirstOrDefaultAsync(m => m.Id == id);

                // Filtrar los registros del historial donde el id del empleado coincida con los empleado_Id registrados
                var historialEmpleado = await _context.Historiales.Where(h => h.Empleado_Id == empleado.Id).ToListAsync();

                ViewBag.Nombres = $"Historial de {empleado.Names} {empleado.LastNames}";
                // Listar los registros que coincidieron
                return View(historialEmpleado);
            }
        }
       

    }
}