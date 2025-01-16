
import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { User } from '../models/user';
import { Globals } from './globals';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;
  baseUrl: string = environment.baseURL;
  constructor(private http: HttpClient, private router: Router) {
    if (!localStorage.getItem('currentUser')) {

    }
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')!));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(username: any, pass: any) {
    var datos = { usuario: username, password: pass };
    return this.http.get<any>(this.baseUrl + "api/login/Autenticacion", { params: datos })
      .pipe(map(usuario => {
        Globals.permisos = usuario.perm;
        var user = usuario.respuesta;

        if (user.idUser > 0 && user.estatus == 1) {
          if (user.password == 'True') {
            if (user.temporal != 2 && user.temporal != 1) {
              localStorage.setItem('perm', JSON.stringify(usuario.perm));
              localStorage.setItem('currentUser', JSON.stringify(user.idUser));
              localStorage.setItem('userName', username);
              localStorage.setItem('rol', usuario.respuesta.rol);
              localStorage.setItem('imagen', 'data:image/jpeg;base64,' + usuario.respuesta.foto);
              localStorage.setItem('imagenVista', usuario.respuesta.foto);
              this.currentUserSubject.next(user);
            }
          }
        }
        return user;
      }));
  }



  logout() {
    const id = localStorage.getItem('currentUser') || '';
    this.http.get<any>(`${this.baseUrl}api/login/logout/${id}`).subscribe({
      next: res => {
        //console.log(res)
      },
      error: err => {
        //console.log(err)
      }
    });
    localStorage.clear();
    Globals.permisos = [];
    this.currentUserSubject.next(new User(0, ""));
  }
  verificarSesion() {
    if (!localStorage.getItem('currentUser')) {
      this.logout();
      this.router.navigate(['/']);
    }
  }

  ConsultarFoto(username: any) {
    var datos = { usuario: username };
    return this.http.get<any>(this.baseUrl + "api/login/ConsultarFoto/{id?}", { params: datos })
      .pipe(map(imagenData => {
        console.log(imagenData);
        var foto = imagenData.respuesta;
        return foto;
      }));
  }
}
