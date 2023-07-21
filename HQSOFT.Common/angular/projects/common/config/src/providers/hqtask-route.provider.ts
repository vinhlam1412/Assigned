import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { eCommonRouteNames } from '../enums/route-names';

export const HQTASKS_HQTASK_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/common/hqtasks',
        parentName: eCommonRouteNames.Common,
        name: 'Common::Menu:HQTasks',
        layout: eLayoutType.application,
        requiredPolicy: 'Common.HQTasks',
      },
    ]);
  };
}
