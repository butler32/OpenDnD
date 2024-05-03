﻿namespace OpenDnD.Model
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<SessionModel> Sessions { get; set; }
        public List<CharacterModel> Characters { get; set; }
    }
}