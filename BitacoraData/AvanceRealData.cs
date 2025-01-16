using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BitacoraModels;

namespace BitacoraData
{
    public class AvanceRealData
    {

        ConsultaCatalogosData _ConsultaCatalogosData = new ConsultaCatalogosData();
        public int ConsultaUnidadEmpleado(int idUser)
        {
            int idUnidad = 0;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    idUnidad = (from e in db.CatEmpleados
                                join r in db.RelacionUsuarioEmpleado on e.Id equals r.IdEmpleado
                                join u in db.CatUsuarios on r.IdUser equals u.Id
                                where u.Id == idUser
                                select e.IdUnidad).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
            return idUnidad;
        }
        
        public List<AvanceRealList> ConsultaAvanceReal(int idUser)
        {
            List<AvanceRealList> listaAvanceR = new List<AvanceRealList>();

            DateTime dt = DateTime.Now;
            DateTime wkStDt = DateTime.MinValue;
            wkStDt = dt.AddDays(1 - Convert.ToDouble(dt.DayOfWeek));
            DateTime fechaFin = wkStDt.Date;
            DateTime fechaInicio = fechaFin.AddDays(-14);

            using (BitacoraContext db = new BitacoraContext())
            {
                try
                {
                    var lista = (from p in db.CatProyectos
                                 join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                 where r.Estatus == "1"
                                 join a in db.AvanceReal on p.Id equals a.IdProyecto
                                 join b in db.BitacoraH on p.Id equals b.IdProyecto
                                 join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                 join re in db.RelacionProyectoEmpleado on p.Id equals re.IdProyecto
                                 join ru in db.RelacionUsuarioEmpleado on re.IdEmpleado equals ru.IdEmpleado
                                 join rua in db.RelacionUsuarioUnidadArea on ru.IdUser equals rua.IdUser
                                 where r.IdUnidad == rua.IdUnidad && r.IdArea == rua.IdArea && re.IdRol == 1 && ru.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                 select new AvanceRealList
                                 {
                                     IdProyecto = p.Id,
                                     NompreProyecto = p.Nombre,
                                     Horas = b.Duracion,
                                     Avance = a.AvanceReal1,
                                     IdUnidad = r.IdUnidad,
                                     Unidad = u.Nombre,
                                     IdArea = r.IdArea == null ? 0 : r.IdArea,
                                     Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                     FechaRegistro = a.FechaRegistro,
                                     Bandera = 1
                                 }).Union
                                     (from p in db.CatProyectos
                                      join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                      where r.Estatus == "1"
                                      join a in db.AvanceReal on p.Id equals a.IdProyecto
                                      join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                      join re in db.RelacionProyectoEmpleado on p.Id equals re.IdProyecto
                                      join ru in db.RelacionUsuarioEmpleado on re.IdEmpleado equals ru.IdEmpleado
                                      join rua in db.RelacionUsuarioUnidadArea on ru.IdUser equals rua.IdUser
                                      where r.IdUnidad == rua.IdUnidad && r.IdArea == rua.IdArea && re.IdRol == 1 && ru.IdUser == idUser
                                      join b in db.BitacoraH on p.Id equals b.IdProyecto into le
                                      from b in le.DefaultIfEmpty()
                                      where b.Id == null
                                      select new AvanceRealList
                                      {
                                          IdProyecto = p.Id,
                                          NompreProyecto = p.Nombre,
                                          Horas = 0,
                                          Avance = a.AvanceReal1,
                                          IdUnidad = r.IdUnidad,
                                          Unidad = u.Nombre,
                                          IdArea = r.IdArea == null ? 0 : r.IdArea,
                                          Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                          FechaRegistro = a.FechaRegistro,
                                          Bandera = 1
                                      }).Union
                                      (from p in db.CatProyectos
                                       join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                       where r.Estatus == "1"
                                       join b in db.BitacoraH on p.Id equals b.IdProyecto
                                       join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                       join re in db.RelacionProyectoEmpleado on p.Id equals re.IdProyecto
                                       join ru in db.RelacionUsuarioEmpleado on re.IdEmpleado equals ru.IdEmpleado
                                       join rua in db.RelacionUsuarioUnidadArea on ru.IdUser equals rua.IdUser
                                       where r.IdUnidad == rua.IdUnidad && r.IdArea == rua.IdArea && re.IdRol == 1 && ru.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                       join a in db.AvanceReal on p.Id equals a.IdProyecto into la
                                       from a in la.DefaultIfEmpty()
                                       where a.IdProyecto == null
                                       select new AvanceRealList
                                       {
                                           IdProyecto = p.Id,
                                           NompreProyecto = p.Nombre,
                                           Horas = b.Duracion,
                                           Avance = 0,
                                           IdUnidad = r.IdUnidad,
                                           Unidad = u.Nombre,
                                           IdArea = r.IdArea == null ? 0 : r.IdArea,
                                           Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                           FechaRegistro = null,
                                           Bandera = 1
                                       }).Union
                                     (from p in db.CatProyectos
                                      join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                      where r.Estatus == "1"
                                      join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                      join re in db.RelacionProyectoEmpleado on p.Id equals re.IdProyecto
                                      join ru in db.RelacionUsuarioEmpleado on re.IdEmpleado equals ru.IdEmpleado
                                      join rua in db.RelacionUsuarioUnidadArea on ru.IdUser equals rua.IdUser
                                      where r.IdUnidad == rua.IdUnidad && r.IdArea == rua.IdArea && re.IdRol == 1 && ru.IdUser == idUser
                                      join b in  (from bt in db.BitacoraH where bt.Fecha >= fechaInicio && bt.Fecha < fechaFin select bt) on p.Id equals b.IdProyecto into le
                                      from b in le.DefaultIfEmpty()
                                      where b.Id == null
                                      join a in db.AvanceReal on p.Id equals a.IdProyecto into la
                                      from a in la.DefaultIfEmpty()
                                      where a.IdProyecto == null
                                      select new AvanceRealList
                                      {
                                          IdProyecto = p.Id,
                                          NompreProyecto = p.Nombre,
                                          Horas = 0,
                                          Avance = 0,
                                          IdUnidad = r.IdUnidad,
                                          Unidad = u.Nombre,
                                          IdArea = r.IdArea == null ? 0 : r.IdArea,
                                          Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                          FechaRegistro = null,
                                          Bandera = 1
                                      }).ToList();


                    listaAvanceR = lista
                        .GroupBy(l => l.IdProyecto)
                        .Select(x => new AvanceRealList
                        {
                            IdProyecto = x.First().IdProyecto,
                            NompreProyecto = x.First().NompreProyecto,
                            Horas = x.Sum(d => d.Horas),
                            Avance = x.First().Avance,
                            IdUnidad = x.First().IdUnidad,
                            Unidad = x.First().Unidad,
                            IdArea = x.First().IdArea,
                            Area = x.First().Area,
                            FechaRegistro = x.First().FechaRegistro,
                            Bandera = x.First().Bandera

                        }).ToList();

                    var lista2 = (from p in db.CatProyectos
                                  join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                  where r.Estatus == "1"
                                  join a in db.AvanceReal on p.Id equals a.IdProyecto
                                  join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                  join b in db.BitacoraH on p.Id equals b.IdProyecto
                                  join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                  where r.IdArea == rua.IdArea && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                  select new AvanceRealList
                                  {
                                      IdProyecto = p.Id,
                                      NompreProyecto = p.Nombre,
                                      Horas = b.Duracion,
                                      Avance = a.AvanceReal1,
                                      IdUnidad = r.IdUnidad,
                                      Unidad = u.Nombre,
                                      IdArea = r.IdArea == null ? 0 : r.IdArea,
                                      Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                      FechaRegistro = a.FechaRegistro,
                                      Bandera = 0
                                  }).Union
                                  (from p in db.CatProyectos
                                   join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                   where r.Estatus == "1"
                                   join a in db.AvanceReal on p.Id equals a.IdProyecto
                                   join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                   join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                   where r.IdArea == rua.IdArea && rua.IdUser == idUser
                                   join b in db.BitacoraH on p.Id equals b.IdProyecto into le
                                   from b in le.DefaultIfEmpty()
                                   where b.Id == null
                                   select new AvanceRealList
                                   {
                                       IdProyecto = p.Id,
                                       NompreProyecto = p.Nombre,
                                       Horas = 0,
                                       Avance = a.AvanceReal1,
                                       IdUnidad = r.IdUnidad,
                                       Unidad = u.Nombre,
                                       IdArea = r.IdArea == null ? 0 : r.IdArea,
                                       Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                       FechaRegistro = a.FechaRegistro,
                                       Bandera = 0
                                   }).Union
                                   (from p in db.CatProyectos
                                    join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                    where r.Estatus == "1"
                                    join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                    join b in db.BitacoraH on p.Id equals b.IdProyecto
                                    join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                    where r.IdArea == rua.IdArea && rua.IdUser == idUser && b.Fecha >= fechaInicio && b.Fecha < fechaFin
                                    join a in db.AvanceReal on p.Id equals a.IdProyecto into la
                                    from a in la.DefaultIfEmpty()
                                    where a.IdProyecto == null
                                    select new AvanceRealList
                                    {
                                        IdProyecto = p.Id,
                                        NompreProyecto = p.Nombre,
                                        Horas = b.Duracion,
                                        Avance = 0,
                                        IdUnidad = r.IdUnidad,
                                        Unidad = u.Nombre,
                                        IdArea = r.IdArea == null ? 0 : r.IdArea,
                                        Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                        FechaRegistro = null,
                                        Bandera = 0
                                    }).Union
                                  (from p in db.CatProyectos
                                   join r in db.RelacionProyectos on p.Id equals r.IdProyecto
                                   where r.Estatus == "1"
                                   join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                   join rua in db.RelacionUsuarioUnidadArea on r.IdUnidad equals rua.IdUnidad
                                   where r.IdArea == rua.IdArea && rua.IdUser == idUser
                                   join b in (from bt in db.BitacoraH where bt.Fecha >= fechaInicio && bt.Fecha < fechaFin select bt) on p.Id equals b.IdProyecto into le
                                   from b in le.DefaultIfEmpty()
                                   where b.Id == null 
                                   join a in db.AvanceReal on p.Id equals a.IdProyecto into la
                                   from a in la.DefaultIfEmpty()
                                   where a.IdProyecto == null
                                   select new AvanceRealList
                                   {
                                       IdProyecto = p.Id,
                                       NompreProyecto = p.Nombre,
                                       Horas = 0,
                                       Avance = 0,
                                       IdUnidad = r.IdUnidad,
                                       Unidad = u.Nombre,
                                       IdArea = r.IdArea == null ? 0 : r.IdArea,
                                       Area = r.IdArea == null ? "" : (from area in db.CatAreasNegocio where area.Id == r.IdArea select area.Nombre).FirstOrDefault(),
                                       FechaRegistro = null,
                                       Bandera = 0
                                   }).ToList();

                    var _listaAvanceR = lista2
                        .GroupBy(l => l.IdProyecto)
                        .Select(x => new AvanceRealList
                        {
                            IdProyecto = x.First().IdProyecto,
                            NompreProyecto = x.First().NompreProyecto,
                            Horas = x.Sum(d => d.Horas),
                            Avance = x.First().Avance,
                            IdUnidad = x.First().IdUnidad,
                            Unidad = x.First().Unidad,
                            IdArea = x.First().IdArea,
                            Area = x.First().Area,
                            FechaRegistro = x.First().FechaRegistro,
                            Bandera = x.First().Bandera
                        }).ToList();


                    var Elementos = (from t in _listaAvanceR where !listaAvanceR.Any(x => x.IdProyecto == t.IdProyecto) select t).ToList();
                    listaAvanceR = listaAvanceR.Concat(Elementos).ToList();

                }
                catch (Exception e)
                {
                    var error = e.Message;
                }
            }

            return listaAvanceR;
        }
        public int InsertaAvanceReal(List<AvanceRealList> lista)
        {
            int result = 0;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    foreach (var item in lista)
                    {
                        AvanceReal avance = new AvanceReal();

                        avance.IdProyecto = item.IdProyecto;
                        avance.AvanceReal1 = item.AvanceReal;
                        avance.FechaRegistro = DateTime.Now;
                        db.AvanceReal.Add(avance);

                        AvanceRealH avanceH = new AvanceRealH();

                        avanceH.IdProyecto = item.IdProyecto;
                        avanceH.AvanceReal = item.AvanceReal;
                        avanceH.FechaRegistro = DateTime.Now;

                        db.AvanceRealH.Add(avanceH);

                        db.SaveChanges();
                        result++;
                    }
                }
            }
            catch (Exception e)
            {
                string res = e.Message;
                result = -1;
            }

