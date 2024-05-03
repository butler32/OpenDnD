using OpenDnD.Model;
using System.Collections.ObjectModel;

namespace OpenDnD.Utilities
{
    public static class Converters
    {
        public static SessionModel SessionConverter(Interfaces.Session session)
        {
            return new SessionModel()
            {
                SessionName = session.SessionName,
                SessionId = session.SessionId,
            };
        }

        public static ObservableCollection<SessionModel> ConvertToObservableCollectionSession(List<Interfaces.Session> sessions)
        {
            var result = new ObservableCollection<SessionModel>();

            foreach (var session in sessions) 
            {
                result.Add(SessionConverter(session));
            }

            return result;
        }

        public static ObservableCollection<SessionModel> ConvertToObservableCollectionSession(List<SessionModel> sessions)
        {
            var result = new ObservableCollection<SessionModel>();

            foreach (var session in sessions)
            {
                result.Add(session);
            }

            return result;
        }
    }
}
