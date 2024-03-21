namespace WinnerPOV_API.Models
{

    //

    public class Player
    {

        public string PlayerID { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public string PicSmallUrl { get; set; }
        public string PicLargeUrl { get; set; }
        public string PicWideUrl { get; set; }

        public int Level { get; set; }

        public string RankImageUrl { get; set; }

        public string RankName { get; set;}

    }
}
