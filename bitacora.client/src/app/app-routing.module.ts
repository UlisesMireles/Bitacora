import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { MenuComponent } from './components/menu/menu.component';
import { AvanceRealComponent } from './components/avance-real/avance-real.component'
import { FormBitacoraComponent } from './components/form-bitacora/form-bitacora.component'

const routes: Routes = [
  { path: '', component: FormBitacoraComponent, title: 'Bitácora' },
  { path: 'bitacora/:usuario', component: MenuComponent, canActivate: [true] },
  { path: 'bitacora/:admin', component: MenuComponent, canActivate: [true] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
