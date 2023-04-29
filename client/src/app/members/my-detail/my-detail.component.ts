import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { DonationsService } from 'src/app/_services/donations.service';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { FeedbackDialogBoxComponent } from '../../feedback-dialog-box/feedback-dialog-box.component';
import { ConfirmationDialog } from '../../confirm/confirm.component';

@Component({
  selector: 'app-my-detail',
  templateUrl: './my-detail.component.html',
  styleUrls: ['./my-detail.component.css'],
})
export class MyDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
  member: Member = {} as Member;
  donations: any;
  collections: any;
  activeTab?: TabDirective;
  messages: Message[] = [];
  user?: User;

  constructor(
    private memberService: MembersService,
    private accountService: AccountService,
    private route: ActivatedRoute,
    private messageService: MessageService,
    public presenceService: PresenceService,
    private donationService: DonationsService,
    private toastr: ToastrService,
    private router: Router,
    public dialog: MatDialog
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) this.user = user;
      },
    });
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  loadMember() {
    var username = this.user?.username;
    if (!username) return;
    this.memberService.getMember(username).subscribe({
      next: (member) => {
        this.member = member;
        console.log(this.member);
      },
    });
  }

  loadDonations() {
    this.donationService.getCurrentUserDonation().subscribe({
      next: (donations) => {
        this.donations = donations;
        console.log(this.donations);
      },
    });
  }

  loadCollections() {
    this.donationService.getCurrentUserCollection().subscribe({
      next: (collections) => {
        this.collections = collections;
        console.log(this.collections);
      },
    });
  }

  ngOnInit(): void {
    this.loadMember();
    this.loadDonations();
    this.loadCollections();

    this.route.data.subscribe({
      next: (data) => (this.member = data['member']),
    });

    this.route.queryParams.subscribe({
      next: (params) => {
        params['tab'] && this.selectTab(params['tab']);
      },
    });
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      this.memberTabs.tabs.find((x) => x.heading === heading)!.active = true;
    }
  }

  loadMessages() {
    if (this.member) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: (messages) => (this.messages = messages),
      });
    }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.user) {
      this.messageService.createHubConnection(this.user, this.member.userName);
    } else {
      this.messageService.stopHubConnection();
    }
  }

  eventEmitted(donation: any): void {
    if (donation.status === 'Donated') {
      this.toastr.error('This donation is already donated!');
    } else if (donation.status === 'Booked') {
      const confirmRef = this.dialog.open(ConfirmationDialog, {
        data: {
          message: 'Are you sure you have collected this donation?',
        },
      });
      confirmRef.afterClosed().subscribe((result) => {
        if (result) {
          var updatedDonation = {
            id: donation.donationId,
            noOfMeals: donation.noOfMeals,
            status: donation.status,
            feedbackByDonor:
              donation.feedbackByDonor +
              '<p><strong>Status(Collected): </strong>Collector collected the food</p>',
            feedbackByCollector:
              donation.feedbackByCollector +
              '<p><strong>Status(Collected): </strong>On the way to donate the food</p>',
            donorId: donation.donorId,
          };
          this.donationService.updateToCollected(updatedDonation).subscribe({
            next: () => {
              this.toastr.success('Collected Successfully!');
              this.loadMember();
            },
            error: (error) => {
              this.toastr.error(error.error);
              this.loadMember();
              console.log(error);
            },
          });
        }
      });
    } else if (donation.status === 'Collected') {
      const confirmRef = this.dialog.open(ConfirmationDialog, {
        data: {
          message: 'Are you sure you have donated this donation?',
        },
      });
      confirmRef.afterClosed().subscribe((result) => {
        if (result) {
          var feedback = 'Food donated to the needy!';
          const dialogRef = this.dialog.open(FeedbackDialogBoxComponent, {
            data: {
              question: 'Please enter some message about donation..',
              answer: feedback,
            },
          });

          dialogRef.afterClosed().subscribe((result) => {
            feedback = result;
            var updatedDonation = {
              id: donation.donationId,
              noOfMeals: donation.noOfMeals,
              status: donation.status,
              feedbackByDonor:
                donation.feedbackByDonor +
                '<p><strong>Status(Donated): </strong>Collector coming to collect the food</p>',
              feedbackByCollector:
                donation.feedbackByCollector +
                '<p><strong>Status(Donated): </strong>' +
                feedback +
                '</p>',
              donorId: donation.donorId,
            };

            this.donationService.updateToDonated(updatedDonation).subscribe({
              next: () => {
                this.toastr.success('Donated Successfully!');
                this.loadMember();
              },
              error: (error) => {
                this.toastr.error(error.error);
                this.loadMember();
                console.log(error);
              },
            });
          });
        }
      });
    }
  }
}
