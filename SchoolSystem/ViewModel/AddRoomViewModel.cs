using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Windows;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    public class AddRoomViewModel : BaseViewModel
    {
        private Room _room = new Room();
        private RoomRepository _roomRepository = new RoomRepository();

        public string Nr_Sali
        {
            get => _room.Number;
            set { _room.Number = value; OnPropertyChanged(nameof(Nr_Sali)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddRoomViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Nr_Sali))
            {
                MessageBox.Show("Numer sali nie może być pusty.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_roomRepository.RoomNumberExists(Nr_Sali))
            {
                MessageBox.Show("Sala o takim numerze już istnieje.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _roomRepository.AddRoom(_room);
            CloseRequested?.Invoke(this, true);
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        public event EventHandler<bool> CloseRequested;
    }
}
