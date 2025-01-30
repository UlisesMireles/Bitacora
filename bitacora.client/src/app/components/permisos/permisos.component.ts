import { Component, OnInit, Inject, ChangeDetectorRef } from '@angular/core';
import { Globals } from '../../services/globals';
import { HttpClient } from '@angular/common/http';
import { Roles } from '../../models/roles';
import { RedireccionService } from '../../services/redireccion.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthenticationService } from '../../services/authentication.service';
import { environment } from '../../../environments/environment';
declare var $: any;

@Component({
  selector: 'app-permisos',
  templateUrl: './permisos.component.html',
  styleUrls: ['./permisos.component.css'],
  standalone: false
})
export class PermisosComponent implements OnInit {
  editar: boolean = false;
  pantallasReporte: any[] = [];
  pantallasReporteSel: any[] = [];
  pantallasCatalogo: any[] = [];
  pantallasCatalogoSel: any[] = [];
  pantallasSinMenu: any[] = [];
  pantallasSinMenuSel: any[] = [];
  sinSeleccionados: any[] = [];
  pantallas: any[] = [];
  pantallasTemp: any[] = [];
  pantallasRol: any[] = [];
  nombre: any;
  desc: any;
  rol: any;
  estatus: any;
  idRol: any;
  catalogoIzquierdoVacio: boolean = false;
  catalogoDerechoVacio: boolean = false;
  reporteIzquierdoVacio: boolean = false;
  reporteDerechoVacio: boolean = false;
  titulo: any;
  mensaje: any;
  vacios: boolean = false;
  baseUrl: string = environment.baseURL;

  constructor(private spinner: NgxSpinnerService, private toastr: ToastrService, private router: Router,
    private http: HttpClient, private cdRef: ChangeDetectorRef,
    private authenticationService: AuthenticationService, private redireccionService: RedireccionService) {

  }
  ngAfterViewInit() {
    var rol = localStorage.getItem('rolEditar');
    if (rol == null) {
      this.editar = false;
    }
    else {
    }
  }
  ngAfterViewChecked() {
    if (!localStorage.getItem('currentUser')) {
      this.authenticationService.verificarSesion();
    }
    this.cdRef.detectChanges();
  }
  ngOnInit() {
    var rol = localStorage.getItem('rolEditar');
    if (rol == null) {
      this.editar = false;
      this.getPantallas();
      this.catalogoDerechoVacio = true;
      this.reporteDerechoVacio = true;
    }
    else {
      this.editar = true;
      this.rol = JSON.parse(rol);
      //console.log(this.rol)
      this.nombre = this.rol.rol;
      this.desc = this.rol.descripcion;
      this.idRol = this.rol.idRol;
      this.estatus = this.rol.estatus;
      //console.log(this.idRol)
      this.getPantallasAsignadas(this.idRol);
      //console.log("aqui")
    }
    Globals.pagina = 2;

    if (Globals.pagina == 2) {
      $(".mat-ink-bar").css({ display: 'none' });
    }
    else {
      $(".mat-ink-bar").css({ display: 'block' });
    }
  }

  getPantallas() {
    this.http.get<any>(this.baseUrl + "api/Roles/ConsultaPantallas/{id?}").subscribe({
      next: (res) => {
        //console.log(res)
        this.pantallas = res.pantallas;

        if (res.pantallas.length > 0) {
          var nombreMenu = res.pantallas[0].nombreMenu;
          for (let index = 0; index < this.pantallas.length; index++) {
            if (this.pantallas[index].nombreMenu == 'Reportes') {
              this.pantallasReporte.push(this.pantallas[index]);
            }
            else if (this.pantallas[index].nombreMenu == 'Catalogos') {
              this.pantallasCatalogo.push(this.pantallas[index]);
            }
            else {
              this.pantallasSinMenu.push(this.pantallas[index]);
            }
          }
          //console.log(this.pantallas)
        }
      },
      error: (err) => {
        //console.log(err)
      },
      complete: () => {
        //console.log("complete")
      }
    });
  }

