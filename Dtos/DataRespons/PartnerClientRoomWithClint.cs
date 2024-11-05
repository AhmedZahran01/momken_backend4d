using momken_backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.DataRespons
{
    public class PartnerClientRoomWithClint
    {
        public Guid Id { get; set; }
        
        public DateTime LastMessageDateTime { get; set;}

        public PartnerClientRoomWithClintClient Client { get; set; }

        public int MessagesNotShow { get; set; }

        public string LastMessage { get; set; }

    }

    public class PartnerClientRoomWithClintClient
    {
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
    }
}
