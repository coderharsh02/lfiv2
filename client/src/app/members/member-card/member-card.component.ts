import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import {
  faCity,
  faUser,
  faDonate,
  faStar,
} from '@fortawesome/free-solid-svg-icons';
import { faStar as farStar } from '@fortawesome/free-regular-svg-icons';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
})
export class MemberCardComponent implements OnInit {
  @Input() member: Member | undefined;

  noOfFilledStars: Array<Number> = [];
  noOfUnfilledStars: Array<Number> = [];

  faCity = faCity;
  faUser = faUser;
  faDonate = faDonate;
  faStar = faStar;
  farStar = farStar;

  constructor(
    private memberService: MembersService,
    private toastr: ToastrService,
    public presenceService: PresenceService
  ) {
    let random = Math.random() * 5;
    for (let index = 0; index < random; index++) {
      this.noOfFilledStars.push(index);
    }
    for (let index = 0; index < 4 - random; index++) {
      this.noOfUnfilledStars.push(index);
    }
  }

  ngOnInit(): void {}

  addLike(member: Member) {
    this.memberService.addLike(member.userName).subscribe({
      next: () => this.toastr.success('You have liked ' + member.name),
    });
  }
}
