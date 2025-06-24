import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PassengerRoutingModule } from './passenger-routing.module';
import { PassengerLayoutComponent } from './passenger-layout/passenger-layout.component';
import { PassengerHomeComponent } from './home/home.component';
import { PassengerSettingsComponent } from './settings/settings.component';

import { SidebarComponent } from './home/sidebar/sidebar.component';

import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule } from '@angular/forms';
import { PassengerMapComponent } from './passenger-map/passenger-map.component';


@NgModule({
  declarations: [
    PassengerLayoutComponent,
    PassengerHomeComponent,
    PassengerSettingsComponent,
    SidebarComponent,
    PassengerMapComponent,
  ],
  imports: [CommonModule, PassengerRoutingModule, SharedModule,FormsModule],
})
export class PassengerModule {}
