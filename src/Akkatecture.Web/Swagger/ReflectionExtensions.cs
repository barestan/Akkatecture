﻿// Decompiled with JetBrains decompiler
// Type: Akkatecture.Web.Swagger.ReflectionExtensions
// Assembly: Akkatecture.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 043A546D-C869-4CCE-8BB0-8D9655232ADF
// Assembly location: C:\Users\naych\source\repos\!!!!!\netcoreapp2.2\Akkatecture.Web.dll

using System;
using System.Linq;

namespace Akkatecture.Web.Swagger
{
  public static class ReflectionExtensions
  {
    public static Type GetSubclassOfRawGeneric(Type generic, Type toCheck)
    {
      for (; toCheck != (Type) null && toCheck != typeof (object); toCheck = toCheck.BaseType)
      {
        var type = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
        if (generic == type)
          return toCheck;
      }
      return null;
    }

    public static Type GetSubclassOfRawGenericInterface(Type generic, Type toCheck)
    {
      return toCheck.GetInterfaces().FirstOrDefault(i =>
      {
          if (i.IsGenericType)
              return i.GetGenericTypeDefinition() == generic;
          return false;
      });
    }
  }
}
