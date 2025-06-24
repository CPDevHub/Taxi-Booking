import {
  Component,
  ElementRef,
  Input,
  OnChanges,
  SimpleChanges,
  ViewChild,
} from '@angular/core';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css'],
})
export class MapComponent implements OnChanges {
  @ViewChild('mapViewNode', { static: true }) mapViewEl!: ElementRef;
  @Input() pickupCoords: any;
  @Input() dropoffCoords: any;
  @Input() rideStarted: boolean = false;

  private _driverCoords: any;
  isLoading:Boolean=true

  view: any;
  routeLayer: any;
  driverRouteLayer: any;
  driverGraphic: any;

  pickupToDropoffRouteDrawn = false;

  @Input()
  set driverCoords(value: any) {
    this._driverCoords = value;

    if (this._driverCoords && this.pickupCoords && this.view) {
      if (!this.driverGraphic) {
        this.addDriverToPickupRoute(); // draw once
      } else {
        this.updateDriverMarker(); // move marker only
      }
    } else if (!this._driverCoords && this.driverRouteLayer) {
      this.driverRouteLayer.removeAll();
      this.driverGraphic = null;
    }
  }

  get driverCoords(): any {
    return this._driverCoords;
  }

  ngOnInit() {
    const arcgisRequire = (window as any).require;
    arcgisRequire(
      ['esri/Map', 'esri/views/MapView', 'esri/Graphic'],
      (Map: any, MapView: any, Graphic: any) => {
        const map = new Map({ basemap: 'streets-navigation-vector' });

        this.view = new MapView({
          container: this.mapViewEl.nativeElement,
          map,
          zoom: 15,
        });

        this.view.ui.components = [];

        navigator.geolocation.getCurrentPosition((pos) => {
          this.view.center = {
            longitude: pos.coords.longitude,
            latitude: pos.coords.latitude,
          };

          const userGraphic = new Graphic({
            geometry: {
              type: 'point',
              longitude: pos.coords.longitude,
              latitude: pos.coords.latitude,
            },
            symbol: {
              type: 'picture-marker',
              url: 'https://cdn-icons-png.flaticon.com/512/684/684908.png',
              width: '30px',
              height: '30px',
            },
          });

          this.view.graphics.add(userGraphic);
          this.isLoading=false;
        });
      }
    );
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      this.pickupCoords &&
      this.dropoffCoords &&
      this.view &&
      !this.pickupToDropoffRouteDrawn
    ) {
      this.addPinsAndRoute();
      // this.pickupToDropoffRouteDrawn = true;
    }

