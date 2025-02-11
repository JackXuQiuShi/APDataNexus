import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { User } from 'src/assets/model/User';
import { Permission } from 'src/assets/model/Permission';
import { SharedService } from 'src/app/services/shared.service';
@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss'],
    animations: [
        trigger('disappearAnimation', [
            state('void', style({ opacity: 0, transform: 'scale(0.5)' })),
            transition(':enter, :leave', [
                animate('0.2s')
            ])
        ])
    ],
    standalone: false
})

export class SidebarComponent implements OnInit {

  isSidebarOpen: string = 'open';
  permission: Permission = new Permission;

  constructor(private sharedService: SharedService) {
    this.loadSidebarState();
    window.addEventListener('setEvent', () => {
      this.loadSidebarState();
    });
  }


  ngOnInit() {
    this.permission = this.sharedService.getPermission();
  }

  loadSidebarState() {
    const sidebarState = localStorage.getItem('sidebar');
    if (sidebarState) {
      this.isSidebarOpen = sidebarState;
    } else {
      this.isSidebarOpen = 'open';
      localStorage.setItem('sidebar', 'open');
    }
  }

}
