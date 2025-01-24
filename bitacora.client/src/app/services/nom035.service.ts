import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class Nom035Service {
    private baseUrl = 'https://localhost:7127/';

    constructor(private http: HttpClient) { }

    postResultados(dataPol: any, infusr: any) {
        return this.http.post<any>(`${this.baseUrl}api/Nom035/Resultados`, { "respuestas": dataPol, "infoUsuario": infusr })
    }
    postInfoParticipante(usuario: any) {
        return this.http.post<any>(`${this.baseUrl}api/Nom035/Infousuario`, { "Usuario": usuario })
    }

    getInformacionEncuestas(): Observable<any> {
        return this.http.post<any>(this.baseUrl + "api/Nom035/InformacionEncuestas/", {});
    }

    getResultadoEncuesta(): Observable<any> {
        return this.http.post<any>(this.baseUrl + "api/Nom035/ConsultaResultadosEncuestas/", {});
    }

    getResultadoEncuestaPorEmpresa(): Observable<any> {
        return this.http.post<any>(this.baseUrl + "api/Nom035/ConsultaResultadosEncuestasTotales/", {});
    }

    getResultadoEncuestaCatPorEmpresa(): Observable<any> {
        return this.http.post<any>(this.baseUrl + "api/Nom035/ConResultEncuestasCategoriaPorEmpresa/", {});
    }

    getResultadoEncuestaDomPorEmpresa(): Observable<any> {
        return this.http.post<any>(this.baseUrl + "api/Nom035/ConResultEncuestasDominioPorEmpresa/", {});
    }
}