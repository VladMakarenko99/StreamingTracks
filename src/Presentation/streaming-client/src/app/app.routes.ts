import { Routes } from '@angular/router';
import { SignInComponent } from './components/sign-in/sign-in.component';
import { HomeComponent } from './components/home/home.component';
import {SoundtrackComponent} from "./components/soundtrack/soundtrack.component";

export const routes: Routes = [
    { path: 'sign-in', component: SignInComponent },
    { path: 'soundtrack/:id', component: SoundtrackComponent },
    { path: '', component: HomeComponent },
];
