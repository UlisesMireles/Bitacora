using System;
using System.Collections.Generic;
using System.Text;
using BitacoraModels;
using System.Linq;
using BitacoraData;
using System.Web;

namespace BitacoraLogic
{
    public class UsuariosLogic
    {
        UsuariosData _UsuariosData = new UsuariosData();
        LoginLogic _login = new LoginLogic();
        public List<Usuarios> ConsultaUsuarios()
        {
            List<Usuarios> listaUsuarios = new List<Usuarios>();
            try
            {
                listaUsuarios = _UsuariosData.ConsultaUsuarios();
                listaUsuarios = (from x in listaUsuarios orderby x.Estatus, x.Usuario select x).ToList();

            }
            catch (Exception e)
            {
                string result = e.Message;
            }
            return listaUsuarios;
        }
        public List<CatEmpleados> ConsultaEmpleados()
        {
            List<CatEmpleados> listaEmpleados = new List<CatEmpleados>();
            try
            {
                listaEmpleados = _UsuariosData.ConsultaEmpleados();
                listaEmpleados = (from x in listaEmpleados orderby x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaEmpleados;
        }

        public List<CatRoles> ConsultaRoles()
        {
            List<CatRoles> Roles= new List<CatRoles>();
            try
            {
                Roles = _UsuariosData.ConsultaRoles();
                Roles = (from x in Roles orderby x.Descripcion select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return Roles;
        }

        public string GeneraUsuario(int idEmpleado)
        {
            string Usuario = "";
            try
            {
                var result = _UsuariosData.GeneraUsuario(idEmpleado);
                string[] nombres = result.Nombre.Split(' '); 
                string apellido =  result.ApellidoPaterno.Replace(" ", "");

                Usuario = nombres[0] + "." + apellido;
                var temporal = Usuario;
                var validacion = _login.Autenticacion(Usuario, "");
                
                if (validacion.IdUser > 0)
                {
                    int consecutivo = 0;
                    do
                    {
                        temporal = Usuario + (consecutivo + 1).ToString();
                        validacion = _login.Autenticacion(temporal, "");
                        consecutivo  ++;
                    }
                    while (validacion.IdUser > 0);
                    Usuario = temporal;
                }
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return Usuario.ToLower();
        }

        public int InsertaUsuario(Usuarios datos)
        {
            int usuario;
            var password = "";
            var bandera = true;

            int idEmpleado = datos.IdEmpleado;
            string email = idEmpleado == 0 ? datos.Email: "";
            string nombre = idEmpleado == 0 ? datos.Nombre: "";

            CatUsuarios datosUsuario = new CatUsuarios();

            datosUsuario.Usuario = datos.Usuario;
            datosUsuario.IdRol = datos.IdRol;
            datosUsuario.IdUsrRegistro = datos.IdUsrRegistro;

            try
            {
                if ((datos.Usuario.Trim()).Length < 4)
                    bandera = false;
                if (datos.IdRol < 1)
                    bandera = false;
                if (datos.IdUsrRegistro < 1 || datos.IdUsrRegistro == null)
                    bandera = false;
                if (((email.Trim()).Length < 10 ) && ((nombre.Trim()).Length < 2) && idEmpleado == 0)
                    bandera = false;
                if (bandera == true)
                {
                    password = _login.GeneraPassword();
                    var passwordEncrypt = _login.Encrypt(password);
                    datosUsuario.Estatus = "1";
                    datosUsuario.FechaRegistro = DateTime.Today;
                    datosUsuario.FechaModificacion = DateTime.Today;
                    datosUsuario.IdUsrModificacion = null;
                    datosUsuario.IdUsrElimino = null;
                    datosUsuario.Temporal = 1;
                    datosUsuario.Password = passwordEncrypt;
                    datosUsuario.RegistroBitacora = datos.Registro;
                    usuario = _UsuariosData.InsertaUsuario(datosUsuario);                    
                }
                else
                    usuario = 0;

                if (usuario > 0)
                {
                    var res = _UsuariosData.InsertaRelacionUserUnArea(datos.ListaUnidadArea, usuario);
                    if (idEmpleado > 0)
                    {
                        RelacionUsuarioEmpleado relacion = new RelacionUsuarioEmpleado();

                        relacion.IdUser = usuario;
                        relacion.IdEmpleado = idEmpleado;

                        var rel = _UsuariosData.InsertaRelacionUE(relacion);
                        email = _UsuariosData.ConsultaEmail(usuario); 
                    }
                    else
                    {
                        RelacionUsuarioEmail relacion = new RelacionUsuarioEmail();

                        relacion.IdUser = usuario;
                        relacion.Email = email;
                        relacion.Nombre = nombre;
                        var rel = _UsuariosData.InsertaRelacionUEmail(relacion);
                    }

                    CatEmpleados dataEmpleado = _UsuariosData.ConsultaEmpleado(idEmpleado);
                    if(dataEmpleado.EstatusERT == 2)
                    {
                        email = (dataEmpleado.EmailAsignado != null && dataEmpleado.EmailAsignado != "") ? dataEmpleado.EmailAsignado : (dataEmpleado.EmailInterno != null && dataEmpleado.EmailInterno != "") ? dataEmpleado.EmailInterno : dataEmpleado.Email;
                    }
                    else
                    {
                        email = dataEmpleado.EmailInterno != null ? dataEmpleado.EmailInterno : dataEmpleado.Email;
                    }
                    if(datos.IdRol == 19)
                    {

                        usuario = _login.EnvioCorreo(datos.Usuario, password, email, "Nom035") == -1 ? -5 : usuario;
                    }
                    else
                    {
                        usuario = _login.EnvioCorreo(datos.Usuario, password, email, "CU") == -1 ? -5 : usuario;
                    }
                    
                }
               

            }
            catch (Exception e)
            {
                String res = e.Message;
                usuario = -1;
            }

            return usuario;
        }

        public int ModificaUsuario(Usuarios datos)
        {
            int usuario;
            var bandera = true;

            string email = datos.IdEmpleado == 0 ? datos.Email : "";
            string nombre = datos.IdEmpleado == 0 ? datos.Nombre : "";
            try
            {
                if (datos.IdUser < 1)
                    bandera = false;
                if (datos.IdUsrModificacion < 1 || datos.IdUsrModificacion == null)
                    bandera = false;
                if (((email.Trim()).Length < 10) && ((nombre.Trim()).Length < 2) && datos.IdEmpleado == 0)
                    bandera = false;                
                else
                    if (datos.Password != null && datos.Password.Length > 3 && datos.Password.Length < 11)
                        datos.Password = _login.Encrypt(datos.Password);
                usuario = bandera == true ?_UsuariosData.ModificaUsuario(datos) : 0;
            }
            catch (Exception e)
            {
                String res = e.Message;
                usuario = -1;
            }

            return usuario;
        }

        public int BajaUsuario(int idUser)
        {
            int usuario;
            try
            {
                usuario = _UsuariosData.BajaUsuario(idUser);
            }
            catch (Exception e)
            {
                String res = e.Message;
                usuario = -1;
            }

            return usuario;
        }

        public int RestablecePassword(int idUser)
        {
            int idUsuario;
            try
            {
                var usuario = _UsuariosData.ConsultaUsuario(idUser);
                var password = _login.GeneraToken();
                var passwordEncrypt = _login.Encrypt(password);
                idUsuario = _UsuariosData.RestablecePassword(idUser, passwordEncrypt);

                var Email = _UsuariosData.ConsultaEmail(idUsuario);

                idUsuario = _login.EnvioCorreo(usuario, password, Email, "RC") == -1 ? -5 : idUsuario;
            }



            catch (Exception e)
            {
                String res = e.Message;
                idUsuario = -1;
            }

            return idUsuario;
        }
        
        public List<RelacionUsuarioUnidadArea> ConsultaRel_UserUnArea(int idUser)
        {
            List<RelacionUsuarioUnidadArea> ListaRelacion = new List<RelacionUsuarioUnidadArea>();
            try
            {
                ListaRelacion = _UsuariosData.ConsultaRel_UserUnArea(idUser);
            }
            catch (Exception e)
            {
                String res = e.Message;
            }

            return ListaRelacion;
        }
    }
}
