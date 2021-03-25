﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongsApi.Models.Songs
{
    public class GetSongsResponse
    {
        public List<SongSummaryItem> data { get; set; }
    }

    public class SongSummaryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string RecommendedBy { get; set; }
    }

}
