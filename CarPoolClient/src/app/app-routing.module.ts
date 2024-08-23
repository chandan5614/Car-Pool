import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { RideListingsComponent } from './components/ride-listings/ride-listings.component';
import { RideBookingComponent } from './components/ride-booking/ride-booking.component';
import { RatingSystemComponent } from './components/rating-system/rating-system.component';
import { NotificationsComponent } from './components/notifications/notifications.component';
import { HomepageComponent } from './components/homepage/homepage.component';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegistrationComponent },
  { path: 'profile', component: UserProfileComponent },
  { path: 'rides', component: RideListingsComponent },
  { path: 'book-ride', component: RideBookingComponent },
  { path: 'ratings', component: RatingSystemComponent },
  { path: 'notifications', component: NotificationsComponent },
  { path: 'home', component: HomepageComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
