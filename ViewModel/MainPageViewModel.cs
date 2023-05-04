using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using UserFormApp.Model;

namespace UserFormApp.ViewModel;

public class MainPageViewModel : BaseViewModel, INotifyDataErrorInfo {

    private readonly ErrorsViewModel _errorsViewModel;
    private string pattern = @"[^a-яА-ЯёЁa-zA-Z]+";

    private string _firstName;
    private string _secondName;
    private DateTime _birthday;
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;

            _errorsViewModel.ClearErrors(nameof(FirstName));

            if (Regex.IsMatch(FirstName, pattern, RegexOptions.IgnoreCase))
            {
                _errorsViewModel.AddError(nameof(FirstName), "Имя должно быть валидно.");
            }

            OnPropertyChanged(nameof(FirstName));
        }
    }
    public string SecondName
    {
        get => _secondName;
        set
        {
            _secondName = value;

            _errorsViewModel.ClearErrors(nameof(SecondName));
            if (Regex.IsMatch(SecondName, pattern, RegexOptions.IgnoreCase))
            {
                _errorsViewModel.AddError(nameof(SecondName), "Фамилия должна быть валидна.");
            }

            OnPropertyChanged(nameof(SecondName));   
        }
    }
    public DateTime Birthday
    {
        get => _birthday;
        set
        {
            if (_birthday != value)
            {
                _birthday = value;
                OnPropertyChanged(nameof(Birthday));
            }
        }
    }
   
    public ICommand SaveData { get; set; }
    public ICommand RestoreData { get; set; }

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;
    public bool CanViewHint => HasErrors;

    public MainPageViewModel() {
        
        SaveData = new Command(() => {
            Person.FirstName = FirstName;
            Person.SecondName = SecondName;
            Person.Birthday= Birthday;
        });

        RestoreData = new Command(() => {
            FirstName = Person.FirstName;
            SecondName = Person.SecondName;
            Birthday = Person.Birthday;
        });

        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
    }

    public IEnumerable GetErrors(string propertyName)
    {
        return _errorsViewModel.GetErrors(propertyName);
    }

    private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this, e);
        OnPropertyChanged(nameof(CanCreate));
        OnPropertyChanged(nameof(CanViewHint));
    }
}

