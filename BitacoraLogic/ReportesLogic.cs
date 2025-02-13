using System;
using System.Collections.Generic;
using System.Text;
using BitacoraModels;
using BitacoraData;
using System.Linq;
using System.IO;
using System.Globalization;

namespace BitacoraLogic
{
    public class ReportesLogic
    {
        ReportesData _ReportesData = new ReportesData();
        AvanceRealData _avanceRealData = new AvanceRealData();
        public static List<ReporteDistribucion> listaDistribucion = new List<ReporteDistribucion>();
        public static List<ReporteDetallado> listaDetallado = new List<ReporteDetallado>();
        public static List<ReportePersonas> listaPersonas= new List<ReportePersonas>();
        public static List<ReporteProyectos> listaProyectos = new List<ReporteProyectos>();
        public static List<ReporteSemanal> listaSemanal = new List<ReporteSemanal>();
        public static List<ReporteSemanal> listaEjecutivo = new List<ReporteSemanal>();
        ExportarExcelLogic _exportar = new ExportarExcelLogic();
        public static int idUnidad = 0;
        public static int idCliente = 0;
        public static int idUnidadUsuario = 0;
        public static int idArea = 0;
        public static int idUsuarioFiltro = 0;
   
        public static string unidad = "Todas";
        public static string area = "Todas";
        public static string usuario = "Todos";
        public static string proyectco = "Todos";
        public static string etapa = "Todas";
        public static string actividad = "Todas";
        public static DateTime fechaIni = DateTime.Today;        
        public static DateTime fechaFin = DateTime.Today;

        public List<ReporteDistribucion> ConsultaDistribucion(DatosReporte datos)
        {
            List<ReporteDistribucion> Distribucion= new List<ReporteDistribucion>();
            try
            {
                idUnidad = datos.IdUnidad;
                idArea = datos.IdArea;
                idCliente = datos.IdCliente;
                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);
                idUnidadUsuario = datos.IdUnidadUsuario;

                Distribucion = _ReportesData.ConsultaDistribucion(datos.IdUser, fechaIni, fechaFin, idUnidadUsuario, idCliente);
                                
                if(idUnidad > 0)
                    Distribucion = (from x in Distribucion where x.IdUnidad == idUnidad select x).ToList();
                
                    

                unidad = idUnidad < 1 ? "Todas" : Distribucion[0].Unidad;
                if (idArea > 0)
                {
                    var DistribucionArea = (from x in Distribucion where x.IdArea == idArea select x).ToList();
                    var Distribucionevento = (from x in Distribucion where x.IdProyecto == 0 select x).ToList();

                    Distribucion = DistribucionArea.Union(Distribucionevento).ToList();

                    area = DistribucionArea[0].Area;
                }
                else area = "Todas";

                listaDistribucion = Distribucion;
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return Distribucion;
        }
        public List<CatClientes> ConsultaClientesDistribucion(DatosReporte datos)
        {
            List<CatClientes> clientes = new List<CatClientes>();
            try
            {
                idUnidad = datos.IdUnidad;
                idArea = datos.IdArea;
                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);
                idUnidadUsuario = datos.IdUnidadUsuario;

                clientes = _ReportesData.ConsultaClientesDistribucion(datos.IdUser, fechaIni, fechaFin, idUnidadUsuario);
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return clientes;
        }
        public List<CatClientes> ConsultaClientesPorProyecto(DatosReporte datos)
        {
            List<CatClientes> clientes = new List<CatClientes>();
            try
            {
                idUnidad = datos.IdUnidad;
                idArea = datos.IdArea;
                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);

                clientes = _ReportesData.ConsultaClientesPorProyecto(datos.IdUser, fechaIni, fechaFin, idUnidad, idArea);
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return clientes;
        }

