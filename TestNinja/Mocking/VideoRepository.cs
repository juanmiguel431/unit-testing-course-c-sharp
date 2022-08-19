using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Mocking
{
    public interface IVideoRepository
    {
        IEnumerable<Video> GetNotProcessed();
    }

    public class VideoRepository: IVideoRepository
    {
        public IEnumerable<Video> GetNotProcessed()
        {
            using (var videoContext = new VideoContext())
            {
                var videos =
                    (from video in videoContext.Videos
                        where !video.IsProcessed
                        select video).ToList();

                return videos;
            }
        }
    }
}