            return result;
        }
        public int ActualizaAvanceReal(List<AvanceRealList> Avance)
        {
            int result = 0;
            try
            {
                List<AvanceRealH> listaConsecutivo = new List<AvanceRealH>();

                using (BitacoraContext db = new BitacoraContext())
                {
                    var query = (from a in db.AvanceReal
                                 join ar in Avance on a.IdProyecto equals ar.IdProyecto
                                 select a).ToList();


                    foreach (var item in query)
                    {

                        item.AvanceReal1 = (from x in Avance where x.IdProyecto == item.IdProyecto select x.AvanceReal).FirstOrDefault();
                        item.FechaRegistro = DateTime.Now;

                        AvanceRealH avanceH = new AvanceRealH();

                        avanceH.IdProyecto = item.IdProyecto;
                        avanceH.AvanceReal = item.AvanceReal1;
                        avanceH.FechaRegistro = DateTime.Now;
                        db.AvanceRealH.Add(avanceH);

                        db.SaveChanges();
                        result++;
                    }

                }
            }
            catch (Exception e)
            {
                var error = e.Message;
                result = -1;
            }

            return result;
        }


        public List<CatUnidadesNegocios> UnidadesRelacionadas(int idUser)
        {
            List<CatUnidadesNegocios> listaUnidades = new List<CatUnidadesNegocios>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaUnidades = (from r in db.RelacionUsuarioUnidadArea
                                     join u in db.CatUnidadesNegocios on r.IdUnidad equals u.Id
                                     where u.Estatus == "1" && r.IdUser == idUser
                                     select new CatUnidadesNegocios
                                     {
                                         Id = u.Id,
                                         Nombre = u.Nombre
                                     }).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaUnidades;
        }

        public List<CatAreasNegocio> AreasRelacionadas(int idUser)
        {
            List<CatAreasNegocio> listaAreas = new List<CatAreasNegocio>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaAreas = (from r in db.RelacionUsuarioUnidadArea
                                  join a in db.CatAreasNegocio on r.IdArea equals a.Id
                                  where a.Estatus == "1" && r.IdUser == idUser
                                  select new CatAreasNegocio
                                  {
                                      Id = a.Id,
                                      Nombre = a.Nombre
                                  }).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaAreas;
        }

        public List<CatAreasNegocio> AreasRelacionadasUnidad(int idUser, int idUnidad)
        {
            List<CatAreasNegocio> listaAreas = new List<CatAreasNegocio>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaAreas = (from r in db.RelacionUsuarioUnidadArea
                                  join a in db.CatAreasNegocio on r.IdArea equals a.Id
                                  where a.Estatus == "1" && r.IdUser == idUser && r.IdUnidad == idUnidad
                                  select new CatAreasNegocio
                                  {
                                      Id = a.Id,
                                      Nombre = a.Nombre
                                  }).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaAreas;
        }

        public List<CatUnidadesNegocios> UnidadesRelacionadasArea(int idUser, int idArea)
        {
            List<CatUnidadesNegocios> listaunidades = new List<CatUnidadesNegocios>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaunidades = (from r in db.RelacionUsuarioUnidadArea
                                     join a in db.CatUnidadesNegocios on r.IdArea equals a.Id
                                     where a.Estatus == "1" && r.IdUser == idUser && r.IdArea == idArea
                                     select new CatUnidadesNegocios
                                     {
                                         Id = a.Id,
                                         Nombre = a.Nombre
                                     }).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaunidades;
        }
    }
}
