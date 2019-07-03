import { Component, OnInit, OnDestroy } from '@angular/core';
import { TransactionService } from 'src/app/transaction.service';
import { Subscription } from 'rxjs';
import * as moment from 'moment';
import * as numeral from 'numeral';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit, OnDestroy {
  transactions: any[] = [];
  
  depositSub: Subscription;
  withdrawSub: Subscription;

  constructor(private service: TransactionService) { }

  ngOnInit() {
    this.fetchData();
    this.depositSub = this.service.onDeposit.subscribe(_ => this.fetchData());
    this.withdrawSub = this.service.onWithdraw.subscribe(_ => this.fetchData());
  }

  ngOnDestroy() {
    this.depositSub.unsubscribe();
    this.withdrawSub.unsubscribe();
  }

  fetchData() {
    this.service.fetchRecentTransactions({quantity: 20})
    .subscribe(res => {
      this.transactions = res['transactions']
        .map((t: { dateTime: moment.MomentInput; amount: number; }) => ({
          date: moment(t.dateTime).format('MMM DD, YYYY'), 
          time: moment(t.dateTime).format('HH:mm:ss'),
          transaction: t.amount < 0 ? 'Withdraw' : 'Deposit ', 
          amount: numeral(Math.abs(t.amount)).format('0,0.00')}));
    });
  }
}
