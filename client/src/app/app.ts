import { Component, inject, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit{
  private http = inject(HttpClient);
  protected readonly title = 'Matchly';
  protected members = signal<any>([]);
  

  ngOnInit(): void {
    this.http.get<{ title: string }>('http://localhost:5001/api/members').subscribe({
      next: (data) => {
        this.members.set(data);
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
