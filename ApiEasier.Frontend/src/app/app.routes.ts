import { Routes } from '@angular/router';
import { CardApiListComponent } from './data/pages/card-api-list/card-api-list.component';
import { EntityCardListComponent } from './data/pages/entity-card-list/entity-card-list.component';
import { EndpointCardListComponent } from './data/pages/endpoints-page/endpoin-card-list/endpoint-card-list.component';
import { PageNotFoundComponent } from './data/pages/page-not-found/page-not-found.component';
import { ApiEndpointListComponent } from './data/pages/api-endpoint-list/api-endpoint-list.component';
import { ErrorDisplayComponent } from './data/pages/error-display/error-display.component';

export const routes: Routes = [
  {
    path: '',
    component: CardApiListComponent,
    title: 'Home page',
  },
  {
    path: 'ApiService/:name',
    component: EntityCardListComponent,
    title: 'Api details',
  },
  {
    path: 'ApiEntity/:apiServiceName/:entityName',
    component: EndpointCardListComponent,
    title: 'Entity details',
  },
  {
    path: 'ApiEndpoint/:name',
    component: ApiEndpointListComponent,
    title: 'copy url for APIEmu',
  },
  {
    path: 'page-not-found',
    component: PageNotFoundComponent,
    title: 'Page Not Found',
  },
  {
    path: 'error',
    component: ErrorDisplayComponent,
    title: 'An error occurred',
  },
  { path: '**', redirectTo: 'page-not-found', pathMatch: 'full' },
];
