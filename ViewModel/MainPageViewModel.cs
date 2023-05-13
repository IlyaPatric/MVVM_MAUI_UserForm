using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UserFormApp.Model;

namespace UserFormApp.ViewModel;

public class MainPageViewModel : BaseViewModel, INotifyDataErrorInfo {

    readonly ErrorsViewModel _errorsViewModel;
    string pattern = @"[^a-яА-ЯёЁa-zA-Z]+";

    string _firstName;
    string _secondName;
    DateTime _birthday;
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

            OnPropertyChanged();
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

            OnPropertyChanged();   
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
                OnPropertyChanged();
            }
        }
    }
   
    public Command SaveData { get; }
    public Command RestoreData { get; }

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanViewHint => HasErrors;

    public MainPageViewModel() {

        SaveData = new Command(SaveData_Command, SaveData_CanExecute);
        RestoreData = new Command(RestoreData_Command, RestoreData_CanExecute);

        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
    }

    void SaveData_Command()
    {
        Person.FirstName = FirstName;
        Person.SecondName = SecondName;
        Person.Birthday = Birthday;
    }
    bool SaveData_CanExecute()
    {
        return !HasErrors;
    } 

    void RestoreData_Command()
    {
        FirstName = Person.FirstName;
        SecondName = Person.SecondName;
        Birthday = Person.Birthday;
    }

    bool RestoreData_CanExecute()
    {
        return true;
    }

    public IEnumerable GetErrors(string propertyName)
    {
        return _errorsViewModel.GetErrors(propertyName);
    }

    private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this, e);
        OnPropertyChanged(nameof(CanViewHint));

        SaveData.ChangeCanExecute();
        RestoreData.ChangeCanExecute();
    }
}

