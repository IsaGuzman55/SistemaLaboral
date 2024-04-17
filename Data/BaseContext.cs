using Microsoft.EntityFrameworkCore;
using SistemaLaboral.Models;


namespace SistemaLaboral.Data{
    public class BaseContext : DbContext{
        public BaseContext(DbContextOptions<BaseContext> options) : base(options){

        }

        public DbSet<Empleado> Empleados {get; set;}
        public DbSet<Historial> Historiales {get; set;}
    }
}