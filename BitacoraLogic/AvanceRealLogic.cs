using System;
using System.Collections.Generic;
using System.Text;
using BitacoraModels;
using BitacoraData;
using System.Linq;

namespace BitacoraLogic
{
    public class AvanceRealLogic
    {
        AvanceRealData _avanceRealData = new AvanceRealData();
        UsuariosLogic _usuariosLogic = new UsuariosLogic();

        public static List<AvanceRealList> listaEditable = new List<AvanceRealList>();
               
        public List<AvanceRealList> AvanceReal(int idUser, int idUnidad, int idArea)
        {
            List<AvanceRealList> listaAvanceR = new List<AvanceRealList>();
            try
            {
                var listaRelaciones = _usuariosLogic.ConsultaRel_UserUnArea(idUser);

                listaAvanceR = _avanceRealData.ConsultaAvanceReal(idUser);

                if (idUnidad > 0)
                    listaAvanceR = (from x in listaAvanceR where x.IdUnidad == idUnidad select x).ToList();

                if (idArea > 0)
                    listaAvanceR = (from x in listaAvanceR where x.IdArea == idArea select x).ToList();

                listaEditable = (from x in listaAvanceR where x.Bandera == 1 select x).ToList();
            }
            catch(Exception e)
            {
                var error = e.Message;
            }

            return listaAvanceR;
        }

        public int ActualizarAvanceReal(List<AvanceRealList> Avance)
        {
            try
            {
                var listaUpdate = (from x in listaEditable where x.FechaRegistro != null select x).ToList();
                listaUpdate = (from x in Avance where listaUpdate.Any(j => j.IdProyecto == x.IdProyecto) select x).ToList();
                listaUpdate = (from x in listaUpdate where x.AvanceReal > x.Avance select x).ToList();

                var countUpdate = _avanceRealData.ActualizaAvanceReal(listaUpdate);

                var listaInsert = (from x in listaEditable where x.FechaRegistro == null select x).ToList();
                listaInsert = (from x in Avance where listaInsert.Any(j => j.IdProyecto == x.IdProyecto) select x).ToList();
                listaInsert = (from x in listaInsert where x.AvanceReal > 0 select x).ToList();

                var countInsert = _avanceRealData.InsertaAvanceReal(listaInsert);
            }
            catch (Exception e)
            {
                var error = e.Message;
            }

            return 0;
        }

        public List<CatUnidadesNegocios> ConsultaUnidades(int idUser)
        {
            List<CatUnidadesNegocios> listaUnidades = new List<CatUnidadesNegocios>();
            try
            {
                listaUnidades = _avanceRealData.UnidadesRelacionadas(idUser);
                listaUnidades = (from x in listaUnidades orderby x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaUnidades;
        }

        public List<CatAreasNegocio> ConsultaAreas(int idUser)
        {
            List<CatAreasNegocio> listaAreas = new List<CatAreasNegocio>();
            try
            {
                listaAreas = _avanceRealData.AreasRelacionadas(idUser);
                listaAreas = (from x in listaAreas orderby x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaAreas;
        }

        public List<CatAreasNegocio> ConsultaAreaRelacionada(int idUser, int idUnidad)
        {
            List<CatAreasNegocio> listaAreas = new List<CatAreasNegocio>();
            try
            {
                listaAreas = _avanceRealData.AreasRelacionadasUnidad(idUser, idUnidad);
                listaAreas = (from x in listaAreas orderby x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaAreas;
        }
        public List<CatUnidadesNegocios> ConsultaUnidadesRelacionadas(int idUser, int idArea)
        {
            List<CatUnidadesNegocios> listaUnidades = new List<CatUnidadesNegocios>();
            try
            {
                listaUnidades = _avanceRealData.UnidadesRelacionadasArea(idUser, idArea);
                listaUnidades = (from x in listaUnidades orderby x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaUnidades;
        }


    }
}
