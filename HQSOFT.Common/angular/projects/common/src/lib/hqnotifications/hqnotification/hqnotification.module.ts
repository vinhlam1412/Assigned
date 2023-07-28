import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import {
  NgbCollapseModule,
  NgbDatepickerModule,
  NgbDropdownModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { PageModule } from '@abp/ng.components/page';
import { HQNotificationComponent } from './components/hqnotification.component';
import { HQNotificationRoutingModule } from './hqnotification-routing.module';

@NgModule({
  declarations: [HQNotificationComponent],
  imports: [
    HQNotificationRoutingModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    NgxValidateCoreModule,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbDropdownModule,

    PageModule,
  ],
})
export class HQNotificationModule {}

export function loadHQNotificationModuleAsChild() {
  return Promise.resolve(HQNotificationModule);
}
