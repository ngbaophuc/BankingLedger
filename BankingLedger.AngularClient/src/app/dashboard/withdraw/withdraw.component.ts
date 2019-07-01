import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { TransactionService } from 'src/app/transaction.service';

@Component({
  selector: 'app-withdraw',
  templateUrl: './withdraw.component.html',
  styleUrls: ['./withdraw.component.css']
})
export class WithdrawComponent implements OnInit {
  withdrawForm: FormGroup;
  @Output() onWithdrawed = new EventEmitter();
  amountInvalid = false;

  constructor(private service: TransactionService) { }

  ngOnInit() {
    this.withdrawForm = new FormGroup({
      'amount': new FormControl(null)
    });
  }

  onSubmit() {
    const withdrawInfo = this.withdrawForm.value;

    this.service.withdraw(withdrawInfo).subscribe(_ => {
      this.onWithdrawed.emit(withdrawInfo);
      this.service.onWithdraw.next(withdrawInfo);
      this.withdrawForm.reset();
      this.amountInvalid = false;
    }, err => {
      if (err.status === 400) this.amountInvalid = true;
    });
  }
}
