export interface AllianceAccessTokenModel {
  id: string;
  allianceId: string;
  token: string;
  fullShareUrl: string;
  expiresAt?: Date;
  isActive: boolean;
  createdAt: Date;
}
