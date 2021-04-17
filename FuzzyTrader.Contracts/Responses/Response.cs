using System;
using System.Collections;
using System.Collections.Generic;

namespace FuzzyTrader.Contracts.Responses
{
    public class Response<T>
    {
        public Response(T response)
        {
            Data = response;
        }

        public T Data { get; set; }
    }
}