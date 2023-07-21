import { AuthGuard, PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HQAssignedComponent } from './components/hqassigned.component';

const routes: Routes = [
  {
    path: '',
    component: HQAssignedComponent,
    canActivate: [AuthGuard, PermissionGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HQAssignedRoutingModule {}
