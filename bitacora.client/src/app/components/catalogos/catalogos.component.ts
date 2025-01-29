import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, HostListener, Inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { throwError } from 'rxjs';
import { catchError, timeout } from 'rxjs/operators';
import { AuthenticationService } from '../../services/authentication.service';
import { Globals } from '../../services/globals';
import { AgregarUsuarioComponent } from '../agregar-usuario/agregar-usuario.component';
import { environment } from '../../../environments/environment';

///import  $ from 'jquery';
declare var $: any;

export interface DialogData{
  unidadNeg:string,
  areas :string
}

@Component({
  selector: 'app-catalogos',
  templateUrl: './catalogos.component.html',
  styleUrls: ['./catalogos.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush // Activa OnPush
})
export class CatalogosComponent implements OnInit{


  loading:boolean = false;
  @HostListener('window:resize', ['$event'])
    onResize(event: Event) {
      this.movil = Globals.movil;
      if(this.movil==true){
        if(Globals.pagina==2){
          $(".mat-ink-bar").css({display:'none'});
        }

      }
    }

  ngOnInit(): void {
    $("#page-container").css("background-color","#EAEAEA");


    //Globals.usuario = '4281';
    //Globals.rolUser = '1';
    //localStorage.setItem('currentUser', '4281');
    //localStorage.setItem('userName', 'ulises.mireles');
    //localStorage.setItem('rol', '1');


    this.movil = Globals.movil;
    this.texto(this.paginas)
    this.pageChanged(1);
    Globals.pagina = 2;
    localStorage.removeItem("rolEditar");
    Globals.filtroActivo=false;
    this.agregarVaciosFiltro=false;
    if(Globals.pagina==2){
      $(".mat-ink-bar").css({display:'none'});
    }
    else{
      $(".mat-ink-bar").css({display:'block'});
    }

  }
  ngAfterViewInit()
  {

    if(!localStorage.getItem('currentUser')){
      this.authenticationService.verificarSesion();
    }

    this.cdRef.detectChanges();
  }
  ngAfterViewChecked()
  {
    if(this.tipoCatalogo=='Clientes'){
      if(this.filtroCuatro!=''){
        this.filtroActivo=true;
        this.coleccionFiltro = Globals.datosFiltrados;
        var pagFiltrados = Math.ceil(this.coleccionFiltro/this.config.itemsPerPage)
        this.pagFiltrados=pagFiltrados;
      }
    }
    else if(this.tipoCatalogo == 'Proyectos'){
      if(this.filtroCinco!=''){
        this.filtroActivo=true;
        this.coleccionFiltro = Globals.datosFiltrados;
        var pagFiltrados = Math.ceil(this.coleccionFiltro/this.config.itemsPerPage)
        this.pagFiltrados=pagFiltrados;
      }
    }
    else if(this.tipoCatalogo == 'Sistemas (Aplicativos)'){
      if(this.filtroTres!=''){
        this.filtroActivo=true;
        this.coleccionFiltro = Globals.datosFiltrados;
        var pagFiltrados = Math.ceil(this.coleccionFiltro/this.config.itemsPerPage)
        this.pagFiltrados=pagFiltrados;
      }
    }
    else if(this.tipoCatalogo == 'Usuarios'){
      if(this.filtroCuatro!=''){
        this.filtroActivo=true;
        this.coleccionFiltro = Globals.datosFiltrados;
        var pagFiltrados = Math.ceil(this.coleccionFiltro/this.config.itemsPerPage)
        this.pagFiltrados=pagFiltrados;
      }
    }
    else if(this.tipoCatalogo == 'Negocio (Unidades de Negocio)'){
      if(this.filtroDos!=''){
        this.filtroActivo=true;
        this.coleccionFiltro = Globals.datosFiltrados;
        var pagFiltrados = Math.ceil(this.coleccionFiltro/this.config.itemsPerPage)
        this.pagFiltrados=pagFiltrados;
      }
    }
    else if(this.tipoCatalogo == 'Áreas de Unidades de Negocio'){
      if(this.filtroDos!=''){
        this.filtroActivo=true;
        this.coleccionFiltro = Globals.datosFiltrados;
        var pagFiltrados = Math.ceil(this.coleccionFiltro/this.config.itemsPerPage)
        this.pagFiltrados=pagFiltrados;
      }
    }
    else if(this.tipoCatalogo == 'Etapas'){
      if(this.filtroDos!=''){
        this.filtroActivo=true;
        this.coleccionFiltro = Globals.datosFiltrados;
        var pagFiltrados = Math.ceil(this.coleccionFiltro/this.config.itemsPerPage)
        this.pagFiltrados=pagFiltrados;
      }
    }
    else if(this.tipoCatalogo == 'Actividades'){
      if(this.filtroTres!=''){
        this.filtroActivo=true;
        this.coleccionFiltro = Globals.datosFiltrados;
        var pagFiltrados = Math.ceil(this.coleccionFiltro/this.config.itemsPerPage)
        this.pagFiltrados=pagFiltrados;
      }
    }

    this.cdRef.detectChanges();
  }
  filtroActivo: boolean = false;
  tipoCatalogo: string = '';
  filtro1: string = '';
  filtro2: string = '';
  filtro3: string = '';
  filtro4: string = '';
  filtro5: string = '';
  filtro6: string = '';
  tablaClientes: boolean = false;
  tablaProyectos: boolean = false;
  tablaAplicativos: boolean = false;
  tablaUsuarios: boolean = false;
  tablaUnidadesNegocios: boolean = false;
  tablaAreasUnidades: boolean = false;
  tablaFases: boolean = false;
  tablaActividades: boolean = false;
  tablaRoles: boolean = false;
  objeto: any = null;
  arreglo: any[] = [];
  arregloProcesado: any[] = [];
  config: any = null;
  coleccion: number = 0;
  coleccionFiltro: number = 0;
  coleccionFiltrados: number = 0;
  paginas: number = 0;
  pagFiltrados: number = 0;
  pag: string = '';
  vacios: any[] = [];
  reg: any = null;
  rol: any = null;
  numResCon: any = null;
  columnasPorVacio: number = 0;
  agregarVacios: boolean = false;
  agregarVaciosFiltro: boolean = false;
  movil: boolean = false;
  registrosPorPagina: number = 10;
  titulo: string = '';
  mensaje: string = '';
  filtroUsuarios: boolean = false;
  filtroClientes: boolean = false;
  filtroUnidades: boolean = false;
  filtroUnidades1: boolean = false;
  filtroAreas: boolean = false;
  filtroAreas1: boolean = false;
  filtroActividades: boolean = false;
  filtroProyectos: boolean = false;
  btnOcultarCampos: boolean = false;
  btnActualizar: boolean = false;
  btnMostrarCampos: boolean = false;

  constructor(private authenticationService:AuthenticationService,private cdRef:ChangeDetectorRef,private spinner: NgxSpinnerService,private toastr:ToastrService,
    private activatedRoute:ActivatedRoute, private router:Router,private http:HttpClient,@Inject("BASE_URL") private baseUrl:string,public dialog:MatDialog ) {
    this.router.onSameUrlNavigation = 'reload';
    //asigno la base url desde el environment
    this.baseUrl = environment.baseURL;

    // Hardcodeando params para simular que se está en el catálogo de "clientes"
/*    this.activatedRoute.snapshot.params = { catalogo: 'usuarios' } as any;*/

    const params = this.activatedRoute.snapshot.params;
    this.config = {
      itemsPerPage: 10,
      currentPage:1,
      totalItems: this.coleccion
    }

    if (params['catalogo']) {
      this.tipoCatalogo = params['usuario'];

      if (params['catalogo'] === 'clientes') {
        this.tipoCatalogo = 'Clientes';
        this.filtro1 = 'Cliente';
        this.filtro2 = 'Alias';
        this.filtro3 = 'Giro';
        this.filtro4 = 'Estatus';
        this.getClientes();
        this.tablaClientes = true;
        this.filtroCuatro = "Activo";
        this.filtroClientes = true;
        this.btnOcultarCampos = true;
      }
      else if (params['catalogo'] === 'proyectos') {
        this.tipoCatalogo = 'Proyectos';
        this.filtro1 = 'Proyecto';
        this.filtro2 = 'Estatus';
        this.filtro3 = 'Resp OP';
        this.filtro4 = 'Líder Proyecto';
        this.filtroCinco = 'Activo';
        this.tablaProyectos = true;
        this.filtroProyectos = true;
        this.btnOcultarCampos = true;
        this.getProyectos();
      }
      else if (params['catalogo'] === 'aplicativos') {
        this.tipoCatalogo = 'Sistemas (Aplicativos)';
        this.filtro1 = 'Aplicativo';
        this.filtro2 = 'Cliente';
        this.filtro3 = 'Estatus';
        this.filtroTres = 'Activo';
        this.filtro4 = '';
        this.tablaAplicativos = true;
        this.filtroUnidades = true;
        this.btnActualizar = true;
        this.getSistemas();
      }
      else if (params['catalogo'] === 'usuarios') {
        this.tipoCatalogo = 'Usuarios';
        this.filtro1 = 'Usuario';
        this.filtro2 = 'Nombre';
        this.filtro3 = 'Rol';
        this.filtro4 = 'Estatus';
        this.filtroCuatro = 'Activo';
        this.filtro6 = 'Situacion';
        this.tablaUsuarios = true;
        this.filtroUsuarios = true;
        this.btnOcultarCampos = true;
        this.getUsuarios();
      }
      else if (params['catalogo'] === 'unidad-negocio') {
        this.tipoCatalogo = 'Negocio (Unidades de Negocio)';
        this.filtro1 = 'Unidad de Negocio';
        this.filtro2 = 'Estatus';
        this.filtro3 = 'Área Relacionada';
        this.filtro4 = '';
        this.filtroDos = 'Activo';
        this.tablaUnidadesNegocios = true;
        this.filtroUnidades1 = true;
        this.btnActualizar = true;
        this.getUnidadesNegocio();
      }
      else if (params['catalogo'] === 'areas-un') {
        this.tipoCatalogo = 'Áreas de Unidades de Negocio';
        this.filtro1 = 'Área';
        this.filtro2 = 'Estatus';
        this.filtro3 = '';
        this.filtro4 = '';
        this.filtroDos = 'Activo';
        this.tablaAreasUnidades = true;
        this.filtroAreas = true;
        this.btnActualizar = true;
        this.getAreasUnidadesNegocio();
      }
      else if (params['catalogo'] === 'fases') {
        this.tipoCatalogo = 'Etapas';
        this.filtro1 = 'Etapa';
        this.filtro2 = 'Estatus';
        this.filtro3 = '';
        this.filtro4 = '';
        this.filtroDos = 'Activo';
        this.tablaFases = true;
        this.filtroAreas1 = true;
        this.btnActualizar = true;
        this.getFases();
      }
      else if (params['catalogo'] === 'actividades') {
        this.tipoCatalogo = 'Actividades';
        this.filtro1 = 'Actividad';
        this.filtro2 = 'Tipo';
        this.filtro3 = 'Estatus';
        this.filtro4 = '';
        this.filtroTres = 'Activo';
        this.tablaActividades = true;
        this.filtroActividades = true;
        this.btnActualizar = true;
        this.getActividades();
      }
      else if (params['catalogo'] === 'roles') {
        this.tipoCatalogo = 'Roles';
        this.tablaRoles = true;
        this.btnActualizar = true;
        this.getRoles();
      }
    }
  }
  filtroUno='';
  filtroDos='';
  filtroTres='';
  filtroCuatro='';
  filtroCinco='';
  filtroSeis ='';
  filtroSiete='';
  filtroOcho='';
  addUser(){
    //console.log("click")
    this.router.navigate(['/agregarUsuario']);
  }

 
  getClientes() {
    this.spinner.show();
    this.coleccion=0;
    this.http.get(this.baseUrl + "api/ConsultaCatalogos/ConsultaClientes/{id?}").subscribe(result => {
      this.objeto = result;
      var areasUnidades=[];
      var nombre='';
      var giro='';
      var alias='';
      var bandera:boolean=false;

      //console.log(this.objeto)
      nombre=this.objeto.clientes[0].nombre;
      alias=this.objeto.clientes[0].alias;
      for (let index = 0; index < this.objeto.clientes.length; index++) {
        if(this.objeto.clientes[index].nombre==nombre && this.objeto.clientes[index].alias==alias){
          if(this.objeto.clientes[index].area!=null){
            var areasUn = {area:this.objeto.clientes[index].area, unidad:this.objeto.clientes[index].unidad};
            areasUnidades.push(areasUn);
          }

          alias=this.objeto.clientes[index].alias;
          giro=this.objeto.clientes[index].giro;
        }
        else if(this.objeto.clientes[index].nombre != nombre && this.objeto.clientes[index].alias!=alias){
          if(areasUnidades.length>0){
            bandera=true;
          }
          else{
            bandera=false;
          }
          var registro={alias:alias, giro:giro, nombre:nombre, areasUnidades:areasUnidades, bandera:bandera, estatus:this.objeto.clientes[index].estatus}

          this.arregloProcesado.push(registro);
          areasUnidades= [];

          nombre=this.objeto.clientes[index].nombre;
          alias=this.objeto.clientes[index].alias;
        }

      }
      //console.log(this.arregloProcesado)
      this.arreglo = this.arregloProcesado;
      this.coleccion = this.arregloProcesado.length;
      this.paginas = Math.ceil(this.coleccion/this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);

      // Notifica cambios manualmente
//this.cdRef.markForCheck();
      this.spinner.hide();
      this.filtroCuatro = 'Activo'
    }, error => {
      this.spinner.hide();
      console.log(error)
    });
    this.pageChanged(1);
  }

  getUsuarios() {
    this.spinner.show();
    this.coleccion=0;
      this.http.get(this.baseUrl + "api/Usuarios/ConsultaUsuarios/{id?}").pipe(
        timeout(4000),
        catchError(e => {
          // do something on a timeout
          console.log("Timeout");
          this.getUsuarios();
          return throwError(e);
        })
      ).subscribe(result => {
        this.objeto = result;
        this.arreglo = this.objeto.usuarios;
      for (let index = 0; index < this.arreglo.length; index++) {
        if(this.arreglo[index].registro == 1 || this.arreglo[index].registro == 2){
          this.arreglo[index].llenaBitacora ="Si";
        }
        else if(this.arreglo[index].registro==0){
          this.arreglo[index].llenaBitacora="No"
        }

      }
      this.coleccion = this.objeto.usuarios.length;
      this.paginas = Math.ceil(this.coleccion/this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);

        // Notifica cambios manualmente
        //this.cdRef.markForCheck();
      this.spinner.hide();
    }, error => {
      this.spinner.hide();
    });
    this.pageChanged(1);

  }
  getProyectos() {
    this.spinner.show();
    this.coleccion=0;
      this.http.get(this.baseUrl + "api/ConsultaCatalogos/ConsultaProyectos/{id?}").subscribe(result => {
      this.objeto = result;
      this.arreglo = this.objeto.proyectos;
   //console.log(this.arreglo)
      this.coleccion = this.objeto.proyectos.length;
      this.paginas = Math.ceil(this.coleccion/this.config.itemsPerPage)
      this.texto(this.paginas);
        this.pageChanged(1);
        // Notifica cambios manualmente
        this.cdRef.markForCheck();
      this.spinner.hide();
    }, error => {
      this.spinner.hide();
      //console.log(error)
    });
    this.pageChanged(1);
  }

  getActividades(){
    this.spinner.show();
    this.coleccion=0;
      this.http.get(this.baseUrl + "api/ConsultaCatalogos/ConsultaActividades/{id?}").subscribe(result => {
      this.objeto = result;
      this.arreglo = this.objeto.actividades;
      // console.log(this.arreglo)
      this.coleccion = this.objeto.actividades.length;
      this.paginas = Math.ceil(this.coleccion/this.config.itemsPerPage)
      this.texto(this.paginas);
        this.pageChanged(1);
        // Notifica cambios manualmente
        this.cdRef.markForCheck();
      this.spinner.hide();
    }, error => {
      this.spinner.hide();
      //console.log(error)
    });
    this.pageChanged(1);
  }

  direccionPermisos(rol?: any){
    if(rol == null){
      Globals.editarRoles = false;

    }
    else{
      Globals.editarRoles = true;
      localStorage.setItem("rolEditar", JSON.stringify(rol));
    }

  }

  getFases(){
    this.spinner.show();
    this.coleccion=0;
    this.http.get(this.baseUrl + "api/ConsultaCatalogos/ConsultaEtapas/{id?}").subscribe(result => {
    this.objeto = result;
    this.arreglo = this.objeto.etapas;
    //console.log(this.arreglo)
    this.coleccion = this.objeto.etapas.length;
    this.paginas = Math.ceil(this.coleccion/this.config.itemsPerPage)
    this.texto(this.paginas);
      this.pageChanged(1);

      // Notifica cambios manualmente
   this.cdRef.markForCheck();
    this.spinner.hide();
    }, error => {

      this.spinner.hide();
      //console.log(error)
    });
    this.pageChanged(1);
  }

  getUnidadesNegocio() {
    this.spinner.show();
    this.http.get(this.baseUrl + "api/ConsultaCatalogos/ConsultaUnidadesNegocio/{id?}").subscribe(result => {
      this.objeto = result;
      //console.log(this.objeto)
      var areas = [];
      var unidad ='';
      var estatus = '';

      unidad= this.objeto.unidadesNegocio[0].nombre;
      estatus = this.objeto.unidadesNegocio[0].estatus;
      for (let index = 0; index < this.objeto.unidadesNegocio.length; index++) {
        if(this.objeto.unidadesNegocio[index].nombre==unidad){
          areas.push(" "+this.objeto.unidadesNegocio[index].area);

        }
        else if(this.objeto.unidadesNegocio[index].nombre != unidad){
          var registro = {unidad:unidad, estatus:estatus ,areas:areas};
          this.arregloProcesado.push(registro);
          estatus = this.objeto.unidadesNegocio[index].estatus;
          unidad = this.objeto.unidadesNegocio[index].nombre;
          areas = [];
          areas.push(this.objeto.unidadesNegocio[index].area);
          if(index == this.objeto.unidadesNegocio.length-1){
          registro = {unidad:this.objeto.unidadesNegocio[index].nombre, estatus:this.objeto.unidadesNegocio[index].estatus ,areas:areas};
          this.arregloProcesado.push(registro);
          }
        }

      }

      //this.arreglo = this.objeto.unidadesNegocio;
      this.arreglo = this.arregloProcesado;
      //this.coleccion = this.objeto.unidadesNegocio.length;

      this.coleccion=this.arregloProcesado.length;
      this.paginas = Math.ceil(this.coleccion/this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);
      // Notifica cambios manualmente
     this.cdRef.markForCheck();  //markForCheck
      this.spinner.hide();
    }, error => {
      this.spinner.hide();
      //console.log(error)
    });
  }

  getAreasUnidadesNegocio() {
    this.spinner.show();
    this.http.get(this.baseUrl + "api/ConsultaCatalogos/ConsultaAreasNegocio/{id?}").subscribe(result => {
      this.objeto = result;

      this.arreglo = this.objeto.areasNegocio;
      this.coleccion = this.objeto.areasNegocio.length;
      this.paginas = Math.ceil(this.coleccion/this.config.itemsPerPage)

      this.texto(this.paginas);
      this.pageChanged(1);
      // Notifica cambios manualmente
      this.cdRef.markForCheck();
      this.spinner.hide();
    }, error => {
      this.spinner.hide();
      //console.log(error)
    });
  }

  getSistemas() {
    this.spinner.show();
    this.http.get(this.baseUrl + "api/ConsultaCatalogos/ConsultaSistemas/{id?}").subscribe(result => {
      this.objeto = result;

      this.arreglo = this.objeto.sistemas;
      this.coleccion = this.objeto.sistemas.length;
      this.paginas = Math.ceil(this.coleccion/this.config.itemsPerPage)

      this.texto(this.paginas);
      this.pageChanged(1);
      // Notifica cambios manualmente
      this.cdRef.markForCheck();
      this.spinner.hide();
    }, error => {
      this.spinner.hide();
      //console.log(error)
    });
  }

  getRoles(){
    this.spinner.show();
    this.coleccion=0;
      this.http.get(this.baseUrl + "api/Roles/ConsultaRoles/{id?}").subscribe(result => {
      this.objeto = result;
      this.arreglo = this.objeto.roles;
      for (let index = 0; index < this.arreglo.length; index++) {
        var fechaReg= new Date(this.arreglo[index].fechaRegistro);
        var fechaMod = new Date(this.arreglo[index].fechaModificacion);

        var mesReg=parseInt(moment(fechaReg).format('M'));
        var diaReg=parseInt(moment(fechaReg).format('D'));
        var añoReg=(moment(fechaReg).year());
        var fechaFormat = diaReg+"/"+(mesReg)+"/"+añoReg;

        var mesMod = parseInt(moment(fechaMod).format('M'));
        var diaMod=parseInt(moment(fechaMod).format('D'));
        var añoMod=(moment(fechaMod).year());
        var fechaFormatMod = diaMod+"/"+(mesMod)+"/"+añoMod;

        this.arreglo[index].fechaRegistro = fechaFormat;
        this.arreglo[index].fechaModificacion = fechaFormatMod;

        this.arreglo[index].listPantallas=this.arreglo[index].listPantallas.join(', ');

      }


      //console.log(result)
      this.coleccion = this.objeto.roles.length;
      //console.log(this.config.itemsPerPage)
      this.paginas = Math.ceil(this.coleccion/this.config.itemsPerPage)
      //console.log(this.paginas)
      this.texto(this.paginas);
        this.pageChanged(1);
        // Notifica cambios manualmente
      this.cdRef.markForCheck();
      this.spinner.hide();
    }, error => {
      this.spinner.hide();
      //console.log(error)
    });

    this.pageChanged(1);
  }
  pageChanged(event: number) {

    this.agregarVacios = false;
    this.agregarVaciosFiltro = false;
    this.config.currentPage = event;
    this.vacios = [];
    var regs = this.coleccion % this.config.itemsPerPage;
    this.coleccionFiltrados = Globals.datosFiltrados;
    this.coleccionFiltro = this.coleccionFiltrados
    var pagFiltrados = Math.ceil(this.coleccionFiltrados / this.config.itemsPerPage)
    this.pagFiltrados = pagFiltrados;
    var regsFiltFinal = Globals.datosFiltrados % this.config.itemsPerPage;
    if (event == this.paginas && regs == 0 && Globals.filtroActivo == false) {

      this.agregarVacios = false;
      this.agregarVaciosFiltro = false;
    }
    else if (Globals.filtroActivo == true && event == this.paginas && regsFiltFinal < this.config.itemsPerPage) {

      for (let index = 0; index < 10 - regsFiltFinal; index++) {
        this.vacios.push('-');
      }
      this.agregarVaciosFiltro = true;
      this.agregarVacios = false;
    }
    else if (Globals.filtroActivo == true && this.coleccionFiltrados < this.config.itemsPerPage || event == pagFiltrados && Globals.filtroActivo == true) {

      for (let index = 0; index < 10 - regsFiltFinal; index++) {
        this.vacios.push('-');
      }
      this.texto(pagFiltrados)
      this.agregarVaciosFiltro = true;
      this.agregarVacios = false;
    }

    else if (event == pagFiltrados && Globals.filtroActivo == false || this.coleccion < this.config.itemsPerPage && Globals.filtroActivo == false) {


      for (let index = 0; index < 10 - regs; index++) {
        this.vacios.push('-');
      }
      this.agregarVacios = true;
    }
    else if (Globals.filtroActivo == false) {
      if (event == this.paginas && regs < 10) {
        for (let index = 0; index < 10 - regs; index++) {
          this.vacios.push('-');
      
        }
      }
      this.agregarVacios = true;
    }
    else {

      this.agregarVacios = false;
    }
   }




  verificarBancerasUN(){
    if(Globals.filtroUN==true || Globals.filtroEstatusUN==true || Globals.filtroAreasUN==true){
      Globals.filtroActivo=true;
      this.filtroActivo=true;
      this.agregarVaciosFiltro=true;
      this.pageChanged(1);
    }
    else if(Globals.filtroUN==false && Globals.filtroEstatusUN==false && Globals.filtroAreasUN==false)
    {
      Globals.filtroActivo=false;
      this.filtroActivo=false;
      this.agregarVaciosFiltro=false;
      this.pageChanged(1);
    }
  }
  verificarFiltroUno(event: string){
    if(event==''){
     Globals.filtroUN = false;
     this.verificarBancerasUN();
    }
    else{
      Globals.filtroUN = true;
      this.verificarBancerasUN();
    }
  }
  verificarFiltroDos(event: string){
    if(event==''){
      Globals.filtroEstatusUN=false;
      this.verificarBancerasUN();
    }
    else{
      Globals.filtroEstatusUN=true;
      this.verificarBancerasUN();
    }
  }

  verificarFiltroTres(event: string){
    if(event==''){
      Globals.filtroEstatusUN = false;
      this.verificarBancerasUN();
    }
    else{
      Globals.filtroEstatusUN=true;
      this.verificarBancerasUN();
    }
  }

  verificarFiltroCuatro(event: string){
    if(event=''){
      Globals.filtroEstatusUN=false;
      this.verificarBancerasUN();

    }
    else{
      Globals.filtroEstatusUN = true;
      this.verificarBancerasUN();
    }
  }
  verificarFiltroCinco(event: string){
    if(event=''){
      Globals.filtroEstatusUN=false;
      this.verificarBancerasUN();
    }
    else{
      Globals.filtroEstatusUN = true;
      this.verificarBancerasUN();
    }
  }

  verificarFiltroSeis(event: string){

    if(event=''){
      Globals.filtroEstatusUN=false;
      this.verificarBancerasUN();
    }
    else{
      Globals.filtroEstatusUN = true;
      this.verificarBancerasUN();
    }
  }
  verificarFiltroSiete(event: string){
    console.log(event);
    if(event=''){
      Globals.filtroEstatusUN=false;
      this.verificarBancerasUN();
    }
    else{
      Globals.filtroEstatusUN = true;
      this.verificarBancerasUN();
    }
  }
  verificarFiltroOcho(event: string){

    if(event=''){
      Globals.filtroEstatusUN=false;
      this.verificarBancerasUN();
    }
    else{
      Globals.filtroEstatusUN = true;
      this.verificarBancerasUN();
    }
  }
  texto(event: number){

    if(event==1){
      this.pag='Página'
    }
    else {
      this.pag='Páginas'
    }
  }


  abrirFormUsuario(usuario: any){
    if(usuario == null){
      Globals.editarUsuario = false;
        this.limpiarUsuarioEdit();
        this.limpiarUsuarioView();
    }
    else{
      Globals.editarUsuario = true;
      localStorage.setItem("usuarioEditar", JSON.stringify(usuario));
      this.limpiarUsuarioView();
    }
    const dialogRef = this.dialog.open(AgregarUsuarioComponent,{
      width: '80vh',disableClose: true
    })
    this.dialog.afterAllClosed.subscribe(()=>{
      this.getUsuarios();
    })
  }

  abrirViewUsuario(usuario: any){
    console.log(usuario)
    if(usuario == null){
      Globals.editarUsuario = false;
        this.limpiarUsuarioEdit();
    }
    else{
      Globals.editarUsuario = true;
      localStorage.setItem("usuarioView", JSON.stringify(usuario));
      localStorage.setItem("usuarioEditar", JSON.stringify(usuario));
    }
    const dialogRef = this.dialog.open(AgregarUsuarioComponent,{
      width: '80vh',disableClose: true
    });
    $("#btnGuardar").css({display:'none'});
    $("#txtPass").css({display:'none'});
    $("#txtPassword").css({display:'none'});
    this.dialog.afterAllClosed.subscribe(()=>{
      this.getUsuarios();
    })
  }
  eliminarRol(rol: { idRol: any }) {
    this.spinner.show();
    //console.log(rol)
    var idRol=rol.idRol;
    this.http.delete<any>(this.baseUrl + `api/Roles/EliminaRol/${idRol}`).subscribe(
      res=>{
        if(res.rol==-2){
          this.titulo = "Error";
          this.mensaje = "El rol no ha podido ser ya que tiene uno o más usuarios asignados"
          this.toastr.error(this.mensaje, this.titulo);
          this.spinner.hide();
        }
        else if(res.rol == -1){
          this.titulo = "Error";
          this.mensaje = "Ha ocurrido un error al intentar eliminar el rol, intente de nuevo."
          this.toastr.error(this.mensaje, this.titulo);
          this.spinner.hide();
        }
        else{
          this.titulo = "Rol Eliminado";
          this.mensaje = "El rol ha sido eliminado correctamente"
          this.toastr.success(this.mensaje, this.titulo);
          this.getRoles();
          this.spinner.hide();

        }
        //console.log(res);

      },
      err=>{
        this.titulo = "Error";
        this.mensaje = "Ha ocurrido un error al intentar eliminar el rol, intente de nuevo."
        this.toastr.error(this.mensaje, this.titulo);
        //console.log(err);
        this.spinner.hide();
      }
    )
  }

  cambiarEstatus(numero: number){
    this.spinner.show();
    var datos = {idUser:numero};
    this.http.get<any>(this.baseUrl + "api/Usuarios/BajaUsuario/{id?}",{params:datos}).subscribe(res=>{
      //console.log(res);
      this.titulo = "Estatus Cambiado"
      this.mensaje = "El estatus del usuario ha sido cambiado correctamente";
      this.toastr.success(this.mensaje, this.titulo);
      this.getUsuarios();
      this.spinner.hide();
    }, err =>{
      this.spinner.hide();
      //console.log(err);
      this.titulo = "Error"
      this.mensaje = "Ha ocurrido un error al intentar cambiar el estatus del usuario";
      this.toastr.error(this.mensaje, this.titulo);
    })

  }

  limpiarUsuarioEdit(){
    localStorage.removeItem("usuarioEditar");
  }
  limpiarUsuarioView(){
    localStorage.removeItem("usuarioView");
  }
  reestablecerPass(id: number) {
    var datos = {idUser:id};
    this.http.get<any>(this.baseUrl + "api/Usuarios/RestablecePassword/{id?}",{params:datos}).subscribe(res=>{
     //console.log(res);
    if(res>1){
      //console.log("se envio un correo con la contraseña")
      this.titulo = "Correo Enviado";
      this.mensaje = "Se ha enviado un correo con los datos para reestablecer la contraseña al usuario.";
      this.toastr.success(this.mensaje, this.titulo);
    }
    else if(res == -1){
      this.titulo = "Error";
      this.mensaje = "Ha ocurrido un error al intentar reestablecer la contraseña, intente de nuevo."
      this.toastr.error(this.mensaje, this.titulo);
    }
  }, err =>{
     //console.log(err);
  })

  }




  cambioRPP(event: any) {
    this.config.itemsPerPage = event;
    this.config.currentPage = 1;
    this.verificarBancerasUN();
    if (this.tipoCatalogo == 'Clientes') {

      this.paginas = Math.ceil(this.coleccion / this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);
    }
    if (this.tipoCatalogo == 'Proyectos') {

      this.paginas = Math.ceil(this.coleccion / this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);
    }
    if (this.tipoCatalogo == 'Sistemas (Aplicativos)') {

      this.paginas = Math.ceil(this.coleccion / this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);
    }
    if (this.tipoCatalogo == 'Usuarios') {

      this.paginas = Math.ceil(this.coleccion / this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);
    }
    if (this.tipoCatalogo == 'Negocio (Unidades de Negocio)') {

      this.paginas = Math.ceil(this.coleccion / this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);
    }
    if (this.tipoCatalogo == 'Áreas de Unidades de Negocio') {

      this.paginas = Math.ceil(this.coleccion / this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);
    }
    if (this.tipoCatalogo == 'Etapas') {

      this.paginas = Math.ceil(this.coleccion / this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);
    }
    if (this.tipoCatalogo == 'Actividades') {

      this.paginas = Math.ceil(this.coleccion / this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);
    }
    if (this.tipoCatalogo == 'Roles') {

      this.paginas = Math.ceil(this.coleccion / this.config.itemsPerPage)
      this.texto(this.paginas);
      this.pageChanged(1);
    }
  }



  abrirModalCambioEstatus(numero: number) {
    //console.log(numero)
    this.reg = numero;
    const dialogRef = this.dialog.open(ModalCambioEstatus,{
      width: '500px', height: 'auto'
    });
    this.dialog.afterAllClosed.subscribe(()=>{
      if(Globals.cambioEstatus){
        this.cambiarEstatus(this.reg.idUser);
        Globals.cambioEstatus = false;
      }
    });
  }

  abrirModalReestablecerContrasenia(numero: number){
    this.numResCon = numero;
    const dialogRef = this.dialog.open(ModalReestablecerContrasenia,{
      width: '500px', height: 'auto'
    });
    this.dialog.afterAllClosed.subscribe(()=>{
      if(Globals.reestableceContrasenia){
        this.reestablecerPass(this.numResCon);
        Globals.reestableceContrasenia = false;
      }
    });
  }
  abrirModalEliminarRol(rol: any){
    this.rol = rol;
    const dialogRef = this.dialog.open(ModalEliminarRol,{
      width: '500px', height: 'auto'
    });
    this.dialog.afterAllClosed.subscribe(()=>{
      if(Globals.eliminarRol){
        this.eliminarRol(this.rol)
        Globals.eliminarRol = false;
      }
    });

  }
  ocultarFiltroCampos(){
    if(this.tablaUsuarios){
    this.filtroUsuarios = false
    this.btnOcultarCampos = false;
    this.btnMostrarCampos = true;
    }else if(this.tablaClientes){
      this.filtroClientes = false
      this.btnOcultarCampos = false;
      this.btnMostrarCampos = true;
      }else  if(this.tablaProyectos){
        this.filtroProyectos = false
        this.btnOcultarCampos = false;
        this.btnMostrarCampos = true;
        }
  }
  mostrarFiltroCampos(){
    if(this.tablaUsuarios){
    this.filtroUsuarios = true
    this.btnOcultarCampos = true;
    this.btnMostrarCampos = false;
    }else  if(this.tablaClientes){
      this.filtroClientes = true
      this.btnOcultarCampos = true;
      this.btnMostrarCampos = false;
      }else  if(this.tablaProyectos){
        this.filtroProyectos = true
        this.btnOcultarCampos = true;
        this.btnMostrarCampos = false;
        }

  }
  limpiarCampos(){

      this.filtroUno='';
    this.filtroDos='';
    this.filtroTres='';
    this.filtroCuatro='';
    this.filtroCinco='';
    this.filtroSeis ='';
    this.filtroSiete ='';
    this.filtroOcho ='';
    if(this.tipoCatalogo == 'Clientes'){
      this.filtro1 = 'Cliente';
      this.filtro2 = 'Alias';
      this.filtro3 = 'Giro';
      this.filtro4 = 'Estatus';
      this.getClientes();
      this.tablaClientes = true;
      this.filtroCuatro="Activo"
    }
    else if(this.tipoCatalogo == 'Proyectos'){
      this.filtro1 = 'Proyecto';
      this.filtro2 = 'Estatus';

      this.filtro3 = 'Resp OP';
      this.filtro4 = 'Líder Proyecto';
      this.filtroCinco = 'Activo'
      this.getProyectos();
    }
    else if(this.tipoCatalogo == 'Sistemas (Aplicativos)'){
        this.filtro1 = 'Aplicativo';
        this.filtro2 = 'Cliente';
        this.filtro3 = 'Estatus';
        this.filtroTres = 'Activo'
        this.filtro4 = '';
        this.getSistemas();
    }
    else if(this.tipoCatalogo == 'Usuarios'){
      this.filtro1 = 'Usuario';
      this.filtro2 = 'Nombre';
      this.filtro3 = 'Rol';
      this.filtro4 = 'Estatus';
      this.filtroCuatro = 'Activo';
      this.filtro6 = 'Situacion';
      this.getUsuarios();
    }
    else if(this.tipoCatalogo == 'Negocio (Unidades de Negocio)'){
      this.filtro1 = 'Unidad de Negocio';
      this.filtro2 = 'Estatus';
      this.filtro3 = 'Área Relacionada';
      this.filtro4 = '';
      this.filtroDos = 'Activo'
      this.getUnidadesNegocio();
    }
    else if(this.tipoCatalogo == 'Áreas de Unidades de Negocio'){

        this.filtro1 = 'Área';
        this.filtro2 = 'Estatus';
        this.filtro3 = '';
        this.filtro4 = '';
        this.filtroDos = 'Activo'
        this.getAreasUnidadesNegocio();

    }
    else if(this.tipoCatalogo == 'Etapas'){
      this.tipoCatalogo = 'Etapas';
      this.filtro1 = 'Etapa';
      this.filtro2 = 'Estatus';
      this.filtro3 = '';
      this.filtro4 = '';
      this.filtroDos = 'Activo'
      this.getFases();
    }
    else if(  this.tipoCatalogo == 'Actividades'){
      this.filtro1 = 'Actividad';
      this.filtro2 = 'Tipo';
      this.filtro3 = 'Estatus';
      this.filtro4 = '';
      this.filtroTres = 'Activo'
      this.getActividades();
    }
    else if(this.tipoCatalogo == 'Roles'){

        this.getRoles();
    }
  }


}

@Component({
  selector: 'confirmacionCambioEstatus',
  templateUrl: 'confirmacionCambioEstatus.html'
})
export class ModalCambioEstatus{
  constructor(
    public dialogRef:MatDialogRef<ModalCambioEstatus>,private router:Router
  ){}

  onNoClick():void{
    Globals.cambioEstatus = false;
    this.dialogRef.close();
  }
  cambiarEstatus(){
    Globals.cambioEstatus = true;
    this.dialogRef.close();
  }

}
@Component({
  selector: 'confirmacionReestablecerContrasenia',
  templateUrl: 'confirmacionReestablecerContrasenia.html'
})
export class ModalReestablecerContrasenia{

  constructor(
    public dialogRef:MatDialogRef<ModalReestablecerContrasenia>,private router:Router
  ){}

  onNoClick():void{
    Globals.reestableceContrasenia = false;
    this.dialogRef.close();
  }
  reestableceCont(){
    Globals.reestableceContrasenia = true;
    this.dialogRef.close();
  }

}

@Component({
  selector: 'confirmacionEliminarRol',
  templateUrl: 'confirmacionEliminarRol.html'
})
export class ModalEliminarRol{

  constructor(
    public dialogRef:MatDialogRef<ModalEliminarRol>,private router:Router
  ){}

  onNoClick():void{
    Globals.eliminarRol = false;
    this.dialogRef.close();
  }
  eliminarRol(){
    Globals.eliminarRol = true;
    this.dialogRef.close();
  }
  

}
