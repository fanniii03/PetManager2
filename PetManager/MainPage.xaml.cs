using CommunityToolkit.Mvvm.Messaging;

namespace PetManager
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel vm;

        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();

            this.vm = vm;
            BindingContext = vm;

            WeakReferenceMessenger.Default
                .Register<string>(this, async (recipient, message) =>
                {
                    await DisplayAlert("Warning", message, "OK");
                });
        }

        private async void MainPage_OnLoaded(object? sender, EventArgs e)
        {
            await vm.InitializeAsync();
        }
    }
}
