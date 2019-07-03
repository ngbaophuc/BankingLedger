import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { TransactionService } from 'src/app/transaction.service';
import Autonumeric from 'autonumeric';

@Component({
  selector: 'app-deposit',
  templateUrl: './deposit.component.html',
  styleUrls: ['./deposit.component.css']
})
export class DepositComponent implements OnInit {
  depositForm: FormGroup;
  @Output() onDeposited = new EventEmitter();
  amountInvalid = false;

  constructor(private service: TransactionService) { }

  ngOnInit() {
    this.depositForm = new FormGroup({
      'amount': new FormControl(null)
    });

    new Autonumeric('#depositAmount');
  }
  onSubmit() {
    const depositInfo = this.depositForm.value;

    this.service.deposit(depositInfo).subscribe(_ => {
      this.onDeposited.emit(depositInfo);
      this.service.onDeposit.next(depositInfo);
      this.depositForm.reset();
      this.amountInvalid = false;
    }, err => {
      if (err.status === 400) this.amountInvalid = true;
    });
  }
}
