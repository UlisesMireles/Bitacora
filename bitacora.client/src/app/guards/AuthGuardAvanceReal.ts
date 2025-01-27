import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import {Globals} from '../services/globals';
import {RedireccionService } from '../services/redireccion.service';
@Injectable()
export class AuthGuardAvanceReal implements CanActivate {

    constructor(private router: Router, private redirige:RedireccionService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (localStorage.getItem('currentUser')) {
            // logged in so return true
            //console.log(Globals.permisos)
            //console.log(this.router.url);
            if(Globals.permisos.length>0){
                //var index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla == "Avance Real"));
              const index = Globals.permisos.find((x: any) => x.nombrePantalla === "AvanceReal") || 0 as number;
              //console.log(index);
                if(index>=0){
                    
                    return true;
                }
                else{
                  var ruta = this.redirige.getUrl(Globals.permisos[0]['nombrePantalla'])
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
                
            }
            else{
                this.router.navigate(['/'], { queryParams: { returnUrl: state.url }});
                return false;
            }
        }

        // not logged in so redirect to login page with the return url
        else{
            this.router.navigate(['/'], { queryParams: { returnUrl: state.url }});
            return false;
        }
    }
}
