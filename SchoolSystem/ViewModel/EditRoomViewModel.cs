using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Windows;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    public class EditRoomViewModel : BaseViewModel
    {
        private Room _originalRoom;
        private Room _editingRoom;
        private RoomRepository _roomRepository;

        public string Nr_Sali
        {
            get => _editingRoom.Number;
            set { _editingRoom.Number = value; OnPropertyChanged(nameof(Nr_Sali)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditRoomViewModel(Room room)
        {
            _originalRoom = room;
            // Tworzymy kopię roboczą
            _editingRoom = new Room(room.ID, room.Number);

            _roomRepository = new RoomRepository();

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

            // Aktualizujemy oryginalny obiekt
            _originalRoom.Number = _editingRoom.Number;

            // Zapisujemy do bazy
            _roomRepository.UpdateRoom(_originalRoom);

            CloseRequested?.Invoke(this, true);
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        public event EventHandler<bool> CloseRequested;
    }
}
