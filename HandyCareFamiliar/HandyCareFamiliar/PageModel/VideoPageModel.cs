using System.IO;
using System.Windows.Input;
using FreshMvvm;
using HandyCareFamiliar.Model;
using Octane.Xam.VideoPlayer;
using PropertyChanged;
using Rox;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class VideoPageModel : FreshBasePageModel
    {
        private VideoView VideoView;
        public Video Video { get; set; }
        public VideoSource VideoPaciente { get; set; }

        public bool AutoPlay { get; set; } = false;

        public bool LoopPlay { get; set; } = false;

        public bool ShowController { get; set; } = false;

        public bool Muted { get; set; } = false;

        public double Volume { get; set; } = 1;

        public override void Init(object initData)
        {
            base.Init(initData);
            VideoView = new VideoView();
            Video = new Video();
            Video = initData as Video;
            VideoPaciente = VideoSource.FromStream(() => new MemoryStream(Video.VidDados),"mp4");
        }
    }
}