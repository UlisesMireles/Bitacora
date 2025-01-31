import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../../environments/environment';
import { Globals } from '../../services/globals';

@Component({
  selector: 'app-cambiocontrasenia',
  templateUrl: './cambiocontrasenia.component.html',
  styleUrls: ['./cambiocontrasenia.component.css'],
  standalone: false
})
export class CambiocontraseniaComponent implements OnInit {
  contForm!: FormGroup;
  showErrorMessage: boolean = false;
  titulo: any;
  mensaje: any;
  currentUser: string = '';
  cambio: boolean = false;
  baseUrl: string = environment.baseURL;
  constructor(private toastr: ToastrService, private formBuilder: FormBuilder, private route: Router, private activatedRouter: ActivatedRoute,
    private http: HttpClient) {
  }

  ngOnInit(): void {
    if (localStorage.getItem('currentUser')) {
      this.currentUser = localStorage.getItem('currentUser') ?? '';
    }
    this.contForm = this.formBuilder.group({
      pass: ['', Validators.required],
      confpass: ['', Validators.required]
    });
    this.contForm.get('confpass')?.disable();
  }
  tamPass(pass: string) {
    if (pass.length < 4 || pass.length > 10) {
      this.contForm.get('confpass')?.disable();
    }
    else {
      this.contForm.get('confpass')?.enable();
    }
  }
  verificar() {
    if (this.contForm.invalid) {
      this.cambio = false;
      return;
    }
    else if (this.f['pass'].value == this.f['confpass'].value) {
      if (this.f['pass'].value.length > 3 && this.f['pass'].value.length < 11) {
        var user = localStorage.getItem('idUs');
        this.cambio = true;
        //ejecutar actualizacion de contraseña
        //console.log(user)
        if (this.currentUser != '') {
          user = this.currentUser;
        }
        const datos = { idUser: user || '', password: this.f['pass'].value }
        if (this.cambio) {
          this.http.get(this.baseUrl + "api/Login/CambioContrasenia/{id?}", { params: datos }).subscribe(res => {
            //console.log(res);
            this.titulo = "Contraseña Actualizada";
            this.mensaje = "La contraseña ha sido actualizada correctamente"
            this.toastr.success(this.mensaje, this.titulo);
            localStorage.removeItem('idUs');
          }, err => {
            //console.log(err);
            this.titulo = "Error";
            this.mensaje = "Ha ocurrido un error al intentar actualizar la contraseña, por favor intentalo más tarde"
            this.toastr.error(this.mensaje, this.titulo);
            localStorage.removeItem('idUs');
          }
          )
        }
        if (this.currentUser != '') {
          this.route.navigate(['/bitacora/' + Globals.usuario]);
        }
        else {
          this.route.navigate(['/login']);
        }
      }
      else {
        this.titulo = "Error";
        this.mensaje = "La contraseña debe contener entre 4 y 10 caracteres"
        this.toastr.error(this.mensaje, this.titulo);
      }
    }
    else {
      this.showErrorMessage = true;
    }
  }

  get f() { return this.contForm.controls; }

}
