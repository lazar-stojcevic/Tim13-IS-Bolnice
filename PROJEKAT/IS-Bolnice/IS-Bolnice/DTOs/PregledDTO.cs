﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.DTOs
{
    class PregledDTO
    {
        public Pacijent Pacijent { get; set; }
        public Lekar Lekar { get; set; }
        public Soba Soba { get; set; }
    }
}
