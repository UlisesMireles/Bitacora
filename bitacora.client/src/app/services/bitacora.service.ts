import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { Globals} from '../services/globals';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
@Injectable({
  providedIn: 'root'
})
@Injectable({providedIn:'root'})
export class BitacoraService {
 titulo: any;
 mensaje: any;
  constructor(private spinner: NgxSpinnerService,private toastr:ToastrService, private router:Router,private http:HttpClient,@Inject("BASE_URL") private baseUrl:string) { }

  recuperarRegistros(idUsuario:any):Observable<any>{
    var datos = {idUser:idUsuario};
    return this.http.get(this.baseUrl + "api/Bitacora/ConsultaBitacora/{id?}",{params:datos});
  }
  eliminarRegistro(id:any){
    this.spinner.show();
    return this.http.delete<any>(this.baseUrl + `api/Bitacora/EliminarBitacora/${id}`)
    .subscribe(result=>{
      //console.log(result);
      this.titulo = "Registro Eliminado";
      this.mensaje = "El registro ha sido eliminado correctamente"
      this.toastr.success(this.mensaje, this.titulo);
      this.router.navigate(['/bitacora/'+Globals.usuario]);
      this.spinner.hide();
    }, error=>{
      this.spinner.hide();
      //console.log(error);
    });
   
  }
}
