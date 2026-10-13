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

        public Pet EditedPet
        {
            set
            {
                if (value != null)
                {
                    if (SelectedPet != null)
                    {
                        Pets.Remove(SelectedPet);
                        SelectedPet = null;
                    }
                    Pets.Add(value);
                }
            }
        }

        public MainPageViewModel()
        {
            Pets = new ObservableCollection<Pet>();
            Pets.Add(new Pet() { Name = "Samu", Species = "Cat", BirthDate = new DateTime(2021, 9, 28) });
            Pets.Add(new Pet() { Name = "Zoé", Species = "Francia bulldog palota pincsi", BirthDate = new DateTime(2014, 12, 14) });
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