    if (
      changes['rideStarted'] &&
      changes['rideStarted'].currentValue === true
    ) {
      this.clearDriverToPickupRoute();
    }
  }

  clearDriverToPickupRoute() {
    this.driverRouteLayer?.removeAll();
    this.driverGraphic = null;
  }

  addPinsAndRoute() {
    const arcgisRequire = (window as any).require;
    arcgisRequire(
      ['esri/Graphic', 'esri/layers/GraphicsLayer', 'esri/request'],
      (Graphic: any, GraphicsLayer: any, esriRequest: any) => {
        if (!this.routeLayer) {
          this.routeLayer = new GraphicsLayer();
          this.view.map.add(this.routeLayer);
        }
        this.routeLayer.removeAll();

        const pickupPoint = new Graphic({
          geometry: {
            type: 'point',
            longitude: this.pickupCoords.longitude,
            latitude: this.pickupCoords.latitude,
          },
          symbol: {
            type: 'picture-marker',
            url: 'https://cdn-icons-png.flaticon.com/512/684/684908.png',
            width: '30px',
            height: '30px',
          },
        });

        const dropoffPoint = new Graphic({
          geometry: {
            type: 'point',
            longitude: this.dropoffCoords.longitude,
            latitude: this.dropoffCoords.latitude,
          },
          symbol: {
            type: 'picture-marker',
            url: 'https://cdn-icons-png.flaticon.com/512/684/684908.png',
            width: '30px',
            height: '30px',
          },
        });

        this.routeLayer.addMany([pickupPoint, dropoffPoint]);

        const routeUrl =
          'https://route-api.arcgis.com/arcgis/rest/services/World/Route/NAServer/Route_World/solve';

        const params: any = {
          stops: JSON.stringify({
            features: [
              {
                geometry: {
                  x: this.pickupCoords.longitude,
                  y: this.pickupCoords.latitude,
                },
              },
              {
                geometry: {
                  x: this.dropoffCoords.longitude,
                  y: this.dropoffCoords.latitude,
                },
              },
            ],
          }),
          f: 'json',
          token:
            'AAPTxy8BH1VEsoebNVZXo8HurHaR50a1_97u0-hAt3_ovHrfT-byZXpAx1e6HChPcal5rujCZN98h2Ora1CTml61SQAyPkaNXG-6oIhYS_au-93XIZbkTEmLlldcR3_V2QAz7Dil9SMqA_oQBg9yHqCqvTxVuo7PwysjqTV5hyU0T-ax4AYbOpahxyZw6QJuKxgdC2NF0pSdKxBpQbH-wyzpUKbr8XFxEMsbb6tHWsOzwGc.AT1_8LUhHhDW',
        };

        esriRequest(routeUrl, {
          query: params,
          responseType: 'json',
          method: 'post',
        }).then((response: any) => {
          const features = response.data.routes.features;
          if (features.length) {
            const routeGraphic = new Graphic({
              geometry: {
                type: 'polyline',
                paths: features[0].geometry.paths,
              },
              symbol: {
                type: 'simple-line',
                color: [0, 0, 255, 0.8],
                width: 4,
              },
            });
            this.routeLayer.add(routeGraphic);
          }
        });

        this.view.goTo({
          center: [this.pickupCoords.longitude, this.pickupCoords.latitude],
          zoom: 13,
        });
      }
    );
  }

  addDriverToPickupRoute() {
    const arcgisRequire = (window as any).require;
    arcgisRequire(
      ['esri/Graphic', 'esri/layers/GraphicsLayer', 'esri/request'],
      (Graphic: any, GraphicsLayer: any, esriRequest: any) => {
        if (!this.driverRouteLayer) {
          this.driverRouteLayer = new GraphicsLayer();
          this.view.map.add(this.driverRouteLayer);
        }

        this.driverRouteLayer.removeAll();

        this.driverGraphic = new Graphic({
          geometry: {
            type: 'point',
            longitude: this.driverCoords.longitude,
            latitude: this.driverCoords.latitude,
          },
          symbol: {
            type: 'picture-marker',
            url: 'https://cdn-icons-png.flaticon.com/512/2202/2202112.png',
            width: '30px',
            height: '30px',
          },
        });

        this.driverRouteLayer.add(this.driverGraphic);

        const routeUrl =
          'https://route-api.arcgis.com/arcgis/rest/services/World/Route/NAServer/Route_World/solve';

        const params: any = {
          stops: JSON.stringify({
            features: [
              {
                geometry: {
                  x: this.driverCoords.longitude,
                  y: this.driverCoords.latitude,
                },
              },
              {
                geometry: {
                  x: this.pickupCoords.longitude,
                  y: this.pickupCoords.latitude,
                },
              },
            ],
          }),
          f: 'json',
          token:
            'AAPTxy8BH1VEsoebNVZXo8HurHaR50a1_97u0-hAt3_ovHrfT-byZXpAx1e6HChPcal5rujCZN98h2Ora1CTml61SQAyPkaNXG-6oIhYS_au-93XIZbkTEmLlldcR3_V2QAz7Dil9SMqA_oQBg9yHqCqvTxVuo7PwysjqTV5hyU0T-ax4AYbOpahxyZw6QJuKxgdC2NF0pSdKxBpQbH-wyzpUKbr8XFxEMsbb6tHWsOzwGc.AT1_8LUhHhDW',
        };

        esriRequest(routeUrl, {
          query: params,
          responseType: 'json',
          method: 'post',
        }).then((response: any) => {
          const features = response.data.routes.features;
          if (features.length) {
            const routeGraphic = new Graphic({
              geometry: {
                type: 'polyline',
                paths: features[0].geometry.paths,
              },
              symbol: {
                type: 'simple-line',
                color: [255, 165, 0, 0.8],
                width: 3,
              },
            });
            this.driverRouteLayer.add(routeGraphic);
          }
        });
      }
    );
  }

  updateDriverMarker() {
    if (this.driverGraphic) {
      this.driverGraphic.geometry.latitude = this.driverCoords.latitude;
      this.driverGraphic.geometry.longitude = this.driverCoords.longitude;
    }
  }

  resetMap() {
    this.pickupToDropoffRouteDrawn = false;
    this.routeLayer?.removeAll();
    this.driverRouteLayer?.removeAll();
    this.driverGraphic = null;
  }
}
