import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Console } from 'console';
import { RideService } from 'src/app/core/services/ride.service';
import { VehicleType } from 'src/app/shared/enums/vehicleType.enums';
import { rideAcceptType } from 'src/app/shared/types/rideAccept.type';
import { rideDetailsType } from 'src/app/shared/types/rideDetails.type';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements OnInit{
  pickupSuggestions: any[] = [];
  dropoffSuggestions: any[] = [];
  carTypes = Object.values(VehicleType);
  selectedCarType: VehicleType = this.carTypes[0];

  pickupLocation: any = null;
  dropOffLocation: any = null;

  @Input() rideDetails:rideAcceptType | null=null;
  @Output() cancelRideModal=new EventEmitter<void>;

  @Output() pickupSelected = new EventEmitter<any>();
  @Output() dropoffSelected = new EventEmitter<any>();

  constructor(private rideService: RideService) {}

  ngOnInit(){
    console.log("ride details:",this.rideDetails);
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
    this.rideService
      .bookRide({
        pickupLocation: this.pickupLocation,
        dropOffLocation: this.dropOffLocation,
        rideVehicle:this.selectedCarType
      })
      .subscribe(() => {});
  }

  onCancelRide(){
    this.cancelRideModal.emit();
  }
}
