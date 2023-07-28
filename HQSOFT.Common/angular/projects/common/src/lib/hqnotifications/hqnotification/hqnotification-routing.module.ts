import { AuthGuard, PermissionGuard } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HQNotificationComponent } from './components/hqnotification.component';

const routes: Routes = [
  {
    path: '',
    component: HQNotificationComponent,
    canActivate: [AuthGuard, PermissionGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HQNotificationRoutingModule {}
