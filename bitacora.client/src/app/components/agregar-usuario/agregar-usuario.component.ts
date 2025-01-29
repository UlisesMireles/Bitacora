import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, Inject, OnDestroy, OnInit, QueryList, ViewChildren} from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatExpansionPanel } from '@angular/material/expansion';
import { Router } from '@angular/router';
/*import $ from 'jquery';*/
import { ToastrService } from 'ngx-toastr';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as _ from 'underscore';
import { RelacionUsuarioUnidadArea } from '../../models/relacionUsuarioUnidadArea';
import { CatUsuarios } from '../../models/usuarioBitacora';
import { AuthenticationService } from '../../services/authentication.service';
import { Globals } from '../../services/globals';
import { environment } from '../../../environments/environment';
declare var $: any;

@Component({
  selector: 'app-agregar-usuario',
  templateUrl: './agregar-usuario.component.html',
  styleUrls: ['./agregar-usuario.component.css'],
  standalone: false
})
export class AgregarUsuarioComponent implements OnInit, OnDestroy {
  ngOnDestroy(){
    this._onDestroy.next();
    this._onDestroy.complete();
  }

  formUsuario: FormGroup = new FormGroup({});
  practicante: any = null;
  estatus: any = null;
  empleado: any = null;
  empleadoSeleccionado: any = null;
  selEmpleado: any = null;
  archivar: any = null;
  sugerenciaNombre: any = null;
  valor: string = "0";
  labelPosition: string = 'before';
  esEmpleado: boolean = true;
  tieneUnidadArea: any = null;
  empleados: any[] = [];
  tiposUsuario: any[] = [];
  nombreUsuario: string = 'Usuario';
  titulo: string = '';
  mensaje: string = '';
  nuevaCont: string = '';
  confirmaCont: string = '';
  unidades: any[] = [];
  areas: any[] = [];
  listaEditar: any[] = [];
  unidadAreasSeleccionadas: any[] = [];
  unidadAreasGuardadas: any[] = [];
  indicesEliminar: any[] = [];
  unidadAreasTemp: any[] = [];
  editaEmpleadoConArea: boolean = false;
  modificaFlag: boolean = false;
  insertaFlag: boolean = false;
  panelOpenState: boolean = false;
  editar: boolean = false;
  view: boolean = false;
  checkRegBitacora: boolean = false;
  checkBloqRegBitacora: boolean = false;
  _onDestroy: Subject<void> = new Subject<void>();
  filtroEmpleados: FormControl = new FormControl();
  empleadoCtrl: FormControl = new FormControl();
  empleadosFiltrados: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);




  constructor(private authenticationService:AuthenticationService,private toastr:ToastrService,private http:HttpClient,private fb:FormBuilder,private cdRef:ChangeDetectorRef,private router:Router,@Inject("BASE_URL") private baseUrl:string, private dialog:MatDialog) {

    //asigno la base url desde el environment
    this.baseUrl = environment.baseURL;

    this.formUsuario = this.fb.group({
      nombre:['',null],
      usuario:['',null],
      tipoUsuario: ['0', null], 
      correo:['',null],
      empleado: [this.editar ? this.formUsuario.get('empleado')?.value : 'S', null],  // Establece 'S' solo si no estás en modo de edición
      selEmpleado:['',null],
      rol:['',null],
      hideRequired: false,
      passChange:['',null],
      passConfirm:['',null],
      floatLabel: 'never'
    })
  }
  ngAfterViewChecked()
  {
    if(!localStorage.getItem('currentUser')){
      this.authenticationService.verificarSesion();
    }
    this.cdRef.detectChanges();
  }
  @ViewChildren("panelExpandible") Panel: QueryList<MatExpansionPanel> = new QueryList<MatExpansionPanel>();
  ngOnInit() {


    Globals.pagina = 2;
    (<any>$('#optgroup')).multiselect();
    this.getEmpleados();

    this.getTiposUsuario();


    var promise = new Promise<void>((res,rej)=>{
      this.getAreas();
      res();
    }).then(
    ()=>{
      if(localStorage.getItem('usuarioEditar')){
        var usuarioEditar = localStorage.getItem('usuarioEditar');
        if (usuarioEditar !== null) {
          var usuario = JSON.parse(usuarioEditar);
          this.relacionUnidadArea(usuario.idUser, this.unidades);
          this.checkRegBitacora = usuario.registro;
          if(usuario.registro==2){
            if(this.checkRegBitacora == true){
              this.checkBloqRegBitacora = true;
            }
          }
        }
        else{

          this.checkBloqRegBitacora = false;
        }
        //console.log(usuario)
      }
      setTimeout(()=>{
      this.getUnidades();
      },500)

    });
    this.filtroEmpleados.valueChanges.pipe(takeUntil(this._onDestroy))
    .subscribe(()=>{
      this.filtrarEmpleados();
    })
    this.formUsuario.get('passConfirm')?.disable();
    if(localStorage.getItem('usuarioView')){
      this.view = true;
      this.editar = true;
      this.checkBloqRegBitacora = true;
      this.formUsuario.get('passConfirm')?.disable();
      this.formUsuario.get('passChange')?.disable();
      this.formUsuario.get('tipoUsuario')?.disable();
    }else{
      this.view = false;
    }
  }
  relacionUnidadArea(idUsuario : any,  unidades: any[]) {
    var datos = {idUser:idUsuario};
    this.http.get<any>(this.baseUrl + "api/Usuarios/ConsultaRel_UserUnArea/{id?}",{params:datos}).subscribe(
      res=>{
        this.unidadAreasGuardadas = res.listaRelacion;
        //console.log(this.unidadAreasGuardadas)
        for (let i = 0; i < this.unidadAreasGuardadas.length; i++) {
          var reg;
          reg = {unidad:this.unidadAreasGuardadas[i].idUnidad, area:this.unidadAreasGuardadas[i].idArea, value:true};
          this.unidadAreasSeleccionadas.push(reg);

        }
        //console.log(this.unidadAreasSeleccionadas)
      },
      err=>{

      }
    )
  }
  checkRegistroBitacora(event: any) {
    this.checkRegBitacora = event.checked;
    //console.log(this.checkRegBitacora);
  }

  clickStopPropagation(event: any){
    //console.log(event)
    if(event.checked == true){
      event.stopPropagation()
    }

  }
  checkUnidades(unidad: number, value: any, areas: any[], indice: number): void {
    // console.log(this.Panel.toArray()[indice])
    // console.log(this.Panel.toArray())
    var index = this.unidades.indexOf(this.unidades.find(x => x.id == unidad));
    this.unidades[index].expanded = false;
    this.unidades[index].disabled = !this.unidades[index].disabled;
    if(this.unidades[index].disabled==true){

    }
    else{
      this.unidades[index].expanded = true;
    }
    //console.log(this.unidadAreasSeleccionadas)
    if(value.checked == false){

      this.Panel.toArray()[indice].close();
      this.unidades[index].expanded = false;
      for (let index = this.unidadAreasSeleccionadas.length-1; index >=0; index--) {
        //console.log(index)
        if(this.unidadAreasSeleccionadas[index].unidad==unidad){
          this.unidadAreasSeleccionadas.splice(index,1);
        }
      }
    }
    else if(value.checked==true){
      //this.Panel.open();
      //console.log(areas)
      for (let i = 0; i < areas.length; i++) {
        if(areas[i].checked==true){
          var reg = {unidad:unidad, area:areas[i].id, value:value.checked}
          this.unidadAreasSeleccionadas.push(reg);
        }

      }
      this.Panel.toArray()[indice].disabled = false;
      this.Panel.toArray()[indice].open();
    }

    //console.log(this.unidadAreasSeleccionadas,areas)
    }
    checkAreas(unidad: number,area: number, value:any){
      //console.log(unidad , area, value.checked)
      var reg = {unidad:unidad, area:area, value:value.checked}
      if(value.checked == false){
        for (let index = 0; index < this.unidadAreasSeleccionadas.length; index++) {
          if(this.unidadAreasSeleccionadas[index].unidad==unidad && this.unidadAreasSeleccionadas[index].area==area){
            this.unidadAreasSeleccionadas.splice(index,1);
          }
        }
        for (let i = 0; i < this.unidades.length; i++) {
          if(this.unidades[i].id==unidad){
            var index = this.unidades[i].areas.indexOf(this.unidades[i].areas.find( (x: any)=>x.id == area));
            this.unidades[i].areas[index].checked = false;
          }

        }
      }else{
        for (let i = 0; i < this.unidades.length; i++) {
          if(this.unidades[i].id==unidad){
            var index = this.unidades[i].areas.indexOf(this.unidades[i].areas.find( (x: any) =>x.id == area));
            this.unidades[i].areas[index].checked = true;
          }

        }
        this.unidadAreasSeleccionadas.push(reg)
      }


      //console.log(this.unidadAreasSeleccionadas, this.unidades)
    }

  radioEmpleado(value:any){
    var nomb = this.formUsuario.get('nombre')?.value;
    var us = this.formUsuario.get('usuario')?.value
    //console.log(nomb)
    var empleado =value.value;
    //console.log(empleado)
    if(empleado=="S"){
      this.esEmpleado=true;
      this.nombreUsuario = "Usuario";
      if(this.editar){
        this.formUsuario.get('nombre')?.disable();
        this.empleadoCtrl.disable();
        this.filtroEmpleados.disable();
      }

    }
    else if(empleado=="N"){
      // this.formUsuario.get('nombre').enable();
      // this.formUsuario.get('usuario').disable();
      this.esEmpleado=false;
      if(!this.editar){

        this.formUsuario.controls['nombre'].setValue('');
        this.formUsuario.controls['selEmpleado'].setValue('');
      }else{

        this.formUsuario.controls['nombre'].disable();
        this.formUsuario.get('usuario')?.disable();
      }


      this.nombreUsuario = "Nombre del Usuario";
    }
  }
  cambioCont(cont:string){
    if(cont==''|| cont==undefined || cont==null){
      this.formUsuario.get('passConfirm')?.disable();
      //console.log(cont)
    }
    else{
      if(cont.length<4 || cont.length>10){
        this.formUsuario.get('passConfirm')?.disable();
      }
      else{
        this.formUsuario.get('passConfirm')?.enable();
      }

      //console.log(cont)
    }
  }

  getEmpleados() {
    this.http.get<any>(this.baseUrl + "api/Usuarios/ConsultaEmpleados/{id?}").subscribe(result => {
    this.empleados = result.empleados;
    this.empleadosFiltrados.next(this.empleados.slice());
      //console.log(this.empleados)
    var usuario = localStorage.getItem('usuarioEditar');
    if(localStorage.getItem('usuarioEditar')){
      if (usuario !== null) {
      var user = JSON.parse(usuario);
      if(user.idRelacion > 0){
        var index = this.empleados.indexOf(this.empleados.find(x => x.nombre == user.nombre));
        // console.log("indice de empleados")
        // console.log(index)
        var empleado = this.empleados[index];
        //console.log(empleado)
        if(empleado!=null){
          this.empleadoSeleccionado = empleado;
        }
      }
    }
    }
    }, error => {
      //console.log(error)
    });
  }
  getTiposUsuario(){
    return this.http.get<any>(this.baseUrl + "api/Usuarios/ConsultaRoles/{id?}").subscribe(result => {
      this.tiposUsuario = result.tipoUsRolesuario;
      // console.log(result)
      var usuario = localStorage.getItem('usuarioEditar');
      if(localStorage.getItem('usuarioEditar')){
        this.editar = true;
        var usuarioData = JSON.parse(localStorage.getItem('usuarioEditar') ?? '');
        if(usuarioData.rol.includes('035')){
          this.tiposUsuario = result.tipoUsRolesuario;
        }else{
          this.tiposUsuario = this.tiposUsuario.filter(x => !x.nombre.includes('035'));
        }
        if (usuario !== null) {

            //this.formUsuario.
        var user = JSON.parse(usuario);
        //console.log(user)
        if(user.idRelacion == 0){
        var idTipo=0;


        this.formUsuario.controls['empleado'].setValue('N');
        var value={value:this.formUsuario.controls['empleado'].value};
        this.radioEmpleado(value);
        this.formUsuario.controls['nombre'].setValue(user.nombre);
        this.formUsuario.controls['usuario'].setValue(user.usuario);
        this.formUsuario.controls['correo'].setValue(user.email);


        var tiposUsuarioSort = _.sortBy(this.tiposUsuario, 'id');
          //console.log(tiposUsuarioSort)
        var index = tiposUsuarioSort.indexOf(tiposUsuarioSort.find((x: any) => x.nombre == user.rol));
        var tipo = tiposUsuarioSort[index].id;
      //console.log(tipo)
        if(tipo!=null){
          this.valor = tipo;
        }


        }
        else{
        this.formUsuario.controls['empleado'].setValue('S');

        var value={value:this.formUsuario.controls['empleado'].value};
        this.radioEmpleado(value);

        this.formUsuario.controls['nombre'].setValue(user.usuario);
        var tiposUsuarioSort = _.sortBy(this.tiposUsuario, 'id');
        //console.log(tiposUsuarioSort)
        var index = tiposUsuarioSort.indexOf(tiposUsuarioSort.find((x: any) => x.nombre == user.rol));
        var tipo = tiposUsuarioSort[index].id;
        //console.log(tipo)

        if(tipo!=null){
          this.valor = tipo;
        }

        }
      }
      else{
        this.tiposUsuario = result.tipoUsRolesuario;
        this.editar = false;
        this.formUsuario.controls['empleado'].setValue('S');
        var value={value:this.formUsuario.controls['empleado'].value};
        this.radioEmpleado(value);

      }
        }
      }, error => {
        //console.log(error)
});

  }

  getUnidades(){
    this.http.get<any>(this.baseUrl + "api/ConsultaCatalogos/ConsultaUnidadesNegocio/{id?}").subscribe(result => {
      let unidadesActivas = [];
      this.unidades = [];
      // console.log(result)
      for (let index = 0; index < result.unidadesNegocio.length; index++) {
        if(result.unidadesNegocio[index].estatus=='Activo'){
          var i = unidadesActivas.indexOf(unidadesActivas.find(x=>x.id == result.unidadesNegocio[index].id));
          if(i == -1){
            unidadesActivas.push(result.unidadesNegocio[index]);
          }

        }

      }
      var usuario = JSON.parse(localStorage.getItem('usuarioEditar') ?? 'null');
      //console.log(usuario)
      this.unidades = unidadesActivas;
      //console.log(this.unidades)
      var indice = -1;
      for (let index = 0; index < this.unidades.length; index++) {
        indice = indice+1;
        this.unidades[index].disabled=true;
        this.unidades[index].expanded=false;
        this.unidades[index].checked= false;
        this.unidades[index].pertenece = false;
        this.unidades[index].indice= indice;
        var area=[];

        // for (let i = 0; i < this.areas.length; i++) {
        //   area.push(this.areas[i]);
        // }
        area = this.areas.map(x => Object.assign({}, x));
        this.unidades[index].areas = area;
        area = [];
      }
      if(localStorage.getItem('usuarioEditar')){
        var i = this.unidades.indexOf(this.unidades.find(x=>x.id == usuario.idUnidad));
        if(i!= -1){
          this.unidades[i].checked = true;
          this.unidades[i].disabled = false;
          this.unidades[i].pertenece = true;
        }
        for (let i = 0; i < this.unidadAreasGuardadas.length; i++) {
          var j = this.unidades.indexOf(this.unidades.find(x => x.id == this.unidadAreasGuardadas[i].idUnidad));
          this.unidades[j].checked = true;
          this.unidades[j].disabled=false;
          var k = this.unidades[j].areas.indexOf(this.unidades[j].areas.find((x: any)=>x.id == this.unidadAreasGuardadas[i].idArea));
          this.unidades[j].areas[k].checked = true;
        }
      }

      // console.log(this.unidades)
    }, err=>{

    }

    )

  }
  getAreas(){
    this.http.get<any>(this.baseUrl + "api/ConsultaCatalogos/ConsultaAreasNegocio/{id?}").subscribe(result => {
      let areasActivas = [];
      this.areas = [];
      for (let index = 0; index < result.areasNegocio.length; index++) {
        if(result.areasNegocio[index].estatus=='Activo'){
          areasActivas.push(result.areasNegocio[index]);
        }

      }

      this.areas = areasActivas;
      for (let index = 0; index < this.areas.length; index++) {
        this.areas[index].checked= false;
        }
      //console.log(this.areas)
    }, err=>{

    }
    )
  }

  filtrarEmpleados(){
    if(!this.empleados){
      return;
    }
    let busqueda =this.filtroEmpleados.value;
    if(!busqueda){
      this.empleadosFiltrados.next(this.empleados.slice());
      return;
    } else{
      busqueda = busqueda.toLowerCase();
    }

    this.empleadosFiltrados.next(
      this.empleados.filter(empleado=>empleado.nombre.toLowerCase().indexOf(busqueda) > -1)
    );
  }
  seleccionEmpleado(event: { value: { id: any } }) {

    var id = event.value.id;
    var datos = {idEmpleado:id};
    return this.http.get<any>(this.baseUrl + "api/Usuarios/GeneraUsuario/{id?}",{params:datos}).subscribe(result => {
      this.sugerenciaNombre = this.quitarAcentos(result.usuario);
      //console.log(result)
      this.formUsuario.controls['nombre'].setValue(this.sugerenciaNombre);
  }, error => {
     //console.log(error)
    });


  }
  quitarAcentos(cadena: string){
    const acentos: { [key: string]: string } = {'á':'a','é':'e','í':'i','ó':'o','ú':'u','Á':'A','É':'E','Í':'I','Ó':'O','Ú':'U'};
    return cadena.split('').map( letra => acentos[letra] || letra).join('').toString();
  }

  guardarUsuario(usuario:any){

    //console.log(this.unidadAreasSeleccionadas)
    //console.log(usuario)
      var iduser = this.empleadoCtrl.value;
    //console.log(iduser)
      var id;
      var correo;
      var pass: string | null | undefined;
      if(iduser!=null){
        id=iduser.id;
      }
      else{
        id=0;
      }
      if(usuario.correo!=null){
        correo = usuario.correo;
      }
      else{
        correo='';
      }
      var registroCheck:number = 0;
      if(this.checkRegBitacora ==false){
        registroCheck = 0;
      }else if(this.checkRegBitacora == true){
        registroCheck = 1;
      }


      var idUsuario = Number(localStorage.getItem('currentUser'));
      let user: CatUsuarios | null = null;

      //si no es empleado

      if(!this.editar){
        var lista=[];
        if(this.unidadAreasSeleccionadas.length>0){
          for (let i = 0; i < this.unidadAreasSeleccionadas.length; i++) {
            var reg:RelacionUsuarioUnidadArea;
            reg = {IdUser:0 ,IdUnidad:this.unidadAreasSeleccionadas[i].unidad, IdArea:this.unidadAreasSeleccionadas[i].area}
            lista.push(reg);
          }
          if(usuario.empleado=='N'){
            this.insertaFlag=false;

            if(usuario.usuario=='' || usuario.nombre=='' || usuario.correo == '' || usuario.tiposUsuario == 0){
              this.insertaFlag=false;
            }
            else{
              this.insertaFlag=true;
            }
            user = {Usuario:usuario.usuario, Nombre: usuario.nombre.toUpperCase(),
              IdRol:usuario.tipoUsuario,IdEmpleado:id,Email:usuario.correo,IdUsrRegistro:idUsuario, ListaUnidadArea:lista, Registro: registroCheck};
          }
          else if(usuario.empleado=='S'){
            if(usuario.nombre=='' || usuario.tipoUsuario ==0 || id ==0){
              this.insertaFlag=false;
            }
            else{
              this.insertaFlag=true;
            }
            user = {Usuario:usuario.nombre ,
              IdRol:usuario.tipoUsuario,IdEmpleado:id,IdUsrRegistro:idUsuario, ListaUnidadArea:lista,  Registro: registroCheck};
          }
          this.tieneUnidadArea = true;
        }
        else{
          this.tieneUnidadArea = false;


        }


      }


  if(!this.editar){
    //console.log(this.insertaFlag)
    if(this.insertaFlag){
      if(this.tieneUnidadArea){
        this.http.post<any>(this.baseUrl + "api/Usuarios/InsertaUsuario/{id?}",user).subscribe(result => {
          if(result > 0){
            this.dialog.closeAll();
            localStorage.removeItem('usuarioEditar');
              this.titulo = "Usuario Creado";
              this.mensaje = "El usuario ha sido creado correctamente"
              this.toastr.success(this.mensaje, this.titulo);
          }
          else if(result == -1){
            this.titulo = "Error";
            this.mensaje = "Ha ocurrido un error al intentar registrar el usuario, intentalo más tarde"
            this.toastr.error(this.mensaje, this.titulo);
          }
          else if( result == -2){
            this.titulo = "Advertencia";
            this.mensaje = "Este usuario ya existe, por favor verifica los datos"
            this.toastr.warning(this.mensaje, this.titulo);
          }

      }, error => {
        this.titulo = "Error";
        this.mensaje = "Ha ocurrido un error al intentar guardar el usuario"
        this.toastr.error(this.mensaje, this.titulo);
        //console.log(error)
      });
      }
      else{
        this.titulo = "Error";
        this.mensaje = "Debes seleccionar al menos una unidad y al menos un área"
        this.toastr.error(this.mensaje, this.titulo);
      }

    }
    else{
      //console.log(user)
      this.titulo = "Error";
      this.mensaje = "No puedes dejar campos vacíos"
      this.toastr.error(this.mensaje, this.titulo);
    }

  }
  else{
    var us = localStorage.getItem('usuarioEditar') ?? 'null';
    var usuarioParse = JSON.parse(us);
    //console.log("usuariop")
      //console.log(usuarioParse)
      this.listaEditar = [];
      if(this.unidadAreasSeleccionadas.length>0){
        for (let i = 0; i < this.unidadAreasSeleccionadas.length; i++) {
          var reg:RelacionUsuarioUnidadArea;
          reg = {IdUser:usuarioParse.idUser ,IdUnidad:this.unidadAreasSeleccionadas[i].unidad, IdArea:this.unidadAreasSeleccionadas[i].area}
          this.listaEditar.push(reg);
        }

    if(usuario.empleado=='N'){

      if(usuarioParse.nombre=='' || usuario.tipoUsuario == 0 || usuario.correo == ''){
        this.modificaFlag = false;
        this.titulo = "Error";
        this.mensaje = "No puedes dejar campos vacíos"
        this.toastr.error(this.mensaje, this.titulo);
      }
      else{
        this.modificaFlag = true;
      }

      if(this.nuevaCont=='' || this.nuevaCont==undefined || this.nuevaCont == null){
        pass = null;
      }
      else{
        if(this.nuevaCont == this.confirmaCont){
          if(this.nuevaCont.length > 3 && this.nuevaCont.length<11){
            pass = this.nuevaCont;
            this.modificaFlag = true;
          }
          else{

            this.modificaFlag = false;
            this.titulo = "Error";
            this.mensaje = "El tamaño de la contraseña debe ser entre 4 y 10 caracteres"
            this.toastr.error(this.mensaje, this.titulo);
          }

        }else{
          this.modificaFlag=false;
          this.titulo = "Error";
          this.mensaje = "Las contraseñas no coinciden, favor de verificarlas"
          this.toastr.error(this.mensaje, this.titulo);
        }
      }
      user = {Usuario:usuarioParse.usuario, Nombre: usuarioParse.nombre.toUpperCase(),  IdUser: usuarioParse.idUser,
        IdRol:usuario.tipoUsuario,IdEmpleado:id,Email:usuario.correo,IdUsrModificacion:idUsuario,Password: pass === null ? undefined : pass, ListaUnidadArea:this.listaEditar,  Registro: registroCheck};
        this.editaEmpleadoConArea = true;
    }
    else if(usuario.empleado=='S'){
      this.editaEmpleadoConArea = false;
      var index = this.unidades.indexOf(this.unidades.find(x=>x.pertenece == true));
      if(index!=-1){
        for (let i = 0; i < this.unidades[index].areas.length; i++) {
          if(this.unidades[index].areas[i].checked == true){
            this.editaEmpleadoConArea = true;
          }
        }
      }
      if(id==0 || usuario.tipoUsuario == 0){
        this.modificaFlag = false;
        this.titulo = "Error";
        this.mensaje = "No puedes dejar campos vacíos"
        this.toastr.error(this.mensaje, this.titulo);
      }
      else{
        this.modificaFlag = true;
      }
      if(this.nuevaCont=='' || this.nuevaCont==undefined || this.nuevaCont == null){
        pass = null;
      }
      else{
        if(this.nuevaCont == this.confirmaCont){
          if(this.nuevaCont.length > 3 && this.nuevaCont.length<11){
            pass = this.nuevaCont;
            this.modificaFlag = true;
          }
          else{

            this.modificaFlag = false;
            this.titulo = "Error";
            this.mensaje = "El tamaño de la contraseña debe ser entre 4 y 10 caracteres"
            this.toastr.error(this.mensaje, this.titulo);
          }

        }else{
          this.modificaFlag=false;
          this.titulo = "Error";
          this.mensaje = "Las contraseñas no coinciden, favor de verificarlas"
          this.toastr.error(this.mensaje, this.titulo);
        }
      }
      user = {Usuario:usuarioParse.nombre , IdUser: usuarioParse.idUser,
        IdRol:usuario.tipoUsuario,IdEmpleado:id,IdUsrModificacion:idUsuario, Password: pass === null ? undefined : pass, ListaUnidadArea:this.listaEditar,  Registro: registroCheck};
    }
    this.tieneUnidadArea = true;
  }
  else{
    this.tieneUnidadArea = false;
    this.titulo = "Error";
    this.mensaje = "Debes seleccionar al menos una unidad y al menos un área"
    this.toastr.error(this.mensaje, this.titulo);
  }
    if(this.modificaFlag){
      if(this.tieneUnidadArea){
        if(this.editaEmpleadoConArea){
          this.editaEmpleadoConArea = false;
          this.http.put<any>(this.baseUrl + "api/Usuarios/ModificaUsuario/{id?}",user).subscribe(result => {
            this.dialog.closeAll();
            localStorage.removeItem('usuarioEditar');
            this.titulo = "Usuario Modificado";
            this.mensaje = "El usuario ha sido modificado correctamente"
            this.toastr.success(this.mensaje, this.titulo);
        }, error => {
          this.titulo = "Error";
          this.mensaje = "Ha ocurrido un error al intentar modificar el usuario"
          this.toastr.error(this.mensaje, this.titulo);
           //console.log(error)
          });
        }
        else{
          this.titulo = "Error";
          this.mensaje = "Debes seleccionar al menos un área de la unidad a la que pertenece este usuario"
          this.toastr.error(this.mensaje, this.titulo);
        }
      }
      else{
        this.titulo = "Error";
        this.mensaje = "Debes seleccionar al menos una unidad y al menos un área"
        this.toastr.error(this.mensaje, this.titulo);
      }

    }


  }

  }
  cancelar(){
    this.dialog.closeAll();
  }
  ev(){
    $('.reportes').dblclick();
  }

}
