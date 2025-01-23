import { Component, HostListener, OnInit, ViewEncapsulation, ChangeDetectorRef, Directive, EventEmitter, ElementRef, Output } from '@angular/core';
import { User } from './models/user';
import { NavigationEnd, Router } from '@angular/router';
import { AuthenticationService } from './services/authentication.service';
import { Globals } from './services/globals';
import { Subject } from 'rxjs';
import { UserIdleService } from 'angular-user-idle';
import { VERSION } from '@angular/material/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { RedireccionService } from './services/redireccion.service';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';
import { HttpClient } from '@angular/common/http';
import $ from "jquery";
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html', styleUrls: ['./app.component.css'], encapsulation: ViewEncapsulation.None,
  standalone: false
})

export class AppComponent implements OnInit {
  public backgroundImg!: SafeStyle;
  version = VERSION;
  mode = 'side'
  opened = false;
  layoutGap = '64';
  fixedInViewport = true;
  userActivity: any;
  userInactive: Subject<any> = new Subject();
  selectedTab = 0;
  currentUser: User = new User(0, '');
  menu1 = true;
  menu2 = false;
  menu3 = false;
  bitacoraMenu: boolean = false;
  reportesMenu: boolean = false;
  distribucion: boolean = false;
  detallado: boolean = false;
  persona: boolean = false;
  proyecto: boolean = false;
  semanal: boolean = false;
  ejecutivo: boolean = false;
  proyectoSemanal: boolean = false;
  catalogosMenu: boolean = false;
  clientesOpc: boolean = false;
  proyectoOpc: boolean = false;
  sistemasOpc: boolean = false;
  usuariosOpc: boolean = false;
  unidadNegocioOpc: boolean = false;
  areaNegocioOpc: boolean = false;
  etapaOpc: boolean = false;
  actividadesOpc: boolean = false;
  rolesOpc: boolean = false;
  avanceMenu: boolean = false;
  permisosMenu: boolean = false;
  permisos035: boolean = false;
  participacion: boolean = false;
  mostrarMenu: boolean = false;
  resultNom35: boolean = false;
  rol: string = '';
  foto: any;
  changeTab(tabIndex: number) {
    this.selectedTab = tabIndex;
  }
  public destroyed = new Subject<any>();

  constructor(private readonly router: Router, private readonly authenticationService: AuthenticationService, private readonly userIdle: UserIdleService, private readonly cdRef: ChangeDetectorRef,
    public readonly dialog: MatDialog, private readonly redirige: RedireccionService, private readonly elementRef: ElementRef, private readonly sanitizer: DomSanitizer, private readonly http: HttpClient) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
    // this.router.routeReuseStrategy.shouldReuseRoute = function(){
    //   return false;
    // }
    this.router.events.subscribe((evt) => {
      if (evt instanceof NavigationEnd) {
        // trick the Router into believing its last link wasn't previously loaded
        this.router.navigated = false;
        // if you need to scroll back to top, here is the right place
        window.scrollTo(0, 0);
      }
    });

