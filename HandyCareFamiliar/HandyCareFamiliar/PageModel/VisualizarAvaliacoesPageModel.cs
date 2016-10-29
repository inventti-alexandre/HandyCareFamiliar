using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    [ImplementPropertyChanged]
    public class VisualizarAvaliacoesPageModel:FreshBasePageModel
    {
        private Avaliacao _selectedAvaliacao;
        public ObservableCollection<Avaliacao> Avaliacoes { get; set; }
        public PageModelHelper PageModelHelper { get; set; }
        public override async void Init(object initData)
        {
            base.Init(initData);
            PageModelHelper = new PageModelHelper
            {
                ActivityRunning = true,
                Visualizar = false
            };
            var x = initData as ObservableCollection<Avaliacao>;
            if (x == null) return;
            Avaliacoes = x;
            PageModelHelper.ActivityRunning = false;
            if(Avaliacoes.Count>0)
            PageModelHelper.Visualizar = true;
        }
        public Avaliacao SelectedAvaliacao
        {
            get { return _selectedAvaliacao; }
            set
            {
                _selectedAvaliacao = value;
                if (value != null)
                {
                    AvaliacaoSelected.Execute(value);
                    SelectedAvaliacao = null;
                }
            }
        }

        public Command<Avaliacao> AvaliacaoSelected
        {
            get
            {
                return new Command<Avaliacao>(async avaliacao =>
                {
                    await CoreMethods.PushPageModel<DetalheAvaliacaoPageModel>(avaliacao);
                    avaliacao = null;
                });
            }
        }


    }
}
