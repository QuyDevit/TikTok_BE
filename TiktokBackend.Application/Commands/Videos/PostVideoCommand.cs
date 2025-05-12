using MediatR;
using System.Text.Json;
using TiktokBackend.Application.Common;
using TiktokBackend.Application.Interfaces;
using TiktokBackend.Application.Payloads;
using TiktokBackend.Application.Validators;
using TiktokBackend.Domain.Entities;
using TiktokBackend.Domain.Interfaces;

namespace TiktokBackend.Application.Commands.Videos
{
    public record PostVideoCommand(PostVideoRequest Data): IRequest<ServiceResponse<bool>>;

    public class PostVideoCommandHandler : IRequestHandler<PostVideoCommand, ServiceResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVideoRepository _videoRepository;
        private readonly IUploadFileService _uploadFileService;
        private readonly IVideoMetaService _videoMetaService;
        private readonly IVideoMetaRepository _videoMetaRepository;
        public PostVideoCommandHandler(IUnitOfWork unitOfWork, IUploadFileService uploadFileService,
            IVideoRepository videoRepository, IVideoMetaService videoMetaService, IVideoMetaRepository videoMetaRepository)
        {
            _unitOfWork = unitOfWork;
            _uploadFileService = uploadFileService;
            _videoRepository = videoRepository;
            _videoMetaService = videoMetaService;
            _videoMetaRepository = videoMetaRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(PostVideoCommand request, CancellationToken cancellationToken)
        {

            var rq = request.Data;
            var validationResult = VideoRequestValidator.Validate(rq);
            if (!validationResult.Success)
                return ServiceResponse<bool>.Fail(validationResult.Message);
            var videoId = Guid.NewGuid();
            var videometa = await _videoMetaService.AnalyzeAsync(rq.Video, rq.OriginalFileName);
            var videoUrl = await _uploadFileService.UploadAsync(rq.Video, videoId.ToString(), "videos", "posts");
            var thumbUrl = await _uploadFileService.UploadAsync(rq.Thumb, videoId.ToString(), "images", "posts");

            var newvideo = new Video
            {
                Id = videoId,
                UserId = rq.UserId,
                Type = videometa.FileFormat,
                ThumbUrl = thumbUrl,
                FileUrl = videoUrl,
                Description = rq.Description,
                Music = "Mặc định",
                LikesCount = 0,
                CommentsCount = 0,
                SharesCount = 0,
                ViewsCount = 0,
                Viewable = rq.Viewable,
                Allows = JsonSerializer.Serialize(rq.Allows),
                PublishedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            };
            var videoMetaEntity = new VideoMeta
            {
                VideoId = videoId,
                FileSize = videometa.FileSize,
                FileFormat = videometa.FileFormat,
                PlaytimeString = videometa.PlaytimeString,
                PlaytimeSeconds = videometa.PlaytimeSeconds,
                ResolutionX = videometa.ResolutionX,
                ResolutionY = videometa.ResolutionY
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _videoRepository.AddVideoAsync(newvideo);
                await _videoMetaRepository.AddVideoMetaAsync(videoMetaEntity);
                await _unitOfWork.CommitAsync();
                return ServiceResponse<bool>.Ok(true, "Upload Video thành công!");
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return ServiceResponse<bool>.Fail("Upload Video không thành công!");
            }
        }
    }
}
