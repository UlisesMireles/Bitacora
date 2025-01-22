import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { MenuComponent } from './components/menu/menu.component';
import { ReportesComponent } from './components/reportes/reportes.component';

const routes: Routes = [
  { path: '', component: LoginComponent, title: 'Bitácora' },
  {path:'bitacora/:usuario',component:MenuComponent,canActivate:[true]},
  {path:'bitacora/:admin',component: MenuComponent,canActivate:[true]},
  {path:'reportes/:reporte',component: ReportesComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
