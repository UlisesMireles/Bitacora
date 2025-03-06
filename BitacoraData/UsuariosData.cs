using System;
using System.Collections.Generic;
using System.Text;
using BitacoraModels;
using System.Linq;
using log4net;

namespace BitacoraData
{
    public class UsuariosData
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(UsuariosData));
        public List<Usuarios> ConsultaUsuarios()
        {
            _log.Info("Bitacora Data antes de consulta... Linea 16");
            List<Usuarios> listaUsuarios = new List<Usuarios>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaUsuarios = (from u in db.CatUsuarios
                                     join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                                     join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                     join ro in db.CatRoles on u.IdRol equals ro.Id
                                     join ee in db.TblCatEstatusEmpleado on e.EstatusERT equals ee.Id
                                     select new Usuarios
                                     {
                                         IdUser = u.Id,
                                         Usuario = u.Usuario,
                                         Nombre = (e.Nombre + ' ' + e.ApellidoPaterno + ' ' + e.ApellidoMaterno),
                                         Rol = ro.Nombre,
                                         Estatus = e.Estatus == "0" ? "Empleado Inactivo": ((u.Estatus == "0" ? "Inactivo" : u.Estatus == "1" ? "Activo" : "")),
                                         IdRelacion = r.Id,
                                         EstausEmpleado = e.Estatus,
                                         IdUnidad = e.IdUnidad,
                                         Email = e.EmailInterno,
                                         EmailAsignado = e.EmailAsignado != null ? e.EmailAsignado : "",
                                         EstatusERT = ee.Descripcion,
                                         Registro = ((from rp in db.RelacionProyectoEmpleado
                                                      join rps in db.RelacionProyectos on rp.IdProyecto equals rps.IdProyecto
                                                      where rp.IdEmpleado == e.Id && rp.IdRol == 10 && rps.Estatus == "1" select rp).Count() > 0 ? 2 : (u.RegistroBitacora == 1 ? 1 : 0)),
                                     }).Distinct().Union
                                  (from u in db.CatUsuarios
                                   join r in db.RelacionUsuarioEmail on u.Id equals r.IdUser
                                   join ro in db.CatRoles on u.IdRol equals ro.Id
                                   select new Usuarios
                                   {
                                       IdUser = u.Id,
                                       Usuario = u.Usuario,
                                       Nombre = r.Nombre,
                                       Rol = ro.Nombre,
                                       Estatus = (u.Estatus == "0" ? "Inactivo" : u.Estatus == "1" ? "Activo" : ""),
                                       IdRelacion = 0,
                                       EstausEmpleado = "",
                                       IdUnidad = 0,
                                       Email = r.Email,
                                       EmailAsignado = "",
                                       EstatusERT = "",
                                       Registro = (u.RegistroBitacora == 1 ? 1 : 0)
                                   }).Distinct().Union
                                  (from u in db.CatUsuarios
                                   join ro in db.CatRoles on u.IdRol equals ro.Id
                                   join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser into ps                                   
                                   from r in ps.DefaultIfEmpty()
                                   where r.Id == null
                                   join re in db.RelacionUsuarioEmail on u.Id equals re.IdUser into pr
                                   from re in pr.DefaultIfEmpty()
                                   where re.Id == null
                                   select new Usuarios
                                   {
                                       IdUser = u.Id,
                                       Usuario = u.Usuario,
                                       Nombre = null,
                                       Rol = ro.Nombre,
                                       Estatus = (u.Estatus == "0" ? "Inactivo" : u.Estatus == "1" ? "Activo" : ""),
                                       IdRelacion = 0,
                                       EstausEmpleado = "",
                                       IdUnidad = 0,
                                       Email = "",
                                       EmailAsignado = "",
                                       EstatusERT = "",
                                       Registro = (u.RegistroBitacora == 1 ? 1 : 0)
                                   }).Distinct().ToList();


                    

                        // query here ....  
                        var query = (from u in db.CatUsuarios
                                     join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                                     join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                     join ro in db.CatRoles on u.IdRol equals ro.Id
                                     join ee in db.TblCatEstatusEmpleado on e.EstatusERT equals ee.Id
                                     select new Usuarios
                                     {
                                         IdUser = u.Id,
                                         Usuario = u.Usuario,
                                         Nombre = (e.Nombre + ' ' + e.ApellidoPaterno + ' ' + e.ApellidoMaterno),
                                         Rol = ro.Nombre,
                                         Estatus = e.Estatus == "0" ? "Empleado Inactivo" : ((u.Estatus == "0" ? "Inactivo" : u.Estatus == "1" ? "Activo" : "")),
                                         IdRelacion = r.Id,
                                         EstausEmpleado = e.Estatus,
                                         IdUnidad = e.IdUnidad,
                                         Email = e.EmailInterno,
                                         EmailAsignado = e.EmailAsignado != null ? e.EmailAsignado : "",
                                         EstatusERT = ee.Descripcion,
                                         Registro = ((from rp in db.RelacionProyectoEmpleado
                                                      join rps in db.RelacionProyectos on rp.IdProyecto equals rps.IdProyecto
                                                      where rp.IdEmpleado == e.Id && rp.IdRol == 10 && rps.Estatus == "1"
                                                      select rp).Count() > 0 ? 2 : (u.RegistroBitacora == 1 ? 1 : 0)),
                                     }).Distinct().Union
                                  (from u in db.CatUsuarios
                                   join r in db.RelacionUsuarioEmail on u.Id equals r.IdUser
                                   join ro in db.CatRoles on u.IdRol equals ro.Id
                                   select new Usuarios
                                   {
                                       IdUser = u.Id,
                                       Usuario = u.Usuario,
                                       Nombre = r.Nombre,
                                       Rol = ro.Nombre,
                                       Estatus = (u.Estatus == "0" ? "Inactivo" : u.Estatus == "1" ? "Activo" : ""),
                                       IdRelacion = 0,
                                       EstausEmpleado = "",
                                       IdUnidad = 0,
                                       Email = r.Email,
                                       EmailAsignado = "",
                                       EstatusERT = "",
                                       Registro = (u.RegistroBitacora == 1 ? 1 : 0)
                                   }).Distinct().Union
                                  (from u in db.CatUsuarios
                                   join ro in db.CatRoles on u.IdRol equals ro.Id
                                   join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser into ps
                                   from r in ps.DefaultIfEmpty()
                                   where r.Id == null
                                   join re in db.RelacionUsuarioEmail on u.Id equals re.IdUser into pr
                                   from re in pr.DefaultIfEmpty()
                                   where re.Id == null
                                   select new Usuarios
                                   {
                                       IdUser = u.Id,
                                       Usuario = u.Usuario,
                                       Nombre = null,
                                       Rol = ro.Nombre,
                                       Estatus = (u.Estatus == "0" ? "Inactivo" : u.Estatus == "1" ? "Activo" : ""),
                                       IdRelacion = 0,
                                       EstausEmpleado = "",
                                       IdUnidad = 0,
                                       Email = "",
                                       EmailAsignado = "",
                                       EstatusERT = "",
                                       Registro = (u.RegistroBitacora == 1 ? 1 : 0)
                                   }).Distinct();

                }
            }
            catch (Exception e)
            {
                string result = e.Message;
                _log.Info("Bitacora Data error catch: " + e.Message);
            }
            _log.Info("Cantidad de registros: " + listaUsuarios.Count());
            _log.Info("Bitacora Data despues de consulta, retorno de lista usuarios");
            return listaUsuarios.Distinct(new CompareUser()).ToList();
            
        }

        public List<CatEmpleados> ConsultaEmpleados()
        {
            List<CatEmpleados> listaEmpleados = new List<CatEmpleados>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaEmpleados = (from e in db.CatEmpleados
                                      where e.Estatus == "1"
                                      select new CatEmpleados { Id = e.Id, Nombre = e.Nombre + " " + e.ApellidoPaterno + " " + e.ApellidoMaterno}).ToList();


                }
            }
            catch (Exception e)
            {
                string Error = e.Message;
            }

            return listaEmpleados;
        }


        public List<CatRoles> ConsultaRoles()
        {
            List<CatRoles> Roles = new List<CatRoles>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    Roles = (from r in db.CatRoles where r.Estatus == 1 select r).ToList();


                }
            }
            catch (Exception e)
            {
                string Error = e.Message;
            }

            return Roles;
        }

        public CatEmpleados ConsultaEmpleado(int IdEmpleado)
        {
            CatEmpleados CatEmpleados = new CatEmpleados();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    CatEmpleados = (from r in db.CatEmpleados where r.Id == IdEmpleado select r).FirstOrDefault();

                }
            }
            catch (Exception e)
            {
                string Error = e.Message;
            }

            return CatEmpleados;
        }


        public CatEmpleados GeneraUsuario(int idEmpleado)
        {
            CatEmpleados usuario = new CatEmpleados();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    usuario = (from e in db.CatEmpleados where e.Id == idEmpleado select e).FirstOrDefault();


                }
            }
            catch (Exception e)
            {
                string Error = e.Message;
            }

            return usuario;
        }
        public int InsertaUsuario(CatUsuarios usuario)
        {
            int result;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    db.CatUsuarios.Add(usuario);
                    db.SaveChanges();
                    result = usuario.Id;
                }
            }
            catch (Exception e)
            {
                string res = e.Message;
                result = -1;
                using (BitacoraContext db = new BitacoraContext())
                {
                    var cont = (from u in db.CatUsuarios where u.Usuario == usuario.Usuario select u).Count();

                    if(cont > 0)
                        result = -2;
                }
            }

            return result;
        }

        public int InsertaRelacionUE(RelacionUsuarioEmpleado relacion)
        {
            int result;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    db.RelacionUsuarioEmpleado.Add(relacion);
                    db.SaveChanges();
                    result = relacion.Id;
                }
            }
            catch (Exception e)
            {
                string res = e.Message;
                result = -1;               
            }

            return result;
        }

        public int InsertaRelacionUEmail(RelacionUsuarioEmail relacion)
        {
            int result;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    db.RelacionUsuarioEmail.Add(relacion);
                    db.SaveChanges();
                    result = relacion.Id;
                }
            }
            catch (Exception e)
            {
                string res = e.Message;
                result = -1;
            }

            return result;
        }

        public int InsertaRelacionUserUnArea(List<RelacionUsuarioUnidadArea> relacion, int idUser)
        {
            int result;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    foreach (RelacionUsuarioUnidadArea item in relacion)
                    {
                        item.IdUser = idUser;
                        db.RelacionUsuarioUnidadArea.Add(item);
                        db.SaveChanges();
                    }
                    result = 1;
                }
            }
            catch (Exception e)
            {
                string res = e.Message;
                result = -1;
            }

            return result;
        }

        public int ActualizaRelacionUserUnArea(List<RelacionUsuarioUnidadArea> relacion, int idUser)
        {
            int result;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    var list = (from r in db.RelacionUsuarioUnidadArea where r.IdUser == idUser select r).ToList();
                    db.RelacionUsuarioUnidadArea.RemoveRange(list);
                    db.SaveChanges();

                    InsertaRelacionUserUnArea(relacion, idUser);

                    result = 1;
                }
            }
            catch (Exception e)
            {
                string res = e.Message;
                result = -1;
            }

            return result;
        }


        public int ModificaUsuario(Usuarios datos)
        {
            int IdUser = 0;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    CatUsuarios registro = new CatUsuarios();

                    registro = (from u in db.CatUsuarios where u.Id == datos.IdUser select u).FirstOrDefault();

                    if (registro != null)
                    {
                        registro.Estatus = datos.Estatus == null ? registro.Estatus : datos.Estatus;
                        registro.IdRol = datos.IdRol < 1 ? registro.IdRol : datos.IdRol;
                        registro.Password = datos.Password == null ? registro.Password : datos.Password;
                        registro.FechaModificacion = DateTime.Now;
                        registro.IdUsrModificacion = datos.IdUsrModificacion;
                        registro.RegistroBitacora = datos.Registro > 1 ? registro.RegistroBitacora : datos.Registro;

                        db.SaveChanges();
                        IdUser = registro.Id;
                    }

                    var relacion = (from r in db.RelacionUsuarioEmail where r.IdUser == datos.IdUser select r).FirstOrDefault();

                    if (relacion != null)
                    {
                        relacion.Email = datos.Email;
                        relacion.Nombre = datos.Nombre; 

                        db.SaveChanges();
                    }
                    else
                    {
                        RelacionUsuarioEmail ObjRelacion = new RelacionUsuarioEmail();

                        ObjRelacion.IdUser = datos.IdUser;
                        ObjRelacion.Email = datos.Email;
                        ObjRelacion.Nombre = datos.Nombre;
                        var rel = InsertaRelacionUEmail(relacion);
                    }

                    ActualizaRelacionUserUnArea(datos.ListaUnidadArea, datos.IdUser);
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
                IdUser = -1;
            }
            return IdUser;
        }

        public int BajaUsuario(int idUser)
        {
            int IdUser = 0;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    CatUsuarios registro = new CatUsuarios();

                    registro = (from u in db.CatUsuarios where u.Id == idUser select u).FirstOrDefault();

                    if (registro != null)
                    {
                        registro.Estatus = registro.Estatus == "1" ? "0": "1";
                       
                        db.SaveChanges();
                        IdUser = registro.Id;
                    }
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
                IdUser = -1;
            }
            return IdUser;
        }

        public string ConsultaEmail(int idUser)
        {
            string Email;
            try {
                using (BitacoraContext db = new BitacoraContext())
                {
                    Email = (from e in db.CatEmpleados
                             join r in db.RelacionUsuarioEmpleado on e.Id equals r.IdEmpleado
                             join u in db.CatUsuarios on r.IdUser equals u.Id
                             where u.Id == idUser
                             select e.EmailInterno).FirstOrDefault();

                    if (Email == null)
                    {
                        Email = (from r in db.RelacionUsuarioEmail
                                 join u in db.CatUsuarios on r.IdUser equals u.Id
                                 where u.Id == idUser
                                 select r.Email).FirstOrDefault();
                    }
                }
            }
            catch (Exception e)
            {
                string Error = e.Message;
                Email = "-1";
            }

            return Email;
        }

        public int RestablecePassword(int idUser, string password)
        {
            int IdUser = 0;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    CatUsuarios registro = new CatUsuarios();

                    registro = (from u in db.CatUsuarios where u.Id == idUser select u).FirstOrDefault();

                    if (registro != null)
                    {
                        registro.Password = password;
                        registro.Temporal = 2;
                        registro.FechaModificacion = DateTime.Now;

                        db.SaveChanges();
                        IdUser = registro.Id;
                    }
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
                IdUser = -1;
            }
            return IdUser;
        }

        public string ConsultaUsuario(int idUser)
        {
            string Usuario;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    Usuario = (from u in db.CatUsuarios
                               where u.Id == idUser
                             select u.Usuario).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                string Error = e.Message;
                Usuario = "";
            }

            return Usuario;
        }

        public List<RelacionUsuarioUnidadArea> ConsultaRel_UserUnArea(int idUser)
        {
            List<RelacionUsuarioUnidadArea> ListaRelacion = new List<RelacionUsuarioUnidadArea>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    ListaRelacion = (from r in db.RelacionUsuarioUnidadArea
                                    where r.IdUser == idUser
                                    select r).ToList();
                }
            }
            catch (Exception e)
            {
                string Error = e.Message;
            }

            return ListaRelacion;
        }

        public CatUsuarios Consulta_UsuarioPassword(int Id)
        {
            CatUsuarios usuario = new CatUsuarios();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    usuario = (from u in db.CatUsuarios
                               where u.Id == Id
                               select u).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                string Error = e.Message;
                usuario = null;
            }
            return usuario;
        }



        ///obtengo roles usando la consulta d elos usurios 
        ///
        public List<ComboRolesUsuarios> ConsultaRolesUsuarios()
        {
            _log.Info("Bitacora Data ConsultaRolesUsuarios antes de consulta... roles de usuarios");
            List<ComboRolesUsuarios> listaRolesUsuarios = new List<ComboRolesUsuarios>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaRolesUsuarios = (from u in db.CatUsuarios
                                     join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                                     join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                     join ro in db.CatRoles on u.IdRol equals ro.Id
                                     join ee in db.TblCatEstatusEmpleado on e.EstatusERT equals ee.Id
                                     select new ComboRolesUsuarios
                                     {
                                         IdRol = u.IdRol,
                                         Rol = ro.Nombre
                                      
                                     }).Distinct().Union
                                  (from u in db.CatUsuarios
                                   join r in db.RelacionUsuarioEmail on u.Id equals r.IdUser
                                   join ro in db.CatRoles on u.IdRol equals ro.Id
                                   select new ComboRolesUsuarios
                                   {
                                       IdRol = u.IdRol,
                                       Rol = ro.Nombre
                                   }).Distinct().Union
                                  (from u in db.CatUsuarios
                                   join ro in db.CatRoles on u.IdRol equals ro.Id
                                   join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser into ps
                                   from r in ps.DefaultIfEmpty()
                                   where r.Id == null
                                   join re in db.RelacionUsuarioEmail on u.Id equals re.IdUser into pr
                                   from re in pr.DefaultIfEmpty()
                                   where re.Id == null
                                   select new ComboRolesUsuarios
                                   {
                                       IdRol = u.IdRol,
                                       Rol = ro.Nombre
                                   }).Distinct().ToList();




                    // query here ....  
                    var query = (from u in db.CatUsuarios
                                 join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                                 join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                 join ro in db.CatRoles on u.IdRol equals ro.Id
                                 join ee in db.TblCatEstatusEmpleado on e.EstatusERT equals ee.Id
                                 select new ComboRolesUsuarios
                                 {
                                     IdRol = u.IdRol,
                                     Rol = ro.Nombre
                                 }).Distinct().Union
                              (from u in db.CatUsuarios
                               join r in db.RelacionUsuarioEmail on u.Id equals r.IdUser
                               join ro in db.CatRoles on u.IdRol equals ro.Id
                               select new ComboRolesUsuarios
                               {
                                   IdRol = u.IdRol,
                                   Rol = ro.Nombre
                               }).Distinct().Union
                              (from u in db.CatUsuarios
                               join ro in db.CatRoles on u.IdRol equals ro.Id
                               join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser into ps
                               from r in ps.DefaultIfEmpty()
                               where r.Id == null
                               join re in db.RelacionUsuarioEmail on u.Id equals re.IdUser into pr
                               from re in pr.DefaultIfEmpty()
                               where re.Id == null
                               select new ComboRolesUsuarios
                               {
                                   IdRol = u.IdRol,
                                   Rol = ro.Nombre
                               }).Distinct();

                }
            }
            catch (Exception e)
            {
                string result = e.Message;
                _log.Info("Bitacora Data error catch: " + e.Message);
            }
            _log.Info("Cantidad de registros: " + listaRolesUsuarios.Count());
            _log.Info("Bitacora Data despues de consulta, retorno de lista usuarios");

            return listaRolesUsuarios.DistinctBy(x => x.IdRol).ToList();

        }




        //combo Estatus ERT

        public List<ComboEstatusERTUsuarios> ConsultaEstatusERTUsuarios()
        {
            _log.Info("Bitacora Data ConsultaRolesUsuarios antes de consulta... roles de usuarios");
            List<ComboEstatusERTUsuarios> listaEstatusERTUsuarios = new List<ComboEstatusERTUsuarios>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    listaEstatusERTUsuarios = (from u in db.CatUsuarios
                                          join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                                          join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                          join ro in db.CatRoles on u.IdRol equals ro.Id
                                          join ee in db.TblCatEstatusEmpleado on e.EstatusERT equals ee.Id
                                          select new ComboEstatusERTUsuarios
                                          {
                                              EstatusERT = ee.Descripcion

                                          }).Distinct().Union
                                  (from u in db.CatUsuarios
                                   join r in db.RelacionUsuarioEmail on u.Id equals r.IdUser
                                   join ro in db.CatRoles on u.IdRol equals ro.Id
                                   select new ComboEstatusERTUsuarios
                                   {
                                       EstatusERT = ""
                                   }).Distinct().Union
                                  (from u in db.CatUsuarios
                                   join ro in db.CatRoles on u.IdRol equals ro.Id
                                   join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser into ps
                                   from r in ps.DefaultIfEmpty()
                                   where r.Id == null
                                   join re in db.RelacionUsuarioEmail on u.Id equals re.IdUser into pr
                                   from re in pr.DefaultIfEmpty()
                                   where re.Id == null
                                   select new ComboEstatusERTUsuarios
                                   {
                                       EstatusERT = ""
                                   }).Distinct().ToList();




                    // query here ....  
                    var query = (from u in db.CatUsuarios
                                 join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                                 join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                                 join ro in db.CatRoles on u.IdRol equals ro.Id
                                 join ee in db.TblCatEstatusEmpleado on e.EstatusERT equals ee.Id
                                 select new ComboEstatusERTUsuarios
                                 {
                                     EstatusERT = ee.Descripcion
                                 }).Distinct().Union
                              (from u in db.CatUsuarios
                               join r in db.RelacionUsuarioEmail on u.Id equals r.IdUser
                               join ro in db.CatRoles on u.IdRol equals ro.Id
                               select new ComboEstatusERTUsuarios
                               {
                                   EstatusERT = ""
                               }).Distinct().Union
                              (from u in db.CatUsuarios
                               join ro in db.CatRoles on u.IdRol equals ro.Id
                               join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser into ps
                               from r in ps.DefaultIfEmpty()
                               where r.Id == null
                               join re in db.RelacionUsuarioEmail on u.Id equals re.IdUser into pr
                               from re in pr.DefaultIfEmpty()
                               where re.Id == null
                               select new ComboEstatusERTUsuarios
                               {
                                   EstatusERT = ""
                               }).Distinct();

                }
            }
            catch (Exception e)
            {
                string result = e.Message;
                _log.Info("Bitacora Data error catch: " + e.Message);
            }
            _log.Info("Cantidad de registros: " + listaEstatusERTUsuarios.Count());
            _log.Info("Bitacora Data despues de consulta, retorno de lista usuarios");

            return listaEstatusERTUsuarios.DistinctBy(x => x.EstatusERT).ToList();

        }
    }
}
