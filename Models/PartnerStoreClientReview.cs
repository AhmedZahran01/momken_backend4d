using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace momken_backend.Models
{
    public class PartnerStoreClientReview : ModelBaseId
    {

        #region  Properties Region

        public Guid clientId { get; set; }
        public Guid partnerStoreId { get; set; }

        public string ReviewMessage { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

        #region Evaluation Number Region

        private int evaluationNumber;
        public int EvaluationNumber
        {
            get => evaluationNumber;
            set
            {
                if (evaluationNumber > 1 || evaluationNumber < 5)

                    evaluationNumber = value;
                else
                    evaluationNumber = 1;
            }

        }


        #endregion
       
        #endregion


        #region Navigational Properties Region

        public PartnerStore? partnerStore { get; set; }
        public Client? client { get; set; } 
        
        #endregion

    }
}
