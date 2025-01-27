import { Pipe, PipeTransform } from '@angular/core';
import { Globals } from '../services/globals';

@Pipe({
  name: 'filtroBitacora',
  standalone: false
})
export class FiltroBitacoraPipe implements PipeTransform {

  transform(value: any,texto: any) :any{
    var resultFiltro = [];
    for(const registro of value){
      if(registro.fecha.toLowerCase().indexOf(texto.toLowerCase()) > -1 ||
      registro.proyecto.toLowerCase().indexOf(texto.toLowerCase()) > -1 ||
      registro.activadad.toLowerCase().indexOf(texto.toLowerCase()) > -1 ||
      registro.etapa.toLowerCase().indexOf(texto.toLowerCase()) > -1 ||
      registro.duracion.toString().toLowerCase().indexOf(texto.toLowerCase()) > -1 ||
      registro.descripcion.toLowerCase().indexOf(texto.toLowerCase()) > -1){
        resultFiltro.push(registro);
      }
    }
    Globals.filtroBitacora = resultFiltro.length;
    return resultFiltro;
  }

}
