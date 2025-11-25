import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterCreds } from '../../../types/user';
import { AccountService } from '../../../core/service/account-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private accountService = inject(AccountService);
  cancleRegister = output<boolean>();
  protected creds = { } as RegisterCreds;
  register(){
  this.accountService.register(this.creds).subscribe({
    next: response=>{
      console.log(response);
      this.cancel();
    },
    error: error => {
      console.log(error);
    }
  });
  }

  cancel(){
    this.cancleRegister.emit(false);
  }
}
