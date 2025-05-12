using TiktokBackend.Application.DTOs;
using TiktokBackend.Application.Interfaces;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace TiktokBackend.Infrastructure.Services
{
    public class VideoMetaService : IVideoMetaService
    {
        public async Task<VideoMetadata> AnalyzeAsync(byte[] videoBytes, string fileName)
        {
            try
            {
                string ffmpegPath = @"D:\Tools\ffmpeg";
                if (!Directory.Exists(ffmpegPath))
                {
                    Directory.CreateDirectory(ffmpegPath);
                }

                //await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, ffmpegPath);

                FFmpeg.SetExecutablesPath(ffmpegPath);

                var tempPath = Path.Combine(Path.GetTempPath(), fileName);
                await File.WriteAllBytesAsync(tempPath, videoBytes);

                var mediaInfo = await FFmpeg.GetMediaInfo(tempPath);
                var videoStream = mediaInfo.VideoStreams.FirstOrDefault();

                var result = new VideoMetadata
                {
                    FileSize = videoBytes.Length,
                    FileFormat = Path.GetExtension(fileName),
                    PlaytimeString = mediaInfo.Duration.ToString(@"hh\:mm\:ss"),
                    PlaytimeSeconds = mediaInfo.Duration.TotalSeconds,
                    ResolutionX = videoStream?.Width ?? 0,
                    ResolutionY = videoStream?.Height ?? 0
                };

                File.Delete(tempPath);
                return result;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return null;
                
            }
        }
    }
}
