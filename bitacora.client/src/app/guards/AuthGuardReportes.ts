import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import {Globals} from '../services/globals';
import { RedireccionService } from '../services/redireccion.service';

@Injectable()
export class AuthGuardReportes implements CanActivate {

    constructor(private router: Router, private redirige:RedireccionService) { console.log('qaa') }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (localStorage.getItem('currentUser')) {
            // logged in so return true
            //console.log(Globals.permisos)
            var index = -1;

            if(route.url.length>0){
                if(route.url[0].path=='reportes'){
                    if(route.url[1]){
                        Globals.url = route.url[1].path;
                    }
                }
            }
            if(Globals.permisos.length>0){
                if(Globals.url=='distribucion'){
                    index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla == 'Reporte Distribución'));
                }
                if(Globals.url=='detallado'){
                    index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla == 'Reporte Detallado'));
                }
                if(Globals.url=='persona'){
                    index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla == 'Reporte por Persona'));
                }
                if(Globals.url=='proyecto'){
                    index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla == 'Reporte por Proyecto'));
                }
                if(Globals.url=='proyectoSemanal'){
                    index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla == 'Reporte por Proyecto Semanal'));
                }
                if(Globals.url=='semanal'){
                    index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla == 'Reporte Semanal'));
                }
                if(Globals.url=='ejecutivo'){
                    index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla == 'Reporte Ejecutivo'));
                }
                if(Globals.url=='participacion'){
                    index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla == 'Reporte de Participación'));
                }
                if(Globals.url=='resultNom35'){
                    index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla == 'Reporte Resultados Nom 35'));
                }
               
               
                
                //console.log(index);
                if(index>=0){
                    
                    return true;
                }
                else{
                    var ruta = this.redirige.getUrl(Globals.permisos[0].nombrePantalla)
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
                

//   {path:'catalogos/:catalogo',component: CatalogosComponent,canActivate:[AuthGuard]},
//   {path:'reportes/:reporte',component: ReportesComponent,canActivate:[AuthGuard]},
//   {path:'avance-real',component: AvanceRealComponent,canActivate:[AuthGuard]},
//   {path:'administra-permisos', component: PermisosComponent,canActivate:[AuthGuard]},
//   {path:'cambio-contraseña',component: CambiocontraseniaComponent},
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