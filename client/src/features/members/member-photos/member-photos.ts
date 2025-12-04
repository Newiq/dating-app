import { Component, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../../core/service/member-service';
import { ActivatedRoute } from '@angular/router';
import { Photo } from '../../../types/member';
import { ImageUopload } from "../../../shared/image-uopload/image-uopload";

@Component({
  selector: 'app-member-photos',
  imports: [ImageUopload],
  templateUrl: './member-photos.html',
  styleUrl: './member-photos.css',
})
export class MemberPhotos implements OnInit{
  protected memberService = inject(MemberService);
  private route = inject(ActivatedRoute);
  protected photos = signal<Photo[]>([]);
  protected loading = signal(false);

  ngOnInit(): void {
    const memberId = this.route.parent?.snapshot.paramMap.get('id');
    if(memberId){
      this.memberService.getMemberPhotos(memberId).subscribe({
        next:photos=>this.photos.set(photos)
      });
    }
  }
onUploadImage(file:File){
  this.loading.set(true);
  this.memberService.uploadPhoto(file).subscribe({
    next:photo=>{
      this.memberService.editMode.set(false);
      this.loading.set(false);
      this.photos.update(photos=>[...photos,photo])
    },
    error:error=>{
      console.log('Error uploading image:',error);
      this.loading.set(false);

    }
  })
}
}
