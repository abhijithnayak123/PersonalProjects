import { Component, OnInit } from '@angular/core';
import { Hero } from '../heroes/hero';
import { HEROES } from '../heroes/mock-heroes';
import { HeroService } from '../hero.service';

@Component({
  selector: 'app-heroes',
  templateUrl: './heroes.component.html',
  styleUrls: ['./heroes.component.css']
})
export class HeroesComponent implements OnInit {

 heroes:Hero[];
//  hero: Hero = {
//     id: 1,
//     name: 'Windstorm'
//   };

 selectedHero: Hero;
onSelect(hero: Hero): void {
  this.selectedHero = hero;
}

getHeroes(): void {
  this.heroService.getHeroes()
      .subscribe(heroes => this.heroes = heroes);
}

  constructor(private heroService: HeroService) { }

  ngOnInit() {
    this.getHeroes();
  }

}
