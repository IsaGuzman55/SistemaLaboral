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

        
        [HttpPost]
        public IActionResult GuardarEntrada(int empleadoId)
        {
            // Obtener la fecha y hora actual
            var fechaHoraActual = DateTime.Now;

            // Crear una instancia de Historial con los datos obtenidos
            var historial = new Historial
            {
                Entrada = fechaHoraActual,
                Empleado_Id = empleadoId,
            };

            // Agregar el historial al contexto de base de datos y guardar los cambios
            _context.Historiales.Add(historial);
            _context.SaveChanges();

            // Redirigir a la página deseada
            return RedirectToAction("Index", "Empleados");
        }


        [HttpPost]
        public IActionResult GuardarSalida(int empleadoId, DateTime salida)
        {
            // Buscar el historial correspondiente al EmpleadoId proporcionado y que aún no tenga una hora de salida registrada
            var historial = _context.Historiales.FirstOrDefault(h => h.Empleado_Id == empleadoId && h.Salida == null);

            if (historial != null)
            {
                // Actualizar solo la hora de salida
                historial.Salida = salida;
                _context.SaveChanges();
            }

            // Redirigir a donde desees después de guardar la salida
            return RedirectToAction("Index", "Empleados");
        }





              
    }
}