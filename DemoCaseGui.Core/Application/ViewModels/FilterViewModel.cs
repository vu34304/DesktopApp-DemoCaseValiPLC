using CommunityToolkit.Mvvm.Input;
using DemoCaseGui.Core.Application.Communication;
using DemoCaseGui.Core.Application.Models;
using DemoCaseGui.Core.Application.Persistence.Queries;
using DemoCaseGui.Core.Application.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoCaseGui.Core.Application.ViewModels
{
    public class FilterViewModel : BaseViewModel
    {
        private readonly S7Client _s7Client;
        private readonly M850Client _m850Client;
        private readonly CPLogixClient _CPLogixClient;

        private readonly ValiIfmLogRepository valiIfmLogRepository;
        private readonly InverterLogRepository inverterLogRepository;
        private readonly ValiSiemensLogRepository valiSiemensLogRepository;
        private readonly StepMotorLogRepository stepMotorLogRepository;
        private readonly ValiMicroLogRepository valiMicroLogRepository;
        private readonly ValiMicro820LogRepository valiMicro820LogRepository;
        private readonly ValiCompactLogRepository valiCompactLogRepository;

        private readonly IExcelExporter _excelExporter;

        public ObservableCollection<FilterEntry> Entries { get; set; } = new();
        public TimeRangeQuery TimeRange { get; set; } = new();

        public ObservableCollection<string> Modes { get; private set; }
        private string tagname = "";
        private string tagname1 = "";
        private string tagname2 = "";

        private string mode = "";

        public string Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                if (mode == "Station 1")
                {
                    Tagname = Tagnames.First();
                    TagnameByModes = Tagnames;

                }
                if (mode == "Station 2")
                {
                    Tagname = Tagnames1.First();
                    TagnameByModes = Tagnames1;
                }
                if (mode == "Station 3")
                {
                    Tagname = Tagnames2.First();
                    TagnameByModes = Tagnames2;
                }

                
            }
        }


        public string Tagname
        {
            get { return tagname; }
            set
            {
                tagname = value;
                //var tag = _s7Client.Tags.First(i => i.dbname == tagname);
                if (Mode == "Station 1")
                {
                    var tag = _s7Client.Tags.First(i => i.dbname == tagname);
                }
                if (Mode == "Station 2")
                {
                    var tag = _m850Client.Tags2.Concat(_m850Client.Tags).First(i => i.dbname == tagname);
                }
                if (Mode == "Station 3")
                {
                    var tag = _CPLogixClient.Tags.First(i => i.dbname == tagname);
                }
            }
        }



        public ObservableCollection<string> TagnameByModes { get; set; } = new();
        public ObservableCollection<string> Tagnames { get; set; } = new();
        public ObservableCollection<string> Tagnames1 { get; set; } = new();
        public ObservableCollection<string> Tagnames2 { get; set; } = new();


        public ICommand FilterCommand { get; set; }
        public ICommand FilterCommand1 { get; set; }
        public ICommand FilterCommand2 { get; set; }
        public ICommand ExportToExcelCommand { get; set; }
        public FilterViewModel()
        {
            _s7Client = new S7Client();
            _m850Client = new M850Client();
            _CPLogixClient = new CPLogixClient();
            _excelExporter = new ExcelExporter();
            valiIfmLogRepository = new ValiIfmLogRepository();
            inverterLogRepository = new InverterLogRepository();
            valiSiemensLogRepository = new ValiSiemensLogRepository();
            valiMicroLogRepository = new ValiMicroLogRepository();
            valiCompactLogRepository = new ValiCompactLogRepository();


            Modes = new ObservableCollection<string>()
            {
                  "Station 1",
                  "Station 2",
                  "Station 3"
            };
            Tagnames = new ObservableCollection<string>(_s7Client.Tags.Select(i => i.dbname).OrderBy(s => s));
            Tagnames1 = new ObservableCollection<string>(_m850Client.Tags2.Concat(_m850Client.Tags).Select(i => i.dbname).OrderBy(s => s));
            Tagnames2 = new ObservableCollection<string>(_CPLogixClient.Tags.Select(i => i.dbname).OrderBy(s => s));

            FilterCommand = new RelayCommand(LoadAsync);

            ExportToExcelCommand = new RelayCommand<string>(ExportToExcel);
        }
        private async void LoadAsync()
        {
            try
            {
                var valiIfmLog = await valiIfmLogRepository.GetListAsync(TimeRange, Tagname);
                var inverterLog = await inverterLogRepository.GetListAsync(TimeRange, Tagname);
                var valiSiemensLog = await valiSiemensLogRepository.GetListAsync(TimeRange, Tagname);
                var stepMotorLog = await stepMotorLogRepository.GetListAsync(TimeRange, Tagname);
                var valiMicroLog = await valiMicroLogRepository.GetListAsync(TimeRange, Tagname);
                var valiMicro820Log = await valiMicro820LogRepository.GetListAsync(TimeRange, Tagname);
                var valiCompactLogs = await valiCompactLogRepository.GetListAsync(TimeRange, Tagname);

                var entriesvaliIfmLog = valiIfmLog.Select(e => new FilterEntry(
                    e.Name,
                    e.Timestamp,
                    e.Value)).ToList();

                var entriesinverterLog = inverterLog.Select(e => new FilterEntry(
                    e.Name,
                    e.Timestamp,
                    e.Value)).ToList();

                var entriesvaliSiemensLog = valiSiemensLog.Select(e => new FilterEntry(
                    e.Name,
                    e.Timestamp,
                    e.Value)).ToList();

                var entriesstepMotorLog = stepMotorLog.Select(e => new FilterEntry(
                  e.Name,
                  e.Timestamp,
                  e.Value)).ToList();

                var entriesvaliMicroLog = valiMicroLog.Select(e => new FilterEntry(
                   e.Name,
                   e.Timestamp,
                   e.Value)).ToList();

                var entriesvaliMicro820Log = valiMicro820Log.Select(e => new FilterEntry(
                   e.Name,
                   e.Timestamp,
                   e.Value)).ToList();

                var entriesvaliCompactLog = valiCompactLogs.Select(e => new FilterEntry(
                  e.Name,
                  e.Timestamp,
                  e.Value)).ToList();


                List<FilterEntry> filters = new();
                foreach (var entry in entriesvaliIfmLog)
                {
                    filters.Add(entry);
                }
                foreach (var entry in entriesinverterLog)
                {
                    filters.Add(entry);
                }
                foreach (var entry in entriesvaliSiemensLog)
                {
                    filters.Add(entry);
                }
                foreach (var entry in entriesstepMotorLog)
                {
                    filters.Add(entry);
                }
                foreach (var entry in entriesvaliMicroLog)
                {
                    filters.Add(entry);
                }
                foreach (var entry in entriesvaliMicro820Log)
                {
                    filters.Add(entry);
                }
                foreach (var entry in entriesvaliCompactLog)
                {
                    filters.Add(entry);
                }



                Entries = new(filters);
            }
            catch (HttpRequestException)
            {
                ShowErrorMessage("Đã có lỗi xảy ra: Mất kết nối với server.");
            }
        }


        private void ExportToExcel(string? filePath)
        {
            if (filePath is not null)
            {
                _excelExporter.ExportReport(filePath, Entries);
            }
        }
    }
}
