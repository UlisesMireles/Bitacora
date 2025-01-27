import { Component, OnInit, HostListener, Inject, ChangeDetectorRef } from '@angular/core';
import {Globals} from '../../services/globals';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AvanceReal } from '../../models/avanceReal';
import moment from 'moment';
import { ReportesService } from '../../services/reportes.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthenticationService } from '../../services/authentication.service';
import { HttpParams } from '@angular/common/http';
import $ from 'jquery';
/*import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';*/
@Component({
  selector: 'app-avance-real',
  templateUrl: './avance-real.component.html',
  styleUrls: ['./avance-real.component.css'],
  standalone:false
})
export class AvanceRealComponent implements OnInit {
  movil: any;
  avanceRealRegs: any = [];
  config:any;
  avanceEditado: any = [];
  unidadesNegocio:any;
  unidadNombre: any;
  unidad=0;
  unidadSeleccionada=-1;
  areaSeleccionada=0;
  areasUnidades:any;
  registrosPorPagina=10;
  iniciando = true;
  jsonArray=[];
  titulo: any;
  coleccion=0;
  paginas = 0;
  pag = "página"
  mensaje: any;
  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    this.movil = Globals.movil;
    if(this.movil==true){
      if(Globals.pagina==2){
        $(".mat-ink-bar").css({display:'none'});
      }

    }
  }
  constructor(private spinner: NgxSpinnerService,private cdRef:ChangeDetectorRef,private authenticationService:AuthenticationService,private toastr:ToastrService, private http:HttpClient,@Inject("BASE_URL") private baseUrl:string, private serviceReportes:ReportesService) {
    this.config= {
      itemsPerPage: 10,
      currentPage:1,
      totalItems: this.avanceRealRegs.length
    }
    localStorage.setItem('currentUser', '4281');
    localStorage.setItem('userName', 'ulises.mireles');
    localStorage.setItem('rol', '1');
  }
  ngAfterViewChecked()
  {
    if(!localStorage.getItem('currentUser')){
      this.authenticationService.verificarSesion();
    }
    this.cdRef.detectChanges();
  }
  ngOnInit() {
    Globals.pagina = 2;
    this.getUnidades();
    this.getAreas();
    this.getAvanceReal();
    Globals.usuario = '4281';
    Globals.rolUser = '1';
    console.log(localStorage.getItem('currentUser'));
    console.log(this.unidadesNegocio);
    try {
      // Código que podría fallar
    } catch (error) {
      console.error('Error en ngOnInit:', error);
    }
  }
  getUnidades(){

    this.serviceReportes.getUnidades(Number(localStorage.getItem('currentUser'))).subscribe(res=>{
      this.unidadesNegocio = res.avanceR;
      console.log(this.unidadesNegocio);
    }, err=>{}
    //console.log(err)
    );

  }
  getAreas(){
    this.serviceReportes.getAreas(Number(localStorage.getItem('currentUser'))).subscribe(result=>{
      this.areasUnidades = result.avanceR;
    }, err=>{}
    //console.log(err)
    )

  }
  getAreasConUnidad(idUnidad: any){
    this.serviceReportes.getAreasConUnidad(Number(localStorage.getItem('currentUser')),idUnidad).subscribe(result=>{
      this.areasUnidades = result.avanceR;
    }, err=>{}//console.log(err)
    );
  }
  getUnidadConAreas(idArea: any){
    this.serviceReportes.getUnidadConAreas(Number(localStorage.getItem('currentUser')),idArea).subscribe(result=>{
      this.unidadesNegocio = result.avanceR
    })
  }
  getAvanceReal() {
    this.spinner.show();
    var idUsuario = localStorage.getItem('currentUser');
    var datos = { idUser: idUsuario||'', idUnidad: this.unidad.toString(), idArea: this.areaSeleccionada.toString() };
    var params = new HttpParams()
      .set('idUser', datos.idUser)
      .set('idUnidad', datos.idUnidad)
      .set('idArea', datos.idArea);
      this.http.get<any>(this.baseUrl + "api/AvanceReal/AvanceAvanceReal/{id?}",{params}).subscribe(result => {

        this.avanceRealRegs = result.avanceR;
        this.coleccion = result.avanceR.length;
        this.paginas = Math.ceil(this.coleccion/10);
        if(this.paginas > 1){
          this.pag = "páginas";
        }
        else if(this.paginas == 1){
          this.pag = "página";
        }
        // console.log(this.avanceRealRegs)
        var fecha;
        for (let index = 0; index < this.avanceRealRegs.length; index++) {
          fecha = new Date(this.avanceRealRegs[index].fechaRegistro);
          //console.log(fecha)
          var mes=parseInt(moment(fecha).format('M'));
          var dia=parseInt(moment(fecha).format('D'));
          var año=(moment(fecha).year());
          var fechaFormat = dia+"/"+(mes)+"/"+año;
          this.avanceRealRegs[index].fechaRegistro = fechaFormat;
        }
        if(this.unidadNombre !=undefined){
          if(this.unidadNombre.length>0){
            this.unidadNombre = result.avanceR[0].unidad;

          var index = this.unidadesNegocio.indexOf(this.unidadesNegocio.find((x: { nombre: any; }) => this.unidadNombre == x.nombre));

          this.unidadSeleccionada = this.unidadesNegocio[index].id;
          this.unidad=this.unidadSeleccionada;
          // if(this.iniciando==true){
          //   this.getAreasConUnidad(this.unidadSeleccionada);
          // }
          }
        }

        this.spinner.hide();
    }, error => {
      this.spinner.hide();
      this.titulo = "Error";
      this.mensaje = "Ha ocurrido un error al obtener datos"
      this.toastr.error(this.mensaje, this.titulo);
      //console.log(error)
    });

  }

  pageChanged(event: any){
    this.config.currentPage = event;
    $('body, html, tbody').scrollTop(0);
  }

  actualizarAreas(unidad: any){
    var dato = this.areaSeleccionada;
    this.unidadSeleccionada = unidad;
    if(unidad!="-1" && this.areaSeleccionada == 0){
      this.getAreasConUnidad(unidad);
    }
    else if(unidad == "-1" && this.areaSeleccionada == 0){
      this.getAreas();
    }
    // if(event=="-1"){
    //   this.getAreas();
    //   this.unidad = event;
    //   this.unidadSeleccionada = event;
    //   this.areaSeleccionada = dato;
    // }
    // else{
    //   this.getAreasConUnidad(event);
    //   this.unidad = event;
    //   this.unidadSeleccionada = event;
    //   this.areaSeleccionada = dato;
    // }

  }
  actualizarUnidades(area: any){
    var dato = this.unidadSeleccionada;
    this.areaSeleccionada = area;
    if(area!="0" && this.unidadSeleccionada == -1){
      this.getUnidadConAreas(area);
    }
    else if(area == "0" && this.unidadSeleccionada == -1){
      this.getUnidades();
    }
    // if(event=="0"){
    //   this.getUnidades();
    //   this.areaSeleccionada = event;
    //   this.unidadSeleccionada = dato;
    // }
    // else{
    //   this.getUnidadConAreas(event);
    //   this.areaSeleccionada = event;

    // }

  }
  buscarAvanceReal(unidad: any,area: any){
    this.areaSeleccionada = area;
    this.unidad = unidad;
    this.iniciando=false;
    this.getAvanceReal();
  }
  registroEditado(avanceNuevo: any, avance: any){
    //console.log(avanceNuevo)
    //console.log(avance)

    var registroAvance:AvanceReal={idProyecto:avance.idProyecto,avanceReal:Number(avanceNuevo),
    avance:avance.avance};
    var idProy = avance.idProyecto;
/*    var index = this.avanceEditado.indexOf(this.avanceEditado.find(x => idProy == x.idProyecto));*/
    var index = this.avanceEditado.indexOf(this.avanceEditado.find((x: { idProyecto: any; }) => idProy == x.idProyecto));
    //console.log(index)
    if(index==-1){
      this.avanceEditado.push(registroAvance);
    }
    else{
      this.avanceEditado.splice(index,1);
      this.avanceEditado.push(registroAvance);
    }


  }
  guardarCambios(){

    this.http.post<any>(this.baseUrl + "api/AvanceReal/ActualizarAvanceReal/",this.avanceEditado)
    .subscribe(result=>{
      //console.log(result.avanceR);
      this.titulo = "Avance Real Actualizado";
      this.mensaje = "El avance real ha sido actualizado correctamente"
      this.toastr.success(this.mensaje, this.titulo);
      this.buscarAvanceReal(this.unidad,this.areaSeleccionada);
    }, err => {
      this.titulo = "Error";
      this.mensaje = "Ha ocurrido un error al intentar actualizar el avance real, intente más tarde"
      this.toastr.error(this.mensaje, this.titulo);
      //console.log(err)
    })
  }

}

