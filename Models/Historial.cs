namespace SistemaLaboral.Models{
    public class Historial{
        public int Id { get; set; }
        public System.DateTime Entrada{get; set;}
        public System.DateTime Salida {get; set;}
        public int Empleado_Id {get; set;}
    }
}