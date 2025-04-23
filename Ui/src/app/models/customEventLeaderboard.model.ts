export interface LeaderboardPointEventModel {
  playerName: string;
  points: number;
}

export interface LeaderboardParticipationEventModel {
  playerName: string;
  participations: number;
}

export interface LeaderboardPointAndParticipationEventModel {
  playerName: string;
  participations: number;
  points: number;
  totalPoints: number;
}
