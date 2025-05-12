using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiktokBackend.Application.Commands.Auths;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.Payloads;
using TiktokBackend.Domain.Entities;

namespace TiktokBackend.Application.Validators
{
    public class VideoRequestValidator
    {
        public static ServiceResponse<bool> Validate(PostVideoRequest request)
        {
            if (request.Video == null)
            {
                return ServiceResponse<bool>.Fail("Video không được rỗng!");
            }
            if (request.Thumb == null)
            {
                return ServiceResponse<bool>.Fail("Ảnh bìa không được rỗng!");
            }
            if (string.IsNullOrEmpty(request.Description))
            {
                return ServiceResponse<bool>.Fail("Mô tả không được rỗng!");
            }
            return ServiceResponse<bool>.Ok(true);
        }
    }
}
