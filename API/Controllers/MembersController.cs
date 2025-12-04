using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{

    public class MembersController(IMemberRepository memberRepository,
    IPhotoService photoService) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
            return Ok(await memberRepository.GetMembersAllAsync());
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var user = await memberRepository.GetMemberByIdAsync(id);
            if (user == null) 
            {
                return NotFound();
            }
            return user;
        }
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            var photos = await memberRepository.GetPhotosAsync(id);
            return Ok(photos);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var memberId = User.GetMemberId();
            var member = await memberRepository.GetMemberForUpdate(memberId);
            if(member == null) return BadRequest("Could not get member");
            member.UserName = memberUpdateDto.UserName??member.UserName;
            member.Description = memberUpdateDto.Description??member.Description;
            member.City = memberUpdateDto.City??member.City;
            member.Country = memberUpdateDto.Country??member.Country;
            member.User.UserName = memberUpdateDto.UserName??member.User.UserName;
            memberRepository.Update(member);
            if(await memberRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update Users");

        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm]IFormFile file)
        {
            var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());
            if(member == null) return BadRequest("Cannot update member");

            var result = await photoService.UploadImageAsync(file);
            if(result.Error!= null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                MemberId = User.GetMemberId()
            };
            if(member.ImageUrl == null)
            {
                member.ImageUrl = photo.Url;
                member.User.ImageUrl = photo.Url;
            }
            member.Photos.Add(photo);

            if(await memberRepository.SaveAllAsync()) return photo;
            return BadRequest("Problem adding photo");
        }
    }
}