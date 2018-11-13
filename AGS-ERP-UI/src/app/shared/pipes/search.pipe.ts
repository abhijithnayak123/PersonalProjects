import {Component, NgModule, Pipe, PipeTransform } from '@angular/core'
import {BrowserModule} from '@angular/platform-browser'


@Pipe({
    name: 'searchPipe'
})
export class SearchPipe implements PipeTransform {
    transform(items: Array<any>, filter: {[key: string]: any }): Array<any> {
        return items.filter(item => {
                let notMatchingField = Object.keys(filter)
                                             .find(key =>item[key] !== filter[key] && filter[key] != null);
                return !notMatchingField; // true if matches all fields
            });
    }
}