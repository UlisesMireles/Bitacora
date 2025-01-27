import { Injectable, Inject } from '@angular/core';
import { HttpClient,HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ReportesService {
  constructor(private http:HttpClient, @Inject("BASE_URL") private baseUrl:string ) { }
  //datosReporte;
  getUnidades(id: any):Observable<any>{
    var datos = { idUser: id };
    var params = new HttpParams()
      .set('idUser', datos.idUser);
    return this.http.get<any>(this.baseUrl + "api/AvanceReal/ConsultaUnidad/{id?}",{params});
  }
  getAreas(id: any):Observable<any>{
    var datos={idUser:id};
    return this.http.get<any>(this.baseUrl + "api/AvanceReal/ConsultaArea/{id?}",{params:datos});
  }
  getAreasConUnidad(idUser: any, idUnidad: any):Observable<any>{
    var datos={idUser:idUser, idUnidad : idUnidad}
    return this.http.get<any>(this.baseUrl + "api/AvanceReal/ConsultaAreaRelacionada/{id?}",{params:datos});
  }
  getUnidadConAreas(idUser: any, idArea: any):Observable<any>{
    var datos={idUser:idUser,idArea : idArea}
    return this.http.get<any>(this.baseUrl + "api/AvanceReal/ConsultaUnidadesRelacionadas/{id?}",{params:datos});
  }
  postRecordatorioNom035(_id: any, _email: any) {
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
}
