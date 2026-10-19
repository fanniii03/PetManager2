using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetManager
{
    [QueryProperty(nameof(EditedPet), "EditedPet")]
    public partial class MainPageViewModel : ObservableObject
    {
        private IPetDatabase database;

        public ObservableCollection<Pet> Pets { get; set; }

        private Pet selectedPet;
        public Pet SelectedPet
        {
            get { return selectedPet; }
            set
            {
                selectedPet = value;
                OnPropertyChanged();
                DeletePetCommand.NotifyCanExecuteChanged();
                EditSelectedPetCommand.NotifyCanExecuteChanged();
            }
        }

        public bool IsSelected => SelectedPet != null;

        [ObservableProperty]
        Pet editedPet;

        //azért kell ez, mert async nem hívható setterből
        //az [ObservableProperty] generálja az alapját, amúgy ilyen metódus nem lenne
        async partial void OnEditedPetChanged(Pet value)
        {
            if (value != null)
            {
                if (SelectedPet != null)
                {
                    Pets.Remove(SelectedPet);
                    SelectedPet = null;
                    await database.UpdatePetAsync(value);
                }
                else
                {
                    await database.CreatePetAsync(value);
                }
                Pets.Add(value);
            }
        }

        public MainPageViewModel(IPetDatabase database)
        {
            this.database = database;
            Pets = new ObservableCollection<Pet>();
        }

        public async Task InitializeAsync()
        {
            // Megjegyzés: A Loaded nem csak a MainPage legelső betöltődésekor sül el, hanem akkor is, amikor Flyout menüből navigálunk rá, ezért kell a Clear().
            var petList = await database.GetPetsAsync();
            Pets.Clear();
            petList.ForEach(p => Pets.Add(p));
            int x = Pets.Count;
        }

        [RelayCommand]
        public async Task ShowPetDetailsAsync()
        {
            if (SelectedPet != null)
            {
                var param = new ShellNavigationQueryParameters
                {
                    { "Potato", SelectedPet }
                };
                await Shell.Current.GoToAsync("petdetails", param);
            }
        }

        [RelayCommand(CanExecute = nameof(IsSelected))]
        public async Task EditSelectedPetAsync()
        {
            if (SelectedPet != null)
            {
                var param = new ShellNavigationQueryParameters
                {
                    { "Pet", SelectedPet }
                };
                await Shell.Current.GoToAsync("editpet", param);
            }
            else
            {
                WeakReferenceMessenger.Default
                    .Send("Select a pet to edit.");
            }
        }

        [RelayCommand(CanExecute = nameof(IsSelected))]
        public void DeletePet()
        {
            if (SelectedPet != null)
            {
                database.DeletePetAsync(SelectedPet);
                Pets.Remove(SelectedPet);
                SelectedPet = null;
            }
            else
            {
                WeakReferenceMessenger.Default
                    .Send("Select a pet a delete.");
            }
        }

        [RelayCommand]
        public async Task NewPetAsync()
        {
            SelectedPet = null;
            var param = new ShellNavigationQueryParameters
            {
                { "Pet", new Pet(){ BirthDate = DateTime.Today } }
            };
            await Shell.Current.GoToAsync("editpet", param);
        }
    }
}
