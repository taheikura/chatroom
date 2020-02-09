import { Component, OnInit } from '@angular/core';
import gql from 'graphql-tag';
import {Apollo} from 'apollo-angular';

const MsgQuery = gql`{messages {groupName userName postedAt msg}}`;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'chat';
  msgs: any[];
  loading = true;
  error: any;
  
  constructor(private apollo: Apollo) {}

  ngOnInit() {
    this.apollo
      .watchQuery({
        query: MsgQuery,
      })
      .valueChanges.subscribe((result: any) => {
        this.msgs = result.data && result.data.messages;
        this.loading = result.loading;
        this.error = result.error;
      });
  }
}
