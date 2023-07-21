import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44324/',
  redirectUri: baseUrl,
  clientId: 'Common_App',
  responseType: 'code',
  scope: 'offline_access Common',
  requireHttps: true
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Common',
    logoUrl: '',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44324',
      rootNamespace: 'HQSOFT.Common',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
    Common: {
      url: 'https://localhost:44373',
      rootNamespace: 'HQSOFT.Common',
    },
  },
} as Environment;
