import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../../services/authentication.service';
import moment from 'moment';

@Component({
  selector: 'app-token-contrasenia',
  templateUrl: './token-contrasenia.component.html',
  styleUrls: ['./token-contrasenia.component.css'],
  standalone: false
})
export class TokenContraseniaComponent implements OnInit {

  contForm!:FormGroup;

  constructor(private authenticationService:AuthenticationService,private toastr:ToastrService,private router:Router, private activatedRoute:ActivatedRoute, private formBuilder:FormBuilder, private http:HttpClient, @Inject("BASE_URL") private baseUrl:string) { }
  usuario:string = '';
  showErrorMessage:boolean = false;
  errorToken: any;
  caducado:boolean = false;
  titulo: any;
  mensaje: any;
  fechaServidor!: Date;
  ngOnInit() {
    if(localStorage.getItem('currentUser')){
      this.authenticationService.logout();
    }
    
    //localStorage.clear();
    this.contForm=this.formBuilder.group({
      token: ['',Validators.required]
    });
    this.activatedRoute.queryParams.subscribe(params => {
      const data: any = params['U'] || null;
      //console.log(data)
      this.usuario = data;
    });
    var datos = {usuario:this.usuario, token:''};
    this.http.get<any>(this.baseUrl+"api/login/ConsultaToken/{id?}",{params:datos}).subscribe(
      res=>{  
        //console.log(res)
        if(res.temporal != 2){
              this.titulo = "Error";
              this.mensaje = "No se ha solicitado un token"
              this.toastr.error(this.mensaje, this.titulo);
              this.router.navigate(['/']);
        }
      },
      err=>{

      })
      this.recuperarFecha();
  
  }
  get f() { return this.contForm.controls; }

  verificar(){    
        if (this.f['token'].value == '') {
          this.showErrorMessage = true;
          this.errorToken = "Debes ingresar el token para recuperar tu contraseña";
        }
        else {
          var datos = { usuario: this.usuario, token: this.f['token'].value };
          //console.log("token "+this.f.token.value);
          this.http.get<any>(this.baseUrl + "api/login/ConsultaToken/{id?}", { params: datos }).subscribe(
            res => {
              
              this.showErrorMessage = false;
              //console.log(res, moment(res.fechaModificacion), new Date(res.fechaModificacion))

              if (res.password == 'True') {
                if (res.temporal != 2) {
                  this.titulo = "Error";
                  this.mensaje = "No se ha solicitado un token"
                  this.toastr.error(this.mensaje, this.titulo);
                }
                else {
                  var x: Date = new Date(res.fechaModificacion);

                  //console.log("Fecha servidor antes conversion: " + this.fechaServidor)
                  if (this.fechaServidor === undefined) {

                  }
                  var fechaActual = new Date(this.fechaServidor);
                  // console.log(x);
                  //console.log("Fechaa actual : " + fechaActual);
                  //console.log("Fechaa res : " + x);
                  //console.log("dia fecha actual: " + fechaActual.getDay());
                  //console.log("dia fecha res: " + x.getDay());
                  var difDia = fechaActual.getDay() - x.getDay();

                  if (difDia == 0) {
                    var difHoras = fechaActual.getHours() - x.getHours()
                    //console.log("hora fecha actual: " + fechaActual.getHours());
                    //console.log("hora fecha res: " + x.getHours());
                    if (difHoras == 0) {
                      var difMinut = fechaActual.getMinutes() - x.getMinutes()
                      //console.log("minuto  fecha actual: " + fechaActual.getMinutes());
                      //console.log("minuto fecha res: " + x.getMinutes());
                      if (difMinut < 11) {
                        localStorage.setItem('idUs', res.idUser);
                        this.router.navigate(['/cambio-contraseña'])
                      }
                      else {
                        this.showErrorMessage = true;
                        this.caducado = true;
                        this.errorToken = "El token ha caducado, pide otro.";
                      }
                    }
                    else {
                      this.showErrorMessage = true;
                      this.caducado = true;
                      this.errorToken = "El token ha caducado, pide otro.";
                    }

                  }
                  else {
                    this.showErrorMessage = true;
                    this.caducado = true;
                    this.errorToken = "El token ha caducado, pide otro.";
                  }


                }
              }
              else {
                this.showErrorMessage = true;
                this.errorToken = "El token no coincide, intenta de nuevo.";
              }
            },
            err => {
              this.showErrorMessage = true;
              this.caducado = true;
              this.errorToken = "Tiempo de espera de servidor excedido.";
            }
          )
        }
    

    
  }

  solicitarToken(){
    var datos = {usuario:this.usuario};
      this.http.get<any>(this.baseUrl+"api/login/RecuperarContraseniaTokenCad/{id?}",{params:datos}).subscribe(
        res=>{
          //console.log(res)
          if(res==0){
            this.showErrorMessage =false;
            this.errorToken = '';
            //correo envido bootstrap notify
            this.titulo = "Correo Enviado";
            this.mensaje = "Hemos enviado un correo con instrucciones para restablecer tu contraseña. Revisa tu bandeja de entrada."
            this.toastr.info(this.mensaje, this.titulo);
            this.caducado = false;
            
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
          else{
            this.showErrorMessage=true;
            this.errorToken = "El token no coincide, intenta de nuevo.";
          }
        },
        err=>{
          //console.log(err)
        }
      )
  }

  recuperarFecha(){
    this.http.get<any>(this.baseUrl+'api/login/FechaServidor/{id?}').subscribe(
      res=>{
        
        this.fechaServidor = res.fechaServ;
        //console.log(this.fechaServidor, res, moment(res.fechaServ), new Date(res.fechaServ))
      },
      err=>{}
    );
  }

}
