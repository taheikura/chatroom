import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ApolloModule, APOLLO_OPTIONS } from 'apollo-angular';
import { HttpLinkModule, HttpLink } from 'apollo-angular-link-http';
import { InMemoryCache } from 'apollo-cache-inmemory';

import { AppComponent } from './app.component';

@NgModule({
  imports: [BrowserModule, HttpClientModule, ApolloModule, HttpLinkModule],
  providers: [{
    provide: APOLLO_OPTIONS,
    useFactory(httpLink: HttpLink) {
      return {
        cache: new InMemoryCache(),
        link: httpLink.create({
          uri: `http://13.48.45.151:5000/graphql`
        })
      }
    },
    deps: [HttpLink]
  }],
  declarations: [AppComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
