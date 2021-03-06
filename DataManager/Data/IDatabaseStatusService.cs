﻿// MIT License

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
using System.Runtime.CompilerServices;

using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Data
{
    public interface IDatabaseStatusService
    {
        DatabaseStatusEnum DatabaseStatus { get; }
        void AddStatusItem(IDatabaseStatus statusItem);

        void RemoveStatusItem(IDatabaseStatus statusItem);

        Task StartRequestWhenReady(Func<Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "");

        Task StartRequestWhenReady<TKey>(TKey key, Func<TKey, Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "");

        Task<TResult> StartRequestWhenReady<TResult>(Func<Task<TResult>> getFunc, UpdateKind updateKind, [CallerMemberName] string callName = "") where TResult : class;

        Task<TResult> StartRequestWhenReady<TResult>(Func<Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue, [CallerMemberName] string callName = "");

        Task<TResult> StartRequestWhenReady<TKey, TResult>(TKey key, Func<TKey, Task<TResult>> getFunc, UpdateKind updateKind, [CallerMemberName] string callName = "") where TResult : class;

        Task<TResult> StartRequestWhenReady<TKey, TResult>(TKey key, Func<TKey, Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue, [CallerMemberName] string callName = "");

        void EndRequest(Guid token, string callName = "");
    }
}