        public List<ReporteDetallado> ConsultaDetallado(DatosReporte datos)
        {
            List<ReporteDetallado> Detallado = new List<ReporteDetallado>();
            try
            {
                idUnidad = datos.IdUnidad;
                idArea = datos.IdArea;
                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);
                var iduser = datos.IdUserFiltro;
                var idProyecto = datos.IdProyecto;
                var idEtapa = datos.IdEtapa;
                var idActividad = datos.IdActividad;
                var idUnidadUsuario = datos.IdUnidadUsuario;
                var varDetalle = datos.varDetalle;

                Detallado =  _ReportesData.ConsultaDetallado(datos.IdUser, fechaIni, fechaFin);

                //  Agregar el filtro por Detalle si varDetalle tiene valor
                if (!string.IsNullOrEmpty(varDetalle))
                {
                    //Detallado = Detallado.Where(x => x.Detalle.Contains(varDetalle)).ToList();
                    varDetalle = varDetalle.ToLower();
                    Detallado = Detallado.Where(x =>
                        CultureInfo.CurrentCulture.CompareInfo
                            .IndexOf(x.Detalle.ToLower(), varDetalle, CompareOptions.IgnoreNonSpace) >= 0
                    ).ToList();
                }

                if (idUnidad > 0)
                    Detallado = (from x in Detallado where x.IdUnidad == idUnidad select x).ToList();
                
                unidad = idUnidad < 1 ? "Todas" : Detallado[0].Unidad;

                if (idArea > 0)
                {
                    var DetalladoArea = (from x in Detallado where x.IdArea == idArea select x).ToList();
                    var DetalladoEvento = (from x in Detallado where x.IdProyecto == 0 select x).ToList();

                    Detallado = DetalladoArea.Union(DetalladoEvento).ToList();

                    area = DetalladoArea[0].Area;
                }
                else area = "Todas";

                if (iduser > 0)
                {
                    Detallado = (from x in Detallado where x.IdUser == iduser select x).ToList();
                    usuario = Detallado[0].Usuario;
                }
                else usuario = "Todos";
                if (idProyecto > 0)
                {
                    Detallado = (from x in Detallado where x.IdProyecto == idProyecto select x).ToList();
                    proyectco = Detallado[0].Proyecto;
                }
                else proyectco = "Todos";
                if (idEtapa > 0)
                {
                    Detallado = (from x in Detallado where x.IdEtapa == idEtapa select x).ToList();
                    etapa = Detallado[0].Etapa;
                }
                else etapa = "Todas";

                if (idActividad > 0)
                {
                    Detallado = (from x in Detallado where x.IdActividad == idActividad select x).ToList();
                    actividad = Detallado[0].Actividad;
                }
                if (idUnidadUsuario > 0)
                {
                    Detallado = (from x in Detallado where x.IdUnidadUsuario == idUnidadUsuario select x).ToList();
                }
                else etapa = "Todas";

                listaDetallado = Detallado;
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return Detallado;
        }

        public List<ReportePersonas> ConsultaPersonas(DatosReporte datos)
        {
            List<ReportePersonas> Personas = new List<ReportePersonas>();
            try
            {
                idUnidad = datos.IdUnidad;
                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);               

                Personas = _ReportesData.ConsultaPersonas(datos.IdUser, fechaIni, fechaFin, datos.IdUserFiltro);

                if (idUnidad > 0)
                    Personas = (from x in Personas where x.IdUnidad == idUnidad orderby x.Horas select x).ToList();

                unidad = idUnidad < 1 ? "Todas" : Personas[0].Unidad;

                listaPersonas = Personas;
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return Personas;
        }
        public List<ReporteProyectos> ConsultaProyectos(DatosReporte datos)
        {
            List<ReporteProyectos> Proyectos = new List<ReporteProyectos>();
            try
            {
                idUnidad = datos.IdUnidad;
                idArea = datos.IdArea;
                idCliente = datos.IdCliente;
                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);

                Proyectos = _ReportesData.ConsultaProyectos(datos.IdUser, fechaIni, fechaFin, idCliente);

                if (idUnidad > 0)
                    Proyectos = (from x in Proyectos where x.IdUnidad == idUnidad  select x).ToList();

                unidad = idUnidad < 1 ? "Todas" : Proyectos[0].Unidad;
                if (idArea > 0)
                {
                    var ProyectosArea = (from x in Proyectos where x.IdArea == idArea select x).ToList();
                    var ProyectosEvento = (from x in Proyectos where x.IdProyecto == 0 select x).ToList();

                    Proyectos = ProyectosArea.Union(ProyectosEvento).ToList();

                    area = ProyectosArea[0].Area;
                }
                else area = "Todas";

                listaProyectos = Proyectos;
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return Proyectos;
        }