  getPantallasAsignadas(idRol: any) {
    var datos = { idRol: idRol }
    this.http.get<any>(this.baseUrl + "api/Roles/ConsultaPantallasRol/{id?}", { params: datos }).subscribe(
      res => {
        //console.log(res)
        this.pantallas = res.pantallas;
        this.pantallasRol = res.pantallas
        for (let index = 0; index < res.pantallas.length; index++) {
          if (res.pantallas[index].asignada == 1) {
            // if($('option#'+res.pantallas[index].nombrePantalla)){
            //   console.log("lo encuentra");
            //   var event = new Event('dblclick')
            //   $('option#'+res.pantallas[index].nombrePantalla).dblclick();
            // }
            // else{
            //   console.log("no lo encuentra")
            // }
            // $('option#'+res.pantallas[index].nombrePantalla).trigger( "dblclick" );;
            if (res.pantallas[index].nombreMenu == 'Reportes') {
              this.pantallasReporteSel.push(res.pantallas[index]);
            }
            else if (res.pantallas[index].nombreMenu == 'Catalogos') {
              this.pantallasCatalogoSel.push(res.pantallas[index]);
            }
            else {
              this.pantallasSinMenuSel.push(res.pantallas[index]);
            }
          } else {
            if (res.pantallas[index].nombreMenu == 'Reportes') {
              this.pantallasReporte.push(res.pantallas[index]);
            }
            else if (res.pantallas[index].nombreMenu == 'Catalogos') {
              this.pantallasCatalogo.push(res.pantallas[index]);
            }
            else {
              this.pantallasSinMenu.push(res.pantallas[index]);
            }
          }
        }
        this.catalogoIzquierdoVacio = this.pantallasCatalogo.length == 0;
        this.catalogoDerechoVacio = this.pantallasCatalogoSel.length == 0;
        this.reporteIzquierdoVacio = this.pantallasReporte.length == 0;
        this.reporteDerechoVacio = this.pantallasReporteSel.length == 0;
      }, err => {
        //console.log(err)
      }
    )
  }

  guardarRol() {
    this.spinner.show()
    this.sinSeleccionados = Globals.pantallasSeleccionadas;
    console.log(this.sinSeleccionados)
    this.pantallasTemp = [];
    for (let index = 0; index < this.pantallas.length; index++) {
      this.pantallasTemp.push(this.pantallas[index].idPantalla);
    }

    for (let index = 0; index < this.sinSeleccionados.length; index++) {
      var indice = this.pantallasTemp.indexOf(this.pantallasTemp.find(x => this.sinSeleccionados[index] == x));
      //console.log(this.sinSeleccionados[index])
      //console.log(this.pantallasTemp[indice])
      //console.log(indice);
      if (indice !== -1) {
        this.pantallasTemp.splice(indice, 1)
      }
    }
    //console.log(this.sinSeleccionados)

    if (this.editar) {
      if (this.nombre.length != undefined || this.desc != undefined || this.pantallasTemp.length > 0) {
        var rol: Roles = { Rol: this.nombre, Descripcion: this.desc, IdPantallas: this.pantallasTemp, idRol: this.idRol, Estatus: this.estatus };

        if (this.desc == undefined || this.pantallasTemp.length < 1) {
          this.titulo = "Error";
          this.mensaje = "No puedes dejar campos vacíos y debes seleccionar al menos un permiso"
          this.toastr.error(this.mensaje, this.titulo);
          this.spinner.hide();
        }
        else {
          if (this.desc.length < 4) {
            this.titulo = "Advertencia";
            this.mensaje = "El campo descripción deben tener al menos 4 caracteres"
            this.toastr.warning(this.mensaje, this.titulo);
            this.spinner.hide();
          } else {
            this.http.put<any>(this.baseUrl + 'api/Roles/ModificaRol/{id?}', rol).subscribe(
              res => {
                //console.log(res)
                this.titulo = "Rol Actualizado";
                this.mensaje = "El rol ha sido actualizado correctamente"
                this.toastr.success(this.mensaje, this.titulo);

                this.router.navigate(['/catalogos', 'roles'], { replaceUrl: true })
                this.spinner.hide();
              },
              err => {
                this.titulo = "Error";
                this.mensaje = "Ha ocurrido un error al intentar actualizar el rol, intente de nuevo más tarde"
                this.toastr.error(this.mensaje, this.titulo);
                //console.log(err)
                this.spinner.hide();
              }
            )
          }
        }
      }
      else {
        if (this.nombre == undefined || this.desc == undefined || this.pantallasTemp.length < 1) {
          this.titulo = "Error";
          this.mensaje = "No puedes dejar campos vacíos y debes seleccionar al menos un permiso"
          this.toastr.error(this.mensaje, this.titulo);
          this.spinner.hide();
        }
      }
    }
    else {
      var rol: Roles = { Rol: this.nombre, Descripcion: this.desc, IdPantallas: this.pantallasTemp };

      if (this.nombre == undefined || this.desc == undefined || this.pantallasTemp.length < 1) {
        this.titulo = "Error";
        this.mensaje = "No puedes dejar campos vacíos y debes seleccionar al menos un permiso"
        this.toastr.error(this.mensaje, this.titulo);
        this.spinner.hide();
      }
      else {
        if (this.nombre.length < 4 || this.desc.length < 4) {
          this.titulo = "Advertencia";
          this.mensaje = "Los campos nombre y descripción deben tener al menos 4 caracteres"
          this.toastr.warning(this.mensaje, this.titulo);
          this.spinner.hide();
        }
        else {
          this.http.post<any>(this.baseUrl + 'api/Roles/InsertaRol/{id?}', rol).subscribe(
            res => {
              //console.log(res)
              this.titulo = "Rol Insertado";
              this.mensaje = "El rol ha sido insertado correctamente"
              this.toastr.success(this.mensaje, this.titulo);
              this.router.navigate(['/catalogos', 'roles'], { replaceUrl: true })
              this.spinner.hide();
            },
            err => {
              this.titulo = "Error";
              this.mensaje = "Ha ocurrido un error al intentar insertar el rol, intente de nuevo más tarde"
              this.toastr.error(this.mensaje, this.titulo);
              //console.log(err)
              this.spinner.hide();
            }
          )
        }
      }
    }
  }

