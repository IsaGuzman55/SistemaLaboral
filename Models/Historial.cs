namespace SistemaLaboral.Models{
    public class Historial{
        public int Id { get; set; }
        public DateTime Entrada{get; set;}
        public DateTime? Salida {get; set;}
        public int Empleado_Id {get; set;}
    }
}