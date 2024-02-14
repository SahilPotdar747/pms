import { style } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { faDisplay } from '@fortawesome/free-solid-svg-icons';
import { truncateSync } from 'fs';
import { MessageService } from 'primeng/api';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';
import { CommonServiceService } from 'src/app/service/common-service.service';


@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent {
  loading = false;
  submitted!: boolean;
  notificationRecieved: boolean = true;
  notification: any;
  notificationid: number = 0;
  isStop: boolean = true;

  constructor(
    private KemarService: KemarServiceService,
    private message: MessageService,
    private commonService: CommonServiceService
  ) {
    this.KemarService.isLoggedIn$ = true;
  }

  ngOnInit(): void {
    this.getAllNotification();
    this.commonService.getActiveNotificationCount();
    setInterval(() => {
      this.getCurrentNotification();
    }, 10000);
  }
  ShowMessage(messageType: string, title: string, message: string) {
    this.message.add({
      severity: messageType,
      summary: title,
      detail: message,
    });
  }

  getAllNotification() {
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getMyNotification).subscribe(
      (res) => {
        this.notification = res;
      },
      (r) => {
        this.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  getCurrentNotification() {
    this.loading = true;
    var listCount = 0;
    if (this.notification != null && this.notification.length > 0) {
      listCount = this.notification.length;
    }
    var query = {
      count: listCount,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.getCurrentNotification,
      query
    ).subscribe(
      (res) => {
        if (res.length > 0) {
          res.forEach((value: any) => {
            this.notification.unshift(value);
          });
          this.commonService.getActiveNotificationCount();
        }
      },
      (r) => {
        this.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  closeNotification(notificationid: number) {
    this.KemarService.delete<any>(
      notificationid,
      ServiceUrl.closeNotificationByUser,
      'delete'
    ).subscribe(
      (res) => {
        this.commonService.getActiveNotificationCount();
      },
      (r) => {
        this.commonService.getActiveNotificationCount();
      }
    );
  }

  closeAllNotification() {
    this.KemarService.delete<any>(null,ServiceUrl.CloseAllNotificationByUser,'delete').subscribe(
      (res) => {
        this.getAllNotification();
        this.commonService.getActiveNotificationCount();
      },
      (r) => {
        this.getAllNotification();
        this.commonService.getActiveNotificationCount();
      }
    );
  }
}
