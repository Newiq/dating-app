import { Component, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../../core/service/member-service';
import { ActivatedRoute } from '@angular/router';
import { Member, Photo } from '../../../types/member';
import { ImageUopload } from "../../../shared/image-uopload/image-uopload";
import { AccountService } from '../../../core/service/account-service';
import { User } from '../../../types/user';
import { DynamicBtn } from "../../../shared/dynamic-btn/dynamic-btn";
import { NotExpr } from '@angular/compiler';
import { DeleteBtn } from "../../../shared/delete-btn/delete-btn";

@Component({
  selector: 'app-member-photos',
  imports: [ImageUopload, DynamicBtn, DeleteBtn],
  templateUrl: './member-photos.html',
  styleUrl: './member-photos.css',
})
export class MemberPhotos implements OnInit{
  protected memberService = inject(MemberService);
  protected accountService = inject(AccountService);
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
setMainPhoto(photo:Photo){
  this.memberService.setMainPhoto(photo).subscribe({
    next:()=>{
      const currentUser = this.accountService.currentUser();
      if(currentUser) currentUser.imageUrl = photo.url;
      this.accountService.setCurrentUser(currentUser as User);
      this.memberService.member.update(member=>({
        ...member,
        imageUrl:photo.url
      })as Member)
    }
  })
}

deletePhoto(photoId:number){
  this.memberService.deletePhoto(photoId).subscribe({
    next: ()=>{
      this.photos.update(photos=>photos.filter(x=>x.id!==photoId))
      
    }}
  )
}
}


