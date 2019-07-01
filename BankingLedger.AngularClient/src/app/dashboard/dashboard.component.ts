import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  balance = 0;

  constructor(private service: AccountService) { }

  ngOnInit() {
  }

  deposited() {
    this.service.getBalance().subscribe(res => {
      this.balance = res['balance'];
    }, err => {

    });
  }

  withdrawed() {
    this.service.getBalance().subscribe(res => {
      this.balance = res['balance'];
    }, err => {

    });
  }
}
