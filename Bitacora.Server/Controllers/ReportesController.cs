using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitacoraLogic;
using Microsoft.AspNetCore.Mvc;
using BitacoraModels;
using System.IO;
using Bitacora.Helpers;
using AutoMapper;
using Newtonsoft.Json;

namespace Bitacora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController
    {
        ReportesLogic _reportesLogic = new ReportesLogic();

        [HttpPost("[action]/{id}")]
        public object ConsultaDistribucion(DatosReporte datos)
        {
            var lista = _reportesLogic.ConsultaDistribucion(datos);
            var Distribucion = (from l in lista where l.IdProyecto > 0 select l).ToList();
            var ActividadesExt = (from l in lista where l.IdProyecto == 0 orderby l.IdUnidad, l.IdActividad ascending select l ).ToList();
            var Usuarios = (from x in lista orderby x.Usuario select x.Usuario).Distinct().ToList();
            var resp = new { result = "", Distribucion = Distribucion, ActividadesExt = ActividadesExt, Usuarios = Usuarios };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public string ExportaReporteDistribucion()
        {
            var resp = _reportesLogic.ExportaReporte("Distribucion");

            return resp;
        }

        [HttpPost("[action]")]
        public object ConsultaDetallado(DatosReporte datos, int? pageIndex, int? pageSize) 
        {
            int pageindex = (pageIndex??1);
            int pagesize = (pageSize ?? 10);

            PaginatedList<ReporteDetallado> listadetallado = PaginatedList<ReporteDetallado>.Create(_reportesLogic.ConsultaDetallado(datos), pageindex, pagesize);
            
            return new { 
                result = "", 
                Detallado = listadetallado, 
                PageList  = new
                {
                    CurrentPage = listadetallado.CurrentPage,
                    ItemsPerPage = listadetallado.ItemsPerPage,
                    TotalPages  = listadetallado.TotalPages,
                    TotalItems = listadetallado.TotalItems,
                    StartPage   = listadetallado.StartPage,
                    EndPage     = listadetallado.EndPage
                }
            };
        }

        [HttpGet("[action]/{id}")]
        public string ExportaReporteDetallado()
        {
            var resp = _reportesLogic.ExportaReporte("Detallado");

            return resp;
        }

        [HttpPost("[action]/{id}")]
        public object ConsultaPersonas(DatosReporte datos)
        {
            var listaPersonas = _reportesLogic.ConsultaPersonas(datos);
            listaPersonas = (from l in listaPersonas orderby l.Horas select l).ToList();
            var resp = new { result = "", Personas = listaPersonas };

            return resp;
        }

        [HttpPost("[action]/{id}")]
        public object ConsultaDetalleUsuario(DatosReporte datos)
        {
            var listaPersonas = _reportesLogic.ConsultaDetalladoUsuario(datos);
            var resp = new { result = "", lista = listaPersonas };

            return resp;
        }

        [HttpPost("[action]/{id}")]
        public object ConsultaPersonas_RegistroPorProyecto(DatosReporte datos)
        {
            var listaPersonas = _reportesLogic.ConsultaPersonas_RegistroPorProyecto(datos);
            var resp = new { result = "", lista = listaPersonas };

            return resp;
        }
        [HttpPost("[action]/{id}")]
        public object ConsultaPersonas_RegistroPorProyectoSemanal(DatosReporte datos)
        {
            var listaPersonas = _reportesLogic.ConsultaPersonas_RegistroPorSemanaProyecto(datos);
            var resp = new { result = "", lista = listaPersonas };

            return resp;
        }

        [HttpPost("[action]/{id}")]
        public object ConsultaDetallado_UsuarioPorProyecto(DatosReporte datos)
        {
            var listaRegistros = _reportesLogic.ConsultaDetallado_UsuarioPorProyecto(datos);
            var resp = new { result = "", lista = listaRegistros };

            return resp;
        }


        [HttpGet("[action]/{id}")]
        public string ExportaReportePersonas()
        {
            var resp = _reportesLogic.ExportaReporte("Personas");

            return resp;
        }

        [HttpPost("[action]/{id}")]
        public object ConsultaProyectos(DatosReporte datos)
        {
            var listaProyectos = _reportesLogic.ConsultaProyectos(datos);
            var Proyectos = (from l in listaProyectos where l.IdProyecto > 0 select l).ToList();
            var ActividadesExt = (from l in listaProyectos where l.IdProyecto == 0 orderby l.IdUnidad, l.IdActividad ascending select l).ToList();
            var resp = new { result = "", Proyectos = Proyectos, ActividadesExt = ActividadesExt};

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public string ExportaReporteProyectos()
        {
            var resp = _reportesLogic.ExportaReporte("Proyectos");

            return resp;
        }

        [HttpPost("[action]/{id}")]
        public object ConsultaSemanal(DatosReporte datos)
        {
            var listaSemanal = _reportesLogic.ConsultaSemanal(datos);
            var Semanal = (from l in listaSemanal where l.IdProyecto > 0 select l).ToList();
            var ActividadesExt = (from l in listaSemanal where l.IdProyecto == 0 orderby l.IdUnidad, l.Actividad ascending select l).ToList();
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var resp = new { result = "", Semanal = Semanal, ActividadesExt = ActividadesExt, direc = currentDirectory };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public string ExportaReporteSemanal()
        {
            var resp = _reportesLogic.ExportaReporte("Semanal");

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public List<BItacoraInf> ConsultaUsuarios(int idUnidad)
        {
            var resp = _reportesLogic.ConsultaListaUsuarios(idUnidad);

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public List<BItacoraInf> ConsultaUsuariosSemanal(int idUnidad)
        {
            var resp = _reportesLogic.ConsultaListaUsuariosSemanal(idUnidad);

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public object ConsultaUsuariosPersona()
        {
            var Usuarios = _reportesLogic.ConsultaListaUsuariosPersona();

            return Usuarios;
        }

        [HttpPost("[action]/{id}")]
        public List<BItacoraInf> ConsultaListaProyectos(DatosReporte datos)
        {
            var resp = _reportesLogic.ConsultaListaProyectos(datos);

            return resp;
        }

        [HttpPost("[action]/{id}")]
        public List<BItacoraInf> ConsultaActividades(DatosReporte datos)
        {
            var resp = _reportesLogic.ConsultaListaActividades(datos);

            return resp;
        }

        [HttpPost("[action]/{id}")]
        public List<BItacoraInf> ConsultaEtapas(DatosReporte datos)
        {
            var resp = _reportesLogic.ConsultaListaEtapas(datos);

            return resp;
        }

        
        [HttpGet("[action]/{fileName}")]
        public async Task<FileStreamResult> descarga(string fileName) { 
           
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            currentDirectory = currentDirectory + "\\Reportes";
            var file = Path.Combine(currentDirectory, fileName);
            var memory = new MemoryStream();
            //using (var stream = new FileStream(currentDirectory,FileMode.Open))
            //{
            //    await stream.CopyToAsync(memory);    
            //}
            memory.Position = 0;
            var stream = File.OpenRead(file);
            return new FileStreamResult(stream, "application/octet-stream");
            //return  File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",fileName);
            

        }

        [HttpPost("[action]/{id}")]
        public object ConsultaEjecutivo(DatosReporte datos)
        {
            List<ReporteSemanal> EjecutivoPrc = new List<ReporteSemanal>();

            var listaEjecutivo = _reportesLogic.ConsultaEjecutivo(datos);
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();

            var totalHoras = (from x in listaEjecutivo where x.IdCategoria == 0 select x.Horas).Sum();
            var proy = new ReporteSemanal();
            var proy2 = new ReporteSemanal();
            var proy3 = new ReporteSemanal();
            proy.IdProyecto = 1;
            proy.Proyecto = "Proyectos Internos";
            proy.Horas = (from x in listaEjecutivo where x.IdCategoria == 2 select x.Horas).Sum();
            proy.Porcentaje = proy.Horas == 0 ? 0 : Decimal.Round(proy.Horas * 100 / totalHoras, 0);
            EjecutivoPrc.Add(proy);

            proy2.IdProyecto = 1;
            proy2.Proyecto = "Proyectos Externos";
            proy2.Horas = (from x in listaEjecutivo where x.IdCategoria == 1 select x.Horas).Sum();
            proy2.Porcentaje = proy2.Horas == 0 ? 0 : Decimal.Round(proy2.Horas * 100 / totalHoras, 0);
            EjecutivoPrc.Add(proy2); 
            
            proy3.IdProyecto = 1;
            proy3.Proyecto = "Proyectos Especiales";
            proy3.Horas = (from x in listaEjecutivo where x.IdCategoria == 3 select x.Horas).Sum();
            proy3.Porcentaje = proy3.Horas == 0 ? 0 : Decimal.Round(proy3.Horas * 100 / totalHoras, 0);
            EjecutivoPrc.Add(proy3);

            proy = new ReporteSemanal();
            proy.IdProyecto = 2;
            proy.Proyecto = "Administrativas";
            proy.Horas = (from x in listaEjecutivo where x.IdProyecto != 1 select x.Horas).Sum();
            proy.Porcentaje = proy.Horas == 0 ? 0 : Decimal.Round(proy.Horas * 100 / totalHoras, 0);
            EjecutivoPrc.Add(proy);

            var resp = new 
            { 
                result = "", Ejecutivo = (from x in listaEjecutivo where x.IdCategoria == 0 select x).ToList(), 
                Total = totalHoras, 
                listaPorcentajes = EjecutivoPrc,
                direc = currentDirectory 
            };

            return resp;
        }

        [HttpGet("[action]/{id}")]
        public string ExportaReporteEjecutivo()
        {
            var resp = _reportesLogic.ExportaReporte("Ejecutivo");

            return resp;
        }

        [HttpPost("[action]/{id}")]
        public object ConsultaClientesDistribucion(DatosReporte datos)
        {
            var clientes = _reportesLogic.ConsultaClientesDistribucion(datos);

            var resp = new{ result = "", clientes = clientes};
            return resp;
        }

        [HttpPost("[action]/{id}")]
        public object ConsultaClientesPorProyecto(DatosReporte datos)
        {
            var clientes = _reportesLogic.ConsultaClientesPorProyecto(datos);

            var resp = new { result = "", clientes = clientes };
            return resp;
        }

    }


}
