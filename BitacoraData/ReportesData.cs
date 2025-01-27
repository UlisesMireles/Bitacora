using System;
using System.Collections.Generic;
using System.Text;
using BitacoraModels;
using System.Linq;

namespace BitacoraData
{
    public class ReportesData
    {
        public List<ReporteDistribucion> ConsultaDistribucion(int idUser, DateTime fechaInicio, DateTime fechaFin, int idUnidadUsuario, int idCliente)
        {
            List<ReporteDistribucion> listaDistribucion = new List<ReporteDistribucion>();
            List<ReporteDistribucion> listaActividades = new List<ReporteDistribucion>();
            List<ReporteDistribucion> listaDistribucionAc = new List<ReporteDistribucion>();

            try
            {

                using (BitacoraContext db = new BitacoraContext())
                {
                    if (idUnidadUsuario > 0)
                    {
                        var lista = (from b in db.BitacoraH
                                     join u in db.CatUsuarios on b.IdUser equals u.Id
                                     join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                     join c in db.CatClientes on r.IdCliente equals c.Id
                                     join p in db.CatProyectos on r.IdProyecto equals p.Id
                                     join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                     join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                     join rue in db.RelacionUsuarioEmpleado on u.Id equals rue.IdUser
                                     join e in db.CatEmpleados on rue.IdEmpleado equals e.Id
                                     where r.IdArea == rua.IdArea && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin && e.IdUnidad == idUnidadUsuario 
                                     select new ReporteDistribucion
                                     {
                                         IdCliente = c.Id,
                                         Clinete = c.Nombre,
                                         IdProyecto = p.Id,
                                         Proyecto = p.Nombre,
                                         IdUser = u.Id,
                                         Usuario = u.Usuario.Replace(".", " "),
                                         Horas = b.Duracion,
                                         IdUnidad = r.IdUnidad,
                                         Unidad = un.Nombre,
                                         IdArea = r.IdArea == null ? 0 : r.IdArea,
                                         Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                         IdUnidadUsuario = e.IdUnidad == null ? 0 : e.IdUnidad 
                                     }).ToList();

                        listaDistribucion = lista
                           .GroupBy(l => new { l.IdProyecto, l.IdUser })
                           .Select(x => new ReporteDistribucion
                           {
                               IdCliente = x.First().IdCliente,
                               Clinete = x.First().Clinete,
                               IdProyecto = x.First().IdProyecto,
                               Proyecto = x.First().Proyecto,
                               IdUser = x.First().IdUser,
                               Usuario = x.First().Usuario,
                               Horas = x.Sum(d => d.Horas),
                               IdUnidad = x.First().IdUnidad,
                               Unidad = x.First().Unidad,
                               IdArea = x.First().IdArea,
                               Area = x.First().Area,
                               IdUnidadUsuario = x.First().IdUnidadUsuario

                           }).ToList();

                        listaActividades = (from b in db.BitacoraH
                                            join u in db.CatUsuarios on b.IdUser equals u.Id
                                            join r in db.RelacionUsuarioEmpleado on b.IdUser equals r.IdUser
                                            join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                            join un in db.CatUnidadesNegocios on e.IdUnidad equals un.Id
                                            join a in db.CatActividades on b.IdActividad equals a.Id
                                            join rua in db.RelacionUsuarioUnidadArea on e.IdUnidad equals rua.IdUnidad
                                            where b.IdProyecto == null && a.Evento == "1" && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin && e.IdUnidad == idUnidadUsuario
                                            select new ReporteDistribucion
                                            {
                                                Id = b.Id,
                                                IdActividad = a.Id,
                                                Actividad = a.Nombre,
                                                IdUser = u.Id,
                                                Usuario = u.Usuario.Replace(".", " "),
                                                Horas = b.Duracion,
                                                IdUnidad = e.IdUnidad,
                                                Unidad = un.Nombre,
                                            }).Distinct().ToList();

                        listaDistribucionAc = listaActividades
                             .GroupBy(l => new { l.IdActividad, l.IdUnidad, l.IdUser })
                             .Select(x => new ReporteDistribucion
                             {
                                 IdActividad = x.First().IdActividad,
                                 Actividad = x.First().Actividad,
                                 IdUser = x.First().IdUser,
                                 Usuario = x.First().Usuario,
                                 Horas = x.Sum(d => d.Horas),
                                 IdUnidad = x.First().IdUnidad,
                                 Unidad = x.First().Unidad
                             }).Distinct().ToList();
                    }
                    else
                    {
                        var lista = (from b in db.BitacoraH
                                     join u in db.CatUsuarios on b.IdUser equals u.Id
                                     join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                     join c in db.CatClientes on r.IdCliente equals c.Id
                                     join p in db.CatProyectos on r.IdProyecto equals p.Id
                                     join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                     join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                     join rue in db.RelacionUsuarioEmpleado on u.Id equals rue.IdUser
                                     join e in db.CatEmpleados on rue.IdEmpleado equals e.Id
                                     where r.IdArea == rua.IdArea && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                     select new ReporteDistribucion
                                     {
                                         IdCliente = c.Id,
                                         Clinete = c.Nombre,
                                         IdProyecto = p.Id,
                                         Proyecto = p.Nombre,
                                         IdUser = u.Id,
                                         Usuario = u.Usuario.Replace(".", " "),
                                         Horas = b.Duracion,
                                         IdUnidad = r.IdUnidad,
                                         Unidad = un.Nombre,
                                         IdArea = r.IdArea == null ? 0 : r.IdArea,
                                         Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                         IdUnidadUsuario = e.IdUnidad == null ? 0 : e.IdUnidad
                                     }).ToList();

                        listaDistribucion = lista
                           .GroupBy(l => new { l.IdProyecto, l.IdUser })
                           .Select(x => new ReporteDistribucion
                           {
                               IdCliente = x.First().IdCliente,
                               Clinete = x.First().Clinete,
                               IdProyecto = x.First().IdProyecto,
                               Proyecto = x.First().Proyecto,
                               IdUser = x.First().IdUser,
                               Usuario = x.First().Usuario,
                               Horas = x.Sum(d => d.Horas),
                               IdUnidad = x.First().IdUnidad,
                               Unidad = x.First().Unidad,
                               IdArea = x.First().IdArea,
                               Area = x.First().Area,
                               IdUnidadUsuario = x.First().IdUnidadUsuario

                           }).ToList();


                        listaActividades = (from b in db.BitacoraH
                                                join u in db.CatUsuarios on b.IdUser equals u.Id
                                                join r in db.RelacionUsuarioEmpleado on b.IdUser equals r.IdUser
                                                join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                                join un in db.CatUnidadesNegocios on e.IdUnidad equals un.Id
                                                join a in db.CatActividades on b.IdActividad equals a.Id
                                                join rua in db.RelacionUsuarioUnidadArea on e.IdUnidad equals rua.IdUnidad
                                                where b.IdProyecto == null && a.Evento == "1" && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                                select new ReporteDistribucion
                                                {
                                                    Id = b.Id,
                                                    IdActividad = a.Id,
                                                    Actividad = a.Nombre,
                                                    IdUser = u.Id,
                                                    Usuario = u.Usuario.Replace(".", " "),
                                                    Horas = b.Duracion,
                                                    IdUnidad = e.IdUnidad,
                                                    Unidad = un.Nombre,
                                                }).Distinct().ToList();

                        listaDistribucionAc = listaActividades
                             .GroupBy(l => new { l.IdActividad, l.IdUnidad, l.IdUser })
                             .Select(x => new ReporteDistribucion
                             {
                                 IdActividad = x.First().IdActividad,
                                 Actividad = x.First().Actividad,
                                 IdUser = x.First().IdUser,
                                 Usuario = x.First().Usuario,
                                 Horas = x.Sum(d => d.Horas),
                                 IdUnidad = x.First().IdUnidad,
                                 Unidad = x.First().Unidad
                             }).Distinct().ToList();
                    }


                    listaDistribucion = listaDistribucion.Union(listaDistribucionAc).ToList();

                    listaDistribucion = (from l in listaDistribucion orderby l.IdUnidad, l.IdArea, l.Clinete, l.Proyecto, l.Actividad, l.Usuario select l).ToList();

                    if(idCliente > 0)
                    {
                        listaDistribucion = (from l in listaDistribucion where l.IdCliente == idCliente orderby l.IdUnidad, l.IdArea, l.Clinete, l.Proyecto, l.Actividad, l.Usuario select l).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
            return listaDistribucion;
        }
        public List<CatClientes> ConsultaClientesDistribucion(int idUser, DateTime fechaInicio, DateTime fechaFin, int idUnidadUsuario)
        {
            List<CatClientes> listaClientes = new List<CatClientes>();

            try
            {

                using (BitacoraContext db = new BitacoraContext())
                {
                    if (idUnidadUsuario > 0)
                    {
                        var lista = (from b in db.BitacoraH
                                     join u in db.CatUsuarios on b.IdUser equals u.Id
                                     join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                     join c in db.CatClientes on r.IdCliente equals c.Id
                                     join p in db.CatProyectos on r.IdProyecto equals p.Id
                                     join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                     join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                     join rue in db.RelacionUsuarioEmpleado on u.Id equals rue.IdUser
                                     join e in db.CatEmpleados on rue.IdEmpleado equals e.Id
                                     where r.IdArea == rua.IdArea && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin && e.IdUnidad == idUnidadUsuario
                                     select new ReporteDistribucion
                                     {
                                         IdCliente = c.Id,
                                         Clinete = c.Nombre,
                                         IdProyecto = p.Id,
                                         Proyecto = p.Nombre,
                                         IdUser = u.Id,
                                         Usuario = u.Usuario.Replace(".", " "),
                                         Horas = b.Duracion,
                                         IdUnidad = r.IdUnidad,
                                         Unidad = un.Nombre,
                                         IdArea = r.IdArea == null ? 0 : r.IdArea,
                                         Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                         IdUnidadUsuario = e.IdUnidad == null ? 0 : e.IdUnidad
                                     }).ToList();

                        listaClientes = lista
                           .GroupBy(l => new { l.IdCliente })
                           .Select(x => new CatClientes
                           {
                               Id = x.First().IdCliente,
                               Nombre = x.First().Clinete,

                           }).ToList();

                    }
                    else
                    {
                        var lista = (from b in db.BitacoraH
                                     join u in db.CatUsuarios on b.IdUser equals u.Id
                                     join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                     join c in db.CatClientes on r.IdCliente equals c.Id
                                     join p in db.CatProyectos on r.IdProyecto equals p.Id
                                     join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                     join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                     join rue in db.RelacionUsuarioEmpleado on u.Id equals rue.IdUser
                                     join e in db.CatEmpleados on rue.IdEmpleado equals e.Id
                                     where r.IdArea == rua.IdArea && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                     select new ReporteDistribucion
                                     {
                                         IdCliente = c.Id,
                                         Clinete = c.Nombre,
                                         IdProyecto = p.Id,
                                         Proyecto = p.Nombre,
                                         IdUser = u.Id,
                                         Usuario = u.Usuario.Replace(".", " "),
                                         Horas = b.Duracion,
                                         IdUnidad = r.IdUnidad,
                                         Unidad = un.Nombre,
                                         IdArea = r.IdArea == null ? 0 : r.IdArea,
                                         Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                         IdUnidadUsuario = e.IdUnidad == null ? 0 : e.IdUnidad
                                     }).ToList();

                        listaClientes = lista
                           .GroupBy(l => new { l.IdCliente })
                           .Select(x => new CatClientes
                           {
                               Id = x.First().IdCliente,
                               Nombre = x.First().Clinete,

                           }).ToList();
                    }



                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
            return listaClientes;
        }

        public List<ReporteDetallado> ConsultaDetallado(int idUser, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteDetallado> listaDetallado = new List<ReporteDetallado>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaDetallado = (from b in db.BitacoraH
                                      join u in db.CatUsuarios on b.IdUser equals u.Id
                                      join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                      join p in db.CatProyectos on r.IdProyecto equals p.Id
                                      join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                      join e in db.CatEtapas on b.IdEtapa equals e.Id
                                      join a in db.CatActividades on b.IdActividad equals a.Id
                                      join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                      join rue in db.RelacionUsuarioEmpleado on u.Id equals rue.IdUser
                                      join em in db.CatEmpleados on rue.IdEmpleado equals em.Id
                                      where r.IdArea == rua.IdArea && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                      select new ReporteDetallado
                                      {
                                          Id = b.Id,
                                          Fecha = b.Fecha,
                                          Horas = b.Duracion,
                                          IdUser = u.Id,
                                          Usuario = u.Usuario.Replace(".", " "),
                                          IdProyecto = p.Id,
                                          Proyecto = p.Nombre,
                                          IdUnidad = un.Id,
                                          Unidad = un.Nombre,
                                          IdArea = r.IdArea == null ? 0 : r.IdArea,
                                          Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                          Detalle = b.Descripcion,
                                          IdEtapa = e.Id,
                                          Etapa = e.Nombre,
                                          IdActividad = a.Id,
                                          Actividad = a.Nombre,
                                          FechaRegistro = b.FechaRegistro,
                                          IdUnidadUsuario = em.IdUnidad == null ? 0 : em.IdUnidad,
                                          UnidadUsuario = em.IdUnidad == null ? "" : (from cu in db.CatUnidadesNegocios where cu.Id == em.IdUnidad select cu.Nombre).FirstOrDefault()
                                      }).Union
                                 (from b in db.BitacoraH
                                  join u in db.CatUsuarios on b.IdUser equals u.Id
                                  join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                  join p in db.CatProyectos on r.IdProyecto equals p.Id
                                  join e in db.CatEtapas on b.IdEtapa equals e.Id
                                  join a in db.CatActividades on b.Id equals a.Id
                                  join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                  join rue in db.RelacionUsuarioEmpleado on u.Id equals rue.IdUser
                                  join em in db.CatEmpleados on rue.IdEmpleado equals em.Id
                                  where r.IdArea == rua.IdArea && rua.IdUser == idUser && r.IdUnidad == null && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                  select new ReporteDetallado
                                  {
                                      Id = b.Id,
                                      Fecha = b.Fecha,
                                      Horas = b.Duracion,
                                      IdUser = u.Id,
                                      Usuario = u.Usuario.Replace(".", " "),
                                      IdProyecto = p.Id,
                                      Proyecto = p.Nombre,
                                      IdUnidad = 0,
                                      Unidad = "",
                                      IdArea = r.IdArea == null ? 0 : r.IdArea,
                                      Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                      Detalle = b.Descripcion,
                                      IdEtapa = e.Id,
                                      Etapa = e.Nombre,
                                      IdActividad = a.Id,
                                      Actividad = a.Nombre,
                                      FechaRegistro = b.FechaRegistro,
                                      IdUnidadUsuario = em.IdUnidad == null ? 0 : em.IdUnidad,
                                      UnidadUsuario = em.IdUnidad == null ? "" : (from cu in db.CatUnidadesNegocios where cu.Id == em.IdUnidad select cu.Nombre).FirstOrDefault()
                                  }).Union
                                  (from b in db.BitacoraH
                                   join u in db.CatUsuarios on b.IdUser equals u.Id
                                   join r in db.RelacionUsuarioEmpleado on b.IdUser equals r.IdUser
                                   join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                   join un in db.CatUnidadesNegocios on e.IdUnidad equals un.Id
                                   join a in db.CatActividades on b.IdActividad equals a.Id
                                   join rua in db.RelacionUsuarioUnidadArea on e.IdUnidad equals rua.IdUnidad
                                   join rue in db.RelacionUsuarioEmpleado on u.Id equals rue.IdUser
                                   join em in db.CatEmpleados on rue.IdEmpleado equals em.Id
                                   where rua.IdUser == idUser && b.IdProyecto == null && a.Evento == "1" && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                   select new ReporteDetallado
                                   {
                                       Id = b.Id,
                                       Fecha = b.Fecha,
                                       Horas = b.Duracion,
                                       IdUser = u.Id,
                                       Usuario = u.Usuario.Replace(".", " "),
                                       IdProyecto = 0,
                                       Proyecto = "",
                                       IdUnidad = un.Id,
                                       Unidad = un.Nombre,
                                       IdArea = 0,
                                       Area = "",
                                       IdActividad = a.Id,
                                       Actividad = a.Nombre,
                                       Detalle = b.Descripcion,
                                       IdEtapa = 0,
                                       Etapa = "",
                                       FechaRegistro = b.FechaRegistro,
                                       IdUnidadUsuario = em.IdUnidad == null ? 0 : em.IdUnidad,
                                       UnidadUsuario = em.IdUnidad == null ? "" : (from cu in db.CatUnidadesNegocios where cu.Id == em.IdUnidad select cu.Nombre).FirstOrDefault()
                                   }).Distinct().ToList();



                    listaDetallado = (from l in listaDetallado orderby l.Fecha, l.Usuario, l.Proyecto, l.Actividad, l.Etapa select l).Distinct(new CompareRepDetallado()).ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
            return listaDetallado;
        }

        public List<ReportePersonas> ConsultaPersonas(int idUser, DateTime fechaInicio, DateTime fechaFin, int idUsuarioFiltrado)
        {
            List<ReportePersonas> listaPersonas = new List<ReportePersonas>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    var lista = (from b in db.BitacoraH
                                 join u in db.CatUsuarios on b.IdUser equals u.Id
                                 join r in db.RelacionUsuarioEmpleado on b.IdUser equals r.IdUser
                                 join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                 join un in db.CatUnidadesNegocios on e.IdUnidad equals un.Id
                                 join a in db.CatActividades on b.IdActividad equals a.Id
                                 join rua in db.RelacionUsuarioUnidadArea on e.IdUnidad equals rua.IdUnidad
                                 where rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                 select new ReportePersonas
                                 {
                                     Id = b.Id,
                                     IdUser = u.Id,
                                     Usuario = u.Usuario.Replace(".", " "),
                                     IdUnidad = e.IdUnidad,
                                     Unidad = un.Nombre,
                                     Horas = b.Duracion,
                                     Estatus = e.EstatusERT == 6 || e.EstatusERT == 4 ? 0 : 1
                                 }).Distinct().ToList();


                    listaPersonas = lista
                       .GroupBy(l => new { l.Usuario })
                       .Select(x => new ReportePersonas
                       {
                           IdUser = x.First().IdUser,
                           Usuario = x.First().Usuario,
                           IdUnidad = x.First().IdUnidad,
                           Unidad = x.First().Unidad,
                           Horas = x.Sum(d => d.Horas),
                           Estatus = x.First().Estatus

                       }).ToList();

                    var usuariosBase = (from u in db.CatUsuarios
                                        join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                                        join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                        join un in db.CatUnidadesNegocios on e.IdUnidad equals un.Id
                                        join rua in db.RelacionUsuarioUnidadArea on e.IdUnidad equals rua.IdUnidad
                                        where rua.IdUser == idUser
                                           && u.Estatus == "1"
                                           && e.Estatus == "1"
                                           && u.RegistroBitacora == 1
                                        select new ReportePersonas
                                        {
                                            IdUser = u.Id,
                                            Usuario = u.Usuario.Replace(".", " "),
                                            IdUnidad = e.IdUnidad,
                                            Unidad = un.Nombre,
                                            Horas = 0,
                                            Estatus = e.EstatusERT == 6 || e.EstatusERT == 4 ? 0 : 1
                                        }).ToList();

                    var usuarios1 = usuariosBase.Where(u => !listaPersonas.Any(b => b.IdUser == u.IdUser) && u.IdUser != 16).ToList();

                    var usuariosBase2 = (from u in db.CatUsuarios
                                         join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                                         join rel in db.RelacionProyectoEmpleado on r.IdEmpleado equals rel.IdEmpleado
                                         join p in db.CatProyectos on rel.IdProyecto equals p.Id
                                         join rp in db.RelacionProyectos on p.Id equals rp.IdProyecto
                                         join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                         join un in db.CatUnidadesNegocios on e.IdUnidad equals un.Id
                                         join rua in db.RelacionUsuarioUnidadArea on e.IdUnidad equals rua.IdUnidad
                                         where rua.IdUser == idUser
                                            && u.Estatus == "1"
                                            && e.Estatus == "1"
                                            && rp.Estatus == "1"
                                            && rel.IdRol == 10
                                         select new
                                         {
                                             u.Id,
                                             u.Usuario,
                                             e.IdUnidad,
                                             UnidadNombre = un.Nombre,
                                             e.EstatusERT
                                         }).ToList();

                    var usuarios2 = usuariosBase2
                                    .Where(u => !listaPersonas.Any(b => b.IdUser == u.Id) && u.Id != 16)
                                    .Select(u => new ReportePersonas
                                    {
                                        IdUser = u.Id,
                                        Usuario = u.Usuario.Replace(".", " "),
                                        IdUnidad = u.IdUnidad,
                                        Unidad = u.UnidadNombre,
                                        Horas = 0,
                                        Estatus = (u.EstatusERT == 6 || u.EstatusERT == 4) ? 0 : 1
                                    })
                                    .ToList();

                    var usuarios = usuarios1.Union(usuarios2).Distinct(new ComparePersonas()).ToList();

                    listaPersonas = listaPersonas.Union(usuarios).Distinct(new ComparePersonas()).ToList();

                    if(idUsuarioFiltrado > 0)
                    {
                        listaPersonas = (from l in listaPersonas where l.IdUser == idUsuarioFiltrado orderby l.Usuario, l.Horas select l).Distinct().ToList();
                    }

                    listaPersonas = (from l in listaPersonas orderby l.Usuario, l.Horas select l).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaPersonas;

        }

        public List<ReporteProyectos> ConsultaProyectos(int idUser, DateTime fechaInicio, DateTime fechaFin, int idCliente)
        {
            List<ReporteProyectos> listaProyectos = new List<ReporteProyectos>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    var lista = (from b in db.BitacoraH
                                 join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                 join c in db.CatClientes on r.IdCliente equals c.Id
                                 join p in db.CatProyectos on r.IdProyecto equals p.Id
                                 join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                 join rue in db.RelacionUsuarioEmpleado on b.IdUser equals rue.IdUser
                                 join e in db.CatEmpleados on rue.IdEmpleado equals e.Id
                                 join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                 where r.IdArea == rua.IdArea && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                 select new ReporteProyectos
                                 {
                                     IdCliente = c.Id,
                                     Cliente = c.Nombre,
                                     IdProyecto = p.Id,
                                     Proyecto = p.Nombre,
                                     Horas = b.Duracion,
                                     IdUnidad = r.IdUnidad,
                                     Unidad = un.Nombre,
                                     IdArea = r.IdArea == null ? 0 : r.IdArea,
                                     Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                     IdUnidadRegistro = e.IdUnidad,
                                     UnidadRegistro = (from unidad in db.CatUnidadesNegocios where unidad.Id == e.IdUnidad select unidad.Nombre).FirstOrDefault(),

                                 }).ToList();

                    listaProyectos = lista
                       .GroupBy(l => new { l.IdProyecto})
                       .Select(x => new ReporteProyectos
                       {
                           IdCliente = x.First().IdCliente,
                           Cliente = x.First().Cliente,
                           IdProyecto = x.First().IdProyecto,
                           Proyecto = x.First().Proyecto,
                           Horas = x.Sum(d => d.Horas),
                           IdUnidad = x.First().IdUnidad,
                           Unidad = x.First().Unidad,
                           IdArea = x.First().IdArea,
                           Area = x.First().Area,
                           IdUnidadRegistro = x.First().IdUnidadRegistro,
                           UnidadRegistro = x.First().UnidadRegistro,

                       }).ToList();

                    var listaActividades = (from b in db.BitacoraH
                                            join u in db.CatUsuarios on b.IdUser equals u.Id
                                            join r in db.RelacionUsuarioEmpleado on b.IdUser equals r.IdUser
                                            join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                            join un in db.CatUnidadesNegocios on e.IdUnidad equals un.Id
                                            join a in db.CatActividades on b.IdActividad equals a.Id
                                            join rua in db.RelacionUsuarioUnidadArea on e.IdUnidad equals rua.IdUnidad
                                            where rua.IdUser == idUser && b.IdProyecto == null && a.Evento == "1" && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                            select new ReporteProyectos
                                            {
                                                Id = b.Id,
                                                IdActividad = a.Id,
                                                Actividad = a.Nombre,
                                                Horas = b.Duracion,
                                                IdUnidad = e.IdUnidad,
                                                Unidad = un.Nombre,
                                            }).Distinct().ToList();

                    var listaDistribucionAc = listaActividades
                         .GroupBy(l => new { l.IdUnidad, l.IdActividad })
                         .Select(x => new ReporteProyectos
                         {
                             IdActividad = x.First().IdActividad,
                             Actividad = x.First().Actividad,
                             Horas = x.Sum(d => d.Horas),
                             IdUnidad = x.First().IdUnidad,
                             Unidad = x.First().Unidad
                         }).ToList();

                    listaProyectos = listaProyectos.Union(listaDistribucionAc).ToList();

                    if (idCliente > 0)
                    {
                        listaProyectos = (from l in listaProyectos where l.IdCliente == idCliente orderby l.Cliente,l.IdUnidad, l.IdArea, l.Cliente, l.Proyecto, l.IdUnidadRegistro, l.Actividad select l).ToList();
                        
                    }

                   
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaProyectos;
        }
        public List<CatClientes> ConsultaClientesPorProyecto(int idUser, DateTime fechaInicio, DateTime fechaFin, int idUnidad, int idArea)
        {
            List<CatClientes> listaClientes = new List<CatClientes>();

            try
            {

                using (BitacoraContext db = new BitacoraContext())
                {
                    var lista = (from b in db.BitacoraH
                                 join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                 join c in db.CatClientes on r.IdCliente equals c.Id
                                 join p in db.CatProyectos on r.IdProyecto equals p.Id
                                 join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                 join rue in db.RelacionUsuarioEmpleado on b.IdUser equals rue.IdUser
                                 join e in db.CatEmpleados on rue.IdEmpleado equals e.Id
                                 join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                 where r.IdArea == rua.IdArea && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                 select new ReporteProyectos
                                 {
                                     IdCliente = c.Id,
                                     Cliente = c.Nombre,
                                     IdProyecto = p.Id,
                                     Proyecto = p.Nombre,
                                     Horas = b.Duracion,
                                     IdUnidad = r.IdUnidad,
                                     Unidad = un.Nombre,
                                     IdArea = r.IdArea == null ? 0 : r.IdArea,
                                     Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                     IdUnidadRegistro = e.IdUnidad,
                                     UnidadRegistro = (from unidad in db.CatUnidadesNegocios where unidad.Id == e.IdUnidad select unidad.Nombre).FirstOrDefault(),

                                 }).ToList();

                    if (idUnidad > 0)
                        lista = (from x in lista where x.IdUnidad == idUnidad select x).ToList();

                    if (idArea > 0)
                    {
                        var ProyectosArea = (from x in lista where x.IdArea == idArea select x).ToList();
                        var ProyectosEvento = (from x in lista where x.IdProyecto == 0 select x).ToList();

                        lista = ProyectosArea.Union(ProyectosEvento).ToList();
                    }

                    listaClientes = lista
                       .GroupBy(l => new { l.IdCliente })
                       .Select(x => new CatClientes
                       {
                           Id = x.First().IdCliente,
                           Nombre = x.First().Cliente,

                       }).ToList();

                    listaClientes = (from x in listaClientes orderby x.Nombre select x).ToList();

                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
            return listaClientes;
        }


        public List<ReporteSemanal> ConsultaSemanal(int idUser, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteSemanal> listaSemanal = new List<ReporteSemanal>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    var lista = (from b in db.BitacoraH
                                 join u in db.CatUsuarios on b.IdUser equals u.Id
                                 join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                 join p in db.CatProyectos on r.IdProyecto equals p.Id
                                 join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                 join a in db.CatActividades on b.IdActividad equals a.Id
                                 join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                 where r.IdArea == rua.IdArea && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                 select new ReporteSemanal
                                 {
                                     IdUser = u.Id,
                                     Usuario = u.Usuario.Replace(".", " "),
                                     IdProyecto = p.Id,
                                     Proyecto = p.Nombre,
                                     IdUnidad = r.IdUnidad,
                                     Unidad = un.Nombre,
                                     IdArea = r.IdArea == null ? 0 : r.IdArea,
                                     Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                     Actividad = a.Nombre,
                                     Horas = b.Duracion
                                 }).ToList();

                    listaSemanal = lista
                       .GroupBy(l => new { l.IdUser, l.IdProyecto })
                       .Select(x => new ReporteSemanal
                       {
                           IdUser = x.First().IdUser,
                           Usuario = x.First().Usuario,
                           IdProyecto = x.First().IdProyecto,
                           Proyecto = x.First().Proyecto,
                           IdUnidad = x.First().IdUnidad,
                           Unidad = x.First().Unidad,
                           IdArea = x.First().IdArea,
                           Area = x.First().Area,
                           Horas = x.Sum(d => d.Horas)
                       }).ToList();

                    var listaActividades = (from b in db.BitacoraH
                                            join u in db.CatUsuarios on b.IdUser equals u.Id
                                            join r in db.RelacionUsuarioEmpleado on b.IdUser equals r.IdUser
                                            join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                            join un in db.CatUnidadesNegocios on e.IdUnidad equals un.Id
                                            join a in db.CatActividades on b.IdActividad equals a.Id
                                            join rua in db.RelacionUsuarioUnidadArea on e.IdUnidad equals rua.IdUnidad
                                            where rua.IdUser == idUser && b.IdProyecto == null && a.Evento == "1" && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                            select new ReporteSemanal
                                            {
                                                Id = b.Id,
                                                IdUser = u.Id,
                                                Usuario = u.Usuario.Replace(".", " "),
                                                IdUnidad = e.IdUnidad,
                                                Unidad = un.Nombre,
                                                Actividad = a.Nombre,
                                                Horas = b.Duracion,
                                            }).Distinct().ToList();

                    var listaSemanalAc = listaActividades
                         .GroupBy(l => new { l.IdUser, l.Actividad })
                         .Select(x => new ReporteSemanal
                         {
                             IdUser = x.First().IdUser,
                             Usuario = x.First().Usuario,
                             Actividad = x.First().Actividad,
                             IdUnidad = x.First().IdUnidad,
                             Unidad = x.First().Unidad,
                             Horas = x.Sum(d => d.Horas)
                         }).ToList();

                    listaSemanal = listaSemanal.Union(listaSemanalAc).ToList();

                    listaSemanal = (from l in listaSemanal orderby l.IdUser, l.IdProyecto, l.IdUnidad, l.IdArea, l.Actividad select l).ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaSemanal;
        }
        public List<ReporteDetallado> ConsultaDetalladoUsuario(int idUser, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteDetallado> listaDetallado = new List<ReporteDetallado>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    var detalle1 = (from b in db.BitacoraH
                                     join u in db.CatUsuarios on b.IdUser equals u.Id
                                     join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                     join p in db.CatProyectos on r.IdProyecto equals p.Id
                                     join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                     join e in db.CatEtapas on b.IdEtapa equals e.Id
                                     join a in db.CatActividades on b.IdActividad equals a.Id
                                     where b.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                     select new ReporteDetallado
                                     {
                                         Id = b.Id,
                                         Fecha = b.Fecha,
                                         Horas = b.Duracion,
                                         IdUser = u.Id,
                                         Usuario = u.Usuario.Replace(".", " "),
                                         IdProyecto = p.Id,
                                         Proyecto = p.Nombre,
                                         IdUnidad = un.Id,
                                         Unidad = un.Nombre,
                                         IdArea = r.IdArea == null ? 0 : r.IdArea,
                                         Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                         Detalle = b.Descripcion,
                                         IdEtapa = e.Id,
                                         Etapa = e.Nombre,
                                         IdActividad = a.Id,
                                         Actividad = a.Nombre,
                                         FechaRegistro = b.FechaRegistro
                                     });

                    var detalle2 = (from b in db.BitacoraH
                                     join u in db.CatUsuarios on b.IdUser equals u.Id
                                     join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                     join p in db.CatProyectos on r.IdProyecto equals p.Id
                                     join e in db.CatEtapas on b.IdEtapa equals e.Id
                                     join a in db.CatActividades on b.Id equals a.Id
                                     where b.IdUser == idUser && r.IdUnidad == null && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                     select new ReporteDetallado
                                     {
                                         Id = b.Id,
                                         Fecha = b.Fecha,
                                         Horas = b.Duracion,
                                         IdUser = u.Id,
                                         Usuario = u.Usuario.Replace(".", " "),
                                         IdProyecto = p.Id,
                                         Proyecto = p.Nombre,
                                         IdUnidad = 0,
                                         Unidad = "",
                                         IdArea = r.IdArea == null ? 0 : r.IdArea,
                                         Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                         Detalle = b.Descripcion,
                                         IdEtapa = e.Id,
                                         Etapa = e.Nombre,
                                         IdActividad = a.Id,
                                         Actividad = a.Nombre,
                                         FechaRegistro = b.FechaRegistro
                                     });

                    var detalle3 = (from b in db.BitacoraH
                                     join u in db.CatUsuarios on b.IdUser equals u.Id
                                     join r in db.RelacionUsuarioEmpleado on b.IdUser equals r.IdUser
                                     join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                     join un in db.CatUnidadesNegocios on e.IdUnidad equals un.Id
                                     join a in db.CatActividades on b.IdActividad equals a.Id
                                     where b.IdUser == idUser && b.IdProyecto == null && a.Evento == "1" && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                     select new ReporteDetallado
                                     {
                                         Id = b.Id,
                                         Fecha = b.Fecha,
                                         Horas = b.Duracion,
                                         IdUser = u.Id,
                                         Usuario = u.Usuario.Replace(".", " "),
                                         IdProyecto = 0,
                                         Proyecto = a.Nombre,
                                         IdUnidad = un.Id,
                                         Unidad = un.Nombre,
                                         IdArea = 0,
                                         Area = "",
                                         Detalle = b.Descripcion,
                                         IdEtapa = 0,
                                         Etapa = "",
                                         IdActividad = a.Id,
                                         Actividad = a.Nombre,
                                         FechaRegistro = b.FechaRegistro
                                     });

                    listaDetallado = detalle1
                                    .Union(detalle2)
                                    .Union(detalle3)
                                    .Distinct()
                                    .ToList();
                    }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaDetallado;
        }

        public List<BItacoraInf> ConsultaListaUsuarios()
        {
            List<BItacoraInf> listaUsuarios = new List<BItacoraInf>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaUsuarios = (from u in db.CatUsuarios
                                     join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                                     join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                     where u.Estatus == "1" && e.Estatus == "1"
                                     select new BItacoraInf
                                     {
                                         IdUsr = u.Id,
                                         Usuario = u.Usuario.Replace(".", " "),
                                         IdUnidad = e.IdUnidad
                                     }).Union
                                 (from u in db.CatUsuarios
                                  join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser into le
                                  from r in le.DefaultIfEmpty()
                                  where r.IdUser == null
                                  where u.Estatus == "1"
                                  select new BItacoraInf
                                  {
                                      IdUsr = u.Id,
                                      Usuario = u.Usuario.Replace(".", " "),
                                      IdUnidad = 0
                                  }).ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaUsuarios;
        }

        public List<BItacoraInf> ConsultaListaProyectos()
        {
            List<BItacoraInf> listaProyectos = new List<BItacoraInf>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaProyectos = (from p in db.CatProyectos
                                      join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                      where r.Estatus == "1"
                                      select new BItacoraInf
                                      {
                                          IdProyecto = p.Id,
                                          Proyecto = p.Nombre,
                                          IdUnidad = r.IdUnidad == null ? 0 : r.IdUnidad,
                                          IdArea = r.IdArea == null ? 0 : r.IdArea
                                      }).ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
            return listaProyectos;
        }

        public List<RepDetalleProyecto> ConsultaPersonas_RegistroPorProyecto(int idProyecto, DateTime fechaInicio, DateTime fechaFin)
        {
            List<RepDetalleProyecto> listaUsuarios = new List<RepDetalleProyecto>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    var lista = (from u in db.CatUsuarios
                                 join b in db.BitacoraH on u.Id equals b.IdUser
                                 join p in db.CatProyectos on b.IdProyecto equals p.Id
                                 where b.Fecha >= fechaInicio && b.Fecha < fechaFin && b.IdProyecto == idProyecto
                                 select new
                                 {
                                     IdUsr = b.IdUser,
                                     Usuario = u.Usuario,
                                     Proyecto = p.Nombre,
                                     FechaInicio = fechaInicio,
                                     FechaFin = fechaFin,
                                     TotalHoras = (from tb in db.BitacoraH
                                                   where tb.Fecha >= fechaInicio && tb.Fecha < fechaFin && tb.IdProyecto == idProyecto && tb.IdUser == u.Id
                                                   select tb.Duracion).Sum()
                                 }).ToList();

                    listaUsuarios = lista.Select(item => new RepDetalleProyecto
                    {
                        IdUsr = item.IdUsr,
                        Usuario = item.Usuario,
                        Proyecto = item.Proyecto,
                        FechaInicio = item.FechaInicio.ToString("dd/MM/yyyy"),
                        FechaFin = item.FechaFin.ToString("dd/MM/yyyy"),
                        TotalHoras = item.TotalHoras,
                        Registros = (from bi in db.BitacoraH
                                     join ui in db.CatUsuarios on bi.IdUser equals ui.Id
                                     join pi in db.CatProyectos on bi.IdProyecto equals pi.Id
                                     join ai in db.CatActividades on bi.IdActividad equals ai.Id
                                     where bi.Fecha >= fechaInicio && bi.Fecha < fechaFin && bi.IdProyecto == idProyecto && bi.IdUser == item.IdUsr
                                     select new BItacoraInf
                                     {
                                         IdUsr = bi.IdUser,
                                         Usuario = ui.Usuario,
                                         Fecha = bi.Fecha,
                                         Activadad = ai.Nombre,
                                         Descripcion = bi.Descripcion,
                                         Etapa = bi.Fecha.ToString("dd/MM/yyyy"),
                                         Duracion = bi.Duracion
                                     }).ToList()
                    }).ToList();

                    listaUsuarios = listaUsuarios
                        .GroupBy(x => new { x.IdUsr, x.Usuario })
                        .Select(x => new RepDetalleProyecto
                        {
                            IdUsr = x.First().IdUsr,
                            Usuario = x.First().Usuario,
                            FechaInicio = x.First().FechaInicio,
                            FechaFin = x.First().FechaFin,
                            Proyecto = x.First().Proyecto,
                            TotalHoras = x.First().TotalHoras,
                            Registros = x.First().Registros
                        }).OrderBy(x => x.Usuario).ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaUsuarios;
        }

        public List<ReporteDetallado> ConsultaDetallado_UsuarioPorProyecto(int idUser, int idProyecto, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteDetallado> listaDetallado = new List<ReporteDetallado>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaDetallado = (from b in db.BitacoraH
                                      join u in db.CatUsuarios on b.IdUser equals u.Id
                                      join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                      join p in db.CatProyectos on r.IdProyecto equals p.Id
                                      join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                      join e in db.CatEtapas on b.IdEtapa equals e.Id
                                      join a in db.CatActividades on b.IdActividad equals a.Id
                                      where b.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin && b.IdProyecto == idProyecto
                                      select new ReporteDetallado
                                      {
                                          Id = b.Id,
                                          Fecha = b.Fecha,
                                          Horas = b.Duracion,
                                          IdUser = u.Id,
                                          Usuario = u.Usuario.Replace(".", " "),
                                          IdProyecto = p.Id,
                                          Proyecto = p.Nombre,
                                          IdUnidad = un.Id,
                                          Unidad = un.Nombre,
                                          IdArea = r.IdArea == null ? 0 : r.IdArea,
                                          Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                          Detalle = b.Descripcion,
                                          IdEtapa = e.Id,
                                          Etapa = e.Nombre,
                                          IdActividad = a.Id,
                                          Actividad = a.Nombre,
                                          FechaRegistro = b.FechaRegistro
                                      }).Union
                                 (from b in db.BitacoraH
                                  join u in db.CatUsuarios on b.IdUser equals u.Id
                                  join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                  join p in db.CatProyectos on r.IdProyecto equals p.Id
                                  join e in db.CatEtapas on b.IdEtapa equals e.Id
                                  join a in db.CatActividades on b.Id equals a.Id
                                  where b.IdUser == idUser && r.IdUnidad == null && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                  select new ReporteDetallado
                                  {
                                      Id = b.Id,
                                      Fecha = b.Fecha,
                                      Horas = b.Duracion,
                                      IdUser = u.Id,
                                      Usuario = u.Usuario.Replace(".", " "),
                                      IdProyecto = p.Id,
                                      Proyecto = p.Nombre,
                                      IdArea = r.IdArea == null ? 0 : r.IdArea,
                                      Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                      Detalle = b.Descripcion,
                                      IdEtapa = e.Id,
                                      Etapa = e.Nombre,
                                      IdActividad = a.Id,
                                      Actividad = a.Nombre,
                                      FechaRegistro = b.FechaRegistro
                                  }).ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaDetallado;
        }


        public List<BItacoraInf> ConsultaListaProyectosUsuario(int idUser)
        {
            List<BItacoraInf> listaProyectos = new List<BItacoraInf>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaProyectos = (from p in db.CatProyectos
                                      join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                      join re in db.RelacionProyectoEmpleado on p.Id equals re.IdProyecto
                                      join ru in db.RelacionUsuarioEmpleado on re.IdEmpleado equals ru.IdEmpleado
                                      join u in db.CatUsuarios on ru.IdUser equals u.Id
                                      where r.Estatus == "1" && u.Id == idUser && re.IdRol == 10
                                      select new BItacoraInf
                                      {
                                          IdProyecto = p.Id,
                                          Proyecto = p.Nombre,
                                          IdUnidad = r.IdUnidad == null ? 0 : r.IdUnidad,
                                          IdArea = r.IdArea == null ? 0 : r.IdArea,
                                      }).ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }


            return listaProyectos;

        }

        public List<BItacoraInf> ConsultaListaActividades()
        {
            List<BItacoraInf> listaActividades = new List<BItacoraInf>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaActividades = (from b in db.BitacoraH
                                        join a in db.CatActividades on b.IdActividad equals a.Id
                                        join r in db.RelacionUsuarioEmpleado on b.IdUser equals r.IdUser
                                        join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                        where a.Estatus == "1" && a.Evento == "1"
                                        select new BItacoraInf
                                        {
                                            IdActividad = a.Id,
                                            Activadad = a.Nombre,
                                            IdUsr = b.IdUser,
                                            IdUnidad = e.IdUnidad
                                        }).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }


            return listaActividades;

        }
        public decimal ConsultaHorasProyecto_Semana(DateTime fechaInicio, DateTime fechaFin, int idProyecto)
        {
            decimal horasSemana = 0;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    horasSemana = (from bi in db.BitacoraH
                                   join ui in db.CatUsuarios on bi.IdUser equals ui.Id
                                   join pi in db.CatProyectos on bi.IdProyecto equals pi.Id
                                   where bi.Fecha >= fechaInicio && bi.Fecha < fechaFin && bi.IdProyecto == idProyecto
                                   select bi.Duracion).Sum();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
            return horasSemana;
        }
        public List<BItacoraInf> ConsultaListaEtapas()
        {
            List<BItacoraInf> listaEtapas = new List<BItacoraInf>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaEtapas = (from b in db.BitacoraH
                                   join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                   join e in db.CatEtapas on b.IdEtapa equals e.Id
                                   where r.Estatus == "1" && e.Estatus == "1"
                                   select new BItacoraInf
                                   {
                                       IdEtapa = e.Id,
                                       Etapa = e.Nombre,
                                       IdUsr = b.IdUser,
                                       IdUnidad = r.IdUnidad == null ? 0 : r.IdUnidad,
                                       IdArea = r.IdArea == null ? 0 : r.IdArea,
                                       IdProyecto = b.IdProyecto
                                   }).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaEtapas;
        }

        public List<ReporteSemanal> ConsultaEjecutivo(int idUser, DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteSemanal> listaSemanal = new List<ReporteSemanal>();

            try
            {
                if (fechaInicio < new DateTime(1753, 1, 1) || fechaInicio > new DateTime(9999, 12, 31))
                {
                    fechaInicio = new DateTime(1753, 1, 1);
                }

                if (fechaFin < new DateTime(1753, 1, 1) || fechaFin > new DateTime(9999, 12, 31))
                {
                    fechaFin = new DateTime(1753, 1, 2);
                }

                using (BitacoraContext db = new BitacoraContext())
                {
                    var lista = (from b in db.BitacoraH
                                 join u in db.CatUsuarios on b.IdUser equals u.Id
                                 join r in db.RelacionProyectos on b.IdProyecto equals r.IdProyecto
                                 join p in db.CatProyectos on r.IdProyecto equals p.Id
                                 join cp in db.CategoriasProyecto on p.IdCategoria equals cp.IdCategoria
                                 join un in db.CatUnidadesNegocios on r.IdUnidad equals un.Id
                                 join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                 where r.IdArea == rua.IdArea && rua.IdUser == idUser /*&& b.Fecha >= fechaInicio*/ && b.Fecha < fechaFin
                                 select new ReporteSemanal
                                 {
                                     IdProyecto = p.Id,
                                     IdCategoria = p.IdCategoria,
                                     IdUnidad = r.IdUnidad,
                                     IdArea = r.IdArea == null ? 0 : r.IdArea,
                                     Horas = b.Duracion
                                 }).ToList();



                    var listaActividades = (from b in db.BitacoraH
                                            join u in db.CatUsuarios on b.IdUser equals u.Id
                                            join r in db.RelacionUsuarioEmpleado on b.IdUser equals r.IdUser
                                            join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                            join un in db.CatUnidadesNegocios on e.IdUnidad equals un.Id
                                            join a in db.CatActividades on b.IdActividad equals a.Id
                                            join rua in db.RelacionUsuarioUnidadArea on e.IdUnidad equals rua.IdUnidad
                                            where rua.IdUser == idUser && b.IdProyecto == null && a.Evento == "1" && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                            select new ReporteSemanal
                                            {
                                                Id = b.Id,
                                                IdUser = u.Id,
                                                Usuario = u.Usuario.Replace(".", " "),
                                                IdUnidad = e.IdUnidad,
                                                Unidad = un.Nombre,
                                                Actividad = a.Nombre,
                                                Horas = b.Duracion,
                                            }).Distinct().ToList();


                    var listaSemanalAc = listaActividades
                         .GroupBy(l => new { l.IdUnidad, l.Actividad })
                         .Select(x => new ReporteSemanal
                         {
                             Proyecto = x.First().Actividad,
                             IdUnidad = x.First().IdUnidad,
                             Unidad = x.First().Unidad,
                             Horas = x.Sum(d => d.Horas)
                         }).ToList();

                    listaSemanal = lista.Union(listaSemanalAc).ToList();

                    listaSemanal = (from l in listaSemanal orderby l.Proyecto select l).ToList();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaSemanal;
        }

    }

}

