using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoBiblioteca.Controllers
{
    public class PrestamoController : Controller
    {
        // GET: Prestamo
        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Consultar()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GuardarPrestamos(PrestamoRegistro objeto)
        {
            bool _respuesta = false;
            string _mensaje = string.Empty;
            int prestamosPendientes = 0;
            int numMaxPrestamos = 3;
            int numPrestamosPermitidos = 0;
            prestamosPendientes = PrestamoLogica.Instancia.CountPrestamosPendientes(objeto.oPersona.IdPersona);
            numPrestamosPermitidos = numMaxPrestamos - prestamosPendientes;
            List<Libro> listaBorrar = new List<Libro>();
            foreach (var libro in objeto.oLibro)
            {
                if(libro.IdLibro == 0)
                {
                    listaBorrar.Add(libro);
                }
            }
            foreach (var libro2 in listaBorrar)
            {
                objeto.oLibro.Remove(libro2);
            }
            if (prestamosPendientes == numMaxPrestamos)
            {
                _respuesta = false;
                _mensaje = string.Format("El lector ya tiene {0} préstamos pendientes favor de verificar", prestamosPendientes);
            }
            else if (objeto.oLibro.Count <= numPrestamosPermitidos)
            {
                for (int i = 0; i < objeto.oLibro.Count; i++)
                {
                    if (objeto.oLibro[i].IdLibro != 0)
                    {
                        Prestamo prestamo = new Prestamo()
                        {
                            IdPrestamo = objeto.IdPrestamo,
                            oEstadoPrestamo = objeto.oEstadoPrestamo,
                            oPersona = objeto.oPersona,
                            oLibro = objeto.oLibro[i],
                            FechaDevolucion = objeto.FechaDevolucion,
                            TextoFechaDevolucion = objeto.TextoFechaDevolucion,
                            FechaConfirmacionDevolucion = objeto.FechaConfirmacionDevolucion,
                            TextoFechaConfirmacionDevolucion = objeto.TextoFechaConfirmacionDevolucion,
                            EstadoEntregado = objeto.EstadoEntregado[i].TextoEstadoEntregado,
                            EstadoRecibido = objeto.EstadoRecibido,
                            Estado = objeto.Estado
                        };
                        _respuesta = PrestamoLogica.Instancia.Registrar(prestamo);
                        if (_respuesta)
                        {
                            _respuesta = PrestamoLogica.Instancia.ActualizarEstadoLibro(objeto.oLibro[i].IdLibro, false);
                        }
                        _mensaje = _respuesta ? string.Format("Registro completo") : "No se pudo registrar";
                    }
                }
            }
            else
            {
                _respuesta = false;
                _mensaje = string.Format("El lector solo tiene {0} préstamo(s) permitido(s) ya cuenta con {1} préstamos pendientes", numPrestamosPermitidos, prestamosPendientes);
            }
            return Json(new { resultado = _respuesta, mensaje = _mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarEstados()
        {
            List<EstadoPrestamo> oLista = new List<EstadoPrestamo>();
            oLista = PrestamoLogica.Instancia.ListarEstados();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Listar(int idestadoprestamo, int idpersona)
        {
            List<Prestamo> oLista = new List<Prestamo>();
            oLista = PrestamoLogica.Instancia.Listar(idestadoprestamo, idpersona);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Devolver(string estadorecibido,int idprestamo)
        {
            bool respuesta = false;
            respuesta = PrestamoLogica.Instancia.Devolver(estadorecibido,idprestamo);
            if(respuesta)
            {
                respuesta = PrestamoLogica.Instancia.ActualizarEstadoIdLibroPrestamo(idprestamo, true);
            }
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}