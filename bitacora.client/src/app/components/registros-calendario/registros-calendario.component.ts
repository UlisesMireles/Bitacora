import { Component, OnInit, Injectable, Inject, HostListener, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import {BitacoraService} from '../../services/bitacora.service';
import  moment from 'moment';
import { Router } from '@angular/router';
import {Globals} from '../../services/globals';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthenticationService } from '../../services/authentication.service';
import $ from 'jquery';

@Component({
  selector: 'registros-calendario',
  templateUrl: './registros-calendario.component.html',
  styleUrls: ['./registros-calendario.component.css'],
  providers: [DatePipe],
  standalone: false,
})
@Injectable({providedIn:'root'})
export class RegistrosCalendarioComponent implements OnInit {
 
  @Output() datosForm: EventEmitter<any[]> =   new EventEmitter();
  idUsuario:number=0;
  regBitacora: number = 0;
  pagBitacora: number = 0;
  rangoRegs:string = "1 a 10";
  rangoRegsFiltro:string = "1 a 10";
  numRegistrosFiltrados:number = 0;
  diaSemana:Date | undefined;
  fechaAct = moment();
  fechaMoment = moment();
  lunes: Date | undefined;
  fechaCalculo: any;
  fechaEditar;
  config:any;
  editable:boolean | undefined;
  registrosPorPagina=10;
  coleccion:number = 0;
  busqueda="";
  filtroActivo:boolean=false;
  headers:HttpHeaders = new HttpHeaders({
    "Content-type":"application/json"
  })
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
      this.resize();
  }
  @Input() registrosBitacora:any[] = [];
  @Input() numRegistrosBitacora:number =0;
  @Input() sumaHoras:number=0;
  constructor(private authenticationService:AuthenticationService,private cdRef:ChangeDetectorRef,private spinner: NgxSpinnerService,private router:Router, private bitacoraService:BitacoraService,private http:HttpClient, @Inject("BASE_URL") private baseUrl:string, private datePipe: DatePipe) {
    
    this.router.routeReuseStrategy.shouldReuseRoute = function(){
      return false;
    }
    this.config= {
      itemsPerPage: this.registrosPorPagina,
      currentPage:1,
      totalItems: this.coleccion
    }
    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    this.fechaCalculo=this.fechaMoment;
    for (let index = 0; index < 6; index++) {
      this.diaSemana = new Date(this.fechaCalculo.format('LLLL'));
      var day = days[this.diaSemana.getDay()];
      if(day !=='Monday'){
        this.fechaCalculo=this.fechaCalculo.subtract(1,'d');
      }
      else{
        this.lunes= this.fechaCalculo;
      }

    }
    this.fechaEditar = moment(this.lunes).subtract(7,'d');

   }

   ngAfterViewChecked()
  {
    if(!localStorage.getItem('currentUser')){
      this.authenticationService.verificarSesion();
    }
    this.cdRef.detectChanges();
  }
  ngAfterViewInit(){
    this.resize();
  }
  resize(){
    var hei=window.innerHeight;
    if(hei >= 985){

      $(".tablabody").height(hei - 387);

    }else if(hei> 821 && hei < 985 ){

      $(".tablabody").height(hei - 387);

    }else if(hei <= 821 && hei > 730){

      $(".tablabody").height(hei - 387);

    }else if(hei <= 730 && hei > 657){

      $(".tablabody").height(hei - 387);

    }
    else if(hei <= 657 && hei > 597){

      $(".tablabody").height(301);

    }else{

      $(".tablabody").height(hei - 387);
    }
  }
 
  ngOnInit() {
    Globals.filtroBitacora=0;
    this.idUsuario = Number(localStorage.getItem("currentUser"));
    var hei=window.innerHeight;
    if(hei>= 658){
      $(".tablabody").height(574);
    }
    else if(hei<658 ){
      $(".tablabody").height(305);
    }
  }

 
  
  cambioRPP(event: any){
    this.config.itemsPerPage = event;
    this.config.currentPage = 1;
    this.registrosPorPagina = event;
    if(this.filtroActivo){
      if(event>this.numRegistrosFiltrados){
        this.rangoRegsFiltro = "1 a "+this.numRegistrosFiltrados;
      }
      else{
        this.rangoRegsFiltro = "1 a "+event;
      }
    }
    else{
      if(event > this.numRegistrosBitacora){
        this.rangoRegs = "1 a "+this.numRegistrosBitacora;
      }
      else{
        this.rangoRegs = "1 a "+event;
      }
    }
    
  }
  pageChanged(event: any){
    this.config.currentPage = event;
    this.pagBitacora = Math.ceil(this.numRegistrosBitacora/this.registrosPorPagina);
    //console.log(this.pagBitacora)
    //console.log(event)
    //console.log("filtro "+this.filtroActivo)
    if(this.filtroActivo==true){
      this.pagBitacora = Math.ceil(this.numRegistrosFiltrados/this.registrosPorPagina);
      if(event == this.pagBitacora){
        var x = this.numRegistrosFiltrados;
        this.rangoRegsFiltro =((event*this.registrosPorPagina)-this.registrosPorPagina+1)+" a "+x;
      }else{
        var x = this.registrosPorPagina*event;
        this.rangoRegsFiltro =(x-this.registrosPorPagina+1)+" a "+x;
      }
    }
    else{
      if(event == this.pagBitacora){
        var x = this.numRegistrosBitacora;
        this.rangoRegs =((event*this.registrosPorPagina)-this.registrosPorPagina+1)+" a "+x;
      }else{
        var x = this.registrosPorPagina*event;
        this.rangoRegs =(x-this.registrosPorPagina+1)+" a "+x;
      }
    }
    
  }
   eliminar(id: any){
    this.bitacoraService.eliminarRegistro(id);
  }
  editar(registro: any){
    this.datosForm.emit(registro)
  }
  filtroBitacora(filtro: any){
    if(filtro==''){
      this.filtroActivo=false;
    }
    else{
      this.filtroActivo=true;
      this.numRegistrosFiltrados = Globals.filtroBitacora;
      this.pageChanged(1);
    }
  }
}
