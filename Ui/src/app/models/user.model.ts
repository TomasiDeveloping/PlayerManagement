export interface UserModel {
  id: string;
  playerName: string;
  email: string;
  role: string;
}

export interface LoggedInUser {
  id: string;
  allianceId: string;
  email: string;
  userName: string;
  allianceName: string;
}

export interface UpdateUserModel {
  id: string;
  playerName: string;
  role: string;
}
