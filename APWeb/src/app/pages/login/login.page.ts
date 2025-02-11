import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { ProgressService } from 'src/app/services/progress.service';
import { User } from 'src/assets/model/User';

@Component({
    selector: 'app-login',
    templateUrl: './login.page.html',
    styleUrls: ['./login.page.scss'],
    standalone: false
})
export class LoginPage implements OnInit {

  loading$ = this.progressService.loading$;

  constructor(private router: Router, private dataService: DataService, private progressService: ProgressService) {

  }

  ngOnInit() {
  }

  data: any;
  user: User = new User;
  IsAccountLogin = false;

  accountLogin() {
    sessionStorage.clear();
    this.dataService.accountLogin(this.user).subscribe({
      next: (data) => {
        sessionStorage.setItem('user', JSON.stringify(data));
        this.reset();
        this.router.navigate(['/home'])
      },
      error: (error) => {
        let errorMessage = error.message || 'accountLogin error occurred!';
        alert(errorMessage);
      }
    });
  }

  domainLogin() {
    sessionStorage.clear();
    this.dataService.domainLogin().subscribe({
      next: (data) => {
        this.data = data;
        sessionStorage.setItem('user', JSON.stringify(data));
        this.reset();
        this.router.navigate(['/home'])
      },
      error: (error) => {
        let errorMessage = error.message || 'domainLogin error occurred!';
        alert(errorMessage);
      }
    });
  }

  setStoreID(){

  }

  reset(){
    this.user = new User;
  }

  


}