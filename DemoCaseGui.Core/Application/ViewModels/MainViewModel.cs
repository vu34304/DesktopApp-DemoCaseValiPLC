using CommunityToolkit.Mvvm.Input;
using DemoCaseGui.Core.Application.Communication;
using System.Windows.Input;
namespace DemoCaseGui.Core.Application.ViewModels;
public class MainViewModel : BaseViewModel
{
    public CaseViewModel CaseViewModel { get; set; }
    public CaseMicroViewModel MicroViewModel { get; set; }
    public FilterViewModel FilterViewModel { get; set; }
    public CaseCompactLogixViewModel CaseCompactLogixViewModel { get; set; }
    public MainViewModel(CaseViewModel caseViewModel, FilterViewModel filterViewModel , CaseMicroViewModel microViewModel, CaseCompactLogixViewModel caseCompactLogixViewModel)
    {
        CaseViewModel = caseViewModel;
        FilterViewModel = filterViewModel;
        MicroViewModel = microViewModel;
        CaseCompactLogixViewModel = caseCompactLogixViewModel;

    }
}
