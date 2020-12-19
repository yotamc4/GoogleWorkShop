using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YOTY.Service.Data.Entities;

namespace YOTY.Service.Data
{
    public class MockedDb
    {
        public Dictionary<string, BidEntity> mockedBidsSet;

        public MockedDb()
        {
            mockedBidsSet = new Dictionary<string, BidEntity>();
        }
    }
}
