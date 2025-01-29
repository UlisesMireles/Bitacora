import { HttpClient } from '@angular/common/http';
import { Component, HostListener, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserIdleService } from 'angular-user-idle';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../../services/authentication.service';
//import { OlvidasteContraseniaComponent } from '../olvidaste-contrasenia/olvidaste-contrasenia.component';
import { MatDialog } from '@angular/material/dialog';
//import { Nom035Service } from 'src/app/services/nom035.service';
import { Globals } from '../../services/globals';
import { Nom035Service } from '../../services/nom035.service';

declare let $: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: false
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  returnUrl: string = "";
  submitted: boolean = false;
  cadena!: any;
  errorLogin: string = "";
  showErrorMessage: boolean = false;
  titulo: any;
  mensaje: any;

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.resize();
  }
  constructor(public dialog: MatDialog, private toastr: ToastrService, private formBuilder: FormBuilder, private route: ActivatedRoute,
    private router: Router, private authenticationService: AuthenticationService, private userIdle: UserIdleService,
    private http: HttpClient, @Inject("BASE_URL") private baseUrl: string,private authenticationServiceNom035:Nom035Service) {
    if (this.authenticationService.currentUserValue) {
      this.authenticationService.logout();

      //this.router.navigate(['/']);
    }
  }
  ngAfterViewInit() {
    this.resize();
  }

  resize() {
    var hei = window.innerHeight;
    var wid = window.innerWidth;

    if (hei >= 658) {
     if (wid < 768) {
       $(".log").css('padding', '45.5% 0%');
     }
     else {
       $(".log").css('padding', '24.87% 0%');
     }
    }
    else if (hei < 658) {
     if (wid < 768) {
       $(".log").css('padding', '22.8% 0%');
     }
     else {
       $(".log").css('padding', '10.85% 0%');
     }
    }
  }
  ngOnInit() {
    localStorage.removeItem("imagen");
    localStorage.clear();
    Globals.pagina = 0;
    Globals.url = ''
    Globals.permisos = [];
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }
  get f() { return this.loginForm.controls; }


  abrirForm() {
    //const dialogRef = this.dialog.open(OlvidasteContraseniaComponent, {
    //  width: '500px', height: '300px'
    //})
  }

  onSubmit() {
    this.submitted = true;
    localStorage.clear();
    if (this.loginForm.invalid) {
      this.showErrorMessage = true;
      this.errorLogin = "Por favor ingrese su usuario y contraseña"
      return;
    }
    this.authenticationService.login(this.loginForm.get('username')?.value, this.loginForm.get('password')?.value)
      .subscribe({
        next: (data: any) => {
          this.cadena = data.idUser;
          if (this.cadena) {
            if (data.estatus == 1) {
              if (data.password == 'True') {
                if (data.temporal == 2) {
                  this.titulo = "Advertencia";
                  this.mensaje = "Se han enviado los pasos para recuperar tu contraseña, por favor revisa tu correo"
                  this.toastr.warning(this.mensaje, this.titulo);
                }
                if (data.temporal == 1) {
                  localStorage.setItem('idUs', data.idUser);
                  this.router.navigate(['/cambio-contraseña'])
                }
                else {
                  Globals.usuario = this.loginForm.get('username')?.value;
                  Globals.rolUser = data.rol;
                  Globals.imagen = 'data:image/jpeg;base64,' + data.foto;
                  Globals.imagenVista = data.foto;
                  if (Globals.permisos.length > 0) {
                    //console.log(Globals.permisos)
                    const index = Globals.permisos.find((x: any) => x.nombrePantalla === "NOM 035") || 0 as number;
                    if (index > -1) {
                      this.authenticationServiceNom035.postInfoParticipante(data.usuario)
                       .subscribe((resp: any) => {
                         if (resp.aplicarExamen > 0) {//retorna al login
                           this.toastr.error("Este usuario esta desactivado debido a que no tiene encuestas pendientes.", "Atención");
                           setTimeout(() => {
                             this.router.navigate(['/']);
                           }, 3000);
                         } else {
                           this.router.navigate(['/nom035']);
                         }
                       });

                    } else {
                      this.router.navigate(['/bitacora/' + this.loginForm.get('username')?.value]);
                    }
                  }
                  else {
                    //console.log("no tiene ningun permiso")
                    this.router.navigate(['/']);
                    this.titulo = "Error";
                    this.mensaje = "Este usuario no cuenta con permisos, por favor contacte con un administrador"
                    this.toastr.error(this.mensaje, this.titulo);

                  }

                }

              }
              else if (data.password == 'False') {
                this.showErrorMessage = true;
                this.errorLogin = "Usuario o contraseña invalido, intenta de nuevo"
              }
              else if (data.password == null) {
                //this.abrirForm();
                var datos = { usuario: this.loginForm.get('username')?.value };
                this.http.get<any>(this.baseUrl + "api/login/RecuperarContrasenia/{id?}", { params: datos }).subscribe(
                  res => {
                    //console.log(res);
                    if (res == 0) {
                      this.titulo = "Correo Enviado";
                      this.mensaje = "Hemos enviado un correo con instrucciones para restablecer tu contraseña. Revisa tu bandeja de entrada."
                      this.toastr.info(this.mensaje, this.titulo);

                    }
                    if (res == -1) {
                      this.titulo = "Error";
                      this.mensaje = "Ha ocurrido un error, verifica tus datos"
                      this.toastr.error(this.mensaje, this.titulo);
                    }
                    if (res == -2) {
                      this.titulo = "Error";
                      this.mensaje = "Este usuario no cuenta con un correo, por favor contacte con un administrador"
                      this.toastr.error(this.mensaje, this.titulo);
                    }
                  }, err => {
                    //console.log(err);
                  }
                )

              }
            }
            else if (data.estatus == 0) {
              this.titulo = "Error";
              this.mensaje = "Su usuario está inactivo, por favor contacte con un administrador"
              this.toastr.error(this.mensaje, this.titulo);
            }
          }
          else if (this.cadena < 0) {
            this.titulo = "Error";
            this.mensaje = "Ocurrio un error, intentalo más tarde"
            this.toastr.error(this.mensaje, this.titulo);
            //console.log(this.cadena)
          }
        },
        error: (err: Error) => {
          this.titulo = "Error";
          this.mensaje = "Ocurrio un error, intentalo más tarde"
          this.toastr.error(this.mensaje, this.titulo);
          //console.log(error);
        }
      });
  }



}
