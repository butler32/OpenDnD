using OpenDnD.Interfaces;
using OpenDnD.Model;
using OpenDnD.Utilities;
using OpenDnD.Utilities.DI;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace OpenDnD.ViewModel
{
    class SessionCreationVM : ViewModelBase
    {
        public SessionCreationVM(IServiceProvider serviceProvider)
        {
            serviceProvider.UseDI(this);

            SaveCommand = ICommand.From(Save);

            Init();
        }

        [FromDI]
        public IServiceProvider ServiceProvider { get; private set; }
        [FromDI]
        public ISessionService SessionService { get; private set; }
        [FromDI]
        public UserAuthToken UserAuthToken { get; private set; }

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

        public ICommand SaveCommand { get; }
        public ICommand<SessionModel> InviteCommand { get; set; }

        private void Save()
        {
            SessionService.Update(UserAuthToken.AuthToken, CurrentSession.SessionId, new SessionRequest
            {
                SessionName = CurrentSession.SessionName,
                PlayersIds = CurrentSession.PlayersIds,
            });
        }

        private void Invite()
        {
            InviteCommand.Execute(CurrentSession);
        }

        private void Init()
        {
            PlayersList = new ObservableCollection<SessionPlayerModel?>
            {
                null,
                new SessionPlayerModel
                {
                    SessionId = Guid.NewGuid(),
                    Image = ConvertByteArrayToBitmapImage (ConvertJpgToByteArray("Images\\art1.jpg")),
                    PlayerId = Guid.NewGuid(),
                    UserName = "Stas",
                    Role = RoleEnum.Player.ToString(),
                },
                new SessionPlayerModel
                {
                    SessionId = Guid.NewGuid(),
                    Image = ConvertByteArrayToBitmapImage (ConvertJpgToByteArray("Images\\art2.jpg")),
                    PlayerId = Guid.NewGuid(),
                    UserName = "Vlad",
                    Role = RoleEnum.Player.ToString(),
                },
                new SessionPlayerModel
                {
                    SessionId = Guid.NewGuid(),
                    Image = ConvertByteArrayToBitmapImage (ConvertJpgToByteArray("Images\\art3.jpg")),
                    PlayerId = Guid.NewGuid(),
                    UserName = "Anton",
                    Role = RoleEnum.Player.ToString(),
                },
                new SessionPlayerModel
                {
                    SessionId = Guid.NewGuid(),
                    Image = ConvertByteArrayToBitmapImage (ConvertJpgToByteArray("Images\\art4.jpg")),
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
