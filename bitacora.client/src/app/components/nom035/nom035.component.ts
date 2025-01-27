import { Component, OnInit, Inject } from '@angular/core';
import { Globals } from '../../services/globals';
import { Nom035Service } from '../../services/nom035.service';
import { AppComponent } from '../../app.component';
import { ToastrService } from 'ngx-toastr';
import moment from 'moment';

@Component({
  selector: 'app-nom035',
  templateUrl: './nom035.component.html',
  styleUrls: ['./nom035.component.css'],
  standalone: false
})
export class Nom035Component implements OnInit {

  constructor(private authenticationService: Nom035Service, private AppComponent: AppComponent, private toastr: ToastrService) { }
  infoUsuario: any;
  nombre = "";
  apellidoPat = "";
  apellidoMat = "";
  value = 0;
  valueRestante = 0;
  color = 'primary';
  fecha: any;
  finalizar = false;
  mostrarBtnSalir = true;
  mostrarBtnsInicio = true;
  mostrarBtns = false;
  mostrarBtnRegresar = false;
  mostrarBtnSiguiente = true;
  mostrarBtnFinalizar = false;
  mostrardiv01 = true;
  mostrardiv02 = false;
  mostrardiv03 = false;
  mostrardiv04 = false;
  mostrardiv05 = false;
  mostrardiv06 = false;
  mostrardiv07 = false;
  mostrardiv08 = false;
  mostrardiv09 = false;
  mostrardiv10 = false;
  title: any;
  pregunta100 = 0;
  pregunta101 = 0;
  pregunta102 = 0;
  pregunta103 = 0;
  pregunta104 = 0;
  pregunta105 = 0;
  pregunta106 = 0;
  pregunta107 = 0;
  pregunta108 = 0;
  pregunta109 = 0;
  pregunta110 = 0;
  pregunta111 = 0;
  pregunta112 = 0;
  pregunta113 = 0;
  pregunta114 = 0;
  pregunta115 = 0;
  pregunta116 = 0;
  pregunta1 = 0;
  pregunta2 = 0;
  pregunta3 = 0;
  pregunta4 = 0;
  pregunta5 = 0;
  pregunta6 = 0;
  pregunta7 = 0;
  pregunta8 = 0;
  pregunta9 = 0;
  pregunta10 = 0;
  pregunta11 = 0;
  pregunta12 = 0;
  pregunta13 = 0;
  pregunta14 = 0;
  pregunta15 = 0;
  pregunta16 = 0;
  pregunta17 = 0;
  pregunta18 = 0;
  pregunta19 = 0;
  pregunta20 = 0;
  pregunta21 = 0;
  pregunta22 = 0;
  pregunta23 = 0;
  pregunta24 = 0;
  pregunta25 = 0;
  pregunta26 = 0;
  pregunta27 = 0;
  pregunta28 = 0;
  pregunta29 = 0;
  pregunta30 = 0;
  pregunta31 = 0;
  pregunta32 = 0;
  pregunta33 = 0;
  pregunta34 = 0;
  pregunta35 = 0;
  pregunta36 = 0;
  pregunta37 = 0;
  pregunta38 = 0;
  pregunta39 = 0;
  pregunta40 = 0;
  pregunta41 = 0;
  pregunta42 = 0;
  pregunta43 = 0;
  pregunta44 = 0;
  pregunta45 = 0;
  pregunta46 = 0;
  contadorDiv = 1;
  chkPoliticas = false;
  mostrarBarraProcceso = false;
  ngOnInit() {
    // $(".divPolitica").css({display:'none'});

    //     $('#div_botones').scroll(function() {
    //       alert('dfdsafds');
    //         if ($(this).scrollTop() == $(this)[0].scrollHeight - $(this).height()) {
    //             alert('dfdsafds');
    //             $('#payment').removeAttr('disabled');
    //         }
    //     });

    const index = Globals.permisos.indexOf(Globals.permisos.find(x => x.nombrePantalla === "NOM 035"));

    if (index > -1) {//verifica que tenga el permiso ver la vista
      //obtiene la informacion del usuario.
      this.authenticationService.postInfoParticipante(localStorage.getItem('userName'))
        .subscribe((resp: any) => {
          //metodos 
          this.infoUsuario = resp;//almacena la inforamcion del usuario
          this.nombre = this.infoUsuario.nombre;//variable para mostrar en pantalla el nombre
          this.apellidoPat = this.infoUsuario.apellidoPat;
          this.apellidoMat = this.infoUsuario.aperllidoMat;
          this.fecha = moment().format('DD-MM-YYYY');
          this.mostrardiv01 = true;
        });

    } else {
      this.AppComponent.logout();
    }
  }

