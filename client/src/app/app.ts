import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit{
  private http = inject(HttpClient);
  protected readonly title = signal('client');

  ngOnInit(): void {
    this.http.get<{ title: string }>('http://localhost:5001/api/members').subscribe({
      next: (data) => {
        this.title.set(data.title);
      },
      error: (err) => {
        console.error('Failed to fetch app info', err);
      },
      complete: () => {        
        console.log('App info fetch complete');
      }
    });
  }
  
}
