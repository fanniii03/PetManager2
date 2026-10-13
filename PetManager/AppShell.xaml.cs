namespace PetManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("petdetails", typeof(PetDetailsPage));
        }
    }
}
