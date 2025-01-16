using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitacoraLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitacoraModels;
using BitacoraData;

namespace Bitacora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class loginController : Controller
    {
        LoginLogic _loginLogic = new LoginLogic();
        LoginData _loginData = new LoginData();

        [HttpGet("[action]/")]
        public Object Autenticacion(string usuario, string password)
        {
            var respuesa = _loginLogic.Autenticacion(usuario, password);
            var permisos = _loginLogic.PermisosUsuario(respuesa.IdUser);

            if (respuesa.Estatus == "1" && respuesa.EstausEmpleado == "1")
            {
                if (respuesa.Password is null)
                    _loginData.InsertaHistorial(respuesa.IdUser, "Login", "Login Fallido", "El Usuario " + respuesa.Usuario + " entró por priera vez al sistema y se receteo su Password");
                if (respuesa.Password == "False")
                    _loginData.InsertaHistorial(respuesa.IdUser, "Login", "Login Fallido", "El Usuario " + respuesa.Usuario + " Ingreso contraseña fallida");
                if (respuesa.Password == "True")
                    _loginData.InsertaHistorial(respuesa.IdUser, "Login", "Login Exitoso", "El Usuario " + respuesa.Usuario + " a iniciado secion exitosamente");
            }
            else
            {
                if (respuesa.IdUser > 0)
                {
                    if (respuesa.Estatus == "1")
                        _loginData.InsertaHistorial(respuesa.IdUser, "Login", "Login Fallido", "El Empleado con usuario en Bitacora " + respuesa.Usuario + " se encuentra dado de baja");
                    else _loginData.InsertaHistorial(respuesa.IdUser, "Login", "Login Fallido", "El Usuario " + respuesa.Usuario + " se encuentra dado de baja");
                }
            }

            return new { respuesta=respuesa, perm = permisos};
        }

        [HttpGet("[action]/{id}")]
        public object ConsultarFoto(string usuario)
        {
            var respuesa = _loginLogic.ConsultarFoto(usuario);

            return new { respuesta = respuesa };
        }

        [HttpGet("[action]/{id}")]
        public List<PermisosPantallas> PermisosUsuario(int iduser)
        {
            var respuesa = _loginLogic.PermisosUsuario(iduser);

            return respuesa;
        }

        [HttpGet("[action]/{id}")]
        public int CambioContrasenia(int idUser, string password)
        {
            int respuesa = _loginLogic.CambioContrasenia(idUser, password);

            return respuesa;
        }

        [HttpGet("[action]/{id}")]
        public int RecuperarContrasenia(string usuario)
        {
            var respuesa = _loginLogic.RecuperarContrasenia(usuario);

            return respuesa;
        }

        [HttpGet("[action]/{id}")]
        public Usuarios ConsultaToken(string usuario, string token)
        {
            var user = "";
            if (usuario.Length < 24)
                user = usuario.PadRight(24, '=');
            if (usuario.Length > 24)
                user = usuario.PadRight(44, '=');
            var respuesa = _loginLogic.Autenticacion(_loginLogic.Decrypt(user), token);

            return respuesa;
        }

        [HttpGet("[action]/{id}")]
        public void logOut(int idUser)
        {
                _loginData.InsertaHistorial(idUser, "LogOut", "Cierre de sesion", "El usuario a cerrado secion");
        }

        [HttpGet("[action]/{id}")]
        public int RecuperarContraseniaTokenCad(string usuario)
        {
            var user = "";
            if (usuario.Length < 24)
                user = usuario.PadRight(24, '=');
            if (usuario.Length > 24)
                user = usuario.PadRight(48, '=');

            usuario = _loginLogic.Decrypt(user);
            var respuesa = _loginLogic.RecuperarContrasenia(usuario);

            return respuesa;
        }


        [HttpGet("[action]/{id}")]
        public Object FechaServidor() 
        {
            var fecha = _loginLogic.ConsultaFecha();
            return new { fechaServ = fecha };
        }
    }
}