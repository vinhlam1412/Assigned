import { ModuleWithProviders, NgModule } from '@angular/core';
import { COMMON_ROUTE_PROVIDERS } from './providers/route.provider';
import { HQTASKS_HQTASK_ROUTE_PROVIDER } from './providers/hqtask-route.provider';
import { HQASSIGNEDS_HQASSIGNED_ROUTE_PROVIDER } from './providers/hqassigned-route.provider';

@NgModule()
export class CommonConfigModule {
  static forRoot(): ModuleWithProviders<CommonConfigModule> {
    return {
      ngModule: CommonConfigModule,
      providers: [
        COMMON_ROUTE_PROVIDERS,
        HQTASKS_HQTASK_ROUTE_PROVIDER,
        HQASSIGNEDS_HQASSIGNED_ROUTE_PROVIDER,
      ],
    };
  }
}
