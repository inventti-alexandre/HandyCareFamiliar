using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class AcionarContatoEmergenciaPageModel : FreshBasePageModel
    {
        private Cuidador _selectedCuidador;
        public Cuidador Cuidador { get; set; }
        public Familiar Familiar { get; set; }
        public Paciente Paciente { get; set; }
        public PageModelHelper oHorario { get; set; }
        public ObservableCollection<Cuidador> Cuidadores { get; set; }
        public ObservableCollection<Parentesco> Parentescos { get; set; }
        public ObservableCollection<Familiar> Familiares { get; set; }

        public Command LigarPM
        {
            get
            {
                return new Command(() =>
                {
#if DEBUG
                    Device.OpenUri(new Uri("tel:0"));
#else
                    Device.OpenUri(new Uri("tel:190"));
#endif
                });
            }
        }

        public Command LigarSAMU
        {
            get
            {
                return new Command(() =>
                {
#if DEBUG
                    Device.OpenUri(new Uri("tel:0"));
#else
                    Device.OpenUri(new Uri("tel:192"));
#endif
                });
            }
        }

        public Command LigarCBM
        {
            get
            {
                return new Command(() =>
                {
#if DEBUG
                    Device.OpenUri(new Uri("tel:0"));
#else
                    Device.OpenUri(new Uri("tel:193"));
#endif
                });
            }
        }

        public Cuidador SelectedCuidador
        {
            get { return _selectedCuidador; }
            set
            {
                _selectedCuidador = value;
                if (value != null)
                {
                    CuidadorSelected.Execute(value);
                    SelectedCuidador = null;
                }
            }
        }

        public Command<Cuidador> CuidadorSelected
        {
            get
            {
                return new Command<Cuidador>(async cuidador =>
                {
                    var contatoemergencia =
                        new ObservableCollection<ContatoEmergencia>(
                                await FamiliarRestService.DefaultManager.RefreshContatoEmergenciaAsync(true))
                            .FirstOrDefault(e => e.Id == cuidador.CuiContatoEmergencia);
                    var contelefone =
                        new ObservableCollection<ConTelefone>(
                                await FamiliarRestService.DefaultManager.RefreshConTelefoneAsync(true))
                            .FirstOrDefault(e => e.Id == contatoemergencia.ConTelefone);
                    var concelular =
                        new ObservableCollection<ConCelular>(
                                await FamiliarRestService.DefaultManager.RefreshConCelularAsync(true))
                            .FirstOrDefault(e => e.Id == contatoemergencia.ConCelular);
                    if ((contelefone != null) && (concelular != null))
                    {
                        var result =
                            await
                                CoreMethods.DisplayActionSheet("Forma de ligação", "Cancelar", null, "Celular",
                                    "Telefone");
                        if (result == "Celular")
                            Device.OpenUri(new Uri("tel:" + concelular.ConNumCelular));
                        else if (result == "Telefone")
                            Device.OpenUri(new Uri("tel:" + contelefone.ConNumTelefone));
                    }
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            oHorario = new PageModelHelper {ActivityRunning = true, Visualizar = false};
            Cuidador = new Cuidador();
            Familiar=new Familiar();
            Paciente = new Paciente();
            var tupla = initData as Tuple<Familiar, Paciente>;
            if (tupla != null)
            {
                Familiar = tupla.Item1;
                Paciente = tupla.Item2;
            }
            GetCuidadores();
            oHorario.ActivityRunning = false;
            oHorario.Visualizar = true;
        }

        private void GetCuidadores()
        {
            Task.Run(async () =>
            {
                var oi = new ObservableCollection<CuidadorPaciente>(
                        await FamiliarRestService.DefaultManager.RefreshCuidadorPacienteAsync(true))
                    .Where(e => e.PacId == Paciente.Id);
                var selection =
                    new ObservableCollection<Cuidador>(
                            await FamiliarRestService.DefaultManager.RefreshCuidadorAsync(true))
                        .Where(e => oi.Select(a => a.CuiId)
                            .Contains(e.Id)).AsEnumerable();
                //var x =
                //    new ObservableCollection<Parentesco>(
                //            await FamiliarRestService.DefaultManager.Re(true))
                //        .Where(e => selection.Select(a => a.FamParentesco)
                //            .Contains(e.Id)).AsEnumerable();
                //foreach (var z in selection)
                //    foreach (var b in x)
                //        if (z.FamParentesco == b.Id)
                //            z.FamDescriParentesco = b.ParDescricao;

                //Parentescos = new ObservableCollection<Parentesco>(x);
                Cuidadores = new ObservableCollection<Cuidador>(selection);
                oHorario.ActivityRunning = false;
            });
        }
    }
}