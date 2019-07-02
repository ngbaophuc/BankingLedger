import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

import { AccountService } from '../account.service';
import { Subscription } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
	selector: 'app-header',
	templateUrl: './header.component.html',
	styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {
	title = 'Banking Ledger';
	authenticated: boolean = false;
	name: string;
	private userSub: Subscription;

	constructor(private service: AccountService, private router: Router) { }

	ngOnInit() {

		this.userSub = this.service.userProfile.subscribe(user => {
			this.authenticated = !!user;

			if (this.authenticated)
				this.name = `${user['firstName']} ${user['lastName']}`;
		});
	}

	ngOnDestroy(): void {
		if (this.userSub)
			this.userSub.unsubscribe();
	}

	signOut() {
		this.service.removeToken();
		this.router.navigate(['/auth']);
	}
}
