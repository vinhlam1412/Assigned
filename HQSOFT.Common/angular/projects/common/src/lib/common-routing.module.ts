import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonComponent } from './components/common.component';
import { loadHQTaskModuleAsChild } from './hqtasks/hqtask/hqtask.module';
import { loadHQAssignedModuleAsChild } from './hqassigneds/hqassigned/hqassigned.module';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: CommonComponent,
  },
  { path: 'hqtasks', loadChildren: loadHQTaskModuleAsChild },
  { path: 'hqassigneds', loadChildren: loadHQAssignedModuleAsChild },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CommonRoutingModule {}
