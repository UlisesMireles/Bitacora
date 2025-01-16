export class UnidadesNegocio{
  id: number = 0;
  nombre: string = "";
  fechaRegistro?: Date;
  fechaModificacion?: Date;
  estatus?: number;
  relacion?: number;
  constructor(
    id: number = 0,
    nombre: string = "",
    fechaRegistro: Date | undefined,
    fechaModificacion?: Date,
    estatus?: number,
    relacion?: number
  ) {
    this.id = id;
    this.nombre = nombre;
    this.fechaRegistro = fechaRegistro;
    this.fechaModificacion = fechaModificacion;
    this.estatus = estatus;
    this.relacion = relacion;
  }
}
