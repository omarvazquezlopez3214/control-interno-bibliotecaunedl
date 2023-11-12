using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class PrestamoRegistro
    {
        public int IdPrestamo { get; set; }
        public EstadoPrestamo oEstadoPrestamo { get; set; }
        public Persona oPersona { get; set; }
        public List<Libro> oLibro { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public string TextoFechaDevolucion { get; set; }
        public DateTime FechaConfirmacionDevolucion { get; set; }
        public string TextoFechaConfirmacionDevolucion { get; set; }
        public List<EstadoEntregado> EstadoEntregado { get; set; }
        public string EstadoRecibido { get; set; }
        public bool Estado { get; set; }
    }
}