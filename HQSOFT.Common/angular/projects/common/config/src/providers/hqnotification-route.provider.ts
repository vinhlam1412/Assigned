import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { eCommonRouteNames } from '../enums/route-names';

export const HQNOTIFICATIONS_HQNOTIFICATION_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/common/hqnotifications',
        parentName: eCommonRouteNames.Common,
        name: 'Common::Menu:HQNotifications',
        layout: eLayoutType.application,
        requiredPolicy: 'Common.HQNotifications',
      },
    ]);
  };
}
