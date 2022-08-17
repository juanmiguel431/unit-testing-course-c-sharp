namespace TestNinja.Mocking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var videoService = new VideoService();
            var title = videoService.ReadVideoTitle();
        }
    }
}