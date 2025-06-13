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

  view: any;
  routeLayer: any;

  ngOnInit() {
    const arcgisRequire = (window as any).require;
    arcgisRequire(
      ['esri/Map', 'esri/views/MapView', 'esri/Graphic'],
      (Map: any, MapView: any, Graphic: any) => {
        const map = new Map({ basemap: 'streets-navigation-vector' });
        this.view = new MapView({
          container: this.mapViewEl.nativeElement,
          map,
          center: [75.7873, 26.9124],
          zoom: 13,
        });

        navigator.geolocation.getCurrentPosition((pos) => {
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
        });
      }
    );
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.pickupCoords && this.dropoffCoords && this.view) {
      this.addPinsAndRoute();
    }
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
          geometry: this.pickupCoords,
          symbol: {
            type: 'picture-marker',
            url: 'https://cdn-icons-png.flaticon.com/512/684/684908.png',
            width: '30px',
            height: '30px',
          },
        });

        const dropoffPoint = new Graphic({
          geometry: this.dropoffCoords,
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
          stops: `{"features":[
        {"geometry":{"x":${this.pickupCoords.longitude},"y":${this.pickupCoords.latitude}}},
        {"geometry":{"x":${this.dropoffCoords.longitude},"y":${this.dropoffCoords.latitude}}}
      ]}`,
          f: 'json',
        };

        esriRequest(routeUrl, {
          query: params,
          responseType: 'json',
          method: 'post',
        }).then((response: any) => {
          const features = response.data.routes.features;
          if (features.length) {
            const routeGraphic = new Graphic({
              geometry: features[0].geometry,
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
}
