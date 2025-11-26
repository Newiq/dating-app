import { Component, inject, OnInit } from '@angular/core';
import { ApiError } from '../../../types/error';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-server-error',
  imports: [],
  templateUrl: './server-error.html',
  styleUrl: './server-error.css',
})
export class ServerError implements OnInit {
  protected error: ApiError | null = null;
  private router = inject(Router);
  protected showDetails = false;
  private location = inject(Location);

  ngOnInit() {
    const navigation = this.router.currentNavigation();
    console.log('Navigation:', navigation); 
    
    this.error = navigation?.extras?.state?.['error'];
    console.log('Error object:', this.error); 
    
    if (!this.error) {
      const navigationState = (this.router as any).getCurrentNavigation?.();
      this.error = navigationState?.extras?.state?.['error'];
    }
  }

  detailsToggled() {
    this.showDetails = !this.showDetails;
  }

  goHome() {
    this.location.back();
  }
}