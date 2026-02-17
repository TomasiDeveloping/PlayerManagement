import { provideZoneChangeDetection } from "@angular/core";

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';


import { ModuleRegistry, AllCommunityModule } from 'ag-charts-community';
ModuleRegistry.registerModules([AllCommunityModule]);

platformBrowserDynamic()
  .bootstrapModule(AppModule, {
    applicationProviders: [provideZoneChangeDetection()],
  })
  .catch(err => console.error(err));
