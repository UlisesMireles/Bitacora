import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { Globals} from '../services/globals';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
@Injectable({providedIn:'root'})
export class BitacoraService {
 titulo: any;
  mensaje: any;
  baseUrl: string = environment.baseURL;
  constructor(private spinner: NgxSpinnerService,private toastr:ToastrService, private router:Router,private http:HttpClient) { }

  recuperarRegistros(idUsuario:any):Observable<any>{
    var datos = {idUser:idUsuario};
    return this.http.get(this.baseUrl + "api/Bitacora/ConsultaBitacora/{id?}",{params:datos});
  }
  eliminarRegistro(id:any){
    this.spinner.show();
    let datos = {
      id: id
    }
    return this.http.post<any>(this.baseUrl + `api/Bitacora/EliminarBitacora/`, datos)
    .subscribe(result=>{
      //console.log(result);
      if (result > -1) {

        this.titulo = "Registro Eliminado";
        this.mensaje = "El registro ha sido eliminado correctamente"
        this.toastr.success(this.mensaje, this.titulo);
        //this.router.navigate(['/bitacora/' + Globals.usuario]);
        this.router.navigate(['/bitacora/' + Globals.usuario], { queryParams: { refresh: new Date().getTime() } });
        this.spinner.hide();
      }
      else {
        this.titulo = "Error";
        this.mensaje = "Ocurrio un error al tratar de eliminar el registro"
        this.toastr.error(this.mensaje, this.titulo);
        this.router.navigate(['/bitacora/' + Globals.usuario], { queryParams: { refresh: new Date().getTime() } });
        //this.router.navigate(['/bitacora/' + Globals.usuario]);
        this.spinner.hide();
      }
    }, error=>{
      this.spinner.hide();
      //console.log(error);
    });
   
  }
}