        public List<ReporteSemanal> ConsultaSemanal(DatosReporte datos)
        {
            List<ReporteSemanal> Semanal = new List<ReporteSemanal>();
            try
            {
                idUnidad = datos.IdUnidad;
                idArea = datos.IdArea;
                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);
                idUsuarioFiltro = datos.IdUserFiltro;

                Semanal = _ReportesData.ConsultaSemanal(datos.IdUser, fechaIni, fechaFin);

                if (idUnidad > 0)
                    Semanal = (from x in Semanal where x.IdUnidad == idUnidad select x).ToList();

                unidad = idUnidad < 1 ? "Todas" : Semanal[0].Unidad;
                if (idArea > 0 )
                {
                    var ProyectosArea = (from x in Semanal where x.IdArea == idArea select x).ToList();
                    var ProyectosEvento = (from x in Semanal where x.IdProyecto == 0 select x).ToList();

                    Semanal = ProyectosArea.Union(ProyectosEvento).ToList();

                    area = ProyectosArea[0].Area;
                }                
                else area = "Todas";

                if (idUsuarioFiltro > 0)
                {
                    var ProyectosArea = (from x in Semanal where x.IdUser == idUsuarioFiltro select x).ToList();
                    var ProyectosEvento = (from x in Semanal where x.IdUser == idUsuarioFiltro select x).ToList();

                    Semanal = ProyectosArea.Union(ProyectosEvento).ToList();
                }

                listaSemanal = Semanal;
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return Semanal;
        }

        public List<ReporteDetallado> ConsultaDetalladoUsuario(DatosReporte datos)
        {
            List<ReporteDetallado> detalle = new List<ReporteDetallado>();
            try
            {
                
                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);

                detalle = _ReportesData.ConsultaDetalladoUsuario(datos.IdUser, fechaIni, fechaFin);

                detalle = (from l in detalle orderby l.Fecha ascending select l).ToList();

            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return detalle;
        }

        public List<RepDetalleProyecto> ConsultaPersonas_RegistroPorProyecto(DatosReporte datos)
        {
            List<RepDetalleProyecto> detalle = new List<RepDetalleProyecto>();
            try
            {

                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);

                detalle = _ReportesData.ConsultaPersonas_RegistroPorProyecto(datos.IdProyecto, fechaIni, fechaFin);

            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return detalle;
        }

        public List<ListReporteDetalladoProyectoPersona> ConsultaPersonas_RegistroPorSemanaProyecto(DatosReporte datos)
        {
            List<ListReporteDetalladoProyectoPersona> response = new List<ListReporteDetalladoProyectoPersona>();
            try
            {

                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);

                List<Semanas> semanas = ConsultaSemanas(datos.FechaIni, datos.FechaFin, datos.IdProyecto);

                foreach (var semana in semanas)
                {
                    response.Add(new ListReporteDetalladoProyectoPersona
                    {
                        FechaInicioMoment = new DateTime(semana.FechaInicio.Year, semana.FechaInicio.Month, semana.FechaInicio.Day, 0, 0, 0),
                        FechaFinMoment = new DateTime(semana.FechaFin.Year, semana.FechaFin.Month, semana.FechaFin.Day, 0, 0, 0),
                        FechaInicioString = semana.FechaInicio.ToString("dd-MM-yyyy"),
                        FechaFinString = semana.FechaFin.ToString("dd-MM-yyyy"),
                        Proyecto = "Prueba",
                        FechaInicio = datos.FechaIni.ToString("dd/MM/yyyy"),
                        FechaFin = datos.FechaFin.ToString("dd/MM/yyyy"),
                        TotalHoras = semana.TotalHoras
                    });
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return response;
        }


        public List<Semanas> ConsultaSemanas(DateTime fechaInicio, DateTime fechaFin, int idProyecto)
        {
            List<Semanas> semanas = new List<Semanas>();
            DateTime _fechaInicio = fechaInicio;
            int delta = _fechaInicio.DayOfWeek - DayOfWeek.Sunday;
            DateTime _fechaFin = fechaInicio.AddDays((7 - delta));
            int NoSemanas = NumeroSemanasFechas(fechaInicio, fechaFin);
            _fechaInicio = new DateTime(_fechaInicio.Year, _fechaInicio.Month, _fechaInicio.Day, 0, 0, 0);
            _fechaFin = new DateTime(_fechaFin.Year, _fechaFin.Month, _fechaFin.Day, 0, 0, 0);
            for (int i = 0; i < NoSemanas; i++)
            {
                decimal totalHorasProyecto = _ReportesData.ConsultaHorasProyecto_Semana(_fechaInicio, _fechaFin, idProyecto);
                if (totalHorasProyecto > 0)
                {
                    semanas.Add(new Semanas
                    {
                        FechaInicio = _fechaInicio,
                        FechaFin = _fechaFin,
                        TotalHoras = totalHorasProyecto
                    });
                }
                _fechaInicio = _fechaFin.AddDays(1);
                _fechaFin = _fechaFin.AddDays(7);
            }

            return semanas;
        }

        public int NumeroSemanasFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            int NoSemanas = 0;
            var fecha = fechaFin - fechaInicio;
            NoSemanas = Convert.ToInt32(Math.Round(fecha.TotalDays / 7));
            return NoSemanas;
        }

