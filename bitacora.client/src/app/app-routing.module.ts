import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { MenuComponent } from './components/menu/menu.component';
import { CambiocontraseniaComponent } from './components/cambiocontrasenia/cambiocontrasenia.component';
import { AuthGuardPermisos } from './guards/AuthGuardPermisos';
import { PermisosComponent } from './components/permisos/permisos.component';

const routes: Routes = [
  { path: '', component: LoginComponent, title: 'Bitácora' },
  {path:'bitacora/:usuario',component:MenuComponent,canActivate:[true]},
  { path: 'bitacora/:admin', component: MenuComponent, canActivate: [true] },
  { path: 'cambio-contraseña', component: CambiocontraseniaComponent },
  { path: 'administra-permisos', component: PermisosComponent, canActivate: [AuthGuardPermisos] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
