import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-dynamic-btn',
  imports: [],
  templateUrl: './dynamic-btn.html',
  styleUrl: './dynamic-btn.css',
})
export class DynamicBtn {
 disabled = input<boolean>();
 selected = input<boolean>();
 clickEvent = output<Event>();


 onClick(evet:Event){
  this.clickEvent.emit(evet);
 }
}
