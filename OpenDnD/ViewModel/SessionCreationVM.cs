using OpenDnD.Interfaces;
using OpenDnD.Model;
using OpenDnD.Utilities;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OpenDnD.ViewModel
{
    class SessionCreationVM : ViewModelBase
    {
        public SessionCreationVM(IServiceProvider serviceProvider, ISessionService sessionService, UserAuthToken userAuthToken)
        {
            ServiceProvider = serviceProvider;
            SessionService = sessionService;
            UserAuthToken = userAuthToken;

            SaveCommand = new RelayCommand(Save);
            InviteCommand = new RelayCommand(Invite);

            Init();
        }

        public IServiceProvider ServiceProvider { get; }
        public ISessionService SessionService { get; }
        public UserAuthToken UserAuthToken { get; }

        private SessionModel _currentSession;
        public SessionModel CurrentSession 
        { 
            get {  return _currentSession; }
            set {  _currentSession = value; OnPropertyChanged(); }
        }

        private SessionPlayerModel _sessionPlayer;
        public SessionPlayerModel SessionPlayer
        {
            get { return _sessionPlayer; }
            set { _sessionPlayer = value; OnPropertyChanged(); }
        }

        private ObservableCollection<SessionPlayerModel?> _playersList;
        public ObservableCollection<SessionPlayerModel?> PlayersList
        {
            get { return _playersList; }
            set { _playersList = value; OnPropertyChanged(); }
        }

        public Action<Guid> InviteEvent;

        public ICommand SaveCommand { get; }
        public ICommand InviteCommand { get; }

        private void Save(object obj)
        {
            SessionService.Update(UserAuthToken.AuthToken, CurrentSession.SessionId, new SessionRequest
            {
                SessionName = CurrentSession.SessionName,
                PlayersIds = CurrentSession.PlayersIds,
            });
        }

        private void Invite(object obj)
        {
            InviteEvent?.Invoke(CurrentSession.SessionId);
        }






        private void Init()
        {
            PlayersList = new ObservableCollection<SessionPlayerModel?>
            {
                null,
                new SessionPlayerModel
                {
                    SessionId = Guid.NewGuid(),
                    Image = ConvertByteArrayToBitmapImage (ConvertJpgToByteArray("D:\\Code\\repos\\OpenDnD\\OpenDnD\\Images\\art1.jpg")),
                    PlayerId = Guid.NewGuid(),
                    UserName = "Stas",
                    Role = RoleEnum.Player.ToString(),
                },
                new SessionPlayerModel
                {
                    SessionId = Guid.NewGuid(),
                    Image = ConvertByteArrayToBitmapImage (ConvertJpgToByteArray("D:\\Code\\repos\\OpenDnD\\OpenDnD\\Images\\art2.jpg")),
                    PlayerId = Guid.NewGuid(),
                    UserName = "Vlad",
                    Role = RoleEnum.Player.ToString(),
                },
                new SessionPlayerModel
                {
                    SessionId = Guid.NewGuid(),
                    Image = ConvertByteArrayToBitmapImage (ConvertJpgToByteArray("D:\\Code\\repos\\OpenDnD\\OpenDnD\\Images\\art3.jpg")),
                    PlayerId = Guid.NewGuid(),
                    UserName = "Anton",
                    Role = RoleEnum.Player.ToString(),
                },
                new SessionPlayerModel
                {
                    SessionId = Guid.NewGuid(),
                    Image = ConvertByteArrayToBitmapImage (ConvertJpgToByteArray("D:\\Code\\repos\\OpenDnD\\OpenDnD\\Images\\art4.jpg")),
                    PlayerId = Guid.NewGuid(),
                    UserName = "Iliya",
                    Role = RoleEnum.Master.ToString(),
                },
            };
        }

        private static byte[] ConvertJpgToByteArray(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int numBytes = (int)fileStream.Length;
                byte[] bytes = new byte[numBytes];
                int bytesRead = fileStream.Read(bytes, 0, numBytes);
                return bytes;
            }
        }

        private static BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
    }
}
