// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueDatabase.DataTransfer;

namespace iRLeagueManager.Data
{
    public interface IModelDataProvider<TModelDTO, TKey> : IDisposable
    {
        Task<TModelDTO[]> GetAsync(TKey[][] requestIds, Type requestType);
        Task<TTarget[]> GetAsync<TTarget>(TKey[][] requestIds) where TTarget : TModelDTO;
        Task<TModelDTO[]> PostAsync(TModelDTO[] items, Type requestType);
        Task<TTarget[]> PostAsync<TTarget>(TTarget[] items) where TTarget : TModelDTO;
        Task<TModelDTO[]> PutAsync(TModelDTO[] items, Type requestType);
        Task<TTarget[]> PutAsync<TTarget>(TTarget[] items) where TTarget : TModelDTO;
        Task<bool> DeleteAsync(TKey[][] requestIds, Type requestType);
        Task<bool> DelAsync<TTarget>(TKey[][] requestIds) where TTarget : TModelDTO;
    }

    public interface IModelDataProvider :  IModelDataProvider<MappableDTO, long> { }
}
