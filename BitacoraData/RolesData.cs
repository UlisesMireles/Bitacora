using System;
using System.Collections.Generic;
using System.Text;
using BitacoraModels;
using System.Linq;

namespace BitacoraData
{
    public class RolesData
    {

        public List<Roles> ConsultaRoles()
        {
            List<Roles> listaRoles= new List<Roles>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaRoles = (from r in db.CatRoles
                                  select new Roles
                                  {
                                      IdRol = r.Id,
                                      Rol = r.Nombre,
                                      Descripcion = r.Descripcion,
                                      Estatus = (r.Estatus == 0 ? "Inactivo" : r.Estatus == 1 ? "Activo" : ""),
                                      FechaRegistro = r.FechaRegistro,
                                      FechaModificacion = r.FechaModificacion,
                                      ListPantallas = (from p in db.Pantallas 
                                                       join rel in db.RelRolPantalla on p.Id equals rel.IdPantalla
                                                       where rel.IdRol == r.Id select p.Nombre).ToList()
                                  }).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaRoles;
        }

        public List<PermisosPantallas> ConsultaPantallas()
        {
            List<PermisosPantallas> pantallas = new List<PermisosPantallas>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    pantallas = (from p in db.Pantallas
                                join m in db.Menu on p.IdMenu equals m.Id
                                select new PermisosPantallas
                                {
                                    IdPantalla = p.Id,
                                    NombrePantalla = p.Nombre,
                                    NombreMenu = m.Nombre,
                                    Orden = m.orden
                                }).ToList();

                    pantallas = (from x in pantallas orderby x.Orden select x).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return pantallas;
        }

        public int InsertaRol(CatRoles rol)
        {
            int result;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    db.CatRoles.Add(rol);
                    db.SaveChanges();
                    result = rol.Id;
                }
            }
            catch (Exception e)
            {
                string res = e.Message;
                result = -1;
                using (BitacoraContext db = new BitacoraContext())
                {
                    var cont = (from r in db.CatRoles where r.Nombre == rol.Nombre select r).Count();

                    if (cont > 0)
                        result = -2;
                }
            }

            return result;
        }

        public int InsertaRolPantalla(int idRol, List<int> idPantallas)
        {
            int result = 1;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    foreach (int x in idPantallas)
                    {
                        RelacionRolPantalla registro = new RelacionRolPantalla();
                        registro.IdPantalla = x;
                        registro.IdRol = idRol;

                        db.RelRolPantalla.Add(registro);
                        db.SaveChanges();
                    }                   
                }
            }
            catch (Exception e)
            {
                result = -1;               
            }

            return result;
        }

        public List<PermisosPantallas> ConsultaPantallasRol(int idRol)
        {
            List<PermisosPantallas> pantallas = new List<PermisosPantallas>();
            List<PermisosPantallas> pantallasS = new List<PermisosPantallas>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    pantallas = (from p in db.Pantallas
                                 join m in db.Menu on p.IdMenu equals m.Id
                                 join r in db.RelRolPantalla on p.Id equals r.IdPantalla
                                 where r.IdRol == idRol
                                 select new PermisosPantallas
                                 {
                                     IdPantalla = p.Id,
                                     NombrePantalla = p.Nombre,
                                     NombreMenu = m.Nombre,
                                     Orden = m.orden,
                                     Asignada = 1
                                 }).ToList();

                    var pantallasNoAsignadas = (from p in db.Pantallas
                                                join m in db.Menu on p.IdMenu equals m.Id
                                                select new PermisosPantallas
                                                {
                                                    IdPantalla = p.Id,
                                                    NombrePantalla = p.Nombre,
                                                    NombreMenu = m.Nombre,
                                                    Orden = m.orden,
                                                    Asignada = 0
                                                }).ToList();

                    pantallasS = pantallasNoAsignadas.Where(tp => !pantallas.Any(pa => pa.IdPantalla == tp.IdPantalla)).ToList();

                    pantallas = pantallas
                                .Concat(pantallasS)
                                .OrderBy(x => x.Asignada)
                                .ThenBy(x => x.Orden)
                                .ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return pantallas;
        }

        public int ModificaRol(Roles datos)
        {
            int IdRol = 0;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    CatRoles registro = new CatRoles();

                    registro = (from r in db.CatRoles where r.Id == datos.IdRol select r).FirstOrDefault();

                    if (registro != null)
                    {
                        registro.Descripcion = datos.Descripcion == null ? registro.Descripcion : datos.Descripcion;
                        registro.Estatus = datos.Estatus == "Activo" ? 1 : 0;
                        registro.FechaModificacion = DateTime.Now;

                        db.SaveChanges();
                        IdRol = registro.Id;
                    }

                    var rp = (from rpr in db.RelRolPantalla where rpr.IdRol == datos.IdRol select rpr).ToList();
                    db.RelRolPantalla.RemoveRange(rp);
                    db.SaveChanges();

                    foreach (int x in datos.IdPantallas)
                    {
                        RelacionRolPantalla registroP = new RelacionRolPantalla();
                        registroP.IdPantalla = x;
                        registroP.IdRol = IdRol;

                        db.RelRolPantalla.Add(registroP);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
                IdRol = -1;
            }
            return IdRol;
        }

        public int EliminaRol(int idRol)
        {
            int IdRol = 0;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    CatRoles registro = new CatRoles();

                    var rolesAsigandos = (from u in db.CatUsuarios where u.IdRol == idRol select u).Count();

                    if (rolesAsigandos > 0)
                        IdRol = -2;
                    else
                    {
                        var rp = (from rpr in db.RelRolPantalla where rpr.IdRol == idRol select rpr).ToList();
                        db.RelRolPantalla.RemoveRange(rp);
                        db.SaveChanges();

                        var r = (from cr in db.CatRoles where cr.Id == idRol select cr).FirstOrDefault();
                        db.CatRoles.Remove(r);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
                IdRol = -1;
            }
            return IdRol;
        }
    }
}
