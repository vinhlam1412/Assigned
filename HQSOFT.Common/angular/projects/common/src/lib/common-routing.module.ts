import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonComponent } from './components/common.component';
import { loadHQTaskModuleAsChild } from './hqtasks/hqtask/hqtask.module';
import { loadHQAssignedModuleAsChild } from './hqassigneds/hqassigned/hqassigned.module';
import { loadHQShareModuleAsChild } from './hqshares/hqshare/hqshare.module';
import { loadHQNotificationModuleAsChild } from './hqnotifications/hqnotification/hqnotification.module';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: CommonComponent,
  },
  { path: 'hqtasks', loadChildren: loadHQTaskModuleAsChild },
  { path: 'hqassigneds', loadChildren: loadHQAssignedModuleAsChild },
  { path: 'hqshares', loadChildren: loadHQShareModuleAsChild },
  { path: 'hqnotifications', loadChildren: loadHQNotificationModuleAsChild },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CommonRoutingModule {}
