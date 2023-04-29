import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { DonationsService } from '../_services/donations.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  topDonors: any;
  topCollectors: any;
  constructor(
    public accountService: AccountService,
    private donationService: DonationsService
  ) {}

  ngOnInit(): void {
    this.loadTopDonorsCollectors();
  }

  loadTopDonorsCollectors() {
    this.donationService.getTopDonorCollector().subscribe((response) => {
      this.topDonors = response.topDonors;
      this.topCollectors = response.topCollectors;
    });
  }
}
