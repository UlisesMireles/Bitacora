using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BitacoraModels;
using System.Net.NetworkInformation;

namespace BitacoraData
{
    public class Bitacora_Data
    {
        public List<BItacoraInf> ConsultaBitacora(int idUser)
        {
            List<BItacoraInf> ListaBitacora = new List<BItacoraInf>();

            DateTime dt = DateTime.Now;
            DateTime wkStDt = DateTime.MinValue;
            wkStDt = dt.AddDays(1 - Convert.ToDouble(dt.DayOfWeek));
            DateTime fechaFin = wkStDt.Date;
            DateTime fechaInicio = fechaFin.AddDays(-14);

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    ListaBitacora = (from b in db.BitacoraH
                                     join p in db.CatProyectos on b.IdProyecto equals p.Id
                                     join a in db.CatActividades on b.IdActividad equals a.Id
                                     join e in db.CatEtapas on b.IdEtapa equals e.Id
                                     where b.IdUser == idUser && b.Fecha >= fechaInicio
                                     select new BItacoraInf
                                     {
                                         Id = b.Id,
                                         Fecha = b.Fecha,
                                         IdProyecto = p.Id,
                                         Proyecto = p.Nombre,
                                         IdActividad = a.Id,
                                         Activadad = a.Nombre,
                                         IdEtapa = e.Id,
                                         Etapa = e.Nombre,
                                         Duracion = b.Duracion,
                                         Descripcion = b.Descripcion
                                     }).Union
                                     (from b in db.BitacoraH
                                        join a in db.CatActividades on b.IdActividad equals a.Id
                                        where b.IdUser == idUser && b.IdProyecto == null && b.Fecha >= fechaInicio
                                      select new BItacoraInf
                                        {
                                            Id = b.Id,
                                            Fecha = b.Fecha,
                                            IdProyecto = 0,
                                            Proyecto = "",
                                            IdActividad = a.Id,
                                            Activadad = a.Nombre,
                                            IdEtapa = 0,
                                            Etapa = "",
                                            Duracion = b.Duracion,
                                            Descripcion = b.Descripcion
                                        }).ToList();
                }

            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return ListaBitacora;
        }

        public List<CatProyectos> GetProyectos(int idUser)
        {
            List<CatProyectos> Proyectos = new List<CatProyectos>();
            List<RelacionProyectos> relacionProyectos = new List<RelacionProyectos>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    Proyectos = (from p in db.CatProyectos
                                 join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                 join re in db.RelacionProyectoEmpleado on p.Id equals re.IdProyecto
                                 join ru in db.RelacionUsuarioEmpleado on re.IdEmpleado equals ru.IdEmpleado
                                 join c in db.CatClientes on r.IdCliente equals c.Id
                                 where r.Estatus == "1" && ru.IdUser == idUser &&  re.IdRol == 10 && r.IdEstatusProceso != null
                                 select new CatProyectos
                                 {
                                     Id = p.Id,
                                     Nombre = p.Nombre.Trim() + " - "  + c.Nombre.Trim(),
                                 }).ToList();
                }
            }catch(Exception e){
                string result = e.Message;
            }

                return Proyectos;
        }

        public List<CatEtapas> GetEtapas()
        {
            List<CatEtapas> etapas = new List<CatEtapas>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    etapas = (from e in db.CatEtapas
                              where e.Estatus == "1"
                              select new CatEtapas { Id = e.Id, Nombre = e.Nombre }).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return etapas;
        }

        public List<RelacionEtapaEstatus> GetRelacionesEtapas(List<string> listaEstatus)
        {
            List<RelacionEtapaEstatus> relacionesEtapas = new List<RelacionEtapaEstatus>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    List<int?> estatusIds = listaEstatus.Select(s => (int?)Convert.ToInt32(s)).ToList();

                    relacionesEtapas = (from e in db.CatEtapas
                              join r in db.RelacionEtapaEstatuses on e.Id equals r.IdEtapa
                              where e.Estatus == "1" && r.Activo == true && estatusIds.Contains(r.IdEstatus)
                                        select new RelacionEtapaEstatus { Id = r.Id, IdEtapa = r.IdEtapa }).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return relacionesEtapas;
        }

        public List<CatActividades> GetActividades()
        {
            List<CatActividades> actividades = new List<CatActividades>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    actividades = (from a in db.CatActividades
                              where a.Estatus == "1"
                              select new CatActividades { Id = a.Id, Nombre = a.Nombre, Evento = a.Evento}).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return actividades;
        }

        public List<RelacionActividadEstatus> GetRelacionesActividades(List<string> listaEstatus)
        {
            List<RelacionActividadEstatus> relacionesEtapas = new List<RelacionActividadEstatus>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    List<int?> estatusIds = listaEstatus.Select(s => (int?)Convert.ToInt32(s)).ToList();

                    relacionesEtapas = (from e in db.CatActividades
                                        join r in db.RelacionActividadEstatuses on e.Id equals r.IdActividad
                                        where e.Estatus == "1" && r.Activo == true && estatusIds.Contains(r.IdEstatus)
                                        select new RelacionActividadEstatus { Id = r.Id, IdActividad = r.IdActividad }).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return relacionesEtapas;
        }

        public List<RelacionProyectos> GetRelacionProyectos(int idUser)
        {
            List<RelacionProyectos> relacionProyectos = new List<RelacionProyectos>();

            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    relacionProyectos = (from p in db.CatProyectos
                                         join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                         join re in db.RelacionProyectoEmpleado on p.Id equals re.IdProyecto
                                         join ru in db.RelacionUsuarioEmpleado on re.IdEmpleado equals ru.IdEmpleado
                                         join c in db.CatClientes on r.IdCliente equals c.Id
                                         where r.Estatus == "1" && ru.IdUser == idUser && re.IdRol == 10 && (r.IdEstatusProceso != null)
                                         select new RelacionProyectos
                                         {
                                             IdProyecto = p.Id,
                                             IdEstatusProceso = r.IdEstatusProceso,
                                         }).ToList();

                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return relacionProyectos;
        }

        public int InsertaBitacora(BitacoraH datos)
        {
            int result = 0;
            BitacoraH registro = new BitacoraH();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    db.BitacoraH.Add(datos);
                    db.SaveChanges();
                    result = datos.Id;
                }
            }
            catch (Exception e)
            {
                string resultado = e.Message;
                result = -1;
            }

            return result;
        }

        public int ModificaBitacora(BitacoraH datos)
        {
            int result;
            BitacoraH registro = new BitacoraH();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    registro = (from b in db.BitacoraH where b.Id == datos.Id select b).FirstOrDefault();

                    registro.Fecha      = datos.Fecha.Year < 1900? registro.Fecha : datos.Fecha;
                    registro.IdProyecto = datos.IdProyecto;
                    registro.IdEtapa    = datos.IdProyecto == null ? null : datos.IdEtapa;
                    registro.IdActividad = datos.IdActividad == 0 ? registro.IdActividad : datos.IdActividad;
                    registro.Descripcion = datos.Descripcion;
                    registro.Duracion    = datos.Duracion;
                    registro.FechaModificacion = DateTime.Today;

                    db.SaveChanges();
                    result = registro.Id;

                   
                }
            }
            catch (Exception e)
            {
                string resultado = e.Message;
                result = -1;
            }

            return result;
        }

        public int EliminarBitacora(int id)
        {
            BitacoraH registro = new BitacoraH();
            int result = 1;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    registro = (from b in db.BitacoraH where b.Id == id select b).FirstOrDefault();

                    db.BitacoraH.Remove(registro);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                result = -1;
            }

            return result;
        }

    }
}
