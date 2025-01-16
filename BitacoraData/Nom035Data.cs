using BitacoraData.Context;
using BitacoraModels;
using Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BitacoraData
{
    public class Nom035Data
    {
        static conexion SQL = new conexion();
        SqlConnection conn = new SqlConnection(SQL.ConexionSQL());
        public UsuarioNom035 InformacionUsuario(string usuario)
        {
            UsuarioNom035 datos = new UsuarioNom035();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    datos = (from u in db.CatUsuarios
                             join r in db.RelacionUsuarioEmpleado on u.Id equals r.IdUser
                             join e in db.CatEmpleados on r.IdEmpleado equals e.Id
                             where u.Usuario == usuario
                             select new UsuarioNom035
                             {
                                 id = u.Id,
                                 Usuario = u.Usuario,
                                 Nombre = e.Nombre,
                                 ApellidoPat = e.ApellidoPaterno,
                                 AperllidoMat = e.ApellidoMaterno
                             }).First();
                    datos.AplicarExamen = (from u in db.TblEncuestasAplicadasNom035
                                           where u.Idempleado == datos.id
                                           select u.Idempleado).Count();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return datos;
        }

        public object resultados(Nom035Model info)
        {
            
            try
            {
                int consecutivo = ConsultaConsecutivoEncuesta();
                foreach (var repuestas in info.respuestas)
                {
                    using (BitacoraContext db = new BitacoraContext())
                    {
                        repuestas.puntos = (from v in db.TblValorPreguntasNom35
                                            where v.IdPregunta == repuestas.IdPregunta && v.IdRespuesta == repuestas.Idrespuestausuario
                                            select v.Puntos).FirstOrDefault();
                        if (repuestas.puntos == null)
                            repuestas.puntos = 0;

                        TblEncuestasAplicadasNom035 guardar = new TblEncuestasAplicadasNom035();
                        guardar.IdEncuesta = consecutivo;
                        guardar.Idempleado = info.infoUsuario.id;
                        guardar.IdPregunta = repuestas.IdPregunta;
                        guardar.Idrespuestausuario = repuestas.Idrespuestausuario;
                        guardar.Puntos = repuestas.puntos;
                        guardar.Fecha = DateTime.Now;
                        db.TblEncuestasAplicadasNom035.Add(guardar);
                        db.SaveChanges();
                    }
                }
                
            }
            catch (Exception e)
            {
                var error = e.Message;
            }


            return new { result = info.respuestas, nombre = info.infoUsuario.Nombre };
        }
        public int ConsultaConsecutivoEncuesta()
        {
            int consecutivo = 1;
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                   var numconsecutivo = db.TblEncuestasAplicadasNom035.OrderByDescending(c => c.IdEncuesta).FirstOrDefault();

                   consecutivo = numconsecutivo != null ? (Convert.ToInt32(numconsecutivo.IdEncuesta) + 1) : 1;
                }
            }
            catch (Exception e)
            {
                string Error = e.Message;
                consecutivo = -1;
            }

            return consecutivo;
        }
        public List<UsuarioNom035> consultaEncuestados()
        {
            List<UsuarioNom035> ListaUsuarioNom035 = new List<UsuarioNom035>();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    ListaUsuarioNom035 = (from e in db.CatEmpleados 
                                          join r in db.RelacionUsuarioEmpleado on e.Id equals r.IdEmpleado
                                          join u in db.CatUsuarios on r.IdUser equals u.Id
                                          join te in db.TblEncuestasAplicadasNom035 on u.Id equals te.Idempleado
                                          join cr in db.CatRoles on u.IdRol equals cr.Id
                                          where cr.Nombre == "Usuarios NOM 035"
                                          select new UsuarioNom035
                                          {
                                              id = u.Id,
                                              Usuario = u.Usuario,
                                             Nombre = e.Nombre,
                                             ApellidoPat = e.ApellidoPaterno,
                                             AperllidoMat = e.ApellidoMaterno,
                                             email = e.EstatusERT == 2 ? (e.EmailAsignado != null && e.EmailAsignado != "") ? e.EmailAsignado : (e.EmailInterno != null && e.EmailAsignado != "") ? e.EmailInterno : e.Email : (e.EmailInterno != null && e.EmailAsignado != "") ? e.EmailInterno : e.Email,
                                             fecha = te.Fecha

                                          }).Distinct().ToList();

                    ListaUsuarioNom035 = (from x in ListaUsuarioNom035 orderby x.fecha descending select x).ToList();
                }

            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return ListaUsuarioNom035;
        }
        public List<UsuarioNom035> consultaNoEncuestados()
        {
            List<UsuarioNom035> ListaUsuarioNom035 = new List<UsuarioNom035>();
            try
            {
                DateTime FechaActual = DateTime.Now;
                DateTime aux = new DateTime();
                using (BitacoraContext db = new BitacoraContext())
                {
                    ListaUsuarioNom035 = (from e in db.CatEmpleados
                                          join r in db.RelacionUsuarioEmpleado on e.Id equals r.IdEmpleado
                                          join u in db.CatUsuarios on r.IdUser equals u.Id
                                          join te in db.TblEncuestasAplicadasNom035 on u.Id equals te.Idempleado into ute
                                          from fd in ute.DefaultIfEmpty()
                                          join cr in db.CatRoles on u.IdRol equals cr.Id
                                          where cr.Nombre == "Usuarios NOM 035" && fd.Idempleado == null 
                                          select new UsuarioNom035
                                          {
                                              id = u.Id,
                                              Usuario = u.Usuario,
                                              Nombre = e.Nombre,
                                              ApellidoPat = e.ApellidoPaterno,
                                              AperllidoMat = e.ApellidoMaterno,
                                              email = e.EstatusERT == 2 ? (e.EmailAsignado != null && e.EmailAsignado != "") ? e.EmailAsignado : (e.EmailInterno != null && e.EmailAsignado != "") ? e.EmailInterno : e.Email : (e.EmailInterno != null && e.EmailAsignado != "") ? e.EmailInterno : e.Email,
                                              fecha = fd.Fecha,
                                              Recordatorio = ConsultaFechaRecordatorio(u.Id) != aux ? (FechaActual - ConsultaFechaRecordatorio(u.Id)).TotalHours > 24 ? 1 : 0 : 1

                                          }).Distinct().ToList();

                    ListaUsuarioNom035 = (from x in ListaUsuarioNom035 orderby x.ApellidoPat ascending select x).ToList();
                }

            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return ListaUsuarioNom035;
        }
        public UsuarioNom035 InformacionEncuestas()
        {
            UsuarioNom035 datos = new UsuarioNom035();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    
                    datos.encuestados = (from e in db.CatEmpleados
                                         join r in db.RelacionUsuarioEmpleado on e.Id equals r.IdEmpleado
                                         join u in db.CatUsuarios on r.IdUser equals u.Id
                                         join te in db.TblEncuestasAplicadasNom035 on u.Id equals te.Idempleado
                                         join cr in db.CatRoles on u.IdRol equals cr.Id
                                         where cr.Nombre == "Usuarios NOM 035"
                                         select new UsuarioNom035
                                         { 
                                             
                                             Usuario = u.Usuario

                                         }).Distinct().Count();

                    datos.noEncuestados = (from e in db.CatEmpleados
                                           join r in db.RelacionUsuarioEmpleado on e.Id equals r.IdEmpleado
                                           join u in db.CatUsuarios on r.IdUser equals u.Id
                                           join te in db.TblEncuestasAplicadasNom035 on u.Id equals te.Idempleado into ute
                                           from fd in ute.DefaultIfEmpty()
                                           join cr in db.CatRoles on u.IdRol equals cr.Id
                                           where cr.Nombre == "Usuarios NOM 035" && fd.Idempleado == null
                                           select new UsuarioNom035
                                           {
                                               Usuario = u.Usuario

                                           }).Distinct().Count();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return datos;
        }

        public static DateTime ConsultaFechaRecordatorio(int Id)
        {
            DateTime recordatorio = new DateTime();
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    recordatorio = (from v in db.TblRecordatorios
                                        where v.IdUsuario == Id orderby v.Id descending
                                        select v.Fecha).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
            }
            return recordatorio;
        }

        public object GuardarRecordatorio(int Id)
        {
            try
            {
                using (BitacoraContext db = new BitacoraContext())
                {
                    TblRecordatorios guardar = new TblRecordatorios();
                    guardar.IdUsuario = Id;
                    guardar.Fecha = DateTime.Now;
                    db.TblRecordatorios.Add(guardar);
                    db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                var error = e.Message;
                return new { result = error, usuario = 0 };
            }


            return new { result = "OK", usuario = Id};
        }

        public object ConsultaResultadosEncuestas()
        {
           
            var Datos = new List<VstCalificacionEncuesta>();
            try
            {
                Datos = DBContext<VstCalificacionEncuesta>.CallSelectStatement("SELECT * FROM vst_puntos_calificacion_encuesta order by puntos desc", x => new VstCalificacionEncuesta
                {
                    IdEncuesta = x.GetInt32(0),
                    Idempleado = x.GetInt32(1),
                    Empleado = x.IsDBNull(2) ? null : x.GetString(2).Trim(),
                    Puntos = x.GetInt32(3),
                    DesCal = x.IsDBNull(4) ? null : x.GetString(4).Trim(),
                }).ToList();

            }
            catch (Exception ex)
            {
                return Datos;
            }
            finally { conn.Close(); }

            return new { result = "OK", objeto = Datos };
        }
        public object ConsultaResultadosEncuestasCategoria()
        {
           
            var Datos = new List<VstCalificacionCategoria>();
            try
            {
                Datos = DBContext<VstCalificacionCategoria>.CallSelectStatement("SELECT * FROM vst_puntos_calificacion_categoria ORDER BY idencuesta, empleado, categoria ASC", x => new VstCalificacionCategoria
                {
                    IdEncuesta = x.GetInt32(0),
                    Idempleado = x.GetInt32(1),
                    Empleado = x.IsDBNull(2) ? null : x.GetString(2).Trim(),
                    IdCategoria = x.GetInt32(3),
                    Categoria = x.IsDBNull(4) ? null : x.GetString(4).Trim(),
                    Puntos = x.GetInt32(5),
                    IdCal = x.GetInt32(6),
                    DesCal = x.IsDBNull(7) ? null : x.GetString(7).Trim(),
                }).ToList();

            }
            catch (Exception ex)
            {
                return Datos;
            }
            finally { conn.Close(); }

            return new { result = "OK", objeto = Datos };
        }
        public object ConsultaResultadosEncuestasDominio()
        {
            var Datos = new List<VstCalificacionDominio>();
            try
            {
                Datos = DBContext<VstCalificacionDominio>.CallSelectStatement("SELECT * FROM vst_puntos_calificacion_dominio ORDER BY idencuesta, empleado, dominio ASC", x => new VstCalificacionDominio
                {
                    IdEncuesta = x.GetInt32(0),
                    Idempleado = x.GetInt32(1),
                    Empleado = x.IsDBNull(2) ? null : x.GetString(2).Trim(),
                    IdDominio= x.GetInt32(5),
                    Dominio = x.IsDBNull(3) ? null : x.GetString(3).Trim(),
                    Puntos = x.GetInt32(4),
                    IdCal = x.GetInt32(6),
                    DesCal = x.IsDBNull(7) ? null : x.GetString(7).Trim(),
                }).ToList();

            }
            catch (Exception ex)
            {
                return Datos;
            }
            finally { conn.Close(); }

            return new { result = "OK", objeto = Datos };
        }

        public object ConResultEncuestasDominioPorEmpleado(int idEmpleado)
        {
            var Datos = new List<VstCalificacionDominio>();
            try
            {
                Datos = DBContext<VstCalificacionDominio>.CallSelectStatement("SELECT * FROM vst_puntos_calificacion_dominio  where idempleado=" + idEmpleado + " ORDER BY idencuesta, empleado, dominio ASC", x => new VstCalificacionDominio
                {
                    IdEncuesta = x.GetInt32(0),
                    Idempleado = x.GetInt32(1),
                    Empleado = x.IsDBNull(2) ? null : x.GetString(2).Trim(),
                    IdDominio = x.GetInt32(5),
                    Dominio = x.IsDBNull(3) ? null : x.GetString(3).Trim(),
                    Puntos = x.GetInt32(4),
                    IdCal = x.GetInt32(6),
                    DesCal = x.IsDBNull(7) ? null : x.GetString(7).Trim(),
                    numPreguntas = x.GetInt32(5) == 1 ? 13 : x.GetInt32(5) == 2 ? 3 : x.GetInt32(5) == 3 ? 7 : x.GetInt32(5) == 4 ? 2 : x.GetInt32(5) == 5 ? 2: x.GetInt32(5) == 6 ? 5 : x.GetInt32(5) == 7 ? 6 : 8,

                    rangoPreguntas = x.GetInt32(5) == 1 ? "0-52" : x.GetInt32(5) == 2 ? "0-12" : x.GetInt32(5) == 3 ? "0-28" : x.GetInt32(5) == 4 ? "0-8" : x.GetInt32(5) == 5 ? "0-8" : x.GetInt32(5) == 6 ? "0-20" : x.GetInt32(5) == 7 ? "0-24" : "0-32",

                    nullDespreciableCadena = x.GetInt32(5) == 1 ? "0-11" : x.GetInt32(5) == 2 ? "0-2" : x.GetInt32(5) == 3 ? "0-4" : x.GetInt32(5) == 4 ? "0" : x.GetInt32(5) == 5 ? "0" : x.GetInt32(5) == 6 ? "0-2" : x.GetInt32(5) == 7 ? "0-4" : "0-6",
                    bajoCadena = x.GetInt32(5) == 1 ? "12-15" : x.GetInt32(5) == 2 ? "3-4" : x.GetInt32(5) == 3 ? "5-7" : x.GetInt32(5) == 4 ? "1" : x.GetInt32(5) == 5 ? "1" : x.GetInt32(5) == 6 ? "3-4" : x.GetInt32(5) == 7 ? "5-7" : "7-9",
                    medioCadena = x.GetInt32(5) == 1 ? "16-19" : x.GetInt32(5) == 2 ? "5-6" : x.GetInt32(5) == 3 ? "8-10" : x.GetInt32(5) == 4 ? "2-3" : x.GetInt32(5) == 5 ? "2-3" : x.GetInt32(5) == 6 ? "5-7" : x.GetInt32(5) == 7 ? "8-10" : "10-12",
                    altoCadena = x.GetInt32(5) == 1 ? "20-23" : x.GetInt32(5) == 2 ? "7-8" : x.GetInt32(5) == 3 ? "11-13" : x.GetInt32(5) == 4 ? "4-5" : x.GetInt32(5) == 5 ? "4-5" : x.GetInt32(5) == 6 ? "8-10" : x.GetInt32(5) == 7 ? "11-13" : "13-15",
                    muyAltoCadena = x.GetInt32(5) == 1 ? "24+" : x.GetInt32(5) == 2 ? "9+" : x.GetInt32(5) == 3 ? "14+" : x.GetInt32(5) == 4 ? "6+" : x.GetInt32(5) == 5 ? "6" : x.GetInt32(5) == 6 ? "11+" : x.GetInt32(5) == 7 ? "14+" : "16+",
                    //nullDespreciable = obtenerRangoDominio(x.GetInt32(5), 1),
                    //bajo = obtenerRangoDominio(x.GetInt32(5), 2),
                    //medio = obtenerRangoDominio(x.GetInt32(5), 3),
                    //alto = obtenerRangoDominio(x.GetInt32(5), 4),
                    //muyAlto = obtenerRangoDominio(x.GetInt32(5), 5),


                }).ToList();

            }
            catch (Exception ex)
            {
                return Datos;
            }
            finally { conn.Close(); }

            return new { result = "OK", objeto = Datos };
        }
                      
        public object ConResultEncuestasCategoriaPorEmpleado(int idEmpleado)
        {
            var Datos = new List<VstCalificacionCategoria>();
            try
            {
                Datos = DBContext<VstCalificacionCategoria>.CallSelectStatement("SELECT * FROM vst_puntos_calificacion_categoria where idempleado=" + idEmpleado + " ORDER BY idencuesta, empleado, categoria ASC", x => new VstCalificacionCategoria
                {
                    IdEncuesta = x.GetInt32(0),
                    Idempleado = x.GetInt32(1),
                    Empleado = x.IsDBNull(2) ? null : x.GetString(2).Trim(),
                    IdCategoria = x.GetInt32(3),
                    Categoria = x.IsDBNull(4) ? null : x.GetString(4).Trim(),
                    Puntos = x.GetInt32(5),
                    IdCal = x.GetInt32(6),
                    DesCal = x.IsDBNull(7) ? null : x.GetString(7).Trim(),
                    numPreguntas = x.GetInt32(3) == 1 ? 3 : x.GetInt32(3) == 2 ? 17 : x.GetInt32(3) == 3 ? 22 : 4,
                    rangoPreguntas = x.GetInt32(3) == 1 ? "0-12" : x.GetInt32(3) == 2 ? "0-68" : x.GetInt32(3) == 3 ? "0-88" : "0-16",
                    nullDespreciableCadena = x.GetInt32(3) == 1 ? "0-2" : x.GetInt32(3) == 2 ? "0-9" : x.GetInt32(3) == 3 ? "0-9" : "0-3",
                    bajoCadena = x.GetInt32(3) == 1 ? "3-4" : x.GetInt32(3) == 2 ? "10-19" : x.GetInt32(3) == 3 ? "10-17" : "4-5",
                    medioCadena = x.GetInt32(3) == 1 ? "5-6" : x.GetInt32(3) == 2 ? "20-29" : x.GetInt32(3) == 3 ? "18-27" : "6-8",
                    altoCadena = x.GetInt32(3) == 1 ? "7-8" : x.GetInt32(3) == 2 ? "30-39" : x.GetInt32(3) == 3 ? "28-37" : "9-11",
                    muyAltoCadena = x.GetInt32(3) == 1 ? "9+" : x.GetInt32(3) == 2 ? "40+" : x.GetInt32(3) == 3 ? "38+" : "12+",
                }).ToList();

            }
            catch (Exception ex)
            {
                return Datos;
            }
            finally { conn.Close(); }

            return new { result = "OK", objeto = Datos };
        }
        public string obtenerRangoDominio(int idDominio, int idCal)
        {
            string rango = "";
            var Datos = new List<rangos>();
            try
            {
                Datos = DBContext<rangos>.CallSelectStatement("SELECT valorMin,valorMax FROM dbo.tbl_cat_opciones_accion_dominio where idDominio = " + idDominio + " and idcal = " + idCal + " order by idDominio", x => new rangos
                {
                    valorMin = x.GetInt32(0),
                    valorMax = x.GetInt32(1),

                }).ToList();

            }
            catch (Exception ex)
            {
                return "";
            }
            finally { conn.Close(); }
            rango = Datos[0].valorMin + "-" + Datos[0].valorMax;

            return rango;
        }
        public List<VstCalificacionCategoria> ConResultEncuestasCategoriaPorEmpresa()
        {
            int nulo = 0;
            int bajo = 0;
            int medio = 0;
            int alto = 0;
            int muyalto = 0;
            string categoriaAmbiente = "";
            List<VstCalificacionCategoria> listCategoria = ConsultaResultadosEncuestasCategoriaLista();
            List<VstCalificacionCategoria> listCategoriaEmpresa = new List<VstCalificacionCategoria>();
            obtenerNumeroCalificacionesCategoria(listCategoria,1,ref categoriaAmbiente, ref nulo,ref bajo, ref medio, ref alto, ref muyalto);

            listCategoriaEmpresa.Add(new VstCalificacionCategoria()
            {
                IdCategoria = 1,
                Categoria = categoriaAmbiente,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""

            });
             nulo = 0;
             bajo = 0;
             medio = 0;
             alto = 0;
             muyalto = 0;
            obtenerNumeroCalificacionesCategoria(listCategoria, 2,ref categoriaAmbiente, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listCategoriaEmpresa.Add(new VstCalificacionCategoria()
            {
                IdCategoria = 2,
                Categoria = categoriaAmbiente,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""

            });
            nulo = 0;
            bajo = 0;
            medio = 0;
            alto = 0;
            muyalto = 0;
            obtenerNumeroCalificacionesCategoria(listCategoria, 3, ref categoriaAmbiente, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listCategoriaEmpresa.Add(new VstCalificacionCategoria()
            {
                IdCategoria = 3,
                Categoria = categoriaAmbiente,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""
            });
            nulo = 0;
            bajo = 0;
            medio = 0;
            alto = 0;
            muyalto = 0;
            obtenerNumeroCalificacionesCategoria(listCategoria, 4, ref categoriaAmbiente, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listCategoriaEmpresa.Add(new VstCalificacionCategoria()
            {
                IdCategoria = 4,
                Categoria = categoriaAmbiente,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""

            });

            return listCategoriaEmpresa;
        }

        public List<VstCalificacionDominio> ConResultEncuestasDominioPorEmpresa()
        {
            int nulo = 0;
            int bajo = 0;
            int medio = 0;
            int alto = 0;
            int muyalto = 0;
            string dominio = "";
            List<VstCalificacionDominio> listDominio = ConsultaResultadosEncuestasDominioLista();
            List<VstCalificacionDominio> listDominioEmpresa = new List<VstCalificacionDominio>();
            obtenerNumeroCalificacionesDominio(listDominio, 1, ref dominio, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listDominioEmpresa.Add(new VstCalificacionDominio()
            {
                IdDominio = 1,
                Dominio = dominio,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""

            });
       
            obtenerNumeroCalificacionesDominio(listDominio, 2, ref dominio, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listDominioEmpresa.Add(new VstCalificacionDominio()
            {
                IdDominio = 2,
                Dominio = dominio,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""

            });
         
            obtenerNumeroCalificacionesDominio(listDominio, 3, ref dominio, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listDominioEmpresa.Add(new VstCalificacionDominio()
            {
                IdDominio = 3,
                Dominio = dominio,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""
            });
            
            obtenerNumeroCalificacionesDominio(listDominio, 4, ref dominio, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listDominioEmpresa.Add(new VstCalificacionDominio()
            {
                IdDominio = 4,
                Dominio = dominio,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""

            });
            obtenerNumeroCalificacionesDominio(listDominio, 5, ref dominio, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listDominioEmpresa.Add(new VstCalificacionDominio()
            {
                IdDominio = 5,
                Dominio = dominio,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""

            });

            obtenerNumeroCalificacionesDominio(listDominio, 6, ref dominio, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listDominioEmpresa.Add(new VstCalificacionDominio()
            {
                IdDominio = 6,
                Dominio = dominio,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""

            });

            obtenerNumeroCalificacionesDominio(listDominio, 7, ref dominio, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listDominioEmpresa.Add(new VstCalificacionDominio()
            {
                IdDominio = 7,
                Dominio = dominio,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""

            });

            obtenerNumeroCalificacionesDominio(listDominio, 8, ref dominio, ref nulo, ref bajo, ref medio, ref alto, ref muyalto);

            listDominioEmpresa.Add(new VstCalificacionDominio()
            {
                IdDominio = 8,
                Dominio = dominio,
                numPreguntas = nulo + bajo + medio + alto + muyalto,
                nullDespreciable = nulo,
                bajo = bajo,
                medio = medio,
                alto = alto,
                muyAlto = muyalto,
                DesCal = ""

            });

            return  listDominioEmpresa;
        }
        public void obtenerNumeroCalificacionesCategoria(List<VstCalificacionCategoria> listCategoria, int numCategoria, ref string categoria, ref int nulo,ref int bajo, ref int medio, ref int alto, ref int muyAlto)
        {

            nulo = 0;
            bajo = 0;
            medio = 0;
            alto = 0;
            muyAlto = 0;

            foreach (VstCalificacionCategoria cat in listCategoria)
            {

                switch (cat.IdCategoria)
                {

                    case 1:
                        if (numCategoria == cat.IdCategoria)
                        {
                            categoria = cat.Categoria;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    case 2:
                        if (numCategoria == cat.IdCategoria)
                        {
                            categoria = cat.Categoria;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    case 3:
                        if (numCategoria == cat.IdCategoria)
                        {
                            categoria = cat.Categoria;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    case 4:
                        if (numCategoria == cat.IdCategoria)
                        {
                            categoria = cat.Categoria;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                            break;
                        
                    default:
                        break;
                }

            }
        }

        public void obtenerNumeroCalificacionesDominio(List<VstCalificacionDominio> listCategoria, int numDominio, ref string dominio, ref int nulo, ref int bajo, ref int medio, ref int alto, ref int muyAlto)
        {
            nulo = 0;
            bajo = 0;
            medio = 0;
            alto = 0;
            muyAlto = 0;
            foreach (VstCalificacionDominio cat in listCategoria)
            {

                switch (cat.IdDominio)
                {

                    case 1:
                        if (numDominio == cat.IdDominio)
                        {
                            dominio = cat.Dominio;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    case 2:
                        if (numDominio == cat.IdDominio)
                        {
                            dominio = cat.Dominio;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    case 3:
                        if (numDominio == cat.IdDominio)
                        {
                            dominio = cat.Dominio;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    case 4:
                        if (numDominio == cat.IdDominio)
                        {
                            dominio = cat.Dominio;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    case 5:
                        if (numDominio == cat.IdDominio)
                        {
                            dominio = cat.Dominio;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    case 6:
                        if (numDominio == cat.IdDominio)
                        {
                            dominio = cat.Dominio;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    case 7:
                        if (numDominio == cat.IdDominio)
                        {
                            dominio = cat.Dominio;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    case 8:
                        if (numDominio == cat.IdDominio)
                        {
                            dominio = cat.Dominio;
                            switch (cat.IdCal)
                            {
                                case 1:
                                    nulo++;
                                    break;
                                case 2:
                                    bajo++;
                                    break;
                                case 3:
                                    medio++;
                                    break;
                                case 4:
                                    alto++;
                                    break;
                                default:
                                    muyAlto++;
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        public List<VstCalificacionCategoria> ConsultaResultadosEncuestasCategoriaLista()
        {

            var Datos = new List<VstCalificacionCategoria>();
            try
            {
                Datos = DBContext<VstCalificacionCategoria>.CallSelectStatement("SELECT * FROM vst_puntos_calificacion_categoria ORDER BY idcategoria", x => new VstCalificacionCategoria
                {
                    IdEncuesta = x.GetInt32(0),
                    Idempleado = x.GetInt32(1),
                    Empleado = x.IsDBNull(2) ? null : x.GetString(2).Trim(),
                    IdCategoria = x.GetInt32(3),
                    Categoria = x.IsDBNull(4) ? null : x.GetString(4).Trim(),
                    Puntos = x.GetInt32(5),
                    IdCal = x.GetInt32(6),
                    DesCal = x.IsDBNull(7) ? null : x.GetString(7).Trim(),
                }).ToList();

            }
            catch (Exception ex)
            {
                return Datos;
            }
            finally { conn.Close(); }

            return Datos;
        }

        public List<VstCalificacionDominio> ConsultaResultadosEncuestasDominioLista()
        {
            
            var Datos = new List<VstCalificacionDominio>();
            try
            {
                Datos = DBContext<VstCalificacionDominio>.CallSelectStatement("SELECT * FROM vst_puntos_calificacion_dominio ORDER BY idencuesta, empleado, dominio ASC", x => new VstCalificacionDominio
                {
                    IdEncuesta = x.GetInt32(0),
                    Idempleado = x.GetInt32(1),
                    Empleado = x.IsDBNull(2) ? null : x.GetString(2).Trim(),
                    IdDominio = x.GetInt32(5),
                    Dominio = x.IsDBNull(3) ? null : x.GetString(3).Trim(),
                    Puntos = x.GetInt32(4),
                    IdCal = x.GetInt32(6),
                    DesCal = x.IsDBNull(7) ? null : x.GetString(7).Trim(),
                }).ToList();

            }
            catch (Exception ex)
            {
                return Datos;
            }
            finally { conn.Close(); }

            return Datos;
        }

        public List<VstCalificacionDominio> ConsultaResultadosEncuestasTotalesLista()
        {

            var Datos = new List<VstCalificacionDominio>();
            try
            {
                Datos = DBContext<VstCalificacionDominio>.CallSelectStatement("SELECT  [Nulo o despreciable],[Bajo], [Medio],[Alto],[Muy Alto] FROM vst_puntos_calificacion_empresa", x => new VstCalificacionDominio
                {
                    nullDespreciable = x.IsDBNull(0) ? 0 : x.GetInt32(0),
                    bajo = x.IsDBNull(1) ? 0 : x.GetInt32(1),
                    medio = x.IsDBNull(2) ? 0 : x.GetInt32(2),
                    alto = x.IsDBNull(3) ? 0 : x.GetInt32(3),
                    muyAlto = x.IsDBNull(4) ? 0 : x.GetInt32(4),
                    numPreguntas =(x.IsDBNull(0) ? 0: x.GetInt32(0)) + (x.IsDBNull(1) ? 0 : x.GetInt32(1)) + (x.IsDBNull(2) ? 0 : x.GetInt32(2)) + (x.IsDBNull(3) ? 0 : x.GetInt32(3)) + (x.IsDBNull(4) ? 0 : x.GetInt32(4)), 
                }).ToList();
            }
            catch (Exception ex)
            {
                return Datos;
            }
            finally { conn.Close(); }


            return  Datos ;
        }
    }
}
