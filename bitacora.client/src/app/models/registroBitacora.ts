export class RegistroBitacora{
  id: number;
  fecha: Date;
  IdUser: number;
  idProyecto: number;
  idEtapa: number;
  idActividad: number;
  descripcion: string;
  duracion: number;
  fechaRegistro: Date;
  fechaModificacion?: Date;
  constructor(
    id: number,
    fecha: Date,
    IdUser: number,
    idProyecto: number,
    idEtapa: number,
    idActividad: number,
    descripcion: string,
    duracion: number,
    fechaRegistro: Date,
    fechaModificacion?: Date
  ) {
    this.id = id;
    this.fecha = fecha;
    this.IdUser = IdUser;
    this.idProyecto = idProyecto;
    this.idEtapa = idEtapa;
    this.idActividad = idActividad;
    this.descripcion = descripcion;
    this.duracion = duracion;
    this.fechaRegistro = fechaRegistro;
    this.fechaModificacion = fechaModificacion;
  }


}
