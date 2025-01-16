using System;
using System.Collections.Generic;
using System.Linq;
using BitacoraModels;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Threading.Tasks;

namespace BitacoraData
{
    public class LoginData
    {

        public Usuarios Autenticacion(string usuario)
        {
            Usuarios datos = new Usuarios();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    datos = (from u in db.CatUsuarios
                             join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                             join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                             join cr in db.CatRoles on u.IdRol equals cr.Id
                             where u.Usuario == usuario
                             select new Usuarios
                             {
                                 IdUser = u.Id,
                                 Usuario = u.Usuario,
                                 Password = u.Password,
                                 IdRol = u.IdRol,
                                 Rol = cr.Nombre,
                                 Estatus = u.Estatus,
                                 Temporal = Convert.ToInt32(u.Temporal),
                                 FechaModificacion = u.FechaModificacion,
                                 EstausEmpleado = e.Estatus,
                                 Foto = e.Foto != null ? e.Foto : "null"
                             }).First();
                }
            }
            catch (Exception v) {
                try
                {
                    using (BitacoraContext db = new BitacoraContext())
                    {
                        datos = (from u in db.CatUsuarios
                                 join r in db.RelacionUsuarioEmail on u.Id equals r.IdUser
                                 where u.Usuario == usuario
                                 select new Usuarios
                                 {
                                     IdUser = u.Id,
                                     Usuario = u.Usuario,
                                     Password = u.Password,
                                     IdRol = u.IdRol,
                                     Estatus = u.Estatus,
                                     Temporal = Convert.ToInt32(u.Temporal),
                                     FechaModificacion = u.FechaModificacion,
                                     EstausEmpleado = "1"
                                 }).First();
                    }
                }
                catch (Exception e)
                {
                    string result = e.Message;
                    datos.IdUser = -1;
                }
            }

            return datos;
        }
        public string GetImage(string usuario)
        {
            string datos = "null";
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    datos = (from u in db.CatUsuarios
                             join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                             join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                             where u.Usuario == usuario
                             select e.Foto != null ? e.Foto : "null"
                             ).First();
                }
            }
            catch (Exception v)
            {
                string result = v.Message;
                datos = "null";
            }

            return datos;
        }
        public string CambioContrasenia(int idUser, string password, int temporal)
        {
            string result = "ok";
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    CatUsuarios user = (from u in db.CatUsuarios where u.Id == idUser select u).First();
                    user.Password = password;
                    user.FechaModificacion = DateTime.Now;
                    user.Temporal = temporal;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        public string consultaParametro(string clave)
        {
            string valor;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    valor = (from p in db.Parametros where p.Clave == clave select p.Valor).First();
                }
            }
            catch (Exception e)
            {
                var resultado = e.Message;
                valor = "-1";
            }

            return valor;
        }

        public List<PermisosPantallas> PermisosUsuario(int idUser)
        {
            List<PermisosPantallas> permisos = new List<PermisosPantallas>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    permisos = (from p in db.Pantallas
                                join m in db.Menu on p.IdMenu equals m.Id
                                join re in db.RelRolPantalla on p.Id equals re.IdPantalla
                                join r in db.CatRoles on re.IdRol equals r.Id
                                join u in db.CatUsuarios on r.Id equals u.IdRol
                                where u.Id == idUser
                                select new PermisosPantallas
                                {
                                    NombrePantalla = p.Nombre,
                                    NombreMenu = m.Nombre,
                                    Orden = m.orden
                                }).ToList();

                    permisos = (from x in permisos orderby x.Orden select x).ToList();
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return permisos;
        }

        public int InsertaHistorial(int idUser, string pantalla, string accion, string descripcion)
        {
            int result = 1;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    HistorialB registro = new HistorialB();

                    registro.IdUser = idUser;
                    registro.Pantalla = pantalla;
                    registro.Accion = accion;
                    registro.Descripcion = descripcion;
                    registro.Fecha = DateTime.Now;

                    db.HistorialB.Add(registro);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                result = -1;
            }

            return result;
        }

        public async Task<string> ConsultaFechaAsync() {
            using (BitacoraContext db = new BitacoraContext())
            {
                var fecha="";
                //var x = new Fecha();

                // x= db.Fecha.FromSql("SELECT CONVERT(VARCHAR(24), GETDATE(),113) AS Nombre");
                //var res = x.FechaActual;
                var conn = db.Database.GetDbConnection();
                await conn.OpenAsync();
                var command = conn.CreateCommand();
                const string query = "SELECT CONVERT(VARCHAR(24), GETDATE(),120) AS Nombre";
                command.CommandText = query;
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    fecha = reader.GetString(0);
                }


                return fecha;
            }
            
           
        }
    }
}


