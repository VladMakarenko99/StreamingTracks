export interface Soundtrack {
  id: string;
  title: string;
  lengthInSeconds: number
  albumCoverFileName: string;
  slug: string;
  nextTrackSlug: string;
  prevTrackSlug: string;
}
