import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from 'src/services/api.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements OnInit {
  logoutForm: FormGroup;
  isLoadingResults = false;

  constructor(
    private router: Router,
    private api: ApiService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.logoutForm = this.formBuilder.group({});
  }

  logout() {
    localStorage.removeItem("jwt");
    this.isLoadingResults = true;
    this.router.navigate(['/login']);
  }

}
