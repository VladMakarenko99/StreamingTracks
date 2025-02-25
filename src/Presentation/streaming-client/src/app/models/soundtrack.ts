export interface Soundtrack {
  id: string;
  title: string;
  lengthInSeconds: number
  slug: string;
  nextTrackSlug: string;
  prevTrackSlug: string;
  musicFileUrl: string;
  albumCoverUrl: string;
  listenings: number;
}
