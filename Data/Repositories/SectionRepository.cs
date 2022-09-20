﻿using Data.Entities;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SegmentRepository : GeneralRepository<SegmentEntity>
    {
        private readonly Realm _realmInstance;
        public SegmentRepository(Realm realmInstance) : base(realmInstance)
        {
            _realmInstance = realmInstance;
        }
    }
}
