import { Injectable } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { RedireccionService } from '../../app/services/redireccion.service';
import { Globals } from '../services/globals';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardCatalogos implements CanActivate {

    constructor(private router: Router, private redirige: RedireccionService, activatedRoute: ActivatedRoute) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (localStorage.getItem('currentUser')) {
            let index = -1;

            // Define la URL actual desde el parámetro de ruta
            if (route.url.length > 0) {
                if (route.url[0].path === 'catalogos') {
                    if (route.url[1]) {
                        Globals.url = route.url[1].path;
                    }
                }
            }

            if (Globals.permisos.length > 0) {
                // Encuentra el índice basado en la URL
                const permiso = Globals.permisos.find((x: any) => {
                    switch (Globals.url) {
                        case 'clientes': return x.nombrePantalla === 'Clientes';
                        case 'proyectos': return x.nombrePantalla === 'Proyecto';
                        case 'aplicativos': return x.nombrePantalla === 'Sistemas';
                        case 'usuarios': return x.nombrePantalla === 'Usuarios';
                        case 'unidad-negocio': return x.nombrePantalla === 'Unidad de Negocio';
                        case 'areas-un': return x.nombrePantalla === 'Área de UN';
                        case 'fases': return x.nombrePantalla === 'Etapas';
                        case 'actividades': return x.nombrePantalla === 'Actividades';
                        case 'roles': return x.nombrePantalla === 'Roles';
                        default: return false;
                    }
                });

                // Asegúrate de manejar el tipo correctamente
                if (permiso) {
                    index = Globals.permisos.indexOf(permiso as never); // Usa "as never" para evitar el error de tipo
                }

                if (index >= 0) {
                    return true;
                } else {
                    let ruta = this.redirige.getUrl((Globals.permisos[0] as any).nombrePantalla);

                    if(ruta == '/bitacora'){
                      ruta+='/'+Globals.usuario;
                      this.router.navigate([ruta])
                  }
                  if(ruta == '/reportes'){
                      ruta+='/'+'distribucion';
                  }
                  if(ruta == '/catalogos'){
                      ruta+='/'+'clientes';
                  }
                  else{
                      this.router.navigate([ruta])
                  }

                  return false;
                }
            } else {
                this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
                return false;
            }
        } else {
            // Usuario no logueado, redirige al login
            this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
            return false;
        }
    }
}
