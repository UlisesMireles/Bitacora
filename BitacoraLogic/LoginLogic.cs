using BitacoraData;
using BitacoraModels;
using j.GAM.Utility;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace BitacoraLogic
{
    public class LoginLogic
    {
        LoginData _loginData = new LoginData();
        UsuariosData _UsuariosData = new UsuariosData();

        public Usuarios Autenticacion(string usuario, string password)
        {
           
            var user = _loginData.Autenticacion(usuario);

            if(user.Password != null)
                user.Password = user.Password == Encrypt(password.Trim()) ? "True" : "False";
           
            return user;
        }

        public string ConsultarFoto(string usuario)
        {
            var foto = _loginData.GetImage(usuario); 
            return foto;
        }
        public List<PermisosPantallas> PermisosUsuario(int iduser)
        {
            var permisos = _loginData.PermisosUsuario(iduser);

            return permisos;
        }

        public string GeneraPassword()
        {
            string cadena = "";
            
            for (int x = 0; x < 8; x++)
            {
                Random rnd = new Random();
                int num;
                switch (x)
                {
                    case 0:
                    case 1:
                        num = rnd.Next(65, 91);
                        cadena = cadena + (char) num;
                        break;
                    case 2:
                    case 3:
                        num = rnd.Next(97, 123);
                        cadena = cadena + (char)num;
                        break;
                    case 4:
                    case 5:
                        num = rnd.Next(48, 58);
                        cadena = cadena + (char)num;
                        break;
                    case 6:
                        num = rnd.Next(33, 39);
                        cadena = cadena + (char)num;
                        break;
                }
            }
            return cadena;
        }

        public string GeneraToken()
        {
            string cadena = "";

            for (int x = 0; x < 6; x++)
            {
                Random rnd = new Random();
                int num;
                
                num = rnd.Next(48, 58);
                cadena = cadena + (char)num;
            }
            return cadena;
        }

        public string Encrypt (string cadena)
        {
            string respuesa = CCrypto.Encrypt(cadena, "Pas5pr@se", "s@1tValue", "SHA1", 2, "@1B2c3D4e5F6g7H8", 0x100); 
            
            return respuesa;
        }

        public string Decrypt(string cadena)
        {
            string respuesa = CCrypto.Decrypt(cadena, "Pas5pr@se", "s@1tValue", "SHA1", 2, "@1B2c3D4e5F6g7H8", 0x100);

            return respuesa;
        }

        public int CambioContrasenia(int idUser, string password)
        {
            var result = -1;
            try
            {
                var x = _loginData.CambioContrasenia(idUser, Encrypt(password), 0);                
            }
            catch (Exception)
            {
                result = -1;
            }

            return result;
        }

        public int EnvioCorreo(string usuario, string password, string email, string tipoCorreo)
        {
            int result = 1;
            string cuerpo, asunto;
            try
            {
                if(tipoCorreo == "Nom035") {

                    cuerpo = CuerpoNom035();
                    cuerpo = cuerpo.Replace("@usuario", usuario);
                    cuerpo = cuerpo.Replace("@contraseña", password);
                    asunto = "Encuesta Nom035";
                }
                else if(tipoCorreo == "RecordatorioNom035")
                {
                    cuerpo = CuerpoRecordatorioNom035();
                    cuerpo = cuerpo.Replace("@usuario", usuario);
                    cuerpo = cuerpo.Replace("@contraseña", password);
                    asunto = "Recordatorio Encuesta Nom035";
                }
                else
                {
                    cuerpo = tipoCorreo == "CU" ? _loginData.consultaParametro("CU_Cuerpo") : _loginData.consultaParametro("RC_Cuerpo");
                    asunto = tipoCorreo == "CU" ? _loginData.consultaParametro("CU_Asunto") : _loginData.consultaParametro("RC_Asunto");

                    cuerpo = cuerpo.Replace("Usuario:", "<br/><br/> <b>Usuario:</b>");
                    cuerpo = cuerpo.Replace("@Usuario", usuario);
                    cuerpo = cuerpo.Replace("Password:", "<br/><b>Password:</b>");
                    cuerpo = cuerpo.Replace("@Password", (password + "<br/><br/>"));
                    cuerpo = cuerpo.Replace("@liga", (tipoCorreo == "CU" ? _loginData.consultaParametro("URLBitacora") : _loginData.consultaParametro("URLToken") + "?U=" + Encrypt(usuario).Replace("=", "")));
                    cuerpo = cuerpo.Replace("@enlaceIP", (tipoCorreo == "CU" ? _loginData.consultaParametro("URLBitacoraIP") : _loginData.consultaParametro("URLTokenIP") + "?U=" + Encrypt(usuario).Replace("=", "")));

                   
                }
                var credentials = new NetworkCredential(_loginData.consultaParametro("SMTP_Usuario"), _loginData.consultaParametro("SMTP_Password"));
                
                var mail = new MailMessage()
                {
                    From = new MailAddress(_loginData.consultaParametro("RTE_Correo")),
                    Subject = asunto,
                    Body = "<HTML><BODY><p style='font-family:calibri;font-size:14;color:#315699'>" + cuerpo + "</br></p></BODY></HTML>"
                };
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(email));

                var client = new SmtpClient()
                {
                    Port = Convert.ToInt32(_loginData.consultaParametro("SMTP_Puerto")),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Host = _loginData.consultaParametro("SMTP_Server"),
                    EnableSsl = true,
                    Credentials = credentials

                };

                client.Send(mail);
            }
            catch (Exception ex)
            {
                var resultado = ex.Message;
                result = -1;
            }

            return result;
        }

        public int RecuperarContrasenia(string usuario)
        {
            int resp = 0;
            try
            {
                var user = _loginData.Autenticacion(usuario);
                var email = _UsuariosData.ConsultaEmail(user.IdUser);

                if (user.IdUser == -1)
                    resp = -1;
                else if (email == null)
                    resp = -2;
                else
                {
                    var password = GeneraToken();

                    var x = _loginData.CambioContrasenia(user.IdUser, Encrypt(password), 2);
                    var res = EnvioCorreo(user.Usuario, password, email, "RC") == -3 ? -5 : user.IdUser;
                }
            }
            catch (Exception)
            {
                resp = 0;
            }

            return resp;
        }
        public String ConsultaFecha()
        {
           var x = Convert.ToString(_loginData.ConsultaFechaAsync().Result);
            return x;
            


        }
        public string CuerpoNom035()
        {
            string cuerpo = $@"
            <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;border-collapse:collapse'>
            <tr>
                <td style='background:black;padding:0cm 7.5pt 0cm 7.5pt'>
                    <div align=center>
                        <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=600
                            style='width:450.0pt;border-collapse:collapse'>
                            <tr>
                                <td valign=top style='background:white;padding:30.0pt 15.0pt 15.0pt 15.0pt'>
                                    <h1 align=center style='margin:0cm;text-align:center;line-height:25.5pt'><span
                                            style='font-size:15.0pt;font-family:Lato;color:#111111;letter-spacing:3.0pt;font-weight:normal'>Hola,
                                            ¡buen día!</span>
                                        <o:p></o:p>
                                    </h1>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style='background:#F4F4F4;padding:0cm 7.5pt 0cm 7.5pt'>
                    <div align=center>
                        <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=600
                            style='width:450.0pt;border-collapse:collapse'>
                            <tr>
                                <td style='background:white;padding:15.0pt 22.5pt 22.5pt 22.5pt'>
                                    <p style='margin:0cm;line-height:18.75pt'><b><span style='font-size:12.0pt;font-family:Lato;color:black'>
                                            Como parte del programa para
                                            fomentar un entorno organizacional favorable y prevenir factores de riesgo
                                            psicosocial, se está implementando esta encuesta en EISEI Innovation para seguir
                                            mejorando como empresa y brindar un mayor bienestar.</span></b>
                                        <o:p></o:p>
                                    </p>
                                    <p style='margin:0cm;line-height:18.75pt'><b><span style='font-size:12.0pt;font-family:Lato;color:black'>&nbsp;</span></b>
                                        <o:p></o:p>
                                    </p>
                                    <p style='mso-margin-top-alt:0cm;margin-right:0cm;margin-bottom:18.75pt;margin-left:0cm;line-height:18.75pt'>
                                        <span style='font-size:12.0pt;font-family:Lato;color:#666666'>Solicitamos de tu apoyo,</span>
                                        <span style='font-size:12.0pt;font-family:Lato;color:black'>respondiendo</span>
                                        <span style='font-size:12.0pt;font-family:Lato;color:#666666'> la encuesta.</span>
                                        <o:p></o:p>
                                    </p>
                                    <p style='margin:0cm;line-height:18.75pt'><b><span style='font-size:12.0pt;font-family:Lato;color:#666666'>Indicaciones para
                                                comenzar:</span></b>
                                        <o:p></o:p>
                                    </p>
                                    <p style='margin:0cm;line-height:18.75pt'><span style='font-size:12.0pt;font-family:Lato;color:#666666'>&nbsp;</span>
                                        <o:p></o:p>
                                    </p>
                                    <p style='margin:0cm;line-height:18.75pt'><span style='font-size:12.0pt;font-family:Lato;color:#666666'>Cuando estés listo, da clic en
                                            el siguiente enlace (o cópialo y pégalo en tu navegador):</span>
                                        <o:p></o:p>
                                    </p>
                                    <p style='mso-margin-top-alt:0cm;margin-right:0cm;margin-bottom:18.75pt;margin-left:7.5pt;line-height:18.75pt'>
                                        <span style='font-size:10.0pt;font-family:Arial,sans-serif;color:black'><a
                                                href='http://bitacora.eisei.net.mx/Bitacora'><span
                                                    style='color:black'>http://bitacora.eisei.net.mx/Bitacora</span></a> <br><a
                                                href='http://192.168.0.117/Bitacora'><span
                                                    style='color:black'>http://192.168.0.117/Bitacora</span></a></span>
                                        <o:p></o:p>
                                    </p>
                                    <p style='margin-top:18.75pt;line-height:18.75pt;' id=pwdKey><span
                                            style='font-size:12.0pt;font-family:Lato;color:#666666'> Usuario: </span><b><span
                                                style='font-size:18.0pt;font-family:Lato;color:black'>@usuario</span></b><span
                                            style='font-size:18.0pt;font-family:Lato;color:black'> </span>
                                    </p>
                                    <p style='margin-top:18.75pt;line-height:18.75pt;' id=pwdKey><span
                                            style='font-size:12.0pt;font-family:Lato;color:#666666'>Contraseña: </span><b><span
                                                style='font-size:18.0pt;font-family:Lato;color:black'>@contraseña</span></b><span
                                            style='font-size:18.0pt;font-family:Lato;color:black'> </span>
                                        <o:p></o:p>
                                    </p>
                                    <p style='mso-margin-top-alt:18.75pt;margin-right:0cm;margin-bottom:0cm;margin-left:0cm;line-height:18.75pt'>
                                        <b><span style='font-size:12.0pt;font-family:Lato;color:#666666'>Tus respuestas son
                                                confidenciales.</span></b>
                                        <o:p></o:p>
                                    </p>
                                    <p style='mso-margin-top-alt:0cm;margin-right:0cm;margin-bottom:18.75pt;margin-left:0cm;line-height:18.75pt'>
                                        <span style='font-size:12.0pt;font-family:Lato;color:#666666'>La fecha límite para contestar  es el </span><span class=email-field><b><span
                                                    style='font-size:13.0pt;font-family:Lato;color:#666666'>4 de septiembre de 2020</span></b></span><span
                                            style='font-size:12.0pt;font-family:Lato;color:#666666'>.</span>
                                        <o:p></o:p>
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td style='background:white;padding:0cm 22.5pt 30.0pt 22.5pt;border-radius: 0px 0px 4px 4px'>
                                    <p style='margin:0cm;line-height:18.75pt'><span style='font-size:12.0pt;font-family:Lato;color:#666666'>Atentamente,<br>EISEI
                                            Innovation</span>
                                        <o:p></o:p>
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            </table>
            </div></td></tr></table>";
            return cuerpo;
        }

        public string CuerpoRecordatorioNom035()
        {
            string cuerpo = $@"
            <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width='100%' style='width:100.0%;border-collapse:collapse'>
            <tr>
                <td style='background:black;padding:0cm 7.5pt 0cm 7.5pt'>
                    <div align=center>
                        <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=600
                            style='width:450.0pt;border-collapse:collapse'>
                            <tr>
                                <td valign=top style='background:white;padding:30.0pt 15.0pt 15.0pt 15.0pt'>
                                    <h1 align=center style='margin:0cm;text-align:center;line-height:25.5pt'><span
                                            style='font-size:15.0pt;font-family:Lato;color:#111111;letter-spacing:3.0pt;font-weight:normal'>Hola,
                                            ¡buen día!</span>
                                        <o:p></o:p>
                                    </h1>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style='background:#F4F4F4;padding:0cm 7.5pt 0cm 7.5pt'>
                    <div align=center>
                        <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=600
                            style='width:450.0pt;border-collapse:collapse'>
                            <tr>
                                <td style='background:white;padding:15.0pt 22.5pt 22.5pt 22.5pt'>
                                    <p style='margin:0cm;line-height:18.75pt'><b><span style='font-size:12.0pt;font-family:Lato;color:black'>
                                            Este es un aviso de que tienes pendiente la aplicación de la encuesta 
                                            sobre la Norma 035. Tu participación es muy valiosa, por lo que 
                                            solicitamos tu apoyo para ingresar con los siguientes datos. .</span></b>
                                        <o:p></o:p>
                                    </p>
                                    <p style='margin:0cm;line-height:18.75pt'><b><span style='font-size:12.0pt;font-family:Lato;color:black'>&nbsp;</span></b>
                                        <o:p></o:p>
                                    </p>
                                   
                                    <p style='margin:0cm;line-height:18.75pt'><b><span style='font-size:12.0pt;font-family:Lato;color:#666666'>Indicaciones para
                                                comenzar:</span></b>
                                        <o:p></o:p>
                                    </p>
                                    <p style='margin:0cm;line-height:18.75pt'><span style='font-size:12.0pt;font-family:Lato;color:#666666'>&nbsp;</span>
                                        <o:p></o:p>
                                    </p>
                                    <p style='margin:0cm;line-height:18.75pt'><span style='font-size:12.0pt;font-family:Lato;color:#666666'>Cuando estés listo, da clic en
                                            el siguiente enlace (o cópialo y pégalo en tu navegador):</span>
                                        <o:p></o:p>
                                    </p>
                                    <p style='mso-margin-top-alt:0cm;margin-right:0cm;margin-bottom:18.75pt;margin-left:7.5pt;line-height:18.75pt'>
                                        <span style='font-size:10.0pt;font-family:Arial,sans-serif;color:black'><a
                                                href='http://bitacora.eisei.net.mx/Bitacora'><span
                                                    style='color:black'>http://bitacora.eisei.net.mx/Bitacora</span></a> <br><a
                                                href='http://192.168.0.117/Bitacora'><span
                                                    style='color:black'>http://192.168.0.117/Bitacora</span></a></span>
                                        <o:p></o:p>
                                    </p>
                                    <p style='margin-top:18.75pt;line-height:18.75pt;' id=pwdKey><span
                                            style='font-size:12.0pt;font-family:Lato;color:#666666'> Usuario: </span><b><span
                                                style='font-size:18.0pt;font-family:Lato;color:black'>@usuario</span></b><span
                                            style='font-size:18.0pt;font-family:Lato;color:black'> </span>
                                    </p>
                                    <p style='margin-top:18.75pt;line-height:18.75pt;' id=pwdKey><span
                                            style='font-size:12.0pt;font-family:Lato;color:#666666'>Contraseña: </span><b><span
                                                style='font-size:18.0pt;font-family:Lato;color:black'>@contraseña</span></b><span
                                            style='font-size:18.0pt;font-family:Lato;color:black'> </span>
                                        <o:p></o:p>
                                    </p>
                                    <p style='mso-margin-top-alt:18.75pt;margin-right:0cm;margin-bottom:0cm;margin-left:0cm;line-height:18.75pt'>
                                        <b><span style='font-size:12.0pt;font-family:Lato;color:#666666'>Tus respuestas son
                                                confidenciales.</span></b>
                                        <o:p></o:p>
                                    </p>
                                    <p style='mso-margin-top-alt:0cm;margin-right:0cm;margin-bottom:18.75pt;margin-left:0cm;line-height:18.75pt'>
                                        <span style='font-size:12.0pt;font-family:Lato;color:#666666'>La fecha límite para contestar  es el </span><span class=email-field><b><span
                                                    style='font-size:13.0pt;font-family:Lato;color:#666666'>4 de septiembre de 2020</span></b></span><span
                                            style='font-size:12.0pt;font-family:Lato;color:#666666'>.</span>
                                        <o:p></o:p>
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td style='background:white;padding:0cm 22.5pt 30.0pt 22.5pt;border-radius: 0px 0px 4px 4px'>
                                    <p style='margin:0cm;line-height:18.75pt'><span style='font-size:12.0pt;font-family:Lato;color:#666666'>Atentamente,<br>EISEI
                                            Innovation</span>
                                        <o:p></o:p>
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            </table>
            </div></td></tr></table>";
            return cuerpo;
        }
    }
}
