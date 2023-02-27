using System;

namespace GGroupp.Infra;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class DbEntityAttribute : Attribute
{
}