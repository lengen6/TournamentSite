using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TieRenTournament.Models
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }
        public int CompetitorRedScore { get; set; } = 0;
        public int CompetitorBlueScore { get; set; } = 0;   
        public int Duration { get; set; } = 0;
        public int Round { get; set; } = 0;
        public int MatchNumber { get; set; } = 0;

        //Foreign Keys
        [ForeignKey("Competitor")]
        public int? CompetitorRedId { get; set; }
        public Competitor? CompeitorRed { get; set; }
        [ForeignKey("Competitor")]
        public int? CompetitorBlueId { get; set; }
        public Competitor? CompeitorBlue { get; set; }
        [ForeignKey("Competitor")]
        public int? WinnerId { get; set; }
        public Competitor? Winner { get; set; }

    }
}
