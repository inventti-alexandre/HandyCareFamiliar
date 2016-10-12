using System.IO;
using System.Windows.Input;
using FreshMvvm;
using HandyCareFamiliar.Model;
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
        public ImageSource VideoPaciente { get; set; }

        public bool AutoPlay { get; set; } = false;

        public bool LoopPlay { get; set; } = false;

        public bool ShowController { get; set; } = false;

        public bool Muted { get; set; } = false;

        public double Volume { get; set; } = 1;

        public double SliderVolume
        {
            get { return Volume*100; }
            set
            {
                try
                {
                    Volume = value/100;
                }
                catch
                {
                    Volume = 0;
                }
            }
        }

        public string LabelVideoStatus { get; private set; }

        public ICommand PlayCommand
        {
            get { return new Command(async () => { await VideoView.Start(); }); }
        }

        public ICommand PauseCommand
        {
            get { return new Command(async () => { await VideoView.Pause(); }); }
        }

        public ICommand StopCommand
        {
            get { return new Command(async () => { await VideoView.Stop(); }); }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            VideoView = new VideoView();
            Video = new Video();
            Video = initData as Video;
            VideoPaciente = ImageSource.FromStream(() => new MemoryStream(Video.VidDados));
        }
    }
}