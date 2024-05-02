using OpenDnD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDnD.ViewModel
{
    class SessionsVM
    {
        public AuthToken AuthToken {  get; private set; }
        public IServiceProvider ServiceProvider { get; }
        public ISessionService SessionService { get; }

        public void SetAuthToken(AuthToken authToken)
        {
            AuthToken = authToken;
        }

        public SessionsVM(IServiceProvider serviceProvider, ISessionService sessionService)
        {
            ServiceProvider = serviceProvider;
            SessionService = sessionService;
        }
    }
}
