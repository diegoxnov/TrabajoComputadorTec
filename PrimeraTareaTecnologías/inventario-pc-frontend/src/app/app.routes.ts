import { Routes } from '@angular/router';
import { ProductoLista } from './components/producto-lista/producto-lista';
import { ProductoForm } from './components/producto-form/producto-form';
import { ArmarPc } from './components/armar-pc/armar-pc';
import { ArmadosHistorial } from './components/armados-historial/armados-historial';

export const routes: Routes = [
  { path: '', redirectTo: 'productos', pathMatch: 'full' },
  { path: 'productos', component: ProductoLista },
  { path: 'productos/nuevo', component: ProductoForm },
  { path: 'productos/:id/editar', component: ProductoForm },
  { path: 'armar', component: ArmarPc },
  { path: 'armados', component: ArmadosHistorial },
];
