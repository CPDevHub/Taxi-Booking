import { NgModule } from '@angular/core';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { FloatingLabelModule } from '@progress/kendo-angular-label';
import {
  StackLayoutModule,
  CardModule,
  GridLayoutModule,
} from '@progress/kendo-angular-layout';
import {
  ComboBoxModule,
  DropDownListModule,
} from '@progress/kendo-angular-dropdowns';
import { IconsModule } from '@progress/kendo-angular-icons';
import { AppBarModule } from '@progress/kendo-angular-navigation';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { GridModule } from '@progress/kendo-angular-grid';
import { CommonModule } from '@angular/common';
import { RideHistoryComponent } from '../core/pages/history/history.component';
import { MapComponent } from '../core/components/map/map.component';
import { ModalComponent } from '../core/components/modal/modal.component';
import { RatingComponent } from '../core/components/rating/rating.component';
import { LoaderComponent } from '../core/components/loader/loader.component';
import { IndicatorsModule } from '@progress/kendo-angular-indicators';

@NgModule({
  declarations: [
    RideHistoryComponent,
    MapComponent,
    ModalComponent,
    RatingComponent,
  ],
  imports: [
    CommonModule,
    StackLayoutModule,
    ButtonsModule,
    InputsModule,
    CardModule,
    FloatingLabelModule,
    DropDownListModule,
    IconsModule,
    ComboBoxModule,
    DialogModule,
    AppBarModule,
    GridModule,
    IndicatorsModule,
  ],
  exports: [
    StackLayoutModule,
    ButtonsModule,
    InputsModule,
    CardModule,
    FloatingLabelModule,
    DropDownListModule,
    IconsModule,
    ComboBoxModule,
    DialogModule,
    AppBarModule,
    IndicatorsModule,
    MapComponent,
    GridModule,
    RideHistoryComponent,
    ModalComponent,
    RatingComponent,
  ],
})
export class SharedModule {}