        public List<ReporteDetallado> ConsultaDetallado_UsuarioPorProyecto(DatosReporte datos)
        {
            List<ReporteDetallado> detalle = new List<ReporteDetallado>();
            try
            {

                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);

                detalle = _ReportesData.ConsultaDetallado_UsuarioPorProyecto(datos.IdUser, datos.IdProyecto, fechaIni, fechaFin);


            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return detalle;
        }


        public List<ReporteSemanal> ConsultaEjecutivo(DatosReporte datos)
        {
            List<ReporteSemanal> Ejecutivo = new List<ReporteSemanal>();
            List<ReporteSemanal> EjecutivoFin = new List<ReporteSemanal>();
            try
            {
                idUnidad = datos.IdUnidad;
                idArea = datos.IdArea;
                fechaIni = Convert.ToDateTime(datos.FechaIni.ToString("dd/MM/yyyy"));
                fechaFin = Convert.ToDateTime(datos.FechaFin.ToString("dd/MM/yyyy")).AddDays(1);

                Ejecutivo = _ReportesData.ConsultaEjecutivo(datos.IdUser, fechaIni, fechaFin);

                if (idUnidad > 0)
                    Ejecutivo = (from x in Ejecutivo where x.IdUnidad == idUnidad select x).ToList();

                unidad = idUnidad < 1 ? "Todas" : Ejecutivo[0].Unidad;
                if (idArea > 0)
                {
                    var ProyectosArea = (from x in Ejecutivo where x.IdArea == idArea select x).ToList();
                    var ProyectosEvento = (from x in Ejecutivo where x.IdProyecto == 0 select x).ToList();

                    Ejecutivo = ProyectosArea.Union(ProyectosEvento).ToList();

                    area = ProyectosArea[0].Area;
                }
                else area = "Todas";

                var horasProyectos = (from x in Ejecutivo where x.IdProyecto != 0 select x.Horas).Sum();
                var horasProyectosInternos = (from x in Ejecutivo where x.IdProyecto != 0 && x.IdCategoria == 2 select x.Horas).Sum();
                var horasProyectosExternos = (from x in Ejecutivo where x.IdProyecto != 0 && x.IdCategoria == 1 select x.Horas).Sum();
                var horasProyectosEspeciales = (from x in Ejecutivo where x.IdProyecto != 0 && x.IdCategoria == 3 select x.Horas).Sum();

                var proyTotal = new ReporteSemanal();
                proyTotal.IdCategoria = 0;
                proyTotal.IdProyecto = 1;
                proyTotal.Proyecto = "Proyectos";
                proyTotal.Horas = horasProyectos;

                var proy = new ReporteSemanal();
                proy.IdCategoria = 2;
                proy.IdProyecto = 1;
                proy.Proyecto = "Proyecto Internos";
                proy.Horas = horasProyectosInternos;

                var proy2 = new ReporteSemanal();
                proy2.IdCategoria = 1;
                proy2.IdProyecto = 1;
                proy2.Proyecto = "Proyecto Externos";
                proy2.Horas = horasProyectosExternos;

                var proy3 = new ReporteSemanal();
                proy3.IdCategoria = 3;
                proy3.IdProyecto = 1;
                proy3.Proyecto = "Proyecto Especiales";
                proy3.Horas = horasProyectosEspeciales;

                EjecutivoFin.Add(proyTotal);
                EjecutivoFin.Add(proy);
                EjecutivoFin.Add(proy2);
                EjecutivoFin.Add(proy3);

                var Actividades = (from x in Ejecutivo where x.IdProyecto == 0 select x).ToList();
                var EjecutivoF = Actividades
                        .GroupBy(l => new { l.Proyecto })
                        .Select(x => new ReporteSemanal
                        {
                            IdProyecto = 2,
                            Proyecto = x.First().Proyecto,
                            Horas = x.Sum(d => d.Horas)
                        }).ToList();

                EjecutivoFin = EjecutivoFin.Union(EjecutivoF).ToList();
                var totalHoras = (from x in Ejecutivo select x.Horas).Sum();

                foreach (ReporteSemanal x in EjecutivoFin)
                {
                    var porc = x.Horas == 0 ? 0 : x.Horas * 100 / totalHoras;

                    x.Porcentaje = Decimal.Round(porc, 2);

                }

                EjecutivoF = (from l in EjecutivoF orderby l.IdProyecto, l.Proyecto select l).ToList();
                listaEjecutivo = EjecutivoFin;
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return EjecutivoFin;
        }


        public string ExportaReporte(string reporte)
        {
            string nombreArchivo = "";
            try
            {
                //var path = @"C:\Users\Admin\Documents\Bitacora\Reportes";
                var path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Reportes";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                path = path + @"\Reporte " + reporte + " " + fechaIni.ToString("dd-MM-yyyy") + " a " + fechaFin.ToString("dd-MM-yyyy") + ".xlsx";
                string encabezado = "R E P O R T E     NOMBRE     D E :      " + fechaIni.ToString(" dd - MM - yyyy  ") + "  A  " + fechaFin.ToString("  dd - MM - yyyy");
                string subEncabezado = "UNIDAD: " + unidad + "          -          AREA: " + area;

                switch(reporte)
                {
                    case "Distribucion":
                        encabezado = encabezado.Replace("NOMBRE", "D I S T R I B U C I O N ");
                        _exportar.ExportReporteDistribucion(path, listaDistribucion, encabezado, subEncabezado);
                        break;
                    case "Detallado":
                        encabezado = encabezado.Replace("NOMBRE", "D E T A L L A D O ");
                        subEncabezado = "UNIDAD: " + unidad + "     -     AREA: " + area + "     -     USUARIO: " + usuario + "     -     PROYECTO: " + proyectco + "     -     ETAPA: " + etapa;
                        _exportar.ExportReporteDetallado(path, listaDetallado, encabezado, subEncabezado);
                        break;
                    case "Personas":
                        encabezado = "REPORTE PERSONAS DE : " + fechaIni.ToString(" dd-MM-yyyy ") + " A " + fechaFin.ToString(" dd-MM-yyyy");
                        subEncabezado = " U N I D A D :   " + unidad;
                        _exportar.ExportReportePersonas(path, listaPersonas, encabezado, subEncabezado);
                        break;
                    case "Proyectos":
                        encabezado = encabezado.Replace("NOMBRE", "P R O Y E C T O S ");
                        _exportar.ExportReporteProyectos(path, listaProyectos, encabezado, subEncabezado);
                        break;
                    case "Semanal":
                        encabezado = encabezado.Replace("NOMBRE", "S E M A N A L ");
                        _exportar.ExportReporteSemanal(path, listaSemanal, encabezado, subEncabezado);
                        break;
                    case "Ejecutivo":
                        encabezado = encabezado.Replace("NOMBRE", "E J E C U T I V O ");
                        _exportar.ExportReporteEjecutivol(path, listaEjecutivo, encabezado, subEncabezado);
                        break;
                }
                nombreArchivo = "Reporte " + reporte + " " + fechaIni.ToString("dd-MM-yyyy") + " a " + fechaFin.ToString("dd-MM-yyyy") + ".xlsx";

            }
            catch (Exception e)
            {
                var error = e.Message;
                nombreArchivo = "-1";
            }

            return nombreArchivo;
        }

        public List<BItacoraInf> ConsultaListaUsuarios(int idUnidad)
        {
            List<BItacoraInf> ListaUsuarios = new List<BItacoraInf>();
            try
            {
                
               ListaUsuarios = idUnidad > 0 ? (from x in listaDetallado where x.IdUnidad == idUnidad select new BItacoraInf {IdUsr = x.IdUser, Usuario = x.Usuario }).ToList() : (from x in listaDetallado select new BItacoraInf { IdUsr = x.IdUser, Usuario = x.Usuario }).ToList();

                ListaUsuarios = (from x in ListaUsuarios orderby x.Usuario select x).Distinct(new CompareUserBitInf()).ToList();
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return ListaUsuarios;
        }
        public List<BItacoraInf> ConsultaListaUsuariosSemanal(int idUnidad)
        {
            List<BItacoraInf> ListaUsuarios = new List<BItacoraInf>();
            try
            {

                ListaUsuarios = idUnidad > 0 ? (from x in listaSemanal where x.IdUnidad == idUnidad select new BItacoraInf { IdUsr = x.IdUser, Usuario = x.Usuario }).ToList() : (from x in listaSemanal select new BItacoraInf { IdUsr = x.IdUser, Usuario = x.Usuario }).ToList();

                ListaUsuarios = (from x in ListaUsuarios orderby x.Usuario select x).Distinct(new CompareUserBitInf()).ToList();
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return ListaUsuarios;
        }
        public object ConsultaListaUsuariosPersona()
        {
            string result = "ok";
            List<BItacoraInf> ListaUsuarios = new List<BItacoraInf>();
            try
            {
                
                ListaUsuarios = (from x in listaPersonas select new BItacoraInf { IdUsr = x.IdUser, Usuario = x.Usuario }).ToList();

                ListaUsuarios = (from x in ListaUsuarios orderby x.Usuario select x).Distinct(new CompareUserBitInf()).ToList();
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
            var resp = new { result = result, Usuarios = ListaUsuarios };
            return resp;
        }
        public List<BItacoraInf> ConsultaListaProyectos(DatosReporte datos)
        {
            List<BItacoraInf> listaProyectos = new List<BItacoraInf>();
            List<ReporteDetallado> lista = listaDetallado;
            try
            {
                var idUnidad = datos.IdUnidad;
                var idArea = datos.IdArea;
                var idusuario = datos.IdUserFiltro;

                
                if (idUnidad > 0)
                    lista = (from x in lista where x.IdUnidad == idUnidad select x).ToList();
                if (idArea > 0)
                    lista = (from x in lista where x.IdArea == idArea select x).ToList();
                if (idusuario > 0)
                    lista = (from x in lista where x.IdUser == idusuario select x).ToList();

                listaProyectos = (from x in lista orderby x.Proyecto select new BItacoraInf { IdProyecto = x.IdProyecto, Proyecto = x.Proyecto }).ToList().Distinct(new CompareProyectoBitInf()).ToList();
              
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaProyectos;
        }

        public List<BItacoraInf> ConsultaListaActividades(DatosReporte datos)
        {
            List<BItacoraInf> listaActividades= new List<BItacoraInf>();
            List<ReporteDetallado> lista = listaDetallado;
            try
            {
                var idUnidad = datos.IdUnidad;
                var idArea = datos.IdArea;
                var idusuario = datos.IdUserFiltro;
                var idProyecto = datos.IdProyecto;
                
                if (idUnidad > 0)
                    lista = (from x in lista where x.IdUnidad == idUnidad select x).ToList();
                if (idArea > 0)
                    lista = (from x in lista where x.IdArea == idArea select x).ToList();
                if (idusuario > 0)
                    lista = (from x in lista where x.IdUser == idusuario select x).ToList();
                if (idProyecto > 0)
                    lista = (from x in lista where x.IdProyecto == idProyecto select x).ToList();

                listaActividades = (from x in lista orderby x.IdActividad select new BItacoraInf { IdActividad = x.IdActividad, Activadad = x.Actividad }).Distinct(new CompareActividadesBitInf()).ToList();
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaActividades;
        }

        public List<BItacoraInf> ConsultaListaEtapas(DatosReporte datos)
        {
            List<BItacoraInf> listaEtapas = new List<BItacoraInf>();
            List<ReporteDetallado> lista = listaDetallado;
            try
            {
                var idUnidad = datos.IdUnidad;
                var idArea = datos.IdArea;
                var idusuario = datos.IdUserFiltro;
                var idProyecto = datos.IdProyecto;
                var idActividad = datos.IdActividad;

               // listaEtapas = _ReportesData.ConsultaListaEtapas();

                if (idUnidad > 0)
                    lista = (from x in lista where x.IdUnidad == idUnidad select x).ToList();
                if (idArea > 0)
                    lista = (from x in lista where x.IdArea== idArea select x).ToList();
                if (idusuario > 0)
                    lista = (from x in lista where x.IdUser == idusuario select x).ToList();
                if (idProyecto > 0)
                    lista = (from x in lista where x.IdProyecto== idProyecto select x).ToList();
                if (idActividad > 0)
                    lista = (from x in lista where x.IdActividad == idActividad select x).ToList();

                listaEtapas = (from x in lista orderby etapa select new BItacoraInf { IdEtapa = x.IdEtapa, Etapa = x.Etapa }).Distinct(new CompareEtapasBitInf()).ToList();
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaEtapas;
        }

    }
}
