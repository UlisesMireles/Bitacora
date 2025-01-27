import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportesService {
  baseUrl: string = environment.baseURL;
  constructor(private http:HttpClient) {  }
  //datosReporte;
  getUnidades(id:number):Observable<any>{
    var datos={idUser:id};
    return this.http.get<any>(this.baseUrl + "api/AvanceReal/ConsultaUnidad/{id?}",{params:datos});
  }
  getAreas(id:number):Observable<any>{
    var datos={idUser:id};
    return this.http.get<any>(this.baseUrl + "api/AvanceReal/ConsultaArea/{id?}",{params:datos});
  }
  getAreasConUnidad(idUser:number,idUnidad:number):Observable<any>{
    var datos={idUser:idUser, idUnidad : idUnidad}
    return this.http.get<any>(this.baseUrl + "api/AvanceReal/ConsultaAreaRelacionada/{id?}",{params:datos});
  }
  getUnidadConAreas(idUser:number,idArea:number):Observable<any>{
    var datos={idUser:idUser,idArea : idArea}
    return this.http.get<any>(this.baseUrl + "api/AvanceReal/ConsultaUnidadesRelacionadas/{id?}",{params:datos});
  }
  postRecordatorioNom035(_id:number, _email:string) {
    //this.datosReporte = {id: _id, email:_email};

    return new Promise((resolve: any) => {
      //console.log("Servicio" +_id + _email)      
      this.http.post<any>(`${this.baseUrl}api/Nom035/RecordatorioNom035`, {"id":_id, "email":_email} ).subscribe(data => {
        resolve(data);
      }, err => {
        console.log(err);
      });
    });
  }
  getConsultaDistribucion(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaDistribucion/",datosReporte);
  }

  exportarReporteDistribucion(requestOptions:Object):Observable<any>{
    return this.http.get<any>(this.baseUrl + "api/Reportes/ExportaReporteDistribucion", requestOptions);
  }

  getConsultaDetallado(datosReporte:any,itemsPerPage: number, currentPage:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaDetallado/?pageSize=" + itemsPerPage + "&pageIndex=" + currentPage, datosReporte);
  }

  exportarReporteDetallado(requestOptions:Object):Observable<any>{
    return  this.http.get<any>(this.baseUrl + "api/Reportes/ExportaReporteDetallado/", requestOptions);
  }

  getConsultaPersonas(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaPersonas/", datosReporte);
  }

  exportarReportePersona(requestOptions:Object):Observable<any>{
    return  this.http.get<any>(this.baseUrl + "api/Reportes/ExportaReportePersonas/", requestOptions);
  }

  getConsultaDetalleUsuario(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaDetalleUsuario/", datosReporte);
  }

  getConsultaPersonas_RegistroPorProyecto(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaPersonas_RegistroPorProyecto/", datosReporte);
  }

  getConsultaPersonas_RegistroPorProyectoSemanal(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaPersonas_RegistroPorProyectoSemanal/", datosReporte);
  }

  getConsultaProyectos(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaProyectos/", datosReporte);
  }

  exportarReporteProyectos(requestOptions:Object):Observable<any>{
    return  this.http.get<any>(this.baseUrl + "api/Reportes/ExportaReporteProyectos/", requestOptions);
  }

  getConsultaSemanal(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaSemanal/", datosReporte);
  }

  exportarReporteSemanal(requestOptions:Object):Observable<any>{
    return this.http.get<any>(this.baseUrl + "api/Reportes/ExportaReporteSemanal/", requestOptions);
  }

  getConsultaUsuarios(datos:any):Observable<any>{
    return this.http.get<any>(this.baseUrl + "api/Reportes/ConsultaUsuarios/", { params: datos });
  }

  getConsultaUsuariosSemanal(datos:any):Observable<any>{
    return this.http.get<any>(this.baseUrl + "api/Reportes/ConsultaUsuariosSemanal/", { params: datos });
  }

  getConsultaUsuariosPersona():Observable<any>{
    return this.http.get<any>(this.baseUrl + "api/Reportes/ConsultaUsuariosPersona/");
  }

  getConsultaListaProyectos(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaListaProyectos/", datosReporte);
  }

  getConsultaActividades(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaActividades/", datosReporte);
  }

  getConsultaEtapas(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaEtapas/", datosReporte);
  }

  getConsultaEjecutivo(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaEjecutivo/", datosReporte);
  }

  exportarReporteEjecutivo(requestOptions:Object):Observable<any>{
    return this.http.get<any>(this.baseUrl + "api/Reportes/ExportaReporteEjecutivo/", requestOptions);
  }

  getConsultaClientesDistribucion(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaClientesDistribucion/", datosReporte);
  }

  getConsultaClientesPorProyecto(datosReporte:any):Observable<any>{
    return this.http.post<any>(this.baseUrl + "api/Reportes/ConsultaClientesPorProyecto/", datosReporte);
  }  
}
