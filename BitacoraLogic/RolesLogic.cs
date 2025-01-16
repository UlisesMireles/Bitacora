using System;
using System.Collections.Generic;
using System.Text;
using BitacoraData;
using BitacoraModels;

namespace BitacoraLogic
{
    public class RolesLogic
    {
        RolesData _rolesData = new RolesData();

        public List<Roles> ConsultaRoles()
        {
            List<Roles> listaRoles = new List<Roles>();

            listaRoles = _rolesData.ConsultaRoles();

            return listaRoles;
        }

        public List<PermisosPantallas> ConsultaPantallas()
        {
            List<PermisosPantallas> listaPantallas = new List<PermisosPantallas>();

            listaPantallas = _rolesData.ConsultaPantallas();

            return listaPantallas;
        }


        public int InsertaRol(Roles rol)
        {
            var bandera = true;
            int idRol = 0;

            try
            {
                if ((rol.Rol.Trim()).Length < 4)
                    bandera = false;
                if ((rol.Descripcion.Trim()).Length < 4)
                    bandera = false;
                if (rol.IdPantallas.Count < 1)
                    bandera = false;
                if (bandera == true)
                {
                    CatRoles datosRol = new CatRoles();


                    datosRol.Nombre = rol.Rol;
                    datosRol.Descripcion = rol.Descripcion;
                    datosRol.FechaRegistro = DateTime.Now;
                    datosRol.FechaModificacion = DateTime.Now;
                    datosRol.Estatus = 1;

                    idRol = _rolesData.InsertaRol(datosRol);
                }
                else
                    idRol = 0;

                if (idRol > 0) { int res = _rolesData.InsertaRolPantalla(idRol, rol.IdPantallas);}
            }
            catch (Exception e)
            {
                String res = e.Message;
                idRol = -1;
            }

            return idRol;
        }

        public List<PermisosPantallas> ConsultaPantallasRol(int idRol)
        {
            List<PermisosPantallas> listaPantallas = new List<PermisosPantallas>();

            listaPantallas = _rolesData.ConsultaPantallasRol(idRol);

            return listaPantallas;
        }

        public int ModificaRol(Roles rol)
        {
            var bandera = true;
            int idRol = 0;

            try
            {
                if(rol.IdRol < 1)
                    bandera = false;
                if ((rol.Rol.Trim()).Length < 4)
                    bandera = false;
                if ((rol.Descripcion.Trim()).Length < 4)
                    bandera = false;
                if (rol.IdPantallas.Count < 1)
                    bandera = false;
                if (bandera == true)
                    idRol = _rolesData.ModificaRol(rol);
                else
                    idRol = 0;
            }
            catch (Exception e)
            {
                String res = e.Message;
                idRol = -1;
            }

            return idRol;
        }

        public int EliminaRol(int idRol)
        {
            var rol = _rolesData.EliminaRol(idRol);
            return rol;
        }
    }
}
