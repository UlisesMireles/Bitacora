using System;
using System.Collections.Generic;

namespace BitacoraModels;

public partial class RelacionEtapaEstatus
{
    public int Id { get; set; }

    public int IdEtapa { get; set; }

    public int? IdEstatus { get; set; }

    public bool Activo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
