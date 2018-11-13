import { NgModule } from "@angular/core";
import { DropDownListModule } from "@progress/kendo-angular-dropdowns";
import { FormsModule } from '@angular/forms';

import { LanguageComponent } from './components/language/language.component';
import { LanguageService } from './services/language.service';
import { UserManagementModule } from '../user-management/user-management.module';
import { TranslationPipe } from "./pipes/translation.pipe";
import { AlertComponent } from './components/alert/alert.component';
import { ConfirmationComponent } from "./components/confirmation/confirmation.component";
import { SuccessComponent } from "./components/success/success.component";
import { DialogModule } from "@progress/kendo-angular-dialog";
import { CommonModule } from '@angular/common'
import { ErrorComponent } from "./components/error/error.component";
import { SpinnerComponent } from './components/spinner/spinner.component';
import { ToastComponent } from './components/toast/toast.component';
import { NumberDirective } from "./directives/number.directive";
import { SortPipe } from './pipes/sort.pipe';
import { SearchPipe} from './pipes/search.pipe';
import { UniquePipe} from './pipes/unique.pipe';
import { GroupByThreeDirective } from './directives/group-by-three.directive';
import { OPTDropDownComponent,TrackScrollDirective } from "./components/opt-combobox/opteamix-dropdown.component";
import { DateFormatPipe } from "./pipes/date.pipe";
import { UnderConstructionComponent } from './components/under-construction/under-construction.component';
import { SessionStorageService } from "./wrappers/session-storage.service";
import { AlphaNumericDirective } from './directives/alpha-numeric.directive';
import {AlphaDirective} from './directives/alpha.directive'
import {AlphaCapitalDirective} from './directives/alphaCapital.directive'
import { RoundUpPipe } from './pipes/round-up.pipe';
import { CurrencyDirective } from './directives/currency.directive';
import { VendorOfContainerPipe } from './pipes/vendor-of-container.pipe';

@NgModule({
    imports: [
        DropDownListModule,
        FormsModule,
        DialogModule,
        CommonModule
    ],
    declarations: [
        LanguageComponent,
        TranslationPipe,
        AlertComponent,
        ConfirmationComponent,
        SuccessComponent,
        ErrorComponent,
        SpinnerComponent,
        ToastComponent,
        NumberDirective,
        SortPipe,
        UniquePipe,
        SearchPipe,
        GroupByThreeDirective,
        OPTDropDownComponent,
        TrackScrollDirective,
        DateFormatPipe,
        UnderConstructionComponent,
        AlphaNumericDirective,
        AlphaDirective,
        AlphaCapitalDirective,
        RoundUpPipe,
        CurrencyDirective,
        VendorOfContainerPipe
    ],
    providers: [LanguageService, SessionStorageService, SearchPipe, SortPipe,UniquePipe,VendorOfContainerPipe],
    exports: [
        LanguageComponent,
        TranslationPipe,
        ConfirmationComponent,
        AlertComponent,
        SuccessComponent,
        ErrorComponent,
        SpinnerComponent,
        ToastComponent,
        NumberDirective,
        SortPipe,
        UniquePipe,
        SearchPipe,
        GroupByThreeDirective,
        OPTDropDownComponent,
        DateFormatPipe,
        UnderConstructionComponent,
        AlphaNumericDirective,
        AlphaDirective,
        AlphaCapitalDirective,
        RoundUpPipe,
        CurrencyDirective,
        VendorOfContainerPipe
    ]
})

export class SharedModule { }
