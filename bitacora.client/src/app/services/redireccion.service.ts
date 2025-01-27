import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root'
})
export class RedireccionService {

  constructor() { }

  getUrl(texto: string) {
    if (texto == 'Bitácora') {
      return '/bitacora';
    }
    if (texto == 'Reporte Distribución' || texto == 'Reporte Distribubción') {
      return '/reportes/distribucion';
    }
    if (texto == 'Reporte Detallado') {
      return '/reportes/detallado';
    }
    if (texto == 'Reporte por Persona') {
      return '/reportes/persona';
    }
    if (texto == 'Reporte por Proyecto') {
      return '/reportes/proyecto';
    }
    if (texto == 'Reporte Semanal') {
      return '/reportes/semanal';
    }
    if (texto == 'Clientes') {
      return '/catalogos/clientes';
    }
    if (texto == 'Proyecto') {
      return '/catalogos/proyectos';
    }
    if (texto == 'Sistemas') {
      return '/catalogos/aplicativos';
    }
    if (texto == 'Usuarios') {
      return '/catalogos/usuarios';
    }
    if (texto == 'Unidad de Negocio') {
      return '/catalogos/unidad-negocio';
    }
    if (texto == 'Área de UN') {
      return '/catalogos/areas-un';
    }
    if (texto == 'Etapas') {
      return '/catalogos/fases';
    }
    if (texto == 'Actividades') {
      return '/catalogos/actividades';
    }
    if (texto == 'Avance Real') {
      return '/avance-real';
    }
    if (texto == 'Permisos') {
      return '/administra-permisos';
    }
    if (texto == 'NOM 035') {
      return '/nom035';
    }
    return '';
  }

  // dbl(pantalla: any){
  //   $('option#'+pantalla).dblclick();
  //   //console.log("sdsd")
  // }
}
