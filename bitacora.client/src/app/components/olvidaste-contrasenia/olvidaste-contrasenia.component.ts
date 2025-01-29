import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-olvidaste-contrasenia',
  standalone: false,
  
  templateUrl: './olvidaste-contrasenia.component.html',
  styleUrl: './olvidaste-contrasenia.component.css'
})
export class OlvidasteContraseniaComponent implements OnInit {
  showErrorMessage: boolean = false;
  errorLogin: any;
  titulo: any;
  mensaje: any;
  form: any;

  constructor(private formBuilder:FormBuilder,private toastr:ToastrService, private http:HttpClient,@Inject("BASE_URL") private baseUrl:string,private dialog:MatDialog) { }

  ngOnInit() {
    this.form=this.formBuilder.group({
      username: ['',Validators.required]
    });
  }
  get f() { return this.form.controls; }
  recuperarContrasenia(){
    if(this.f.username.value==''){
        this.showErrorMessage=true;
        this.errorLogin = "Debes ingresar tu usuario para recuperar tu contraseña";
    }
    else{
      this.showErrorMessage = false;
      var datos = {usuario:this.f.username.value};
      this.http.get<any>(this.baseUrl+"api/login/RecuperarContrasenia/{id?}",{params:datos}).subscribe(
        res=>{
          //console.log(res);
          if(res==0){
            //correo envido bootstrap notify
            this.titulo = "Correo Enviado";
            this.mensaje = "Hemos enviado un correo con instrucciones para restablecer tu contraseña. Revisa tu bandeja de entrada."
            this.toastr.info(this.mensaje, this.titulo);
            this.dialog.closeAll();
          }
          if(res==-1){
            this.titulo = "Error";
            this.mensaje = "Ha ocurrido un error, verifica tus datos"
            this.toastr.error(this.mensaje, this.titulo);
          }
          if(res==-2){
            this.titulo = "Error";
            this.mensaje = "Este usuario no cuenta con un correo, por favor contacte con un administrador"
            this.toastr.error(this.mensaje, this.titulo);
          }
        }, err=>{
          //console.log(err);
        }
      )
    }
  }
}
