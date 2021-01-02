// Copyright (c) YOTY Corporation and contributors. All rights reserved.

namespace UniBuy.Service.Data
{
    using System.Collections.Generic;
    using UniBuy.Service.Data.Entities;

    public class MockedDb
    {
        public Dictionary<string, BidEntity> mockedBidsSet;

        public MockedDb()
        {
            mockedBidsSet = new Dictionary<string, BidEntity>();
        }
    }
}
