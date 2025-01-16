using BitacoraData;
using BitacoraModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BitacoraLogic
{
    public class ConsultaCatalogosLogic
    {
        ConsultaCatalogosData _consultaData = new ConsultaCatalogosData();
        public List<CatUnidadesNegocios> ConsultaUnidadesNegocio()
        {
            List<CatUnidadesNegocios> listaUnidades = new List<CatUnidadesNegocios>();
            try
            {
                listaUnidades = _consultaData.ConsultaUnidadesNegocio();
                listaUnidades = (from x in listaUnidades orderby x.Estatus, x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaUnidades;
        }

        public List<CatAreasNegocio> ConsultaAreasNegocio()
        {
            List<CatAreasNegocio> listaAreas = new List<CatAreasNegocio>();
            try
            {
                listaAreas = _consultaData.ConsultaAreasNegocio();
                listaAreas = (from x in listaAreas orderby x.Estatus, x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaAreas;
        }
        public List<CatClientes> ConsultaClientes()
        {
            List<CatClientes> listaClientes = new List<CatClientes>();
            try
            {
                listaClientes = _consultaData.ConsultaClientes();
                listaClientes = (from x in listaClientes orderby x.Estatus, x.Nombre, x.Unidad, x.Area select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaClientes;
        }
                
        public List<CatSistemas> ConsultaSistemas()
        {
            List<CatSistemas> listaSistemas= new List<CatSistemas>();
            try
            {
                listaSistemas = _consultaData.ConsultaSistemas();
                listaSistemas = (from x in listaSistemas orderby x.Estatus, x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaSistemas;
        }

        public List<CatEtapas> ConsultaEtapas()
        {
            List<CatEtapas> listaEtapas = new List<CatEtapas>();
            try
            {
                listaEtapas = _consultaData.ConsultaEtapas();
                listaEtapas = (from x in listaEtapas orderby x.Estatus, x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaEtapas;
        }

        public List<CatActividades> ConsultaActiviades()
        {
            List<CatActividades> listaActividades= new List<CatActividades>();
            try
            {
                listaActividades = _consultaData.ConsultaActividades();
                listaActividades = (from x in listaActividades orderby x.Estatus, x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaActividades;
        }

        public List<CatProyectos> ConsultaProyectos()
        {
            List<CatProyectos> listaProyectos = new List<CatProyectos>();
            try
            {
                listaProyectos = _consultaData.ConsultaProyectos();
                listaProyectos = (from x in listaProyectos orderby x.Estatus, x.Nombre select x).ToList();
            }
            catch (Exception e)
            {
                string result = e.Message;
            }

            return listaProyectos;
        }    

    }
}
