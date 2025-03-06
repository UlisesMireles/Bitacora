using BitacoraModels;
using BitacoraData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitacoraLogic
{
    public class Bitacora_Logic
    {
        Bitacora_Data _BitacoraData = new Bitacora_Data();
        public object ConsultaBitacora(int idUser)
        {
            string result = "ok";
            List<BItacoraInf> listaBitacora = new List<BItacoraInf>();
            try
            {
                listaBitacora = _BitacoraData.ConsultaBitacora(idUser);
                listaBitacora = (from x in listaBitacora orderby x.Fecha descending, x.Id descending select x).ToList();

            }
            catch (Exception e)
            {
                result = e.Message; 
            }

            var resp = new { result = result, listaBitacora = listaBitacora };

            return resp;
        }

        public object GetProyectos(int idUser)
        {
            string result = "ok";
            List<CatProyectos> proyectos = new List<CatProyectos>();
            try
            {
                proyectos = _BitacoraData.GetProyectos(idUser);
                proyectos = (from x in proyectos orderby x.Nombre select x).ToList();

            }
            catch (Exception e)
            {
                result = e.Message;
            }

            var resp = new { result = result, Proyectos = proyectos };

            return resp;
        }

        public object GetEtapas()
        {
            string result = "ok";
            List<CatEtapas> etapas = new List<CatEtapas>();
            try
            {
                etapas = _BitacoraData.GetEtapas();
                etapas = (from x in etapas orderby x.Nombre select x).ToList();

            }
            catch (Exception e)
            {
                result = e.Message;
            }

            var resp = new { result = result, Etapas = etapas };

            return resp;
        }

        public object GetRelacionEtapas(List<string> listaEstatus)
        {
            string result = "ok";
            List<RelacionEtapaEstatus> relacionEtapasEstatus = new List<RelacionEtapaEstatus>();
            try
            {
                relacionEtapasEstatus = _BitacoraData.GetRelacionesEtapas(listaEstatus);
                relacionEtapasEstatus = (from x in relacionEtapasEstatus orderby x.Id select x).ToList();

            }
            catch (Exception e)
            {
                result = e.Message;
            }

            var resp = new { result = result, RelacionesEtapasEstatus = relacionEtapasEstatus };

            return resp;
        }

        public object GetActividades()
        {
            string result = "ok";
            List<CatActividades> actividades = new List<CatActividades>();
            try
            {
                actividades = _BitacoraData.GetActividades();
                actividades = (from x in actividades orderby x.Nombre select x).ToList();

            }
            catch (Exception e)
            {
                result = e.Message;
            }

            var resp = new { result = result, Actividades = actividades };

            return resp;
        }

        public object GetRelacionActividades(List<string> listaEstatus)
        {
            string result = "ok";
            List<RelacionActividadEstatus> relacionActividadEstatus = new List<RelacionActividadEstatus>();
            try
            {
                relacionActividadEstatus = _BitacoraData.GetRelacionesActividades(listaEstatus);
                relacionActividadEstatus = (from x in relacionActividadEstatus orderby x.Id select x).ToList();

            }
            catch (Exception e)
            {
                result = e.Message;
            }

            var resp = new { result = result, RelacionesActividadEstatus = relacionActividadEstatus };

            return resp;
        }

        public object GetRelacionProyectos(int idUser)
        {
            string result = "ok";
            List<RelacionProyectos> relacionProyectos = new List<RelacionProyectos>();
            try
            {
                relacionProyectos = _BitacoraData.GetRelacionProyectos(idUser);
                relacionProyectos = (from x in relacionProyectos orderby x.IdProyecto select x).ToList();

            }
            catch (Exception e)
            {
                result = e.Message;
            }

            var resp = new { result = result, RelacionesProyectos = relacionProyectos };

            return resp;
        }

        public int InsertaBitacora(BitacoraH datos)
        {
            int result;
            BitacoraH bitacora = new BitacoraH();
            try
            {
                result = _BitacoraData.InsertaBitacora(datos);

            }
            catch (Exception e)
            {
                string resultado = e.Message;
                result = -1;
            }
            return result;
        }

        public int ModificaBitacora(BitacoraH datos)
        {
            int result;
            BitacoraH registro = new BitacoraH();
            try
            {
                result = _BitacoraData.ModificaBitacora(datos);
            }
            catch (Exception e)
            {
               string resultado = e.Message;
                result = -1;
            }
            return result;
        }

        public int EliminarBitacora(int id)
        {
            int result;
            try
            {
                result = _BitacoraData.EliminarBitacora(id);
            }
            catch (Exception e)
            {
                result = -1;
            }

            return result;
        }
    }
}
