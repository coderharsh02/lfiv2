import { Member } from "./member";

export interface Donation {
    donationId: number;
    noOfMeals: number;
    status: string;
    feedbackByDonor: string;
    feedbackByCollector: string;
    Donor: Member;
    Collector?: Member;
}