  empezarEncuesta() {
    if (!this.chkPoliticas) {
      setTimeout(() => {
        this.toastr.error("Debes aceptar las políticas de la NOM 035 antes de comenzar la encuesta", "No puedes acceder a la encuesta");
      }, 500);
      return false;
    }
    this.mostrarBtnsInicio = false;
    this.mostrarBtns = true;
    this.mostrardiv02 = true;
    this.value = this.value + 11.12;
    this.mostrarBarraProcceso = true;
    return true;
  }
  btnSiguiente() {
    window.scroll(0, 0);

    if (this.contadorDiv == 1) {
      if (this.pregunta100 != 0) {
        this.mostrarBtnRegresar = true;
        this.mostrarBtnSalir = false;
        this.value = this.value + 11.11;
        if (this.pregunta100 == 7) {
          this.contadorDiv = this.contadorDiv + 1;
          this.ocultarBotones();
          this.mostrardiv03 = true;
        } else {
          this.contadorDiv = this.contadorDiv + 2;
          this.ocultarBotones();
          this.mostrardiv04 = true;
          this.value = this.value + 11.11;

        }
      } else {
        this.toastr.error("Conteste todas las preguntas para poder avanzar", "Atención");
      }
    } else if (this.contadorDiv == 2) {
      if (this.validarVacioDiv03()) {
        this.value = this.value + 11.11;
        this.contadorDiv = this.contadorDiv + 1;
        this.ocultarBotones();
        this.mostrardiv04 = true;
      }

    } else if (this.contadorDiv == 3) {
      if (this.validarVacioDiv04()) {
        this.contadorDiv = this.contadorDiv + 1;
        this.value = this.value + 11.11;
        this.ocultarBotones();
        this.mostrardiv05 = true;
      }
    } else if (this.contadorDiv == 4) {
      if (this.validarVacioDiv05()) {
        this.value = this.value + 11.11;
        this.contadorDiv = this.contadorDiv + 1;
        this.ocultarBotones();
        this.mostrardiv06 = true;
      }
    } else if (this.contadorDiv == 5) {
      if (this.validarVacioDiv06()) {
        this.value = this.value + 11.11;
        this.contadorDiv = this.contadorDiv + 1;
        this.ocultarBotones();
        this.mostrardiv07 = true;
      }
    } else if (this.contadorDiv == 6) {
      if (this.pregunta115 != 0) {
        this.value = this.value + 11.11;
        if (this.pregunta115 == 7) {

          this.contadorDiv = this.contadorDiv + 1;

          this.ocultarBotones();
          this.mostrardiv08 = true;
        } else {
          this.contadorDiv = this.contadorDiv + 2;
          this.ocultarBotones();
          this.mostrardiv09 = true;
          this.value = this.value + 11.11;
        }
      } else {
        this.toastr.error("Conteste todas las preguntas para poder avanzar", "Atención");
      }
    } else if (this.contadorDiv == 7) {
      if (this.validarVacioDiv07()) {
        this.value = this.value + 11.11;
        this.contadorDiv = this.contadorDiv + 1;
        this.ocultarBotones();
        this.mostrardiv09 = true;
      }

    } else if (this.contadorDiv == 8) {
      if (this.pregunta116 != 0) {
        if (this.pregunta116 == 7) {
          this.value = this.value + 11.11;
          this.contadorDiv = this.contadorDiv + 1;
          this.ocultarBotones();
          this.mostrardiv10 = true;
          this.mostrarBtnSiguiente = false;
          this.mostrarBtnFinalizar = true;
          console.log(this.contadorDiv)
        } else {
          this.mostrarBtnSiguiente = false;
          this.mostrarBtnFinalizar = true;

        }
      } else {
        this.toastr.error("Conteste todas las preguntas para poder avanzar", "Atención");
      }
    }
    console.log("value" + this.value);
  }