    this.setTimeout();
    this.mostrarMenu = false;
    this.userInactive.subscribe(() => this.logout())
  }

  @HostListener('document:click', ['$event'])
  public onDocumentClick(event: MouseEvent): void {
    const targetElement = event.target as HTMLElement;
    // Check if the click was outside the element
    if (targetElement && this.elementRef.nativeElement.contains(targetElement)) {
      $(".dropdown-user-menu").css({ display: 'none' });
    }
  }

  ngAfterViewInit() {
    this.resize();

  }

  resize() {
    let hei = window.innerHeight;
    if (hei >= 658) {
      $(".resize").height(848);
    }
    else if (hei < 658 && hei == 557) {
      $(".resize").height(477);
    } else {
      $(".resize").height(477);
    }


  }

  ngAfterViewChecked() {

    this.resize();
    if (localStorage.getItem('perm')) {
      Globals.permisos = JSON.parse(localStorage.getItem('perm') || '{}') as []; // Add type assertion
      if (localStorage.getItem('perm')) {
        const perm = localStorage.getItem('perm');
        if (perm) {
          Globals.permisos = JSON.parse(perm);
        }
        for (const permiso of Globals.permisos) {
          if (permiso['nombreMenu'] == "Bitácora") {
            this.bitacoraMenu = true;
          }
          if (permiso['nombreMenu'] == "Avance Real") {
            this.avanceMenu = true;
          }
          if (permiso['nombreMenu'] == "Reportes") {
            this.reportesMenu = true;
          }
          if (permiso['nombrePantalla'] == "Reporte Distribución") {
            this.distribucion = true;
          }
          if (permiso['nombrePantalla'] == "Reporte Detallado") {
            this.detallado = true;
          }
          if (permiso['nombrePantalla'] == "Reporte por Persona") {
            this.persona = true;
          }
          if (permiso['nombrePantalla'] == "Reporte por Proyecto") {
            this.proyecto = true;
          }
          if (permiso['nombrePantalla'] == "Reporte por Proyecto Semanal") {
            this.proyectoSemanal = true;
          }
          if (permiso['nombrePantalla'] == "Reporte Semanal") {
            this.semanal = true;
          }
          if (permiso['nombrePantalla'] == "Reporte Ejecutivo") {
            this.ejecutivo = true;
          }
          if (permiso['nombreMenu'] == "Catalogos") {
            this.catalogosMenu = true;
          }
          if (permiso['nombrePantalla'] == "Clientes") {
            this.clientesOpc = true;
          }
          if (permiso['nombrePantalla'] == "Proyecto") {
            this.proyectoOpc = true;
          }
          if (permiso['nombrePantalla'] == "Sistemas") {
            this.sistemasOpc = true;
          }
          if (permiso['nombrePantalla'] == "Usuarios") {
            this.usuariosOpc = true;
          }
          if (permiso['nombrePantalla'] == "Unidad de Negocio") {
            this.unidadNegocioOpc = true;
          }
          if (permiso['nombrePantalla'] == "Área de UN") {
            this.areaNegocioOpc = true;
          }
          if (permiso['nombrePantalla'] == "Etapas") {
            this.etapaOpc = true;
          }
          if (permiso['nombrePantalla'] == "Actividades") {
            this.actividadesOpc = true;
          }
          if (permiso['nombrePantalla'] == "Roles") {
            this.permisosMenu = true;
          }
          if (permiso['nombrePantalla'] == "NOM 035") {
            this.permisos035 = true;
          }
          if (permiso['nombrePantalla'] == "Reporte de Participación") {
            this.participacion = true;
          }
          if (permiso['nombrePantalla'] == "Reporte Resultados Nom 35") {
            this.resultNom35 = true;
          }
        }


      }
    }
    else {
      //this.authenticationService.logout();

      this.bitacoraMenu = false;
      this.avanceMenu = false;
      this.reportesMenu = false;
      this.distribucion = false;
      this.detallado = false;
      this.persona = false;
      this.proyecto = false;
      this.semanal = false;
      this.catalogosMenu = false;
      this.clientesOpc = false;
      this.proyectoOpc = false;
      this.sistemasOpc = false;
      this.usuariosOpc = false;
      this.unidadNegocioOpc = false;
      this.areaNegocioOpc = false;
      this.etapaOpc = false;
      this.actividadesOpc = false;
      this.permisosMenu = false;
      this.permisos035 = false;
      this.participacion = false;
      this.mostrarMenu = false;
      this.resultNom35 = false;
    }
    ////console.log(JSON.parse(localStorage.getItem('perm')));
    //Globals.permisos = JSON.parse(localStorage.getItem('perm'));

    this.cdRef.detectChanges();
  }
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.resize();
    Globals.ventana = window.innerWidth;

    if (Globals.pagina == 2) {
      $(".mat-ink-bar").css({ display: 'none' });
    }
    else {
      $(".mat-ink-bar").css({ display: 'block' });
    }
    ////console.log('bandera movil '+Globals.movil)
    ////console.log('pag '+Globals.pagina)
    if (Globals.ventana >= 992) {
      Globals.movil = false;
      this.opened = false;
      this.menu1 = true;
      this.menu2 = false;
      this.menu3 = false;
    }
    else if (Globals.ventana < 992) {
      Globals.movil = true;
    }

  }

  ngOnInit() {
    this.banderasFalse();
    //Globals.url='';
    if (this.authenticationService.currentUserValue) {
      Globals.usuario = localStorage.getItem('userName') as string;
      Globals.rolUser = localStorage.getItem('rol') as string;
      Globals.imagen = localStorage.getItem('imagen') as string;
      Globals.imagenVista = localStorage.getItem('imagenVista') as string;
      // if(this.imagen != 'null'){
      //   var imagenBase = this.imagen;
      //   let objectURL = 'data:image/jpeg;base64,' + imagenBase;
      //   this.foto = this.sanitizer.bypassSecurityTrustUrl(objectURL);
      // } 
      // this.authenticationService.ConsultarFoto(this.usuario).subscribe(data=>{
      //   var stringImagen = data.foto;
      //   if(stringImagen != 'null'){
      //     var imagenBase = stringImagen;
      //     let objectURL = 'data:image/jpeg;base64,' + imagenBase;
      //     this.foto = this.sanitizer.bypassSecurityTrustUrl(objectURL);

      //   }
      // });
    }
    Globals.ventana = window.innerWidth;
    ////console.log('ventana ancho ' + Globals.ventana);
    if (Globals.ventana >= 992) {
      Globals.movil = false;

    }
    else if (Globals.ventana < 992) {
      Globals.movil = true;
      ////console.log('pagina' + Globals.pagina)
      if (Globals.pagina == 0 || Globals.pagina == 1) {
        Globals.mostrarRouterOutlet = false;
      }
      else if (Globals.pagina == 2 || Globals.pagina == 5) {
        Globals.mostrarRouterOutlet = true;
      }
    }
  }
  abrirPerfilUsuario() {
    $(".dropdown-user-menu").css({ display: 'block' });
  }
  logout() {
    this.foto = null;
    localStorage.clear();
    this.mostrarMenu = false;
    this.authenticationService.logout();
    this.banderasFalse();
    this.router.navigate(['/']);
  }
  banderasFalse() {
    this.bitacoraMenu = false;
    this.reportesMenu = false;
    this.distribucion = false;
    this.detallado = false;
    this.persona = false;
    this.proyecto = false;
    this.semanal = false;
    this.catalogosMenu = false;
    this.clientesOpc = false;
    this.proyectoOpc = false;
    this.sistemasOpc = false;
    this.usuariosOpc = false;
    this.unidadNegocioOpc = false;
    this.areaNegocioOpc = false;
    this.etapaOpc = false;
    this.actividadesOpc = false;
    this.rolesOpc = false;
    this.avanceMenu = false;
    this.permisosMenu = false;
    this.mostrarMenu = false;
  }

  abrirModalCerrarSesion() {
    const dialogRef = this.dialog.open(Modal, {
      width: '500px', height: 'auto'
    });
    this.dialog.afterAllClosed.subscribe(() => {
      if (localStorage.length == 0) {
        this.opened = false;
      }
      if (Globals.cerrarSesion) {
        Globals.cerrarSesion = false;
        Globals.permisos = [];
        Globals.pagina = 0;
        this.mostrarMenu = false;
        this.authenticationService.logout();
        this.banderasFalse();
        this.router.navigate(['/']);
      }

    });
  }

  catalogo(catalogo: string) {
    Globals.pagina = 2;
    Globals.url = catalogo;
    this.router.navigate(['/catalogos', catalogo], { replaceUrl: true });
  }
  reporte(reporte: string) {
    Globals.pagina = 2;
    Globals.url = reporte;
    this.router.navigate(['/reportes', reporte]);
  }
  volverLogin() {
    this.router.navigate(['/']);
  }
  bitacora(num: number) {

    if (this.currentUser) {
      Globals.pagina = num;
      this.router.navigate(['/bitacora', Globals.usuario])
    }

  }
  redirigeUr() {
    let ruta = this.redirige.getUrl(Globals.permisos[0]['nombrePantalla'])
    if (ruta == '/bitacora') {
      ruta += '/' + Globals.usuario;
      this.router.navigate([ruta])
    }
    if (ruta == '/reportes') {
      ruta += '/' + 'distribucion';
    }
    if (ruta == '/catalogos') {
      ruta += '/' + 'clientes';
    }
    else {
      this.router.navigate([ruta])
    }
  }
  avanceReal() {
    Globals.pagina = 2;
    this.router.navigate(['/avance-real'])
  }

  rolesFunc() {
    Globals.pagina = 2;
    this.router.navigate(['/administra-permisos'])
  }

  get usuario() {
    return Globals.usuario;
  }

  get rolUser() {
    return Globals.rolUser;
  }

  get imagen() {
    return Globals.imagen;
  }

  get imagenVista() {
    if (Globals.imagenVista == "null") {
      return false;
    }
    else {
      return true;
    }
  }

  get pagina() {
    return Globals.pagina
  }
  cambioPagina(ev: any) {

    Globals.pagina = ev.index;
    ////console.log('cambio a pag '+ev.index +" "+Globals.pagina);
    if (Globals.pagina == 0) {
      Globals.mostrarRouterOutlet = true;
      $(".mat-ink-bar").css({ display: 'block' });
      this.bitacora(0);
    }
    else if (Globals.pagina == 1) {
      Globals.mostrarRouterOutlet = false;
      $(".mat-ink-bar").css({ display: 'block' });
      this.bitacora(1);
    }
    else if (Globals.pagina == 2) {
      $(".mat-ink-bar").css({ display: 'none' });
      Globals.mostrarRouterOutlet = true;
    }
  }
  get movil() {
    return Globals.movil;
  }
  get mostrarRouter() {
    return Globals.mostrarRouterOutlet;
  }

  setTimeout() {
    if (this.currentUser) {
      this.userActivity = setTimeout(() => this.userInactive.next(undefined), 1000 * 60 * 30);
    } else {
      this.mostrarMenu = false;
    }
  }
  setMenu() {
    this.opened = false;
    this.menu1 = true;
    this.menu2 = false;
    this.menu3 = false;
  }
  volverMenu() {
    this.menu1 = true;
    this.menu2 = false;
    this.menu3 = false;
  }

  @HostListener('window:mousemove') refreshUserState() {
    if (this.currentUser) {
      clearTimeout(this.userActivity);
      this.setTimeout();
    }
  }
  cancelar() {
    this.dialog.closeAll();
  }

  cambiarContrasenia() {
    $(".dropdown-user-menu").css({ display: 'none' });
    this.router.navigate(['/cambio-contraseña'])
  }


}



@Component({
  selector: 'modal',
  templateUrl: './modal.html',
  standalone: false
})
export class Modal {

  constructor(
    public dialogRef: MatDialogRef<Modal>, private authenticationService: AuthenticationService, private router: Router
  ) { }

  onNoClick(): void {
    Globals.cerrarSesion = false;
    this.dialogRef.close();
  }
  logout() {
    $(".dropdown-user-menu").css({ display: 'none' });
    Globals.cerrarSesion = true;
    //this.authenticationService.logout();
    this.dialogRef.close();
    //this.router.navigate(['/']);
  }

}

