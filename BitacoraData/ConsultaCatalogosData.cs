using System;
using System.Collections.Generic;
using System.Linq;
using BitacoraModels;

namespace BitacoraData
{
    public class ConsultaCatalogosData
    {
        public List<CatUnidadesNegocios> ConsultaUnidadesNegocio()
        {
            List<CatUnidadesNegocios> listaUnidades = new List<CatUnidadesNegocios>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaUnidades = (from r in db.RelacionProyectos
                                     join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                     join a in db.CatAreasNegocio on r.IdArea equals a.Id
                                     select new CatUnidadesNegocios
                                     {
                                         Id = u.Id,
                                         Nombre = u.Nombre,
                                         Area = a.Nombre,
                                         Estatus = (u.Estatus == "0" ? "Inactivo" : u.Estatus == "1" ? "Activo" : "")
                                     }).Distinct().ToList();
                    var lista = (from u in db.CatUnidadesNegocios
                                       select new CatUnidadesNegocios
                                       {
                                           Id = u.Id,
                                           Nombre = u.Nombre,
                                           Area = "",
                                           Estatus = (u.Estatus == "0" ? "Inactivo" : u.Estatus == "1" ? "Activo" : "")
                                       }).ToList();

                    var Elementos = (from l in lista where !listaUnidades.Any(x => x.Id == l.Id) select l).ToList();
                    listaUnidades = listaUnidades.Concat(Elementos).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaUnidades;
        }      

        public List<CatAreasNegocio> ConsultaAreasNegocio()
        {
            List<CatAreasNegocio> listaUnidades = new List<CatAreasNegocio>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaUnidades = (from a in db.CatAreasNegocio
                                     select new CatAreasNegocio
                                     {
                                         Id = a.Id,
                                         Nombre = a.Nombre,
                                         Estatus = (a.Estatus == "0" ? "Inactivo" : a.Estatus == "1" ? "Activo" :  "")
                                     }).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaUnidades;
        }

        public List<CatClientes> ConsultaClientes()
        {
            List<CatClientes> listaClientes = new List<CatClientes>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaClientes = (from c in db.CatClientes
                                     join r in db.RelacionProyectos on c.Id equals r.IdCliente
                                     select new CatClientes
                                     {
                                         Id = c.Id,
                                         Nombre = c.Nombre,
                                         Alias = c.Alias,
                                         Giro = c.Giro,
                                         Unidad = r.IdUnidad == null ? "" : (from u in db.CatUnidadesNegocios where u.Id == r.IdUnidad select u.Nombre).FirstOrDefault(),
                                         Area = r.IdArea == null ? "" : (from a in db.CatAreasNegocio where a.Id == r.IdArea select a.Nombre).FirstOrDefault(),
                                         Estatus = (c.Estatus == "0" ? "Inactivo" : c.Estatus == "1" ? "Activo" : "")
                                     }).Distinct().Union
                                     (from c in db.CatClientes
                                      join r in db.RelacionProyectos on c.Id equals r.IdCliente into ps                                      
                                      from r in ps.DefaultIfEmpty()
                                      where r.IdProyecto == null
                                      select new CatClientes
                                      {
                                          Id = c.Id,
                                          Nombre = c.Nombre,
                                          Alias = c.Alias,
                                          Giro = c.Giro,
                                          Unidad = null,
                                          Area = null,
                                          Estatus = (c.Estatus == "0" ? "Inactivo" : c.Estatus == "1" ? "Activo" : "")
                                      }).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaClientes;
        }

