namespace PetManager;

public partial class EditPetPage : ContentPage
{
    EditPetPageViewModel vm;

    public EditPetPage(EditPetPageViewModel vm)
	{
		InitializeComponent();
        this.vm = vm;
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        vm.InitDraft();
    }
}