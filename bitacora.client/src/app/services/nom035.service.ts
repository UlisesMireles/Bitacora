import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class Nom035Service {

    constructor(private http: HttpClient, @Inject("BASE_URL") private baseUrl: string) { }

    postResultados(dataPol: any, infusr: any) {
        return this.http.post<any>(`${this.baseUrl}api/Nom035/Resultados`, { "respuestas": dataPol, "infoUsuario": infusr })        
    }
    postInfoParticipante(usuario: any) {
        return this.http.post<any>(`${this.baseUrl}api/Nom035/Infousuario`, { "Usuario": usuario })
    }
}