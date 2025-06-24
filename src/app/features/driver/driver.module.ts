import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DriverRoutingModule } from './driver-routing.module';
import { DriverHomeComponent } from './home/home.component';
import { DriverSettingsComponent } from './settings/settings.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DriverLayoutComponent } from './driver-layout/driver-layout.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { DriverMapComponent } from './driver-map/driver-map.component';

@NgModule({
  declarations: [
    DriverHomeComponent,
    DriverSettingsComponent,
    DashboardComponent,
    DriverLayoutComponent,
    DriverMapComponent,
  ],
  imports: [CommonModule, DriverRoutingModule, SharedModule],
})
export class DriverModule {}
