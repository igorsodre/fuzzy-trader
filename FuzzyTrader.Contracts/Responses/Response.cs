using System;
using System.Collections;
using System.Collections.Generic;

namespace FuzzyTrader.Contracts.Responses
{
    public class Response<T>
    {
        public T Data { get; set; }
        public IEnumerable<Object> Errors { get; set; }
    }
}