export class CatUsuarios {
  Estatus?: number;
  fechaModificacion?: Date;
  fechaRegistro?: Date;
  IdRol?: number;
  IdTipoNavigation?: number;
  IdUser?: number;
  IdUsrElimino?: number;
  IdUsrModificacion?: number;
  IdUsrRegistro?: number;
  Password?: string;
  Usuario?: string;
  Nombre?: string;
  Rol?: string;
  temporal?: string;
  Registro?: number;
  IdEmpleado?: number;
  Email?: string;
  ListaUnidadArea?: string;

  constructor(
    Estatus?: number,
    fechaModificacion?: Date,
    fechaRegistro?: Date,
    IdRol?: number,
    IdTipoNavigation?: number,
    IdUser?: number,
    IdUsrElimino?: number,
    IdUsrModificacion?: number,
    IdUsrRegistro?: number,
    Password?: string,
    Usuario?: string,
    Nombre?: string,
    Rol?: string,
    temporal?: string,
    Registro?: number,
    IdEmpleado?: number,
    Email?: string,
    ListaUnidadArea?: string
  ) {
    this.Estatus = Estatus;
    this.fechaModificacion = fechaModificacion;
    this.fechaRegistro = fechaRegistro;
    this.IdRol = IdRol;
    this.IdTipoNavigation = IdTipoNavigation;
    this.IdUser = IdUser;
    this.IdUsrElimino = IdUsrElimino;
    this.IdUsrModificacion = IdUsrModificacion;
    this.IdUsrRegistro = IdUsrRegistro;
    this.Password = Password;
    this.Usuario = Usuario;
    this.Nombre = Nombre;
    this.Rol = Rol;
    this.temporal = temporal;
    this.Registro = Registro;
    this.IdEmpleado = IdEmpleado;
    this.Email = Email;
    this.ListaUnidadArea = ListaUnidadArea;
  }
}
