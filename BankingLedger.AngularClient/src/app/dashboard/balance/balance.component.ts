import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { AccountService } from 'src/app/account.service';
import * as numeral from 'numeral';

@Component({
  selector: 'app-balance',
  templateUrl: './balance.component.html',
  styleUrls: ['./balance.component.css']
})
export class BalanceComponent implements OnInit, OnChanges {
  @Input() balance: string;

  constructor(private service: AccountService) { }

  ngOnInit() {
    this.service.getBalance().subscribe(res => {
      this.balance = numeral(res['balance']).format('0,0.00');
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.balance = numeral(changes.balance.currentValue).format('0,0.00');
  }
}
