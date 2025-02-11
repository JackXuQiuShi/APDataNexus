import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { ProgressService } from 'src/app/services/progress.service';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.scss'],
    standalone: false
})
export class NavbarComponent implements OnInit {

  loading$ = this.progressService.loading$;

  constructor(private router: Router, private dataService: DataService, private progressService: ProgressService) { }


  ngOnInit() {

  }

  logout() {
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }

  sidebar() {
    const currentSidebarState = localStorage.getItem('sidebar');
    if (currentSidebarState === 'close') {
      localStorage.setItem('sidebar', 'open');
    } else {
      localStorage.setItem('sidebar', 'close');
    }
    const setEvent = new Event('setEvent');
    window.dispatchEvent(setEvent);
  }

  back() {
    window.history.back();
  }


}
