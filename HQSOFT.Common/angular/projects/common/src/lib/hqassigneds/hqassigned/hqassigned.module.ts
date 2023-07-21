import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import {
  NgbCollapseModule,
  NgbDatepickerModule,
  NgbDropdownModule,
  NgbNavModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { PageModule } from '@abp/ng.components/page';
import { HQAssignedComponent } from './components/hqassigned.component';
import { HQAssignedRoutingModule } from './hqassigned-routing.module';

@NgModule({
  declarations: [HQAssignedComponent],
  imports: [
    HQAssignedRoutingModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    NgxValidateCoreModule,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbDropdownModule,
    NgbNavModule,
    PageModule,
  ],
})
export class HQAssignedModule {}

export function loadHQAssignedModuleAsChild() {
  return Promise.resolve(HQAssignedModule);
}
