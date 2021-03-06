﻿// Decompiled with JetBrains decompiler
// Type: Akkatecture.Commands.ISerializedCommandPublisher
// Assembly: Akkatecture, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 61DF059E-E5F5-4992-B320-644C3E4F5C82
// Assembly location: C:\Users\naych\source\repos\!!!!!\netcoreapp2.2\Akkatecture.dll

using System.Threading;
using System.Threading.Tasks;
using Akkatecture.Commands.ExecutionResults;

namespace Akkatecture.Commands
{
  public interface ISerializedCommandPublisher
  {
    Task<IExecutionResult> PublishSerilizedCommandAsync(
      string name,
      int version,
      string json,
      CancellationToken cancellationToken);
  }
}
