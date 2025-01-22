import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportesService {
  private baseUrl = 'https://localhost:7127/';
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
}
