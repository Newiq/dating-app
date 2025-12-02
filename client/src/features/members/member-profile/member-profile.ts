import { Component, inject, OnDestroy, OnInit, signal, ViewChild, viewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EditableMember, Member } from '../../../types/member';
import { DatePipe } from '@angular/common';
import { MemberService } from '../../../core/service/member-service';
import { NgForm } from '@angular/forms';
import { ToastService } from '../../../core/service/toast-service';

@Component({
  selector: 'app-member-profile',
  imports: [DatePipe],
  templateUrl: './member-profile.html',
  styleUrl: './member-profile.css',
})
export class MemberProfile implements OnInit, OnDestroy{

  @ViewChild('editForm') editForm?:NgForm;
  private toast = inject(ToastService);
  protected memberService = inject(MemberService);
  private route =  inject(ActivatedRoute);
  protected member = signal<Member|undefined>(undefined);
  protected editableMember?: EditableMember;

  ngOnInit(): void {
    this.route.parent?.data.subscribe(data=>{
      this.member.set(data['member']);
    })
    this.editableMember = {
      userName : this.member()?.userName ||'',
      description : this.member()?.description ||'',
      city : this.member()?.city ||'',
      country : this.member()?.country ||'',
    }
  }
  updateProfile(){
    if(!this.member()) return;
    const updateMember = {...this.member(), ...this.editableMember}
    console.log(updateMember);
    this.toast.success('Profile updated successfully');
    this.memberService.editMode.set(false);
  }

    ngOnDestroy(): void {
    if(this.memberService.editMode()){
      this.memberService.editMode.set(false);
    }
  }
}
