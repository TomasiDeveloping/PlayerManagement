export interface ZombieSiegeParticipantModel {
  id: string;
  playerId: string;
  zombieSiegeId: string;
  survivedWaves: number;
  playerName: string;
}

export interface CreateZombieSiegeParticipantModel {
  playerId: string;
  zombieSiegeId: string;
  survivedWaves: number;
}
