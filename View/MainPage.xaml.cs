using UserFormApp.ViewModel;

namespace UserFormApp;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }

    async void Button_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Уведомление", "Поля Имя и Фамилия не должны содержать цифры или другие спец. знаки!", "ОК");
    }
}

