namespace PetManager;

public partial class EditPetPage : ContentPage
{
    EditPetPageViewModel vm;

    public EditPetPage()
	{
		InitializeComponent();
        this.vm = new EditPetPageViewModel();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        vm.InitDraft();
    }
}