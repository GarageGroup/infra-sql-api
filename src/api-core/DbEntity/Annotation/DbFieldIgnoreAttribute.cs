using System;

namespace GGroupp.Infra;

[AttributeUsage(AttributeTargets.Property)]
public sealed class DbFieldIgnoreAttribute : Attribute
{
}