import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AccountService } from 'src/app/account.service';
import { Router } from '@angular/router';

@Component({
	selector: 'app-signin',
	templateUrl: './signin.component.html',
	styleUrls: ['./signin.component.css']
})
export class SigninComponent implements OnInit {
	errorMessage: string;

	constructor(private service: AccountService, private router: Router) { }

	ngOnInit() {
	}

	onSubmit(form: NgForm) {
		const signInInfo = form.value;

		this.service.signIn(signInInfo).subscribe(res => {
			this.service.setToken(res['token']);
			this.router.navigate(['/']);
			form.reset();

		}, err => {
			if (err.status === 401) {
				this.errorMessage = 'Invalid username or password.'
			}
		});
	}
}
