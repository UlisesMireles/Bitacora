import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSlider } from '@angular/material/slider';
import { MatSortModule } from '@angular/material/sort';
import { MatTabsModule } from '@angular/material/tabs';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastNoAnimationModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './components/menu/menu.component';
import { RegistrosCalendarioComponent } from './components/registros-calendario/registros-calendario.component';
//import { CatalogosComponent, ModalCambioEstatus, ModalReestablecerContrasenia, ModalEliminarRol } from './components/catalogos/catalogos.component';
import { ReportesComponent } from './components/reportes/reportes.component';
//import { AvanceRealComponent } from './components/avance-real/avance-real.component';
import { CambiocontraseniaComponent } from './components/cambiocontrasenia/cambiocontrasenia.component';
import { TokenContraseniaComponent } from './components/token-contrasenia/token-contrasenia.component';
//import { FiltroPipe } from './pipes/filtro.pipe';
import {DialogTable} from './components/reportes/reportes.component';
import {DialogTable2} from './components/reportes/reportes.component';
import {DialogTable3} from './components/reportes/reportes.component';
//import { AgregarUsuarioComponent } from './components/agregar-usuario/agregar-usuario.component';
import { FiltroBitacoraPipe } from './pipes/filtro-bitacora.pipe';
//import { RegistrosCalendarioComponent } from './components/registros-calendario/registros-calendario.component';
import { CatalogosComponent } from './components/catalogos/catalogos.component';
//import { AvanceRealComponent } from './components/avance-real/avance-real.component';
//import { CambiocontraseniaComponent } from './components/cambiocontrasenia/cambiocontrasenia.component';
//import { TokenContraseniaComponent } from './components/token-contrasenia/token-contrasenia.component';
import { FiltroPipe } from './pipes/filtro.pipe';
//import { FiltroBitacoraPipe } from './pipes/filtro-bitacora.pipe';
//import {MatCheckboxModule} from '@angular/material/checkbox';
import { FiltroProyectosPipe } from './pipes/filtro-proyectos.pipe';
import { AuthGuardBitacora } from './guards/AuthGuardBitacora';
import { PermisosComponent } from './components/permisos/permisos.component';
import { AuthGuardCatalogos } from './guards/AuthGuardCatalogos';
import { AuthGuardAvanceReal } from './guards/AuthGuardAvanceReal';
//import { AuthGuardPermisos } from './guards/AuthGuardPermisos';
//import { OlvidasteContraseniaComponent } from './components/olvidaste-contrasenia/olvidaste-contrasenia.component';
import { Nom035Component } from './components/nom035/nom035.component';
import { AuthGuard } from './guards/AuthGuard';
//import { Nom035Component } from './components/nom035/nom035.component';
//import { AuthGuard } from './guards/AuthGuard';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { AgregarUsuarioComponent } from './components/agregar-usuario/agregar-usuario.component';
import { LoginComponent } from './components/login/login.component';
import { AvanceRealComponent } from './components/avance-real/avance-real.component'
import { FormBitacoraComponent } from './components/form-bitacora/form-bitacora.component'
import { ToastrModule } from 'ngx-toastr';
import { AuthGuardPermisos } from './guards/AuthGuardPermisos';
import { AuthGuardReportes } from './guards/AuthGuardReportes';
import { OlvidasteContraseniaComponent } from './components/olvidaste-contrasenia/olvidaste-contrasenia.component';
export function getBaseUrl() {
  return 'https://localhost:7127/'
  //return document.getElementsByTagName('base')[0].href;
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AvanceRealComponent,
    FormBitacoraComponent,
    RegistrosCalendarioComponent,
    MenuComponent,
    ReportesComponent,
    DialogTable,
    DialogTable2,
    DialogTable3,
    CambiocontraseniaComponent,
    PermisosComponent,
    RegistrosCalendarioComponent,
    TokenContraseniaComponent,
    Nom035Component,
    FiltroBitacoraPipe,
    CatalogosComponent,
    AgregarUsuarioComponent,
    FiltroPipe,
    FiltroProyectosPipe,
    OlvidasteContraseniaComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatTabsModule,
    BrowserAnimationsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatCardModule,
    MatMenuModule,
    NgxSpinnerModule,
    MatButtonModule,
    NgxPaginationModule,
    MatDialogModule,
    MatCheckboxModule,
    MatRadioModule,
    NgxMatSelectSearchModule,
    MatSidenavModule,
    MatSlider,
    MatProgressSpinnerModule,
    MatSlideToggleModule,
    MatExpansionModule,
    MatSortModule,
    MatProgressBarModule,
    MatTooltipModule,
    ToastNoAnimationModule.forRoot({
      timeOut: 2000,
      positionClass: 'toast-top-center',
      preventDuplicates: true,
      closeButton: false
      // autoDismiss:false,
      // closeButton:true,
      // disableTimeOut:true,
    }),
    ToastrModule.forRoot({
      timeOut: 3000, // Duración de la notificación en milisegundos
      positionClass: 'toast-center-center', // Posición de las notificaciones
      preventDuplicates: true, // Evita duplicados
    }), // Configura Toastr
    MatDialogModule

  ],
  providers: [
    { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },AuthGuardBitacora, AuthGuardPermisos, AuthGuard, AuthGuardReportes, AuthGuardCatalogos, AuthGuardAvanceReal,
    provideAnimationsAsync()
  ],
  //  entryComponents: [DialogTable, DialogTable2, DialogTable3, CatalogosComponent, AgregarUsuarioComponent, Modal, OlvidasteContraseniaComponent, ModalCambioEstatus, ModalReestablecerContrasenia, ModalEliminarRol],

  //providers: [{ provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }, AuthGuardBitacora, AuthGuardCatalogos, AuthGuardReportes, AuthGuardAvanceReal, AuthGuardAvanceReal, AuthGuardPermisos, AuthGuard],

  bootstrap: [AppComponent]
})
export class AppModule { }
