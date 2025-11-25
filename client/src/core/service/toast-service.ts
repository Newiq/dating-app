import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  constructor() {
    this.createToastContainer();
  }
  private createToastContainer() {
    if (!document.getElementById('toast-container')) {
      const container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'toast toast-bottom toast-end';
      container.style.position = 'fixed';
      container.style.bottom = '20px';
      container.style.right = '20px';
      container.style.zIndex = '9999';
      document.body.appendChild(container);
    }
  }
  private createToastElement(message: string, alertClass:string, duration: number = 5000) {
    const toastContainer = document.getElementById('toast-container')!.appendChild(document.createElement('div')) as HTMLDivElement;
    if(!toastContainer) return;

    const toast = document.createElement('div');
    toast.classList.add('alert', alertClass,'shadow-lg');
    toast.innerHTML = `
      <div>
        <span>${message}</span>
        <button class="btn btn-sm btn-ghost">✖</button>
      </div>
    `;
    const btn = toast.querySelector('button');
    btn?.addEventListener('click', () => {
      toast.remove();
    });
    toastContainer.append(toast);
    setTimeout(() => {
      if(toastContainer.contains(toast)){
        toastContainer.removeChild(toast);
      }
    }, duration);

  }

  success(message: string, duration?: number) {
    this.createToastElement(message, 'alert-success', duration);
  }
  error(message: string, duration?: number) {
    this.createToastElement(message, 'alert-error', duration);
  }
  warning(message: string, duration?: number) {
    this.createToastElement(message, 'alert-warning', duration);
  }
  info(message: string, duration?: number) {
    this.createToastElement(message, 'alert-info', duration);
  }
  
}
