export interface Member {
    id: number;
    userName: string;
    name: string;
    donorType: string;
    volunteerType: string;
    addressLine1: string;
    addressLine2: string;
    city: string;
    photoUrl: string;
    pincode: number;
    phoneNumber: string;
    memberSince: Date;
    lastActive: Date;
    description: string;
    lifeGoals: string;
}