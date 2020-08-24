import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TeamComponent } from './team/team.component';
import { ApiDemoComponent } from './api/api-demo.component';
import { IntroductionComponent } from './introduction/introduction.component';
import { DebugComponent } from './debug/debug.component';


const routes: Routes = [
  { path: 'intro', component: IntroductionComponent },
  { path: 'team', component: TeamComponent },
  { path: 'team/:name', component: TeamComponent },
  { path: 'api', component: ApiDemoComponent },
  { path: 'debug', component: DebugComponent },
  { path: '**', redirectTo: '/intro' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
