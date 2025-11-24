import { inject,Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../../types/user';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  currentUser = signal<User|null>(null);

  baseUrl ='http://localhost:5001/api/';

  login(creds:any){
    return this.http.post(this.baseUrl + 'account/login', creds).pipe(
      tap(user=>{
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user as User);
        }
        
      })
    )
  }
  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}



