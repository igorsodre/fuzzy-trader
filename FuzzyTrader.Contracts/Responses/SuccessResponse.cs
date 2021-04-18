using System;
using System.Collections;
using System.Collections.Generic;

namespace FuzzyTrader.Contracts.Responses
{
    public class SuccessResponse<T>
    {
        public SuccessResponse(T response)
        {
            Data = response;
        }

        public T Data { get; set; }
    }
}