import { Component, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { NavItem } from 'src/app/service/user-model.service'; 
import { DataService } from 'src/app/service/data.service';
import { Router } from '@angular/router';
import Swal from "sweetalert2";




interface sidebarMenu {
  link: string;
  icon: string;
  menu: string;
}

@Component({
  selector: 'app-full',
  templateUrl: './full.component.html',
  styleUrls: ['./full.component.scss']
})
export class FullComponent implements OnInit{
  public userDisplayName = "";
  search: boolean = false;
  public userRole = "";
  items: any;
  navItems!: Array<NavItem>;
  userDetails: any;
  profileImageUrl = '';
  

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(private breakpointObserver: BreakpointObserver, private dataApi: DataService,
    private router: Router) { 
    this.navItems = this.dataApi.getUserMenu();
    this.userRole = this.dataApi.getUserRole();
    
    this.userDisplayName = this.dataApi?.getUserDisplayName();
    this.userDetails = this.dataApi.getUserDetail();
    
  }


  ngOnInit(): void {

    let profileImage = this.dataApi.getUserProfileImagePath();
    if (profileImage != null && profileImage != undefined) {
      this.profileImageUrl = 'http:\\\\103.240.90.141:6600\\emp\\'+profileImage ;
    }
    else {
      this.profileImageUrl = 'http:\\\\103.240.90.141:6600\\emp\\NoProfile.jpg';
    }


    this.items = this.navItems.map((x) => ({
      label: x.displayName,
      icon: x.menuIcon,
      items:
        x.children && x.children.length > 0
          ? x.children.map((child) => ({
              label: child.displayName,
              icon: child.menuIcon,
              routerLink: child.routingURL,
            }))
          : [],
      routerLink: x.routingURL,
    }));

  }

  routerActive: string = "activelink";

  sidebarMenu: sidebarMenu[] = [
    {
      link: "/home",
      icon: "home",
      menu: "Dashboard",
    },
   
  ]


  logout() {
    Swal.fire({
      title: "Logout",
      text: "Are you sure you want to logout?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, logout!",
    }).then((result) => {
      if (result.isConfirmed) {
        this.router.navigate(["/login"]);
      }
    });
  }

}
