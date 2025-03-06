import { Pipe, PipeTransform } from '@angular/core';
import { Globals } from '../services/globals';

@Pipe({
  name: 'filtroProyectos',
  standalone: false
})
export class FiltroProyectosPipe implements PipeTransform {

  transform(value: any, filtroUno: any, filtroDos: any, filtroTres: any, filtroCuatro: any, filtroCinco: any, filtroSeis: any): any {
    var resultFiltroUno = [];
    var resultFiltroDos = [];
    var resultFiltroTres = [];
    var resultFiltroCuatro = [];
    var resultFiltroCinco = [];
    var resultFiltroSeis = [];
    var resultFiltros = [];

    for (const proyecto of value) {
      if (proyecto.nombre == null) {
        proyecto.nombre = '';
      }
      if (proyecto.nombre.toLowerCase().indexOf(filtroUno.toLowerCase()) > -1) {
        resultFiltroUno.push(proyecto);
      }

    }
    for (const nomCorto of resultFiltroUno) {
      if (nomCorto.nombreCorto == null) {
        nomCorto.nombreCorto = '';
      }
      if (nomCorto.nombreCorto.toLowerCase().indexOf(filtroDos.toLowerCase()) > -1) {
        resultFiltroDos.push(nomCorto);
      }
    }
    for (const sistema of resultFiltroDos) {
      if (sistema.sistema == null) {
        sistema.sistema = '';
      }
      if (sistema.sistema.toLowerCase().indexOf(filtroTres.toLowerCase()) > -1) {
        resultFiltroTres.push(sistema);
      }
    }
    for (const cliente of resultFiltroTres) {
      if (cliente.cliente == null) {
        cliente.cliente = '';
      }
      if (cliente.cliente.toLowerCase().indexOf(filtroCuatro.toLowerCase()) > -1) {
        resultFiltroCuatro.push(cliente);
      }
    }

    for (const estatus of resultFiltroCuatro) {

      if (estatus.estatus == null) {
        estatus.estatus = '';
      }
      if (filtroCinco === '' || estatus.estatus.substring(0, 1).toLowerCase() == filtroCinco.substring(0, 1).toLowerCase()) {
        resultFiltroCinco.push(estatus);
      }
    }

    for (const estatusProceso of resultFiltroCinco) {

      if (estatusProceso.estatusProceso == null) {
        estatusProceso.estatusProceso = '';
      }
      if (estatusProceso.estatusProceso.toLowerCase() == (filtroSeis.toLowerCase())) {
        resultFiltroSeis.push(estatusProceso);
      }
    }

    if (filtroDos !== '') {
      resultFiltroUno = [];
      resultFiltroUno = resultFiltroDos;
    }
    if (filtroTres !== '') {
      resultFiltroUno = [];
      resultFiltroUno = resultFiltroTres;
    }
    if (filtroCuatro !== '') {
      resultFiltroUno = [];
      resultFiltroUno = resultFiltroCuatro;
    }
    if (filtroCinco !== '') {
      resultFiltroUno = [];
      resultFiltroUno = resultFiltroCinco;
    }
    if (filtroSeis !== '') {
      resultFiltroUno = [];
      resultFiltroUno = resultFiltroSeis;
    }
    Globals.datosFiltrados = resultFiltroUno.length;
    return resultFiltroUno;
  }

}