        public List<CatSistemas> ConsultaSistemas()
        {
            List<CatSistemas> listaSistemas= new List<CatSistemas>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaSistemas = (from s in db.CatSistemas
                                     join r in db.RelacionSistemaCliente on s.Id equals r.IdSistema
                                     join c in db.CatClientes on r.IdCliente equals c.Id
                                     select new CatSistemas
                                     {
                                         Id = s.Id,
                                         Nombre = s.Nombre,
                                         Cliente = c.Nombre,
                                         Estatus = (s.Estatus == "1" ? "Activo" : s.Estatus == "0" ? "Inactivo" : "")
                                     }
                                     ).Distinct().Union 
                                     (from s in db.CatSistemas
                                     join r in db.RelacionSistemaCliente on s.Id equals r.IdSistema into ps
                                     from r in ps.DefaultIfEmpty()
                                     where r.Id == null
                                     select new CatSistemas
                                     {
                                         Id = s.Id,
                                         Nombre = s.Nombre,
                                         Cliente = null,
                                         Estatus = (s.Estatus == "1" ? "Activo" : s.Estatus == "0" ? "Inactivo" : "")
                                     }).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaSistemas;
        }

        public List<CatEtapas> ConsultaEtapas()
        {
            List<CatEtapas> listaEtapas = new List<CatEtapas>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaEtapas = (from e in db.CatEtapas select new CatEtapas { Id = e.Id, Nombre = e.Nombre, Estatus = (e.Estatus == "1" ? "Activo" : e.Estatus == "0" ? "Inactivo" : "") }).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaEtapas;
        }

        public List<CatActividades> ConsultaActividades()
        {
            List<CatActividades> listaActividades = new List<CatActividades>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaActividades = (from a in db.CatActividades select 
                                        new CatActividades
                                        {
                                            Id = a.Id, Nombre = a.Nombre,
                                            Evento = (a.Evento == "1" ? "Extraordinaria" : "Proyecto"),
                                            Estatus = (a.Estatus == "1" ? "Activo" : a.Estatus == "0" ? "Inactivo" : "")
                                        }).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaActividades;
        }

        public List<CatProyectos> ConsultaProyectos()
        {
            List<CatProyectos> listaProyectos = new List<CatProyectos>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaProyectos = (from p in db.CatProyectos
                                      join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                      join c in db.CatClientes on r.IdCliente equals c.Id
                                      join s in db.CatSistemas on r.IdSistema equals s.Id
                                      join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                      join a in db.CatAreasNegocio on r.IdArea equals a.Id
                                      select new CatProyectos
                                      {
                                          Id = p.Id,
                                          Nombre = p.Nombre,
                                          NombreCorto = p.NombreCorto,
                                          TotalHoras = p.TotalHoras,
                                          Sistema = s.Nombre,
                                          Cliente = c.Nombre,
                                          UnidadArea = u.Nombre + "-" + a.Nombre,
                                          Estatus = (r.Estatus == "1" ? "Activo" : r.Estatus == "0" ? "Inactivo" : "")
                                      }).Union
                                      (from p in db.CatProyectos
                                       join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                       join c in db.CatClientes on r.IdCliente equals c.Id
                                       join s in db.CatSistemas on r.IdSistema equals s.Id
                                       join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id into ps
                                       from u in ps.DefaultIfEmpty()
                                       where u.Id == null
                                       select new CatProyectos
                                       {
                                           Id = p.Id,
                                           Nombre = p.Nombre,
                                           NombreCorto = p.NombreCorto,
                                           TotalHoras = p.TotalHoras,
                                           Sistema = s.Nombre,
                                           Cliente = c.Nombre,
                                           UnidadArea = null,
                                           Estatus = (r.Estatus == "1" ? "Activo" : r.Estatus == "0" ? "Inactivo" : "")
                                       }).Union
                                      (from p in db.CatProyectos
                                       join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                       join c in db.CatClientes on r.IdCliente equals c.Id
                                       join s in db.CatSistemas on r.IdSistema equals s.Id into ps
                                       from s in ps.DefaultIfEmpty()
                                       where s.Id == null
                                       select new CatProyectos
                                       {
                                           Id = p.Id,
                                           Nombre = p.Nombre,
                                           NombreCorto = p.NombreCorto,
                                           TotalHoras = p.TotalHoras,
                                           Sistema = null,
                                           Cliente = c.Nombre,
                                           UnidadArea = null,
                                           Estatus = (r.Estatus == "1" ? "Activo" : r.Estatus == "0" ? "Inactivo" : "")
                                       }).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }


            return listaProyectos;

        }

    }
}
