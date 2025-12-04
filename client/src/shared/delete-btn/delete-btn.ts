import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-delete-btn',
  imports: [],
  templateUrl: './delete-btn.html',
  styleUrl: './delete-btn.css',
})
export class DeleteBtn {

  disabled = input<boolean>();
  clickEvent = output<Event>();

  onClick(event:Event){
    this.clickEvent.emit(event);
  }

}
