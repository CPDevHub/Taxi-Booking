import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';

import { ToastrService } from 'ngx-toastr';
import { MapComponent } from 'src/app/core/components/map/map.component';
import { RideService } from 'src/app/core/services/ride.service';
import { SignalrService } from 'src/app/core/services/signalrService.service';
import { VehicleType } from 'src/app/shared/enums/vehicleType.enums';
import { rideAcceptType } from 'src/app/shared/types/rideAccept.type';
import { RideBookResponseType } from 'src/app/shared/types/rideBookResponse.type';


@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements OnInit {
  pickupSuggestions: any[] = [];
  dropoffSuggestions: any[] = [];
  carTypes = Object.values(VehicleType);
  selectedCarType: VehicleType = this.carTypes[0];

  pickupLocation: any = null;
  dropOffLocation: any = null;
  rideResponse!: RideBookResponseType;
  rideCompleted: boolean = false;
  
  @Input() rideDetails: rideAcceptType | null = null;
  @Output() cancelRideModal = new EventEmitter<void>();

  @Output() pickupSelected = new EventEmitter<any>();
  @Output() dropoffSelected = new EventEmitter<any>();
  @Output() completeAndSubmit = new EventEmitter();

  constructor(
    private rideService: RideService,
    private signalrService: SignalrService,
    private toaster: ToastrService
  ) {}

  ngOnInit() {
    console.log('ride details:', this.rideDetails);
    this.signalrService.rideCompleted$.subscribe(() => {
      this.rideCompleted = true;
      console.log(this.rideCompleted);
    });
  }

  private fetchSuggestions(query: string, callback: (results: any[]) => void) {
    const arcgisRequire = (window as any).require;

    arcgisRequire(['esri/rest/locator'], (locator: any) => {
      const geocodeUrl =
        'https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer';

      locator
        .addressToLocations(geocodeUrl, {
          address: {
            SingleLine: query,
          },
          outFields: ['*'],
          maxLocations: 5,
        })
        .then((results: any[]) => {
          const suggestions = results.map((result: any) => ({
            text: result.address,
            coords: result.location,
          }));
          console.log(suggestions);
          callback(suggestions);
        })
        .catch((error: any) => {
          console.error('Geocoding failed:', error);
        });
    });
  }

  onPickupChange(value: string) {
    this.fetchSuggestions(value, (suggestions) => {
      this.pickupSuggestions = suggestions;
    });
  }

  onDropoffChange(value: string) {
    this.fetchSuggestions(value, (suggestions) => {
      this.dropoffSuggestions = suggestions;
    });
  }

  onPickupSelect(item: any) {
    if (item && item.coords) {
      this.pickupLocation = {
        latitude: item.coords.latitude,
        longitude: item.coords.longitude,
        address: item.text,
      };
      this.pickupSelected.emit(item.coords);
    } else {
      const found = this.pickupSuggestions.find((s) => s.text === item);
      if (found) {
        this.pickupLocation = {
          latitude: item.coords.latitude,
          longitude: item.coords.longitude,
          address: item.text,
        };
        this.pickupSelected.emit(found.coords);
      }
    }
  }

  onDropoffSelect(item: any) {
    if (item && item.coords) {
      this.dropOffLocation = {
        latitude: item.coords.latitude,
        longitude: item.coords.longitude,
        address: item.text,
      };
      this.dropoffSelected.emit(item.coords);
    } else {
      const found = this.dropoffSuggestions.find((s) => s.text === item);
      if (found) {
        this.dropOffLocation = {
          latitude: item.coords.latitude,
          longitude: item.coords.longitude,
          address: item.text,
        };
        this.dropoffSelected.emit(found.coords);
      }
    }
  }

  bookRide() {
    this.rideCompleted=false
    if (
      this.pickupLocation.latitude == this.dropOffLocation.latitude &&
      this.pickupLocation.latitude == this.pickupLocation.longitude
    ) {
      this.toaster.warning('Pickup and DropOff location not be same');
      return;
    }
    this.rideService
      .bookRide({
        pickupLocation: this.pickupLocation,
        dropOffLocation: this.dropOffLocation,
        rideVehicle: this.selectedCarType,
      })
      .subscribe((data: RideBookResponseType) => {
        this.rideResponse = data;
      });
  }

  onCancelRide() {
    this.cancelRideModal.emit();
  }

  cancelRideBeforeAcceptance() {
    this.signalrService.cancelRideBeforeAcceptance(this.rideResponse.rideId);
  }

  ratingSubmitted() {
    this.completeAndSubmit.emit();
  }
}
