import { Component, OnInit, Inject, Injectable, OnDestroy, HostListener, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';  
import { MatSelectChange, MatSelect } from '@angular/material/select';
import { AppDateAdapter, APP_DATE_FORMATS } from './format-datepicker';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Globals } from '../../services/globals';
import moment from 'moment';
import { takeUntil, take } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RegistroBitacora } from '../../models/registroBitacora';
import {BitacoraService} from '../../services/bitacora.service';
import { ToastrService } from 'ngx-toastr';
import { Subject, ReplaySubject } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthenticationService } from '../../services/authentication.service';
import { HttpParams } from '@angular/common/http';
import  $ from 'jquery';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'form-bitacora',
  templateUrl: './form-bitacora.component.html',
  styleUrls: ['./form-bitacora.component.css'],
  providers: [
    {provide: DateAdapter, useClass: AppDateAdapter},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ],
  standalone:false
})
@Injectable({providedIn:'root'})
export class FormBitacoraComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
    this._onDestroy.next();
    this._onDestroy.complete();
  }
  registrosBitacora:any[]=[];
  numRegistrosBitacora:number = 0;
  formBitacora: FormGroup = new FormGroup({});

  fechaSel:boolean=false;
  proyectoSelN:boolean=false;
  proyectoSel:boolean=false;
  faseSel:boolean=false;
  actividadSel:boolean=false;
  detalleSel:boolean=false;
  eventoSel:boolean=false;
  duracionSel:boolean=false;
  seleccion:any;
  diaSemana:Date | undefined;
  fechaAct = moment();
  fechaMoment = moment();
  lunes: Date | undefined;
  fechaCalculo: any;
  fechaEditar;
  fechaMinima:Date | undefined;
  fechaSuma;
  proyectoId: any;
  fecha = new Date(new Date().setHours(0,0,0,0));
  minDate:Date;
  maxDate:Date;
  proyectos: any[] =[];
  actividades: any[]=[];
  eventosExtra: any []=[];
  fases: any[] = [];
  idUsuario: any;
  campos:string="";
  fechaDatePicker: Date = new Date(new Date().setHours(0, 0, 0, 0));;
  edicionRegistro = false;
  registroEditar: any;
  sumaHoras: any;
  registroActual:number=0;
  esEditable:boolean=false;
  actualMostrar:number=1;
  editar:boolean = false;
  mostrarFase: boolean = false;
  titulo: any;
  mensaje: any;
  proyectoSeleccionado: any;
  etapaSeleccionada: any;
  actividadSeleccionada: any;
  eventoSeleccionado: any;
  tieneErrores: any;

  titleProy = "";
  titleEtapa="";
  titleActividad="";
  titleEvento="";

  _onDestroy = new Subject<void>();
 filtroProyecto:FormControl = new FormControl();
 filtroEtapa:FormControl = new FormControl();
 filtroActividad:FormControl = new FormControl();
 filtroEvento:FormControl = new FormControl();


 proyectoCtrl:FormControl = new FormControl();
 etapaCtrl:FormControl = new FormControl();
 actividadCtrl:FormControl = new FormControl();
 eventoCtrl:FormControl = new FormControl();

 proyectosFiltrados:ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
 etapasFiltradas:ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
 actividadesFiltradas:ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
 eventosFiltrados:ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
 formExpandido:boolean | undefined;
 histExpandido:boolean | undefined;
  filtroFecha = (d:Date):boolean =>{
    const day = d.getDay();
    return day !==0 && day !==6;
  }
  @ViewChild("fasef", {static: false}) Etapa!: MatSelect 
  @ViewChild("proyectoS", {static: false}) Proyecto!: MatSelect
  @ViewChild("actividadf", {static: false}) Actividad!: MatSelect
  @ViewChild("eventof", { static: false }) Evento!: MatSelect
  baseUrl: string = environment.baseURL;
  abrirSelect(select: any, event: KeyboardEvent){
    if(event.key=="Tab"){
      if(select=='fase'){
        this.Etapa.open();
      }
      else if(select == 'proyecto'){
        this.Proyecto.open();
      }
      else if(select== 'actividad'){
        this.Actividad.open();
      }
      else if(select=='evento'){
        this.Evento.open();
      }
    }
    
    
  }
  ngAfterViewChecked()
  {
    if(!localStorage.getItem('currentUser')){
      this.authenticationService.verificarSesion();
    }
    this.cdRef.detectChanges();
  }
  constructor(private spinner: NgxSpinnerService,private cdRef:ChangeDetectorRef,private toastr:ToastrService,private http:HttpClient,private fb:FormBuilder, private router:Router, private bitacoraService:BitacoraService, private authenticationService:AuthenticationService) {
    

    this.router.onSameUrlNavigation ='reload';
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
    var mes=parseInt(this.fechaAct.format('M'));
    var dia=parseInt(this.fechaAct.format('D'));
    var año=(this.fechaAct.year())
    
    this.maxDate = new Date(new Date(año,mes-1,dia).setHours(0,0,0,0));


    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    this.fechaCalculo=this.fechaMoment;
    for (let index = 0; index < 6; index++) {
      this.diaSemana = new Date(new Date(this.fechaCalculo.format('LLLL')).setHours(0,0,0,0));
      var day = days[this.diaSemana.getDay()];
      if(day !=='Monday'){
        this.fechaCalculo=this.fechaCalculo.subtract(1,'d');
      }
      else{
        this.lunes= this.fechaCalculo;
      }

    }
    

    this.fechaEditar = moment(this.lunes).subtract(7,'d');
    var mesSuma=parseInt(moment(this.lunes).format('M'));
    var diaSuma=parseInt(moment(this.lunes).format('D'));
    var añoSuma=(moment(this.lunes).year());
    this.fechaSuma = new Date(new Date(añoSuma,mesSuma-1,diaSuma).setHours(0,0,0,0));
    var mesmin=parseInt(this.fechaEditar.format('M'));
    var diamin=parseInt(this.fechaEditar.format('D'));
    var añomin=(this.fechaEditar.year())
    
    this.minDate = new Date(new Date(añomin,mesmin-1,diamin).setHours(0,0,0,0));


    this.formBitacora = this.fb.group({
      fecha:['',Validators.required],
      
      proyectoText:['',null],
      
      //actividad:['',null],
      //evento:['',Validators.required],
      detalle:['',Validators.required],
      duracion:['',Validators.required],
      hideRequired: false,
      floatLabel: 'never',
    })

  }

  @HostListener('window:resize', ['$event'])
    onResize(event : Event) :void {
      this.resize();
  }
  ngAfterViewInit(){
    this.resize();
  }
  resize(){
    var hei=window.innerHeight;
    if(hei>= 658 && hei >= 985 ){
      $(".formRegistro").height(821);
    }else if(hei>= 658 && hei < 985 ){
      $(".formRegistro").height(821);
      $(".mat-drawer-content").css('height',hei - 35);
      $(".historial").height(hei - 35 );
      $(".mat-drawer-content").css('overflow','hidden');
    }
    else if(hei<658 && hei != 597){
      $(".formRegistro").height(509);
    }else{
      $(".formRegistro").height(509);
      $(".mat-drawer-content").css('height',hei - 35);
      $(".mat-drawer-content").css('overflow','hidden');
      $(".historial").height(hei - 35);
    }


  }
  ngOnInit() {
    this.idUsuario = Number(localStorage.getItem("currentUser"));
    this.recuperarProyectos(this.idUsuario);
    this.recuperarActividades();
    this.recuperarEtapas();
    this.recuperarRegistros();
    this.fechaMinima = new Date(new Date(this.fechaEditar.format('LLLL')).setHours(0,0,0,0));
    this.fechaDatePicker = new Date(new Date().setHours(0,0,0,0));
    this.filtroProyecto.valueChanges.pipe(takeUntil(this._onDestroy))
    .subscribe(()=>{
      this.filtraProyectos();
    })
    this.filtroEtapa.valueChanges.pipe(takeUntil(this._onDestroy))
    .subscribe(()=>{
      this.filtraEtapas();
    })
    this.filtroActividad.valueChanges.pipe(takeUntil(this._onDestroy))
    .subscribe(()=>{
      this.filtraActividades();
    })
    this.filtroEvento.valueChanges.pipe(takeUntil(this._onDestroy))
    .subscribe(()=>{
      this.filtraEventos();
    })
  }

  filtraProyectos(){
    if(!this.proyectos){
      return;
    }
    let busqueda = this.filtroProyecto.value.normalize('NFD')
    .replace(/([aeio])\u0301|(u)[\u0301\u0308]/gi,"$1$2")
    .normalize();
   
    if(!busqueda){
      this.proyectosFiltrados.next(this.proyectos.slice());
      return;
    }
    else{
      busqueda = busqueda.toLowerCase();
    }

    this.proyectosFiltrados.next(
      this.proyectos.filter(proyecto=>this.acentos(proyecto.nombre.toLowerCase()).indexOf(busqueda) > -1)
    );
  }

  acentos(text : String){
    text = text.normalize('NFD')
    .replace(/([aeio])\u0301|(u)[\u0301\u0308]/gi,"$1$2")
    .normalize();
    return text.toString();
  }
  filtraEtapas(){
    if(!this.fases){
      return;
    }
    let busqueda = this.filtroEtapa.value.normalize('NFD')
    .replace(/([aeio])\u0301|(u)[\u0301\u0308]/gi,"$1$2")
    .normalize();
    if(!busqueda){
      this.etapasFiltradas.next(this.fases.slice());
      return;
    }
    else{
      busqueda = busqueda.toLowerCase();
    }

    this.etapasFiltradas.next(
      this.fases.filter(fase=>this.acentos(fase.nombre.toLowerCase()).indexOf(busqueda) > -1)
    );
  }
  filtraActividades(){
    if(!this.actividades){
      return;
    }
    let busqueda = this.filtroActividad.value.normalize('NFD')
    .replace(/([aeio])\u0301|(u)[\u0301\u0308]/gi,"$1$2")
    .normalize();
    if(!busqueda){
      this.actividadesFiltradas.next(this.actividades.slice());
      return;
    }
    else{
      busqueda = busqueda.toLowerCase();
    }

    this.actividadesFiltradas.next(
      this.actividades.filter(actividad=>this.acentos(actividad.nombre.toLowerCase()).indexOf(busqueda) > -1)
    );
  }
  filtraEventos(){
    if(!this.eventosExtra){
      return;
    }
    let busqueda = this.filtroEvento.value.normalize('NFD')
    .replace(/([aeio])\u0301|(u)[\u0301\u0308]/gi,"$1$2")
    .normalize();
    if(!busqueda){
      this.eventosFiltrados.next(this.eventosExtra.slice());
      return;
    }
    else{
      busqueda = busqueda.toLowerCase();
    }

    this.eventosFiltrados.next(
      this.eventosExtra.filter(evento=>this.acentos(evento.nombre.toLowerCase()).indexOf(busqueda) > -1)
    );
  }
  get pagina(){
    return Globals.pagina;
  }
  get movil(){
    return Globals.movil;
  }
  fechaSeleccion(event:MatDatepickerInputEvent<Date>){
    if(event.value!=null){
      this.fechaSel=false;
    }
    else{
      this.fechaSel=true;
    }
  }
  proyectoSeleccion(event : Event){

    
    var index = this.proyectos.indexOf(this.proyectos.find(x => x.id == event));
    if(index!=-1){
      this.titleProy = this.proyectos[index].nombre
    }
    //const fase = this.formBitacora.get('fase');
    const fase = this.etapaSeleccionada;
    const actividad = this.actividadSeleccionada;
    //const actividad = this.formBitacora.get('actividad');
    const evento = this.eventoSeleccionado;
    const proyectoText=this.formBitacora.get('proyectoText');
    if(Number(event)>0){
      this.proyectoSel=true;
      this.mostrarFase = true;
      //this.formBitacora.get('fase').enable();
      //this.formBitacora.get('fase').setValue('');
      // // //this.etapaSeleccionada = undefined;
      this.eventoSeleccionado = undefined;
      //this.formBitacora.get('evento').setValue('');
   
      //evento.setValidators(null);
      //actividad.setValidators([Validators.required]);
      //fase.setValidators([Validators.required]);
      proyectoText!.setValidators(null);
     
    }else{
      this.proyectoSeleccionado = undefined;
      this.proyectoSel=false;
      this.mostrarFase = false;
      //this.formBitacora.get('fase').disable();
      //this.formBitacora.get('fase').setValue('');
      
      this.etapaSeleccionada = undefined;
      this.actividadSeleccionada = undefined;
      //this.formBitacora.get('actividad').setValue('');
  
      //fase.setValidators(null);
      //actividad.setValidators(null);
      //evento.setValidators([Validators.required]);
      proyectoText!.setValidators(null);
    }
    //evento.updateValueAndValidity();
    //actividad.updateValueAndValidity();
    //fase.updateValueAndValidity();
    proyectoText!.updateValueAndValidity();
  }



  faseSeleccion(event:MatSelectChange){
    var index = this.fases.indexOf(this.fases.find(x => x.id == event.value));
    if(index!=-1){
      this.titleEtapa = this.fases[index].nombre
    }
    if(event.value!=''){
      this.faseSel=false;
    }else{
      this.faseSel=true;
    }
  }

  actividadSeleccion(event:MatSelectChange){
    if(this.proyectoSeleccionado!=undefined){
      var index = this.actividades.indexOf(this.actividades.find(x => x.id == event.value));
      if(index!=-1){
        this.titleEvento = this.actividades[index].nombre
      }
    }else{
      var index = this.eventosExtra.indexOf(this.eventosExtra.find(x => x.id == event.value));
      if(index!=-1){
        this.titleEvento = this.eventosExtra[index].nombre
      }
    }
    
    if(event.value!=''){
      this.actividadSel=false;
    }else{
      this.actividadSel=true;
    }
  }

  detalleSeleccion(value:string){
    if(value==''){
      this.detalleSel=true;
    }
    else{
      this.detalleSel=false;
    }
  }

  duracionSeleccion(value:string){
    if(value==''){
      this.duracionSel=true;
    }
    else{
      this.duracionSel=false;
    }
  }
 
  recuperarProyectos(idUsuario : any){
    var datos = { idUser: idUsuario };
    var params = new HttpParams().set('idUser',datos.idUser);
    return this.http.get<any>(this.baseUrl + "api/Bitacora/GetProyectos/{id?}",{params})
    .subscribe(result=>{
      this.proyectos=result.proyectos;
      this.proyectosFiltrados.next(this.proyectos.slice());
      //console.log(this.proyectos)
    }, error=>{
      //console.log(error)
    })
  }


  recuperarEtapas(){
    return this.http.get<any>(this.baseUrl + "api/Bitacora/GetEtapas/{id?}")
    .subscribe(result=>{
      this.fases=result.etapas;
      this.etapasFiltradas.next(this.fases.slice());

    }, error=>{
      //console.log(error)
    })
  }
  recuperarActividades(){
    return this.http.get<any>(this.baseUrl + "api/Bitacora/GetActividades/{id?}")
    .subscribe(result=>{
      for (let index = 0; index < result.actividades.length; index++) {
        if(result.actividades[index].evento==0){
          this.actividades.push(result.actividades[index]);
          this.actividadesFiltradas.next(this.actividades.slice());
        }
        else if( result.actividades[index].evento==1){
          this.eventosExtra.push(result.actividades[index]);
          this.eventosFiltrados.next(this.eventosExtra.slice());
        }
        
      }
    }, error=>{
      //console.log(error)
    })
  }

  limpiarForm(){
    let fecha = this.fechaDatePicker ? new Date(this.fechaDatePicker) : new Date();
    this.titleProy = '';
    this.titleEtapa = '';
    this.titleEvento = '';
    this.titleActividad = '';
    this.formBitacora.reset({floatLabel: 'never'});
    this.fechaDatePicker = fecha;
    //const fase = this.formBitacora.get('fase');
    //const actividad = this.formBitacora.get('actividad');
    //const evento = this.formBitacora.get('evento');
    const proyectoText=this.formBitacora.get('proyectoText');
    //fase.setValidators(null);
    //actividad.setValidators(null);
    proyectoText!.setValidators(null);
    //evento.setValidators([Validators.required]);
    //evento.updateValueAndValidity();
    //actividad.updateValueAndValidity();
    //fase.updateValueAndValidity();
    this.proyectoSeleccionado = undefined;
    this.eventoSeleccionado = undefined;
    this.actividadSeleccionada = undefined;
    proyectoText!.updateValueAndValidity();
    //this.formBitacora.get('fase').disable();

    this.proyectoSel=false;

    this.edicionRegistro = false;
    this.mostrarFase = false;
  }
  guardarRegistro(value : any){
    this.campos="";
    var fecha = this.fechaDatePicker;
    if(this.edicionRegistro){
      this.formBitacora.controls['fecha'].setValue(fecha);
    }
    if(this.proyectoSeleccionado == -1){
      this.proyectoSeleccionado = undefined;
    }
    var proy = this.proyectoSeleccionado;
   
    //var fase = this.formBitacora.get('fase').value;
    var fase = this.etapaSeleccionada;
    //var evento = this.formBitacora.get('evento').value;
    var evento = this.eventoSeleccionado;
    var actividad = this.actividadSeleccionada;
    //var actividad = this.formBitacora.get('actividad').value;
    var detalle = this.formBitacora.get('detalle')!.value;
    var duracion = this.formBitacora.get('duracion')!.value;
    
    if(this.formBitacora.valid ){

     
      if(proy==undefined){
        if(this.eventoSeleccionado == undefined){
          this.campos+= "| Evento ";
        }
        if(this.eventoSeleccionado != undefined){
          if (value.duracion >8 || value.duracion <0) {
        
            this.campos+= "| Duración debe ser menor o igual a 8 Horas"
            this.titulo = "Advertencia";
            this.mensaje = "Revisar campo(s): "+this.campos;
            this.toastr.warning(this.mensaje, this.titulo);
          }
          else {
            if(this.edicionRegistro==false){            
              this.insertarBitacora(value);
                     
            }
            else{
              this.editarBitacora(value,fecha);
            }
          }
        }
        else{
            this.titulo = "Advertencia";
            this.mensaje = "Revisar campo(s): "+this.campos;
            this.toastr.warning(this.mensaje, this.titulo);
        }
      }
      else if(proy!= undefined){
        if(this.etapaSeleccionada == undefined){
          this.campos+= "| Etapa ";
        }
        if(this.actividadSeleccionada == undefined){
          this.campos+= "| Actividad ";
        }
        if(this.etapaSeleccionada != undefined && this.actividadSeleccionada != undefined){
          if (value.duracion >8 || value.duracion <0) {
           
            this.campos+= "| Duración debe ser menor o igual a 8 Horas "
            
            this.titulo = "Advertencia";
            this.mensaje = "Revisar campo(s): "+this.campos;
            this.toastr.warning(this.mensaje, this.titulo);
      
          }
          else{
            if(this.edicionRegistro==false){
              this.insertarBitacora(value);
         
            }
            else{
              this.editarBitacora(value,fecha);
            }
          }
        }else{
          this.titulo = "Advertencia";
            this.mensaje = "Revisar campo(s): "+this.campos;
            this.toastr.warning(this.mensaje, this.titulo);
        }
      }

    }
    
    else{
      
      if(proy == undefined){
        if(fecha==null){
          this.campos+= "| Fecha ";
        }
        if(evento == undefined || evento == ''){
          this.campos+= "| Evento ";
        }
        if(detalle=='' || detalle==null){
          this.campos+= "| Detalle ";
        }
        if(duracion=='' || duracion == null){
          this.campos+= "| Duracion ";
        }
        this.campos+="|";
      }
      if(proy!=undefined){
        if(fecha==null){
          this.campos+= "| Fecha ";
        }
        if(fase==undefined || fase==''){
          this.campos+= "| Etapa ";
        }
        if(actividad == undefined || actividad==''){
          this.campos+= "| Actividad ";
        }
        if(detalle=='' || detalle==null){
          this.campos+= "| Detalle ";
        }
        if(duracion=='' || duracion == null){
          this.campos+= "| Duracion ";
        }
        this.campos+="|";
      }
      this.titulo = "Advertencia";
      this.mensaje = "Revisar campo(s): "+this.campos;
      this.toastr.warning(this.mensaje, this.titulo);
     
     
    }

  }
  editarBitacora(value: any, fecha: any){
    this.spinner.show();
    var etapa = this.etapaSeleccionada;
    var actividad = this.actividadSeleccionada;
    var proyecto = this.proyectoSeleccionado;
    if( etapa==null){
      etapa="";
    }
    if( actividad==null){
      actividad="";
    }
    if( proyecto==null || proyecto==""){
      proyecto=undefined;
      actividad = this.eventoSeleccionado;
    }
    //console.log(actividad)
    var idUs = Number(localStorage.getItem('currentUser'));
    var datos:RegistroBitacora;
        datos = {id: this.registroEditar.id,fecha:fecha, IdUser:idUs, idProyecto:this.proyectoSeleccionado,idEtapa:this.etapaSeleccionada,idActividad:actividad,descripcion:value.detalle,
        duracion:value.duracion, fechaRegistro:value.fechaRegistro};
    return this.http.put(this.baseUrl + "api/Bitacora/ModificarBitacora/{id?}",datos).subscribe(res=>{
      this.spinner.hide();
      this.titulo = "Registro Editado";
      this.mensaje = "Tu registro se ha editado correctamente"
      this.toastr.success(this.mensaje, this.titulo);
      this.registroEditar=null;
      this.limpiarForm();
      this.recuperarRegistros();
      this.editar = false;
    }, err =>{
          this.spinner.hide();
          this.titulo = "Error";
          this.mensaje = "Ha ocurrido un error al intentar guardar los cambios, intentalo más tarde";
          this.toastr.error(this.mensaje, this.titulo);
      //console.log(err);
    })
  }
  insertarBitacora(value: any){
    this.spinner.show();
   
    var etapa = value.fase;
    var actividad = this.actividadSeleccionada;
    var proyecto = this.proyectoSeleccionado;
    //console.log(actividad)
   
   
    if( proyecto==null || proyecto==""){
      actividad = this.eventoSeleccionado;
    }
    var idUs = Number(localStorage.getItem('currentUser'));
    var datos:RegistroBitacora;
        datos = {id: 0,fecha:value.fecha, IdUser:idUs, idProyecto:proyecto,idEtapa:this.etapaSeleccionada,idActividad:actividad,descripcion:value.detalle,
        duracion:value.duracion, fechaRegistro:new Date()};
        return this.http.post<any>(this.baseUrl + "api/Bitacora/InsertaBitacora/{id?}",datos)
        .subscribe(result=>{
          if(result>0){          
            this.titulo = "Registro Guardado";
            this.mensaje = "Tu registro se ha guardado correctamente"
            this.toastr.success(this.mensaje, this.titulo);
            this.limpiarForm();
            this.recuperarRegistros();
          }
          else{
            this.titulo = "Error";
            this.mensaje = "Ha ocurrido un error al intentar guardar tu registro, verifica los datos ingresados";
            this.toastr.error(this.mensaje, this.titulo);
          }
          this.spinner.hide();
        }, error=>{
          this.spinner.hide();
          this.titulo = "Error";
          this.mensaje = "Ha ocurrido un error al intentar guardar tu registro, intentalo más tarde";
          this.toastr.error(this.mensaje, this.titulo);
          //console.log(error)
        })
   }

   recuperarRegistros(){    
      this.spinner.show();    
    this.bitacoraService.recuperarRegistros(localStorage.getItem('currentUser')).subscribe(
      res=>{  
        this.registrosBitacora = res.listaBitacora;
        this.numRegistrosBitacora = res.listaBitacora.length;
        if(this.numRegistrosBitacora>0){
          //console.log(this.registrosBitacora);
        this.sumaHoras=0;
        var fecha;
        for (let index = 0; index < this.numRegistrosBitacora; index++) {
          fecha= new Date(this.registrosBitacora[index].fecha);

          
          var mes=parseInt(moment(fecha).format('M'));
          var dia=parseInt(moment(fecha).format('D'));
          var año=(moment(fecha).year());
          var fechaFormat = dia+"/"+(mes)+"/"+año;
          this.registrosBitacora[index].fecha = fechaFormat;
   
          
          if(fecha>=this.fechaSuma){
         
            this.sumaHoras += this.registrosBitacora[index].duracion;
          }

          if(fecha>=this.fechaMinima!){
            this.registrosBitacora[index].editable = true;
          }
          else{
            this.registrosBitacora[index].editable=false;
          }
        }
   
        if(Globals.movil == true && Globals.pagina==1){

          this.registrosBitacora[this.registroActual];
          this.llenaDatos(this.registrosBitacora[this.registroActual]);
          
          this.esEditable = this.registrosBitacora[this.registroActual].editable;
          
        }
        }
        this.spinner.hide();
      },error=>{
        this.spinner.hide();
          this.titulo = "Error";
          this.mensaje = "Ha ocurrido un error al intentar recuperar tus registros de bitácora"
          this.toastr.error(this.mensaje, this.titulo);
        //console.log(error);
      }
    );
   }
   
   expandirHistorial(){
    if($('.historial').hasClass('col-lg-9')){
      $('.historial').removeClass("col-lg-9").addClass("col-lg-12");
      $('.formRegistro').removeClass("col-lg-3");
      $('.formRegistro').css("display","none");
      $('.mybtn').css('right',"97.9%");
      $('.izq').css("background-color","lightgrey");
      $('.izq mat-icon').css("color","black");
      this.histExpandido = true;
      this.formExpandido = false;
    }
    else if($('.formRegistro').hasClass('col-lg-12')){
      $('.formRegistro').removeClass('col-lg-12').addClass('col-lg-3');
      $('.historial').addClass("col-lg-9");
      $('.historial').css('display','block');
      $('.mybtn').css('right', "73.9%");
      $('.der').css("background-color","#0E3E62");
      $('.der mat-icon').css("color","lightgrey");
      this.formExpandido = false;
      this.histExpandido = false;
    }
  }
  expandirRegistro(){
    if($('.historial').hasClass('col-lg-12')){

      $('.historial').removeClass('col-lg-12').addClass('col-lg-9');
      $('.formRegistro').addClass("col-lg-3");
      $('.formRegistro').css('display', 'block');
      $('.mybtn').css('right', "73.9%");
      $('.izq').css("background-color","#0E3E62");
      $('.izq mat-icon').css("color","lightgrey");
      this.formExpandido =false;
      this.histExpandido = false;
    }
    else if($('.formRegistro').hasClass('col-lg-3')){
      $('.formRegistro').removeClass('col-lg-3').addClass('col-lg-12');
      $('.historial').removeClass("col-lg-9");
      $('.historial').css('display','none')
      $('.mybtn').css('right', "97.9%");
      $('.der').css("background-color","lightgrey");
      $('.der mat-icon').css("color","black");
      this.formExpandido = true;
      this.histExpandido =false;
    }
  }

  stringToDate(_date: any, _format : any, _delimiter: any)
  {
              var formatLowerCase=_format.toLowerCase();
              var formatItems=formatLowerCase.split(_delimiter);
              var dateItems=_date.split(_delimiter);
              var monthIndex=formatItems.indexOf("mm");
              var dayIndex=formatItems.indexOf("dd");
              var yearIndex=formatItems.indexOf("yyyy");
              var month=parseInt(dateItems[monthIndex]);
              month-=1;
              var formatedDate = new Date(dateItems[yearIndex],month,dateItems[dayIndex]);
              return formatedDate;
  }
  
  llenaDatos(registro : any){
    //console.log(registro)
    if(this.histExpandido == true && Globals.movil==false){
      this.expandirRegistro();
    }
    this.registroEditar = registro;
    var dateParts = registro.fecha.split("/");

// month is 0-based, that's why we need dataParts[1] - 1
    var fecha = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]); 
    //this.fechaDatePicker=new Date(registro.fecha);
    //var fecha = moment(registro.fecha);
    this.fechaDatePicker=new Date(fecha.setHours(0,0,0,0));
    //console.log(this.fechaDatePicker)
    //this.fechaDatePicker=this.stringToDate(registro.fecha,"dd/MM/yyyy","/");
    //this.formBitacora.controls['fecha'].setValue(registro.Fecha);

    this.edicionRegistro=true;
    if(registro.idProyecto!=0){
    this.formBitacora.controls['proyectoText'].setValue(registro.proyecto)
    this.proyectoSeleccion(registro.idProyecto);
    this.proyectoId = registro.idProyecto;
    this.proyectoSeleccionado = registro.idProyecto;
    this.etapaSeleccionada = registro.idEtapa;
    this.actividadSeleccionada = registro.idActividad;
    }
    else{
      //this.formBitacora.controls['eventoCtrl'].setValue(registro.idEtapa);
      this.proyectoId = registro.idProyecto;
      this.proyectoSeleccion(registro.idProyecto);
      
    this.etapaSeleccionada = registro.idEtapa;
    
      this.eventoSeleccionado = registro.idActividad;
    }
      
    
    this.formBitacora.controls['detalle'].setValue(registro.descripcion);
    this.formBitacora.controls['duracion'].setValue(registro.duracion);
    if(this.pagina==1 && this.movil==true){
      this.formBitacora.controls['fecha'].disable();
      this.formBitacora.controls['proyectoText'].disable();
      this.formBitacora.controls['detalle'].disable();
      this.formBitacora.controls['duracion'].disable();
      this.verificarDisabled();
    }
    this.spinner.hide();
  }
  anteriorRegistro(){
    this.limpiarForm();
    this.editar=false;
    if(this.registroActual>0){
      this.registroActual = this.registroActual-1;
      this.actualMostrar=this.actualMostrar-1;
      //console.log(this.registrosBitacora[this.registroActual]);
      this.llenaDatos(this.registrosBitacora[this.registroActual]);
      this.esEditable = this.registrosBitacora[this.registroActual].editable;
      this.formBitacora.controls['proyectoText'].setValue(this.registrosBitacora[this.registroActual].proyecto);
    }
    
  }
  verificarDisabled(){
    if(this.formBitacora.get('fecha')!.enabled){
      $('.fechaIn').css({"background-color":"white","border-radius":" 0px"});
    }
    else if(!this.formBitacora.get('fecha')!.enabled){
      $('.fechaIn').css({"background-color":"gainsboro","border-radius":" 0px;"});
    }

    if(this.editar){
       $('.proyectoIn').css({"background-color":"white","border-radius":" 0px"});
       $('.eventoIn').css({"background-color":"white","border-radius":" 0px"});
       $('.faseIn').css({"background-color":"white","border-radius":" 0px"});
       $('.actividadIn').css({"background-color":"white","border-radius":" 0px"});
    }
    else {
       $('.proyectoIn').css({"background-color":"gainsboro","border-radius":" 0px;"});
       $('.eventoIn').css({"background-color":"gainsboro","border-radius":" 0px;"});
       $('.faseIn').css({"background-color":"gainsboro","border-radius":" 0px;"});
       $('.actividadIn').css({"background-color":"gainsboro","border-radius":" 0px;"});
    }

    if(this.formBitacora.get('proyectoText')!.enabled){
      $('.proyectotxtIn').css({"background-color":"white","border-radius":" 0px"});
    }
    else if(!this.formBitacora.get('proyectoText')!.enabled){
      $('.proyectotxtIn').css({"background-color":"gainsboro","border-radius":" 0px;"});
    }

    if(this.formBitacora.get('detalle')!.enabled){
      $('.detalleIn').css({"background-color":"white","border-radius":" 0px"});
    }
    else if(!this.formBitacora.get('detalle')!.enabled){
      $('.detalleIn').css({"background-color":"gainsboro","border-radius":" 0px;"});
    }
    if(this.formBitacora.get('duracion')!.enabled){
      $('.duracionIn').css({"background-color":"white","border-radius":" 0px"});
    }
    else if(!this.formBitacora.get('duracion')!.enabled){
      $('.duracionIn').css({"background-color":"gainsboro","border-radius":" 0px;"});
    }
  }
  siguienteRegistro(){
    this.limpiarForm();
    this.editar=false;
    if(this.registroActual<this.numRegistrosBitacora-1){
      this.registroActual = this.registroActual+1;
      this.actualMostrar=this.actualMostrar+1;
      //console.log(this.registrosBitacora[this.registroActual]);
      this.llenaDatos(this.registrosBitacora[this.registroActual]);
      this.esEditable = this.registrosBitacora[this.registroActual].editable;
      this.formBitacora.controls['proyectoText'].setValue(this.registrosBitacora[this.registroActual].proyecto);

    }
  }
  mostrarDatos(registro : any){
   
    this.registroEditar = registro;
    this.fechaDatePicker=new Date(registro.fecha);
    this.formBitacora.controls['fecha'].setValue(registro.Fecha);

    this.edicionRegistro=true;
    if(registro.idProyecto!=0){
    this.proyectoSeleccion(registro.idProyecto);
    this.formBitacora.controls['proyecto'].setValue(registro.idProyecto);
    this.formBitacora.controls['fase'].setValue(registro.idEtapa);
    this.formBitacora.controls['actividad'].setValue(registro.idActividad);
    }
    else{
      this.formBitacora.controls['evento'].setValue(registro.idEtapa);
    }
    this.formBitacora.controls['detalle'].setValue(registro.descripcion);
    this.formBitacora.controls['duracion'].setValue(registro.duracion);
  }
  editarRegistro(){
    this.formBitacora.get('fecha')!.enable();
    this.formBitacora.get('proyectoText')!.enable();
    this.formBitacora.get('detalle')!.enable();
    this.formBitacora.get('duracion')!.enable();
    this.editar = true;
    if(this.movil==true){
      this.verificarDisabled();
    }
    
  }
  cancelar(){
   
    this.editar=false;
    this.formBitacora.get('fecha')!.enable();
    this.formBitacora.get('proyectoText')!.enable();
    this.formBitacora.get('detalle')!.enable();
    this.formBitacora.get('duracion')!.enable();
    this.llenaDatos(this.registrosBitacora[this.registroActual]);
  }
  
  timeStringToFloat(time: any) {
     var hoursMinutes = time.split(/[.:]/);
     if(hoursMinutes[1].length<2){
       hoursMinutes[1]= hoursMinutes[1]+'0';
     }
      var hours = parseInt(hoursMinutes[0], 10);
       var minutes = hoursMinutes[1] ? parseInt(hoursMinutes[1], 10) : 0; 
       //console.log("decimal a horas min")
    
    var hormin = hours + minutes / 60;
    //console.log(hormin)
    this.floatToString(hormin.toString());
  } 
  floatToString(time :any){
    var time = time.toString();
    var hoursMinutes = time.split(/[.:]/);
    if(hoursMinutes[1].length<2){
      hoursMinutes[1]= hoursMinutes[1]+'0';
    }
    var minutes = hoursMinutes[1] * 60 /100;
    var hours = hoursMinutes[0];
    //console.log("horas min a decimal")
    //console.log(hours +':'+ minutes);

  }
  
}