  btnRegresar() {

    this.value = this.value - 11.11;
    if (this.mostrarBtnFinalizar == true && this.pregunta116 == 6) {

      this.value = this.value - 11.11;
    }
    window.scroll(0, 0);
    this.contadorDiv = this.contadorDiv - 1;
    if (this.contadorDiv == 8) {
      this.ocultarBotones();
      this.mostrardiv09 = true;
      this.mostrarBtnSiguiente = true;
      this.mostrarBtnFinalizar = false;
    } else if (this.contadorDiv == 7) {
      if (this.mostrardiv09 && this.pregunta115 == 7) {
        this.ocultarBotones();
        this.mostrardiv08 = true;
      }
      else {
        this.contadorDiv = this.contadorDiv - 1;
        this.ocultarBotones();
        this.mostrardiv07 = true;
        this.mostrarBtnSiguiente = true;
        this.mostrarBtnFinalizar = false;
        this.value = this.value - 11.11;
      }
    } else if (this.contadorDiv == 6) {
      this.ocultarBotones();
      this.mostrardiv07 = true;

    }
    else if (this.contadorDiv == 5) {
      this.ocultarBotones();
      this.mostrardiv06 = true;
    } else if (this.contadorDiv == 4) {
      this.ocultarBotones();
      this.mostrardiv05 = true;
    }
    else if (this.contadorDiv == 3) {
      this.ocultarBotones();
      this.mostrardiv04 = true;
    }
    else if (this.contadorDiv == 2) {
      if (this.pregunta100 == 7) {
        this.ocultarBotones();
        this.mostrardiv03 = true;
      } else {
        this.ocultarBotones();
        this.contadorDiv = this.contadorDiv - 1;
        this.mostrardiv02 = true;
        this.mostrarBtnRegresar = false;
        this.mostrarBtnSalir = true;
        this.value = this.value - 11.11;
      }
    }
    else if (this.contadorDiv == 1) {
      this.ocultarBotones();
      this.mostrardiv02 = true;
      this.mostrarBtnRegresar = false;
      this.mostrarBtnSalir = true;
      this.value = 11.2;
    }

  }
  muestraBtnFinalizar() {
    this.mostrarBtnSiguiente = false;
    this.mostrarBtnFinalizar = true;
    this.finalizar = true;
    this.valueRestante = 100 - this.value;
    this.value = 100;
  }
  muestraBtnSiguiente() {
    if (this.mostrarBtnFinalizar == true) {
      this.mostrarBtnSiguiente = true;
      this.mostrarBtnFinalizar = false;
      this.finalizar = false;
      this.value = this.value - 11.11;
    }
  }
  ocultarBotones() {
    this.mostrardiv01 = false;
    this.mostrardiv02 = false;
    this.mostrardiv03 = false;
    this.mostrardiv04 = false;
    this.mostrardiv05 = false;
    this.mostrardiv06 = false;
    this.mostrardiv07 = false;
    this.mostrardiv08 = false;
    this.mostrardiv09 = false;
    this.mostrardiv10 = false;
  }
  btnFinalizar() {
    if (this.validarVacioDiv09() || this.finalizar) {
      this.ajustarRespuestas();
      //metodo final para enviar las respuestas y la informacion del usuario
      const buffer = [
        { idempleado: this.infoUsuario.id, IdPregunta: 101, Idrespuestausuario: this.pregunta101, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 102, Idrespuestausuario: this.pregunta102, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 103, Idrespuestausuario: this.pregunta103, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 104, Idrespuestausuario: this.pregunta104, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 105, Idrespuestausuario: this.pregunta105, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 106, Idrespuestausuario: this.pregunta106, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 107, Idrespuestausuario: this.pregunta107, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 108, Idrespuestausuario: this.pregunta108, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 109, Idrespuestausuario: this.pregunta109, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 110, Idrespuestausuario: this.pregunta110, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 111, Idrespuestausuario: this.pregunta111, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 112, Idrespuestausuario: this.pregunta112, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 113, Idrespuestausuario: this.pregunta113, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 114, Idrespuestausuario: this.pregunta114, puntos: 0, fecha: moment().format('YYYY-MM-DD') }

        ,
        { idempleado: this.infoUsuario.id, IdPregunta: 1, Idrespuestausuario: this.pregunta1, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 2, Idrespuestausuario: this.pregunta2, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 3, Idrespuestausuario: this.pregunta3, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 4, Idrespuestausuario: this.pregunta4, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 5, Idrespuestausuario: this.pregunta5, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 6, Idrespuestausuario: this.pregunta6, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 7, Idrespuestausuario: this.pregunta7, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 8, Idrespuestausuario: this.pregunta8, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 9, Idrespuestausuario: this.pregunta9, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 10, Idrespuestausuario: this.pregunta10, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 11, Idrespuestausuario: this.pregunta11, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 12, Idrespuestausuario: this.pregunta12, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 13, Idrespuestausuario: this.pregunta13, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 14, Idrespuestausuario: this.pregunta14, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 15, Idrespuestausuario: this.pregunta15, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 16, Idrespuestausuario: this.pregunta16, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 17, Idrespuestausuario: this.pregunta17, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 18, Idrespuestausuario: this.pregunta18, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 19, Idrespuestausuario: this.pregunta19, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 20, Idrespuestausuario: this.pregunta20, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 21, Idrespuestausuario: this.pregunta21, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 22, Idrespuestausuario: this.pregunta22, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 23, Idrespuestausuario: this.pregunta23, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 24, Idrespuestausuario: this.pregunta24, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 25, Idrespuestausuario: this.pregunta25, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 26, Idrespuestausuario: this.pregunta26, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 27, Idrespuestausuario: this.pregunta27, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 28, Idrespuestausuario: this.pregunta28, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 29, Idrespuestausuario: this.pregunta29, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 30, Idrespuestausuario: this.pregunta30, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 31, Idrespuestausuario: this.pregunta31, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 32, Idrespuestausuario: this.pregunta32, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 33, Idrespuestausuario: this.pregunta33, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 34, Idrespuestausuario: this.pregunta34, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 35, Idrespuestausuario: this.pregunta35, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 36, Idrespuestausuario: this.pregunta36, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 37, Idrespuestausuario: this.pregunta37, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 38, Idrespuestausuario: this.pregunta38, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 39, Idrespuestausuario: this.pregunta39, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 40, Idrespuestausuario: this.pregunta40, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 41, Idrespuestausuario: this.pregunta41, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 42, Idrespuestausuario: this.pregunta42, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 43, Idrespuestausuario: this.pregunta43, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 44, Idrespuestausuario: this.pregunta44, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 45, Idrespuestausuario: this.pregunta45, puntos: 0, fecha: moment().format('YYYY-MM-DD') },
        { idempleado: this.infoUsuario.id, IdPregunta: 46, Idrespuestausuario: this.pregunta46, puntos: 0, fecha: moment().format('YYYY-MM-DD') }

      ];
      this.authenticationService.postResultados(buffer, this.infoUsuario)
        .subscribe((resp: any) => {
          console.log(resp);
          //metodos
        });
      console.log(buffer);
      //validar si ya respondio
      this.toastr.info("Sus respuestas se han guardado correctamente.", "Encuesta finalizada");
      setTimeout(() => {
        this.AppComponent.logout();
      }, 3000);
    } else {

      this.toastr.error("Conteste todas las preguntas para poder avanzar", "Atención");
    }
  }
  validarVacioDiv03() {
    if (this.pregunta101 == 0 ||
      this.pregunta102 == 0 ||
      this.pregunta103 == 0 ||
      this.pregunta104 == 0 ||
      this.pregunta105 == 0 ||
      this.pregunta106 == 0 ||
      this.pregunta107 == 0 ||
      this.pregunta108 == 0 ||
      this.pregunta109 == 0 ||
      this.pregunta110 == 0 ||
      this.pregunta111 == 0 ||
      this.pregunta112 == 0 ||
      this.pregunta113 == 0 ||
      this.pregunta114 == 0) {
      this.toastr.error("Conteste todas las preguntas para poder avanzar", "Atención");
      return false;
    }
    return true;
  }
  validarVacioDiv04() {
    if (this.pregunta1 == 0 ||
      this.pregunta2 == 0 ||
      this.pregunta3 == 0 ||
      this.pregunta4 == 0 ||
      this.pregunta5 == 0 ||
      this.pregunta6 == 0 ||
      this.pregunta7 == 0 ||
      this.pregunta8 == 0 ||
      this.pregunta9 == 0 ||
      this.pregunta10 == 0 ||
      this.pregunta11 == 0 ||
      this.pregunta12 == 0 ||
      this.pregunta13 == 0) {
      this.toastr.error("Conteste todas las preguntas para poder avanzar", "Atención");
      return false;
    }
    return true;
  }
  validarVacioDiv05() {
    if (this.pregunta14 == 0 ||
      this.pregunta15 == 0 ||
      this.pregunta16 == 0 ||
      this.pregunta17 == 0 ||
      this.pregunta18 == 0 ||
      this.pregunta19 == 0 ||
      this.pregunta20 == 0 ||
      this.pregunta21 == 0 ||
      this.pregunta22 == 0) {
      this.toastr.error("Conteste todas las preguntas para poder avanzar", "Atención");
      return false;
    }
    return true;
  }
  validarVacioDiv06() {
    if (this.pregunta23 == 0 ||
      this.pregunta24 == 0 ||
      this.pregunta25 == 0 ||
      this.pregunta26 == 0 ||
      this.pregunta27 == 0 ||
      this.pregunta28 == 0 ||
      this.pregunta29 == 0 ||
      this.pregunta30 == 0 ||
      this.pregunta31 == 0 ||
      this.pregunta32 == 0 ||
      this.pregunta33 == 0 ||
      this.pregunta34 == 0 ||
      this.pregunta35 == 0 ||
      this.pregunta36 == 0 ||
      this.pregunta37 == 0 ||
      this.pregunta38 == 0 ||
      this.pregunta39 == 0 ||
      this.pregunta40 == 0) {
      this.toastr.error("Conteste todas las preguntas para poder avanzar", "Atención");
      return false;
    }
    return true;
  }
  validarVacioDiv07() {
    if (this.pregunta41 == 0 ||
      this.pregunta42 == 0 ||
      this.pregunta43 == 0) {
      this.toastr.error("Conteste todas las preguntas para poder avanzar", "Atención");
      return false;
    }
    return true;
  }
  validarVacioDiv09() {
    if (this.pregunta44 == 0 ||
      this.pregunta45 == 0 ||
      this.pregunta46 == 0) {
      return false;
    }
    return true;
  }
  btnSalir() {
    this.AppComponent.logout();
  }
  ajustarRespuestas() {
    if (this.pregunta100 == 6) {
      this.pregunta101 = 0;
      this.pregunta102 = 0;
      this.pregunta103 = 0;
      this.pregunta104 = 0;
      this.pregunta105 = 0;
      this.pregunta106 = 0;
      this.pregunta107 = 0;
      this.pregunta108 = 0;
      this.pregunta109 = 0;
      this.pregunta110 = 0;
      this.pregunta111 = 0;
      this.pregunta112 = 0;
      this.pregunta113 = 0;
      this.pregunta114 = 0;
    }
    if (this.pregunta115 == 6) {
      this.pregunta41 = 0;
      this.pregunta42 = 0;
      this.pregunta43 = 0;
    }
    if (this.pregunta116 == 6) {
      this.pregunta44 = 0;
      this.pregunta45 = 0;
      this.pregunta46 = 0;
    }
  }
}
