import { Component, OnInit, HostListener, Inject, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { Globals } from '../../services/globals';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from '../../services/authentication.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ReportesService } from '../../services/reportes.service';
import { DescargaService } from '../../services/descarga.service';
import moment from 'moment';
//import { DateAdapter, MAT_DATE_FORMATS, MatDialogRef, MAT_DIALOG_DATA, MatDialog, MatSelectChange, MatSelect  } from '@angular/material';
// import { AppDateAdapter, APP_DATE_FORMATS } from '../form-bitacora/format-datepicker';
import { saveAs } from 'file-saver';
// import { AuthenticationService } from 'src/app/services/authentication.service';
import { Chart, ChartType, registerables, ChartConfiguration, ChartEvent } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import 'chartjs-plugin-datalabels';
import { MatSelect } from '@angular/material/select';
import { Observable, ReplaySubject, Subject, takeUntil } from 'rxjs';
import { Sort } from '@angular/material/sort';
import { UnidadesNegocio } from '../../models/UnidadesNegicio';
import { FormControl } from '@angular/forms';
import { User } from '../../models/user';
import jQuery from 'jquery';
import { APP_DATE_FORMATS, AppDateAdapter } from './datepicker-format';
import { Nom035Service } from '../../services/nom035.service';

export interface DialogData {
  unidadNeg: string,
  horas: number
}

export interface Usuario {
  pos: number,
  usuario: string
}

declare global {
  interface JQuery {
    modal(action: 'show' | 'hide' | 'toggle'): JQuery;
    modal(options?: {
      backdrop?: boolean | 'static';
      keyboard?: boolean;
      focus?: boolean;
      show?: boolean;
    }): JQuery;
  }
}
declare const $: JQueryStatic;

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes.component.html',
  styleUrls: ['./reportes.component.css'],
  standalone: false,
  providers:[
    { provide: DateAdapter, useClass: AppDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class ReportesComponent implements OnInit {
  movil: any;

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.movil = Globals.movil;
    if (this.movil == true) {
      if (Globals.pagina == 2) {
        $(".mat-ink-bar").css({ display: 'none' });
      }

    }
    this.resize()
  }

  ngOnDestroy(): void {
    this._onDestroy.next();
    this._onDestroy.complete();
  }

  ngAfterViewInit() {
    this.resize();
  }

  myControl = new FormControl();
  options: User[] = [{ id: 1, username: 'ulises.mireles' }];
  filteredOptions!: Observable<User[]>;

  Usuarios: any[] = [];
  UsuariosFiltrados: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  filtroUsuario: FormControl = new FormControl();

  usuarioCtrl: FormControl = new FormControl();



  @ViewChild("usuariof", { static: false })
  Usuario: MatSelect = new MatSelect;
  _onDestroy = new Subject<void>();

  reporte: string = '';
  tipoReporte: string = '';
  usuarioCampo: boolean = false;
  proyectoCampo: boolean = false;
  fase: boolean = false;
  distribucion: boolean = false;
  detallado: boolean = false;
  persona: boolean = false;
  proyecto: boolean = false;
  semanal: boolean = false;
  ejecutivo: boolean = false;
  mostrarFiltro: boolean = false;
  vacio = [];
  unidadesNegocio: any[] = [];
  areasUnidades: any[] = [];
  areaSeleccionada = 0;
  usuarios: any[] = [];
  usuariosFiltro: any[] = [];
  proyectosFiltro: any[] = [];
  proyectosFiltroOriginal = [];
  actividadesFiltro: any[] = [];
  etapasFiltro: any[] = [];
  registrosDistribucion = [];
  clientes: any[] = [];
  proyectos = [];
  registro: any;
  registroExtra: any;
  datosReporte: any;
  datosTabla: any[] = [];
  datosEventosExtra: any[] = [];
  datosDistribucion: any[] = [];
  usuariosProyecto = [];
  eventosExtra = [];
  reporteDetallado: any[] = [];
  reportePersona: any[] = [];
  reporteProyecto: any[] = [];
  reporteProyectoActExt: any[] = [];
  reporteSemanal: any[] = [];
  reporteSemanalTotal: any[] = [];
  reporteEjecutivo: any[] = [];
  resultDetallePersona: any[] = [];
  graficaRepEjecutivo = [];
  reporteSemanalActExt: any[] = [];
  reporteProyectosProc: any[] = [];
  usuariosTotal: any[] = [];
  usuariosTotalOperaciones: any[] = [];
  usuariosTiemposMuertos: any[] = [];
  unidadesHoras: any[] = [];
  fechaAct = moment();
  //   fechaMod = moment();
  horasTotales: number = 0;
  horasTotalesEjecutivo = 0;
  horasTotalesOperaciones = 0;
  horasTotalesTiemposMuertos = 0;
  lunesRepo: Date;
  lunesRepuesto: Date;
  domingoRepo: Date;
  domingoRepuesto: Date;
  fechaInicio: Date;
  fechaFin: Date;
  //   unidadSel;
  //   areaSel;
  unidadSeleccionada: number | never = 0;
  usuarioSeleccionado = 0;
  proyectoSeleccionado = 0;
  etapaSeleccionada = 0;
  actividadSeleccionada = 0;
  UnidadUsuarioSeleccionada = 0;
  clienteSeleccionado = 0;
  unidadNombre: any;
  //   cambio = "ruta";
  sinRegistros: boolean = true;
  inicio: boolean = true;
  idUnidad = 0;
  //   idProyecto;
  actividadDesac: boolean = true;
  titulo: string = '';
  mensaje: string = '';
  idUsuario: number = 0;
  sortedData: any;
  participacion: boolean = false;
  reporteNoEncuestados: any[] = [];
  reporteEncuestados: any[] = [];
  registrosTabEncuestados: number = 0;
  registrosTabNoEncuestados: number = 0;
  cabecera: boolean = true;
  filtros: any;
  public chart: any = null;
  canvas: any;
  ctx: any;
  myChart: any;
  chartParticipaciones:any;
  chartEjecutivo: any;
  encuestadosTotales: number = 0;
  porcentajeParticipacion = 0.00;
  porcentajeNoParticipacion = 0.00;
  nombreUsuario: any;

  registrosParticipacion: number = 0;
  registrosNoParticipacion: number = 0;
  resultNom35: boolean = false;
  reporteresultNom35: any[] = [];
  registrosTabresultNom35: number = 0;

  reporteresultCategoriaEmpleado: any[] = [];
  registrosTabCategoriaEmpleado = 0;

  reporteresultDominioEmpleado: any[] = [];
  registrosTabDominioEmpleado = 0;

  reporteresultCategoriaEmpresa: any[] = [];
  registrosTabCategoriaEmpresa: number = 0;

  reporteresultDominioEmpresa: any[] = [];
  registrosTabDominioEmpresa: number = 0;

  reporteresultTotalesEmpresa: any[] = [];
  registrosTabTotalesEmpresa: number = 0;
  sinRegistrosModal: boolean = false;
  resEmpleado: any;
  resPuntos: any;
  resDesCal: any;
  encuestaPorEmpleado: boolean = false;
  //   encuestaPorEmpleadoTab;
  encuestaPorEmpresa: boolean = true;
  tituloGrafico: string = '';
  resultadosTotalesPie = 0;
  resultadosTotalesPieAmb = 0;
  resultadosTotalesPieFac = 0;
  resultadosTotalesPieOrg = 0;
  resultadosTotalesPieLid = 0;
  porcentajeNulo = 0.00;
  porcentajeBajo = 0.00;
  porcentajeMedio = 0.00;
  porcentajeAlto = 0.00;
  porcentajeMuyAlto = 0.00;
  porcentajeNuloFac = 0.00;
  porcentajeBajoFac = 0.00;
  porcentajeMedioFac = 0.00;
  porcentajeAltoFac = 0.00;
  porcentajeMuyAltoFac = 0.00;
  porcentajeNuloOrg = 0.00;
  porcentajeBajoOrg = 0.00;
  porcentajeMedioOrg = 0.00;
  porcentajeAltoOrg = 0.00;
  porcentajeMuyAltoOrg = 0.00;
  porcentajeNuloLid = 0.00;
  porcentajeBajoLid = 0.00;
  porcentajeMedioLid = 0.00;
  porcentajeAltoLid = 0.00;
  porcentajeMuyAltoLid = 0.00;
  registrosNulo: number = 0;
  registrosbajo: number = 0;
  registrosMedio: number = 0;
  registrosAlto: number = 0;
  registrosMuyAlto: number = 0;
  charTotales: boolean = true;
  charCategorias: boolean = false;
  charDominioSelect: boolean = false;
  charDominio: boolean = false;
  config: any;
  pagina: string = '';
  registrosNuloDataUno = 0;
  registrosBajoDataUno = 0;
  registrosMedioDataUno = 0;
  registrosAltoDataUno = 0;
  registrosMuyAltoDataUno = 0;
  registrosNuloDataDos = 0;
  registrosBajoDataDos = 0;
  registrosMedioDataDos = 0;
  registrosAltoDataDos = 0;
  registrosMuyAltoDataDos = 0;
  registrosNuloDataTres = 0;
  registrosBajoDataTres = 0;
  registrosMedioDataTres = 0;
  registrosAltoDataTres = 0;
  registrosMuyAltoDataTres = 0;
  registrosNuloDataCuatro = 0;
  registrosBajoDataCuatro = 0;
  registrosMedioDataCuatro = 0;
  registrosAltoDataCuatro = 0;
  registrosMuyAltoDataCuatro = 0;
  registrosNuloDataCinco = 0;
  registrosBajoDataCinco = 0;
  registrosMedioDataCinco = 0;
  registrosAltoDataCinco = 0;
  registrosMuyAltoDataCinco = 0;
  registrosNuloDataSeis = 0;
  registrosBajoDataSeis = 0;
  registrosMedioDataSeis = 0;
  registrosAltoDataSeis = 0;
  registrosMuyAltoDataSeis = 0;
  registrosNuloDataSiete = 0;
  registrosBajoDataSiete = 0;
  registrosMedioDataSiete = 0;
  registrosAltoDataSiete = 0;
  registrosMuyAltoDataSiete = 0;
  registrosNuloDataOcho = 0;
  registrosBajoDataOcho = 0;
  registrosMedioDataOcho = 0;
  registrosAltoDataOcho = 0;
  registrosMuyAltoDataOcho = 0;
  registrosNuloDataUnoDom = 0;
  registrosBajoDataUnoDom = 0;
  registrosMedioDataUnoDom = 0;
  registrosAltoDataUnoDom = 0;
  registrosMuyAltoDataUnoDom = 0;
  registrosNuloDataDosDom = 0;
  registrosBajoDataDosDom = 0;
  registrosMedioDataDosDom = 0;
  registrosAltoDataDosDom = 0;
  registrosMuyAltoDataDosDom = 0;
  registrosNuloDataTresDom = 0;
  registrosBajoDataTresDom = 0;
  registrosMedioDataTresDom = 0;
  registrosAltoDataTresDom = 0;
  registrosMuyAltoDataTresDom = 0;
  registrosNuloDataCuatroDom = 0;
  registrosBajoDataCuatroDom = 0;
  registrosMedioDataCuatroDom = 0;
  registrosAltoDataCuatroDom = 0;
  registrosMuyAltoDataCuatroDom = 0;
  dominioFiltro: any[] = [];
  resizeResultNom035: boolean = false;
  charFactores: boolean = true;
  charAmbiente: boolean = true;
  charOrganizacion: boolean = true;
  charLiderazgo: boolean = true;
  fuenteLeyendaChart: number = 0;
  //   blurLeyendaChart;
  ShadowBlurChart: number = 0;
  StrokeWidth: number = 0;
  hoverTotal: boolean = false;
  hoverCat: boolean = false;
  hoverDom: boolean = this.fase;
  regresar: boolean = false;
  valorFiltroDetalle = "";
  _id = 0;
  _email = "";
  resize() {
    var hei = window.innerHeight;
    this.filtros = $(".divFiltros").height();
    if (hei >= 985) {

      $(".tablabody").height(hei - (this.filtros + 292));

    } else if (hei > 821 && hei < 985) {

      $(".tablabody").height(hei - (this.filtros + 292));

    } else if (hei <= 821 && hei > 730) {

      $(".tablabody").height(hei - (this.filtros + 292));

    } else if (hei <= 730 && hei > 657) {

      $(".tablabody").height(hei - (this.filtros + 292));

    }
    else if (hei <= 657 && hei > 597) {
      if (!this.resultNom35) {
        $(".tablabody").height(hei - (this.filtros + 292));

      }
    } else {

      if (!this.resultNom35) {
        $(".tablabody").height(hei - (this.filtros + 292));
      } else {
        this.resizeResultNom035 = true;
      }

    }
  }

  @ViewChild('charCategoria', { static: false }) canvasCatRef!: ElementRef;
  @ViewChild('charDominio', { static: false }) canvasDomRef!: ElementRef;
  constructor(public dialog: MatDialog, private spinner: NgxSpinnerService, private toastr: ToastrService, private authenticationService: AuthenticationService,
    private cdRef: ChangeDetectorRef, private activatedRoute: ActivatedRoute, private router: Router, private http: HttpClient,
    private serviceReportes: ReportesService, private descargaService: DescargaService, private nom35Service: Nom035Service) {

      document.addEventListener('hide.bs.modal', () => {
        if (document.activeElement) {
          (document.activeElement as HTMLElement).blur();
        }
      });

      Chart.register(...registerables);
      Chart.register(ChartDataLabels);

    const params = this.activatedRoute.snapshot.params;
    this.config = {
      itemsPerPage: 10,
      currentPage: 1,
      totalItems: 0,
      totalPages: 0
    }
    if (params['reporte']) {

      if (params['reporte'] == 'distribucion') {
        this.distribucion = true;
        this.usuarioCampo = false;
        this.proyectoCampo = false;
        this.fase = false;
        this.encuestaPorEmpresa = false;
        this.encuestaPorEmpleado = false;
        this.resultNom35 = false;
        this.tipoReporte = 'de Distribución'
      }
      if (params['reporte'] == 'detallado') {
        this.detallado = true;
        this.usuarioCampo = true;
        this.proyectoCampo = true;
        this.fase = true;
        this.tipoReporte = 'Detallado';
        this.encuestaPorEmpresa = false;
        this.encuestaPorEmpleado = false;
        this.resultNom35 = false;
      }
      if (params['reporte'] == 'persona') {
        this.persona = true;
        this.usuarioCampo = false;
        this.proyectoCampo = false;
        this.fase = false;
        this.tipoReporte = 'por Persona';
        this.encuestaPorEmpresa = false;
        this.encuestaPorEmpleado = false;
        this.resultNom35 = false;
      }
      if (params['reporte'] == 'proyecto') {
        this.proyecto = true;
        this.usuarioCampo = false;
        this.proyectoCampo = false;
        this.fase = false;
        this.tipoReporte = 'por Proyecto';
        this.encuestaPorEmpresa = false;
        this.encuestaPorEmpleado = false;
        this.resultNom35 = false;
      }
      if (params['reporte'] == 'proyectoSemanal') {
        this.proyecto = true;
        this.usuarioCampo = false;
        this.proyectoCampo = false;
        this.fase = false;
        this.tipoReporte = 'por Proyecto Semanal';
        this.encuestaPorEmpresa = false;
        this.encuestaPorEmpleado = false;
        this.resultNom35 = false;
      }
      if (params['reporte'] == 'semanal') {
        this.semanal = true;
        this.usuarioCampo = true;
        this.proyectoCampo = false;
        this.fase = false;
        this.tipoReporte = 'Semanal';
        this.encuestaPorEmpresa = false;
        this.encuestaPorEmpleado = false;
        this.resultNom35 = false;
      }
      if (params['reporte'] == 'participacion') {
        this.participacion = true;
        this.usuarioCampo = false;
        this.proyectoCampo = false;
        this.cabecera = false;
        this.fase = false;
        this.tipoReporte = 'participación';
        this.getParticipantes();
        this.encuestaPorEmpresa = false;
        this.encuestaPorEmpleado = false;
        this.resultNom35 = false;
        // this.getEncuestados();
        //this.getNoEncuestados();
      }
      if (params['reporte'] == 'resultNom35') {
        this.resultNom35 = true;
        this.usuarioCampo = false;
        this.proyectoCampo = false;
        this.cabecera = false;
        this.fase = false;
        this.tipoReporte = 'resultados NOM-035-STPS-2018';
        this.getResultadoEncuesta();
        // this.getEncuestados();
        //this.getNoEncuestados();
      }
      if (params['reporte'] == 'ejecutivo') {
        this.resultNom35 = false;
        this.usuarioCampo = false;
        this.proyectoCampo = false;
        this.cabecera = true;
        this.fase = false;
        this.ejecutivo = true;
        this.tipoReporte = 'Ejecutivo';
        this.getReporteEjecutivo();
        // this.getEncuestados();
        //this.getNoEncuestados();
      }

    }

    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var fechaCalculo = this.fechaAct;
    for (let index = 0; index < 6; index++) {
      var diaSemana = new Date(fechaCalculo.format('LLLL'));
      var day = days[diaSemana.getDay()];
      if (day !== 'Monday') {
        fechaCalculo = fechaCalculo.subtract(1, 'd');

      }
      else {
        var lunesMom = fechaCalculo;
      }

    }
    fechaCalculo = fechaCalculo.subtract(7, 'd');
    lunesMom = fechaCalculo;
    var lunes = new Date(new Date(lunesMom.format('LLLL')).setHours(0, 0, 0, 0));
    var domingo = new Date(lunesMom.format('LLLL'));
    var domingoMom = moment(lunes).add(6, 'd');
    //domingoMom = domingoMom.add(7,'d');
    domingo = new Date(new Date(domingoMom.format('LLLL')).setHours(0, 0, 0, 0));
    this.lunesRepo = lunes;
    this.lunesRepuesto = lunes;
    this.domingoRepo = domingo;
    this.domingoRepuesto = domingo;
    this.fechaInicio = lunes;
    this.fechaFin = domingo;


  }

  getfiltroDetalle(valor: string) {
    this.valorFiltroDetalle = valor; // Guardar el valor ingresado en la variable

  }

  abrirSelect(select: string, event: KeyboardEvent): void {
    if (event.key === 'Tab') {
      if (select === 'usuario') {
        this.Usuario.open();
      }
    }
  }
  sortData(sort: Sort) {
    //console.log(this.datosDistribucion, this.sortedData)

    if (sort.direction !== 'asc' && sort.direction !== 'desc') {
      if (this.distribucion) { this.datosDistribucion = this.sortedData.map((x: any) => Object.assign({}, x)); }
      else if (this.detallado) { this.reporteDetallado = this.sortedData.map((x: any) => Object.assign({}, x)); }
      else if (this.persona) { this.reportePersona = this.sortedData.map((x: any) => Object.assign({}, x)); }
      else if (this.proyecto) { this.reporteProyecto = this.sortedData.map((x: any) => Object.assign({}, x)); }
      else if (this.semanal) { this.reporteSemanalTotal = this.sortedData.map((x: any) => Object.assign({}, x)); }

    }
    //sort reporte distribucion
    if (sort.active == 'clienteDistribucionSort') {
      if (sort.direction === 'asc') {
        this.datosDistribucion = this.datosDistribucion.sort((a: any, b: any) => (a.cliente > b.cliente) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.datosDistribucion = this.datosDistribucion.sort((a: any, b: any) => (a.cliente < b.cliente) ? 1 : -1)
      }
    }
    else if (sort.active == 'nombreDistribucionSort') {
      if (sort.direction === 'asc') {
        this.datosDistribucion = this.datosDistribucion.sort((a: any, b: any) => (a.proyecto > b.proyecto) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.datosDistribucion = this.datosDistribucion.sort((a: any, b: any) => (a.proyecto < b.proyecto) ? 1 : -1)
      }
    }
    else if (sort.active == 'unidadDistribucionSort') {
      if (sort.direction === 'asc') {
        this.datosDistribucion = this.datosDistribucion.sort((a: any, b: any) => (a.unidad > b.unidad) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.datosDistribucion = this.datosDistribucion.sort((a: any, b: any) => (a.unidad < b.unidad) ? 1 : -1)
      }
    }
    else if (sort.active == 'areaDistribucionSort') {
      if (sort.direction === 'asc') {
        this.datosDistribucion = this.datosDistribucion.sort((a: any, b: any) => (a.area > b.area) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.datosDistribucion = this.datosDistribucion.sort((a: any, b: any) => (a.area < b.area) ? 1 : -1)
      }
    }
    else if (sort.active == 'actividadDistribucionSort') {
      if (sort.direction === 'asc') {
        this.datosDistribucion = this.datosDistribucion.sort((a: any, b: any) => (a.actividad > b.actividad) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.datosDistribucion = this.datosDistribucion.sort((a: any, b: any) => (a.actividad < b.actividad) ? 1 : -1)
      }
    }
    //sort proyecto detallado
    if (sort.active == 'fechaDetalladoSort') {
      if (sort.direction === 'asc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (new Date(a.fecha) > new Date(b.fecha) ? 1 : -1))
      }
      else if (sort.direction === 'desc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (new Date(a.fecha) < new Date(b.fecha) ? 1 : -1))
      }
    }
    else if (sort.active == 'duracionDetalladoSort') {
      if (sort.direction === 'asc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (a.horas > b.horas) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (a.horas < b.horas) ? 1 : -1)
      }
    }
    else if (sort.active == 'usuarioDetalladoSort') {
      if (sort.direction === 'asc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (a.usuario > b.usuario) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (a.usuario < b.usuario) ? 1 : -1)
      }
    }
    else if (sort.active == 'unidadDetalladoSort') {
      if (sort.direction === 'asc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (a.unidad > b.unidad) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (a.unidad < b.unidad) ? 1 : -1)
      }
    }
    else if (sort.active == 'areaDetalladoSort') {
      if (sort.direction === 'asc') {

        this.reporteDetallado = this.reporteDetallado.sort(function (a: any, b: any) {
          var va = (a.area === null) ? "" : "" + a.area,
            vb = (b.area === null) ? "" : "" + b.area;
          return va > vb ? 1 : (va === vb ? 0 : -1);
        });
      }
      else if (sort.direction === 'desc') {
        this.reporteDetallado = this.reporteDetallado.sort(function (a: any, b: any) {
          var va = (a.area === null) ? "" : "" + a.area,
            vb = (b.area === null) ? "" : "" + b.area;
          return va < vb ? 1 : (va === vb ? 0 : -1);
        });
      }
    }
    else if (sort.active == 'proyectoDetalladoSort') {
      if (sort.direction === 'asc') {
        this.reporteDetallado = this.reporteDetallado.sort(function (a: any, b: any) {
          var va = (a.proyecto === null) ? "" : "" + a.proyecto,
            vb = (b.proyecto === null) ? "" : "" + b.proyecto;
          return va > vb ? 1 : (va === vb ? 0 : -1);
        });
      }
      else if (sort.direction === 'desc') {
        this.reporteDetallado = this.reporteDetallado.sort(function (a: any, b: any) {
          var va = (a.proyecto === null) ? "" : "" + a.proyecto,
            vb = (b.proyecto === null) ? "" : "" + b.proyecto;
          return va < vb ? 1 : (va === vb ? 0 : -1);
        });
      }
    }
    else if (sort.active == 'actividadDetalladoSort') {
      if (sort.direction === 'asc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (a.actividad > b.actividad) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (a.actividad < b.actividad) ? 1 : -1)
      }
    }
    else if (sort.active == 'detalleDetalladoSort') {
      if (sort.direction === 'asc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (a.detalle > b.detalle) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (a.detalle < b.detalle) ? 1 : -1)
      }
    }
    else if (sort.active == 'etapaDetalladoSort') {
      if (sort.direction === 'asc') {
        this.reporteDetallado = this.reporteDetallado.sort(function (a: any, b: any) {
          var va = (a.etapa === null) ? "" : "" + a.etapa,
            vb = (b.etapa === null) ? "" : "" + b.etapa;
          return va > vb ? 1 : (va === vb ? 0 : -1);
        });
      }
      else if (sort.direction === 'desc') {
        this.reporteDetallado = this.reporteDetallado.sort(function (a: any, b: any) {
          var va = (a.etapa === null) ? "" : "" + a.etapa,
            vb = (b.etapa === null) ? "" : "" + b.etapa;
          return va < vb ? 1 : (va === vb ? 0 : -1);
        });
      }
    }
    else if (sort.active == 'fechaRegDetalladoSort') {
      if (sort.direction === 'asc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (new Date(a.fechaRegistro) > new Date(b.fechaRegistro)) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteDetallado = this.reporteDetallado.sort((a: any, b: any) => (new Date(a.fechaRegistro) < new Date(b.fechaRegistro)) ? 1 : -1)
      }
    }
    //sort proyecto persona
    if (sort.active == 'usuarioPersonaSort') {
      if (sort.direction === 'asc') {
        this.reportePersona = this.reportePersona.sort((a: any, b: any) => a.usuario.toLowerCase() > b.usuario.toLowerCase() ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reportePersona = this.reportePersona.sort((a: any, b: any) => a.usuario.toLowerCase() < b.usuario.toLowerCase() ? 1 : -1)
      }
    }
    else if (sort.active == 'unidadPersonaSort') {
      if (sort.direction === 'asc') {
        this.reportePersona = this.reportePersona.sort((a: any, b: any) => (a.unidad > b.unidad) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reportePersona = this.reportePersona.sort((a: any, b: any) => (a.unidad < b.unidad) ? 1 : -1)
      }
    }
    else if (sort.active == 'horasPersonaSort') {
      if (sort.direction === 'asc') {
        this.reportePersona = this.reportePersona.sort((a: any, b: any) => (a.horas > b.horas) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reportePersona = this.reportePersona.sort((a: any, b: any) => (a.horas < b.horas) ? 1 : -1)
      }
    }
    //sort proyecto proyecto
    if (sort.active == 'clienteProyectoSort') {
      if (sort.direction === 'asc') {
        this.reporteProyecto = this.reporteProyecto.sort(function (a: any, b: any) {
          var va = (a.cliente === null) ? "" : "" + a.cliente,
            vb = (b.cliente === null) ? "" : "" + b.cliente;
          return va > vb ? 1 : (va === vb ? 0 : -1);
        });
      }
      else if (sort.direction === 'desc') {
        this.reporteProyecto = this.reporteProyecto.sort(function (a: any, b: any) {
          var va = (a.cliente === null) ? "" : "" + a.cliente,
            vb = (b.cliente === null) ? "" : "" + b.cliente;
          return va < vb ? 1 : (va === vb ? 0 : -1);
        });
      }
    }
    else if (sort.active == 'proyectoProyectoSort') {
      if (sort.direction === 'asc') {
        this.reporteProyecto = this.reporteProyecto.sort(function (a: any, b: any) {
          var va = (a.proyecto === null) ? "" : "" + a.proyecto,
            vb = (b.proyecto === null) ? "" : "" + b.proyecto;
          return va > vb ? 1 : (va === vb ? 0 : -1);
        });
      }
      else if (sort.direction === 'desc') {
        this.reporteProyecto = this.reporteProyecto.sort(function (a: any, b: any) {
          var va = (a.proyecto === null) ? "" : "" + a.proyecto,
            vb = (b.proyecto === null) ? "" : "" + b.proyecto;
          return va < vb ? 1 : (va === vb ? 0 : -1);
        });
      }
    }
    else if (sort.active == 'unidadProyectoSort') {
      if (sort.direction === 'asc') {
        this.reporteProyecto = this.reporteProyecto.sort((a: any, b: any) => (a.unidad > b.unidad) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteProyecto = this.reporteProyecto.sort((a: any, b: any) => (a.unidad < b.unidad) ? 1 : -1)
      }
    }
    else if (sort.active == 'areaProyectoSort') {
      if (sort.direction === 'asc') {
        this.reporteProyecto = this.reporteProyecto.sort(function (a: any, b: any) {
          var va = (a.area === null) ? "" : "" + a.area,
            vb = (b.area === null) ? "" : "" + b.area;
          return va > vb ? 1 : (va === vb ? 0 : -1);
        });
      }
      else if (sort.direction === 'desc') {
        this.reporteProyecto = this.reporteProyecto.sort(function (a: any, b: any) {
          var va = (a.area === null) ? "" : "" + a.area,
            vb = (b.area === null) ? "" : "" + b.area;
          return va < vb ? 1 : (va === vb ? 0 : -1);
        });
      }
    }
    else if (sort.active == 'actividadProyectoSort') {
      if (sort.direction === 'asc') {
        this.reporteProyecto = this.reporteProyecto.sort(function (a: any, b: any) {
          var va = (a.actividad === null) ? "" : "" + a.actividad,
            vb = (b.actividad === null) ? "" : "" + b.actividad;
          return va > vb ? 1 : (va === vb ? 0 : -1);
        });
      }
      else if (sort.direction === 'desc') {
        this.reporteProyecto = this.reporteProyecto.sort(function (a: any, b: any) {
          var va = (a.actividad === null) ? "" : "" + a.actividad,
            vb = (b.actividad === null) ? "" : "" + b.actividad;
          return va < vb ? 1 : (va === vb ? 0 : -1);
        });
      }
    }
    else if (sort.active == 'horasProyectoSort') {
      if (sort.direction === 'asc') {
        this.reporteProyecto = this.reporteProyecto.sort((a: any, b: any) => (a.horas > b.horas) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteProyecto = this.reporteProyecto.sort((a: any, b: any) => (a.horas < b.horas) ? 1 : -1)
      }
    }
    //sort proyecto semanal
    if (sort.active == 'usuarioSemanalSort') {
      if (sort.direction === 'asc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort((a: any, b: any) => (a.usuario > b.usuario) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort((a: any, b: any) => (a.usuario < b.usuario) ? 1 : -1)
      }
    }
    else if (sort.active == 'proyectoSemanalSort') {
      if (sort.direction === 'asc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort(function (a: any, b: any) {
          var va = (a.proyecto === null) ? "" : "" + a.proyecto,
            vb = (b.proyecto === null) ? "" : "" + b.proyecto;
          return va > vb ? 1 : (va === vb ? 0 : -1);
        });
      }
      else if (sort.direction === 'desc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort(function (a: any, b: any) {
          var va = (a.proyecto === null) ? "" : "" + a.proyecto,
            vb = (b.proyecto === null) ? "" : "" + b.proyecto;
          return va < vb ? 1 : (va === vb ? 0 : -1);
        });
      }
    }
    else if (sort.active == 'unidadSemanalSort') {
      if (sort.direction === 'asc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort((a: any, b: any) => (a.unidad > b.unidad) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort((a: any, b: any) => (a.unidad < b.unidad) ? 1 : -1)
      }
    }
    else if (sort.active == 'areaSemanalSort') {
      if (sort.direction === 'asc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort(function (a: any, b: any) {
          var va = (a.area === null) ? "" : "" + a.area,
            vb = (b.area === null) ? "" : "" + b.area;
          return va > vb ? 1 : (va === vb ? 0 : -1);
        });
      }
      else if (sort.direction === 'desc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort(function (a: any, b: any) {
          var va = (a.area === null) ? "" : "" + a.area,
            vb = (b.area === null) ? "" : "" + b.area;
          return va < vb ? 1 : (va === vb ? 0 : -1);
        });
      }
    }
    else if (sort.active == 'actividadSemanalSort') {
      if (sort.direction === 'asc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort(function (a: any, b: any) {
          var va = (a.actividad === null) ? "" : "" + a.actividad,
            vb = (b.actividad === null) ? "" : "" + b.actividad;
          return va > vb ? 1 : (va === vb ? 0 : -1);
        });
      }
      else if (sort.direction === 'desc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort(function (a: any, b: any) {
          var va = (a.actividad === null) ? "" : "" + a.actividad,
            vb = (b.actividad === null) ? "" : "" + b.actividad;
          return va < vb ? 1 : (va === vb ? 0 : -1);
        });
      }
    }
    else if (sort.active == 'horasSemanalSort') {
      if (sort.direction === 'asc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort((a: any, b: any) => (a.horas > b.horas) ? 1 : -1)
      }
      else if (sort.direction === 'desc') {
        this.reporteSemanalTotal = this.reporteSemanalTotal.sort((a: any, b: any) => (a.horas < b.horas) ? 1 : -1)
      }
    }
    ///fin de sort
  }

  ngAfterViewChecked() {
    if (!localStorage.getItem('currentUser')) {
      this.authenticationService.verificarSesion();
    }
    this.cdRef.detectChanges();
  }

  ngOnInit() {
    this.mostrarFiltro = true;
    $("#page-container").css("background-color", "#EAEAEA");
    this.unidadSeleccionada = -1;
    this.clienteSeleccionado = -1;
    this.idUsuario = Number(localStorage.getItem('currentUser'));

    Globals.pagina = 2;
    this.getUnidades();
    this.getAreas();
    var promise = new Promise<void>((res, rej) => {
      this.resize();
      res();
    }).then(
      () => {
        setTimeout(() => {
          if (this.distribucion) {
            this.sinRegistros = true;
            this.getClientesDistibucion();
            this.getReporteDistribucion();

          }
          if (this.detallado) {
            this.sinRegistros = true;
            $('#etapaSel').prop('disabled', true);
            this.getReporteDetallado();

          }
          if (this.persona) {
            this.getReportePersona();
            this.sinRegistros = true;
            this.recuperarUsuarios();
            this.filtroUsuario.valueChanges.pipe(takeUntil(this._onDestroy))
              .subscribe(() => {
                this.filtraUsuarios();
              })

          }
          if (this.proyecto) {
            this.sinRegistros = true;
            this.getClientesPorProyecto();
            this.getReporteProyectos();
          }
          if (this.semanal) {
            this.sinRegistros = true;
            this.getReporteSemanal();
          }
          if (this.ejecutivo) {
            this.sinRegistros = true;
            this.getReporteEjecutivo();
          }

          this.pageChanged(1);
        }, 1000);

        if (this.participacion) {
          this.sinRegistros = true;
          this.cabecera = false;

        }
        if (this.resultNom35) {
          // this.sinRegistros = true;
          //  this.cabecera = false;
        }
      });
    return promise;
  }


  displayFn(user: User): string {
    return user && user.username ? user.username : '';
  }


  private _filter(name: string): User[] {
    const filterValue = name.toLowerCase();
    return this.options.filter(option => option.username.toLowerCase().indexOf(filterValue) === 0);
  }


  getUnidades() {
    this.serviceReportes.getUnidades(Number(localStorage.getItem('currentUser'))).subscribe(res => {
      this.unidadesNegocio = res.avanceR;
      //console.log(this.unidadesNegocio)
    }, err => {//console.log(err)
    });

  }
  getClientesDistibucion() {
    this.clientes = [];
    var idUsuario = parseInt(localStorage.getItem('currentUser')!);
    this.datosReporte = { idUser: idUsuario, idUnidad: this.unidadSeleccionada, idArea: this.areaSeleccionada, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo, IdUnidadUsuario: this.UnidadUsuarioSeleccionada };
    //var datos = {idUser:idUsuario, idUnidad:this.unidad.toString(), idArea:this.areaSeleccionada.toString()};
    var datos = { datos: this.datosReporte }
    this.serviceReportes.getConsultaClientesDistribucion(this.datosReporte).subscribe(
      res => {
        this.clientes = res.clientes;
        //console.log(this.unidadesNegocio)
      }, err => {//console.log(err)
      });
  }

  getClientesPorProyecto() {
    this.clientes = [];
    var idUsuario = parseInt(localStorage.getItem('currentUser')!);
    this.datosReporte = { idUser: idUsuario, idUnidad: this.unidadSeleccionada, idArea: this.areaSeleccionada, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo };
    //var datos = {idUser:idUsuario, idUnidad:this.unidad.toString(), idArea:this.areaSeleccionada.toString()};
    var datos = { datos: this.datosReporte }
    this.serviceReportes.getConsultaClientesPorProyecto(this.datosReporte).subscribe(
      res => {
        this.clientes = res.clientes;
        //console.log(this.unidadesNegocio)
      }, err => {//console.log(err)
      });

  }
  getAreas() {
    this.serviceReportes.getAreas(Number(localStorage.getItem('currentUser'))).subscribe(result => {
      this.areasUnidades = result.avanceR;
      //console.log(this.areasUnidades)
    }, err => { }//console.log(err)
    )
  }

  getAreasConUnidad(idUnidad: number) {
    this.serviceReportes.getAreasConUnidad(Number(localStorage.getItem('currentUser')), idUnidad).subscribe(result => {
      this.areasUnidades = result.avanceR;
    }, err => { }//console.log(err)
    );
  }

  getUnidadConAreas(idArea: number) {
    this.serviceReportes.getUnidadConAreas(Number(localStorage.getItem('currentUser')), idArea).subscribe(result => {
      this.unidadesNegocio = result.avanceR
    })
  }
  getDatosReporteSize(size: any) {
    this.config.itemsPerPage = size;
    this.config.currentPage = 1;
    this.pageChanged(1);

  }
  getDatosReporte(reset = true) {
    this.lunesRepo = this.fechaInicio;
    this.domingoRepo = this.fechaFin;


    if (this.distribucion) {

      if (this.lunesRepo <= this.domingoRepo) {
        this.getClientesDistibucion();
        this.getReporteDistribucion();
      }
      else {
        this.titulo = "Advertencia";
        this.mensaje = "Seleccione fechas válidas"
        this.toastr.warning(this.mensaje, this.titulo);

      }
    }
    else if (this.detallado) {

      if (this.lunesRepo <= this.domingoRepo) {
        if (reset) this.config.currentPage = 1;
        this.getReporteDetallado();
      }
      else {
        this.titulo = "Advertencia";
        this.mensaje = "Seleccione fechas válidas"
        this.toastr.warning(this.mensaje, this.titulo);

      }
    }
    else if (this.persona) {
      if (this.lunesRepo <= this.domingoRepo) {
        this.getReportePersona();
      }
      else {
        this.titulo = "Advertencia";
        this.mensaje = "Seleccione fechas válidas"
        this.toastr.warning(this.mensaje, this.titulo);

      }
    }
    else if (this.proyecto) {
      if (this.lunesRepo <= this.domingoRepo) {
        if (reset) {
          this.getClientesPorProyecto();
          this.getReporteProyectos();
        }
      }
      else {
        this.titulo = "Advertencia";
        this.mensaje = "Seleccione fechas válidas"
        this.toastr.warning(this.mensaje, this.titulo);

      }
    }
    else if (this.semanal) {
      if (this.lunesRepo <= this.domingoRepo) {
        this.getReporteSemanal();
      } else if (this.participacion) {
        this.cabecera = false;

      }
      else {
        this.titulo = "Advertencia";
        this.mensaje = "Seleccione fechas válidas"
        this.toastr.warning(this.mensaje, this.titulo);

      }

    }
    else if (this.ejecutivo) {
      if (this.lunesRepo <= this.domingoRepo) {
        this.getReporteEjecutivo();
      } else if (this.participacion) {
        this.cabecera = false;

      }
      else {
        this.titulo = "Advertencia";
        this.mensaje = "Seleccione fechas válidas"
        this.toastr.warning(this.mensaje, this.titulo);

      }

    }

  }

  limpiar() {
    // this.fechaInicio = null;
    // this.fechaFin = null;
    this.areaSeleccionada = 0;
    this.unidadSeleccionada = 0;
    this.clienteSeleccionado = 0;
    this.usuarioSeleccionado = 0;
    this.proyectoSeleccionado = 0;
    this.etapaSeleccionada = 0;
    this.actividadSeleccionada = 0;
    this.config.currentPage = 1;
    this.config.totalItems = 0;
    this.valorFiltroDetalle = ""; 

    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var fechaCalculo = moment();
    for (let index = 0; index < 6; index++) {
      var diaSemana = new Date(fechaCalculo.format('LLLL'));
      var day = days[diaSemana.getDay()];
      if (day !== 'Monday') {
        fechaCalculo = fechaCalculo.subtract(1, 'd');

      }
      else {
        var lunesMom = fechaCalculo;
      }

    }
    fechaCalculo = fechaCalculo.subtract(7, 'd');
    lunesMom = fechaCalculo;
    var lunes = new Date(new Date(lunesMom.format('LLLL')).setHours(0, 0, 0, 0));
    var domingo = new Date(lunesMom.format('LLLL'));
    var domingoMom = moment(lunes).add(6, 'd');
    //domingoMom = domingoMom.add(7,'d');
    domingo = new Date(new Date(domingoMom.format('LLLL')).setHours(0, 0, 0, 0));
    this.lunesRepo = this.lunesRepuesto;
    this.domingoRepo = this.domingoRepuesto;
    this.fechaInicio = lunes;
    this.fechaFin = domingo;
    //this.getEncuestados();
    //this.getNoEncuestados();
    setTimeout(() => {
      this.selUnidad(-1)
      this.getAreas();
      this.getUnidades();
      this.getUsuarios(-1);
      this.getUsuariosSemanal(-1);
      this.getUsuariosPersonas(-1);
      this.getProyectos(-1);
      this.getActividades(-1);
      this.getEtapas(-1);
    }, 500);

    setTimeout(() => {
      if (this.tipoReporte == 'de Distribución') {
        this.getReporteDistribucion();
      }
      else if (this.tipoReporte == 'Detallado') {
        this.getReporteDetallado();
      }
      else if (this.tipoReporte == 'por Persona') {
        this.getReportePersona();
        this.recuperarUsuarios();
      }
      else if (this.tipoReporte == 'por Proyecto') {
        this.getReporteProyectos();
      }
      else if (this.tipoReporte == 'por Proyecto Semanal') {
        this.getReporteProyectos();
      }
      else if (this.tipoReporte == 'Semanal') {
        this.getReporteSemanal();
      }
      else if (this.tipoReporte == 'Ejecutivo') {
        this.getReporteEjecutivo();
      }
      else if (this.tipoReporte == 'participación') {
        this.cabecera = false;

      }
    }, 1000);

  }
  filtraUsuarios() {
    if (!this.usuarios) {
      return;
    }
    let busqueda = this.filtroUsuario.value.normalize('NFD')
      .replace(/([aeio])\u0301|(u)[\u0301\u0308]/gi, "$1$2")
      .normalize();
    if (!busqueda) {
      this.UsuariosFiltrados.next(this.usuarios.slice());
      return;
    }
    else {
      busqueda = busqueda.toLowerCase();
    }
    this.UsuariosFiltrados.next(
      this.usuarios.filter(u => this.acentos(u.usuario.toLowerCase()).indexOf(busqueda) > -1)
    );
  }

  acentos(text: string) {
    text = text.normalize('NFD')
      .replace(/([aeio])\u0301|(u)[\u0301\u0308]/gi, "$1$2")
      .normalize();
    return text.toString();
  }

  getReporteDetallado() {
    this.reporteDetallado = [];
    this.spinner.show();
    var idUsuario = parseInt(localStorage.getItem('currentUser')!);

    this.datosReporte = {
      idUser: idUsuario, idUnidad: this.unidadSeleccionada, idArea: this.areaSeleccionada,
      idProyecto: this.proyectoSeleccionado, idEtapa: this.etapaSeleccionada, idUserFiltro: this.usuarioSeleccionado,
      fechaIni: this.lunesRepo, fechaFin: this.domingoRepo, idActividad: this.actividadSeleccionada, idUnidadusuario: this.UnidadUsuarioSeleccionada,
      varDetalle: this.valorFiltroDetalle
    };
    console.log(this.datosReporte)
    this.serviceReportes.getConsultaDetallado(this.datosReporte,this.config.itemsPerPage, this.config.currentPage).subscribe(
      res => {
        //console.log(res)
        this.reporteDetallado = res.detallado

        if (res.detallado.length > 0) {
          var fecha;
          var fechaRegistro;
          for (let index = 0; index < res.detallado.length; index++) {
            fecha = new Date(res.detallado[index].fecha);
            fechaRegistro = new Date(res.detallado[index].fechaRegistro);

            var mes = parseInt(moment(fecha).format('M'));
            var dia = parseInt(moment(fecha).format('D'));
            var año = (moment(fecha).year());
            var fechaFormat = dia + "/" + (mes) + "/" + año;
            res.detallado[index].fecha = fechaFormat;

            var mesReg = parseInt(moment(fechaRegistro).format('M'));
            var diaReg = parseInt(moment(fechaRegistro).format('D'));
            var añoReg = (moment(fechaRegistro).year());
            var fechaFormatReg = diaReg + "/" + (mesReg) + "/" + añoReg;
            res.detallado[index].fechaRegistro = fechaFormatReg;
          }

          this.config.totalPages = res.pageList.totalPages;
          this.config.totalItems = res.pageList.totalItems;

          this.getUsuarios(this.unidadSeleccionada);
          this.getProyectos(this.unidadSeleccionada);
          this.getActividades(this.unidadSeleccionada);
          this.getEtapas(this.unidadSeleccionada);

          this.sinRegistros = false;
          if (this.inicio) {
            this.unidadSeleccionada = -1
          }
        } else {
          this.sinRegistros = true;
        }
        this.sortedData = this.reporteDetallado.map(x => Object.assign({}, x));
        this.inicio = false;
        this.spinner.hide();
      }, err => {
        this.spinner.hide();
        //console.log(err)
      }
    );

  }
  getReportePersona() {
    //this.spinner.show();
    this.reportePersona = [];
    var idUsuario = parseInt(localStorage.getItem('currentUser')!);
    this.datosReporte = { idUser: idUsuario, idUnidad: this.unidadSeleccionada, idArea: this.areaSeleccionada, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo, IdUserFiltro: this.usuarioSeleccionado };

    this.serviceReportes.getConsultaPersonas(this.datosReporte).subscribe(
      res => {
        this.reportePersona = res.personas;
        //console.log(res)
        if (this.inicio) {
          this.recuperarUsuarios();
          if (res.personas.length > 0) {

            this.unidadNombre = res.personas[0].unidad;
            if (this.unidadNombre == undefined) {
              this.unidadSeleccionada = -1;
            }
            else {
              var index = this.unidadesNegocio.indexOf(this.unidadesNegocio.find((x: any) => this.unidadNombre == x.nombre)!);
              if (index != -1) {
                this.unidadSeleccionada = this.unidadesNegocio[index].id;
                this.idUnidad = this.unidadesNegocio[index].id;
              }
              else {
                // this.unidadSeleccionada = -1;
              }
            }
          }
        }
        if (res.personas.length > 0) {
          this.sinRegistros = false;
          if (this.inicio) {
            this.unidadSeleccionada = -1;
          }
        } else {
          this.sinRegistros = true;
          //this.unidadSeleccionada = -1;
        }
        this.sortedData = this.reportePersona.map(x => Object.assign({}, x));
        this.inicio = false;
        this.spinner.hide();
      }, err => {
        this.spinner.hide();
        //console.log(err)
      })
  }

  getReportePersonaTodos() {
    //this.spinner.show();
    this.reportePersona = [];
    var idUsuario = parseInt(localStorage.getItem('currentUser')!);
    this.datosReporte = { idUser: idUsuario, idUnidad: this.unidadSeleccionada, idArea: this.areaSeleccionada, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo, IdUserFiltro: 0 };

    this.serviceReportes.getConsultaPersonas(this.datosReporte).subscribe(
      res => {
        this.reportePersona = res.personas;
        //console.log(res)
        if (this.inicio) {

          this.getUsuariosPersonas(-1);
          if (res.personas.length > 0) {

            this.unidadNombre = res.personas[0].unidad;
            if (this.unidadNombre == undefined) {
              this.unidadSeleccionada = -1;
            }
            else {
              var index = this.unidadesNegocio.indexOf(this.unidadesNegocio.find(x => this.unidadNombre == x.nombre));
              if (index != -1) {
                this.unidadSeleccionada = this.unidadesNegocio[index].id;
                this.idUnidad = this.unidadesNegocio[index].id;
              }
              else {
                // this.unidadSeleccionada = -1;
              }
            }

          }


        }
        if (res.personas.length > 0) {
          this.sinRegistros = false;
          if (this.inicio) {
            this.unidadSeleccionada = -1;
          }
        } else {
          this.sinRegistros = true;
          //this.unidadSeleccionada = -1;
        }
        this.sortedData = this.reportePersona.map(x => Object.assign({}, x));
        this.inicio = false;
        this.spinner.hide();
      }, err => {
        this.spinner.hide();
        //console.log(err)
      })
  }

  getReporteDistribucion() {
    console
    this.spinner.show();
    this.datosDistribucion = [];
    var idUsuario = parseInt(localStorage.getItem('currentUser')!);
    this.datosReporte = { idUser: idUsuario, idUnidad: this.unidadSeleccionada, idArea: this.areaSeleccionada, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo, IdUnidadUsuario: this.UnidadUsuarioSeleccionada, IdCliente: this.clienteSeleccionado };
    //var datos = {idUser:idUsuario, idUnidad:this.unidad.toString(), idArea:this.areaSeleccionada.toString()};
    var datos = { datos: this.datosReporte }
    this.serviceReportes.getConsultaDistribucion(this.datosReporte).subscribe(
      res => {
        //console.log(res)
        
        this.datosTabla = [];
        this.datosEventosExtra = [];
        this.usuarios = [];
        this.eventosExtra = [];
        for (let index = 0; index < res.usuarios.length; index++) {
          let datos = { pos: index, usuario: res.usuarios[index] }
          this.usuarios.push(datos)
        }
        this.registrosDistribucion = res.distribucion;
        this.eventosExtra = res.actividadesExt;
        if (res.distribucion.length > 0) {

          if (this.inicio) {
            if (res.distribucion.length > 0) {
              this.unidadNombre = res.distribucion[0].unidad;
              if (this.unidadNombre == undefined || this.unidadNombre == null) {
                this.unidadSeleccionada = -1;
              }
              else {
                var index = this.unidadesNegocio.indexOf(this.unidadesNegocio.find((x: any) => this.unidadNombre == x.nombre)!);
                if (index == -1) {
                  //this.unidadSeleccionada = -1;
                } else {
                  this.unidadSeleccionada = this.unidadesNegocio[index].id;
                  this.idUnidad = this.unidadesNegocio[index].id;
                }
              }
            }
            else {
              // this.unidadSeleccionada = -1;
            }
          }
          this.registro = res.distribucion[0];
        }
        if (res.actividadesExt.length > 0) {
          this.registroExtra = res.actividadesExt[0];
        }

        let users = this.usuarios.map(x => Object.assign({}, x));

        var cantRegs = res.distribucion.length + res.actividadesExt.length;
        if (cantRegs > 0) {
          this.sinRegistros = false;
          if (this.inicio) {
            this.unidadSeleccionada = -1;
          }

        }
        else {
          this.sinRegistros = true;
          //this.unidadSeleccionada = -1;
        }
        users = this.usuarios.map(x => Object.assign({}, x));
        for (let index = 0; index < res.distribucion.length; index++) {
          if (res.distribucion[index].idCliente == this.registro.idCliente) {
            if (res.distribucion[index].idProyecto == this.registro.idProyecto) {
              //usuarios
              //var usuarioProyecto = {usuario:res.distribucion[index].usuario, horas:res.distribucion[index].horas} 
              var indiceUser = users.indexOf(users.find(x => res.distribucion[index].usuario == x.usuario));
              users[indiceUser].horas = res.distribucion[index].horas;
              //this.usuariosProyecto.push(usuarioProyecto);
              if (index == res.distribucion.length - 1) {
                var datosT = { cliente: this.registro.clinete, proyecto: this.registro.proyecto, unidad: this.registro.unidad, usuarios: users, area: this.registro.area }
                this.datosTabla.push(datosT);
                this.datosDistribucion.push(datosT);
              }
            }
            else {
              var datosT = { cliente: this.registro.clinete, proyecto: this.registro.proyecto, unidad: this.registro.unidad, usuarios: users, area: this.registro.area }
              this.datosTabla.push(datosT);
              this.datosDistribucion.push(datosT);
              this.registro = res.distribucion[index];
              //this.usuariosProyecto = [];
              users = this.usuarios.map(x => Object.assign({}, x));
              var indiceUser = users.indexOf(users.find(x => res.distribucion[index].usuario == x.usuario));
              users[indiceUser].horas = res.distribucion[index].horas;
              //var usuarioProyecto = {usuario:res.distribucion[index].usuario, horas:res.distribucion[index].horas} 
              //this.usuariosProyecto.push(usuarioProyecto);
              if (index == res.distribucion.length - 1) {
                var datosT = { cliente: this.registro.clinete, proyecto: this.registro.proyecto, unidad: this.registro.unidad, usuarios: users, area: this.registro.area }
                this.datosTabla.push(datosT);
                this.datosDistribucion.push(datosT);
              }
            }
          }
          else if (res.distribucion[index].idCliente !== this.registro.idCliente) {

            var datosT = { cliente: this.registro.clinete, proyecto: this.registro.proyecto, unidad: this.registro.unidad, usuarios: users, area: this.registro.area }
            this.datosTabla.push(datosT);
            this.datosDistribucion.push(datosT);
            this.registro = res.distribucion[index];
            //this.usuariosProyecto = [];
            users = this.usuarios.map(x => Object.assign({}, x));
            var indiceUser = users.indexOf(users.find(x => res.distribucion[index].usuario == x.usuario));
            users[indiceUser].horas = res.distribucion[index].horas;

            if (index == res.distribucion.length - 1) {
              users = this.usuarios.map(x => Object.assign({}, x));
              var indiceUser = users.indexOf(users.find(x => res.distribucion[index].usuario == x.usuario));
              users[indiceUser].horas = res.distribucion[index].horas;
              var datosT = { cliente: res.distribucion[index].clinete, proyecto: res.distribucion[index].proyecto, unidad: res.distribucion[index].unidad, usuarios: users, area: res.distribucion[index].area }
              this.datosTabla.push(datosT);
              this.datosDistribucion.push(datosT);
            }
            //var usuarioProyecto = {usuario:res.distribucion[index].usuario, horas:res.distribucion[index].horas} 
            //this.usuariosProyecto.push(usuarioProyecto);

          }

        }
        users = this.usuarios.map(x => Object.assign({}, x));
        for (let index = 0; index < res.actividadesExt.length; index++) {

          if (res.actividadesExt[index].idUnidad == this.registroExtra.idUnidad) {
            if (res.actividadesExt[index].idActividad == this.registroExtra.idActividad) {
              //usuarios
              //var usuarioProyecto = {usuario:res.distribucion[index].usuario, horas:res.distribucion[index].horas} 
              var indiceUser = users.indexOf(users.find(x => res.actividadesExt[index].usuario == x.usuario));
              users[indiceUser].horas = res.actividadesExt[index].horas;
              //this.usuariosProyecto.push(usuarioProyecto);
              if (index == res.actividadesExt.length - 1) {
                var datosTExt = { cliente: '', proyecto: '', unidad: this.registroExtra.unidad, area: this.registroExtra.area, idactividad: this.registroExtra.idActividad, actividad: this.registroExtra.actividad, usuarios: users }
                this.datosEventosExtra.push(datosTExt);
                this.datosDistribucion.push(datosTExt);

              }
            }
            else {
              var datosTExt = { cliente: '', proyecto: '', unidad: this.registroExtra.unidad, area: this.registroExtra.area, idactividad: this.registroExtra.idActividad, actividad: this.registroExtra.actividad, usuarios: users }
              this.datosEventosExtra.push(datosTExt);
              this.datosDistribucion.push(datosTExt);
              this.registroExtra = res.actividadesExt[index];
              //this.usuariosProyecto = [];
              users = this.usuarios.map(x => Object.assign({}, x));
              var indiceUser = users.indexOf(users.find(x => res.actividadesExt[index].usuario == x.usuario));
              users[indiceUser].horas = res.actividadesExt[index].horas;

              if (index == res.actividadesExt.length - 1) {
                users = this.usuarios.map(x => Object.assign({}, x));
                var indiceUser = users.indexOf(users.find(x => res.actividadesExt[index].usuario == x.usuario));
                users[indiceUser].horas = res.actividadesExt[index].horas;
                var datosTExt = { cliente: '', proyecto: '', unidad: res.actividadesExt[index].unidad, area: res.actividadesExt[index].area, idactividad: this.registroExtra.idActividad, actividad: res.actividadesExt[index].actividad, usuarios: users }
                this.datosEventosExtra.push(datosTExt);
                this.datosDistribucion.push(datosTExt);
              }
            }
          }

          else if (res.actividadesExt[index].idUnidad !== this.registroExtra.idUnidad) {

            var datosTExt = { cliente: '', proyecto: '', unidad: this.registroExtra.unidad, area: this.registroExtra.area, idactividad: this.registroExtra.idActividad, actividad: this.registroExtra.actividad, usuarios: users }
            this.datosEventosExtra.push(datosTExt);
            this.datosDistribucion.push(datosTExt);

            this.registroExtra = res.actividadesExt[index];
            //console.log(this.datosEventosExtra, this.registroExtra)
            //this.usuariosProyecto = [];
            users = this.usuarios.map(x => Object.assign({}, x));
            var indiceUser = users.indexOf(users.find(x => res.actividadesExt[index].usuario == x.usuario));
            users[indiceUser].horas = res.actividadesExt[index].horas;


            //aqui empieza

            if (index == res.actividadesExt.length - 1) {
              users = this.usuarios.map(x => Object.assign({}, x));
              var indiceUser = users.indexOf(users.find(x => res.actividadesExt[index].usuario == x.usuario));
              users[indiceUser].horas = res.actividadesExt[index].horas;
              var datosTExt = { cliente: '', proyecto: '', unidad: res.actividadesExt[index].unidad, area: res.actividadesExt[index].area, idactividad: this.registroExtra.idActividad, actividad: res.actividadesExt[index].actividad, usuarios: users }
              this.datosEventosExtra.push(datosTExt);
              this.datosDistribucion.push(datosTExt);
            }
            //aqui termina



          }
          this.inicio = false;
        }


        // console.log(this.datosTabla)
        ////total horas proyecto
        var horasProy = 0;
        for (let i = 0; i < this.datosTabla.length; i++) {
          for (let ind = 0; ind < this.datosTabla[i].usuarios.length; ind++) {
            if (this.datosTabla[i].usuarios[ind].horas) {
              horasProy = horasProy + this.datosTabla[i].usuarios[ind].horas;
            }

          }
          this.datosTabla[i].totalHoras = horasProy;
          horasProy = 0;

        }
        var horasProy = 0;
        for (let i = 0; i < this.datosEventosExtra.length; i++) {
          for (let ind = 0; ind < this.datosEventosExtra[i].usuarios.length; ind++) {
            if (this.datosEventosExtra[i].usuarios[ind].horas) {
              horasProy = horasProy + this.datosEventosExtra[i].usuarios[ind].horas;
            }

          }
          this.datosEventosExtra[i].totalHoras = horasProy;
          horasProy = 0;

        }

        /////total horas usuarios (columnas)


        this.usuariosTotal = users = this.usuarios.map(x => Object.assign({}, x));
        this.usuariosTotalOperaciones = users = this.usuarios.map(x => Object.assign({}, x));

        for (let i = 0; i < this.usuariosTotalOperaciones.length; i++) {
          this.usuariosTotalOperaciones[i].horas = 0;
        }
        for (let i = 0; i < this.usuariosTotal.length; i++) {
          this.usuariosTotal[i].horas = 0;
        }

        for (let index = 0; index < this.datosTabla.length; index++) {
          for (let ind = 0; ind < this.datosTabla[index].usuarios.length; ind++) {
            if (this.datosTabla[index].usuarios[ind].horas !== undefined) {
              this.usuariosTotal[ind].horas += this.datosTabla[index].usuarios[ind].horas;
              this.usuariosTotalOperaciones[ind].horas += this.datosTabla[index].usuarios[ind].horas;
            }

          }
        }
        for (let index = 0; index < this.datosEventosExtra.length; index++) {
          for (let ind = 0; ind < this.datosEventosExtra[index].usuarios.length; ind++) {
            if (this.datosEventosExtra[index].usuarios[ind].horas !== undefined) {
              this.usuariosTotal[ind].horas += this.datosEventosExtra[index].usuarios[ind].horas;
              if (this.datosEventosExtra[index].idactividad === 40 || this.datosEventosExtra[index].idactividad === 42 || this.datosEventosExtra[index].idactividad === 54) {
                this.usuariosTotalOperaciones[ind].horas += this.datosEventosExtra[index].usuarios[ind].horas;
              }
            }
          }
        }

        // ------------- Total filas --------------------------------
        this.horasTotales = 0;
        for (let index = 0; index < this.usuariosTotal.length; index++) {
          if (this.usuariosTotal[index].horas !== undefined) {
            this.horasTotales += this.usuariosTotal[index].horas;

          }
        }
        this.horasTotalesOperaciones = 0;
        for (let index = 0; index < this.usuariosTotalOperaciones.length; index++) {
          if (this.usuariosTotalOperaciones[index].horas !== undefined) {
            this.horasTotalesOperaciones += this.usuariosTotalOperaciones[index].horas;

          }
        }

        // Total tiempos muertos (columna)

        this.usuariosTiemposMuertos = users = this.usuarios.map(x => Object.assign({}, x));
        for (let i = 0; i < this.usuariosTiemposMuertos.length; i++) {
          this.usuariosTiemposMuertos[i].horas = this.usuariosTotal[i].horas;
        }
        for (let index = 0; index < this.datosTabla.length; index++) {
          for (let ind = 0; ind < this.datosTabla[index].usuarios.length; ind++) {
            if (this.datosTabla[index].usuarios[ind].horas !== undefined) {
              if (this.datosTabla[index].usuarios[ind].usuario == this.usuariosTiemposMuertos[ind].usuario) {
                this.usuariosTiemposMuertos[ind].horas -= this.datosTabla[index].usuarios[ind].horas;
              }
            }
          }
        }

        for (let index = 0; index < this.datosEventosExtra.length; index++) {
          for (let ind = 0; ind < this.datosEventosExtra[index].usuarios.length; ind++) {
            if (this.datosEventosExtra[index].usuarios[ind].horas !== undefined) {
              if (this.datosEventosExtra[index].usuarios[ind].usuario == this.usuariosTiemposMuertos[ind].usuario) {
                if (this.datosEventosExtra[index].idactividad === 41 || this.datosEventosExtra[index].idactividad === 45 ||
                  this.datosEventosExtra[index].idactividad === 40 || this.datosEventosExtra[index].idactividad === 42 || this.datosEventosExtra[index].idactividad === 54) {
                  this.usuariosTiemposMuertos[ind].horas -= this.datosEventosExtra[index].usuarios[ind].horas;
                }
              }
            }
          }
        }

        this.horasTotalesTiemposMuertos = 0;
        for (let index = 0; index < this.usuariosTiemposMuertos.length; index++) {
          if (this.usuariosTiemposMuertos[index].horas !== undefined) {
            this.horasTotalesTiemposMuertos += this.usuariosTiemposMuertos[index].horas;

          }
        }

        const distinctArray = [...new Set(this.datosDistribucion.map((item:any) => JSON.stringify(item)))].map(item => JSON.parse(item));
        this.datosDistribucion = distinctArray;

        this.datosDistribucion;
        this.sortedData = this.datosDistribucion.map(x => Object.assign({}, x));

        //console.log(this.datosEventosExtra)
        this.spinner.hide();
      }
      , error => {
        this.spinner.hide();
        //console.log(error)
      }
    );
  }

  getReporteProyectos() {
    this.spinner.show();

    this.reporteProyecto = [];
    this.reporteProyectosProc = this.vacio.map(x => Object.assign({}, x));
    // console.log(this.reporteProyectosProc)
    var idUsuario = parseInt(localStorage.getItem('currentUser')!);
    this.datosReporte = { idUser: idUsuario, idUnidad: this.unidadSeleccionada, idArea: this.areaSeleccionada, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo, IdCliente: this.clienteSeleccionado };
    this.serviceReportes.getConsultaProyectos(this.datosReporte).subscribe(res => {
      //console.log(res);
      for (let i = 0; i < this.reporteProyectosProc.length; i++) {
        if (this.reporteProyectosProc[i].unidadesHora) {
          for (let ind = 0; ind < this.reporteProyectosProc[i].unidadesHora.length; ind++) {
            this.reporteProyectosProc[i].unidadesHora = []
          }
        }

      }
      var idProy = 0;
      var unidadesHoras = [];
      this.unidadesHoras = [];
      var horasProyecto = 0;
      var result = res;
      if (result.proyectos.length > 0) {
        var registro = result.proyectos[0];
        idProy = result.proyectos[0].idProyecto;
      }
      for (let i = 0; i < result.proyectos.length; i++) {
        if (result.proyectos[i].idProyecto == idProy) {
          horasProyecto += result.proyectos[i].horas;
          registro = result.proyectos[i];
          var unidadHoras = { unidad: result.proyectos[i].unidadRegistro, horas: result.proyectos[i].horas };
          this.unidadesHoras.push(unidadHoras);
          if (i == result.proyectos.length - 1) {
            //horasProyecto+=result.proyectos[i].horas;
            //registro = result.proyectos[i];
            // var unidadHoras = {unidad:result.proyectos[i].unidadRegistro,  horas:result.proyectos[i].horas};
            // this.unidadesHoras.push(unidadHoras);
            unidadesHoras = this.unidadesHoras.map(x => Object.assign({}, x));
            this.unidadesHoras = [];
            var regProy = { area: registro.area, cliente: registro.cliente, idProyecto: registro.idProyecto, proyecto: registro.proyecto, unidad: registro.unidad, horas: horasProyecto, unidadesHora: unidadesHoras, actExt: false }
            this.reporteProyectosProc.push(regProy);
            //console.log(regProy)
            this.unidadesHoras = [];
          }
        }
        else {
          // console.log(unidadesHoras)
          // console.log(this.unidadesHoras)
          unidadesHoras = this.unidadesHoras.map(x => Object.assign({}, x));
          this.unidadesHoras = [];
          var regProy = { area: registro.area, cliente: registro.cliente, idProyecto: registro.idProyecto, proyecto: registro.proyecto, unidad: registro.unidad, horas: horasProyecto, unidadesHora: unidadesHoras, actExt: false }
          this.reporteProyectosProc.push(regProy);
          //console.log(regProy)

          registro = result.proyectos[i];
          horasProyecto = 0;
          horasProyecto += result.proyectos[i].horas;
          idProy = result.proyectos[i].idProyecto;
          var unidadHoras = { unidad: result.proyectos[i].unidadRegistro, horas: result.proyectos[i].horas };
          this.unidadesHoras.push(unidadHoras);

          if (i == result.proyectos.length - 1) {
            unidadesHoras = this.unidadesHoras.map(x => Object.assign({}, x));
            this.unidadesHoras = [];
            var regProy = { area: registro.area, cliente: registro.cliente, idProyecto: registro.idProyecto, proyecto: registro.proyecto, unidad: registro.unidad, horas: horasProyecto, unidadesHora: unidadesHoras, actExt: false }
            this.reporteProyectosProc.push(regProy);
            //console.log(regProy)
            this.unidadesHoras = [];

          }
        }
      }
      // console.log(this.reporteProyectosProc)
      //console.log(this.reporteProyectosProc)
      //this.reporteProyecto = result.proyectos;
      for (let i = 0; i < this.reporteProyectosProc.length; i++) {
        //console.log( this.reporteProyectosProc[i])

        this.reporteProyecto.push(this.reporteProyectosProc[i])
        //console.log( this.reporteProyectosProc[i], this.reporteProyecto)

      }
      this.reporteProyectoActExt = result.actividadesExt;
      for (let i = 0; i < this.reporteProyectoActExt.length; i++) {
        this.reporteProyectoActExt[i].actExt = true;
        this.reporteProyecto.push(this.reporteProyectoActExt[i])

      }
      var cant = result.proyectos.length + result.actividadesExt.length;

      if (this.inicio) {
        if (result.proyectos.length > 0) {

          this.unidadNombre = result.proyectos[0].unidad;
          if (this.unidadNombre == null || this.unidadNombre == undefined) {
            this.unidadSeleccionada = -1;
          }
          else {
            var index = this.unidadesNegocio.indexOf(this.unidadesNegocio.find(x => this.unidadNombre == x.nombre));
            if (index == -1) {
              //this.unidadSeleccionada=-1;
            } else {
              this.unidadSeleccionada = this.unidadesNegocio[index].id;
              this.idUnidad = this.unidadesNegocio[index].id;
            }

          }


        }


      }
      if (cant > 0) {
        this.sinRegistros = false;
        if (this.inicio) {
          this.unidadSeleccionada = -1;
        }
      }
      else {
        //this.unidadSeleccionada = -1;
        this.sinRegistros = true;
      }
      //console.log(this.reporteProyecto)
      this.sortedData = this.reporteProyecto.map(x => Object.assign({}, x));
      this.inicio = false;
      this.spinner.hide();
    }, err => {
      this.spinner.hide();
      //console.log(err)
    });
  }
  abrirTabla(datos: any) {
    console.log(datos)
    if (datos.length > 0) {
      const dialogRef = this.dialog.open(DialogTable, {
        width: '450px',
        data: datos
      });
    }
  }

  abrirDetallePersona(idUsuario: number, usuario: any) {
    this.datosReporte = { idUser: idUsuario, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo };
    console.log(this.datosReporte);
    this.nombreUsuario = usuario;
    this.serviceReportes.getConsultaDetalleUsuario(this.datosReporte).subscribe(res => {
      //console.log(res);
      //console.log(this.datosReporte)
      this.resultDetallePersona = res.lista;
      var cant = res.lista.length;
      var fecha;
      var fechaRegistro;
      for (let index = 0; index < res.lista.length; index++) {
        fecha = new Date(res.lista[index].fecha);
        fechaRegistro = new Date(res.lista[index].fechaRegistro);

        var mes = parseInt(moment(fecha).format('M'));
        var dia = parseInt(moment(fecha).format('D'));
        var año = (moment(fecha).year());
        var fechaFormat = dia + "/" + (mes) + "/" + año;
        res.lista[index].fecha = fechaFormat;

        var mesReg = parseInt(moment(fechaRegistro).format('M'));
        var diaReg = parseInt(moment(fechaRegistro).format('D'));
        var añoReg = (moment(fechaRegistro).year());
        var fechaFormatReg = diaReg + "/" + (mesReg) + "/" + añoReg;
        res.lista[index].fechaRegistro = fechaFormatReg;
      }

      var fechaInicio = new Date(this.lunesRepo);
      var mes = parseInt(moment(fechaInicio).format('M'));
      var dia = parseInt(moment(fechaInicio).format('D'));
      var año = (moment(fechaInicio).year());
      var fechaFormatInicio = dia + "/" + (mes) + "/" + año;

      var fechaFin = new Date(this.lunesRepo);
      var mesFin = parseInt(moment(fechaFin).format('M'));
      var diaFin = parseInt(moment(fechaFin).format('D'));
      var añoFin = (moment(fechaFin).year());
      var fechaFormatFin = diaFin + "/" + (mesFin) + "/" + añoFin;

      if (window.innerWidth > 1300) {
        const dialogRef = this.dialog.open(DialogTable2, {
          width: '70vw',
          height: '62vh',
          data: [res.lista, usuario, fechaFormatInicio, fechaFormatFin]
        });
      } else {
        const dialogRef = this.dialog.open(DialogTable2, {
          width: '70vw',
          height: '70vh',
          data: [res.lista, usuario, fechaFormatInicio, fechaFormatFin]
        });
      }


      if (cant > 0) {
        this.sinRegistrosModal = false;
        if (this.inicio) {
          this.unidadSeleccionada = -1;
        }
      }
      else {
        this.sinRegistrosModal = true;
      }
      this.inicio = false;
      this.spinner.hide();
    }, err => {//console.log(err)
      this.spinner.hide();
    });

  }

  abrirDetalleHoras(idProyecto: number, proyecto: any) {
    this.spinner.show();
    this.datosReporte = { IdProyecto: idProyecto, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo };
    console.log(this.datosReporte);

    this.serviceReportes.getConsultaPersonas_RegistroPorProyecto(this.datosReporte).subscribe(res => {
      console.log(res);
      //console.log(this.datosReporte)
      this.resultDetallePersona = res.lista;
      var cant = res.lista.length;
      if (window.innerWidth > 1300) {
        const dialogRef = this.dialog.open(DialogTable3, {
          width: '70vw',
          height: '73vh',
          data: [res.lista, "Personas", idProyecto, proyecto]
        });
      } else {
        const dialogRef = this.dialog.open(DialogTable3, {
          width: '70vw',
          height: '89vh',
          data: [res.lista, "Personas", idProyecto, proyecto]
        });
      }

      if (cant > 0) {
        this.sinRegistrosModal = false;
        if (this.inicio) {
          this.unidadSeleccionada = -1;
        }
      }
      else {
        this.sinRegistrosModal = true;
      }
      this.inicio = false;
      this.spinner.hide();
    }, err => {//console.log(err)
      this.spinner.hide();
    });


  }

  abrirDetalleHorasSemanal(idProyecto: number, proyecto: any) {
    this.spinner.show();
    this.datosReporte = { IdProyecto: idProyecto, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo };

    this.serviceReportes.getConsultaPersonas_RegistroPorProyectoSemanal(this.datosReporte).subscribe(res => {
      console.log(res);
      //console.log(this.datosReporte)
      this.resultDetallePersona = res.lista;
      var cant = res.lista.length;
      if (window.innerWidth > 1300) {
        const dialogRef = this.dialog.open(DialogTable3, {
          width: '70vw',
          height: '73vh',
          data: [res.lista, "Semanal", idProyecto, proyecto]
        });
      } else {
        const dialogRef = this.dialog.open(DialogTable3, {
          width: '70vw',
          height: '89vh',
          data: [res.lista, "Semanal", idProyecto, proyecto]
        });
      }

      if (cant > 0) {
        this.sinRegistrosModal = false;
        if (this.inicio) {
          this.unidadSeleccionada = -1;
        }
      }
      else {
        this.sinRegistrosModal = true;
      }
      this.inicio = false;
      this.spinner.hide();
    }, err => {//console.log(err)
      this.spinner.hide();
    });
  }

  getReporteSemanal() {
    this.reporteSemanalTotal = [];
    this.spinner.show();
    var idUsuario = parseInt(localStorage.getItem('currentUser')!);
    //console.log(this.usuarioSeleccionado);
    this.datosReporte = { idUser: idUsuario, idUnidad: this.unidadSeleccionada, idArea: this.areaSeleccionada, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo, idUserFiltro: this.usuarioSeleccionado };
    this.serviceReportes.getConsultaSemanal(this.datosReporte).subscribe(res => {
      //console.log(res);
      //console.log(this.datosReporte)
      //console.log(res.direc)
      this.reporteSemanal = res.semanal;
      this.reporteSemanalActExt = res.actividadesExt;


      var cant = res.semanal.length + res.actividadesExt.length;

      if (this.inicio) {
        this.idUnidad = -1;
        this.getUsuariosSemanal(this.idUnidad);
      }

      if (cant > 0) {
        this.sinRegistros = false;
        if (this.inicio) {
          this.unidadSeleccionada = -1;
        }
      }
      else {
        this.sinRegistros = true;
      }
      for (let i = 0; i < this.reporteSemanal.length; i++) {
        this.reporteSemanalTotal.push(this.reporteSemanal[i]);
      }
      for (let i = 0; i < this.reporteSemanalActExt.length; i++) {
        this.reporteSemanalTotal.push(this.reporteSemanalActExt[i]);
      }

      const distinctArray = [...new Set(this.reporteSemanalTotal.map((item:any) => JSON.stringify(item)))].map(item => JSON.parse(item));
      this.reporteSemanalTotal = distinctArray;

      this.sortedData = this.reporteSemanalTotal.map(x => Object.assign({}, x));
      this.inicio = false;
      this.spinner.hide();
    }, err => {//console.log(err)
      this.spinner.hide();
    });
  }



  getReporteEjecutivo() {
    this.reporteEjecutivo = [];
    this.spinner.show();
    var idUsuario = parseInt(localStorage.getItem('currentUser')!);
    this.datosReporte = { idUser: idUsuario, idUnidad: this.unidadSeleccionada, idArea: this.areaSeleccionada, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo };
    console.log(this.datosReporte);
    if (this.datosReporte.fechaIni == undefined) {
      return;
    }
    this.serviceReportes.getConsultaEjecutivo(this.datosReporte).subscribe(res => {
      console.log(res);
      //console.log(this.datosReporte)
      this.reporteEjecutivo = res.ejecutivo;
      this.horasTotalesEjecutivo = res.total;
      this.graficaRepEjecutivo = res.listaPorcentajes;
      this.graficaReporteejecutivo(this.graficaRepEjecutivo);
      var cant = res.ejecutivo.length;

      if (cant > 0) {
        this.sinRegistros = false;
        if (this.inicio) {
          this.unidadSeleccionada = -1;
        }
      }
      else {
        this.sinRegistros = true;
      }
      this.inicio = false;
      this.spinner.hide();
    }, err => {//console.log(err)
      this.spinner.hide();
    });

  }

  filterListCareUnit(val: any) {
    console.log(val);
    this.usuariosFiltro = this.usuariosFiltro.filter((unit) => unit.label.indexOf(val) > -1);
  }

  selUnidad(event: any) {
    let unidad = event;
    if (event != -1) {
      unidad = parseInt((event.target as HTMLSelectElement).value);
    }
    this.unidadSeleccionada = unidad;
    if (unidad != -1 && this.areaSeleccionada == 0) {
      this.serviceReportes.getAreasConUnidad(Number(localStorage.getItem('currentUser')), unidad).subscribe(result => {
        this.areasUnidades = result.avanceR;
      }, err => { }//console.log(err)
      );
    }
    else if (unidad == -1 && this.areaSeleccionada == 0) {
      this.getAreas();
    }
    if (this.tipoReporte == 'Detallado') {
      this.getUsuarios(unidad);
    }
    else if (this.tipoReporte == 'Semanal') {
      this.getUsuariosSemanal(unidad);
    }
    else if (this.tipoReporte == 'por Persona') {
    }
    this.getProyectos(unidad);
    this.getActividades(unidad);
    this.getEtapas(unidad);
    this.usuarioSeleccionado = 0;
    this.proyectoSeleccionado = 0;
    this.etapaSeleccionada = 0;
    this.actividadSeleccionada = 0;
    this.valorFiltroDetalle = ""; 

  }

  selArea(event: any) {
    const area = parseInt((event.target as HTMLSelectElement).value);
    this.areaSeleccionada = area;

    if (area != 0 && this.unidadSeleccionada == -1) {
      this.serviceReportes.getUnidadConAreas(Number(localStorage.getItem('currentUser')), area).subscribe(result => {
        this.unidadesNegocio = result.avanceR;
      },
        err => { }
      )
    }
    else if (area == 0 && this.unidadSeleccionada == -1) {
      this.getUnidades();
    }
    if (this.tipoReporte == 'Detallado') {
      this.getUsuarios(this.unidadSeleccionada);
    }
    else if (this.tipoReporte == 'Semanal') {
      this.getUsuariosSemanal(this.unidadSeleccionada);
    }
    this.getProyectos(this.unidadSeleccionada);
    this.getActividades(this.unidadSeleccionada);
    this.getEtapas(this.unidadSeleccionada);
    this.usuarioSeleccionado = 0;
    this.proyectoSeleccionado = 0;
    this.etapaSeleccionada = 0;
    this.actividadSeleccionada = 0;
  }

  selProyecto(event: any) {
    const value = parseInt((event.target as HTMLSelectElement).value);
    this.proyectoSeleccionado = value;
    if (value > 0) {
      this.actividadSeleccionada = 0;
      this.valorFiltroDetalle = ""; 
      $('#etapaSel').prop('disabled', false);
      // $('#actividadSel').prop('disabled',true);
      this.actividadDesac = true;
    }
    else {
      // $('#actividadSel').prop('disabled',false);
      this.etapaSeleccionada = 0;
      $('#etapaSel').prop('disabled', true);

      this.actividadDesac = false;
    }

    this.getActividades(this.unidadSeleccionada);
    this.getEtapas(this.unidadSeleccionada);

    this.etapaSeleccionada = 0;
    this.actividadSeleccionada = 0;
    this.valorFiltroDetalle = ""; 
  }

  selEtapa(event: any) {
    const etapa = parseInt((event.target as HTMLSelectElement).value);
    this.etapaSeleccionada = etapa;
    this.getActividades(this.unidadSeleccionada);

    this.actividadSeleccionada = 0;
    this.valorFiltroDetalle = ""; 
  }

  selUser(event: any) {
    const usuario = parseInt((event.target as HTMLSelectElement).value);
    this.usuarioSeleccionado = usuario;
    this.getProyectos(this.unidadSeleccionada);
    this.getActividades(this.unidadSeleccionada);
    this.getEtapas(this.unidadSeleccionada);

    this.proyectoSeleccionado = 0;
    this.etapaSeleccionada = 0;
    this.actividadSeleccionada = 0;
    this.valorFiltroDetalle = ""; 
  }

  selActividad(event: any) {
    const actividad = parseInt((event.target as HTMLSelectElement).value);
    this.actividadSeleccionada = actividad;
  }



  exportar() {
    //this.spinner.show();
    console.log("entra")
    const requestOptions: Object = {
      responseType: 'text'
    }
    if (this.distribucion == true) {
      this.serviceReportes.exportarReporteDistribucion(requestOptions).subscribe(
        res => {
          //console.log(res)

          this.downloadFile(res);
          this.spinner.hide();
        }, err => {
          this.spinner.hide();
          //console.log(err)
        }
      );
    }
    if (this.detallado) {
      this.serviceReportes.exportarReporteDetallado(requestOptions).subscribe(
        res => {
          //console.log(res)

          this.downloadFile(res);
          this.spinner.hide();
        }, err => {
          this.spinner.hide();
          //console.log(err)
        }
      )
    }
    if (this.persona) {
      this.serviceReportes.exportarReportePersona(requestOptions).subscribe(
        res => {
          //console.log(res)

          this.downloadFile(res);
          this.spinner.hide();
        }, err => {
          this.spinner.hide();
          //console.log(err)
        }
      )
    }
    if (this.proyecto) {
      this.serviceReportes.exportarReporteProyectos(requestOptions).subscribe(
        res => {
          //console.log(res)

          this.downloadFile(res);
          this.spinner.hide();

        }, err => {
          this.spinner.hide();
          //console.log(err)
        }
      )
    }
    if (this.semanal) {
      this.serviceReportes.exportarReporteSemanal(requestOptions).subscribe(
        res => {
          //console.log(res)

          this.downloadFile(res);
          this.spinner.hide();
        }, err => {

          this.spinner.hide();
          //console.log(err)
        }
      )
    }

    if (this.ejecutivo) {
      this.serviceReportes.exportarReporteEjecutivo(requestOptions).subscribe(
        res => {
          //console.log(res)
          this.downloadFile(res);
          this.spinner.hide();
        }, err => {

          this.spinner.hide();
          //console.log(err)
        }
      )
    }

  }

  downloadFile(archivo: any) {
    var blob = this.descargaService.downloadFile(archivo).subscribe(res => {
      console.log(res);
      saveAs(res, archivo);
    }, err => {
      this.titulo = "Error";
      this.mensaje = "Ha ocurrido un error al descargar el archivo, intente nuevamente más tarde"
      this.toastr.error(this.mensaje, this.titulo);
      //console.log(err)
    });

  }


  getUsuarios(unidad: any) {
    this.usuariosFiltro = [];
    var datos = { idUnidad: unidad.toString() }    
      this.serviceReportes.getConsultaUsuarios(datos).subscribe(res => {
        this.usuariosFiltro = res;
      }, err => { }//console.log(err)
      );
  }

  getUsuariosSemanal(unidad: any) {
    this.usuariosFiltro = [];
    var datos = { idUnidad: unidad.toString() }    
      this.serviceReportes.getConsultaUsuariosSemanal(datos).subscribe(res => {
        this.usuariosFiltro = res;
      }, err => { }//console.log(err)
      );
  }

  recuperarUsuarios() {
    this.usuarios = [];

    var idUsuario = parseInt(localStorage.getItem('currentUser')!);
    this.datosReporte = { idUser: idUsuario, idUnidad: this.unidadSeleccionada, idArea: this.areaSeleccionada, fechaIni: this.lunesRepo, fechaFin: this.domingoRepo, IdUserFiltro: this.usuarioSeleccionado };

    return this.serviceReportes.getConsultaUsuariosPersona().subscribe(res => {
        for (let index = 0; index < res.usuarios.length; index++) {
          this.usuarios.push(res.usuarios[index]);
          this.UsuariosFiltrados.next(this.usuarios.slice());
        }
      }, error => {
        //console.log(error)
      });

  }

  getUsuariosPersonas(unidad: any) {
    this.usuariosFiltro = [];
    var datos = { idUnidad: unidad.toString() }
    this.serviceReportes.getConsultaUsuariosPersona()
      .subscribe(res => {
        this.usuariosFiltro = res;
      }, err => { }//console.log(err)
      );
  }

  getProyectos(unidad: any) {
    var datosP = { idUser: this.idUsuario, idUnidad: unidad, idArea: this.areaSeleccionada, idUserFiltro: this.usuarioSeleccionado };
    var datos = { idUser: this.usuarioSeleccionado.toString(), idUnidad: unidad.toString(), idArea: this.areaSeleccionada.toString() };

    this.serviceReportes.getConsultaListaProyectos(datosP)
      .subscribe(res => {//console.log(res)
        this.proyectosFiltro = res;
        this.proyectosFiltroOriginal = res;
      }, err =>//console.log(err)
      { });

    if (this.proyectosFiltroOriginal.length > this.proyectosFiltro.length) {
      this.proyectosFiltro = this.proyectosFiltroOriginal
    }
  }

  getActividades(unidad: any) {
    var datosP = { idUser: this.idUsuario, idUnidad: unidad, idArea: this.areaSeleccionada, idProyecto: this.proyectoSeleccionado, idUserFiltro: this.usuarioSeleccionado, idActividad: this.actividadSeleccionada, idEtapa: this.etapaSeleccionada };
    this.serviceReportes.getConsultaActividades(datosP)
      .subscribe(res => {//console.log(res)
        this.actividadesFiltro = res;
      }, err =>//console.log(err)
      { });
  }

  getEtapas(unidad: any) {
    var datosP = { idUser: this.idUsuario, idUnidad: unidad, idArea: this.areaSeleccionada, idProyecto: this.proyectoSeleccionado, idUserFiltro: this.usuarioSeleccionado, idActividad: this.actividadSeleccionada, idEtapa: this.etapaSeleccionada };
    this.serviceReportes.getConsultaEtapas(datosP)
      .subscribe(res => {//console.log(res)
        this.etapasFiltro = res;
      }, err =>//console.log(err)
      { });
  }
  getEncuestados() {
    this.reporteEncuestados = [];
    //console.log(this.datosReporte)
    this.nom35Service.getConsultaEncuestadoso().subscribe(
      res => {
        //console.log(res)
        this.reporteEncuestados = res.encuestados
        this.registrosTabEncuestados = this.reporteEncuestados.length;
        if (res.encuestados.length > 0) {
          this.sinRegistros = false;
          var fecha;
          var fechaRegistro;
          for (let index = 0; index < res.encuestados.length; index++) {
            fecha = new Date(res.encuestados[index].fecha);
            var mes = parseInt(moment(fecha).format('M'));
            var dia = parseInt(moment(fecha).format('D'));
            var año = (moment(fecha).year());
            var fechaFormat = dia + "/" + (mes) + "/" + año;
            res.encuestados[index].fecha = fechaFormat;
          }

        }
      }, err => {
      }
    );
    this.getNoEncuestados();
  }

  getNoEncuestados() {
    this.reporteNoEncuestados = [];
    //console.log(this.datosReporte)
    this.nom35Service.getNoEncuestados().subscribe(
      res => {
        //console.log(res)
        this.reporteNoEncuestados = res.encuestados
        this.registrosTabNoEncuestados = this.reporteNoEncuestados.length;
        if (res.encuestados.length > 0) {
          this.sinRegistros = false;
          var fecha;
          var fechaRegistro;
          for (let index = 0; index < res.encuestados.length; index++) {
            fecha = new Date(res.encuestados[index].fecha);
            var mes = parseInt(moment(fecha).format('M'));
            var dia = parseInt(moment(fecha).format('D'));
            var año = (moment(fecha).year());
            var fechaFormat = dia + "/" + (mes) + "/" + año;
            res.encuestados[index].fecha = fechaFormat;
          }
        }
      }, err => {
      }
    );
  }

  graficaParticipaciones() {
    this.encuestadosTotales = this.registrosParticipacion + this.registrosNoParticipacion;
    this.porcentajeParticipacion = (this.registrosParticipacion * 100) / this.encuestadosTotales;
    this.porcentajeNoParticipacion = (this.registrosNoParticipacion * 100) / this.encuestadosTotales;

    const chartContainer = document.getElementById("chartContainerParticipaciones");
    if (!chartContainer) return;

    if (this.chartParticipaciones) {
      this.chartParticipaciones.destroy();
    }

    // // chartContainer.innerHTML = '&nbsp;';
    chartContainer.innerHTML = '<canvas id="chartParticipaciones"></canvas>';

    this.canvas = document.getElementById('chartParticipaciones') as HTMLCanvasElement;
    if (!this.canvas) return;

    this.ctx = this.canvas.getContext('2d');
    if (!this.ctx) return;

    let myChart = new Chart(this.ctx, {
      type: 'doughnut',
      //  this.registrosParticipacion,this.registrosNoParticipacion
      data: {

        labels: ["Encuestas terminadas " + "(" + this.porcentajeParticipacion.toFixed(2) + "%)",
        "Encuestas sin contestar " + "(" + this.porcentajeNoParticipacion.toFixed(2) + "%)"],
        datasets: [{
          label: 'Participacion',
          data: [this.registrosParticipacion, this.registrosNoParticipacion],
          backgroundColor: [
            'rgba(50, 195, 132, 1)',
            'rgba(237, 28, 36, 1)'
          ],
          borderWidth: 1,
        }]
      },
      options: {
        responsive: true,
        onClick: (event: ChartEvent, elements: any[]) => {
          try {
            if (elements.length > 0) {
              const index = elements[0].index;
              if (index === 1) {
                ($('#modalNoEncuestados') as any).modal('show');
                $("#modalNoEncuestados").css("padding-right", "254px");
              } else {
                ($('#modalEncuestados') as any).modal('show');
                $("#modalEncuestados").css("padding-right", "371px");
              }
            }
          } catch (error) {
            console.error('Error en onClick:', error);
          }
        },
        rotation: 0.5 * Math.PI,
        plugins: {
          legend: {
            display: true,
            position: screen.width > 720 ? 'right' : 'bottom',
            labels: {
              boxWidth: 45,
              font: {
                size: 16
              },
            },
            onClick: function (event, elem) {
              if (elem.text.indexOf("Encuestas sin contestar") >= 0) {
                ($('#modalNoEncuestados') as any).modal('show');
                $("#modalNoEncuestados").css("padding-right", "254px");
              } else {
                ($('#modalEncuestados') as any).modal('show');
                $("#modalEncuestados").css("padding-right", "371px");
              }
            }
          },
          tooltip: {
            enabled: true
          },
          datalabels: {
            formatter: (value, ctx) => {
              let percentage = (value);
              return percentage;
            },
            color: 'white',
            textShadowBlur: 15,
            textShadowColor: 'black',
            textStrokeColor: 'black',
            textStrokeWidth: 8,
            font: {
              size: 20,
              weight: 600
            }
          }
        }
      },
    });
    this.getEncuestados();
  }


  graficaReporteejecutivo(graficaRepEjecutivo: any) {
    const chartContainer = document.getElementById("chartContainer");
    if (!chartContainer) return;

    if (this.chartEjecutivo) {
      this.chartEjecutivo.destroy();
    }

    // chartContainer.innerHTML = '&nbsp;';
    chartContainer.innerHTML = '<canvas id="chartEjecutivo"></canvas>';

    this.canvas = document.getElementById('chartEjecutivo') as HTMLCanvasElement;
    if (!this.canvas) return;

    this.ctx = this.canvas.getContext('2d');
    if (!this.ctx) return;

    const colores = [
      '#13871B', // Verde oscuro
      '#20C92C', // Verde claro
      '#B9FFBC', // Verde pastel
      '#0e3e62'  // Azul oscuro
    ];

    const mapaColores = new Map<string, string>();

    graficaRepEjecutivo.forEach((item: any, index: number) => {
      if (!mapaColores.has(item.proyecto)) {
        const color = colores[index];
        mapaColores.set(item.proyecto, color);
      }
    });

    const datosGrafica = graficaRepEjecutivo.filter((item: any) => item.porcentaje > 0);

    console.log('Datos para la gráfica:', datosGrafica);
    console.log('Colores para la gráfica:', datosGrafica.map((item:any) => item.color));

    this.chartEjecutivo = new Chart(this.ctx, {
      type: 'pie',
      data: {
        labels: datosGrafica.map((item:any) => `${item.proyecto} (${item.horas} hr)`),
        datasets: [{
          data: datosGrafica.map((item:any) => item.porcentaje),
          backgroundColor: datosGrafica.map((item: any) => mapaColores.get(item.proyecto)),
          borderWidth: 1,
        }]
      },
      options: {
        responsive: true,
        rotation: 0.5 * Math.PI,
        plugins: {
          legend: {
            display: true,
            position: 'left',
            align: 'center',
            labels: {
              boxWidth: 40,
              padding: 15,
              font: {
                size: 14
              },
              generateLabels: (chart) => {
                return graficaRepEjecutivo.map((item: any) => ({
                  text: `${item.proyecto} (${item.horas} hr)`,
                  fillStyle: item.porcentaje === 0 
                    ? '#cccccc' 
                    : mapaColores.get(item.proyecto), 
                  hidden: false,
                  lineCap: 'butt',
                  lineDash: [],
                  lineDashOffset: 0,
                  lineJoin: 'miter',
                  lineWidth: 1,
                  strokeStyle: '#fff',
                  pointStyle: 'circle',
                  datasetIndex: 0
                }));
              }
            },
          },
          tooltip: {
            enabled: true
          },
          datalabels: {
            formatter: (value, ctx) => {
              let percentage = (value);
              return percentage + "%";
            },
            color: 'white',
            textShadowBlur: 14,
            textShadowColor: 'black',
            textStrokeColor: 'black',
            textStrokeWidth: 8,
            font: {
              size: 12,
              weight: 400
            }
          }
        },
        layout: {
          padding: {
            left: 0,  
            right: 60,
            top: 20,
            bottom: 20
          }
        },
        aspectRatio: 2.5,
      },
    });
  }

  resize_modal() {
    $("#modalEncuestados").css("padding-right", "371px");
    $("#modalNoEncuestados").css("padding-right", "254px");
  }

  getParticipantes(): void {
    this.nom35Service.getInformacionEncuestas().subscribe(
      res => {

        this.registrosParticipacion = res.encuestados;
        this.registrosNoParticipacion = res.noEncuestados;
        this.graficaParticipaciones();
      }, err => {
      }
    );
  }

  enviarRecordatorio(id: number, email: string) {
    this._id = id;
    this._email = email;
    try {
      this.serviceReportes.postRecordatorioNom035(this._id, this._email)
      this.toastr.info("Recordatorio enviado correctamente.", "Reporte participantes");
      this.getNoEncuestados();
    } catch (error) {
      this.toastr.error("Error al enviar recordatorio: " + error, "Reporte participantes");
    }
  }

  mostrarFiltros() {
    this.mostrarFiltro = true;
    this.cabecera = true;
    let tablabody = $(".tablabody").height();
    $(".tablabody").height(tablabody! - this.filtros);
  }

  ocultarFiltros() {
    this.mostrarFiltro = false;
    this.cabecera = false;
    this.filtros = $(".divFiltros").height();
    var tablabody = $(".tablabody").height();

    $(".tablabody").height(tablabody + this.filtros);
  }


  //   //Parte de Resultado de encuestas
  getResultadoEncuesta() {
    this.reporteresultNom35 = [];
    var numEmpleado = 0;
    var empleado;
    var puntos;
    var desCal;
    this.nom35Service.getResultadoEncuesta().subscribe(
      res => {
        this.reporteresultNom35 = res.objeto
        this.registrosTabresultNom35 = this.reporteresultNom35.length;
        if (res.objeto.length > 0) {
          this.sinRegistros = false;
        }


        numEmpleado = res.objeto[0].idempleado;
        empleado = res.objeto[0].empleado;
        puntos = res.objeto[0].puntos;
        desCal = res.objeto[0].desCal;

        this.getResultadoEncuestaPorEmpresa();
      }, err => {
        console.log(err)
      }
    );

  }

  getResultadoEncuestaPorEmpleado(idEmpleado: any, empleado: any, puntos: any, desCal: any, esPorEmpelado: any) {
    this.encuestaPorEmpleado = true;
    this.encuestaPorEmpresa = false;
    this.resEmpleado = empleado;
    this.resPuntos = puntos;
    this.resDesCal = desCal;
    this.reporteresultCategoriaEmpleado = [];
    this.nom35Service.getEncuestasCategoriaPorEmpleado(idEmpleado).subscribe(
      res => {
        this.reporteresultCategoriaEmpleado = res.objeto
        this.registrosTabresultNom35 = this.reporteresultCategoriaEmpleado.length;
        if (res.objeto.length > 0) {
          this.sinRegistros = false;
        }
        // console.log("Resultado:::" + this.reporteresultCategoriaEmpleado )
      }, err => {
        console.log(err)
      }
    );

    this.getResultadoEncuestaPorDomEmpleado(idEmpleado, esPorEmpelado);

  }

  getResultadoEncuestaPorDomEmpleado(idEmpleado: any, esPorEmpelado: any) {
    this.reporteresultDominioEmpleado = [];
    this.nom35Service.getEncuestasDominioPorEmpleado(idEmpleado).subscribe(
      res => {
        this.reporteresultDominioEmpleado = res.objeto
        this.registrosTabresultNom35 = this.reporteresultDominioEmpleado.length;
        if (res.objeto.length > 0) {
          this.sinRegistros = false;
        }
        // console.log("Resultado:::" + this.reporteresultCategoriaEmpleado )
      }, err => {
        console.log(err)
      }
    );

  }
  getResultadoEncuestaPorEmpresa() {
    this.spinner.show();
    this.reporteresultTotalesEmpresa = [];
    this.nom35Service.getResultadoEncuestaPorEmpresa().subscribe(
      res => {
        this.reporteresultTotalesEmpresa = res.objeto
        this.registrosTabTotalesEmpresa = this.reporteresultTotalesEmpresa.length;
        if (res.objeto.length > 0) {
          this.sinRegistros = false;
          for (let index = 0; index < res.objeto.length; index++) {
            this.registrosNulo = res.objeto[index].nullDespreciable;
            this.registrosbajo = res.objeto[index].bajo;
            this.registrosMedio = res.objeto[index].medio;
            this.registrosAlto = res.objeto[index].alto;
            this.registrosMuyAlto = res.objeto[index].muyAlto;
          }
        }
        // console.log("Resultado:::" + this.reporteresultCategoriaEmpleado )
        this.spinner.hide();
      }, err => {
        this.spinner.hide();
        console.log(err)
      }
    );
    this.getResultadoEncuestaCatPorEmpresa();
  }

  getResultadoEncuestaCatPorEmpresa() {
    this.reporteresultCategoriaEmpresa = [];
    this.nom35Service.getResultadoEncuestaCatPorEmpresa().subscribe(
      res => {
        this.reporteresultCategoriaEmpresa = res.objeto
        this.registrosTabCategoriaEmpresa = this.reporteresultCategoriaEmpresa.length;
        if (res.objeto.length > 0) {
          this.sinRegistros = false;
        }
        this.obtenerDataGraficasCat(res.objeto);

        // console.log("Resultado:::" + this.reporteresultCategoriaEmpleado )
      }, err => {
        console.log(err)
      }
    );

    this.getResultadoEncuestaDomPorEmpresa();
  }

  getResultadoEncuestaDomPorEmpresa() {
    this.reporteresultDominioEmpresa = [];
    this.nom35Service.getResultadoEncuestaDomPorEmpresa().subscribe(
      res => {
        this.reporteresultDominioEmpresa = res.objeto
        this.registrosTabDominioEmpresa = this.reporteresultDominioEmpresa.length;
        if (res.objeto.length > 0) {
          this.sinRegistros = false;
        }
        this.obtenerDataGraficasDom(res.objeto);
        // console.log("Resultado:::" + this.reporteresultCategoriaEmpleado )
      }, err => {
        console.log(err)
      }
    );
  }

  setOcultarChartCategorias(parametro: any) {
    if (!this.regresar) {
      this.fuenteLeyendaChart = 20;
      this.ShadowBlurChart = 15;
      this.StrokeWidth = 8
      this.regresar = true;
      if (parametro == "Ambiente") {
        this.charFactores = false;
        this.charLiderazgo = false;
        this.charOrganizacion = false;

        setTimeout(() => {
          this.graficaCategoriaAmbiente();
        }, 50);
      } else if (parametro == "Factores") {

        this.charAmbiente = false;
        this.charLiderazgo = false;
        this.charOrganizacion = false;
        setTimeout(() => {
          this.graficaCategoriaFactores();
        }, 50);
      } else if (parametro == "Liderazgo") {

        this.charAmbiente = false;
        this.charFactores = false;
        this.charOrganizacion = false;
        setTimeout(() => {
          this.graficaCategoriaOrgani();
        }, 50);
      } else {

        this.charAmbiente = false;
        this.charFactores = false;
        this.charLiderazgo = false;
        setTimeout(() => {
          this.graficaCategoriaLider();
        }, 50);
      }
    }
  }

  getMostrarPor(parametro: string) {
    if (parametro == "empleado") {
      this.encuestaPorEmpleado = false;
      this.encuestaPorEmpresa = true;
      this.resEmpleado = "";
    } else if (parametro == "empresa") {
      this.encuestaPorEmpleado = false;
      this.encuestaPorEmpresa = true;

    } else if (parametro == "Niveles") {
      this.tituloGrafico = "Criterios para toma de acciones";
      ($('#modalAcciones') as any).modal('show');
    } else if (parametro == "graficos") {
      this.charTotales = true;
      this.tituloGrafico = "Total de Entrevistas -  Final";
      ($('#modalGraficas') as any).modal('show');
      $("#modalGraficas").css("padding-right", "488px"); //612px
      $("#divGraficos").css("width", "920px");
      this.charDominioSelect = false;
      this.charCategorias = false;
      this.charDominio = false;
      setTimeout(() => {
        this.graficaTotal();
      }, 15);

    } else if (parametro == "charTotales") {

      this.charTotales = true;
      this.charCategorias = false;
      this.charDominioSelect = false;
      this.charDominio = false;
      setTimeout(() => {
        $("#divGraficos").css("width", "920px");
        this.graficaTotal();

      }, 50);

    } else if (parametro == "charCategorias") {
      this.tituloGrafico = "Total de Entrevistas -  Categorias";
      this.charCategorias = true;
      this.charTotales = false;
      this.charDominioSelect = false;
      this.charDominio = false;
      this.charFactores = true;
      this.charAmbiente = true;
      this.charLiderazgo = true;
      this.charOrganizacion = true;
      this.fuenteLeyendaChart = 14;
      this.ShadowBlurChart = 0;
      this.StrokeWidth = 3
      this.regresar = false;
      setTimeout(() => {
        this.graficaCategoria();
      }, 50);

    } else {
      this.dominioFiltro = [{ "idDom": 2, "dominio": "Condiciones en el ambiente de trabajo" },
      { "idDom": 3, "dominio": "Falta de control sobre el trabajo" }, { "idDom": 4, "dominio": "Interferencia en la relación trabajo-familia" },
      { "idDom": 5, "dominio": "Jornada de trabajo" },
      { "idDom": 6, "dominio": "Liderazgo" },
      { "idDom": 7, "dominio": "Relaciones en el trabajo" },
      { "idDom": 8, "dominio": "Violencia" }]
      this.tituloGrafico = "Total de Entrevistas -  Dominio";
      this.charDominioSelect = true;
      this.charDominio = true;
      this.charTotales = false;
      this.charCategorias = false;

      this.graficaDominio("Carga de trabajo");

    }
  }

  graficaTotal() {
    this.resultadosTotalesPie = this.registrosNulo + this.registrosbajo + this.registrosMedio + this.registrosAlto + this.registrosMuyAlto;
    this.canvas = document.getElementById('chartTotales');
    this.ctx = this.canvas.getContext('2d');
    this.porcentajeNulo = (this.registrosNulo * 100) / this.resultadosTotalesPie;
    this.porcentajeBajo = (this.registrosbajo * 100) / this.resultadosTotalesPie;
    this.porcentajeMedio = (this.registrosMedio * 100) / this.resultadosTotalesPie;
    this.porcentajeAlto = (this.registrosAlto * 100) / this.resultadosTotalesPie;
    this.porcentajeMuyAlto = (this.registrosMuyAlto * 100) / this.resultadosTotalesPie;
    var pointBackgroundColors = [];
    let myChart = new Chart(this.ctx, {
      type: 'doughnut',
      data: {
        //labels: ["Encuestas terminadas " + "("+ this.porcentajeParticipacion.toFixed(2)+"%)",
        // "Encuestas sin contestar " +"("+this.porcentajeNoParticipacion.toFixed(2)+"%)"],
        labels: ["Nulo o despreciable " + "(" + this.porcentajeNulo.toFixed(2) + "%)",
        "Bajo " + "(" + this.porcentajeBajo.toFixed(2) + "%)",
        "Medio " + "(" + this.porcentajeMedio.toFixed(2) + "%)",
        "Alto " + "(" + this.porcentajeAlto.toFixed(2) + "%)",
        "Muy alto " + "(" + this.porcentajeMuyAlto.toFixed(2) + "%)"
        ],
        datasets: [{
          label: 'Participacion',
          //data: [this.registrosParticipacion,this.registrosNoParticipacion],
          data: [this.registrosNulo, this.registrosbajo, this.registrosMedio, this.registrosAlto, this.registrosMuyAlto],
          backgroundColor: [
            'rgba(0, 176, 240, 1)',
            'rgba(146, 208, 80, 1)',
            'rgba(255, 255, 0, 1)',
            'rgba(255, 192, 0, 1)',
            'rgba(255, 0, 0, 1)'
          ],
          borderWidth: 1,

        }]
      },
      options: {
        cutout: '0%',
        responsive: true,
        rotation: 0.5 * Math.PI,
        plugins: {
          legend: {
            display: true,
            position: screen.width > 720 ? 'right' : 'bottom',
            labels: {
              boxWidth: 35,
              font: {
                size: 13
              }
            }

          },
          tooltip: {
            enabled: true
          },
          datalabels: {
            anchor: 'end',
            align: 'start',
            offset: 10,
            formatter: (value, ctx) => {
              let percentage = (value);
              return percentage;
            },
            display: function (context) {
              var index = context.dataIndex;
              var value = context.dataset.data[index];
              return value == 0 ? false : true;
            },
            color: 'white',
            textShadowBlur: 15,
            textShadowColor: 'black',
            textStrokeColor: 'black',
            textStrokeWidth: 8,
            font: {
              size: 20,
              weight: 600
            }
          }
        }
      },
    });
    /* for (var i = 0; i < myChart.data.datasets[0].data.length; i++) {
       if (myChart.data.datasets[0].data[i] == 0) {
           pointBackgroundColors.push("auto");
           console.log("Es cero")
       } else {
           pointBackgroundColors.push("true");
       }
     }
     myChart.update();*/
  }

  graficaCategoria() {
    this.graficaCategoriaAmbiente();
    this.graficaCategoriaFactores();
    this.graficaCategoriaOrgani();
    this.graficaCategoriaLider();
  }

  graficaCategoriaAmbiente() {
    this.resultadosTotalesPieAmb = this.registrosBajoDataUno + this.registrosBajoDataUno + this.registrosMedioDataUno + this.registrosAltoDataUno + this.registrosMuyAltoDataUno;
    this.canvas = document.getElementById('charCategoriaAmbiente');
    this.ctx = this.canvas.getContext('2d');
    let myChart = Chart.getChart(this.ctx);
    if (myChart) {
      myChart.clear();
      myChart.destroy();
    }
    this.porcentajeNulo = (this.registrosBajoDataUno * 100) / this.resultadosTotalesPieAmb;
    this.porcentajeBajo = (this.registrosBajoDataUno * 100) / this.resultadosTotalesPieAmb;
    this.porcentajeMedio = (this.registrosMedioDataUno * 100) / this.resultadosTotalesPieAmb;
    this.porcentajeAlto = (this.registrosAltoDataUno * 100) / this.resultadosTotalesPieAmb;
    this.porcentajeMuyAlto = (this.registrosMuyAltoDataUno * 100) / this.resultadosTotalesPieAmb;
    myChart = new Chart(this.ctx, {
      type: 'doughnut',
      data: {
        //labels: ["Encuestas terminadas " + "("+ this.porcentajeParticipacion.toFixed(2)+"%)",
        // "Encuestas sin contestar " +"("+this.porcentajeNoParticipacion.toFixed(2)+"%)"],
        labels: ["Nulo o despreciable " + "(" + this.porcentajeNulo.toFixed(2) + "%)",
        "Bajo " + "(" + this.porcentajeBajo.toFixed(2) + "%)",
        "Medio " + "(" + this.porcentajeMedio.toFixed(2) + "%)",
        "Alto " + "(" + this.porcentajeAlto.toFixed(2) + "%)",
        "Muy alto " + "(" + this.porcentajeMuyAlto.toFixed(2) + "%)"
        ],
        datasets: [{
          label: 'Participacion',
          //data: [this.registrosParticipacion,this.registrosNoParticipacion],
          data: [this.registrosBajoDataUno, this.registrosBajoDataUno, this.registrosMedioDataUno, this.registrosAltoDataUno, this.registrosMuyAltoDataUno],
          backgroundColor: [
            'rgba(0, 176, 240, 1)',
            'rgba(146, 208, 80, 1)',
            'rgba(255, 255, 0, 1)',
            'rgba(255, 192, 0, 1)',
            'rgba(255, 0, 0, 1)'
          ],
          borderWidth: 1,
        }]
      },
      options: {
        cutout: '0%',
        responsive: true,
        rotation: 0.5 * Math.PI,
        plugins: {
          legend: {
            display: true,
            position: screen.width > 720 ? 'right' : 'bottom',
            labels: {
              boxWidth: 25,
              font: {
                size: 11
              }
            }
          },
          tooltip: {
            enabled: true
          },
          datalabels: {
            anchor: 'end',
            align: 'start',
            display: function (context) {
              var index = context.dataIndex;
              var value = context.dataset.data[index];
              return value == 0 ? false : true;
            },
            formatter: (value, ctx) => {
              let percentage = (value);
              return percentage;
            },
            color: 'white',
            textShadowColor: 'black',
            textStrokeColor: 'black',
            textStrokeWidth: this.StrokeWidth,
            textShadowBlur: this.ShadowBlurChart,
            font: {
              size: this.fuenteLeyendaChart,
              weight: 600
            }
          }
        }
      },
    });
  }

  graficaCategoriaFactores() {
    this.resultadosTotalesPieFac = this.registrosNuloDataDos + this.registrosBajoDataDos + this.registrosMedioDataDos + this.registrosAltoDataDos + this.registrosMuyAltoDataDos;
    this.canvas = document.getElementById('charCategoriaFactores');
    this.ctx = this.canvas.getContext('2d');
    this.porcentajeNuloFac = (this.registrosNuloDataDos * 100) / this.resultadosTotalesPieFac;
    this.porcentajeBajoFac = (this.registrosBajoDataDos * 100) / this.resultadosTotalesPieFac;
    this.porcentajeMedioFac = (this.registrosMedioDataDos * 100) / this.resultadosTotalesPieFac;
    this.porcentajeAltoFac = (this.registrosAltoDataDos * 100) / this.resultadosTotalesPieFac;
    this.porcentajeMuyAltoFac = (this.registrosMuyAltoDataDos * 100) / this.resultadosTotalesPieFac;
    let myChart = Chart.getChart(this.ctx);
    if (myChart) {
      myChart.clear();
      myChart.destroy();
    }
    myChart = new Chart(this.ctx, {
      type: 'doughnut',
      data: {
        //labels: ["Encuestas terminadas " + "("+ this.porcentajeParticipacion.toFixed(2)+"%)",
        // "Encuestas sin contestar " +"("+this.porcentajeNoParticipacion.toFixed(2)+"%)"],
        labels: ["Nulo o despreciable " + "(" + this.porcentajeNuloFac.toFixed(2) + "%)",
        "Bajo " + "(" + this.porcentajeBajoFac.toFixed(2) + "%)",
        "Medio " + "(" + this.porcentajeMedioFac.toFixed(2) + "%)",
        "Alto " + "(" + this.porcentajeAltoFac.toFixed(2) + "%)",
        "Muy alto " + "(" + this.porcentajeMuyAltoFac.toFixed(2) + "%)"
        ],
        datasets: [{
          label: 'Participacion',
          //data: [this.registrosParticipacion,this.registrosNoParticipacion],
          data: [this.registrosNuloDataDos, this.registrosBajoDataDos, this.registrosMedioDataDos, this.registrosAltoDataDos, this.registrosMuyAltoDataDos],
          backgroundColor: [
            'rgba(0, 176, 240, 1)',
            'rgba(146, 208, 80, 1)',
            'rgba(255, 255, 0, 1)',
            'rgba(255, 192, 0, 1)',
            'rgba(255, 0, 0, 1)'
          ],
          borderWidth: 1,
        }]
      },
      options: {
        cutout: '0%',
        responsive: true,
        rotation: 0.5 * Math.PI,
        plugins: {
          legend: {
            display: true,
            position: screen.width > 720 ? 'right' : 'bottom',
            labels: {
              boxWidth: 25,
              font: {
                size: 11
              }
            }

          },
          tooltip: {
            enabled: true
          },
          datalabels: {
            anchor: 'end',
            align: 'start',
            display: function (context) {
              var index = context.dataIndex;
              var value = context.dataset.data[index];
              return value == 0 ? false : true;
            },
            formatter: (value, ctx) => {
              let percentage = (value);
              return percentage;
            },
            color: 'white',
            textShadowColor: 'black',
            textStrokeColor: 'black',
            textStrokeWidth: this.StrokeWidth,
            textShadowBlur: this.ShadowBlurChart,
            font: {
              size: this.fuenteLeyendaChart,
              weight: 600
            }
          }
        }
      },
    });
  }

  graficaCategoriaOrgani() {
    this.resultadosTotalesPieOrg = this.registrosNuloDataTres + this.registrosBajoDataTres + this.registrosMedioDataTres + this.registrosAltoDataTres + this.registrosMuyAltoDataTres;
    this.canvas = document.getElementById('charCategoriaOrganizacion');
    this.ctx = this.canvas.getContext('2d');
    this.porcentajeNuloOrg = (this.registrosNuloDataTres * 100) / this.resultadosTotalesPieOrg;
    this.porcentajeBajoOrg = (this.registrosBajoDataTres * 100) / this.resultadosTotalesPieOrg;
    this.porcentajeMedioOrg = (this.registrosMedioDataTres * 100) / this.resultadosTotalesPieOrg;
    this.porcentajeAltoOrg = (this.registrosAltoDataTres * 100) / this.resultadosTotalesPieOrg;
    this.porcentajeMuyAltoOrg = (this.registrosMuyAltoDataTres * 100) / this.resultadosTotalesPieOrg;
    const conf = {};
    let myChart = Chart.getChart(this.ctx);
    if (myChart) {
      myChart.clear();
      myChart.destroy();
    }
    myChart = new Chart(this.ctx, {
      type: 'doughnut',
      data: {
        //labels: ["Encuestas terminadas " + "("+ this.porcentajeParticipacion.toFixed(2)+"%)",
        // "Encuestas sin contestar " +"("+this.porcentajeNoParticipacion.toFixed(2)+"%)"],
        labels: ["Nulo o despreciable " + "(" + this.porcentajeNuloOrg.toFixed(2) + "%)",
        "Bajo " + "(" + this.porcentajeBajoOrg.toFixed(2) + "%)",
        "Medio " + "(" + this.porcentajeMedioOrg.toFixed(2) + "%)",
        "Alto " + "(" + this.porcentajeAltoOrg.toFixed(2) + "%)",
        "Muy alto " + "(" + this.porcentajeMuyAltoOrg.toFixed(2) + "%)"
        ],
        datasets: [{
          label: 'Participacion',
          //data: [this.registrosParticipacion,this.registrosNoParticipacion],
          data: [this.registrosNuloDataTres, this.registrosBajoDataTres, this.registrosMedioDataTres, this.registrosAltoDataTres, this.registrosMuyAltoDataTres],
          backgroundColor: [
            'rgba(0, 176, 240, 1)',
            'rgba(146, 208, 80, 1)',
            'rgba(255, 255, 0, 1)',
            'rgba(255, 192, 0, 1)',
            'rgba(255, 0, 0, 1)'
          ],
          borderWidth: 1,
        }]
      },
      options: {
        cutout: '0%',
        responsive: true,
        rotation: 0.5 * Math.PI,
        plugins: {
          legend: {
            display: true,
            position: screen.width > 720 ? 'right' : 'bottom',
            labels: {
              boxWidth: 25,
              font: {
                size: 11
              }
            }

          },
          tooltip: {
            enabled: true
          },
          datalabels: {
            anchor: 'end',
            align: 'start',
            display: function (context) {
              var index = context.dataIndex;
              var value = context.dataset.data[index];
              return value == 0 ? false : true;
            },
            formatter: (value, ctx) => {
              let percentage = (value);
              return percentage;
            },
            color: 'white',
            textShadowColor: 'black',
            textStrokeColor: 'black',
            textStrokeWidth: this.StrokeWidth,
            textShadowBlur: this.ShadowBlurChart,
            font: {
              size: this.fuenteLeyendaChart,
              weight: 600
            }
          }
        }
      },
    });
  }

  graficaCategoriaLider() {
    this.resultadosTotalesPieOrg = this.registrosNuloDataCuatro + this.registrosBajoDataCuatro + this.registrosMedioDataCuatro + this.registrosAltoDataCuatro + this.registrosMuyAltoDataCuatro;
    this.canvas = document.getElementById('charCategoriaLiderazgo');
    this.ctx = this.canvas.getContext('2d');
    this.porcentajeNuloLid = (this.registrosNuloDataCuatro * 100) / this.resultadosTotalesPieOrg;
    this.porcentajeBajoLid = (this.registrosBajoDataCuatro * 100) / this.resultadosTotalesPieOrg;
    this.porcentajeMedioLid = (this.registrosMedioDataCuatro * 100) / this.resultadosTotalesPieOrg;
    this.porcentajeAltoLid = (this.registrosAltoDataCuatro * 100) / this.resultadosTotalesPieOrg;
    this.porcentajeMuyAltoLid = (this.registrosMuyAltoDataCuatro * 100) / this.resultadosTotalesPieOrg;
    let myChart = new Chart(this.ctx, {
      type: 'doughnut',
      data: {
        //labels: ["Encuestas terminadas " + "("+ this.porcentajeParticipacion.toFixed(2)+"%)",
        // "Encuestas sin contestar " +"("+this.porcentajeNoParticipacion.toFixed(2)+"%)"],
        labels: ["Nulo o despreciable " + "(" + this.porcentajeNuloLid.toFixed(2) + "%)",
        "Bajo " + "(" + this.porcentajeBajoLid.toFixed(2) + "%)",
        "Medio " + "(" + this.porcentajeMedioLid.toFixed(2) + "%)",
        "Alto " + "(" + this.porcentajeAltoLid.toFixed(2) + "%)",
        "Muy alto " + "(" + this.porcentajeMuyAltoLid.toFixed(2) + "%)"
        ],
        datasets: [{
          label: 'Participacion',
          //data: [this.registrosParticipacion,this.registrosNoParticipacion],
          data: [this.registrosNuloDataCuatro, this.registrosBajoDataCuatro, this.registrosMedioDataCuatro, this.registrosAltoDataCuatro, this.registrosMuyAltoDataCuatro],
          backgroundColor: [
            'rgba(0, 176, 240, 1)',
            'rgba(146, 208, 80, 1)',
            'rgba(255, 255, 0, 1)',
            'rgba(255, 192, 0, 1)',
            'rgba(255, 0, 0, 1)'
          ],
          borderWidth: 1,
        }]
      },
      options: {
        cutout: '0%',
        responsive: true,
        rotation: 0.5 * Math.PI,
        plugins: {
          legend: {
            display: true,
            position: screen.width > 720 ? 'right' : 'bottom',
            labels: {
              boxWidth: 25,
              font: {
                size: 11
              }
            }

          },
          tooltip: {
            enabled: true
          },
          datalabels: {
            anchor: 'end',
            align: 'start',
            display: function (context) {
              var index = context.dataIndex;
              var value = context.dataset.data[index];
              return value == 0 ? false : true;
            },
            formatter: (value, ctx) => {
              let percentage = (value);
              return percentage;
            },
            color: 'white',
            textShadowColor: 'black',
            textStrokeColor: 'black',
            textStrokeWidth: this.StrokeWidth,
            textShadowBlur: this.ShadowBlurChart,
            font: {
              size: this.fuenteLeyendaChart,
              weight: 600
            }
          }
        }
      },
    });
  }

  graficaDominio(event: any) {
    let tipo_dominio = "";
    tipo_dominio = event;
    if (event != "Carga de trabajo") {
      tipo_dominio = (event.target as HTMLSelectElement).value;
    }

    switch (tipo_dominio) {
      case "Carga de trabajo": {
        setTimeout(() => {
          this.graficaDominioPor(this.registrosNuloDataUnoDom, this.registrosBajoDataUnoDom, this.registrosMedioDataUnoDom, this.registrosAltoDataUnoDom, this.registrosMuyAltoDataUnoDom)
        }, 1);
        break;
      }
      case "Condiciones en el ambiente de trabajo": {
        setTimeout(() => {
          this.graficaDominioPor(this.registrosNuloDataDosDom, this.registrosBajoDataDosDom, this.registrosMedioDataDosDom, this.registrosAltoDataDosDom, this.registrosMuyAltoDataDosDom)
        }, 1);
        break;
      }
      case "Falta de control sobre el trabajo": {
        setTimeout(() => {
          this.graficaDominioPor(this.registrosNuloDataTresDom, this.registrosBajoDataTresDom, this.registrosMedioDataTresDom, this.registrosAltoDataTresDom, this.registrosMuyAltoDataTresDom)
        }, 1);
        break;
      }
      case "Interferencia en la relación trabajo-familia": {

        setTimeout(() => {
          this.graficaDominioPor(this.registrosNuloDataCuatroDom, this.registrosBajoDataCuatroDom, this.registrosMedioDataCuatroDom, this.registrosAltoDataCuatroDom, this.registrosMuyAltoDataCuatroDom)
        }, 1);
        break;
      }
      case "Jornada de trabajo": {

        setTimeout(() => {
          this.graficaDominioPor(this.registrosNuloDataCinco, this.registrosBajoDataCinco, this.registrosMedioDataCinco, this.registrosAltoDataCinco, this.registrosMuyAltoDataCinco)
        }, 1);
        break;
      }
      case "Liderazgo": {
        setTimeout(() => {
          this.graficaDominioPor(this.registrosNuloDataSeis, this.registrosBajoDataSeis, this.registrosMedioDataSeis, this.registrosAltoDataSeis, this.registrosMuyAltoDataSeis)
        }, 1);
        break;
      }
      case "Relaciones en el trabajo": {
        setTimeout(() => {
          this.graficaDominioPor(this.registrosNuloDataSiete, this.registrosBajoDataSiete, this.registrosMedioDataSiete, this.registrosAltoDataSiete, this.registrosMuyAltoDataSiete)
        }, 1);
        break;
      }
      default: {
        setTimeout(() => {
          this.graficaDominioPor(this.registrosNuloDataOcho, this.registrosBajoDataOcho, this.registrosMedioDataOcho, this.registrosAltoDataOcho, this.registrosMuyAltoDataOcho)
        }, 1);
        break;
      }
    }
  }

  graficaDominioPor(registrosNulo: number, registrosbajo: number, registrosMedio: number, registrosAlto: number, registrosMuyAlto: number) {
    this.resultadosTotalesPie = registrosNulo + registrosbajo + registrosMedio + registrosAlto + registrosMuyAlto;
    this.canvas = document.getElementById('charDominio');
    this.ctx = this.canvas.getContext('2d');

    this.porcentajeNulo = (registrosNulo * 100) / this.resultadosTotalesPie;
    this.porcentajeBajo = (registrosbajo * 100) / this.resultadosTotalesPie;
    this.porcentajeMedio = (registrosMedio * 100) / this.resultadosTotalesPie;
    this.porcentajeAlto = (registrosAlto * 100) / this.resultadosTotalesPie;
    this.porcentajeMuyAlto = (registrosMuyAlto * 100) / this.resultadosTotalesPie;
    if (this.myChart) {
      this.myChart.clear();
      this.myChart.destroy();
    }
    this.myChart = new Chart(this.ctx, {
      type: 'doughnut',
      data: {
        //labels: ["Encuestas terminadas " + "("+ this.porcentajeParticipacion.toFixed(2)+"%)",
        // "Encuestas sin contestar " +"("+this.porcentajeNoParticipacion.toFixed(2)+"%)"],
        labels: ["Nulo o despreciable " + "(" + this.porcentajeNulo.toFixed(2) + "%)",
        "Bajo " + "(" + this.porcentajeBajo.toFixed(2) + "%)",
        "Medio " + "(" + this.porcentajeMedio.toFixed(2) + "%)",
        "Alto " + "(" + this.porcentajeAlto.toFixed(2) + "%)",
        "Muy alto " + "(" + this.porcentajeMuyAlto.toFixed(2) + "%)"
        ],
        datasets: [{
          label: 'Participacion',
          //data: [this.registrosParticipacion,this.registrosNoParticipacion],
          data: [registrosNulo, registrosbajo, registrosMedio, registrosAlto, registrosMuyAlto],
          backgroundColor: [
            'rgba(0, 176, 240, 1)',
            'rgba(146, 208, 80, 1)',
            'rgba(255, 255, 0, 1)',
            'rgba(255, 192, 0, 1)',
            'rgba(255, 0, 0, 1)'
          ],
          borderWidth: 1,
        }]
      },
      options: {
        cutout: '0%',
        responsive: true,
        rotation: 0.5 * Math.PI,
        plugins: {
          legend: {
            display: true,
            position: screen.width > 720 ? 'right' : 'bottom',
            labels: {
              boxWidth: 35,
              font: {
                size: 13
              }
            }

          },
          tooltip: {
            enabled: true
          },
          datalabels: {
            anchor: 'end',
            align: 'start',
            display: function (context) {
              var index = context.dataIndex;
              var value = context.dataset.data[index];
              return value == 0 ? false : true;
            },
            formatter: (value, ctx) => {
              let percentage = (value);
              return percentage;
            },
            color: 'white',
            textShadowBlur: 15,
            textShadowColor: 'black',
            textStrokeColor: 'black',
            textStrokeWidth: 8,
            font: {
              size: 20,
              weight: 600
            }
          }
        }
      },
    });

  }
  obtenerDataGraficasCat(parametro: any) {

    if (parametro.length > 0) {
      this.sinRegistros = false;

      for (let index = 0; index < parametro.length; index++) {


        switch (parametro[index].idCategoria) {
          case 1: {
            this.registrosNuloDataUno = parametro[index].nullDespreciable;
            this.registrosBajoDataUno = parametro[index].bajo;
            this.registrosMedioDataUno = parametro[index].medio;
            this.registrosAltoDataUno = parametro[index].alto;
            this.registrosMuyAltoDataUno = parametro[index].muyAlto;
            break;
          }
          case 2: {
            this.registrosNuloDataDos = parametro[index].nullDespreciable;
            this.registrosBajoDataDos = parametro[index].bajo;
            this.registrosMedioDataDos = parametro[index].medio;
            this.registrosAltoDataDos = parametro[index].alto;
            this.registrosMuyAltoDataDos = parametro[index].muyAlto;
            break;
          }
          case 3: {
            this.registrosNuloDataTres = parametro[index].nullDespreciable;
            this.registrosBajoDataTres = parametro[index].bajo;
            this.registrosMedioDataTres = parametro[index].medio;
            this.registrosAltoDataTres = parametro[index].alto;
            this.registrosMuyAltoDataTres = parametro[index].muyAlto;
            break;
          }
          case 4: {
            this.registrosNuloDataCuatro = parametro[index].nullDespreciable;
            this.registrosBajoDataCuatro = parametro[index].bajo;
            this.registrosMedioDataCuatro = parametro[index].medio;
            this.registrosAltoDataCuatro = parametro[index].alto;
            this.registrosMuyAltoDataCuatro = parametro[index].muyAlto;
            break;
          }

          default: {
            //statements; 
            break;
          }
        }

      }
    }
  }

  obtenerDataGraficasDom(parametro: any) {
    if (parametro.length > 0) {
      this.sinRegistros = false;

      for (let index = 0; index < parametro.length; index++) {
        switch (parametro[index].idDominio) {
          case 1: {
            this.registrosNuloDataUnoDom = parametro[index].nullDespreciable;
            this.registrosBajoDataUnoDom = parametro[index].bajo;
            this.registrosMedioDataUnoDom = parametro[index].medio;
            this.registrosAltoDataUnoDom = parametro[index].alto;
            this.registrosMuyAltoDataUnoDom = parametro[index].muyAlto;
            break;
          }
          case 2: {
            this.registrosNuloDataDosDom = parametro[index].nullDespreciable;
            this.registrosBajoDataDosDom = parametro[index].bajo;
            this.registrosMedioDataDosDom = parametro[index].medio;
            this.registrosAltoDataDosDom = parametro[index].alto;
            this.registrosMuyAltoDataDosDom = parametro[index].muyAlto;
            break;
          }
          case 3: {
            this.registrosNuloDataTresDom = parametro[index].nullDespreciable;
            this.registrosBajoDataTresDom = parametro[index].bajo;
            this.registrosMedioDataTresDom = parametro[index].medio;
            this.registrosAltoDataTresDom = parametro[index].alto;
            this.registrosMuyAltoDataTresDom = parametro[index].muyAlto;
            break;
          }
          case 4: {
            this.registrosNuloDataCuatroDom = parametro[index].nullDespreciable;
            this.registrosBajoDataCuatroDom = parametro[index].bajo;
            this.registrosMedioDataCuatroDom = parametro[index].medio;
            this.registrosAltoDataCuatroDom = parametro[index].alto;
            this.registrosMuyAltoDataCuatroDom = parametro[index].muyAlto;
            break;
          }
          case 5: {
            this.registrosNuloDataCinco = parametro[index].nullDespreciable;
            this.registrosBajoDataCinco = parametro[index].bajo;
            this.registrosMedioDataCinco = parametro[index].medio;
            this.registrosAltoDataCinco = parametro[index].alto;
            this.registrosMuyAltoDataCinco = parametro[index].muyAlto;
            break;
          }
          case 6: {
            this.registrosNuloDataSeis = parametro[index].nullDespreciable;
            this.registrosBajoDataSeis = parametro[index].bajo;
            this.registrosMedioDataSeis = parametro[index].medio;
            this.registrosAltoDataSeis = parametro[index].alto;
            this.registrosMuyAltoDataSeis = parametro[index].muyAlto;
            break;
          }
          case 7: {
            this.registrosNuloDataSiete = parametro[index].nullDespreciable;
            this.registrosBajoDataSiete = parametro[index].bajo;
            this.registrosMedioDataSiete = parametro[index].medio;
            this.registrosAltoDataSiete = parametro[index].alto;
            this.registrosMuyAltoDataSiete = parametro[index].muyAlto;
            break;
          }
          case 8: {
            this.registrosNuloDataOcho = parametro[index].nullDespreciable;
            this.registrosBajoDataOcho = parametro[index].bajo;
            this.registrosMedioDataOcho = parametro[index].medio;
            this.registrosAltoDataOcho = parametro[index].alto;
            this.registrosMuyAltoDataOcho = parametro[index].muyAlto;
            break;
          }
          default: {
            //statements; 
            break;
          }
        }

      }
    }
  }

  pageChanged(event: any) {
    this.config.currentPage = event;

    this.getDatosReporte(false);

    if (event == 1) {
      this.pagina = 'Página'
    }
    else {
      this.pagina = 'Páginas'
    }
  }
}//final


