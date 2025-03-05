using System;
using System.Collections.Generic;

namespace BitacoraModels;

public partial class RelacionActividadEstatus
{
    public int Id { get; set; }

    public int IdActividad { get; set; }

    public int? IdEstatus { get; set; }

    public bool Activo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
