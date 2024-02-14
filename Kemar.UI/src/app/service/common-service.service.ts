import { Injectable } from '@angular/core';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';

@Injectable({
  providedIn: 'root'
})
export class CommonServiceService {
  notificationCount = '0';

  constructor(private KemarService: KemarServiceService) { }

  getActiveNotificationCount(){
    this.KemarService.get<any>(null,ServiceUrl.getMyActiveNotificationCount).subscribe(
      res=>{
        this.notificationCount = res;
      },
      r=>{
        this.notificationCount = '0';
      });
  }
}