  guardarArreglo(arreglo: any) {
    for (let index = 0; index < arreglo.length; index++) {
      this.sinSeleccionados.push(arreglo[index]);
    }
  }

  moverDerecha(pantalla: any) {
    if (pantalla.nombreMenu == 'Reportes') {
      this.pantallasReporte = this.pantallasReporte.filter(p => p.idPantalla !== pantalla.idPantalla);
      this.pantallasReporteSel.push(pantalla);
    } else if (pantalla.nombreMenu == 'Catalogos') {
      this.pantallasCatalogo = this.pantallasCatalogo.filter(p => p.idPantalla !== pantalla.idPantalla);
      this.pantallasCatalogoSel.push(pantalla);
    } else {
      this.pantallasSinMenu = this.pantallasSinMenu.filter(p => p.idPantalla !== pantalla.idPantalla);
      this.pantallasSinMenuSel.push(pantalla);
    }
    this.actualizarSeleccionadas();
  }

  moverIzquierda(pantalla: any) {
    if (pantalla.nombreMenu == 'Reportes') {
      this.pantallasReporteSel = this.pantallasReporteSel.filter(p => p.idPantalla !== pantalla.idPantalla);
      this.pantallasReporte.push(pantalla);
    } else if (pantalla.nombreMenu == 'Catalogos') {
      this.pantallasCatalogoSel = this.pantallasCatalogoSel.filter(p => p.idPantalla !== pantalla.idPantalla);
      this.pantallasCatalogo.push(pantalla);
    } else {
      this.pantallasSinMenuSel = this.pantallasSinMenuSel.filter(p => p.idPantalla !== pantalla.idPantalla);
      this.pantallasSinMenu.push(pantalla);
    }
    this.actualizarSeleccionadas();
  }


  moverTodoDerecha() {
    this.pantallasSinMenuSel = [...this.pantallasSinMenuSel, ...this.pantallasSinMenu];
    this.pantallasReporteSel = [...this.pantallasReporteSel, ...this.pantallasReporte];
    this.pantallasCatalogoSel = [...this.pantallasCatalogoSel, ...this.pantallasCatalogo];

    this.pantallasSinMenu = [];
    this.pantallasReporte = [];
    this.pantallasCatalogo = [];

    this.actualizarSeleccionadas();
  }

  moverTodoIzquierda() {
    this.pantallasSinMenu = [...this.pantallasSinMenu, ...this.pantallasSinMenuSel];
    this.pantallasReporte = [...this.pantallasReporte, ...this.pantallasReporteSel];
    this.pantallasCatalogo = [...this.pantallasCatalogo, ...this.pantallasCatalogoSel];

    this.pantallasSinMenuSel = [];
    this.pantallasReporteSel = [];
    this.pantallasCatalogoSel = [];

    this.actualizarSeleccionadas();
  }
  actualizarSeleccionadas() {
    Globals.pantallasSeleccionadas = [
      ...this.pantallasReporteSel.map(p => p.idPantalla),
      ...this.pantallasCatalogoSel.map(p => p.idPantalla),
      ...this.pantallasSinMenuSel.map(p => p.idPantalla),
    ];

    this.catalogoIzquierdoVacio = this.pantallasCatalogo.length == 0;
    this.catalogoDerechoVacio = this.pantallasCatalogoSel.length == 0;
    this.reporteIzquierdoVacio = this.pantallasReporte.length == 0;
    this.reporteDerechoVacio = this.pantallasReporteSel.length == 0;
  }
}
