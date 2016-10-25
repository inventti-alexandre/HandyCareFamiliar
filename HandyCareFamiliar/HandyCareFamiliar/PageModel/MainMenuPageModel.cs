using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareFamiliar.Data;
using HandyCareFamiliar.Helper;
using HandyCareFamiliar.Model;
using Xamarin.Forms;

namespace HandyCareFamiliar.PageModel
{
    public class MainMenuPageModel : FreshBasePageModel
    {
        private Paciente _selectedPaciente;
        public Image image;
        private Familiar Familiar { get; set; }
        public ObservableCollection<Paciente> Pacientes { get; set; }
        public PageModelHelper oHorario { get; set; }
        public Paciente oPaciente { get; set; }
        public PacienteFamiliar PacienteFamiliar { get; set; }
        public ObservableCollection<PacienteFamiliar> FamiliaresPacientes { get; set; }

        public Command ShowFoto
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        var tupla = new Tuple<Familiar, Paciente>(Familiar, SelectedPaciente);
                        //await CoreMethods.PushPageModel<FotoPageModel>(tupla);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }
        public Command ShowAvaliacao
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        PacienteFamiliar = FamiliaresPacientes.FirstOrDefault(e => e.PacId == SelectedPaciente.Id);
                        var x = new Tuple<PacienteFamiliar>(PacienteFamiliar);
                        await CoreMethods.PushPageModel<ListaAvaliacaoPageModel>(x);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command ShowVideo
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        var tupla = new Tuple<Familiar, Paciente>(Familiar, SelectedPaciente);
                        //await CoreMethods.PushPageModel<VideoPageModel>(tupla);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command ShowFamiliar
        {
            get
            {
                return new Command(async () =>
                {
                    //await CoreMethods.PushPageModel<FamiliarPageModel>(Familiar);
                });
            }
        }

        public Command ShowAfazeres
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        PacienteFamiliar = FamiliaresPacientes.FirstOrDefault(e => e.PacId == SelectedPaciente.Id);
                        var x = new Tuple<Paciente, PacienteFamiliar>(SelectedPaciente, PacienteFamiliar);
                        //await CoreMethods.PushPageModel<ListaAfazerPageModel>(x);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }
        public Command ShowBusca
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<BuscarPageModel>(Familiar);
                });
            }
        }

        public Command ShowPaciente
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        var x = new Tuple<Paciente, bool, Familiar>(SelectedPaciente, false, Familiar);
                        //await CoreMethods.PushPageModel<PacientePageModel>(x);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command AddPaciente
        {
            get
            {
                return new Command(async () =>
                {
                    var x = new Tuple<Paciente, bool, Familiar>(SelectedPaciente, true, Familiar);
                    //await CoreMethods.PushPageModel<PacientePageModel>(x);
                });
            }
        }

        public Command AlertarContatos
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        var tupla = new Tuple<Familiar, Paciente>(Familiar, SelectedPaciente);
                        //await CoreMethods.PushPageModel<AcionarContatoEmergencia>(tupla);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command ShowMateriais
        {
            get
            {
                return new Command(async () =>
                {
                    if (SelectedPaciente != null)
                    {
                        //await CoreMethods.PushPageModel<ListaMaterialPageModel>(SelectedPaciente);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Paciente SelectedPaciente
        {
            get { return _selectedPaciente; }
            set
            {
                _selectedPaciente = value;
                if (value != null)
                {
                    //ShowMedicamentos.Execute(value);
                    //SelectedPaciente = null;
                }
            }
        }

        public Command ShowMedicamentos
        {
            get
            {
                return new Command(async () =>
                {
//ENVIAR ID DO PACIENTE
                    if (SelectedPaciente != null)
                    {
                        //await CoreMethods.PushPageModel<ListaMedicamentoPageModel>(SelectedPaciente);
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Informação",
                            "Selecione um paciente", "OK");
                    }
                });
            }
        }

        public Command ShowMapa
        {
            get
            {
                return new Command(async () =>
                {
                    //await CoreMethods.PushPageModel<MapPageModel>();
                });
            }
        }

        public void ShowImage(string filepath)
        {
            image.Source = ImageSource.FromFile(filepath);
        }

        public override void Init(object initData)
        {
            try
            {
                base.Init(initData);
                Familiar = new Familiar();
                Familiar = initData as Familiar;
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Ih, rapaz {0}", e.Message);
                throw;
            }
        }

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            oPaciente = new Paciente();
            oHorario = new PageModelHelper
            {
                ActivityRunning = true,
                Visualizar = false,
                BoasVindas = "Olá, " + Familiar.FamNomeCompleto
            };
            await GetPacientes();
            //MedImage = new Image {Source = ImageSource.FromFile("pills.png")};
        }

        private async Task GetPacientes()
        {
            try
            {
                await Task.Run(async () =>
                {
                    //FamiliarRestService.DefaultManager.RefreshPacienteAsync();
                    //PacienteFamiliar = new PacienteFamiliar();
                    //PacienteFamiliar =
                    //    new ObservableCollection<PacienteFamiliar>(
                    //        await FamiliarRestService.DefaultManager.RefreshPacienteFamiliarAsync()).FirstOrDefault(
                    //            e => e.CuiId == Familiar.Id);
                    var familiaresPacientes = new ObservableCollection<PacienteFamiliar>(
                        await FamiliarRestService.DefaultManager.RefreshPacienteFamiliarAsync()).Where(
                        e => e.FamId == Familiar.Id).AsEnumerable();
                    FamiliaresPacientes = new ObservableCollection<PacienteFamiliar>(familiaresPacientes);
                    if (FamiliaresPacientes.Any())
                    {
                        var result =
                            new ObservableCollection<Paciente>(
                                FamiliarRestService.DefaultManager.RefreshPacienteAsync().Result);
                        var resulto =
                            result.Where(e => FamiliaresPacientes.Select(m => m.PacId).Contains(e.Id)).AsEnumerable();
                        Pacientes = new ObservableCollection<Paciente>(resulto);
                    }
                    oHorario.ActivityRunning = false;
                    oHorario.Visualizar = true;
                });
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
    }
}