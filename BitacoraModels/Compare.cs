using System;
using System.Collections.Generic;
using System.Text;
using BitacoraModels;

namespace BitacoraData
{
    public class ComparePersonas : IEqualityComparer<ReportePersonas>
    {
        public bool Equals(ReportePersonas x, ReportePersonas y)
        {
            return x.IdUser == y.IdUser;
        }

        public int GetHashCode(ReportePersonas obj)
        {
            return obj.Id.GetHashCode();
        }
    }
    public class CompareRepDetallado : IEqualityComparer<ReporteDetallado>
    {
        public bool Equals(ReporteDetallado x, ReporteDetallado y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(ReporteDetallado obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class CompareEtapasBitInf : IEqualityComparer<BItacoraInf>
    {
        public bool Equals(BItacoraInf x, BItacoraInf y)
        {
            return x.IdEtapa == y.IdEtapa;
        }

        public int GetHashCode(BItacoraInf obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class CompareActividadesBitInf : IEqualityComparer<BItacoraInf>
    {
        public bool Equals(BItacoraInf x, BItacoraInf y)
        {
            return x.IdActividad == y.IdActividad;
        }

        public int GetHashCode(BItacoraInf obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class CompareUserBitInf : IEqualityComparer<BItacoraInf>
    {
        public bool Equals(BItacoraInf x, BItacoraInf y)
        {
            return x.IdUsr == y.IdUsr;
        }

        public int GetHashCode(BItacoraInf obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class CompareProyectoBitInf : IEqualityComparer<BItacoraInf>
    {
        public bool Equals(BItacoraInf x, BItacoraInf y)
        {
            return x.IdProyecto == y.IdProyecto;
        }

        public int GetHashCode(BItacoraInf obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class CompareUser : IEqualityComparer<Usuarios>
    {
        public bool Equals(Usuarios x, Usuarios y)
        {
            return x.IdUser == y.IdUser;
        }

        public int GetHashCode(Usuarios obj)
        {
            return obj.IdUser.GetHashCode();
        }
    }
}
