import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  // any type available
  values: any;

  // make sure to import HttpClient from angular and NOT SELENIUM
  constructor(private http: HttpClient) { }

  // ComponentDidMount Equivalent
  ngOnInit() {
    this.getValues();
  }

  getValues() {
    // Returns an observable, got to subscribe
    this.http.get('http://localhost:5000/api/values/1').subscribe(response => {
      this.values = response;
    }, error => {
      console.log(error);
    });
  }
}
