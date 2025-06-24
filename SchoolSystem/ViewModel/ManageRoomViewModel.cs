using SchoolSystem.Model;
using SchoolSystem.ViewModel.BaseClass;
using SchoolSystem.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using SchoolSystem.View;

namespace SchoolSystem.ViewModel
{
    public class ManageRoomViewModel : BaseViewModel
    {
        private readonly RoomRepository _roomRepository = new RoomRepository();

        private ObservableCollection<Room> _rooms;
        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }

        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddRoomCommand { get; }
        public ICommand EditRoomCommand { get; }
        public ICommand DeleteRoomCommand { get; }

        public ManageRoomViewModel()
        {
            Rooms = new ObservableCollection<Room>();

            AddRoomCommand = new RelayCommand(AddRoom);
            EditRoomCommand = new RelayCommand(EditRoom, () => SelectedRoom != null);
            DeleteRoomCommand = new RelayCommand(DeleteRoom, () => SelectedRoom != null);

            LoadRooms();
        }

        private void LoadRooms()
        {
            try
            {
                var roomsFromDb = _roomRepository.GetAllRooms();
                Rooms = new ObservableCollection<Room>(roomsFromDb);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania sal: {ex.Message}");
            }
        }

        private void AddRoom()
        {
            var vm = new AddRoomViewModel();
            var view = new AddRoomView { DataContext = vm };

            var window = new Window
            {
                Title = "Dodaj salę",
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            vm.CloseRequested += (s, e) =>
            {
                if (e) LoadRooms();
                window.DialogResult = e;
                window.Close();
            };

            window.ShowDialog();
        }

        private void EditRoom()
        {
            if (SelectedRoom == null) return;

            var vm = new EditRoomViewModel(SelectedRoom);
            var view = new EditRoomView { DataContext = vm };

            var window = new Window
            {
                Title = "Edytuj salę",
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            vm.CloseRequested += (s, e) =>
            {
                if (e)
                {
                    LoadRooms();
                }
                window.DialogResult = e;
                window.Close();
            };

            window.ShowDialog();
        }

        private void DeleteRoom()
        {
            if (SelectedRoom == null) return;

            if (MessageBox.Show($"Czy na pewno chcesz usunąć salę {SelectedRoom.Number}?",
                "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                bool deleted = _roomRepository.DeleteRoom(SelectedRoom.ID);

                if (deleted)
                {
                    Rooms.Remove(SelectedRoom);
                }
                else
                {
                    MessageBox.Show("Nie można usunąć sali, ponieważ jest powiązana z innymi rekordami.",
                        "Błąd usuwania", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
