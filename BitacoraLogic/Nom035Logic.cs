using BitacoraData;
using BitacoraModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BitacoraLogic
{
    public class Nom035Logic
    {
        Nom035Data _Nom035Data = new Nom035Data();
        LoginLogic _login = new LoginLogic();
        UsuariosData _usuarioData = new UsuariosData();
        public UsuarioNom035 InformacionUsuario(string usuario) {
            return _Nom035Data.InformacionUsuario(usuario);
        }

        public object resultados(Nom035Model info)
        {
            return _Nom035Data.resultados(info);
        }
        public int ConsultaConsecutivoEncuesta()
        {
            return _Nom035Data.ConsultaConsecutivoEncuesta();
        }
        public object consultaEncuestados()
        {
           
            List<UsuarioNom035> listaUsuarioNom035 = new List<UsuarioNom035>();
            try
            {
                listaUsuarioNom035 = _Nom035Data.consultaEncuestados();

            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaUsuarioNom035;
        }
        public object consultaNoEncuestados()
        {
           
            List<UsuarioNom035> listaUsuarioNom035 = new List<UsuarioNom035>();
            try
            {
                listaUsuarioNom035 = _Nom035Data.consultaNoEncuestados();

            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return listaUsuarioNom035;
        }

        public int RecordatorioNom035(int Id, string email)
        {
            GuardarRecordatorio(Id);
            CatUsuarios usuario = _usuarioData.Consulta_UsuarioPassword(Id);
            string passwordEncrypt = _login.Decrypt(usuario.Password);
            int result = _login.EnvioCorreo(usuario.Usuario, passwordEncrypt, email, "RecordatorioNom035") == -1 ? -5 : usuario.Id;
            return result;
        }
        public UsuarioNom035 InformacionEncuestas()
        {
            return _Nom035Data.InformacionEncuestas();
        }

        public object GuardarRecordatorio(int Id)
        {
            return _Nom035Data.GuardarRecordatorio(Id);
        }

        public object ConsultaResultadosEncuestas()
        {
            return _Nom035Data.ConsultaResultadosEncuestas();
        }
        public object ConsultaResultadosEncuestasCategoria()
        {
            return _Nom035Data.ConsultaResultadosEncuestasCategoria();
        }
        public object ConsultaResultadosEncuestasDominio()
        {
            return _Nom035Data.ConsultaResultadosEncuestasDominio();
        }
        public object ConResultEncuestasCategoriaPorEmpleado(int idEmpleado)
        {
            return _Nom035Data.ConResultEncuestasCategoriaPorEmpleado(idEmpleado);
        }
        public object ConResultEncuestasDominioPorEmpleado(int idEmpleado)
        {
            return _Nom035Data.ConResultEncuestasDominioPorEmpleado(idEmpleado);
        }
        public object ConResultEncuestasCategoriaPorEmpresa()
        {
            List<VstCalificacionCategoria> lista = _Nom035Data.ConResultEncuestasCategoriaPorEmpresa();
            List<VstCalificacionCategoria> listCategoriaEmpresa = new List<VstCalificacionCategoria>();
            string des_cal = string.Empty;
            foreach (VstCalificacionCategoria item in lista)
            {
                des_cal = obtenerDescripcionCalificacion(item.numPreguntas, item.nullDespreciable, item.bajo, item.medio, item.alto, item.muyAlto);
                listCategoriaEmpresa.Add(new VstCalificacionCategoria()
                {
                    IdCategoria = item.IdCategoria,
                    Categoria = item.Categoria,
                    nullDespreciable = item.nullDespreciable,
                    bajo = item.bajo,
                    medio = item.medio,
                    alto = item.alto,
                    muyAlto = item.muyAlto,
                    numPreguntas = item.numPreguntas,
                    DesCal = des_cal
                });
            }
            return new { result = "OK", objeto = listCategoriaEmpresa };
        }
        public object ConResultEncuestasDominioPorEmpresa()
        {
            List<VstCalificacionDominio> lista = _Nom035Data.ConResultEncuestasDominioPorEmpresa();
            List<VstCalificacionDominio> listDominioEmpresa = new List<VstCalificacionDominio>();
            string des_cal = string.Empty;
            foreach (VstCalificacionDominio item in lista)
            {
                des_cal = obtenerDescripcionCalificacion(item.numPreguntas, item.nullDespreciable, item.bajo, item.medio, item.alto, item.muyAlto);
                listDominioEmpresa.Add(new VstCalificacionDominio()
                {
                    IdDominio = item.IdDominio,
                    Dominio = item.Dominio,
                    nullDespreciable = item.nullDespreciable,
                    bajo = item.bajo,
                    medio = item.medio,
                    alto = item.alto,
                    muyAlto = item.muyAlto,
                    numPreguntas = item.numPreguntas,
                    DesCal = des_cal
                });
            }
            return new { result = "OK", objeto = listDominioEmpresa };
        }
        public object ConsultaResultadosEncuestasTotalesLista()
        {
            List<VstCalificacionDominio> lista = _Nom035Data.ConsultaResultadosEncuestasTotalesLista();

            List<VstCalificacionDominio> listaNueva = new List<VstCalificacionDominio>();
          
            string des_cal = string.Empty;
            foreach (VstCalificacionDominio item in lista)
            {
                des_cal = obtenerDescripcionCalificacion(item.numPreguntas, item.nullDespreciable, item.bajo, item.medio, item.alto, item.muyAlto);

                listaNueva.Add(new VstCalificacionDominio()
                {
                    
                   nullDespreciable =  item.nullDespreciable,
                   bajo = item.bajo,
                   medio = item.medio ,
                   alto =   item.alto,
                   muyAlto = item.muyAlto,
                   numPreguntas = item.numPreguntas,
                   DesCal = des_cal

                });
            }

            return new { result = "OK", objeto = listaNueva };
        }
        private string obtenerDescripcionCalificacion(int numPreguntas, int nullDespreciable, int bajo, int medio, int alto, int muyAlto)
        {
            int rango = 0, rango_nulo = 0, rango_bajo = 0, rango_medio = 0, rango_alto = 0, rango_muyAlto = 0, num_cal_total = 0;
            float divTotal = 0.0F;
            string des_cal = string.Empty;
        
                rango = (numPreguntas / 5);
                rango_nulo = rango - 1;
                rango_bajo = rango_nulo + rango;
                rango_medio = rango_bajo + rango;
                rango_alto = rango_medio + rango;
                divTotal = ((nullDespreciable * 1) + (bajo * 2) + (medio * 3) + (alto * 4) + (muyAlto * 5)) / 5f;
                num_cal_total = Convert.ToInt32(Math.Round(divTotal));
                
      
            if (num_cal_total <= rango_nulo)
                {
                    des_cal = "Nulo o despreciable";
                }
                else if (num_cal_total > rango_nulo && num_cal_total <= rango_bajo)
                {
                    des_cal = "Bajo";
                }
                else if (num_cal_total > rango_bajo && num_cal_total <= rango_medio)
                {
                    des_cal = "Medio";
                }
                else if (num_cal_total > rango_medio && num_cal_total <= rango_alto)
                {
                    des_cal = "Alto";
                }
                else if(num_cal_total> rango_alto)
                {
                    des_cal = "Muy alto";
                }

            return des_cal;


        }
    }
}
