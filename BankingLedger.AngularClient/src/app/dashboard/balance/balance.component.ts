import { Component, OnInit, Input } from '@angular/core';
import { AccountService } from 'src/app/account.service';

@Component({
  selector: 'app-balance',
  templateUrl: './balance.component.html',
  styleUrls: ['./balance.component.css']
})
export class BalanceComponent implements OnInit {

  @Input() balance = 0.00;

  constructor(private service: AccountService) { }

  ngOnInit() {
    this.service.getBalance().subscribe(res => {
      this.balance = res['balance'];
    });
  }

}
