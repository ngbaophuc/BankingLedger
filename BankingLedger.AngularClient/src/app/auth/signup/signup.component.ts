import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AccountService } from 'src/app/account.service';
import { Router } from '@angular/router';

@Component({
	selector: 'app-signup',
	templateUrl: './signup.component.html',
	styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
	signUpForm: FormGroup;
	private usernameTaken: string = '';

	constructor(private service: AccountService, private router: Router) { }

	ngOnInit() {
		this.signUpForm = new FormGroup({
			'username': new FormControl(null, [Validators.required, this.usernameAvailableValidator.bind(this)]),
			'firstName': new FormControl(null),
			'lastName': new FormControl(null),
			'password': new FormControl(null, Validators.required),
			'confirmPassword': new FormControl(null, [Validators.required, this.confirmPasswordValidator.bind(this)])
		});
	}

	onUsernameKeyUp(e) {

		if (this.usernameTaken == '') return;

		if (this.usernameTaken != e.target.value) {
			this.usernameTaken = '';
		}
	}

	onSubmit() {
		const signUpInfo = this.signUpForm.value;

		this.service.signUp(signUpInfo).subscribe(_ => {
			this.signUpForm.reset();

			this.service.signIn(signUpInfo).subscribe(res => {
				this.service.setToken(res['token']);
				this.router.navigate(['/']);
			});

		}, err => {
			if (err.status === 400) {
				this.signUpForm.controls.username.setErrors({
					'usernameTaken': true
				});

				this.usernameTaken = signUpInfo.username;
			}
		});

	}

	usernameAvailableValidator(control: FormControl): { [key: string]: boolean } {
		if (this.usernameTaken != '') {
			return { 'usernameTaken': true };
		}

		return null;
	}

	confirmPasswordValidator(control: FormControl): { [key: string]: boolean } {
		if (!this.signUpForm) return null;

		if (control.value && this.signUpForm.controls.confirmPassword.value != this.signUpForm.controls.password.value) {
			return { 'confirmPassMismatched': true };
		}

		return null;
	}
}
