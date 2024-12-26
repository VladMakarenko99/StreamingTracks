export interface Soundtrack {
  id: string;
  title: string;
  lengthInSeconds: number
  albumCoverFileName: string;
  showPlayer: boolean;
  nextTrackId: string;
  prevTrackId: string;
}
