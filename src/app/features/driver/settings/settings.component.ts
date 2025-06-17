import { Component, OnInit } from '@angular/core';
import { DriverService } from 'src/app/core/services/driver.service';
import { SignalrService } from 'src/app/core/services/signalrService.service';
import { DriverSettings } from 'src/app/shared/types/driverSettings.type';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
})
export class DriverSettingsComponent implements OnInit {
  driver!: DriverSettings;
  constructor(private driverService: DriverService,private signalrService:SignalrService) {}

  ngOnInit(): void {
    this.driverService.getDriverDetails().subscribe((data: DriverSettings) => {
      this.driver = data;
    });
  }

  onStatusToggle(): void {
    const newStatus = this.driver.status === 'Available' ? 'Unavailable' : 'Available';
    this.signalrService.updateStatus(newStatus)
    this.driver.status=newStatus
  }
}
