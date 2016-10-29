using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class ListaVideoPageModel:FreshBasePageModel
    {
        private Video _selectedVideo;
        public Familiar Familiar { get; set; }
        public PageModelHelper oHorario { get; set; }
        public ObservableCollection<VideoFamiliar> VideoFamiliar { get; set; }
        public ObservableCollection<Video> Videos { get; set; }

        public Video SelectedVideo
        {
            get { return _selectedVideo; }
            set
            {
                _selectedVideo = value;
                if (value == null) return;
                VideoSelected.Execute(value);
                SelectedVideo = null;
            }
        }

        public Command<Video> VideoSelected
        {
            get
            {
                return new Command<Video>(async video =>
                {
                    await CoreMethods.PushPageModel<VideoPageModel>(video);
                    video = null;
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            oHorario = new PageModelHelper { ActivityRunning = true };
            Familiar = new Familiar();
            Familiar = initData as Familiar;
            GetVideos();
        }

        private async void GetVideos()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var x =
                        new ObservableCollection<VideoFamiliar>(
                                await FamiliarRestService.DefaultManager.RefreshVideoFamiliarAsync()).
                            Where(e => e.FamId == Familiar.Id)
                            .AsEnumerable();
                    var VideoFamiliars = x as VideoFamiliar[] ?? x.ToArray();
                    VideoFamiliar = new ObservableCollection<VideoFamiliar>(VideoFamiliars);
                    var y = new ObservableCollection<Video>(await FamiliarRestService.DefaultManager.RefreshVideoAsync()).
                        Where(e => VideoFamiliars.Select(b => b.VidId).Contains(e.Id))
                        .AsEnumerable();
                    Videos = new ObservableCollection<Video>(y.OrderBy(e => e.CreatedAt));
                    oHorario.ActivityRunning = false;
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
