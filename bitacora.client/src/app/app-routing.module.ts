import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { MenuComponent } from './components/menu/menu.component';
import { CambiocontraseniaComponent } from './components/cambiocontrasenia/cambiocontrasenia.component';
import { AuthGuardPermisos } from './guards/AuthGuardPermisos';
import { PermisosComponent } from './components/permisos/permisos.component';
import { TokenContraseniaComponent } from './components/token-contrasenia/token-contrasenia.component';
import { Nom035Component } from './components/nom035/nom035.component';
import { AuthGuard } from './guards/AuthGuard';
import { ReportesComponent } from './components/reportes/reportes.component';
import { AvanceRealComponent } from './components/avance-real/avance-real.component'
import { FormBitacoraComponent } from './components/form-bitacora/form-bitacora.component'

const routes: Routes = [
  { path: '', component: LoginComponent, title: 'Bitácora' },
  { path:'bitacora/:usuario',component:MenuComponent},
  { path: 'bitacora/:admin', component: MenuComponent },
  { path: 'cambio-contraseña', component: CambiocontraseniaComponent },
  { path: 'administra-permisos', component: PermisosComponent, canActivate: [AuthGuardPermisos] },
  { path:'ingresa-token', component: TokenContraseniaComponent},
  { path: 'nom035', component: Nom035Component, canActivate:[AuthGuard]},
  {path:'reportes/:reporte',component: ReportesComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
