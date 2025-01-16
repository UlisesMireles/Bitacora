export class Roles {
  idRol?: Number;
  Rol: String;
  Descripcion: String;
  Estatus?: number;
  FechaRegistro?: Date;
  FechaModificacion?: Date;
  ListPantallas?: Array<String>;
  IdPantallas: Array<Number>;

  constructor(
    idRol: Number | undefined,
    Rol: String,
    Descripcion: String,
    Estatus: number | undefined,
    FechaRegistro: Date | undefined,
    FechaModificacion: Date | undefined,
    ListPantallas: Array<String> | undefined,
    IdPantallas: Array<Number>,
  ) {
    this.idRol = idRol;
    this.Rol = Rol;
    this.Descripcion = Descripcion;
    this.Estatus = Estatus;
    this.FechaRegistro = FechaRegistro;
    this.FechaModificacion = FechaModificacion;
    this.ListPantallas = ListPantallas;
    this.IdPantallas = IdPantallas;
  }
}
