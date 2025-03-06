import { Pipe, PipeTransform } from '@angular/core';
import { Globals } from '../services/globals';
@Pipe({
  name: 'filtro',
  standalone: false
})
export class FiltroPipe implements PipeTransform {

  transform(value: any,catalogo:string, filtroUno:any, filtroDos:any, filtroTres:any, filtroCuatro:any, filtroCinco:any, filtroSeis:any, filtroSiete:any, filtroOcho:any): any {
    var resultFiltroUno = [];
    var resultFiltroDos = [];
    var resultFiltroTres = [];
    var resultFiltroCuatro: string[] = [];
    var resultFiltroCinco = [];
    var resultFiltros = [];

    var resultFiltroUnoUsuarios = [];
    var resultFiltroDosUsuarios = [];
    var resultFiltroTresUsuarios = [];
    var resultFiltroCuatroUsuarios = [];
    var resultFiltroCincoUsuarios = [];
    var resultFiltrosUsuarios = [];
    var resultFiltroSieteUsuarios = [];
    var resultFiltroOchoUsuarios = [];
    if(catalogo=='unidadesNegocio'){

      for(const texto of value){
        if(texto.unidad==null){
          texto.unidad='';
        }
        if(texto.unidad.toLowerCase().indexOf(filtroUno.toLowerCase()) > -1){
          resultFiltroUno.push(texto);
        };
      };
      for(const estatus of resultFiltroUno){

        if(estatus.estatus==null || estatus.estatus == undefined){
          estatus.estatus='';
        }
        if(estatus.estatus.substring(0,1).toLowerCase()==(filtroDos.substring(0,1).toLowerCase())){
          resultFiltros.push(estatus);
        }
        // if(estatus.estatus.toLowerCase().indexOf(filtroDos.toLowerCase()) > -1){
        //   resultFiltros.push(estatus);
        // }
      }

      for(const area of resultFiltros ){

        if(area.areas==null){
          area.areas='';
        }
        var x="";
        var loContiene:boolean=false;

        for (let index = 0; index < area.areas.length; index++) {

          x=area.areas[index];
          x=x.replace(" ","");

          if(x.toLowerCase().indexOf(filtroTres.toLowerCase()) > -1){
            loContiene=true;
          }
        }
        if(loContiene==true){
          resultFiltroDos.push(area);
        }



      }

    }
    else if(catalogo=='areasUnidades'){
      for(const texto of value){

        if(texto.nombre.toLowerCase().indexOf(filtroUno.toLowerCase()) > -1){
          resultFiltroUno.push(texto);
        };
      };

      for(const estatus of resultFiltroUno){
        //console.log(estatus)

        if(estatus.estatus.substring(0,1).toLowerCase()==(filtroDos.substring(0,1).toLowerCase())){
          resultFiltros.push(estatus);
        }
      }
    }

    else if(catalogo=='usuarios'){
      // filtros usuario nombre tipo-usuario(rol) estatus
      for(const nombre of value){

        if(nombre.nombre==null){
          nombre.nombre='';
        }
        if(nombre.nombre.toLowerCase().indexOf(filtroDos.toLowerCase()) > -1){
          resultFiltroUnoUsuarios.push(nombre);
        }
      }
      for(const usuario of resultFiltroUnoUsuarios){
        if(usuario.usuario.toLowerCase().indexOf(filtroUno.toLowerCase()) > -1){
          resultFiltrosUsuarios.push(usuario);
        }
      }
      //filtros correos
      for(const tipoUs of resultFiltrosUsuarios ){

        if(tipoUs.email==null){
          tipoUs.email='';
        }

        if(tipoUs.email.toLowerCase().indexOf(filtroSiete.toLowerCase()) > -1){
          resultFiltroSieteUsuarios.push(tipoUs);
        }
      }

      for(const tipoUs of resultFiltroSieteUsuarios ){

        if(tipoUs.emailAsignado==null){
          tipoUs.emailAsignado='';
        }

        if(tipoUs.emailAsignado.toLowerCase().indexOf(filtroOcho.toLowerCase()) > -1){
          resultFiltroOchoUsuarios.push(tipoUs);
        }
      }

      ///termina filtro correos


      for(const tipoUs of resultFiltroOchoUsuarios ){

        if(tipoUs.rol==null){
          tipoUs.rol='';
        }

        if(tipoUs.rol.toLowerCase().indexOf(filtroTres.toLowerCase()) > -1){
          resultFiltroDosUsuarios.push(tipoUs);
        }
      }

      for (const bitEstatus of resultFiltroDosUsuarios) {

        if (bitEstatus.estatusERT == null) {
          bitEstatus.estatusERT = '';
        }
        if(bitEstatus.estatusERT.toLowerCase().indexOf(filtroSeis.toLowerCase()) > -1){

          resultFiltroTresUsuarios.push(bitEstatus);
        }
      }
      ///se comenta codigo original para aplicar el cambio de hacer combo registro bitacora y el combo estatus ponerle la opcion todos

      //for (const bit of resultFiltroTresUsuarios) {

      //  if (bit.llenaBitacora == null) {
      //    bit.llenaBitacora = '';
      //  }
      //  if (bit.llenaBitacora.substring(0, 1).toLowerCase().indexOf(filtroCinco.substring(0, 1).toLowerCase()) > -1) {
      //    resultFiltroCuatroUsuarios.push(bit);
      //  }
      //}
      //for(const estatus of resultFiltroCuatroUsuarios ){

      //  if(estatus.estatus==null || estatus.estatus==undefined){
      //    estatus.rol='';
      //  }
      //  if(estatus.estatus.substring(0,1).toLowerCase()==(filtroCuatro.substring(0,1).toLowerCase())){
      //    resultFiltroCincoUsuarios.push(estatus);
      //  }

      //}

      for (const bit of resultFiltroTresUsuarios) {

        if (bit.llenaBitacora == null) {
          bit.llenaBitacora = '';
        }

        // Si el usuario selecciona "Todos" (valor vacío ""), incluimos todos los elementos
        if (filtroCinco === '') {
          resultFiltroCuatroUsuarios.push(bit);
        }
        // Si el valor no es "Todos", filtramos normalmente
        else if (bit.llenaBitacora.toLowerCase() === filtroCinco.toLowerCase()) {
          resultFiltroCuatroUsuarios.push(bit);
        }
      }

      for (const estatus of resultFiltroCuatroUsuarios) {

        // Si el estatus es null o undefined, asignamos un valor vacío
        if (estatus.estatus == null || estatus.estatus == undefined) {
          estatus.rol = '';
        }

        // Si filtroCuatro es "0", se debe incluir tanto "activo" como "inactivo"
        if (filtroCuatro === '0') {
          resultFiltroCincoUsuarios.push(estatus);
        }
        // Si filtroCuatro no es "0", se compara el primer carácter de "estatus" y "filtroCuatro" sin distinguir mayúsculas
        else if (estatus.estatus.substring(0, 1).toLowerCase() === filtroCuatro.substring(0, 1).toLowerCase()) {
          resultFiltroCincoUsuarios.push(estatus);
        }
      }

    }

    else if(catalogo=='clientes'){
      // filtros nombre alias giro estatus
      for(const cliente of value){
        if(cliente.nombre.toLowerCase().indexOf(filtroUno.toLowerCase()) > -1){
          resultFiltroUno.push(cliente);
        }
      }
      for(const alias of resultFiltroUno){

        if(alias.alias==null){
          alias.alias='';
        }
        if(alias.alias.toLowerCase().indexOf(filtroDos.toLowerCase()) > -1){
          resultFiltros.push(alias);
        }
      }
      for(const giro of resultFiltros ){

        if(giro.giro==null){
          giro.giro='';
        }

        if(giro.giro.toLowerCase().indexOf(filtroTres.toLowerCase()) > -1){
          resultFiltroDos.push(giro);
        }
      }
      for(const estatus of resultFiltroDos){

        if(estatus.estatus==null){
          estatus.estatus='';
        }
        // if(estatus.estatus.toLowerCase().indexOf(filtroDos.toLowerCase()) > -1){
        //   resultFiltros.push(estatus);
        // }
        if(estatus.estatus.substring(0,1).toLowerCase()==(filtroCuatro.substring(0,1).toLowerCase())){
          resultFiltroTres.push(estatus);
        }
      }


    }
    else if(catalogo=='actividades'){
      for(const actividad of value){
        if(actividad.nombre==null){
          actividad.nombre='';
        }
        if(actividad.nombre.toLowerCase().indexOf(filtroUno.toLowerCase()) > -1){
          resultFiltroUno.push(actividad);
        };
      };
      for(const evento of resultFiltroUno){
        if(evento.evento==null){
          evento.evento='';
        }
        if(evento.evento.toLowerCase().indexOf(filtroDos.toLowerCase()) > -1){
          resultFiltros.push(evento);
        }

      }
      for(const estatus of resultFiltros ){

        if(estatus.estatus==null){
          estatus.estatus='';
        }
        // if(estatus.estatus.toLowerCase().indexOf(filtroDos.toLowerCase()) > -1){
        //   resultFiltros.push(estatus);
        // }
        if(estatus.estatus.substring(0,1).toLowerCase()==(filtroTres.substring(0,1).toLowerCase())){
          resultFiltroDos.push(estatus);
        }
      }
    }
    else if(catalogo=='sistemas'){
      // filtros aplicativo cliente estatus

      for(const aplicativo of value){
        if(aplicativo.nombre==null){
          aplicativo.nombre='';
        }
        if(aplicativo.nombre.toLowerCase().indexOf(filtroUno.toLowerCase()) > -1){
          resultFiltroUno.push(aplicativo);
        }
      }
      for(const cliente of resultFiltroUno){

        if(cliente.cliente==null){
          cliente.cliente='';
        }
        if(cliente.cliente.toLowerCase().indexOf(filtroDos.toLowerCase()) > -1){
          resultFiltros.push(cliente);
        }
      }
      for(const estatus of resultFiltros ){

        if(estatus.estatus==null){
          estatus.estatus='';
        }
        if(estatus.estatus.substring(0,1).toLowerCase()==(filtroTres.substring(0,1).toLowerCase())){
          resultFiltroDos.push(estatus);
        }
        // if(estatus.estatus.toLowerCase().indexOf(filtroTres.toLowerCase()) > -1){
        //   resultFiltroDos.push(estatus);
        // }
      }
    }
    else if(catalogo=='proyectos'){
      // filtros aplicativo cliente estatus
      for(const proyecto of value){

          resultFiltroUno.push(proyecto);

      }
    }
    else if(catalogo=='etapas'){
      for(const texto of value){

        if(texto.nombre.toLowerCase().indexOf(filtroUno.toLowerCase()) > -1){
          resultFiltroUno.push(texto);
        };
      };

      for(const estatus of resultFiltroUno){
        //console.log(estatus)

        if(estatus.estatus.substring(0,1).toLowerCase()==(filtroDos.substring(0,1).toLowerCase())){
          resultFiltros.push(estatus);
        }
      }
    }

    if(catalogo=='usuarios'){
      if(filtroDos!==''){
        resultFiltroUno = [];
        resultFiltroUno = resultFiltroUnoUsuarios;
      }
      if(filtroSiete!==''){
        resultFiltroUno = [];
        resultFiltroUno = resultFiltroSieteUsuarios;
      }
      if(filtroOcho!==''){
        resultFiltroUno = [];
        resultFiltroUno = resultFiltroOchoUsuarios;
      }
      if(filtroTres!==''){
        resultFiltroUno = []
        resultFiltroUno = resultFiltroDosUsuarios;
      }
      if(filtroSeis!==''){
        resultFiltroUno=[];
        resultFiltroUno = resultFiltroTresUsuarios;
      }
      if(filtroCinco!==''){
        resultFiltroUno=[];
        resultFiltroUno = resultFiltroCuatroUsuarios
      }
      if(filtroCuatro!==''){
        resultFiltroUno = [];
        resultFiltroUno = resultFiltroCincoUsuarios;
      }
    }else{
      if(filtroDos!==''){
        resultFiltroUno = [];
        resultFiltroUno = resultFiltros;
      }
      if(filtroTres!==''){
        resultFiltroUno = []
        resultFiltroUno = resultFiltroDos;
      }
      if(filtroCuatro!==''){
        resultFiltroUno = [];
        resultFiltroUno = resultFiltroTres;
      }
      if(filtroCinco!==''){
        resultFiltroUno=[];
        resultFiltroUno = resultFiltroCuatro;
      }

    }

    Globals.datosFiltrados= resultFiltroUno.length;
    return resultFiltroUno;
  }

}

