import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { Globals } from '../services/globals';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { map } from 'rxjs/operators';
import { saveAs } from 'file-saver';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CatalogosService {
  baseUrl: string = environment.baseURL;
  constructor(private http: HttpClient) { }



  getConsultaRolesUsuarios(): Observable<any> {
    return this.http.get<any>(this.baseUrl + "api/Usuarios/ConsultaRolesUsuarios/");
  }


  getConsultaEstatusERTUsuarios(): Observable<any> {
    return this.http.get<any>(this.baseUrl + "api/Usuarios/ConsultaEstatusERTUsuarios/");
  }

  getConsultaEstatusProceso(): Observable<any> {
    return this.http.get<any>(this.baseUrl + "api/ConsultaCatalogos/ConsultaEstatusProceso/");
  }
}
