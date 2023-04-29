import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { DonationsService } from '../_services/donations.service';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-donate-form',
  templateUrl: './donate-form.component.html',
  styleUrls: ['./donate-form.component.css'],
})
export class DonateFormComponent implements OnInit {
  member: any;

  constructor(
    private donationService: DonationsService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  hour: number = 1;
  model: any = {
    noOfMeals: 3,
    pickUpTimeFrom: new Date().toISOString(),
  };

  donateNow() {
    let date = new Date(this.model.pickUpTimeFrom);
    date.setHours(date.getHours() + this.hour);
    this.model.pickUpTimeTo = date;
    this.donationService.donateNow(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/collect');
      },
      error: (error) => {
        this.toastr.error(error.error);
        console.log(error);
      },
    });
  }
}