@Component({
  selector: 'dialog-table',
  templateUrl: '../../components/reportes/dialog-table.html',
  standalone: false
})
export class DialogTable {
  dataArray: any[] = [];
  constructor(
    public dialogRef: MatDialogRef<DialogTable>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {
    this.dataArray = Array.isArray(this.data) ? this.data : Object.values(this.data);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}

@Component({
  selector: 'modalDetallePersona',
  templateUrl: '../../components/reportes/modalDetallePersona.html',
  standalone: false
})
export class DialogTable2 {
  dataArray: any[] = [];
  constructor(
    public dialogRef: MatDialogRef<DialogTable2>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {
    console.log(data)
    const dialog = this.dialogRef
    dialog.afterOpened().subscribe(_ => {
      setTimeout(() => {
        dialog.close();
      }, 1000 * 60 * 30)
    });
    this.dataArray = Array.isArray(this.data) ? this.data : Object.values(this.data);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  cerrar(): void {
    this.dialogRef.close();
  }

}


@Component({
  selector: 'modalDetallePersona',
  templateUrl: '../../components/reportes/modalDetalleHorasProyecto.html',
  standalone: false
})
export class DialogTable3 {
  datosReporte: any;
  UsuariosRegistros: any[] = [];
  dataArray: any[] = [];
  constructor(
    public dialogRef: MatDialogRef<DialogTable3>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private serviceReportes:ReportesService, private spinner: NgxSpinnerService,
    public dialog: MatDialog
  ) {
    console.log(data)
    const dialogR = this.dialogRef
    dialogR.afterOpened().subscribe(_ => {
      setTimeout(() => {
        dialogR.close();
      }, 1000 * 60 * 30)
    });
    this.dataArray = Array.isArray(this.data) ? this.data : Object.values(this.data);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  cerrar(): void {
    this.dialogRef.close();
  }
  abrirDetalle(_idProyecto: number, fechaIni: string, fechaFin: string): void {
    this.UsuariosRegistros = [];
    let newDate1 = new Date(fechaIni);
    let newDate2 = new Date(fechaFin);
    this.datosReporte = { IdProyecto: _idProyecto, fechaIni: newDate1, fechaFin: newDate2 };

    this.serviceReportes.getConsultaPersonas_RegistroPorProyecto(this.datosReporte).subscribe(res => {
      //console.log(this.datosReporte)
      this.UsuariosRegistros = res.lista;


      this.spinner.hide();
    }, err => {//console.log(err)
      this.spinner.hide();
    });

  }
}
