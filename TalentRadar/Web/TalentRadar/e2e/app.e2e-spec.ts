import { TalentRadarPage } from './app.po';

describe('talent-radar App', () => {
  let page: TalentRadarPage;

  beforeEach(() => {
    page = new TalentRadarPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
