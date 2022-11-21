using System.ComponentModel.DataAnnotations;

namespace TieRenTournament.Models
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }
        public int CompetitorRedScore { get; set; } = 0;
        public int CompetitorBlueScore { get; set; } = 0;   
        public Competitor Winner { get; set; }
        public int Duration { get; set; } = 0;

        //Foreign Keys

        public int CompetitorRedId { get; set; }
        public Competitor CompeitorRed { get; set; }
        public int CompetitorBlueId { get; set; }
        public Competitor CompeitorBlue { get; set; }

    }
}
