import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title: string = "";
  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.title = 'Bitacora';
  }

}
