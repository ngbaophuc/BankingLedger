import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  onDeposit = new Subject<{amount: number}>();
  onWithdraw = new Subject<{amount: number}>();

  constructor(private http: HttpClient) { }

  deposit(depositInfo: { amount: number }) {
    return this.http.post(environment.apiUrl + 'transaction/deposit', depositInfo);
  }

  withdraw(withdrawInfo: { amount: number }) {
    return this.http.post(environment.apiUrl + 'transaction/withdraw', withdrawInfo);
  }

  fetchRecentTransactions(fetchTransactionsInfo: { quantity: number }) {
    return this.http.get(environment.apiUrl + `transaction/transactions?order=desc&orderby=date&pagesize=${fetchTransactionsInfo.quantity}&pagenum=1`);
  }
}
